using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;
using RenData.Types;
using System.Numerics;

namespace RenData.Definitions;


///*
//** Weapon Type Names
//*/
//char* WeaponStyleNames[NUM_WEAPON_HOLD_STYLES] = {
//    "C4",					// WEAPON_HOLD_STYLE_C4					= 0,
//	"---",				// WEAPON_HOLD_STYLE_NOT_USED,
//	"Shoulder",			// WEAPON_HOLD_STYLE_AT_SHOULDER,	// 2
//	"Hip",				// WEAPON_HOLD_STYLE_AT_HIP,
//	"Launcher",			// WEAPON_HOLD_STYLE_LAUNCHER,
//	"Handgun",			// WEAPON_HOLD_STYLE_HANDGUN,
//	"Beacon",			// WEAPON_HOLD_STYLE_BEACON
//	"Empty Hands",		// WEAPON_HOLD_STYLE_EMPTY_HANDS,
//	"At Chest",			// WEAPON_HOLD_STYLE_AT_CHEST,
//	"Hands Down",		// WEAPON_HOLD_STYLE_HANDS_DOWN,
//};

///*
//** Weapon Definition Structure - These describe the basic functionality of
//** a weapon, and CANNOT be modified by instances
//*/
[RegisterDefinition(ChunkId.CHUNKID_WEAPON_DEF)]
public partial class WeaponDefinitionClass : DefinitionClass
{
    public WeaponDefinitionClass() { }

    //DECLARE_DEFINITION_FACTORY(WeaponDefinitionClass, CLASSID_DEF_WEAPON, "Weapon") _WeaponDefDefFactory;

    //const PersistFactoryClass & WeaponDefinitionClass::Get_Factory (void) const { return _WeaponDefPersistFactory; }

