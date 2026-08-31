namespace ObjectsReaderUI.Editing;

/// <summary>
/// Describes how one definition type should be edited. Implementations hold only the
/// definition-specific knowledge (which fields are enums, what their values are, what
/// numeric ranges apply); the generic <see cref="DefinitionEditor"/> supplies the rest.
/// </summary>
public interface IDefinitionSchema
{
    /// <summary>Editing metadata for a member, or null to use the type-inferred default.</summary>
    FieldEditHint? GetHint(string memberName);
}
