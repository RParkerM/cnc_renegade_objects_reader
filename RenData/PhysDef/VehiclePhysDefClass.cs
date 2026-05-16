using RenData.ChunkIO;
using RenData.SaveLoad;
using RenData.IDs;

namespace RenData.PhysDef;

[RegisterDefinition(ChunkId.PHYSICS_CHUNKID_VEHICLEPHYSDEF)]
public partial class VehiclePhysDefClass : RigidBodyDefClass
{
    public VehiclePhysDefClass()
    {
        SpringConstant = Wheel.DEFAULT_SPRING_CONSTANT;
        DampingConstant = Wheel.DEFAULT_DAMPING_COEFFICIENT;
        SpringLength = Wheel.DEFAULT_SPRING_LENGTH;
        TractionMultiplier = 2.0f;
        LateralMomentArm = 0.0f;
        TractiveMomentArm = 0.0f;
        EngineFlameLength = 1.0f;
        IsFake = false;
        // make our parameters editable!
        //EDITABLE_PARAM(VehiclePhysDefClass, ParameterClass.TYPE_BOOL, IsFake);
        //FLOAT_UNITS_PARAM(VehiclePhysDefClass, SpringConstant, 0.0f, 100000.0f, "N/m");
        //FLOAT_UNITS_PARAM(VehiclePhysDefClass, DampingConstant, 0.0f, 100000.0f, "N/(m/s)");
        //FLOAT_UNITS_PARAM(VehiclePhysDefClass, SpringLength, 0.0f, 100.0f, "m");
        //FLOAT_EDITABLE_PARAM(VehiclePhysDefClass, TractionMultiplier, 0.5f, 5.0f);
        //FLOAT_UNITS_PARAM(VehiclePhysDefClass, LateralMomentArm, 0.0f, 10.0f, "m");
        //FLOAT_UNITS_PARAM(VehiclePhysDefClass, TractiveMomentArm, 0.0f, 10.0f, "m");
        //FLOAT_UNITS_PARAM(VehiclePhysDefClass, EngineFlameLength, 0.0f, 100.0f, "m");

    }

    // From DefinitionClass
    public override uint Get_Class_ID() => ClassId.CLASSID_VEHICLEPHYSDEF;

    // From PhysDefClass
    public override string Get_Type_Name() { return "VehiclePhysDef"; }
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

    // Read access to the ants
    public float Get_Spring_Constant() { return SpringConstant; }
    public float Get_Damping_Constant() { return DampingConstant; }
    public float Get_Spring_Length() { return SpringLength; }
    public float Get_Traction_Multiplier() { return TractionMultiplier; }
    public float Get_Lateral_Moment_Arm() { return LateralMomentArm; }
    public float Get_Tractive_Moment_Arm() { return TractiveMomentArm; }
    public bool Is_Fake() { return IsFake; }

    // Write access (DEBUGGING/TESTING ONLY)
    public void Set_Spring_Constant(float ks) { SpringConstant = ks; }
    public void Set_Damping_Constant(float kd) { DampingConstant = kd; }
    public void Set_Spring_Length(float l) { SpringLength = l; }
    public void Set_Traction_Multiplier(float k) { TractionMultiplier = k; }
    public void Set_Lateral_Moment_Arm(float r) { LateralMomentArm = r; }
    public void Set_Tractive_Moment_Arm(float r) { TractiveMomentArm = r; }


