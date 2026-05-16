using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;
using System.Threading;

namespace RenData.PhysDef;

[RegisterDefinition(ChunkId.PHYSICS_CHUNKID_RIGIDBODYDEF)]
public partial class RigidBodyDefClass : MoveablePhysDefClass
{
    public RigidBodyDefClass()
    {
        AerodynamicDragCoefficient = 0.0f;
        CollisionDisabled = false;
        // make our parameters editable
        //FLOAT_EDITABLE_PARAM(RigidBodyDefClass, AerodynamicDragCoefficient, 0.0f, 100.0f);
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_RIGIDBODYDEF;

    public override PersistClass Create()
    {
        //RigidBodyClass obj = NEW_REF(RigidBodyClass, ());
        //obj.Init(this);
        //return obj;
        throw new NotImplementedException();
    }

    // From PhysDefClass
    public override string Get_Type_Name() { return "RigidBodyDef"; }
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
        csave.Begin_Chunk(RIGIDBODYDEF_CHUNK_MOVEABLEPHYSDEF);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(RIGIDBODYDEF_CHUNK_VARIABLES);
        csave.WriteMicro(RIGIDBODYDEF_VARIABLE_AERODYNAMICDRAGCOEFFICIENT, AerodynamicDragCoefficient);
        csave.WriteMicro(RIGIDBODYDEF_VARIABLE_COLLISIONDISABLED, CollisionDisabled);
        csave.End_Chunk();
        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {

            switch (cload.Cur_Chunk_ID)
            {

                case RIGIDBODYDEF_CHUNK_MOVEABLEPHYSDEF:
                    base.Load(cload);
                    break;

                case RIGIDBODYDEF_CHUNK_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case RIGIDBODYDEF_VARIABLE_AERODYNAMICDRAGCOEFFICIENT:
                                cload.Read(ref AerodynamicDragCoefficient);
                                break;
                            case RIGIDBODYDEF_VARIABLE_COLLISIONDISABLED:
                                cload.Read(ref CollisionDisabled);
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
    public float Get_Aerodynamic_Drag() { return AerodynamicDragCoefficient; }
    public void Set_Aerodynamic_Drag(float new_drag) { AerodynamicDragCoefficient = new_drag; }

    //	Editable interface requirements
    //DECLARE_EDITABLE(RigidBodyDefClass, MoveablePhysDefClass);


    protected float AerodynamicDragCoefficient;
    protected bool CollisionDisabled;

    private const int RIGIDBODYDEF_CHUNK_MOVEABLEPHYSDEF = 0x01106650;            // (parent class)
    private const int RIGIDBODYDEF_CHUNK_VARIABLES = 0x01106651;

    private const int RIGIDBODYDEF_VARIABLE_AERODYNAMICDRAGCOEFFICIENT = 0x00;
    private const int RIGIDBODYDEF_VARIABLE_COLLISIONDISABLED = 0x01;
};


