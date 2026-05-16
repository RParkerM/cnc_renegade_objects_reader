using RenData.ChunkIO;
using RenData.SaveLoad;
using RenData.IDs;
using RenData.PhysDef;

namespace RenData.PhysDef;

[RegisterDefinition(ChunkId.PHYSICS_CHUNKID_VTOLVEHICLEDEF)]
public partial class VTOLVehicleDefClass : VehiclePhysDefClass
{
    public VTOLVehicleDefClass()
    {

        MaxVerticalAcceleration = 0.0f;
        MaxHorizontalAcceleration = 0.0f;
        MaxFuselagePitch = WWMath.DEG_TO_RADF(15.0f);
        MaxFuselageRoll = WWMath.DEG_TO_RADF(20.0f);
        PitchControllerGain = 45.5f;
        PitchControllerDamping = 12.75f;
        RollControllerGain = 45.5f;
        RollControllerDamping = 12.75f;
        MaxYawVelocity = WWMath.DEG_TO_RADF(180.0f);
        YawControllerGain = 5.0f;
        MaxEngineRotation = WWMath.DEG_TO_RADF(25.0f);
        RotorSpeed = WWMath.DEG_TO_RADF(360.0f);
        RotorAcceleration = WWMath.DEG_TO_RADF(180.0f);
        RotorDeceleration = WWMath.DEG_TO_RADF(180.0f);
        //{
        // make our parameters editable
        //FLOAT_UNITS_PARAM(VTOLVehicleDefClass, MaxVerticalAcceleration, 0.0f, 100000.0f, "m/s^2");
        //FLOAT_UNITS_PARAM(VTOLVehicleDefClass, MaxHorizontalAcceleration, 0.0f, 100000.0f, "m/s^2");
        //ANGLE_EDITABLE_PARAM(VTOLVehicleDefClass, MaxFuselagePitch, DEG_TO_RADF(0.01f), DEG_TO_RADF(50.0f));
        //ANGLE_EDITABLE_PARAM(VTOLVehicleDefClass, MaxFuselageRoll, DEG_TO_RADF(0.01f), DEG_TO_RADF(50.0f));
        //FLOAT_EDITABLE_PARAM(VTOLVehicleDefClass, PitchControllerGain, 0.0f, 10000.0f);
        //FLOAT_EDITABLE_PARAM(VTOLVehicleDefClass, PitchControllerDamping, 0.0f, 10000.0f);
        //FLOAT_EDITABLE_PARAM(VTOLVehicleDefClass, RollControllerGain, 0.0f, 10000.0f);
        //FLOAT_EDITABLE_PARAM(VTOLVehicleDefClass, RollControllerDamping, 0.0f, 10000.0f);
        //ANGLE_EDITABLE_PARAM(VTOLVehicleDefClass, MaxYawVelocity, DEG_TO_RADF(0.01f), DEG_TO_RADF(360.0f));
        //FLOAT_EDITABLE_PARAM(VTOLVehicleDefClass, YawControllerGain, 0.0f, 10000.0f);
        //ANGLE_EDITABLE_PARAM(VTOLVehicleDefClass, MaxEngineRotation, DEG_TO_RADF(0.01f), DEG_TO_RADF(50.0f));
        //ANGLE_EDITABLE_PARAM(VTOLVehicleDefClass, RotorSpeed, DEG_TO_RADF(0.01f), DEG_TO_RADF(640.0f));
        //ANGLE_EDITABLE_PARAM(VTOLVehicleDefClass, RotorAcceleration, DEG_TO_RADF(0.01f), DEG_TO_RADF(640.0f));
        //ANGLE_EDITABLE_PARAM(VTOLVehicleDefClass, RotorDeceleration, DEG_TO_RADF(0.01f), DEG_TO_RADF(640.0f));
        //}

    }

    public override uint Get_Class_ID() => ClassId.CLASSID_VTOLVEHICLEDEF;

    public override PersistClass Create()
    {
        //VTOLVehicleClass obj = NEW_REF(VTOLVehicleClass, ());
        //obj.Init(this);
        //return obj;
        throw new NotImplementedException();
    }

