using System.Numerics;
using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;
using RenData.Types;

namespace RenData.Definitions;

[RegisterDefinition(ChunkId.CHUNKID_GLOBAL_SETTINGS_DEF_HUD)]
public partial class HUDGlobalSettingsDef : DefinitionClass
{
    public HUDGlobalSettingsDef()
    {
        NodColor = new Vector3(1, 0, 0);
        GDIColor = new Vector3(1, 1, 0);
        NeutralColor = new Vector3(1, 1, 1);
        MutantColor = new Vector3(0, 1, 0);
        RenegadeColor = new Vector3(0, 0, 1);
        PrimaryObjectiveColor = new Vector3(0, 1, 0);
        SecondaryObjectiveColor = new Vector3(0, 0, 1);
        TertiaryObjectiveColor = new Vector3(1, 0, 1);
        HealthHighColor = new Vector3(0, 1, 0);
        HealthMedColor = new Vector3(1, 1, 0);
        HealthLowColor = new Vector3(1, 0, 0);
        EnemyColor = new Vector3(1, 0, 0);
        FriendlyColor = new Vector3(0, 1, 0);
        NoRelationColor = new Vector3(1, 1, 1);

        InfoTextureSize = 128;

        // Star info defaults (computed from #defines in C++)
        StarBracketSize = new Vector2(72, 56);
        StarBracketOffset = new Vector2(-70, -58);
        StarBracketUV = new RectClass(55, 1, 127, 57);
        StarBracketTopSize = new Vector2(38, 16);
        StarBracketTopOffset = new Vector2(-45, -74);
        StarBracketTopUV = new RectClass(2, 81, 40, 97);
        StarBracketTopArmedSize = new Vector2(38, 16);
        StarBracketTopArmedOffset = new Vector2(-45, -74);
        StarBracketTopArmedUV = new RectClass(40, 81, 2, 97);
        StarBarSize = new Vector2(130, 18);
        StarBarOffset = new Vector2(-198, -27);
        StarBarUV = new RectClass(55, 60, 79, 78);
        StarBarEndSize = new Vector2(32, 24);
        StarBarEndOffset = new Vector2(-224, -29);
        StarBarEndUV = new RectClass(79, 57, 111, 81);
        StarHealthSize = new Vector2(130, 6);
        StarHealthOffset = new Vector2(-198, -24);
        StarHealthUV = new RectClass(112, 66, 126, 72);
        StarShieldSize = new Vector2(134, 6);
        StarShieldOffset = new Vector2(-202, -17);
        StarShieldUV = new RectClass(112, 66, 126, 72);
        StarWeaponIconSize = new Vector2(64, 64);
        StarWeaponIconOffset = new Vector2(-70, -128);

        // Target info defaults
        TargetBracketSize = new Vector2(21, 52);
        TargetBracketOffset = new Vector2(68, -60);
        TargetBracketUV = new RectClass(2, 1, 23, 53);
        TargetIconSize = new Vector2(64, 64);
        TargetIconOffset = new Vector2(2, -68);
        TargetNameBarSize = new Vector2(120, 20);
        TargetNameBarOffset = new Vector2(87, -58);
        TargetNameBarUV = new RectClass(1, 59, 31, 79);
        TargetNameOffset = new Vector2(92, -52);
        TargetBarSize = new Vector2(130, 18);
        TargetBarOffset = new Vector2(87, -27);
        TargetBarUV = new RectClass(55, 60, 79, 78);
        TargetBarEndSize = new Vector2(32, 24);
        TargetBarEndOffset = new Vector2(211, -29);
        TargetBarEndUV = new RectClass(111, 57, 79, 81);
        TargetHealthSize = new Vector2(130, 6);
        TargetHealthOffset = new Vector2(87, -24);
        TargetHealthUV = new RectClass(126, 66, 112, 72);
        TargetShieldSize = new Vector2(134, 6);
        TargetShieldOffset = new Vector2(87, -17);
        TargetShieldUV = new RectClass(126, 66, 112, 72);

        // Radar defaults
        RadarTextureSize = 128;
        RadarOffset = new Vector2(82, -124);
        RadarRadius = 64;
        RadarFrameSize = new Vector2(112, 128);
        RadarFrameUV = new RectClass(0, 0, 112, 128);
        RadarCompassOffset = new Vector2(-7, 54);
        RadarCompassSize = new Vector2(16, 8);
        RadarCompassBaseUV = new RectClass(112, 64, 128, 72);
        RadarCompassUVOffset = new Vector2(0, 8);
        RadarHumanBlipUV = new RectClass(112, 0, 120, 8);
        RadarVehicleBlipUV = new RectClass(120, 0, 128, 8);
        RadarStationaryBlipUV = new RectClass(112, 8, 120, 16);
        RadarObjectiveBlipUV = new RectClass(120, 8, 128, 16);
        RadarBlipBracketUV = new RectClass(112, 16, 120, 24);
        RadarSweepUV = new RectClass(121, 24, 127, 32);
        RadarOnSoundID = 0;
        RadarOffSoundID = 0;

        // Sniper defaults
        SniperTextureSize = 256;
        SniperView = new RectClass(0.2f, 0.12f, 0.8f, 0.88f);
        SniperViewUV = new RectClass(0, 0, 240, 227);
        SniperScanLineUV = new RectClass(0.01f, 0.01f, 0.05f, 0.05f);
        SniperBlackCoverUV = new RectClass(0.01f, 0.01f, 0.05f, 0.05f);
        SniperTiltBar = new RectClass(0.20f, 0.25f, 0.225f, 0.75f);
        SniperTiltBarRate = 1;
        SniperTiltBarUV = new RectClass(245, 3, 250, 208);
        SniperTurnBar = new RectClass(0.35f, 0.25f, 0.65f, 0.275f);
        SniperTurnBarRate = 1;
        SniperTurnBarUV = new RectClass(1, 244, 109, 253);
        SniperDistanceGraph = new RectClass(0.175f, 0.3f, 0.2f, 0.65f);
        SniperDistanceGraphUV = new RectClass(1, 231, 87, 239);
        SniperDistanceGraphMax = 200;
        SniperZoomGraph = new RectClass(0.72f, 0.18f, 0.8f, 0.22f);
        SniperZoomGraphUV = new RectClass(218, 247, 254, 254);

        DamageIndicatorUV = new RectClass(31, 1, 51, 59);
        DamageDiagIndicatorUV = new RectClass(1, 60, 47, 106);
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GLOBAL_SETTINGS_DEF_HUD;

    public override PersistClass? Create()
    {
        //WWASSERT(0);
        return null;
    }

    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_HUD_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_HUD_DEF_VARIABLES);

