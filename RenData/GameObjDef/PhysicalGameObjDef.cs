using RenData.Types;
using RenData.ChunkIO;

namespace RenData.GameObjDef;

public abstract class PhysicalGameObjDef : DamageableGameObjDef
{
    public PhysicalGameObjDef()
    {
        Type = 0;
        BullseyeOffsetZ = 0.0f;
        RadarBlipType = 0;
        PhysDefID = 0;
        KilledExplosion = 0;
        OratorType = (int)OratorTypes.ORATOR_TYPE_START - 1;
        DefaultHibernationEnable = true;
        AllowInnateConversations = false;
        UseCreationEffect = false;

        #region param editing
        //# ifdef	PARAM_EDITING_ON
        //    int i;
        //    EnumParameterClass* param;

        //    EDITABLE_PARAM(PhysicalGameObjDef, ParameterClass::TYPE_FLOAT, BullseyeOffsetZ);

        //    param = new EnumParameterClass(&RadarBlipType);
        //    param->Set_Name("Radar Blip Type");
        //    for (i = 0; i < RadarManager::Get_Num_Blip_Shape_Types(); i++)
        //    {
        //        param->Add_Value(RadarManager::Get_Blip_Shape_Type_Name(i), i);
        //    }
        //    GENERIC_EDITABLE_PARAM(PhysicalGameObjDef, param)


        //    EDITABLE_PARAM(PhysicalGameObjDef, ParameterClass::TYPE_STRING, Animation);

        //    EDITABLE_PARAM(PhysicalGameObjDef, ParameterClass::TYPE_EXPLOSIONDEFINITIONID, KilledExplosion);

        //    EDITABLE_PARAM(PhysicalGameObjDef, ParameterClass::TYPE_BOOL, DefaultHibernationEnable);

        //    EDITABLE_PARAM(PhysicalGameObjDef, ParameterClass::TYPE_BOOL, AllowInnateConversations);

        //    EDITABLE_PARAM(PhysicalGameObjDef, ParameterClass::TYPE_BOOL, UseCreationEffect);

        //    //
        //    //	Configure the orator types parameter
        //    //
        //    EnumParameterClass* orator_type_param = new EnumParameterClass(&OratorType);
        //    orator_type_param->Set_Name("Orator Type");

        //    //
        //    //	Add all the orator types to the list
        //    //
        //    int count = OratorTypeClass::Get_Count();
        //    for (int index = 0; index < count; index++)
        //    {
        //        orator_type_param->Add_Value(OratorTypeClass::Get_Description(index),
        //                                    OratorTypeClass::Get_ID(index));
        //    }

        //    GENERIC_EDITABLE_PARAM(PhysicalGameObjDef, orator_type_param);

        //#endif
        //}
        #endregion
    }

    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        csave.WriteMicro(MICROCHUNKID_DEF_TYPE, Type);
        csave.WriteMicro(MICROCHUNKID_DEF_BULLSEYE_OFFSET_Z, BullseyeOffsetZ);
        csave.WriteMicro(MICROCHUNKID_DEF_BLIP_TYPE, RadarBlipType);
        //WRITE_MICRO_CHUNK_WWSTRING(csave, MICROCHUNKID_DEF_ANIMATION, Animation);
        csave.WriteMicroString(MICROCHUNKID_DEF_ANIMATION, Animation);
        csave.WriteMicro(MICROCHUNKID_DEF_PHYS_ID, PhysDefID);
        csave.WriteMicro(MICROCHUNKID_DEF_KILLED_EXPLOSION, KilledExplosion);
        csave.WriteMicro(MICROCHUNKID_DEF_DEFAULT_HIBERNATION_ENABLE, DefaultHibernationEnable);
        csave.WriteMicro(MICROCHUNKID_DEF_ALLOW_INNATE_CONVERSATIONS, AllowInnateConversations);
        csave.WriteMicro(MICROCHUNKID_DEF_ORATOR_TYPE, OratorType);
        csave.WriteMicro(MICROCHUNKID_DEF_USE_CREATION_EFFECT, UseCreationEffect);

