using RenData.Definitions;

namespace ObjectsReaderUI.Editing;

/// <summary>
/// Declares which definition members hold an <c>int</c> ID that points at another
/// definition, so the viewer can render them as click-through links instead of plain
/// numbers. This is a display concern (navigation), separate from the edit schema.
/// Add new reference fields here.
/// </summary>
public static class DefinitionReferences
{
    // (declaring type, member name) pairs whose int value is a definition ID.
    private static readonly HashSet<(Type, string)> References =
    [
        (typeof(WeaponDefinitionClass), nameof(WeaponDefinitionClass.PrimaryAmmoDefID)),
        (typeof(WeaponDefinitionClass), nameof(WeaponDefinitionClass.SecondaryAmmoDefID)),
    ];

    /// <summary>True if the given member of the given type is a definition-ID reference.</summary>
    public static bool IsReference(Type declaringType, string memberName) =>
        References.Contains((declaringType, memberName));
}