    public override string Get_Type_Name() { return "VTOLVehicleDef"; }
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


    public override PersistFactoryClass Get_Factory() => _persistFactory;
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(VTOLVEHICLEDEF_CHUNK_VEHICLEPHYSDEF);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(VTOLVEHICLEDEF_CHUNK_VARIABLES);
        csave.WriteMicro(VTOLVEHICLEDEF_VARIABLE_MAXVERTICALACCELERATION, MaxVerticalAcceleration);
        csave.WriteMicro(VTOLVEHICLEDEF_VARIABLE_MAXHORIZONTALACCELERATION, MaxHorizontalAcceleration);
        csave.WriteMicro(VTOLVEHICLEDEF_VARIABLE_MAXFUSELAGEPITCH, MaxFuselagePitch);
        csave.WriteMicro(VTOLVEHICLEDEF_VARIABLE_MAXFUSELAGEROLL, MaxFuselageRoll);
        csave.WriteMicro(VTOLVEHICLEDEF_VARIABLE_PITCHCONTROLLERGAIN, PitchControllerGain);
        csave.WriteMicro(VTOLVEHICLEDEF_VARIABLE_PITCHCONTROLLERDAMPING, PitchControllerDamping);
        csave.WriteMicro(VTOLVEHICLEDEF_VARIABLE_ROLLCONTROLLERGAIN, RollControllerGain);
        csave.WriteMicro(VTOLVEHICLEDEF_VARIABLE_ROLLCONTROLLERDAMPING, RollControllerDamping);
        csave.WriteMicro(VTOLVEHICLEDEF_VARIABLE_MAXYAWVELOCITY, MaxYawVelocity);
        csave.WriteMicro(VTOLVEHICLEDEF_VARIABLE_YAWCONTROLLERGAIN, YawControllerGain);
        csave.WriteMicro(VTOLVEHICLEDEF_VARIABLE_MAXENGINEROTATION, MaxEngineRotation);
        csave.WriteMicro(VTOLVEHICLEDEF_VARIABLE_ROTORSPEED, RotorSpeed);
        csave.WriteMicro(VTOLVEHICLEDEF_VARIABLE_ROTORACCELERATION, RotorAcceleration);
        csave.WriteMicro(VTOLVEHICLEDEF_VARIABLE_ROTORDECELERATION, RotorDeceleration);
        csave.End_Chunk();
        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {

            switch (cload.Cur_Chunk_ID)
            {

                case VTOLVEHICLEDEF_CHUNK_VEHICLEPHYSDEF:
                    base.Load(cload);
                    break;

                case VTOLVEHICLEDEF_CHUNK_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case VTOLVEHICLEDEF_VARIABLE_MAXVERTICALACCELERATION:
                                cload.Read(ref MaxVerticalAcceleration);
                                break;
                            case VTOLVEHICLEDEF_VARIABLE_MAXHORIZONTALACCELERATION:
                                cload.Read(ref MaxHorizontalAcceleration);
                                break;
                            case VTOLVEHICLEDEF_VARIABLE_MAXFUSELAGEPITCH:
                                cload.Read(ref MaxFuselagePitch);
                                break;
                            case VTOLVEHICLEDEF_VARIABLE_MAXFUSELAGEROLL:
                                cload.Read(ref MaxFuselageRoll);
                                break;
                            case VTOLVEHICLEDEF_VARIABLE_PITCHCONTROLLERGAIN:
                                cload.Read(ref PitchControllerGain);
                                break;
                            case VTOLVEHICLEDEF_VARIABLE_PITCHCONTROLLERDAMPING:
                                cload.Read(ref PitchControllerDamping);
                                break;
                            case VTOLVEHICLEDEF_VARIABLE_ROLLCONTROLLERGAIN:
                                cload.Read(ref RollControllerGain);
                                break;
                            case VTOLVEHICLEDEF_VARIABLE_ROLLCONTROLLERDAMPING:
                                cload.Read(ref RollControllerDamping);
                                break;
                            case VTOLVEHICLEDEF_VARIABLE_MAXYAWVELOCITY:
                                cload.Read(ref MaxYawVelocity);
                                break;
                            case VTOLVEHICLEDEF_VARIABLE_YAWCONTROLLERGAIN:
                                cload.Read(ref YawControllerGain);
                                break;
                            case VTOLVEHICLEDEF_VARIABLE_MAXENGINEROTATION:
                                cload.Read(ref MaxEngineRotation);
                                break;
                            case VTOLVEHICLEDEF_VARIABLE_ROTORSPEED:
                                cload.Read(ref RotorSpeed);
                                break;
                            case VTOLVEHICLEDEF_VARIABLE_ROTORACCELERATION:
                                cload.Read(ref RotorAcceleration);
                                break;
                            case VTOLVEHICLEDEF_VARIABLE_ROTORDECELERATION:
                                cload.Read(ref RotorDeceleration);
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unhandled Chunk: 0x{0:X8} (depth {1}) in {2}", cload.Cur_Chunk_ID, cload.Cur_Chunk_Depth, nameof(VTOLVehicleDefClass));
                    break;
            }

            cload.Close_Chunk();
        }

