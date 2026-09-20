using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ObjectsReaderUI.ViewModels;

public enum PropertyEditorKind
{
    ReadOnly,   // display only — shown as a plain text block
    Text,       // free-form entry — numbers and strings
    Choice,     // constrained set of values — shown as a dropdown
    Link,       // click-through reference — shown as a hyperlink-style button
}

/// <summary>
/// A single row in the properties panel. Read-only rows just carry a label and a
/// value string. Editable rows also carry an "apply" callback that writes the edited
/// value onto the live definition object and returns the canonical stored value, so
/// normalization (e.g. clamping a color channel to 0–1) is reflected back to the user.
/// </summary>
public partial class PropertyRow : ObservableObject
{
    public string Name { get; }
    public PropertyEditorKind Kind { get; }
    public IReadOnlyList<string>? Options { get; }

    private readonly Func<string, string?>? _apply;
    private readonly Action? _navigate;
    private bool _syncing;

    [ObservableProperty] private string _value;

    public bool IsReadOnly => Kind == PropertyEditorKind.ReadOnly;
    public bool IsText => Kind == PropertyEditorKind.Text;
    public bool IsChoice => Kind == PropertyEditorKind.Choice;
    public bool IsLink => Kind == PropertyEditorKind.Link;

    /// <summary>Read-only row (unchanged behavior for non-editable data).</summary>
    public PropertyRow(string name, string value)
    {
        Name = name;
        _value = value ?? "";
        Kind = PropertyEditorKind.ReadOnly;
    }

    /// <summary>
    /// Link row. Displays <paramref name="value"/> as a hyperlink-style button that
    /// runs <paramref name="navigate"/> when clicked (e.g. jump to a referenced definition).
    /// </summary>
    public PropertyRow(string name, string value, Action navigate)
    {
        Name = name;
        _value = value ?? "";
        Kind = PropertyEditorKind.Link;
        _navigate = navigate;
    }

    [RelayCommand]
    private void Navigate() => _navigate?.Invoke();

    /// <summary>
    /// Editable row. <paramref name="apply"/> stores the new value on the target object and
    /// returns the canonical value to display (or null to leave the entered text as-is).
    /// </summary>
    public PropertyRow(string name, string value, PropertyEditorKind kind,
                       Func<string, string?> apply, IReadOnlyList<string>? options = null)
    {
        Name = name;
        _value = value ?? "";
        Kind = kind;
        _apply = apply;
        Options = options;
    }

    partial void OnValueChanged(string value)
    {
        // Only user edits reach here — the initial value is assigned to the backing field in
        // the constructor, bypassing this callback. _syncing guards the re-entrant write below.
        if (_apply is null || _syncing)
            return;

        string? canonical;
        try { canonical = _apply(value); }
        catch { return; }   // invalid input: leave the typed text, live object untouched

        if (canonical is not null && canonical != value)
        {
            _syncing = true;
            Value = canonical;
            _syncing = false;
        }
    }
}