    // Save/Load support from PersistClass
    public override PersistFactoryClass Get_Factory() => _persistFactory;
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(VEHICLEPHYSDEF_CHUNK_RIGIDBODYDEF);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(VEHICLEPHYSDEF_CHUNK_VARIABLES);
        csave.WriteMicro(VEHICLEPHYSDEF_VARIABLE_SPRINGCONSTANT, SpringConstant);
        csave.WriteMicro(VEHICLEPHYSDEF_VARIABLE_DAMPINGCONSTANT, DampingConstant);
        csave.WriteMicro(VEHICLEPHYSDEF_VARIABLE_SPRINGLENGTH, SpringLength);
        csave.WriteMicro(VEHICLEPHYSDEF_VARIABLE_TRACTIONMULTIPLIER, TractionMultiplier);
        csave.WriteMicro(VEHICLEPHYSDEF_VARIABLE_LATERALMOMENTARM, LateralMomentArm);
        csave.WriteMicro(VEHICLEPHYSDEF_VARIABLE_TRACTIVEMOMENTARM, TractiveMomentArm);
        csave.WriteMicro(VEHICLEPHYSDEF_VARIABLE_ENGINEFLAMELENGTH, EngineFlameLength);
        csave.WriteMicro(VEHICLEPHYSDEF_VARIABLE_ISFAKE, IsFake);
        csave.End_Chunk();
        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {

            switch (cload.Cur_Chunk_ID)
            {

                case VEHICLEPHYSDEF_CHUNK_RIGIDBODYDEF:
                    base.Load(cload);
                    break;

                case VEHICLEPHYSDEF_CHUNK_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case VEHICLEPHYSDEF_VARIABLE_SPRINGCONSTANT:
                                cload.Read(ref SpringConstant);
                                break;
                            case VEHICLEPHYSDEF_VARIABLE_DAMPINGCONSTANT:
                                cload.Read(ref DampingConstant);
                                break;
                            case VEHICLEPHYSDEF_VARIABLE_SPRINGLENGTH:
                                cload.Read(ref SpringLength);
                                break;
                            case VEHICLEPHYSDEF_VARIABLE_TRACTIONMULTIPLIER:
                                cload.Read(ref TractionMultiplier);
                                break;
                            case VEHICLEPHYSDEF_VARIABLE_LATERALMOMENTARM:
                                cload.Read(ref LateralMomentArm);
                                break;
                            case VEHICLEPHYSDEF_VARIABLE_TRACTIVEMOMENTARM:
                                cload.Read(ref TractiveMomentArm);
                                break;
                            case VEHICLEPHYSDEF_VARIABLE_ENGINEFLAMELENGTH:
                                cload.Read(ref EngineFlameLength);
                                break;
                            case VEHICLEPHYSDEF_VARIABLE_ISFAKE:
                                cload.Read(ref IsFake);
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unhandled Chunk: {cload.Cur_Chunk_ID}");
                    break;
            }

            cload.Close_Chunk();
        }

        return true;
    }

    //	Editable interface requirements
    //DECLARE_EDITABLE(VehiclePhysDefClass, RigidBodyDefClass);

    protected float SpringConstant;           // suspension spring ant
    protected float DampingConstant;          // suspension damping ant (shock absorber)
    protected float SpringLength;             // suspension spring length
    protected float TractionMultiplier;       // scales the downforce for more traction
    protected float LateralMomentArm;         // arbitrary dist from x-y plane to apply lateral tire forces
    protected float TractiveMomentArm;        // arbitrary dist from x-y plane to apply tractive tire forces
    protected float EngineFlameLength;        // max length of the engine flame (how far to translate at flame=1.0)
    protected bool IsFake;                      // short-circuit all possible physics calculations!	


    private const int VEHICLEPHYSDEF_CHUNK_RIGIDBODYDEF = 405001519;          // (parent class;
    private const int VEHICLEPHYSDEF_CHUNK_VARIABLES = 405001520;

    private const int VEHICLEPHYSDEF_VARIABLE_SPRINGCONSTANT = 0x00;
    private const int VEHICLEPHYSDEF_VARIABLE_DAMPINGCONSTANT = 0x01;
    private const int VEHICLEPHYSDEF_VARIABLE_SPRINGLENGTH = 0x02;
    private const int VEHICLEPHYSDEF_VARIABLE_TRACTIONMULTIPLIER = 0x03;
    private const int VEHICLEPHYSDEF_VARIABLE_LATERALMOMENTARM = 0x04;
    private const int VEHICLEPHYSDEF_VARIABLE_TRACTIVEMOMENTARM = 0x05;
    private const int VEHICLEPHYSDEF_VARIABLE_ENGINEFLAMELENGTH = 0x06;
    private const int VEHICLEPHYSDEF_VARIABLE_ISFAKE = 0x07;

    protected bool RigidBodyDefClassLoad(ChunkLoadClass cload) => base.Load(cload);
    protected bool RigidBodyDefClassSave(ChunkSaveClass csave) => base.Save(csave);
};
