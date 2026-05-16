using RenData.ChunkIO;
using RenData.SaveLoad;
using RenData.IDs;
using System;
using System.Numerics;
using RenData.Phys;

namespace RenData.PhysDef;

[RegisterDefinition(ChunkId.PHYSICS_CHUNKID_PROJECTILEDEF)]
public partial class ProjectileDefClass : MoveablePhysDefClass
{
    public ProjectileDefClass()
    {
        CollidesOnMove = true;
        OrientationMode = ProjectileClass.ORIENTATION_ALIGNED;
        TumbleAxis = new(1, 2, 1);
        TumbleRate = WWMath.DEG_TO_RADF(10.0f);
        Lifetime = 2.0f;
        BounceCount = 0;
        //# ifdef PARAM_EDITING_ON
        //    // make our parameters editable!
        //    //EDITABLE_PARAM(ProjectileDefClass, ParameterClass.TYPE_BOOL, CollidesOnMove);

        //    EnumParameterClass param = new EnumParameterClass(OrientationMode);
        //    param.Set_Name("OrientationMode");
        //    param.Add_Value("ALIGNED", ProjectileClass.ORIENTATION_ALIGNED);
        //    param.Add_Value("FIXED", ProjectileClass.ORIENTATION_FIXED);
        //    param.Add_Value("TUMBLE", ProjectileClass.ORIENTATION_TUMBLING);
        //    GENERIC_EDITABLE_PARAM(ProjectileDefClass, param)

        //        //FLOAT_EDITABLE_PARAM(ProjectileDefClass, TumbleAxis.X, 0.0f, 10.0f);
        //        //FLOAT_EDITABLE_PARAM(ProjectileDefClass, TumbleAxis.Y, 0.0f, 10.0f);
        //        //FLOAT_EDITABLE_PARAM(ProjectileDefClass, TumbleAxis.Z, 0.0f, 10.0f);
        //    ANGLE_EDITABLE_PARAM(ProjectileDefClass, TumbleRate, DEG_TO_RADF(0.1f), DEG_TO_RADF(90.0f));
        //    //FLOAT_EDITABLE_PARAM(ProjectileDefClass, Lifetime, 0.01f, 100.0f);
        //    //INT_EDITABLE_PARAM(ProjectileDefClass, BounceCount, 0, 32);
        //#endif
    }

    // From Definition
    public override uint Get_Class_ID() => ClassId.CLASSID_PROJECTILEDEF;
    public override PersistClass Create()
    {
        //ProjectileClass obj = NEW_REF(ProjectileClass, ());
        //obj.Init(this);
        //return obj;
        throw new NotImplementedException();
    }

    // From PhysDefClass
    public override string Get_Type_Name() { return "ProjectileDef"; }
    public override bool Is_Type(string type_name)
    {
        // merged implementation from original free function
        if (string.Equals(type_name, Get_Type_Name(), StringComparison.Ordinal))
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
        csave.Begin_Chunk(PROJECTILEDEF_CHUNK_MOVEABLEPHYSDEF);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(PROJECTILEDEF_CHUNK_VARIABLES);
        csave.WriteMicro(PROJECTILEDEF_VARIABLE_COLLIDESONMOVE, CollidesOnMove);
        csave.WriteMicro(PROJECTILEDEF_VARIABLE_ORIENTATIONMODE, OrientationMode);
        csave.WriteMicro(PROJECTILEDEF_VARIABLE_TUMBLEAXIS, TumbleAxis);
        csave.WriteMicro(PROJECTILEDEF_VARIABLE_TUMBLERATE, TumbleRate);
        csave.WriteMicro(PROJECTILEDEF_VARIABLE_LIFETIME, Lifetime);
        csave.WriteMicro(PROJECTILEDEF_VARIABLE_BOUNCECOUNT, BounceCount);
        csave.End_Chunk();
        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {

            switch (cload.Cur_Chunk_ID)
            {

                case PROJECTILEDEF_CHUNK_MOVEABLEPHYSDEF:
                    base.Load(cload);
                    break;

                case PROJECTILEDEF_CHUNK_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case PROJECTILEDEF_VARIABLE_COLLIDESONMOVE:
                                cload.Read(ref CollidesOnMove);
                                break;
                            case PROJECTILEDEF_VARIABLE_ORIENTATIONMODE:
                                cload.Read(ref OrientationMode);
                                break;
                            case PROJECTILEDEF_VARIABLE_TUMBLEAXIS:
                                cload.Read(ref TumbleAxis);
                                break;
                            case PROJECTILEDEF_VARIABLE_TUMBLERATE:
                                cload.Read(ref TumbleRate);
                                break;
                            case PROJECTILEDEF_VARIABLE_LIFETIME:
                                cload.Read(ref Lifetime);
                                break;
                            case PROJECTILEDEF_VARIABLE_BOUNCECOUNT:
                                cload.Read(ref BounceCount);
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unhandled Chunk: 0x{0:X8} (depth {1}) in {2}", cload.Cur_Chunk_ID, cload.Cur_Chunk_Depth, nameof(ProjectileDefClass));
                    break;
            }

            cload.Close_Chunk();
        }
        return true;
    }

    //	Editable interface requirements
    //DECLARE_EDITABLE(ProjectileDefClass, MoveablePhysDefClass);


	protected bool CollidesOnMove;
    protected int OrientationMode;
    protected Vector3 TumbleAxis;
    protected float TumbleRate;
    protected float Lifetime;
    protected int BounceCount;

	// Replaced C-style enum with private const ints scoped to the class
	private const int PROJECTILEDEF_CHUNK_MOVEABLEPHYSDEF = 0x01210011; // (parent class)
	private const int PROJECTILEDEF_CHUNK_VARIABLES = 0x01210012;

	private const int PROJECTILEDEF_VARIABLE_COLLIDESONMOVE = 0x00;
	private const int PROJECTILEDEF_VARIABLE_ORIENTATIONMODE = 0x01;
	private const int PROJECTILEDEF_VARIABLE_TUMBLEAXIS = 0x02;
	private const int PROJECTILEDEF_VARIABLE_TUMBLERATE = 0x03;
	private const int PROJECTILEDEF_VARIABLE_LIFETIME = 0x04;
	private const int PROJECTILEDEF_VARIABLE_BOUNCECOUNT = 0x05;
}
