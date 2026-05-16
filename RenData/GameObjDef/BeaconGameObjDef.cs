using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.GameObjDef;

[RegisterDefinition(ChunkId.CHUNKID_GAME_OBJECT_DEF_BEACON)]
public partial class BeaconGameObjDef : SimpleGameObjDef
{
    public BeaconGameObjDef()
    {
        BroadcastToAllTime = 5.0F;
        ArmTime = 10.0F;
        DisarmTime = 10.0F;
        PreDetonateCinematicDelay = 0;
        DetonateTime = 30.0F;
        PostDetonateTime = 10.0F;
        ArmedSoundDefID = 0;
        DisarmingTextID = 0;
        DisarmedTextID = 0;
        ArmingTextID = 0;
        ArmingInterruptedTextID = 0;
        DisarmingInterruptedTextID = 0;
        PreDetonateCinematicDefID = 0;
        PostDetonateCinematicDefID = 0;
        ExplosionDefID = 0;
        IsNuke = 1;

        //            //
        //            //	Editable support
        //            //
        //            EDITABLE_PARAM(BeaconGameObjDef, ParameterClass::TYPE_STRING, ArmingAnimationName);
        //            EDITABLE_PARAM(BeaconGameObjDef, ParameterClass::TYPE_FLOAT, BroadcastToAllTime);
        //            EDITABLE_PARAM(BeaconGameObjDef, ParameterClass::TYPE_FLOAT, ArmTime);
        //            EDITABLE_PARAM(BeaconGameObjDef, ParameterClass::TYPE_FLOAT, DisarmTime);
        //            EDITABLE_PARAM(BeaconGameObjDef, ParameterClass::TYPE_FLOAT, PreDetonateCinematicDelay);
        //            EDITABLE_PARAM(BeaconGameObjDef, ParameterClass::TYPE_FLOAT, DetonateTime);
        //            EDITABLE_PARAM(BeaconGameObjDef, ParameterClass::TYPE_FLOAT, PostDetonateTime);
        //            EDITABLE_PARAM(BeaconGameObjDef, ParameterClass::TYPE_SOUNDDEFINITIONID, ArmedSoundDefID);
        //            EDITABLE_PARAM(BeaconGameObjDef, ParameterClass::TYPE_STRINGSDB_ID, ArmingTextID);
        //            EDITABLE_PARAM(BeaconGameObjDef, ParameterClass::TYPE_STRINGSDB_ID, ArmingInterruptedTextID);
        //            EDITABLE_PARAM(BeaconGameObjDef, ParameterClass::TYPE_STRINGSDB_ID, DisarmingTextID);
        //            EDITABLE_PARAM(BeaconGameObjDef, ParameterClass::TYPE_STRINGSDB_ID, DisarmingInterruptedTextID);
        //            EDITABLE_PARAM(BeaconGameObjDef, ParameterClass::TYPE_STRINGSDB_ID, DisarmedTextID);
        //            EDITABLE_PARAM(BeaconGameObjDef, ParameterClass::TYPE_BOOL, IsNuke);

        //# ifdef PARAM_EDITING_ON
        //            GenericDefParameterClass* param = new GenericDefParameterClass(&PreDetonateCinematicDefID);
        //            param->Set_Class_ID(CLASSID_GAME_OBJECT_DEF_CINEMATIC);
        //            param->Set_Name("Pre-Detonate Cinematic Obj");
        //            GENERIC_EDITABLE_PARAM(BeaconGameObjDef, param)


        //        param = new GenericDefParameterClass(&PostDetonateCinematicDefID);
        //            param->Set_Class_ID(CLASSID_GAME_OBJECT_DEF_CINEMATIC);
        //            param->Set_Name("Post-Detonate Cinematic Obj");
        //            GENERIC_EDITABLE_PARAM(BeaconGameObjDef, param)


        //        param = new GenericDefParameterClass(&ExplosionDefID);
        //            param->Set_Class_ID(CLASSID_DEF_EXPLOSION);
        //            param->Set_Name("Explosion Obj");
        //            GENERIC_EDITABLE_PARAM(BeaconGameObjDef, param)

        //    #endif //PARAM_EDITING_ON

    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GAME_OBJECT_DEF_BEACON;
    public override PersistClass Create()
    {
        //BeaconGameObj* beacon = new BeaconGameObj;
        //beacon->Init(*this);

        //return beacon;
        throw new NotImplementedException();
    }
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);

