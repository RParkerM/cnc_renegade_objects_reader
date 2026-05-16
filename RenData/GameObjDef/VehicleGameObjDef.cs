using RenData.IDs;
using RenData.Types;
using RenData.ChunkIO;
using RenData.SaveLoad;
using RenData.Transitions;
using RenData.GameObjDef;

namespace RenData.Definitions;

[RegisterDefinition(ChunkId.CHUNKID_GAME_OBJECT_DEF_VEHICLE)]
public partial class VehicleGameObjDef : SmartGameObjDef
{
    public VehicleGameObjDef()
    {
        Type = VehicleType.VEHICLE_TYPE_CAR;
        TurnRadius = 10.0f;
        OccupantsVisible = true;
        EngineSoundMaxPitchFactor = 2.0F;
        SightDownMuzzle = false;
        Aim2D = true;
        SquishVelocity = 1.5f;
        VehicleNameID = 0;
        NumSeats = 2;
        GDIDamageReportID = 0;
        NodDamageReportID = 0;
        GDIDestroyReportID = 0;
        NodDestroyReportID = 0;
        // initialize all engine sound defs to zero
        for (int i = 0; i < MAX_ENGINE_SOUND_STATES; i++)
        {
            EngineSound[i] = 0;
        }

        //    MODEL_DEF_PARAM(VehicleGameObjDef, PhysDefID, "DynamicPhysDef");

        //    EDITABLE_PARAM(VehicleGameObjDef, ParameterClass::TYPE_STRING, TypeName);
        //    EDITABLE_PARAM(VehicleGameObjDef, ParameterClass::TYPE_STRING, Fire0Anim);
        //    EDITABLE_PARAM(VehicleGameObjDef, ParameterClass::TYPE_STRING, Fire1Anim);
        //    EDITABLE_PARAM(VehicleGameObjDef, ParameterClass::TYPE_STRING, Profile);

        //    EDITABLE_PARAM(VehicleGameObjDef, ParameterClass::TYPE_FLOAT, TurnRadius);
        //    EDITABLE_PARAM(VehicleGameObjDef, ParameterClass::TYPE_FLOAT, SquishVelocity);
        //    EDITABLE_PARAM(VehicleGameObjDef, ParameterClass::TYPE_BOOL, Aim2D);

        //# ifdef	PARAM_EDITING_ON
        //    EnumParameterClass* param;
        //    param = new EnumParameterClass((int*)&Type);
        //    param->Set_Name("Type");
        //    param->Add_Value("Car", VEHICLE_TYPE_CAR);
        //    param->Add_Value("Tank", VEHICLE_TYPE_TANK);
        //    param->Add_Value("Bike", VEHICLE_TYPE_BIKE);
        //    param->Add_Value("Flying", VEHICLE_TYPE_FLYING);
        //    param->Add_Value("Turret", VEHICLE_TYPE_TURRET);

        //    GENERIC_EDITABLE_PARAM(VehicleGameObjDef, param)
        //#endif

        //	EDITABLE_PARAM(VehicleGameObjDef, ParameterClass::TYPE_BOOL, OccupantsVisible);

        //    // engine sounds
        //    FLOAT_EDITABLE_PARAM(VehicleGameObjDef, EngineSoundMaxPitchFactor, 1.0F, 10.0F);
        //    NAMED_EDITABLE_PARAM(VehicleGameObjDef, ParameterClass::TYPE_SOUNDDEFINITIONID, EngineSound[ENGINE_SOUND_STATE_STARTING], "Engine Start Sound");
        //    NAMED_EDITABLE_PARAM(VehicleGameObjDef, ParameterClass::TYPE_SOUNDDEFINITIONID, EngineSound[ENGINE_SOUND_STATE_RUNNING], "Engine Running Loop");
        //    NAMED_EDITABLE_PARAM(VehicleGameObjDef, ParameterClass::TYPE_SOUNDDEFINITIONID, EngineSound[ENGINE_SOUND_STATE_STOPPING], "Engine Stop Sound");
        //    //	NAMED_EDITABLE_PARAM(VehicleGameObjDef, ParameterClass::TYPE_SOUNDDEFINITIONID, EngineSound[ENGINE_SOUND_STATE_OFF],"Engine Off Sound");

        //    EDITABLE_PARAM(VehicleGameObjDef, ParameterClass::TYPE_BOOL, SightDownMuzzle);

        //    EDITABLE_PARAM(VehicleGameObjDef, ParameterClass::TYPE_STRINGSDB_ID, VehicleNameID);

        //    EDITABLE_PARAM(VehicleGameObjDef, ParameterClass::TYPE_INT, NumSeats);

        //    EDITABLE_PARAM(VehicleGameObjDef, ParameterClass::TYPE_STRINGSDB_ID, GDIDamageReportID);
        //    EDITABLE_PARAM(VehicleGameObjDef, ParameterClass::TYPE_STRINGSDB_ID, NodDamageReportID);
        //    EDITABLE_PARAM(VehicleGameObjDef, ParameterClass::TYPE_STRINGSDB_ID, GDIDestroyReportID);
        //    EDITABLE_PARAM(VehicleGameObjDef, ParameterClass::TYPE_STRINGSDB_ID, NodDestroyReportID);
    }
    ~VehicleGameObjDef()
    {
        Free_Transition_List();
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GAME_OBJECT_DEF_VEHICLE;
    public override PersistClass Create()
    {
        //PersistClass* VehicleGameObjDef::Create(void) const
        //{
        //	VehicleGameObj * obj = new VehicleGameObj;
        //obj->Init(*this);
        //return obj;
        //}
        throw new NotImplementedException();
    }
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        csave.WriteMicro(MICROCHUNKID_TYPE, (uint)Type);
        ArgumentNullException.ThrowIfNull(TypeName);
        csave.WriteMicroString(MICROCHUNKID_TYPE_NAME, TypeName);
        ArgumentNullException.ThrowIfNull(Fire0Anim);
        csave.WriteMicroString(MICROCHUNKID_FIRE0ANIM, Fire0Anim);
        ArgumentNullException.ThrowIfNull(Fire1Anim);
        csave.WriteMicroString(MICROCHUNKID_FIRE1ANIM, Fire1Anim);
        ArgumentNullException.ThrowIfNull(Profile);
        csave.WriteMicroString(MICROCHUNKID_PROFILE, Profile);
        csave.WriteMicro(MICROCHUNKID_TURN_RADIUS, TurnRadius);
        csave.WriteMicro(MICROCHUNKID_OCCUPANTS_VISIBLE, OccupantsVisible);
        csave.WriteMicro(MICROCHUNKID_ENGINE_SOUND_MAX_PITCH_FACTOR, EngineSoundMaxPitchFactor);
        csave.WriteMicro(MICROCHUNKID_ENGINE_START_SOUND, EngineSound[ENGINE_SOUND_STATE_STARTING]);
        csave.WriteMicro(MICROCHUNKID_ENGINE_RUN_SOUND, EngineSound[ENGINE_SOUND_STATE_RUNNING]);
        csave.WriteMicro(MICROCHUNKID_ENGINE_STOP_SOUND, EngineSound[ENGINE_SOUND_STATE_STOPPING]);
        csave.WriteMicro(MICROCHUNKID_ENGINE_OFF_SOUND, EngineSound[ENGINE_SOUND_STATE_OFF]);
        csave.WriteMicro(MICROCHUNKID_DEF_SIGHT_DOWN_MUZZLE, SightDownMuzzle);
        csave.WriteMicro(MICROCHUNKID_DEF_AIM_2D, Aim2D);
        csave.WriteMicro(MICROCHUNKID_DEF_SQUISH_VELOCITY, SquishVelocity);
        csave.WriteMicro(MICROCHUNKID_DEF_VEHICLE_NAME_ID, VehicleNameID);
        csave.WriteMicro(MICROCHUNKID_DEF_NUM_SEATS, NumSeats);
        csave.WriteMicro(MICROCHUNKID_DEF_GDI_DAMAGE_REPORT_ID, GDIDamageReportID);
        csave.WriteMicro(MICROCHUNKID_DEF_NOD_DAMAGE_REPORT_ID, NodDamageReportID);
        csave.WriteMicro(MICROCHUNKID_DEF_GDI_DESTROY_REPORT_ID, GDIDestroyReportID);
        csave.WriteMicro(MICROCHUNKID_DEF_NOD_DESTROY_REPORT_ID, NodDestroyReportID);
        csave.End_Chunk();

        //	Save each of the transition 'definitions' in our list.
        for (int index = 0; index < Transitions.Count; index++)
        {
            TransitionDataClass transition = Transitions[index];
            if (transition is not null)
            {
                //	Save this transition 'definition' to its own chunk.
                csave.Begin_Chunk(CHUNKID_DEF_TRANSITION);
                transition.Save(csave);
                csave.End_Chunk();
            }
        }

        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        Free_Transition_List();

        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {

                case CHUNKID_DEF_PARENT:
                    base.Load(cload);
                    break;

                case CHUNKID_DEF_TRANSITION:
                    {
                        TransitionDataClass transition = new();
                        transition.Load(cload);
                        Transitions.Add(transition);
                    }
                    break;

                case CHUNKID_DEF_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case MICROCHUNKID_TYPE:
                                cload.Read(ref Type);
                                break;
                            case MICROCHUNKID_TYPE_NAME:
                                TypeName = cload.ReadMicroChunkWWString();
                                break;
                            case MICROCHUNKID_FIRE0ANIM:
                                Fire0Anim = cload.ReadMicroChunkWWString();
                                break;
                            case MICROCHUNKID_FIRE1ANIM:
                                Fire1Anim = cload.ReadMicroChunkWWString();
                                break;
                            case MICROCHUNKID_PROFILE:
                                Profile = cload.ReadMicroChunkWWString();
                                break;
                            case MICROCHUNKID_PHYS_ID:
                                cload.Read(ref PhysDefID);
                                break;
                            case MICROCHUNKID_TURN_RADIUS:
                                cload.Read(ref TurnRadius);
                                break;
                            case MICROCHUNKID_OCCUPANTS_VISIBLE:
                                cload.Read(ref OccupantsVisible);
                                break;
                            case MICROCHUNKID_ENGINE_SOUND_MAX_PITCH_FACTOR:
                                cload.Read(ref EngineSoundMaxPitchFactor);
                                break;
                            case MICROCHUNKID_ENGINE_START_SOUND:
                                cload.Read(ref EngineSound[ENGINE_SOUND_STATE_STARTING]);
                                break;
                            case MICROCHUNKID_ENGINE_RUN_SOUND:
                                cload.Read(ref EngineSound[ENGINE_SOUND_STATE_RUNNING]);
                                break;
                            case MICROCHUNKID_ENGINE_STOP_SOUND:
                                cload.Read(ref EngineSound[ENGINE_SOUND_STATE_STOPPING]);
                                break;
                            case MICROCHUNKID_ENGINE_OFF_SOUND:
                                cload.Read(ref EngineSound[ENGINE_SOUND_STATE_OFF]);
                                break;
                            case MICROCHUNKID_DEF_SIGHT_DOWN_MUZZLE:
                                cload.Read(ref SightDownMuzzle);
                                break;
                            case MICROCHUNKID_DEF_AIM_2D:
                                cload.Read(ref Aim2D);
                                break;
                            case MICROCHUNKID_DEF_SQUISH_VELOCITY:
                                cload.Read(ref SquishVelocity);
                                break;
                            case MICROCHUNKID_DEF_VEHICLE_NAME_ID:
                                cload.Read(ref VehicleNameID);
                                break;
                            case MICROCHUNKID_DEF_NUM_SEATS:
                                cload.Read(ref NumSeats);
                                break;
                            case MICROCHUNKID_DEF_GDI_DAMAGE_REPORT_ID:
                                cload.Read(ref GDIDamageReportID);
                                break;
                            case MICROCHUNKID_DEF_NOD_DAMAGE_REPORT_ID:
                                cload.Read(ref NodDamageReportID);
                                break;
                            case MICROCHUNKID_DEF_GDI_DESTROY_REPORT_ID:
                                cload.Read(ref GDIDestroyReportID);
                                break;
                            case MICROCHUNKID_DEF_NOD_DESTROY_REPORT_ID:
                                cload.Read(ref NodDestroyReportID);
                                break;
                        default:
                                Console.WriteLine($"Unrecognized VehicleDef Variable chunkID {cload.Cur_Micro_Chunk_ID}");
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine($"Unrecognized VehicleDef chunkID {cload.Cur_Chunk_ID}");
                    break;
            }
            cload.Close_Chunk();
        }

