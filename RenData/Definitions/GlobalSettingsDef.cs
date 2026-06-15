using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.Definitions;

[RegisterDefinition(ChunkId.CHUNKID_GLOBAL_SETTINGS_DEF_GENERAL)]
public partial class GlobalSettingsDef : DefinitionClass
{
    public GlobalSettingsDef()
    {
        DeathSoundID = 0;
        EVAObjectivesSoundID = 0;
        HUDHelpTextSoundID = 0;
        MaxConversationDist = 10.0f;
        MaxCombatConversationDist = 10.0f;
        SoldierWalkSpeed = 0.25f;
        SoldierCrouchSpeed = 0.25f;
        EncyclopediaEventStringID = 0;
        FallingDamageMinDistance = 5;
        FallingDamageMaxDistance = 20;
        FallingDamageWarhead = 15;
        StealthDistanceHuman = 15.0f;
        StealthDistanceVehicle = 25.0f;
        MPStealthDistanceHuman = 15.0f;
        MPStealthDistanceVehicle = 25.0f;
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GLOBAL_SETTINGS_DEF_GENERAL;

    public override PersistClass? Create()
    {
        //WWASSERT(0);
        return null;
    }

    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        csave.WriteMicro(MICROCHUNKID_DEF_DEATH_SOUND, DeathSoundID);
        csave.WriteMicro(MICROCHUNKID_DEF_EVA_MO_SOUND, EVAObjectivesSoundID);
        csave.WriteMicro(MICROCHUNKID_DEF_HELP_TXT_SOUND, HUDHelpTextSoundID);
        csave.WriteMicro(MICROCHUNKID_DEF_MAX_CONV_DIST, MaxConversationDist);
        csave.WriteMicro(MICROCHUNKID_DEF_MAX_COMBAT_CONV_DIST, MaxCombatConversationDist);
        csave.WriteMicro(MICROCHUNKID_DEF_SOLDIER_WALK_SPEED, SoldierWalkSpeed);
        csave.WriteMicro(MICROCHUNKID_DEF_SOLDIER_CROUCH_SPEED, SoldierCrouchSpeed);
        csave.WriteMicro(MICROCHUNKID_DEF_FALLING_DAMAGE_MIN_DISTANCE, FallingDamageMinDistance);
        csave.WriteMicro(MICROCHUNKID_DEF_FALLING_DAMAGE_MAX_DISTANCE, FallingDamageMaxDistance);
        csave.WriteMicro(MICROCHUNKID_DEF_FALLING_DAMAGE_WARHEAD, FallingDamageWarhead);
        csave.WriteMicro(MICROCHUNKID_DEF_ENCY_EVENT_STRING_ID, EncyclopediaEventStringID);

        ArgumentNullException.ThrowIfNull(PurchaseGDICharactersTexture);
        ArgumentNullException.ThrowIfNull(PurchaseGDIVehiclesTexture);
        ArgumentNullException.ThrowIfNull(PurchaseGDIEquipmentTexture);
        ArgumentNullException.ThrowIfNull(PurchaseNODCharactersTexture);
        ArgumentNullException.ThrowIfNull(PurchaseNODVehiclesTexture);
        ArgumentNullException.ThrowIfNull(PurchaseNODEquipmentTexture);
        ArgumentNullException.ThrowIfNull(PurchaseGDIMUTCharactersTexture);
        ArgumentNullException.ThrowIfNull(PurchaseGDIMUTVehiclesTexture);
        ArgumentNullException.ThrowIfNull(PurchaseGDIMUTEquipmentTexture);
        ArgumentNullException.ThrowIfNull(PurchaseNODMUTCharactersTexture);
        ArgumentNullException.ThrowIfNull(PurchaseNODMUTVehiclesTexture);
        ArgumentNullException.ThrowIfNull(PurchaseNODMUTEquipmentTexture);

        csave.WriteMicroString(MICROCHUNKID_DEF_PURCHASE_GDI_CHARS_TEXTURE, PurchaseGDICharactersTexture);
        csave.WriteMicroString(MICROCHUNKID_DEF_PURCHASE_GDI_VEHICLES_TEXTURE, PurchaseGDIVehiclesTexture);
        csave.WriteMicroString(MICROCHUNKID_DEF_PURCHASE_GDI_EQUIP_TEXTURE, PurchaseGDIEquipmentTexture);

        csave.WriteMicroString(MICROCHUNKID_DEF_PURCHASE_NOD_CHARS_TEXTURE, PurchaseNODCharactersTexture);
        csave.WriteMicroString(MICROCHUNKID_DEF_PURCHASE_NOD_VEHICLES_TEXTURE, PurchaseNODVehiclesTexture);
        csave.WriteMicroString(MICROCHUNKID_DEF_PURCHASE_NOD_EQUIP_TEXTURE, PurchaseNODEquipmentTexture);

        csave.WriteMicroString(MICROCHUNKID_DEF_PURCHASE_GDI_MUT_CHARS_TEXTURE, PurchaseGDIMUTCharactersTexture);
        csave.WriteMicroString(MICROCHUNKID_DEF_PURCHASE_GDI_MUT_VEHICLES_TEXTURE, PurchaseGDIMUTVehiclesTexture);
        csave.WriteMicroString(MICROCHUNKID_DEF_PURCHASE_GDI_MUT_EQUIP_TEXTURE, PurchaseGDIMUTEquipmentTexture);

        csave.WriteMicroString(MICROCHUNKID_DEF_PURCHASE_NOD_MUT_CHARS_TEXTURE, PurchaseNODMUTCharactersTexture);
        csave.WriteMicroString(MICROCHUNKID_DEF_PURCHASE_NOD_MUT_VEHICLES_TEXTURE, PurchaseNODMUTVehiclesTexture);
        csave.WriteMicroString(MICROCHUNKID_DEF_PURCHASE_NOD_MUT_EQUIP_TEXTURE, PurchaseNODMUTEquipmentTexture);

        csave.WriteMicro(MICROCHUNKID_DEF_STEALTH_DISTANCE_HUMAN, StealthDistanceHuman);
        csave.WriteMicro(MICROCHUNKID_DEF_STEALTH_DISTANCE_VEHICLE, StealthDistanceVehicle);
        csave.WriteMicro(MICROCHUNKID_DEF_MP_STEALTH_DISTANCE_HUMAN, MPStealthDistanceHuman);
        csave.WriteMicro(MICROCHUNKID_DEF_MP_STEALTH_DISTANCE_VEHICLE, MPStealthDistanceVehicle);
        csave.End_Chunk();

        return true;
    }

    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {
                case CHUNKID_DEF_PARENT:
                    base.Load(cload);
                    break;

                case CHUNKID_DEF_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case MICROCHUNKID_DEF_DEATH_SOUND: cload.Read(ref DeathSoundID); break;
                            case MICROCHUNKID_DEF_EVA_MO_SOUND: cload.Read(ref EVAObjectivesSoundID); break;
                            case MICROCHUNKID_DEF_HELP_TXT_SOUND: cload.Read(ref HUDHelpTextSoundID); break;
                            case MICROCHUNKID_DEF_MAX_CONV_DIST: cload.Read(ref MaxConversationDist); break;
                            case MICROCHUNKID_DEF_MAX_COMBAT_CONV_DIST: cload.Read(ref MaxCombatConversationDist); break;
                            case MICROCHUNKID_DEF_SOLDIER_WALK_SPEED: cload.Read(ref SoldierWalkSpeed); break;
                            case MICROCHUNKID_DEF_SOLDIER_CROUCH_SPEED: cload.Read(ref SoldierCrouchSpeed); break;
                            case MICROCHUNKID_DEF_FALLING_DAMAGE_MIN_DISTANCE: cload.Read(ref FallingDamageMinDistance); break;
                            case MICROCHUNKID_DEF_FALLING_DAMAGE_MAX_DISTANCE: cload.Read(ref FallingDamageMaxDistance); break;
                            case MICROCHUNKID_DEF_FALLING_DAMAGE_WARHEAD: cload.Read(ref FallingDamageWarhead); break;
                            case MICROCHUNKID_DEF_ENCY_EVENT_STRING_ID: cload.Read(ref EncyclopediaEventStringID); break;

                            case MICROCHUNKID_DEF_PURCHASE_GDI_CHARS_TEXTURE: cload.ReadMicroChunkWWString(out PurchaseGDICharactersTexture); break;
                            case MICROCHUNKID_DEF_PURCHASE_GDI_VEHICLES_TEXTURE: cload.ReadMicroChunkWWString(out PurchaseGDIVehiclesTexture); break;
                            case MICROCHUNKID_DEF_PURCHASE_GDI_EQUIP_TEXTURE: cload.ReadMicroChunkWWString(out PurchaseGDIEquipmentTexture); break;

                            case MICROCHUNKID_DEF_PURCHASE_NOD_CHARS_TEXTURE: cload.ReadMicroChunkWWString(out PurchaseNODCharactersTexture); break;
                            case MICROCHUNKID_DEF_PURCHASE_NOD_VEHICLES_TEXTURE: cload.ReadMicroChunkWWString(out PurchaseNODVehiclesTexture); break;
                            case MICROCHUNKID_DEF_PURCHASE_NOD_EQUIP_TEXTURE: cload.ReadMicroChunkWWString(out PurchaseNODEquipmentTexture); break;

                            case MICROCHUNKID_DEF_PURCHASE_GDI_MUT_CHARS_TEXTURE: cload.ReadMicroChunkWWString(out PurchaseGDIMUTCharactersTexture); break;
                            case MICROCHUNKID_DEF_PURCHASE_GDI_MUT_VEHICLES_TEXTURE: cload.ReadMicroChunkWWString(out PurchaseGDIMUTVehiclesTexture); break;
                            case MICROCHUNKID_DEF_PURCHASE_GDI_MUT_EQUIP_TEXTURE: cload.ReadMicroChunkWWString(out PurchaseGDIMUTEquipmentTexture); break;

                            case MICROCHUNKID_DEF_PURCHASE_NOD_MUT_CHARS_TEXTURE: cload.ReadMicroChunkWWString(out PurchaseNODMUTCharactersTexture); break;
                            case MICROCHUNKID_DEF_PURCHASE_NOD_MUT_VEHICLES_TEXTURE: cload.ReadMicroChunkWWString(out PurchaseNODMUTVehiclesTexture); break;
                            case MICROCHUNKID_DEF_PURCHASE_NOD_MUT_EQUIP_TEXTURE: cload.ReadMicroChunkWWString(out PurchaseNODMUTEquipmentTexture); break;

                            case MICROCHUNKID_DEF_STEALTH_DISTANCE_HUMAN: cload.Read(ref StealthDistanceHuman); break;
                            case MICROCHUNKID_DEF_STEALTH_DISTANCE_VEHICLE: cload.Read(ref StealthDistanceVehicle); break;
                            case MICROCHUNKID_DEF_MP_STEALTH_DISTANCE_HUMAN: cload.Read(ref MPStealthDistanceHuman); break;
                            case MICROCHUNKID_DEF_MP_STEALTH_DISTANCE_VEHICLE: cload.Read(ref MPStealthDistanceVehicle); break;

                            default:
                                Console.WriteLine("Unhandled GlobalSettingsDef Variable chunkID\n");
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unhandled GlobalSettingsDef chunkID\n");
                    break;
            }
            cload.Close_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    protected int DeathSoundID;
    protected int EVAObjectivesSoundID;
    protected int HUDHelpTextSoundID;
    protected float MaxConversationDist;
    protected float MaxCombatConversationDist;
    protected float SoldierWalkSpeed;
    protected float SoldierCrouchSpeed;
    protected int EncyclopediaEventStringID;
    protected float FallingDamageMinDistance;
    protected float FallingDamageMaxDistance;
    protected int FallingDamageWarhead;
    protected float StealthDistanceHuman;
    protected float StealthDistanceVehicle;
    protected float MPStealthDistanceHuman;
    protected float MPStealthDistanceVehicle;