        ArgumentNullException.ThrowIfNull(ArmingAnimationName);
        csave.WriteMicroString(MICROCHUNKID_DEF_ARMING_ANIM_NAME, ArmingAnimationName);
        csave.WriteMicro(MICROCHUNKID_DEF_BROADCAST_TIME, BroadcastToAllTime);
        csave.WriteMicro(MICROCHUNKID_DEF_ARM_TIME, ArmTime);
        csave.WriteMicro(MICROCHUNKID_DEF_DISARM_TIME, DisarmTime);
        csave.WriteMicro(MICROCHUNKID_DEF_PRE_DETONATE_CINEMATIC_DELAY, PreDetonateCinematicDelay);
        csave.WriteMicro(MICROCHUNKID_DEF_DETONATE_TIME, DetonateTime);
        csave.WriteMicro(MICROCHUNKID_DEF_POST_DETONATE_TIME, PostDetonateTime);
        csave.WriteMicro(MICROCHUNKID_DEF_ARMED_SOUNDID, ArmedSoundDefID);
        csave.WriteMicro(MICROCHUNKID_DEF_DISARMING_TEXTID, DisarmingTextID);
        csave.WriteMicro(MICROCHUNKID_DEF_DISARMED_TEXTID, DisarmedTextID);
        csave.WriteMicro(MICROCHUNKID_DEF_ARMING_TEXTID, ArmingTextID);
        csave.WriteMicro(MICROCHUNKID_DEF_ARM_INTERRUPT_TEXTID, ArmingInterruptedTextID);
        csave.WriteMicro(MICROCHUNKID_DEF_DISARM_INTERRUPT_TEXTID, DisarmingInterruptedTextID);
        csave.WriteMicro(MICROCHUNKID_DEF_PRE_CINEMATIC_DEFID, PreDetonateCinematicDefID);
        csave.WriteMicro(MICROCHUNKID_DEF_POST_CINEMATIC_DEFID, PostDetonateCinematicDefID);
        csave.WriteMicro(MICROCHUNKID_DEF_EXPLOSION_DEFID, ExplosionDefID);
        csave.WriteMicro(MICROCHUNKID_DEF_IS_NUKE, IsNuke);

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
                    Load_Variables(cload);
                    break;