        return true;
    }
    public override PersistFactoryClass Get_Factory() => _persistFactory;

    //DECLARE_EDITABLE(VehicleGameObjDef, SmartGameObjDef );

    public List<TransitionDataClass> Get_Transition_List()
    {
        return Transitions;
    }
    public void Free_Transition_List()
    {
        Transitions.Clear();
    }

    public int Get_Damage_Report(int team)
    {
        if ((int)PlayerType.PLAYERTYPE_GDI == team)
        {
            return GDIDamageReportID;
        }
        else if ((int)PlayerType.PLAYERTYPE_NOD == team)
        {
            return NodDamageReportID;
        }

        return 0;
    }
    public int Get_Destroy_Report(int team)
    {
        if ((int)PlayerType.PLAYERTYPE_GDI == team)
        {
            return GDIDestroyReportID;
        }
        else if ((int)PlayerType.PLAYERTYPE_NOD == team)
        {
            return NodDestroyReportID;
        }

        return 0;
    }

    protected new VehicleType Type;
    protected string? TypeName;
    protected string? Fire0Anim;
    protected string? Fire1Anim;
    protected string? Profile;
    protected List<TransitionDataClass> Transitions = new();
    protected float TurnRadius;
    protected bool OccupantsVisible;
    protected bool SightDownMuzzle;
    protected bool Aim2D;

    protected float EngineSoundMaxPitchFactor;
    protected int[] EngineSound = new int[MAX_ENGINE_SOUND_STATES];

    protected float SquishVelocity;
    protected int VehicleNameID;

    protected int NumSeats;

    protected int GDIDamageReportID;
    protected int NodDamageReportID;
    protected int GDIDestroyReportID;
    protected int NodDestroyReportID;

    private const int ENGINE_SOUND_STATE_STARTING = 0;
    private const int ENGINE_SOUND_STATE_RUNNING = 1;
    private const int ENGINE_SOUND_STATE_STOPPING = 2;
    private const int ENGINE_SOUND_STATE_OFF = 3;
    private const int MAX_ENGINE_SOUND_STATES = 4;

    private const int CHUNKID_DEF_PARENT = 930991656;
    private const int CHUNKID_DEF_VARIABLES = 930991657;
    private const int CHUNKID_DEF_TRANSITION = 930991658;

    private const int MICROCHUNKID_TYPE = 1;
    private const int MICROCHUNKID_TYPE_NAME = 2;
    private const int MICROCHUNKID_FIRE0ANIM = 3;
    private const int MICROCHUNKID_FIRE1ANIM = 4;
    private const int MICROCHUNKID_PROFILE = 5;
    private const int MICROCHUNKID_WEAPON_TURN_TRANS = 6;

    private const int XXXMICROCHUNKID_SOUND = 7;
    private const int MICROCHUNKID_EMITER_NAME = 8;
    private const int MICROCHUNKID_EMITER_OFFSET = 9;
    private const int MICROCHUNKID_EMITER2_NAME = 10;
    private const int MICROCHUNKID_EMITER2_OFFSET = 11;
    private const int XXX_MICROCHUNKID_MASS = 12;
    private const int XXX_MICROCHUNKID_MAX_ENGINE_TORQUE = 13;
    private const int XXXMICROCHUNKID_STEERING_ANGLE = 14;
    private const int XXXMICROCHUNKID_SPRING_CONSTANT = 15;
    private const int XXXMICROCHUNKID_DAMPING_COEFFIENT = 16;
    private const int XXXMICROCHUNKID_SPRING_LENGTH = 17;
    private const int MICROCHUNKID_PHYS_ID = 18;
    private const int MICROCHUNKID_TURN_RADIUS = 19;
    private const int MICROCHUNKID_OCCUPANTS_VISIBLE = 20;
    private const int XXXMICROCHUNKID_ENGINE_SOUND_RPM_SCALE_MIN = 21;
    private const int XXXMICROCHUNKID_ENGINE_SOUND_RPM_SCALE_MAX = 22;
    private const int MICROCHUNKID_ENGINE_START_SOUND = 23;
    private const int MICROCHUNKID_ENGINE_RUN_SOUND = 24;
    private const int MICROCHUNKID_ENGINE_STOP_SOUND = 25;
    private const int MICROCHUNKID_ENGINE_OFF_SOUND = 26;

    private const int MICROCHUNKID_DEF_SIGHT_DOWN_MUZZLE = 27;
    private const int MICROCHUNKID_DEF_AIM_2D = 28;
    private const int MICROCHUNKID_DEF_SQUISH_VELOCITY = 29;
    private const int MICROCHUNKID_ENGINE_SOUND_MAX_PITCH_FACTOR = 30;
    private const int MICROCHUNKID_DEF_VEHICLE_NAME_ID = 31;
    private const int MICROCHUNKID_DEF_NUM_SEATS = 32;
    private const int MICROCHUNKID_DEF_GDI_DAMAGE_REPORT_ID = 33;
    private const int MICROCHUNKID_DEF_NOD_DAMAGE_REPORT_ID = 34;
    private const int MICROCHUNKID_DEF_GDI_DESTROY_REPORT_ID = 35;
    private const int MICROCHUNKID_DEF_NOD_DESTROY_REPORT_ID = 36;
}


