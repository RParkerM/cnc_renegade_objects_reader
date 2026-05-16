using RenData.ChunkIO;

namespace RenData.PhysDef;

public abstract class MoveablePhysDefClass : DynamicPhysDefClass
{
    public MoveablePhysDefClass()
    {
        Mass = 1.0f;
        GravScale = 1.0f;
        Elasticity = 0.1f;
        CinematicCollisionMode = CINEMATIC_COLLISION_PUSH;
        //    // make our parameters editable!
        //    FLOAT_EDITABLE_PARAM(MoveablePhysDefClass, Mass, 0.01f, 100000.0f);
        //    FLOAT_EDITABLE_PARAM(MoveablePhysDefClass, GravScale, 0.0f, 10.0f);
        //    FLOAT_EDITABLE_PARAM(MoveablePhysDefClass, Elasticity, 0.0f, 1.0f);

        //# ifdef PARAM_EDITING_ON
        //    EnumParameterClass cinematic_param = new EnumParameterClass(CinematicCollisionMode);
        //    cinematic_param.Set_Name("CinematicCollisionMode");
        //    cinematic_param.Add_Value("NONE", CINEMATIC_COLLISION_NONE);
        //    cinematic_param.Add_Value("STOP", CINEMATIC_COLLISION_STOP);
        //    cinematic_param.Add_Value("PUSH", CINEMATIC_COLLISION_PUSH);
        //    cinematic_param.Add_Value("KILL", CINEMATIC_COLLISION_KILL);
        //    GENERIC_EDITABLE_PARAM(MoveablePhysDefClass, cinematic_param);
        //#endif

    }
    public override string Get_Type_Name() { return "MoveablePhysDef"; }
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
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(MOVEABLEPHYSDEF_CHUNK_DYNAMICPHYSDEF);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(MOVEABLEPHYSDEF_CHUNK_VARIABLES);
        csave.WriteMicro(MOVEABLEPHYSDEF_VARIABLE_MASS, Mass);
        csave.WriteMicro(MOVEABLEPHYSDEF_VARIABLE_GRAVSCALE, GravScale);
        csave.WriteMicro(MOVEABLEPHYSDEF_VARIABLE_ELASTICITY, Elasticity);
        csave.WriteMicro(MOVEABLEPHYSDEF_VARIABLE_CINEMATICCOLLISIONMODE, CinematicCollisionMode);
        csave.End_Chunk();
        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {

            switch (cload.Cur_Chunk_ID)
            {

                case MOVEABLEPHYSDEF_CHUNK_PHYSDEF:
                    PhysDefClassLoad(cload);
                    break;

                case MOVEABLEPHYSDEF_CHUNK_DYNAMICPHYSDEF:
                    base.Load(cload);
                    break;

                case MOVEABLEPHYSDEF_CHUNK_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case MOVEABLEPHYSDEF_VARIABLE_MASS:
                                cload.Read(ref Mass);
                                break;
                            case MOVEABLEPHYSDEF_VARIABLE_GRAVSCALE:
                                cload.Read(ref GravScale);
                                break;
                            case MOVEABLEPHYSDEF_VARIABLE_ELASTICITY:
                                cload.Read(ref Elasticity);
                                break;
                            case MOVEABLEPHYSDEF_VARIABLE_CINEMATICCOLLISIONMODE:
                                cload.Read(ref CinematicCollisionMode);
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

    // In-Game Editing (DEBUGGING/TESTING ONLY)
    public float Get_Mass() { return Mass; }
    public float Get_Grav_Scale() { return GravScale; }
    public void Set_Mass(float new_mass) { Mass = new_mass; }
    public void Set_Grav_Scale(float new_g) { GravScale = new_g; }

    //	Editable interface requirements
    //DECLARE_EDITABLE(MoveablePhysDefClass, DynamicPhysDefClass);

    protected float Mass;
    protected float GravScale;
    protected float Elasticity;

    protected const int CINEMATIC_COLLISION_NONE = 0;
    protected const int CINEMATIC_COLLISION_STOP = 1;
    protected const int CINEMATIC_COLLISION_PUSH = 2;
    protected const int CINEMATIC_COLLISION_KILL = 3;

    protected int CinematicCollisionMode;

    private const int MOVEABLEPHYSDEF_CHUNK_PHYSDEF = 0x04486000;
    private const int MOVEABLEPHYSDEF_CHUNK_VARIABLES = 0x04486001;
    private const int MOVEABLEPHYSDEF_CHUNK_DYNAMICPHYSDEF = 0x04486002;

    private const int MOVEABLEPHYSDEF_VARIABLE_MASS = 0x00;
    private const int MOVEABLEPHYSDEF_VARIABLE_GRAVSCALE = 0x01;
    private const int MOVEABLEPHYSDEF_VARIABLE_ELASTICITY = 0x02;
    private const int MOVEABLEPHYSDEF_VARIABLE_CINEMATICCOLLISIONMODE = 0x03;

};
