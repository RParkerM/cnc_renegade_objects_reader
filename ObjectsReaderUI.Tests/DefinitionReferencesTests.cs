using ObjectsReaderUI.Editing;

namespace ObjectsReaderUI.Tests;

public class DefinitionReferencesTests
{
    /// <summary>
    /// Every registered reference pair must name a real <c>int</c>, <c>int[]</c>, or
    /// <c>int[,]</c> instance field on its type (or a base type). The member names are string
    /// literals — some are protected fields that can't be <c>nameof</c>-checked — so this guards
    /// against typos and future renames that would otherwise silently stop the click-through
    /// links from resolving.
    /// </summary>
    public static IEnumerable<object[]> References =>
        DefinitionReferences.All.Select(r => new object[] { r.Type, r.Member });

    [Theory]
    [MemberData(nameof(References))]
    public void RegisteredReference_ResolvesToIntField(Type type, string member)
    {
        var field = DefinitionReflection.GetAllInstanceFields(type)
            .FirstOrDefault(f => f.Name == member);

        Assert.True(field is not null,
            $"{type.Name} has no instance field named '{member}'.");
        Assert.True(field!.FieldType == typeof(int) || field.FieldType == typeof(int[])
                    || field.FieldType == typeof(int[,]),
            $"{type.Name}.{member} is {field.FieldType.Name}, expected int, int[], or int[,].");
    }

    /// <summary>The runtime matcher must agree with the registry it's built from.</summary>
    [Theory]
    [MemberData(nameof(References))]
    public void RegisteredReference_IsMatchedByIsReference(Type type, string member)
    {
        Assert.True(DefinitionReferences.IsReference(type, member),
            $"IsReference did not match registered pair ({type.Name}, {member}).");
    }
}
