namespace RenData.Types;

public static class BuildingConstants
{
    public enum BuildingType : int
    {
        TYPE_NONE = -1,
        TYPE_POWER_PLANT = 0,
        TYPE_SOLDIER_FACTORY = 1,
        TYPE_VEHICLE_FACTORY = 2,
        TYPE_REFINERY = 3,
        TYPE_COM_CENTER = 4,
        TYPE_REPAIR_BAY = 5,
        TYPE_SHRINE = 6,
        TYPE_HELIPAD = 7,
        TYPE_CONYARD = 8,
        TYPE_BASE_DEFENSE = 9,
        TYPE_COUNT = 10
    }

    public enum LegacyBuildingTeam : int
    {
        LEGACY_TEAM_GDI = 0,
        LEGACY_TEAM_NOD = 1
    }

    public const int BASE_COUNT = 2;
}
