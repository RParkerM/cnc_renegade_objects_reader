using System.Globalization;
using System.Numerics;
using System.Reflection;
using ObjectsReaderUI.ViewModels;
using RenData.Types;

namespace ObjectsReaderUI.Editing;

/// <summary>
/// Type-driven engine that turns a definition's members into editable <see cref="PropertyRow"/>s.
/// It knows nothing about any specific definition — all definition-specific knowledge comes from
/// an <see cref="IDefinitionSchema"/>. Booleans and enums become dropdowns; numbers and strings
/// become text; Vector2/Vector3/RectClass flatten into per-component rows.
/// </summary>
public static class DefinitionEditor
{
    private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

    // Identity/plumbing shared by all definitions — never editable, regardless of schema.
    // (m_Name/m_ID are the definition's identity and dictionary key; the rest is internal
    // save-state. They surface as read-only rows but must not be hand-edited.)
    private static readonly HashSet<string> AlwaysReadOnly =
        ["m_DefinitionMgrLink", "Old_Object_Pointer", "m_Name", "m_ID", "m_GenericUserData", "m_SaveEnabled"];

    /// <summary>Returns the edit schema for a definition, or null if it isn't editable.</summary>
    public static IDefinitionSchema? GetSchema(object definition) => DefinitionSchemaRegistry.For(definition);

    /// <summary>
    /// Appends editable row(s) for one member. Returns false if the member isn't editable
    /// (unknown type or excluded), so the caller can fall back to a read-only row.
    /// </summary>
    public static bool TryAddRows(List<PropertyRow> rows, string name, string memberName,
        Type memberType, Func<object?> get, Action<object?> set, IDefinitionSchema schema)
    {
        if (AlwaysReadOnly.Contains(memberName))
            return false;

        var hint = schema.GetHint(memberName);

        // Vectors / rects: flatten into per-component float rows (bounds apply to each component).
        if (IsExpandableComposite(memberType))
        {
            ExpandComposite(rows, name, get, set, hint);
            return true;
        }

        // int-backed enums declared by the schema: named values, stored as the index.
        if (memberType == typeof(int) && hint?.Choices is { } choices)
        {
            int current = (int)(get() ?? 0);
            string display = current >= 0 && current < choices.Count ? choices[current] : current.ToString(Inv);
            rows.Add(new PropertyRow(name, display, PropertyEditorKind.Choice, v =>
            {
                int idx = IndexOf(choices, v);
                if (idx >= 0) set(idx);
                return v;
            }, choices));
            return true;
        }

        // Real enum types: dropdown of the enum names.
        if (memberType.IsEnum)
        {
            var names = Enum.GetNames(memberType);
            rows.Add(new PropertyRow(name, get()?.ToString() ?? "", PropertyEditorKind.Choice, v =>
            {
                set(Enum.Parse(memberType, v));
                return v;
            }, names));
            return true;
        }

        // Booleans: True/False dropdown.
        if (memberType == typeof(bool))
        {
            rows.Add(new PropertyRow(name, get()?.ToString() ?? "False", PropertyEditorKind.Choice, v =>
            {
                set(bool.Parse(v));
                return v;
            }, ["False", "True"]));
            return true;
        }

        // Numbers and strings: free-form text with culture-invariant round-tripping.
        if (memberType == typeof(string) || (memberType.IsPrimitive && memberType != typeof(char) && memberType != typeof(IntPtr)))
        {
            string display = memberType == typeof(string) ? get() as string ?? "" : Convert.ToString(get(), Inv) ?? "";
            rows.Add(new PropertyRow(name, display, PropertyEditorKind.Text,
                v => ApplyScalar(memberType, v, get, set, hint)));
            return true;
        }

        return false;
    }

    // Parses text into the target type, clamps it, writes it, and returns the canonical
    // stored value so the editor can reflect any clamping back to the user.
    private static string? ApplyScalar(Type type, string v, Func<object?> get, Action<object?> set, FieldEditHint? hint)
    {
        if (type == typeof(string)) { set(v); return v; }
        object num = Clamp(Convert.ChangeType(v, type, Inv), hint);
        set(num);
        return Convert.ToString(get(), Inv);
    }

    private static bool IsExpandableComposite(Type t) =>
        t == typeof(Vector2) || t == typeof(Vector3) || t == typeof(RectClass);

    // Emits one editable row per public instance float component. Each leaf setter reads the
    // current composite, mutates one component, and writes the whole thing back — required
    // because Vector2/Vector3 are structs (GetValue returns a boxed copy).
    private static void ExpandComposite(List<PropertyRow> rows, string prefix,
        Func<object?> get, Action<object?> set, FieldEditHint? hint)
    {
        var container = get();
        if (container is null) return;

        foreach (var leaf in container.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!leaf.FieldType.IsPrimitive) continue;   // only scalar components (float, etc.)

            var field = leaf;   // capture per-iteration for the closures
            string display = Convert.ToString(field.GetValue(container), Inv) ?? "";
            rows.Add(new PropertyRow($"{prefix}.{field.Name}", display, PropertyEditorKind.Text, v =>
            {
                object? box = get();
                if (box is null) return null;
                field.SetValue(box, Clamp(Convert.ChangeType(v, field.FieldType, Inv), hint));
                set(box);
                var stored = get();
                return stored is null ? null : Convert.ToString(field.GetValue(stored), Inv);
            }));
        }
    }

    // Clamps a numeric value to the hint's inclusive bounds, preserving its CLR type.
    private static object Clamp(object value, FieldEditHint? hint)
    {
        if (hint is null || (hint.Min is null && hint.Max is null) || value is not IConvertible)
            return value;

        double d = Convert.ToDouble(value, Inv);
        if (hint.Min is { } min && d < min) d = min;
        if (hint.Max is { } max && d > max) d = max;
        return Convert.ChangeType(d, value.GetType(), Inv);
    }

    private static int IndexOf(IReadOnlyList<string> list, string value)
    {
        for (int i = 0; i < list.Count; i++)
            if (list[i] == value) return i;
        return -1;
    }
}
