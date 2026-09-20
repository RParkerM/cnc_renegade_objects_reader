using RenData.Definitions;

namespace ObjectsReaderUI.Editing;

/// <summary>
/// Maps definition CLR types to their edit schema. A type present here is editable;
/// anything absent stays read-only in the viewer. Register new editable definitions here.
/// </summary>
public static class DefinitionSchemaRegistry
{
    private static readonly Dictionary<Type, IDefinitionSchema> Schemas = new()
    {
        [typeof(AmmoDefinitionClass)] = new AmmoDefinitionSchema(),
    };

    public static IDefinitionSchema? For(object definition) => Schemas.GetValueOrDefault(definition.GetType());
}
