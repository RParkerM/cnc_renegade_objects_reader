namespace ObjectsReaderUI.Editing;

/// <summary>
/// Per-field editing metadata that the generic editor can't infer from the CLR type
/// alone — the constrained set of values for an int-backed enum, and numeric bounds.
/// For a composite member (Vector2/Vector3/RectClass) the bounds apply to every
/// component (e.g. an RGB color constrained to 0–1 per channel).
/// </summary>
public sealed record FieldEditHint
{
    /// <summary>Named values for an int-backed enum. The list index is the stored integer.</summary>
    public IReadOnlyList<string>? Choices { get; init; }

    /// <summary>Inclusive lower bound applied to numeric input (and each component of a composite).</summary>
    public float? Min { get; init; }

    /// <summary>Inclusive upper bound applied to numeric input (and each component of a composite).</summary>
    public float? Max { get; init; }
}