    public override uint Get_Class_ID() => ClassId.CLASSID_DEF_WEAPON;
    public override PersistClass Create()
    {
        //WWASSERT( 0 ); 
        //return NULL;
        throw new NotImplementedException();
    }
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_WEAPON_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_WEAPON_DEF_VARIABLES);
        csave.WriteMicro(MICROCHUNKID_WEAPON_DEF_STYLE, Style);
        ArgumentNullException.ThrowIfNull(Model);
        csave.WriteMicroString(MICROCHUNKID_WEAPON_DEF_MODEL, Model);
        ArgumentNullException.ThrowIfNull(IdleAnim);
        csave.WriteMicroString(MICROCHUNKID_WEAPON_DEF_IDLE_ANIM, IdleAnim);
        ArgumentNullException.ThrowIfNull(FireAnim);
        csave.WriteMicroString(MICROCHUNKID_WEAPON_DEF_FIRE_ANIM, FireAnim);
        ArgumentNullException.ThrowIfNull(BackModel);
        csave.WriteMicroString(MICROCHUNKID_WEAPON_DEF_BACK_MODEL, BackModel);
        csave.WriteMicro(MICROCHUNKID_WEAPON_DEF_SWITCH_TIME, SwitchTime);
        csave.WriteMicro(MICROCHUNKID_WEAPON_DEF_RELOAD_TIME, ReloadTime);
        csave.WriteMicro(MICROCHUNKID_WEAPON_DEF_KEY_NUMBER, KeyNumber);
        csave.WriteMicro(MICROCHUNKID_WEAPON_DEF_CAN_SNIPE, CanSnipe);
        csave.WriteMicro(MICROCHUNKID_WEAPON_DEF_GENERIC_AMMO_OK, CanReceiveGenericCnCAmmo);
        csave.WriteMicro(MICROCHUNKID_WEAPON_DEF_EJECT_PHYS_DEF_ID, EjectPhysDefID);
        csave.WriteMicro(MICROCHUNKID_WEAPON_DEF_MUZZLE_FLASH_PHYS_DEF_ID, MuzzleFlashPhysDefID);
        csave.WriteMicro(MICROCHUNKID_WEAPON_DEF_RATING, Rating);
        ArgumentNullException.ThrowIfNull(FirstPersonModel);
        csave.WriteMicroString(MICROCHUNKID_WEAPON_DEF_FIRST_PERSON_MODEL, FirstPersonModel);
        csave.WriteMicro(MICROCHUNKID_WEAPON_DEF_FIRST_PERSON_OFFSET, FirstPersonOffset);
        csave.WriteMicro(MICROCHUNKID_WEAPON_DEF_RECOIL_IMPULSE, RecoilImpulse);
        ArgumentNullException.ThrowIfNull(HUDIconTextureName);
        csave.WriteMicroString(MICROCHUNKID_WEAPON_DEF_HUD_ICON_TEXTURE_NAME, HUDIconTextureName);
        csave.WriteMicro(MICROCHUNKID_WEAPON_DEF_RELOAD_SOUND_DEFID, ReloadSoundDefID);
        csave.WriteMicro(MICROCHUNKID_WEAPON_DEF_EMPTY_SOUND_DEFID, EmptySoundDefID);
        csave.WriteMicro(MICROCHUNKID_WEAPON_DEF_PRIMARY_AMMO_DEF_ID, PrimaryAmmoDefID);
        csave.WriteMicro(MICROCHUNKID_WEAPON_DEF_SECONDARY_AMMO_DEF_ID, SecondaryAmmoDefID);
        csave.WriteMicro(MICROCHUNKID_WEAPON_DEF_CLIP_SIZE, ClipSize);
        csave.WriteMicro(MICROCHUNKID_WEAPON_DEF_RECOIL_TIME, RecoilTime);
        csave.WriteMicro(MICROCHUNKID_WEAPON_DEF_RECOIL_SCALE, RecoilScale);
        csave.WriteMicro(MICROCHUNKID_WEAPON_DEF_A_GIVE_WEAPONS_WEAPON, AGiveWeaponsWeapon);
        csave.WriteMicro(MICROCHUNKID_WEAPON_DEF_MAX_INVENTORY_ROUNDS, MaxInventoryRounds);
        csave.WriteMicro(MICROCHUNKID_WEAPON_ICON_NAME_ID, IconNameID);
        ArgumentNullException.ThrowIfNull(IconTextureName);
        csave.WriteMicroString(MICROCHUNKID_WEAPON_ICON_TEXTURE_NAME, IconTextureName);
        csave.WriteMicro(MICROCHUNKID_WEAPON_ICON_TEXTURE_UV, (RectClassStruct)IconTextureUV);
        csave.WriteMicro(MICROCHUNKID_WEAPON_ICON_OFFSET, IconOffset);
        ArgumentNullException.ThrowIfNull(HumanFiringAnimation);
        csave.WriteMicroString(MICROCHUNKID_HUMAN_FIRING_ANIMATION, HumanFiringAnimation);
        csave.End_Chunk();
        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {

                case CHUNKID_WEAPON_DEF_PARENT:
                    base.Load(cload);
                    break;

                case CHUNKID_WEAPON_DEF_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case MICROCHUNKID_WEAPON_DEF_STYLE:
                                cload.Read(ref Style);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_MODEL:
                                cload.ReadMicroChunkWWString(out Model);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_IDLE_ANIM:
                                cload.ReadMicroChunkWWString(out IdleAnim);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_FIRE_ANIM:
                                cload.ReadMicroChunkWWString(out FireAnim);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_BACK_MODEL:
                                cload.ReadMicroChunkWWString(out BackModel);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_SWITCH_TIME:
                                cload.Read(ref SwitchTime);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_RELOAD_TIME:
                                cload.Read(ref ReloadTime);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_KEY_NUMBER:
                                cload.Read(ref KeyNumber);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_CAN_SNIPE:
                                cload.Read(ref CanSnipe);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_GENERIC_AMMO_OK:
                                cload.Read(ref CanReceiveGenericCnCAmmo);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_EJECT_PHYS_DEF_ID:
                                cload.Read(ref EjectPhysDefID);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_MUZZLE_FLASH_PHYS_DEF_ID:
                                cload.Read(ref MuzzleFlashPhysDefID);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_RATING:
                                cload.Read(ref Rating);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_FIRST_PERSON_MODEL:
                                cload.ReadMicroChunkWWString(out FirstPersonModel);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_FIRST_PERSON_OFFSET:
                                cload.Read(ref FirstPersonOffset);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_RECOIL_IMPULSE:
                                cload.Read(ref RecoilImpulse);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_HUD_ICON_TEXTURE_NAME:
                                cload.ReadMicroChunkWWString(out HUDIconTextureName);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_RELOAD_SOUND_DEFID:
                                cload.Read(ref ReloadSoundDefID);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_EMPTY_SOUND_DEFID:
                                cload.Read(ref EmptySoundDefID);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_PRIMARY_AMMO_DEF_ID:
                                cload.Read(ref PrimaryAmmoDefID);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_SECONDARY_AMMO_DEF_ID:
                                cload.Read(ref SecondaryAmmoDefID);
                                break;
                                case MICROCHUNKID_WEAPON_DEF_CLIP_SIZE:
                                cload.Read(ref ClipSize);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_RECOIL_TIME:
                                cload.Read(ref RecoilTime);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_RECOIL_SCALE:
                                cload.Read(ref RecoilScale);
                                break;
                            case MICROCHUNKID_WEAPON_DEF_A_GIVE_WEAPONS_WEAPON:
                                cload.Read(ref AGiveWeaponsWeapon);
                                break;
                                case MICROCHUNKID_WEAPON_DEF_MAX_INVENTORY_ROUNDS:
                                cload.Read(ref MaxInventoryRounds);
                                break;
                            case MICROCHUNKID_WEAPON_ICON_NAME_ID:
                                cload.Read(ref IconNameID);
                                break;
                            case MICROCHUNKID_WEAPON_ICON_TEXTURE_NAME:
                                cload.ReadMicroChunkWWString(out IconTextureName);
                                break;
                            case MICROCHUNKID_WEAPON_ICON_TEXTURE_UV:
                                RectClassStruct rcStruct = new RectClassStruct();
                                cload.Read(ref rcStruct);
                                IconTextureUV.Set(rcStruct);
                                break;
                            case MICROCHUNKID_WEAPON_ICON_OFFSET:
                                cload.Read(ref IconOffset);
                                break;
                            case MICROCHUNKID_HUMAN_FIRING_ANIMATION:
                                cload.ReadMicroChunkWWString(out HumanFiringAnimation);
                                break;

                            default:
                                Console.WriteLine("Unrecognized WeaponDef Variable chunkID\n");
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unrecognized WeaponDef chunkID\n");
                    break;

            }
            cload.Close_Chunk();
        }

        return true;
    }
    public override PersistFactoryClass Get_Factory() => _persistFactory;

    //DECLARE_EDITABLE(WeaponDefinitionClass, DefinitionClass);

    //bool operator ==( WeaponDefinitionClass & vector)  { return false; }
    //bool operator !=( WeaponDefinitionClass & vector)  { return true; }

    public int Style;
    public string? Model;
    public string? IdleAnim;
    public string? FireAnim;
    public string? BackModel;
    //	int				LegacyFireSoundDefID;
    public float SwitchTime;
    public float ReloadTime;
    public float KeyNumber;
    public bool CanSnipe;
    public bool CanReceiveGenericCnCAmmo;
    public float Rating;
    public int EjectPhysDefID;
    public int MuzzleFlashPhysDefID;
    public string? FirstPersonModel;
    public Vector3 FirstPersonOffset;
    public float RecoilImpulse;
    public string? HUDIconTextureName;
    public int ReloadSoundDefID;
    public int EmptySoundDefID;
    public int PrimaryAmmoDefID;
    public int SecondaryAmmoDefID;
    public int ClipSize;
    public float RecoilTime;
    public float RecoilScale;
    public bool AGiveWeaponsWeapon;
    public int MaxInventoryRounds;

    public int IconNameID;
    public string? IconTextureName;
    public RectClass IconTextureUV = new();
    public Vector2 IconOffset;

    public string? HumanFiringAnimation;

    private const uint CHUNKID_WEAPON_DEF_VARIABLES = 1205091654;
    private const uint CHUNKID_WEAPON_DEF_PARENT = 1205091655;

    private const uint MICROCHUNKID_WEAPON_DEF_STYLE = 1;
    private const uint MICROCHUNKID_WEAPON_DEF_MODEL = 2;
    private const uint MICROCHUNKID_WEAPON_DEF_IDLE_ANIM = 3;
    private const uint MICROCHUNKID_WEAPON_DEF_FIRE_ANIM = 4;
    private const uint MICROCHUNKID_WEAPON_DEF_BACK_MODEL = 5;
    private const uint MICROCHUNKID_WEAPON_DEF_XXXXX = 6;
    private const uint XXXMICROCHUNKID_WEAPON_DEF_RATE_OF_FIRE = 7;
    private const uint MICROCHUNKID_WEAPON_DEF_SWITCH_TIME = 8;
    private const uint MICROCHUNKID_WEAPON_DEF_RELOAD_TIME = 9;
    private const uint XXXMICROCHUNKID_WEAPON_DEF_HELP_ANGLE = 10;
    private const uint XXXMICROCHUNKID_WEAPON_DEF_MUZZLE_FLASH_MODEL = 11;
    private const uint XXXMICROCHUNKID_WEAPON_DEF_MUZZLE_FLASH_DURATION = 12;
    private const uint XXXMICROCHUNKID_WEAPON_DEF_EJECT_EMITTER = 13;
    private const uint XXXMICROCHUNKID_WEAPON_DEF_TILT = 14;
    private const uint XXXMICROCHUNKID_WEAPON_DEF_KEY_NUMBER = 15;
    private const uint MICROCHUNKID_WEAPON_DEF_CAN_SNIPE = 16;
    private const uint XXXMICROCHUNKID_WEAPON_DEF_SOUND_RADIUS = 17;
    private const uint LEGACY_MICROCHUNKID_WEAPON_DEF_FIRE_SOUND_DEFID = 18;
    private const uint MICROCHUNKID_WEAPON_DEF_EJECT_PHYS_DEF_ID = 19;
    private const uint MICROCHUNKID_WEAPON_DEF_MUZZLE_FLASH_PHYS_DEF_ID = 20;
    private const uint MICROCHUNKID_WEAPON_DEF_RATING = 21;
    private const uint MICROCHUNKID_WEAPON_DEF_FIRST_PERSON_MODEL = 22;
    private const uint MICROCHUNKID_WEAPON_DEF_FIRST_PERSON_OFFSET = 23;
    private const uint MICROCHUNKID_WEAPON_DEF_RECOIL_IMPULSE = 24;
    private const uint XXX_MICROCHUNKID_WEAPON_DEF_HUD_MODEL = 25;
    private const uint MICROCHUNKID_WEAPON_DEF_RELOAD_SOUND_DEFID = 26;
    private const uint MICROCHUNKID_WEAPON_DEF_PRIMARY_AMMO_DEF_ID = 27;
    private const uint MICROCHUNKID_WEAPON_DEF_SECONDARY_AMMO_DEF_ID = 28;
    private const uint MICROCHUNKID_WEAPON_DEF_CLIP_SIZE = 29;
    private const uint MICROCHUNKID_WEAPON_DEF_RECOIL_TIME = 30;
    private const uint MICROCHUNKID_WEAPON_DEF_RECOIL_SCALE = 31;
    private const uint MICROCHUNKID_WEAPON_DEF_A_GIVE_WEAPONS_WEAPON = 32;
    private const uint XXXMICROCHUNKID_WEAPON_DEF_FIRST_PERSON_TYPE = 33;
    private const uint MICROCHUNKID_WEAPON_DEF_HUD_ICON_TEXTURE_NAME = 34;
    private const uint MICROCHUNKID_WEAPON_DEF_MAX_INVENTORY_ROUNDS = 35;
    private const uint XXXMICROCHUNKID_WEAPON_DEF_FIRST_PERSON_TYPE_NAME = 36;
    private const uint MICROCHUNKID_WEAPON_DEF_KEY_NUMBER = 37;
    private const uint MICROCHUNKID_WEAPON_ICON_NAME_ID = 38;
    private const uint MICROCHUNKID_WEAPON_ICON_TEXTURE_NAME = 39;
    private const uint MICROCHUNKID_WEAPON_ICON_TEXTURE_UV = 40;
    private const uint MICROCHUNKID_WEAPON_ICON_OFFSET = 41;
    private const uint MICROCHUNKID_HUMAN_FIRING_ANIMATION = 42;
    private const uint MICROCHUNKID_WEAPON_DEF_EMPTY_SOUND_DEFID = 43;
    private const uint MICROCHUNKID_WEAPON_DEF_GENERIC_AMMO_OK = 44;
};

