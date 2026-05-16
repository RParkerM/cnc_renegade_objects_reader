using RenData.ChunkIO;
using RenData.SaveLoad;
using RenData.IDs;

namespace RenData.PhysDef;

[RegisterDefinition(ChunkId.PHYSICS_CHUNKID_MOTORVEHICLEDEF)]
public partial class MotorVehicleDefClass : VehiclePhysDefClass
{

    public MotorVehicleDefClass()
    {
        MaxEngineTorque = 5.0f;
        EngineTorqueCurveFilename = "Vehicles\\PhysicsTables\\DefaultEngineTorque.tbl";
        EngineTorqueCurve = null;
        GearCount = 4;
        FinalDriveGearRatio = 2.92f;
        ShiftUpRpm = 7000;
        ShiftDownRpm = 2000;
        DriveTrainInertia = 0.1f;

        GearRatio[0] = 12.01f;  // 1989 Ford Taurus gear ratios :-)
        GearRatio[1] = 7.82f;
        GearRatio[2] = 5.16f;
        GearRatio[3] = 3.81f;
        GearRatio[4] = 2.79f;
        GearRatio[5] = 1.0f;

        ShiftUpAvel = MotorVehicle.RPM_TO_RADS(ShiftUpRpm);
        ShiftDownAvel = MotorVehicle.RPM_TO_RADS(ShiftDownRpm);


        // make our parameters editable
        //FLOAT_UNITS_PARAM(MotorVehicleDefClass, MaxEngineTorque, 0.01f, 100000.0f, "Nm");
        //FILENAME_PARAM(MotorVehicleDefClass, EngineTorqueCurveFilename, "Table Files", ".tbl");

        //INT_EDITABLE_PARAM(MotorVehicleDefClass, GearCount, 1, 6);

        //FLOAT_EDITABLE_PARAM(MotorVehicleDefClass, GearRatio[0], 1.0f, 100.0f);
        //FLOAT_EDITABLE_PARAM(MotorVehicleDefClass, GearRatio[1], 1.0f, 100.0f);
        //FLOAT_EDITABLE_PARAM(MotorVehicleDefClass, GearRatio[2], 1.0f, 100.0f);
        //FLOAT_EDITABLE_PARAM(MotorVehicleDefClass, GearRatio[3], 1.0f, 100.0f);
        //FLOAT_EDITABLE_PARAM(MotorVehicleDefClass, GearRatio[4], 1.0f, 100.0f);
        //FLOAT_EDITABLE_PARAM(MotorVehicleDefClass, GearRatio[5], 1.0f, 100.0f);
        //FLOAT_EDITABLE_PARAM(MotorVehicleDefClass, FinalDriveGearRatio, 0.0f, 100.0f);

        //FLOAT_EDITABLE_PARAM(MotorVehicleDefClass, DriveTrainInertia, 0.001f, 100000.0f);

        //FLOAT_EDITABLE_PARAM(MotorVehicleDefClass, ShiftUpRpm, 1.0f, 100000.0f);
        //FLOAT_EDITABLE_PARAM(MotorVehicleDefClass, ShiftDownRpm, 1.0f, 100000.0f);
    }

    ~MotorVehicleDefClass()
    {
        //REF_PTR_RELEASE(EngineTorqueCurve);
    }

    // From PersistClass
    public override uint Get_Class_ID() => ClassId.CLASSID_MOTORVEHICLEDEF;

    // From PhysDefClass
    public override string Get_Type_Name() { return "MotorVehicleDef"; }
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

        csave.Begin_Chunk(MOTORVEHICLEDEF_CHUNK_VEHICLEPHYSDEF);
        base.Save(csave);
        csave.End_Chunk();

        ShiftUpAvel = MotorVehicle.RPM_TO_RADS(ShiftUpRpm);
        ShiftDownAvel = MotorVehicle.RPM_TO_RADS(ShiftDownRpm);