    protected string? PurchaseGDICharactersTexture;
    protected string? PurchaseGDIVehiclesTexture;
    protected string? PurchaseGDIEquipmentTexture;
    protected string? PurchaseNODCharactersTexture;
    protected string? PurchaseNODVehiclesTexture;
    protected string? PurchaseNODEquipmentTexture;
    protected string? PurchaseGDIMUTCharactersTexture;
    protected string? PurchaseGDIMUTVehiclesTexture;
    protected string? PurchaseGDIMUTEquipmentTexture;
    protected string? PurchaseNODMUTCharactersTexture;
    protected string? PurchaseNODMUTVehiclesTexture;
    protected string? PurchaseNODMUTEquipmentTexture;


    private const uint CHUNKID_DEF_PARENT = 803001812;
    private const uint CHUNKID_DEF_VARIABLES = 803001813;

    private const byte MICROCHUNKID_DEF_XXX = 1;
    private const byte MICROCHUNKID_DEF_DEATH_SOUND = 2;
    private const byte MICROCHUNKID_DEF_EVA_MO_SOUND = 3;
    private const byte MICROCHUNKID_DEF_MAX_CONV_DIST = 4;
    private const byte MICROCHUNKID_DEF_MAX_COMBAT_CONV_DIST = 5;
    private const byte MICROCHUNKID_DEF_SOLDIER_WALK_SPEED = 6;
    private const byte MICROCHUNKID_DEF_SOLDIER_CROUCH_SPEED = 7;
    private const byte MICROCHUNKID_DEF_FALLING_DAMAGE_MIN_DISTANCE = 8;
    private const byte MICROCHUNKID_DEF_FALLING_DAMAGE_MAX_DISTANCE = 9;
    private const byte MICROCHUNKID_DEF_FALLING_DAMAGE_WARHEAD = 10;

