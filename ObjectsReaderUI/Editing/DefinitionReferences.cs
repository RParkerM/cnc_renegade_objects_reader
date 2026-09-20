using RenData.Definitions;
using RenData.GameObjDef;

namespace ObjectsReaderUI.Editing;

/// <summary>
/// Declares which definition members hold an <c>int</c> ID that points at another
/// definition, so the viewer can render them as click-through links instead of plain
/// numbers. This is a display concern (navigation), separate from the edit schema.
/// Add new reference fields here.
/// </summary>
public static class DefinitionReferences
{
    // (owning type, member name) pairs whose int value is a definition ID. The type is
    // matched by assignability, so registering a base type (e.g. VehicleGameObjDef) also
    // covers its subclasses, and fields declared on an abstract base are matched on the
    // concrete instance type. Names of non-public inherited fields are given as literals.
    private static readonly (Type Type, string Member)[] References =
    [
        (typeof(WeaponDefinitionClass), nameof(WeaponDefinitionClass.PrimaryAmmoDefID)),
        (typeof(WeaponDefinitionClass), nameof(WeaponDefinitionClass.SecondaryAmmoDefID)),
        (typeof(AmmoDefinitionClass), nameof(AmmoDefinitionClass.ExplosionDefID)),
        (typeof(AmmoDefinitionClass), nameof(AmmoDefinitionClass.FireSoundDefID)),
        // WeaponDefID / SecondaryWeaponDefID are protected fields on ArmedGameObjDef;
        // assignability matching makes these cover every armed object (vehicles, soldiers,
        // buildings, bosses, ...).
        (typeof(ArmedGameObjDef), "WeaponDefID"),
        (typeof(ArmedGameObjDef), "SecondaryWeaponDefID"),
    ];

    /// <summary>The registered reference pairs, exposed so tests can validate that each
    /// literal member name still resolves to an <c>int</c> field (the names can't be
    /// <c>nameof</c>-checked because some are protected fields on base types).</summary>
    public static IReadOnlyList<(Type Type, string Member)> All => References;

    /// <summary>True if the given member of the given object type is a definition-ID reference.</summary>
    public static bool IsReference(Type objectType, string memberName)
    {
        foreach (var (type, member) in References)
            if (member == memberName && type.IsAssignableFrom(objectType))
                return true;
        return false;
    }
}