        csave.Begin_Chunk(MOTORVEHICLEDEF_CHUNK_VARIABLES);
        csave.WriteMicro(MOTORVEHICLEDEF_VARIABLE_MAXENGINETORQUE, MaxEngineTorque);
        ArgumentNullException.ThrowIfNull(EngineTorqueCurveFilename);
        csave.WriteMicroString(MOTORVEHICLEDEF_VARIABLE_ENGINETORQUECURVEFILENAME, EngineTorqueCurveFilename);
        csave.WriteMicro(MOTORVEHICLEDEF_VARIABLE_GEARCOUNT, GearCount);
        csave.WriteMicro(MOTORVEHICLEDEF_VARIABLE_GEARRATIO1, GearRatio[0]);
        csave.WriteMicro(MOTORVEHICLEDEF_VARIABLE_GEARRATIO2, GearRatio[1]);
        csave.WriteMicro(MOTORVEHICLEDEF_VARIABLE_GEARRATIO3, GearRatio[2]);
        csave.WriteMicro(MOTORVEHICLEDEF_VARIABLE_GEARRATIO4, GearRatio[3]);
        csave.WriteMicro(MOTORVEHICLEDEF_VARIABLE_GEARRATIO5, GearRatio[4]);
        csave.WriteMicro(MOTORVEHICLEDEF_VARIABLE_GEARRATIO6, GearRatio[5]);
        csave.WriteMicro(MOTORVEHICLEDEF_VARIABLE_FINALDRIVEGEARRATIO, FinalDriveGearRatio);
        csave.WriteMicro(MOTORVEHICLEDEF_VARIABLE_SHIFTUPRPM, ShiftUpRpm);
        csave.WriteMicro(MOTORVEHICLEDEF_VARIABLE_SHIFTDOWNRPM, ShiftDownRpm);
        csave.WriteMicro(MOTORVEHICLEDEF_VARIABLE_DRIVETRAININERTIA, DriveTrainInertia);
        csave.End_Chunk();
        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {

            switch (cload.Cur_Chunk_ID)
            {

                case MOTORVEHICLEDEF_CHUNK_RIGIDBODYDEF:        // old parent class
                    RigidBodyDefClassLoad(cload);
                    break;

                case MOTORVEHICLEDEF_CHUNK_VEHICLEPHYSDEF:  // current parent class
                    base.Load(cload);
                    break;

                case MOTORVEHICLEDEF_CHUNK_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case MOTORVEHICLEDEF_VARIABLE_MAXENGINETORQUE:
                                cload.Read(ref MaxEngineTorque);
                                break;
                            case MOTORVEHICLEDEF_VARIABLE_ENGINETORQUECURVEFILENAME:
                                cload.ReadMicroChunkWWString(out EngineTorqueCurveFilename);
                                break;
                            case MOTORVEHICLEDEF_VARIABLE_GEARCOUNT:
                                cload.Read(ref GearCount);
                                break;
                            case MOTORVEHICLEDEF_VARIABLE_GEARRATIO1:
                                cload.Read(ref GearRatio[0]);
                                break;
                            case MOTORVEHICLEDEF_VARIABLE_GEARRATIO2:
                                cload.Read(ref GearRatio[1]);
                                break;
                            case MOTORVEHICLEDEF_VARIABLE_GEARRATIO3:
                                cload.Read(ref GearRatio[2]);
                                break;
                            case MOTORVEHICLEDEF_VARIABLE_GEARRATIO4:
                                cload.Read(ref GearRatio[3]);
                                break;
                            case MOTORVEHICLEDEF_VARIABLE_GEARRATIO5:
                                cload.Read(ref GearRatio[4]);
                                break;
                            case MOTORVEHICLEDEF_VARIABLE_GEARRATIO6:
                                cload.Read(ref GearRatio[5]);
                                break;
                            case MOTORVEHICLEDEF_VARIABLE_FINALDRIVEGEARRATIO:
                                cload.Read(ref FinalDriveGearRatio);
                                break;
                            //OBSOLETE_MICRO_CHUNK(OBSOLETE_MOTORVEHICLEDEF_VARIABLE_ENGINEINERTIA);
                            //OBSOLETE_MICRO_CHUNK(OBSOLETE_MOTORVEHICLEDEF_VARIABLE_TRANSMISSIONINERTIA);
                            //OBSOLETE_MICRO_CHUNK(OBSOLETE_MOTORVEHICLEDEF_VARIABLE_FINALDRIVEINERTIA);
                            case MOTORVEHICLEDEF_VARIABLE_SHIFTUPRPM:
                                cload.Read(ref ShiftUpRpm);
                                break;
                            case MOTORVEHICLEDEF_VARIABLE_SHIFTDOWNRPM:
                                cload.Read(ref ShiftDownRpm);
                                break;
                            case MOTORVEHICLEDEF_VARIABLE_DRIVETRAININERTIA:
                                cload.Read(ref DriveTrainInertia);
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine($"Unhandled Chunk: {cload.Cur_Chunk_ID}");
                    break;
            }

            cload.Close_Chunk();
        }

        ShiftUpAvel = MotorVehicle.RPM_TO_RADS(ShiftUpRpm);
        ShiftDownAvel = MotorVehicle.RPM_TO_RADS(ShiftDownRpm);