        return true;
    }

    //DECLARE_EDITABLE(VTOLVehicleDefClass, VehiclePhysDefClass);

    protected float MaxVerticalAcceleration;
    protected float MaxHorizontalAcceleration;

    protected float MaxFuselagePitch;               //	DEG_TO_RADF(15.0f);
    protected float MaxFuselageRoll;                // DEG_TO_RADF(20.0f);
    protected float PitchControllerGain;            // 45.5
    protected float PitchControllerDamping;     // 12.75
    protected float RollControllerGain;         // 45.5
    protected float RollControllerDamping;      // 12.75

    protected float MaxYawVelocity;             // DEG_TO_RADF(180.0f);
    protected float YawControllerGain;          // 5.0f;

    protected float MaxEngineRotation;

    protected float RotorSpeed;
    protected float RotorAcceleration;
    protected float RotorDeceleration;

    // Replaced file-scope enum with private consts scoped to the class
    private const uint VTOLVEHICLEDEF_CHUNK_VEHICLEPHYSDEF = 408000936u; // (parent class)
    private const uint VTOLVEHICLEDEF_CHUNK_VARIABLES = 408000937u;

    private const int VTOLVEHICLEDEF_VARIABLE_MAXVERTICALACCELERATION = 0x00;
    private const int VTOLVEHICLEDEF_VARIABLE_MAXHORIZONTALACCELERATION = 0x01;
    private const int VTOLVEHICLEDEF_VARIABLE_MAXFUSELAGEPITCH = 0x02;
    private const int VTOLVEHICLEDEF_VARIABLE_MAXFUSELAGEROLL = 0x03;
    private const int VTOLVEHICLEDEF_VARIABLE_PITCHCONTROLLERGAIN = 0x04;
    private const int VTOLVEHICLEDEF_VARIABLE_PITCHCONTROLLERDAMPING = 0x05;
    private const int VTOLVEHICLEDEF_VARIABLE_ROLLCONTROLLERGAIN = 0x06;
    private const int VTOLVEHICLEDEF_VARIABLE_ROLLCONTROLLERDAMPING = 0x07;
    private const int VTOLVEHICLEDEF_VARIABLE_MAXYAWVELOCITY = 0x08;
    private const int VTOLVEHICLEDEF_VARIABLE_YAWCONTROLLERGAIN = 0x09;
    private const int VTOLVEHICLEDEF_VARIABLE_MAXENGINEROTATION = 0x0A;
    private const int VTOLVEHICLEDEF_VARIABLE_ROTORSPEED = 0x0B;
    private const int VTOLVEHICLEDEF_VARIABLE_ROTORACCELERATION = 0x0C;
    private const int VTOLVEHICLEDEF_VARIABLE_ROTORDECELERATION = 0x0D;
}
