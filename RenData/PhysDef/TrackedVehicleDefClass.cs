using RenData.ChunkIO;
using RenData.SaveLoad;
using RenData.IDs;

namespace RenData.PhysDef;


[RegisterDefinition(ChunkId.PHYSICS_CHUNKID_TRACKEDVEHICLEDEF)]
public partial class TrackedVehicleDefClass : VehiclePhysDefClass
{
    public TrackedVehicleDefClass()
    {
        MaxEngineTorque = 0.0f;
        TrackUScaleFactor = 25.0f;
        TrackVScaleFactor = 0.0f;
        TurnTorqueScaleFactor = 1.0f;
        // make our parameters editable
        //FLOAT_EDITABLE_PARAM(TrackedVehicleDefClass, MaxEngineTorque, 0.0f, 100000.0f);
        //FLOAT_EDITABLE_PARAM(TrackedVehicleDefClass, TrackUScaleFactor, -1000.0f, 1000.0f);
        //FLOAT_EDITABLE_PARAM(TrackedVehicleDefClass, TrackVScaleFactor, -1000.0f, 1000.0f);
        //FLOAT_EDITABLE_PARAM(TrackedVehicleDefClass, TurnTorqueScaleFactor, 0.0f, 1.0f);
    }

    // From DefinitionClass
    public override uint Get_Class_ID() => ClassId.CLASSID_TRACKEDVEHICLEDEF;
    public override PersistClass Create()
    {
        //TrackedVehicleClass obj = NEW_REF(TrackedVehicleClass, ());
        //obj.Init(this);
        //return obj;
        throw new NotImplementedException();
    }

    // From PhysDefClass
    public override string Get_Type_Name() { return "TrackedVehicleDef"; }
    public override bool Is_Type(string type_name)
    {
        if (string.Compare(type_name, Get_Type_Name()) == 0)
        {
            return true;
        }
        else
        {
            return base.Is_Type(type_name);
        }
    }

    // From PersistClass
    public override PersistFactoryClass Get_Factory() => _persistFactory;
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(TRACKEDVEHICLEDEF_CHUNK_VEHICLEPHYSDEF);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(TRACKEDVEHICLEDEF_CHUNK_VARIABLES);
        csave.WriteMicro(TRACKEDVEHICLEDEF_VARIABLE_MAXENGINETORQUE, MaxEngineTorque);
        csave.WriteMicro(TRACKEDVEHICLEDEF_VARIABLE_TRACKUSCALEFACTOR, TrackUScaleFactor);
        csave.WriteMicro(TRACKEDVEHICLEDEF_VARIABLE_TRACKVSCALEFACTOR, TrackVScaleFactor);
        csave.WriteMicro(TRACKEDVEHICLEDEF_VARIABLE_TURNTORQUESCALEFACTOR, TurnTorqueScaleFactor);
        csave.End_Chunk();
        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {

            switch (cload.Cur_Chunk_ID)
            {

                case TRACKEDVEHICLEDEF_CHUNK_VEHICLEPHYSDEF:
                    base.Load(cload);
                    break;

                case TRACKEDVEHICLEDEF_CHUNK_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case TRACKEDVEHICLEDEF_VARIABLE_MAXENGINETORQUE:
                                cload.Read(ref MaxEngineTorque);
                                break;
                            case TRACKEDVEHICLEDEF_VARIABLE_TRACKUSCALEFACTOR:
                                cload.Read(ref TrackUScaleFactor);
                                break;
                            case TRACKEDVEHICLEDEF_VARIABLE_TRACKVSCALEFACTOR:
                                cload.Read(ref TrackVScaleFactor);
                                break;
                            case TRACKEDVEHICLEDEF_VARIABLE_TURNTORQUESCALEFACTOR:
                                cload.Read(ref TurnTorqueScaleFactor);
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unhandled Chunk: 0x{0:X8} (depth {1}) in {2}", cload.Cur_Chunk_ID, cload.Cur_Chunk_Depth, nameof(TrackedVehicleDefClass));

                    break;
            }

            cload.Close_Chunk();
        }

        return true;
    }

    // Read access to the ants
    public float Get_Max_Engine_Torque() { return MaxEngineTorque; }
    public float Get_Track_U_Scale_Factor() { return TrackUScaleFactor; }
    public float Get_Track_V_Scale_Factor() { return TrackVScaleFactor; }
    public float Get_Turn_Torque_Scale_Factor() { return TurnTorqueScaleFactor; }

    // Write access (DEBUGGING/TESTING ONLY)
    public void Set_Max_Engine_Torque(float t) { MaxEngineTorque = t; }
    public void Set_Track_U_Scale_Factor(float k) { TrackUScaleFactor = k; }
    public void Set_Track_V_Scale_Factor(float k) { TrackVScaleFactor = k; }
    public void Set_Turn_Torque_Scale_Factor(float k) { TurnTorqueScaleFactor = k; }

    //	Editable interface requirements
    //DECLARE_EDITABLE(TrackedVehicleDefClass, VehiclePhysDefClass);


    protected float MaxEngineTorque;
    protected float TrackUScaleFactor;
    protected float TrackVScaleFactor;
    protected float TurnTorqueScaleFactor;


    private const int TRACKEDVEHICLEDEF_CHUNK_VEHICLEPHYSDEF = 68682540;//0406001454;
    private const int TRACKEDVEHICLEDEF_CHUNK_VARIABLES = 68682541; //0406001455;

    private const int TRACKEDVEHICLEDEF_VARIABLE_MAXENGINETORQUE = 0x00;
    private const int TRACKEDVEHICLEDEF_VARIABLE_TRACKUSCALEFACTOR = 0x01;
    private const int TRACKEDVEHICLEDEF_VARIABLE_TRACKVSCALEFACTOR = 0x02;
    private const int TRACKEDVEHICLEDEF_VARIABLE_TURNTORQUESCALEFACTOR = 0x03;
};