        //REF_PTR_RELEASE(EngineTorqueCurve);
        if (!string.IsNullOrEmpty(EngineTorqueCurveFilename))
        {
            string filepath = Path.GetFileName(EngineTorqueCurveFilename);

            EngineTorqueCurve = LookupTableMgrClass.Get_Table(filepath);
        }
        if (EngineTorqueCurve == null)
        {
            Console.WriteLine($"Missing EngineTorqueCurve Table file: {EngineTorqueCurveFilename}");
            EngineTorqueCurve = LookupTableMgrClass.Get_Table("DefaultTable");
        }

        return true;
    }

    // Read/Write access (DEBUGGING/TESTING ONLY)
    public float Get_Max_Engine_Torque() { return MaxEngineTorque; }
    public int Get_Gear_Count() { return GearCount; }
    public float Get_Gear_Ratio(int gear) { return GearRatio[gear]; }
    public float Get_Shift_Up_Rpm() { return ShiftUpRpm; }
    public float Get_Shift_Down_Rpm() { return ShiftDownRpm; }

    public void Set_Max_Engine_Torque(float t) { MaxEngineTorque = t; }
    public void Set_Gear_Count(int count) { if ((count >= 0) && (count <= 5)) GearCount = count; }
    public void Set_Gear_Ratio(int gear, float ratio) { if ((gear >= 0) && (gear <= 5)) GearRatio[gear] = ratio; }
    public void Set_Shift_Up_Rpm(float rpm) { ShiftUpRpm = rpm; ShiftUpAvel = MotorVehicle.RPM_TO_RADS(ShiftUpRpm); }
    public void Set_Shift_Down_Rpm(float rpm) { ShiftDownRpm = rpm; ShiftDownAvel = MotorVehicle.RPM_TO_RADS(ShiftDownRpm); }

    //	Editable interface requirements
    //DECLARE_EDITABLE(MotorVehicleDefClass, VehiclePhysDefClass);


    // Engine response
    protected float MaxEngineTorque;
    protected string EngineTorqueCurveFilename;
    protected LookupTableClass? EngineTorqueCurve;           // pointer to the torque curve 

    // Transmission and Differential Gear Ratios
    protected const int MAX_GEAR_COUNT = 6;

    protected int GearCount;
    protected float[] GearRatio = new float[MAX_GEAR_COUNT];
    protected float[] PeakEngineTorque = new float[MAX_GEAR_COUNT];
    protected float FinalDriveGearRatio;

    // Inertias in the drive train
    protected float DriveTrainInertia;

    // Shifting variables
    protected float ShiftUpRpm;
    protected float ShiftDownRpm;
    protected float ShiftUpAvel;        // same as ShiftUpRpm but in radians per second
    protected float ShiftDownAvel;		// same as ShiftDownRpm but in radians per second

    private const int MOTORVEHICLEDEF_CHUNK_RIGIDBODYDEF = 0x00516000;
    private const int MOTORVEHICLEDEF_CHUNK_VARIABLES = 0x00516001;
    private const int MOTORVEHICLEDEF_CHUNK_VEHICLEPHYSDEF = 0x00516002;

    private const int MOTORVEHICLEDEF_VARIABLE_MAXENGINETORQUE = 0;
    private const int MOTORVEHICLEDEF_VARIABLE_ENGINETORQUECURVEFILENAME = 1;

    private const int MOTORVEHICLEDEF_VARIABLE_GEARCOUNT = 2;
    private const int MOTORVEHICLEDEF_VARIABLE_GEARRATIO1 = 3;
    private const int MOTORVEHICLEDEF_VARIABLE_GEARRATIO2 = 4;
    private const int MOTORVEHICLEDEF_VARIABLE_GEARRATIO3 = 5;
    private const int MOTORVEHICLEDEF_VARIABLE_GEARRATIO4 = 6;
    private const int MOTORVEHICLEDEF_VARIABLE_GEARRATIO5 = 7;
    private const int MOTORVEHICLEDEF_VARIABLE_GEARRATIO6 = 8;
    private const int MOTORVEHICLEDEF_VARIABLE_FINALDRIVEGEARRATIO = 9;

    private const int OBSOLETE_MOTORVEHICLEDEF_VARIABLE_ENGINEINERTIA = 10;
    private const int OBSOLETE_MOTORVEHICLEDEF_VARIABLE_TRANSMISSIONINERTIA = 11;
    private const int OBSOLETE_MOTORVEHICLEDEF_VARIABLE_FINALDRIVEINERTIA = 12;

    private const int MOTORVEHICLEDEF_VARIABLE_SHIFTUPRPM = 13;
    private const int MOTORVEHICLEDEF_VARIABLE_SHIFTDOWNRPM = 14;
    private const int MOTORVEHICLEDEF_VARIABLE_DRIVETRAININERTIA = 15;

};