///*
//** WeaponDefinitionClass
//*/


//WeaponDefinitionClass::WeaponDefinitionClass(void) :
//	Style(1),
//	SwitchTime(0),
//	ReloadTime(0),
//	KeyNumber(0),
//	CanSnipe(false),
//	CanReceiveGenericCnCAmmo(true),
////	LegacyFireSoundDefID( 0 ),
//	EjectPhysDefID(0),
//	MuzzleFlashPhysDefID(0),
//	Rating(0.1f),
//	FirstPersonOffset(0, 0, 0),
//	RecoilImpulse(0.0f),
//	ReloadSoundDefID(0),
//	EmptySoundDefID(0),
//	PrimaryAmmoDefID(0),
//	SecondaryAmmoDefID(0),
//	ClipSize(0),
//	RecoilTime(0.1f),
//	RecoilScale(1.0f),
//	AGiveWeaponsWeapon(false),
//	MaxInventoryRounds(100),
//	IconNameID(0),
//	IconTextureUV(0, 0, 0, 0),
//	IconOffset(0, 0)
//{
//# ifdef PARAM_EDITING_ON
//    int i;
//    //	EDITABLE_PARAM( WeaponDefinitionClass, ParameterClass::TYPE_INT,			Style);
//    EnumParameterClass* param;
//    param = new EnumParameterClass(&Style);
//    param->Set_Name("Style");
//    for (i = 0; i <= WEAPON_HOLD_STYLE_HANDGUN; i++)
//    {
//        param->Add_Value(WeaponStyleNames[i], i);
//    }
//    param->Add_Value(WeaponStyleNames[WEAPON_HOLD_STYLE_BEACON], WEAPON_HOLD_STYLE_BEACON);
//    GENERIC_EDITABLE_PARAM(WeaponDefinitionClass, param)