public enum VehicleType
{
    VEHICLE_TYPE_CAR,
    VEHICLE_TYPE_TANK,
    VEHICLE_TYPE_BIKE,
    VEHICLE_TYPE_FLYING,
    VEHICLE_TYPE_TURRET,
}

enum EngineSoundState
{
    ENGINE_SOUND_STATE_STARTING = 0,
    ENGINE_SOUND_STATE_RUNNING,
    ENGINE_SOUND_STATE_STOPPING,
    ENGINE_SOUND_STATE_OFF,

    MAX_ENGINE_SOUND_STATES
};

public enum VehicleChunkId
{
    CHUNKID_DEF_PARENT = 930991656,
    CHUNKID_DEF_VARIABLES,
    CHUNKID_DEF_TRANSITION,

    MICROCHUNKID_TYPE = 1,
    MICROCHUNKID_TYPE_NAME,
    MICROCHUNKID_FIRE0ANIM,
    MICROCHUNKID_FIRE1ANIM,
    MICROCHUNKID_PROFILE,
    MICROCHUNKID_WEAPON_TURN_TRANS,

    XXXMICROCHUNKID_SOUND,
    MICROCHUNKID_EMITER_NAME,
    MICROCHUNKID_EMITER_OFFSET,
    MICROCHUNKID_EMITER2_NAME,
    MICROCHUNKID_EMITER2_OFFSET,
    XXX_MICROCHUNKID_MASS,
    XXX_MICROCHUNKID_MAX_ENGINE_TORQUE,
    XXX_MICROCHUNKID_STEERING_ANGLE,
    XXX_MICROCHUNKID_SPRING_CONSTANT,
    XXX_MICROCHUNKID_DAMPING_COEFFIENT,
    XXX_MICROCHUNKID_SPRING_LENGTH,
    MICROCHUNKID_PHYS_ID,
    MICROCHUNKID_TURN_RADIUS,
    MICROCHUNKID_OCCUPANTS_VISIBLE,

