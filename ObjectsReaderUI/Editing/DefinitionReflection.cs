using System.Reflection;
using System.Runtime.CompilerServices;

namespace ObjectsReaderUI.Editing;

/// <summary>
/// Shared reflection helpers for walking a definition's members. Kept in one place so the
/// property grid and the JSON exporter surface exactly the same set of fields.
/// </summary>
public static class DefinitionReflection
{
    /// <summary>
    /// Enumerates every instance field on the type and its bases, public and non-public alike,
    /// ordered by name. Reflection.GetFields never returns inherited non-public members, so we
    /// walk the base-type chain ourselves — most definitions (VehicleGameObjDef and the whole
    /// GameObjDef hierarchy) store their data in protected fields. Most-derived declarations win
    /// so `new`-shadowed fields aren't duplicated, and compiler-generated auto-property backing
    /// fields are skipped because the properties themselves are surfaced separately.
    /// </summary>
    public static IEnumerable<FieldInfo> GetAllInstanceFields(Type type)
    {
        var seen = new HashSet<string>();
        var fields = new List<FieldInfo>();
        for (var t = type; t is not null && t != typeof(object); t = t.BaseType)
        {
            foreach (var field in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (field.IsDefined(typeof(CompilerGeneratedAttribute), false)) continue;
                if (seen.Add(field.Name)) fields.Add(field);
            }
        }
        return fields.OrderBy(f => f.Name);
    }
}