//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_FILENAME, Model);
//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_FILENAME, IdleAnim);
//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_FILENAME, FireAnim);
//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_FILENAME, BackModel);
//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_FLOAT, SwitchTime);
//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_FLOAT, ReloadTime);
//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_FLOAT, KeyNumber);

//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_BOOL, CanSnipe);
//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_BOOL, CanReceiveGenericCnCAmmo);
//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_FLOAT, Rating);

//    PHYS_DEF_PARAM(WeaponDefinitionClass, EjectPhysDefID, "ProjectileDef");
//    PHYS_DEF_PARAM(WeaponDefinitionClass, MuzzleFlashPhysDefID, "TimedDecorationPhysDef");

//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_FILENAME, FirstPersonModel);
//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_VECTOR3, FirstPersonOffset);
//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_FLOAT, RecoilImpulse);

//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_FILENAME, HUDIconTextureName);
//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_SOUNDDEFINITIONID, ReloadSoundDefID);
//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_SOUNDDEFINITIONID, EmptySoundDefID);

//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_AMMOOBJDEFINITIONID, PrimaryAmmoDefID);
//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_AMMOOBJDEFINITIONID, SecondaryAmmoDefID);

//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_INT, ClipSize);
//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_INT, MaxInventoryRounds);

//    FLOAT_UNITS_PARAM(WeaponDefinitionClass, RecoilTime, 0.0f, 10.0f, "seconds");
//    FLOAT_EDITABLE_PARAM(WeaponDefinitionClass, RecoilScale, 0.0f, 10.0f);

//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_BOOL, AGiveWeaponsWeapon);

//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_STRINGSDB_ID, IconNameID);
//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_FILENAME, IconTextureName);
//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_RECT, IconTextureUV);
//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_VECTOR2, IconOffset);

//    EDITABLE_PARAM(WeaponDefinitionClass, ParameterClass::TYPE_STRING, HumanFiringAnimation);

//#endif	//PARAM_EDITING_ON
//}