    XXX_MICROCHUNKID_ENGINE_SOUND_RPM_SCALE_MIN,
    XXX_MICROCHUNKID_ENGINE_SOUND_RPM_SCALE_MAX,
    MICROCHUNKID_ENGINE_START_SOUND,
    MICROCHUNKID_ENGINE_RUN_SOUND,
    MICROCHUNKID_ENGINE_STOP_SOUND,
    MICROCHUNKID_ENGINE_OFF_SOUND,

    MICROCHUNKID_DEF_SIGHT_DOWN_MUZZLE,
    MICROCHUNKID_DEF_AIM_2D,

    MICROCHUNKID_DEF_SQUISH_VELOCITY,
    MICROCHUNKID_ENGINE_SOUND_MAX_PITCH_FACTOR,
    MICROCHUNKID_DEF_VEHICLE_NAME_ID,
    MICROCHUNKID_DEF_NUM_SEATS,
    MICROCHUNKID_DEF_GDI_DAMAGE_REPORT_ID,
    MICROCHUNKID_DEF_NOD_DAMAGE_REPORT_ID,
    MICROCHUNKID_DEF_GDI_DESTROY_REPORT_ID,
    MICROCHUNKID_DEF_NOD_DESTROY_REPORT_ID,
}

//file static class VehicleGameObjDefRegistration
//{
//    private static SimplePersistFactoryClass<VehicleGameObjDef> _VehicleGameObjDefPersistFactory
//        = new((uint)PresetChunkId.CHUNKID_GAME_OBJECT_DEF_VEHICLE);

//    [ModuleInitializer]
//    internal static void Init() 
//    {
//        GC.KeepAlive(_VehicleGameObjDefPersistFactory);
//    }
//}