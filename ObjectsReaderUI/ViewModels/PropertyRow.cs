using CommunityToolkit.Mvvm.ComponentModel;

namespace ObjectsReaderUI.ViewModels;

public enum PropertyEditorKind
{
    ReadOnly,   // display only — shown as a plain text block
    Text,       // free-form entry — numbers and strings
    Choice,     // constrained set of values — shown as a dropdown
}

/// <summary>
/// A single row in the properties panel. Read-only rows just carry a label and a
/// value string. Editable rows also carry a setter that writes the edited value
/// straight back onto the live definition object, so changes are picked up by Save.
/// </summary>
public partial class PropertyRow : ObservableObject
{
    public string Name { get; }
    public PropertyEditorKind Kind { get; }
    public IReadOnlyList<string>? Options { get; }

    private readonly Action<string>? _set;

    [ObservableProperty] private string _value;

    public bool IsReadOnly => Kind == PropertyEditorKind.ReadOnly;
    public bool IsText => Kind == PropertyEditorKind.Text;
    public bool IsChoice => Kind == PropertyEditorKind.Choice;

    /// <summary>Read-only row (unchanged behavior for non-editable data).</summary>
    public PropertyRow(string name, string value)
    {
        Name = name;
        _value = value ?? "";
        Kind = PropertyEditorKind.ReadOnly;
    }

    /// <summary>Editable row. <paramref name="set"/> applies the new value to the target object.</summary>
    public PropertyRow(string name, string value, PropertyEditorKind kind,
                       Action<string> set, IReadOnlyList<string>? options = null)
    {
        Name = name;
        _value = value ?? "";
        Kind = kind;
        _set = set;
        Options = options;
    }

    partial void OnValueChanged(string value)
    {
        // Only user edits reach here — the initial value is assigned to the backing
        // field in the constructor, bypassing this callback. A parse failure leaves
        // the live object untouched rather than crashing the edit.
        try { _set?.Invoke(value); }
        catch { /* invalid input: keep the previous stored value */ }
    }
}