                default:
                    Console.WriteLine("Unrecognized Beacon Def chunkID\n");
                    break;

            }
            cload.Close_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    public bool Is_Nuke() { return (IsNuke != 0); }

    //DECLARE_EDITABLE(BeaconGameObjDef, SimpleGameObjDef);

    protected void Load_Variables(ChunkLoadClass cload)
    {

        while (cload.Open_Micro_Chunk())
        {
            switch (cload.Cur_Micro_Chunk_ID)
            {
                case MICROCHUNKID_DEF_ARMING_ANIM_NAME:
                    cload.ReadMicroChunkWWString(out ArmingAnimationName);
                    break;
                //READ_MICRO_CHUNK_WWSTRING(cload, MICROCHUNKID_DEF_ARMING_ANIM_NAME, ArmingAnimationName);
                case MICROCHUNKID_DEF_BROADCAST_TIME:
                    cload.Read(ref BroadcastToAllTime);
                    break;
                case MICROCHUNKID_DEF_ARM_TIME:
                    cload.Read(ref ArmTime);
                    break;
                case MICROCHUNKID_DEF_DISARM_TIME:
                    cload.Read(ref DisarmTime);
                    break;
                case MICROCHUNKID_DEF_PRE_DETONATE_CINEMATIC_DELAY:
                    cload.Read(ref PreDetonateCinematicDelay);
                    break;
                case MICROCHUNKID_DEF_DETONATE_TIME:
                    cload.Read(ref DetonateTime);
                    break;
                case MICROCHUNKID_DEF_POST_DETONATE_TIME:
                    cload.Read(ref PostDetonateTime);
                    break;
                case MICROCHUNKID_DEF_ARMED_SOUNDID:
                    cload.Read(ref ArmedSoundDefID);
                    break;
                case MICROCHUNKID_DEF_DISARMING_TEXTID:
                    cload.Read(ref DisarmingTextID);
                    break;
                case MICROCHUNKID_DEF_DISARMED_TEXTID:
                    cload.Read(ref DisarmedTextID);
                    break;
                case MICROCHUNKID_DEF_ARMING_TEXTID:
                    cload.Read(ref ArmingTextID);
                    break;
                case MICROCHUNKID_DEF_ARM_INTERRUPT_TEXTID:
                    cload.Read(ref ArmingInterruptedTextID);
                    break;
                case MICROCHUNKID_DEF_DISARM_INTERRUPT_TEXTID:
                    cload.Read(ref DisarmingInterruptedTextID);
                    break;
                case MICROCHUNKID_DEF_PRE_CINEMATIC_DEFID:
                    cload.Read(ref PreDetonateCinematicDefID);
                    break;
                case MICROCHUNKID_DEF_POST_CINEMATIC_DEFID:
                    cload.Read(ref PostDetonateCinematicDefID);
                    break;
                case MICROCHUNKID_DEF_EXPLOSION_DEFID:
                    cload.Read(ref ExplosionDefID);
                    break;
                case MICROCHUNKID_DEF_IS_NUKE:
                    cload.Read(ref IsNuke);
                    break;

                default:
                    Console.WriteLine("Unrecognized Beacon Def Variable chunkID\n");
                    break;
            }

            cload.Close_Micro_Chunk();
        }

        return;

    }

    protected float BroadcastToAllTime;

    protected float ArmTime;
    protected float DisarmTime;
    protected float PreDetonateCinematicDelay;
    protected float DetonateTime;
    protected float PostDetonateTime;

    protected int ArmedSoundDefID;

    protected int DisarmingTextID;
    protected int DisarmedTextID;
    protected int ArmingTextID;

    protected int ArmingInterruptedTextID;
    protected int DisarmingInterruptedTextID;

    protected int PreDetonateCinematicDefID;
    protected int PostDetonateCinematicDefID;

    protected int ExplosionDefID;
    protected int IsNuke;

    protected string? ArmingAnimationName;

    private const uint CHUNKID_MONITOR_PARENT = 0x07250256;
    private const uint CHUNKID_DEF_PARENT = 0x02190435;
    private const uint CHUNKID_DEF_VARIABLES = 0x02190436;

    private const uint MICROCHUNKID_DEF_BROADCAST_TIME = 1;
    private const uint MICROCHUNKID_DEF_ARM_TIME = 2;
    private const uint MICROCHUNKID_DEF_DISARM_TIME = 3;
    private const uint MICROCHUNKID_DEF_DETONATE_TIME = 4;
    private const uint MICROCHUNKID_DEF_ARMED_SOUNDID = 5;
    private const uint MICROCHUNKID_DEF_DISARMING_TEXTID = 6;
    private const uint MICROCHUNKID_DEF_DISARMED_TEXTID = 7;
    private const uint MICROCHUNKID_DEF_ARMING_TEXTID = 8;
    private const uint MICROCHUNKID_DEF_POST_CINEMATIC_DEFID = 9;
    private const uint MICROCHUNKID_DEF_ARM_INTERRUPT_TEXTID = 10;
    private const uint MICROCHUNKID_DEF_DISARM_INTERRUPT_TEXTID = 11;
    private const uint MICROCHUNKID_DEF_ARMING_ANIM_NAME = 12;
    private const uint MICROCHUNKID_DEF_PRE_CINEMATIC_DEFID = 13;
    private const uint MICROCHUNKID_DEF_EXPLOSION_DEFID = 14;
    private const uint MICROCHUNKID_DEF_POST_DETONATE_TIME = 15;
    private const uint MICROCHUNKID_DEF_PRE_DETONATE_CINEMATIC_DELAY = 16;
    private const uint MICROCHUNKID_DEF_IS_NUKE = 17;

};

//SimplePersistFactoryClass<BeaconGameObjDef, CHUNKID_GAME_OBJECT_DEF_BEACON> _BeaconGameObjDefPersistFactory;
//SimplePersistFactoryClass<BeaconGameObj, CHUNKID_GAME_OBJECT_BEACON> _BeaconGameObjPersistFactory;
//DECLARE_DEFINITION_FACTORY(BeaconGameObjDef, CLASSID_GAME_OBJECT_DEF_BEACON, "Beacon")   _BeaconGameObjDefDefFactory;