        csave.WriteMicro(MICROCHUNKID_HUD_DEF_NOD_COLOR, NodColor);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_GDI_COLOR, GDIColor);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_NEUTRAL_COLOR, NeutralColor);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_MUTANT_COLOR, MutantColor);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_RENEGADE_COLOR, RenegadeColor);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_PRIMARY_OBJECTIVE_COLOR, PrimaryObjectiveColor);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_SECONDARY_OBJECTIVE_COLOR, SecondaryObjectiveColor);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TERTIARY_OBJECTIVE_COLOR, TertiaryObjectiveColor);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_HEALTH_HIGH_COLOR, HealthHighColor);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_HEALTH_MED_COLOR, HealthMedColor);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_HEALTH_LOW_COLOR, HealthLowColor);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_ENEMY_COLOR, EnemyColor);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_FRIENDLY_COLOR, FriendlyColor);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_NO_RELATION_COLOR, NoRelationColor);

        csave.WriteMicro(MICROCHUNKID_HUD_DEF_INFO_TEXTURE_SIZE, InfoTextureSize);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_BRACKET_SIZE, StarBracketSize);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_BRACKET_OFFSET, StarBracketOffset);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_BRACKET_UV, (RectClassStruct)StarBracketUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_BRACKET_TOP_SIZE, StarBracketTopSize);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_BRACKET_TOP_OFFSET, StarBracketTopOffset);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_BRACKET_TOP_UV, (RectClassStruct)StarBracketTopUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_BRACKET_TOP_ARMED_SIZE, StarBracketTopArmedSize);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_BRACKET_TOP_ARMED_OFFSET, StarBracketTopArmedOffset);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_BRACKET_TOP_ARMED_UV, (RectClassStruct)StarBracketTopArmedUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_BAR_SIZE, StarBarSize);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_BAR_OFFSET, StarBarOffset);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_BAR_UV, (RectClassStruct)StarBarUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_BAR_END_SIZE, StarBarEndSize);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_BAR_END_OFFSET, StarBarEndOffset);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_BAR_END_UV, (RectClassStruct)StarBarEndUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_HEALTH_SIZE, StarHealthSize);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_HEALTH_OFFSET, StarHealthOffset);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_HEALTH_UV, (RectClassStruct)StarHealthUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_SHIELD_SIZE, StarShieldSize);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_SHIELD_OFFSET, StarShieldOffset);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_SHIELD_UV, (RectClassStruct)StarShieldUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_WEAPON_ICON_SIZE, StarWeaponIconSize);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_STAR_WEAPON_ICON_OFFSET, StarWeaponIconOffset);

        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TARGET_BRACKET_SIZE, TargetBracketSize);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TARGET_BRACKET_OFFSET, TargetBracketOffset);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TARGET_BRACKET_UV, (RectClassStruct)TargetBracketUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TARGET_ICON_SIZE, TargetIconSize);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TARGET_ICON_OFFSET, TargetIconOffset);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TARGET_NAME_BAR_SIZE, TargetNameBarSize);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TARGET_NAME_BAR_OFFSET, TargetNameBarOffset);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TARGET_NAME_BAR_UV, (RectClassStruct)TargetNameBarUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TARGET_NAME_OFFSET, TargetNameOffset);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TARGET_BAR_SIZE, TargetBarSize);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TARGET_BAR_OFFSET, TargetBarOffset);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TARGET_BAR_UV, (RectClassStruct)TargetBarUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TARGET_BAR_END_SIZE, TargetBarEndSize);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TARGET_BAR_END_OFFSET, TargetBarEndOffset);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TARGET_BAR_END_UV, (RectClassStruct)TargetBarEndUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TARGET_HEALTH_SIZE, TargetHealthSize);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TARGET_HEALTH_OFFSET, TargetHealthOffset);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TARGET_HEALTH_UV, (RectClassStruct)TargetHealthUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TARGET_SHIELD_SIZE, TargetShieldSize);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TARGET_SHIELD_OFFSET, TargetShieldOffset);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_TARGET_SHIELD_UV, (RectClassStruct)TargetShieldUV);

        csave.WriteMicro(MICROCHUNKID_HUD_DEF_RADAR_TEXTURE_SIZE, RadarTextureSize);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_RADAR_OFFSET, RadarOffset);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_RADAR_RADIUS, RadarRadius);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_RADAR_FRAME_SIZE, RadarFrameSize);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_RADAR_FRAME_UV, (RectClassStruct)RadarFrameUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_RADAR_COMPASS_OFFSET, RadarCompassOffset);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_RADAR_COMPASS_SIZE, RadarCompassSize);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_RADAR_COMPASS_BASE_UV, (RectClassStruct)RadarCompassBaseUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_RADAR_COMPASS_UV_OFFSET, RadarCompassUVOffset);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_RADAR_HUMAN_BLIP_UV, (RectClassStruct)RadarHumanBlipUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_RADAR_VEHICLE_BLIP_UV, (RectClassStruct)RadarVehicleBlipUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_RADAR_STATIONARY_BLIP_UV, (RectClassStruct)RadarStationaryBlipUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_RADAR_OBJECTIVE_BLIP_UV, (RectClassStruct)RadarObjectiveBlipUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_RADAR_BLIP_BRACKET_UV, (RectClassStruct)RadarBlipBracketUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_RADAR_SWEEP_UV, (RectClassStruct)RadarSweepUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_RADAR_ON_SOUND_ID, RadarOnSoundID);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_RADAR_OFF_SOUND_ID, RadarOffSoundID);

        csave.WriteMicro(MICROCHUNKID_HUD_DEF_SNIPER_TEXTURE_SIZE, SniperTextureSize);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_SNIPER_VIEW, (RectClassStruct)SniperView);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_SNIPER_VIEW_UV, (RectClassStruct)SniperViewUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_SNIPER_SCAN_LINE_UV, (RectClassStruct)SniperScanLineUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_SNIPER_BLACK_COVER_UV, (RectClassStruct)SniperBlackCoverUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_SNIPER_TILT_BAR, (RectClassStruct)SniperTiltBar);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_SNIPER_TILT_BAR_RATE, SniperTiltBarRate);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_SNIPER_TILT_BAR_UV, (RectClassStruct)SniperTiltBarUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_SNIPER_TURN_BAR, (RectClassStruct)SniperTurnBar);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_SNIPER_TURN_BAR_RATE, SniperTurnBarRate);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_SNIPER_TURN_BAR_UV, (RectClassStruct)SniperTurnBarUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_SNIPER_DISTANCE_GRAPH, (RectClassStruct)SniperDistanceGraph);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_SNIPER_DISTANCE_GRAPH_UV, (RectClassStruct)SniperDistanceGraphUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_SNIPER_DISTANCE_GRAPH_MAX, SniperDistanceGraphMax);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_SNIPER_ZOOM_GRAPH, (RectClassStruct)SniperZoomGraph);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_SNIPER_ZOOM_GRAPH_UV, (RectClassStruct)SniperZoomGraphUV);

        csave.WriteMicro(MICROCHUNKID_HUD_DEF_DAMAGE_INDICATOR_UV, (RectClassStruct)DamageIndicatorUV);
        csave.WriteMicro(MICROCHUNKID_HUD_DEF_DAMAGE_DIAG_INDICATOR_UV, (RectClassStruct)DamageDiagIndicatorUV);

        csave.End_Chunk();

        return true;
    }

    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {
                case CHUNKID_HUD_DEF_PARENT:
                    base.Load(cload);
                    break;

                case CHUNKID_HUD_DEF_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        RectClassStruct rc = default;
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case MICROCHUNKID_HUD_DEF_NOD_COLOR: cload.Read(ref NodColor); break;
                            case MICROCHUNKID_HUD_DEF_GDI_COLOR: cload.Read(ref GDIColor); break;
                            case MICROCHUNKID_HUD_DEF_NEUTRAL_COLOR: cload.Read(ref NeutralColor); break;
                            case MICROCHUNKID_HUD_DEF_MUTANT_COLOR: cload.Read(ref MutantColor); break;
                            case MICROCHUNKID_HUD_DEF_RENEGADE_COLOR: cload.Read(ref RenegadeColor); break;
                            case MICROCHUNKID_HUD_DEF_PRIMARY_OBJECTIVE_COLOR: cload.Read(ref PrimaryObjectiveColor); break;
                            case MICROCHUNKID_HUD_DEF_SECONDARY_OBJECTIVE_COLOR: cload.Read(ref SecondaryObjectiveColor); break;
                            case MICROCHUNKID_HUD_DEF_TERTIARY_OBJECTIVE_COLOR: cload.Read(ref TertiaryObjectiveColor); break;
                            case MICROCHUNKID_HUD_DEF_HEALTH_HIGH_COLOR: cload.Read(ref HealthHighColor); break;
                            case MICROCHUNKID_HUD_DEF_HEALTH_MED_COLOR: cload.Read(ref HealthMedColor); break;
                            case MICROCHUNKID_HUD_DEF_HEALTH_LOW_COLOR: cload.Read(ref HealthLowColor); break;
                            case MICROCHUNKID_HUD_DEF_ENEMY_COLOR: cload.Read(ref EnemyColor); break;
                            case MICROCHUNKID_HUD_DEF_FRIENDLY_COLOR: cload.Read(ref FriendlyColor); break;
                            case MICROCHUNKID_HUD_DEF_NO_RELATION_COLOR: cload.Read(ref NoRelationColor); break;

                            case MICROCHUNKID_HUD_DEF_INFO_TEXTURE_SIZE: cload.Read(ref InfoTextureSize); break;
                            case MICROCHUNKID_HUD_DEF_STAR_BRACKET_SIZE: cload.Read(ref StarBracketSize); break;
                            case MICROCHUNKID_HUD_DEF_STAR_BRACKET_OFFSET: cload.Read(ref StarBracketOffset); break;
                            case MICROCHUNKID_HUD_DEF_STAR_BRACKET_UV: cload.Read(ref rc); StarBracketUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_STAR_BRACKET_TOP_SIZE: cload.Read(ref StarBracketTopSize); break;
                            case MICROCHUNKID_HUD_DEF_STAR_BRACKET_TOP_OFFSET: cload.Read(ref StarBracketTopOffset); break;
                            case MICROCHUNKID_HUD_DEF_STAR_BRACKET_TOP_UV: cload.Read(ref rc); StarBracketTopUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_STAR_BRACKET_TOP_ARMED_SIZE: cload.Read(ref StarBracketTopArmedSize); break;
                            case MICROCHUNKID_HUD_DEF_STAR_BRACKET_TOP_ARMED_OFFSET: cload.Read(ref StarBracketTopArmedOffset); break;
                            case MICROCHUNKID_HUD_DEF_STAR_BRACKET_TOP_ARMED_UV: cload.Read(ref rc); StarBracketTopArmedUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_STAR_BAR_SIZE: cload.Read(ref StarBarSize); break;
                            case MICROCHUNKID_HUD_DEF_STAR_BAR_OFFSET: cload.Read(ref StarBarOffset); break;
                            case MICROCHUNKID_HUD_DEF_STAR_BAR_UV: cload.Read(ref rc); StarBarUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_STAR_BAR_END_SIZE: cload.Read(ref StarBarEndSize); break;
                            case MICROCHUNKID_HUD_DEF_STAR_BAR_END_OFFSET: cload.Read(ref StarBarEndOffset); break;
                            case MICROCHUNKID_HUD_DEF_STAR_BAR_END_UV: cload.Read(ref rc); StarBarEndUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_STAR_HEALTH_SIZE: cload.Read(ref StarHealthSize); break;
                            case MICROCHUNKID_HUD_DEF_STAR_HEALTH_OFFSET: cload.Read(ref StarHealthOffset); break;
                            case MICROCHUNKID_HUD_DEF_STAR_HEALTH_UV: cload.Read(ref rc); StarHealthUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_STAR_SHIELD_SIZE: cload.Read(ref StarShieldSize); break;
                            case MICROCHUNKID_HUD_DEF_STAR_SHIELD_OFFSET: cload.Read(ref StarShieldOffset); break;
                            case MICROCHUNKID_HUD_DEF_STAR_SHIELD_UV: cload.Read(ref rc); StarShieldUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_STAR_WEAPON_ICON_SIZE: cload.Read(ref StarWeaponIconSize); break;
                            case MICROCHUNKID_HUD_DEF_STAR_WEAPON_ICON_OFFSET: cload.Read(ref StarWeaponIconOffset); break;

                            case MICROCHUNKID_HUD_DEF_TARGET_BRACKET_SIZE: cload.Read(ref TargetBracketSize); break;
                            case MICROCHUNKID_HUD_DEF_TARGET_BRACKET_OFFSET: cload.Read(ref TargetBracketOffset); break;
                            case MICROCHUNKID_HUD_DEF_TARGET_BRACKET_UV: cload.Read(ref rc); TargetBracketUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_TARGET_ICON_SIZE: cload.Read(ref TargetIconSize); break;
                            case MICROCHUNKID_HUD_DEF_TARGET_ICON_OFFSET: cload.Read(ref TargetIconOffset); break;
                            case MICROCHUNKID_HUD_DEF_TARGET_NAME_BAR_SIZE: cload.Read(ref TargetNameBarSize); break;
                            case MICROCHUNKID_HUD_DEF_TARGET_NAME_BAR_OFFSET: cload.Read(ref TargetNameBarOffset); break;
                            case MICROCHUNKID_HUD_DEF_TARGET_NAME_BAR_UV: cload.Read(ref rc); TargetNameBarUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_TARGET_NAME_OFFSET: cload.Read(ref TargetNameOffset); break;
                            case MICROCHUNKID_HUD_DEF_TARGET_BAR_SIZE: cload.Read(ref TargetBarSize); break;
                            case MICROCHUNKID_HUD_DEF_TARGET_BAR_OFFSET: cload.Read(ref TargetBarOffset); break;
                            case MICROCHUNKID_HUD_DEF_TARGET_BAR_UV: cload.Read(ref rc); TargetBarUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_TARGET_BAR_END_SIZE: cload.Read(ref TargetBarEndSize); break;
                            case MICROCHUNKID_HUD_DEF_TARGET_BAR_END_OFFSET: cload.Read(ref TargetBarEndOffset); break;
                            case MICROCHUNKID_HUD_DEF_TARGET_BAR_END_UV: cload.Read(ref rc); TargetBarEndUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_TARGET_HEALTH_SIZE: cload.Read(ref TargetHealthSize); break;
                            case MICROCHUNKID_HUD_DEF_TARGET_HEALTH_OFFSET: cload.Read(ref TargetHealthOffset); break;
                            case MICROCHUNKID_HUD_DEF_TARGET_HEALTH_UV: cload.Read(ref rc); TargetHealthUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_TARGET_SHIELD_SIZE: cload.Read(ref TargetShieldSize); break;
                            case MICROCHUNKID_HUD_DEF_TARGET_SHIELD_OFFSET: cload.Read(ref TargetShieldOffset); break;
                            case MICROCHUNKID_HUD_DEF_TARGET_SHIELD_UV: cload.Read(ref rc); TargetShieldUV.Set(rc); break;

                            case MICROCHUNKID_HUD_DEF_RADAR_TEXTURE_SIZE: cload.Read(ref RadarTextureSize); break;
                            case MICROCHUNKID_HUD_DEF_RADAR_OFFSET: cload.Read(ref RadarOffset); break;
                            case MICROCHUNKID_HUD_DEF_RADAR_RADIUS: cload.Read(ref RadarRadius); break;
                            case MICROCHUNKID_HUD_DEF_RADAR_FRAME_SIZE: cload.Read(ref RadarFrameSize); break;
                            case MICROCHUNKID_HUD_DEF_RADAR_FRAME_UV: cload.Read(ref rc); RadarFrameUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_RADAR_COMPASS_OFFSET: cload.Read(ref RadarCompassOffset); break;
                            case MICROCHUNKID_HUD_DEF_RADAR_COMPASS_SIZE: cload.Read(ref RadarCompassSize); break;
                            case MICROCHUNKID_HUD_DEF_RADAR_COMPASS_BASE_UV: cload.Read(ref rc); RadarCompassBaseUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_RADAR_COMPASS_UV_OFFSET: cload.Read(ref RadarCompassUVOffset); break;
                            case MICROCHUNKID_HUD_DEF_RADAR_HUMAN_BLIP_UV: cload.Read(ref rc); RadarHumanBlipUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_RADAR_VEHICLE_BLIP_UV: cload.Read(ref rc); RadarVehicleBlipUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_RADAR_STATIONARY_BLIP_UV: cload.Read(ref rc); RadarStationaryBlipUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_RADAR_OBJECTIVE_BLIP_UV: cload.Read(ref rc); RadarObjectiveBlipUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_RADAR_BLIP_BRACKET_UV: cload.Read(ref rc); RadarBlipBracketUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_RADAR_SWEEP_UV: cload.Read(ref rc); RadarSweepUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_RADAR_ON_SOUND_ID: cload.Read(ref RadarOnSoundID); break;
                            case MICROCHUNKID_HUD_DEF_RADAR_OFF_SOUND_ID: cload.Read(ref RadarOffSoundID); break;

                            case MICROCHUNKID_HUD_DEF_SNIPER_TEXTURE_SIZE: cload.Read(ref SniperTextureSize); break;
                            case MICROCHUNKID_HUD_DEF_SNIPER_VIEW: cload.Read(ref rc); SniperView.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_SNIPER_VIEW_UV: cload.Read(ref rc); SniperViewUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_SNIPER_SCAN_LINE_UV: cload.Read(ref rc); SniperScanLineUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_SNIPER_BLACK_COVER_UV: cload.Read(ref rc); SniperBlackCoverUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_SNIPER_TILT_BAR: cload.Read(ref rc); SniperTiltBar.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_SNIPER_TILT_BAR_RATE: cload.Read(ref SniperTiltBarRate); break;
                            case MICROCHUNKID_HUD_DEF_SNIPER_TILT_BAR_UV: cload.Read(ref rc); SniperTiltBarUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_SNIPER_TURN_BAR: cload.Read(ref rc); SniperTurnBar.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_SNIPER_TURN_BAR_RATE: cload.Read(ref SniperTurnBarRate); break;
                            case MICROCHUNKID_HUD_DEF_SNIPER_TURN_BAR_UV: cload.Read(ref rc); SniperTurnBarUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_SNIPER_DISTANCE_GRAPH: cload.Read(ref rc); SniperDistanceGraph.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_SNIPER_DISTANCE_GRAPH_UV: cload.Read(ref rc); SniperDistanceGraphUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_SNIPER_DISTANCE_GRAPH_MAX: cload.Read(ref SniperDistanceGraphMax); break;
                            case MICROCHUNKID_HUD_DEF_SNIPER_ZOOM_GRAPH: cload.Read(ref rc); SniperZoomGraph.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_SNIPER_ZOOM_GRAPH_UV: cload.Read(ref rc); SniperZoomGraphUV.Set(rc); break;

                            case MICROCHUNKID_HUD_DEF_DAMAGE_INDICATOR_UV: cload.Read(ref rc); DamageIndicatorUV.Set(rc); break;
                            case MICROCHUNKID_HUD_DEF_DAMAGE_DIAG_INDICATOR_UV: cload.Read(ref rc); DamageDiagIndicatorUV.Set(rc); break;

                            default:
                                Console.WriteLine("Unhandled HUDGlobalSettingsDef Variable chunkID\n");
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unhandled HUDGlobalSettingsDef chunkID\n");
                    break;
            }
            cload.Close_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    // Colors (Vector3)
    protected Vector3 NodColor;
    protected Vector3 GDIColor;
    protected Vector3 NeutralColor;
    protected Vector3 MutantColor;
    protected Vector3 RenegadeColor;
    protected Vector3 PrimaryObjectiveColor;
    protected Vector3 SecondaryObjectiveColor;
    protected Vector3 TertiaryObjectiveColor;
    protected Vector3 HealthHighColor;
    protected Vector3 HealthMedColor;
    protected Vector3 HealthLowColor;
    protected Vector3 EnemyColor;
    protected Vector3 FriendlyColor;
    protected Vector3 NoRelationColor;

    // Star info
    protected float InfoTextureSize;
    protected Vector2 StarBracketSize;
    protected Vector2 StarBracketOffset;
    protected RectClass StarBracketUV;
    protected Vector2 StarBracketTopSize;
    protected Vector2 StarBracketTopOffset;
    protected RectClass StarBracketTopUV;
    protected Vector2 StarBracketTopArmedSize;
    protected Vector2 StarBracketTopArmedOffset;
    protected RectClass StarBracketTopArmedUV;
    protected Vector2 StarBarSize;
    protected Vector2 StarBarOffset;
    protected RectClass StarBarUV;
    protected Vector2 StarBarEndSize;
    protected Vector2 StarBarEndOffset;
    protected RectClass StarBarEndUV;
    protected Vector2 StarHealthSize;
    protected Vector2 StarHealthOffset;
    protected RectClass StarHealthUV;
    protected Vector2 StarShieldSize;
    protected Vector2 StarShieldOffset;
    protected RectClass StarShieldUV;
    protected Vector2 StarWeaponIconSize;
    protected Vector2 StarWeaponIconOffset;

    // Target info
    protected Vector2 TargetBracketSize;
    protected Vector2 TargetBracketOffset;
    protected RectClass TargetBracketUV;
    protected Vector2 TargetIconSize;
    protected Vector2 TargetIconOffset;
    protected Vector2 TargetNameBarSize;
    protected Vector2 TargetNameBarOffset;
    protected RectClass TargetNameBarUV;
    protected Vector2 TargetNameOffset;
    protected Vector2 TargetBarSize;
    protected Vector2 TargetBarOffset;
    protected RectClass TargetBarUV;
    protected Vector2 TargetBarEndSize;
    protected Vector2 TargetBarEndOffset;
    protected RectClass TargetBarEndUV;
    protected Vector2 TargetHealthSize;
    protected Vector2 TargetHealthOffset;
    protected RectClass TargetHealthUV;
    protected Vector2 TargetShieldSize;
    protected Vector2 TargetShieldOffset;
    protected RectClass TargetShieldUV;

    // Radar
    protected float RadarTextureSize;
    protected Vector2 RadarOffset;
    protected float RadarRadius;
    protected Vector2 RadarFrameSize;
    protected RectClass RadarFrameUV;
    protected Vector2 RadarCompassOffset;
    protected Vector2 RadarCompassSize;
    protected RectClass RadarCompassBaseUV;
    protected Vector2 RadarCompassUVOffset;
    protected RectClass RadarHumanBlipUV;
    protected RectClass RadarVehicleBlipUV;
    protected RectClass RadarStationaryBlipUV;
    protected RectClass RadarObjectiveBlipUV;
    protected RectClass RadarBlipBracketUV;
    protected RectClass RadarSweepUV;
    protected int RadarOnSoundID;
    protected int RadarOffSoundID;

    // Sniper
    protected float SniperTextureSize;
    protected RectClass SniperView;
    protected RectClass SniperViewUV;
    protected RectClass SniperScanLineUV;
    protected RectClass SniperBlackCoverUV;
    protected RectClass SniperTiltBar;
    protected float SniperTiltBarRate;
    protected RectClass SniperTiltBarUV;
    protected RectClass SniperTurnBar;
    protected float SniperTurnBarRate;
    protected RectClass SniperTurnBarUV;
    protected RectClass SniperDistanceGraph;
    protected RectClass SniperDistanceGraphUV;
    protected float SniperDistanceGraphMax;
    protected RectClass SniperZoomGraph;
    protected RectClass SniperZoomGraphUV;

    // Damage indicators
    protected RectClass DamageIndicatorUV;
    protected RectClass DamageDiagIndicatorUV;


    private const uint CHUNKID_HUD_DEF_PARENT = 803001812;
    private const uint CHUNKID_HUD_DEF_VARIABLES = 803001813;

    private const byte MICROCHUNKID_HUD_DEF_NOD_COLOR = 1;
    private const byte MICROCHUNKID_HUD_DEF_GDI_COLOR = 2;
    private const byte MICROCHUNKID_HUD_DEF_NEUTRAL_COLOR = 3;
    private const byte MICROCHUNKID_HUD_DEF_PRIMARY_OBJECTIVE_COLOR = 4;
    private const byte MICROCHUNKID_HUD_DEF_SECONDARY_OBJECTIVE_COLOR = 5;
    private const byte MICROCHUNKID_HUD_DEF_TERTIARY_OBJECTIVE_COLOR = 6;
    private const byte MICROCHUNKID_HUD_DEF_STAR_BRACKET_SIZE = 7;
    private const byte MICROCHUNKID_HUD_DEF_STAR_BRACKET_OFFSET = 8;
    private const byte MICROCHUNKID_HUD_DEF_STAR_BRACKET_UV = 9;
    private const byte MICROCHUNKID_HUD_DEF_STAR_BRACKET_TOP_SIZE = 10;
    private const byte MICROCHUNKID_HUD_DEF_STAR_BRACKET_TOP_OFFSET = 11;
    private const byte MICROCHUNKID_HUD_DEF_STAR_BRACKET_TOP_UV = 12;
    private const byte MICROCHUNKID_HUD_DEF_STAR_BAR_SIZE = 13;
    private const byte MICROCHUNKID_HUD_DEF_STAR_BAR_OFFSET = 14;
    private const byte MICROCHUNKID_HUD_DEF_STAR_BAR_UV = 15;
    private const byte MICROCHUNKID_HUD_DEF_STAR_BAR_END_SIZE = 16;
    private const byte MICROCHUNKID_HUD_DEF_STAR_BAR_END_OFFSET = 17;
    private const byte MICROCHUNKID_HUD_DEF_STAR_BAR_END_UV = 18;
    private const byte MICROCHUNKID_HUD_DEF_STAR_HEALTH_SIZE = 19;
    private const byte MICROCHUNKID_HUD_DEF_STAR_HEALTH_OFFSET = 20;
    private const byte MICROCHUNKID_HUD_DEF_STAR_HEALTH_UV = 21;
    private const byte MICROCHUNKID_HUD_DEF_STAR_SHIELD_SIZE = 22;
    private const byte MICROCHUNKID_HUD_DEF_STAR_SHIELD_OFFSET = 23;
    private const byte MICROCHUNKID_HUD_DEF_STAR_SHIELD_UV = 24;
    private const byte MICROCHUNKID_HUD_DEF_TARGET_BRACKET_SIZE = 25;
    private const byte MICROCHUNKID_HUD_DEF_TARGET_BRACKET_OFFSET = 26;
    private const byte MICROCHUNKID_HUD_DEF_TARGET_BRACKET_UV = 27;
    private const byte MICROCHUNKID_HUD_DEF_TARGET_ICON_SIZE = 28;
    private const byte MICROCHUNKID_HUD_DEF_TARGET_ICON_OFFSET = 29;
    private const byte MICROCHUNKID_HUD_DEF_TARGET_NAME_BAR_SIZE = 30;
    private const byte MICROCHUNKID_HUD_DEF_TARGET_NAME_BAR_OFFSET = 31;
    private const byte MICROCHUNKID_HUD_DEF_TARGET_NAME_BAR_UV = 32;
    private const byte MICROCHUNKID_HUD_DEF_TARGET_NAME_OFFSET = 33;
    private const byte MICROCHUNKID_HUD_DEF_TARGET_BAR_SIZE = 34;
    private const byte MICROCHUNKID_HUD_DEF_TARGET_BAR_OFFSET = 35;
    private const byte MICROCHUNKID_HUD_DEF_TARGET_BAR_UV = 36;
    private const byte MICROCHUNKID_HUD_DEF_TARGET_BAR_END_SIZE = 37;
    private const byte MICROCHUNKID_HUD_DEF_TARGET_BAR_END_OFFSET = 38;
    private const byte MICROCHUNKID_HUD_DEF_TARGET_BAR_END_UV = 39;
    private const byte MICROCHUNKID_HUD_DEF_TARGET_HEALTH_SIZE = 40;
    private const byte MICROCHUNKID_HUD_DEF_TARGET_HEALTH_OFFSET = 41;
    private const byte MICROCHUNKID_HUD_DEF_TARGET_HEALTH_UV = 42;
    private const byte MICROCHUNKID_HUD_DEF_TARGET_SHIELD_SIZE = 43;
    private const byte MICROCHUNKID_HUD_DEF_TARGET_SHIELD_OFFSET = 44;
    private const byte MICROCHUNKID_HUD_DEF_TARGET_SHIELD_UV = 45;
    private const byte MICROCHUNKID_HUD_DEF_RADAR_OFFSET = 46;
    private const byte MICROCHUNKID_HUD_DEF_RADAR_RADIUS = 47;
    private const byte MICROCHUNKID_HUD_DEF_RADAR_FRAME_SIZE = 48;
    private const byte MICROCHUNKID_HUD_DEF_RADAR_FRAME_UV = 49;
    private const byte MICROCHUNKID_HUD_DEF_RADAR_COMPASS_OFFSET = 50;
    private const byte MICROCHUNKID_HUD_DEF_RADAR_COMPASS_SIZE = 51;
    private const byte MICROCHUNKID_HUD_DEF_RADAR_COMPASS_BASE_UV = 52;
    private const byte MICROCHUNKID_HUD_DEF_RADAR_COMPASS_UV_OFFSET = 53;
    private const byte MICROCHUNKID_HUD_DEF_RADAR_HUMAN_BLIP_UV = 54;
    private const byte MICROCHUNKID_HUD_DEF_RADAR_VEHICLE_BLIP_UV = 55;
    private const byte MICROCHUNKID_HUD_DEF_RADAR_STATIONARY_BLIP_UV = 56;
    private const byte MICROCHUNKID_HUD_DEF_RADAR_OBJECTIVE_BLIP_UV = 57;
    private const byte MICROCHUNKID_HUD_DEF_RADAR_BLIP_BRACKET_UV = 58;
    private const byte MICROCHUNKID_HUD_DEF_RADAR_SWEEP_UV = 59;
    private const byte MICROCHUNKID_HUD_DEF_STAR_BRACKET_TOP_ARMED_SIZE = 60;
    private const byte MICROCHUNKID_HUD_DEF_STAR_BRACKET_TOP_ARMED_OFFSET = 61;
    private const byte MICROCHUNKID_HUD_DEF_STAR_BRACKET_TOP_ARMED_UV = 62;
    private const byte MICROCHUNKID_HUD_DEF_STAR_WEAPON_ICON_SIZE = 63;
    private const byte MICROCHUNKID_HUD_DEF_STAR_WEAPON_ICON_OFFSET = 64;
    private const byte MICROCHUNKID_HUD_DEF_SNIPER_VIEW = 65;
    private const byte MICROCHUNKID_HUD_DEF_SNIPER_VIEW_UV = 66;
    private const byte MICROCHUNKID_HUD_DEF_SNIPER_BLACK_COVER_UV = 67;
    private const byte MICROCHUNKID_HUD_DEF_SNIPER_TILT_BAR = 68;
    private const byte MICROCHUNKID_HUD_DEF_SNIPER_TILT_BAR_RATE = 69;
    private const byte MICROCHUNKID_HUD_DEF_SNIPER_TILT_BAR_UV = 70;
    private const byte MICROCHUNKID_HUD_DEF_SNIPER_TURN_BAR = 71;
    private const byte MICROCHUNKID_HUD_DEF_SNIPER_TURN_BAR_RATE = 72;
    private const byte MICROCHUNKID_HUD_DEF_SNIPER_TURN_BAR_UV = 73;
    private const byte MICROCHUNKID_HUD_DEF_SNIPER_DISTANCE_GRAPH = 74;
    private const byte MICROCHUNKID_HUD_DEF_SNIPER_DISTANCE_GRAPH_UV = 75;
    private const byte MICROCHUNKID_HUD_DEF_SNIPER_DISTANCE_GRAPH_MAX = 76;
    private const byte MICROCHUNKID_HUD_DEF_SNIPER_SCAN_LINE_UV = 77;
    private const byte MICROCHUNKID_HUD_DEF_INFO_TEXTURE_SIZE = 78;
    private const byte MICROCHUNKID_HUD_DEF_RADAR_TEXTURE_SIZE = 79;
    private const byte MICROCHUNKID_HUD_DEF_SNIPER_TEXTURE_SIZE = 80;
    private const byte MICROCHUNKID_HUD_DEF_DAMAGE_INDICATOR_UV = 81;
    private const byte MICROCHUNKID_HUD_DEF_DAMAGE_DIAG_INDICATOR_UV = 82;
    private const byte MICROCHUNKID_HUD_DEF_SNIPER_ZOOM_GRAPH = 83;
    private const byte MICROCHUNKID_HUD_DEF_SNIPER_ZOOM_GRAPH_UV = 84;
    private const byte MICROCHUNKID_HUD_DEF_RADAR_ON_SOUND_ID = 85;
    private const byte MICROCHUNKID_HUD_DEF_RADAR_OFF_SOUND_ID = 86;
    private const byte MICROCHUNKID_HUD_DEF_MUTANT_COLOR = 87;
    private const byte MICROCHUNKID_HUD_DEF_RENEGADE_COLOR = 88;
    private const byte MICROCHUNKID_HUD_DEF_HEALTH_HIGH_COLOR = 89;
    private const byte MICROCHUNKID_HUD_DEF_HEALTH_MED_COLOR = 90;
    private const byte MICROCHUNKID_HUD_DEF_HEALTH_LOW_COLOR = 91;
    private const byte MICROCHUNKID_HUD_DEF_ENEMY_COLOR = 92;
    private const byte MICROCHUNKID_HUD_DEF_FRIENDLY_COLOR = 93;
    private const byte MICROCHUNKID_HUD_DEF_NO_RELATION_COLOR = 94;
}
