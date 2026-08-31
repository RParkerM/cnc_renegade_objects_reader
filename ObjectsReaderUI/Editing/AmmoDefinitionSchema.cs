namespace ObjectsReaderUI.Editing;

/// <summary>
/// Editing rules specific to <see cref="RenData.Definitions.AmmoDefinitionClass"/>.
/// </summary>
public sealed class AmmoDefinitionSchema : IDefinitionSchema
{
    private static readonly Dictionary<string, FieldEditHint> Hints = new()
    {
        // AmmoType is stored as an int; these are the engine's AMMO_TYPE_* values (index = value).
        ["AmmoType"] = new FieldEditHint
        {
            Choices = ["AMMO_TYPE_NORMAL", "AMMO_TYPE_C4_REMOTE", "AMMO_TYPE_C4_TIMED", "AMMO_TYPE_C4_PROXIMITY"],
        },

        // BeamColor is an RGB color (X=R, Y=G, Z=B). The engine asserts each channel is in
        // [0,1] (DX8Wrapper::Convert_Color) and scales it to 0–255, so clamp per component.
        ["BeamColor"] = new FieldEditHint { Min = 0f, Max = 1f },
    };

    public FieldEditHint? GetHint(string memberName) => Hints.GetValueOrDefault(memberName);
}