    private const byte MICROCHUNKID_DEF_PURCHASE_GDI_CHARS_TEXTURE = 11;
    private const byte MICROCHUNKID_DEF_PURCHASE_GDI_VEHICLES_TEXTURE = 12;
    private const byte MICROCHUNKID_DEF_PURCHASE_GDI_EQUIP_TEXTURE = 13;
    private const byte XXXMICROCHUNKID_DEF_PURCHASE_ADV_EQUIP_TEXTURE = 14;

    private const byte MICROCHUNKID_DEF_PURCHASE_NOD_CHARS_TEXTURE = 15;
    private const byte MICROCHUNKID_DEF_PURCHASE_NOD_VEHICLES_TEXTURE = 16;
    private const byte MICROCHUNKID_DEF_PURCHASE_NOD_EQUIP_TEXTURE = 17;

    private const byte MICROCHUNKID_DEF_PURCHASE_GDI_MUT_CHARS_TEXTURE = 18;
    private const byte MICROCHUNKID_DEF_PURCHASE_GDI_MUT_VEHICLES_TEXTURE = 19;
    private const byte MICROCHUNKID_DEF_PURCHASE_GDI_MUT_EQUIP_TEXTURE = 20;

    private const byte MICROCHUNKID_DEF_PURCHASE_NOD_MUT_CHARS_TEXTURE = 21;
    private const byte MICROCHUNKID_DEF_PURCHASE_NOD_MUT_VEHICLES_TEXTURE = 22;
    private const byte MICROCHUNKID_DEF_PURCHASE_NOD_MUT_EQUIP_TEXTURE = 23;

    private const byte MICROCHUNKID_DEF_ENCY_EVENT_STRING_ID = 24;
    private const byte MICROCHUNKID_DEF_HELP_TXT_SOUND = 25;

    private const byte MICROCHUNKID_DEF_STEALTH_DISTANCE_HUMAN = 26;
    private const byte MICROCHUNKID_DEF_STEALTH_DISTANCE_VEHICLE = 27;
    private const byte MICROCHUNKID_DEF_MP_STEALTH_DISTANCE_HUMAN = 28;
    private const byte MICROCHUNKID_DEF_MP_STEALTH_DISTANCE_VEHICLE = 29;
}