        csave.End_Chunk();
        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {

                case LEGACY_CHUNKID_DEF_PARENT_OLD:
                    ScriptableGameObjDefLoad(cload);
                    break;

                case CHUNKID_DEF_PARENT:
                    base.Load(cload);
                    break;

                case CHUNKID_DEF_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case MICROCHUNKID_DEF_TYPE:
                                cload.Read(ref Type);
                                break;
                            case MICROCHUNKID_DEF_BULLSEYE_OFFSET_Z:
                                cload.Read(ref BullseyeOffsetZ);
                                break;
                            case MICROCHUNKID_DEF_BLIP_TYPE:
                                cload.Read(ref RadarBlipType);
                                break;
                            case MICROCHUNKID_DEF_ANIMATION:
                                Animation = cload.ReadMicroChunkWWString();
                                break;
                            case MICROCHUNKID_DEF_PHYS_ID:
                                cload.Read(ref PhysDefID);
                                break;
                            case LEGACY_MICROCHUNKID_DEF_DEFAULT_PLAYER_TYPE:
                                cload.Read(ref DefaultPlayerType);
                                break;
                            case MICROCHUNKID_DEF_KILLED_EXPLOSION:
                                cload.Read(ref KilledExplosion);
                                break;
                            case LEGACY_MICROCHUNKID_DEF_TRANSLATED_NAME_ID:
                                cload.Read(ref TranslatedNameID);
                                break;
                            case MICROCHUNKID_DEF_DEFAULT_HIBERNATION_ENABLE:
                                cload.Read(ref DefaultHibernationEnable);
                                break;
                            case MICROCHUNKID_DEF_ALLOW_INNATE_CONVERSATIONS:
                                cload.Read(ref AllowInnateConversations);
                                break;
                            case MICROCHUNKID_DEF_ORATOR_TYPE:
                                cload.Read(ref OratorType);
                                break;
                            case MICROCHUNKID_DEF_USE_CREATION_EFFECT:
                                cload.Read(ref UseCreationEffect);
                                break;
                            default:
                                Console.WriteLine($"Unrecognized PhysicalDef Variable chunkID {cload.Cur_Micro_Chunk_ID}");
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;


                case LEGACY_CHUNKID_DEF_DEFENSEOBJECTDEF:
                    DefenseObjectDef.Load(cload);
                    break;

                default:
                    Console.Write($"Unrecognized PhysicalGameObjDef chunkID {cload.Cur_Chunk_ID}");
                    break;

            }
            cload.Close_Chunk();
        }

        return true;
    }
    //public override bool Is_Valid_Config(string message)
    //{
    //    throw new NotImplementedException();
    //}

    public int Get_Phys_Def_ID() { return PhysDefID; }
    public int Get_Orator_Type() { return OratorType; }

    //	DECLARE_EDITABLE(PhysicalGameObjDef, DamageableGameObjDef);

    protected int Type;
    protected int RadarBlipType;
    protected float BullseyeOffsetZ;
    protected string Animation = string.Empty;
    protected int PhysDefID;
    protected int KilledExplosion;
    protected bool DefaultHibernationEnable;
    protected bool AllowInnateConversations;
    protected int OratorType;
    protected bool UseCreationEffect;

    private const int CHUNKID_DEF_VARIABLES = 909991657;
    private const int LEGACY_CHUNKID_DEF_PARENT_OLD = 909991658;
    private const int XXXCHUNKID_DEF_PARENT_OLD_OLD = 909991659;
    private const int LEGACY_CHUNKID_DEF_DEFENSEOBJECTDEF = 909991660;
    private const int CHUNKID_DEF_PARENT = 909991661;

    private const int MICROCHUNKID_DEF_TYPE = 1;
    private const int MICROCHUNKID_DEF_BULLSEYE_OFFSET_Z = 2;
    private const int XXXMICROCHUNKID_DEF_DEFAULT_GANG = 3;
    private const int MICROCHUNKID_DEF_BLIP_TYPE = 4;
    private const int XXXMICROCHUNKID_DEF_MODEL_NAME = 5;
    private const int XXXMICROCHUNKID_DEF_HEALTH = 6;
    private const int XXXMICROCHUNKID_DEF_HEALTH_MAX = 7;
    private const int XXXMICROCHUNKID_DEF_SKIN = 8;
    private const int XXXMICROCHUNKID_DEF_SHIELD_STRENGTH = 9;
    private const int XXXMICROCHUNKID_DEF_SHIELD_STRENGTH_MAX = 10;
    private const int XXXMICROCHUNKID_DEF_SHIELD_TYPE = 11;
    private const int XXXMICROCHUNKID_DEF_LISTEN_RANGE = 12;
    private const int XXX_MICROCHUNKID_DEF_SCRIPT_NAME = 13;
    private const int XXXMICROCHUNKID_DEF_SCRIPT_PARAMETERS = 14;
    private const int XXXMICROCHUNKID_DEF_POSITION = 15;   // ???
    private const int MICROCHUNKID_DEF_FACING = 16;
    private const int MICROCHUNKID_DEF_ANIMATION = 17;
    private const int MICROCHUNKID_DEF_PHYS_ID = 18;
    private const int LEGACY_MICROCHUNKID_DEF_DEFAULT_PLAYER_TYPE = 19;
    private const int MICROCHUNKID_DEF_KILLED_EXPLOSION = 20;
    private const int LEGACY_MICROCHUNKID_DEF_TRANSLATED_NAME_ID = 21;
    private const int MICROCHUNKID_DEF_DEFAULT_HIBERNATION_ENABLE = 22;
    private const int MICROCHUNKID_DEF_ALLOW_INNATE_CONVERSATIONS = 23;
    private const int MICROCHUNKID_DEF_ORATOR_TYPE = 24;
    private const int MICROCHUNKID_DEF_USE_CREATION_EFFECT = 25;
}

//bool PhysicalGameObjDef::Is_Valid_Config(StringClass &message)
//{
//    bool retval = false;

//    DefinitionClass* phys_def = DefinitionMgrClass::Find_Definition(PhysDefID);
//    if (phys_def != NULL)
//    {
//        retval = phys_def->Is_Valid_Config(message);
//    }
//    else
//    {
//        message += "Can't find physics object definition.\n";
//    }

//    return retval;
//}
