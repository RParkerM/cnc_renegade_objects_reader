using RenData.ChunkIO;
using RenData.SaveLoad;
using RenData.IDs;

namespace RenData.PhysDef;

[RegisterDefinition(ChunkId.PHYSICS_CHUNKID_WHEELEDVEHICLEDEF)]
public partial class WheeledVehicleDefClass : MotorVehicleDefClass
{
    public WheeledVehicleDefClass() { }

    public override uint Get_Class_ID() => ClassId.CLASSID_WHEELEDVEHICLEDEF;
    public override PersistClass Create()
    {
        //WheeledVehicleClass obj = NEW_REF(WheeledVehicleClass, ());
        //obj.Init(this);
        //return obj;
        throw new NotImplementedException();
    }

    public override string Get_Type_Name() { return "WheeledVehicleDef"; }
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
        csave.Begin_Chunk(WHEELEDVEHICLEDEF_CHUNK_MOTORVEHICLEDEF);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(WHEELEDVEHICLEDEF_CHUNK_VARIABLES);
        csave.WriteMicro(WHEELEDVEHICLEDEF_VARIABLE_MAXSTEERINGANGLE, MaxSteeringAngle);
        csave.End_Chunk();

        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {

            switch (cload.Cur_Chunk_ID)
            {

                case WHEELEDVEHICLEDEF_CHUNK_MOTORVEHICLEDEF:
                    base.Load(cload);
                    break;

                case WHEELEDVEHICLEDEF_CHUNK_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {

                            case WHEELEDVEHICLEDEF_VARIABLE_MAXSTEERINGANGLE:
                                cload.Read(ref MaxSteeringAngle);
                                break;

                            // NOTE: these variables are now in VehiclePhysDefClass...
                            case OBSOLETE_WHEELEDVEHICLEDEF_VARIABLE_SPRINGCONSTANT:
                                cload.Read(ref SpringConstant);
                                break;
                            case OBSOLETE_WHEELEDVEHICLEDEF_VARIABLE_DAMPINGCONSTANT:
                                cload.Read(ref DampingConstant);
                                break;
                            case OBSOLETE_WHEELEDVEHICLEDEF_VARIABLE_SPRINGLENGTH:
                                cload.Read(ref SpringLength);
                                break;
                            case OBSOLETE_WHEELEDVEHICLEDEF_VARIABLE_TRACTIONMULTIPLIER:
                                cload.Read(ref TractionMultiplier);
                                break;
                            case OBSOLETE_WHEELEDVEHICLEDEF_VARIABLE_LATERALMOMENTARM:
                                cload.Read(ref LateralMomentArm);
                                break;
                            case OBSOLETE_WHEELEDVEHICLEDEF_VARIABLE_TRACTIVEMOMENTARM:
                                cload.Read(ref TractiveMomentArm);
                                break;

                                //OBSOLETE_MICRO_CHUNK(OBSOLETE_WHEELEDVEHICLEDEF_VARIABLE_TRACTIVEFRICTIONCURVEFILENAME);
                                //OBSOLETE_MICRO_CHUNK(OBSOLETE_WHEELEDVEHICLEDEF_VARIABLE_LATERALFRICTIONCURVEFILENAME);
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

        return true;
    }

    // Read/Write access to our variables
    public float Get_Max_Steering_Angle() { return MaxSteeringAngle; }
    public void Set_Max_Steering_Angle(float a) { MaxSteeringAngle = a; }

    //	Editable interface requirements
    //DECLARE_EDITABLE(WheeledVehicleDefClass, MotorVehicleDefClass);

    protected float MaxSteeringAngle;           // maximum angle for the steering wheels

    // Replaced enum with private const ints
    private const int WHEELEDVEHICLEDEF_CHUNK_MOTORVEHICLEDEF = 0x00990066; // (parent class)
    private const int WHEELEDVEHICLEDEF_CHUNK_VARIABLES = 0x00990067;

    private const int WHEELEDVEHICLEDEF_VARIABLE_MAXSTEERINGANGLE = 0x00;
    private const int OBSOLETE_WHEELEDVEHICLEDEF_VARIABLE_SPRINGCONSTANT = 0x01;
    private const int OBSOLETE_WHEELEDVEHICLEDEF_VARIABLE_DAMPINGCONSTANT = 0x02;
    private const int OBSOLETE_WHEELEDVEHICLEDEF_VARIABLE_SPRINGLENGTH = 0x03;
    private const int OBSOLETE_WHEELEDVEHICLEDEF_VARIABLE_TRACTIVEFRICTIONCURVEFILENAME = 0x04;
    private const int OBSOLETE_WHEELEDVEHICLEDEF_VARIABLE_LATERALFRICTIONCURVEFILENAME = 0x05;
    private const int OBSOLETE_WHEELEDVEHICLEDEF_VARIABLE_TRACTIONMULTIPLIER = 0x06;
    private const int OBSOLETE_WHEELEDVEHICLEDEF_VARIABLE_LATERALMOMENTARM = 0x07;
    private const int OBSOLETE_WHEELEDVEHICLEDEF_VARIABLE_TRACTIVEMOMENTARM = 0x08;
};
