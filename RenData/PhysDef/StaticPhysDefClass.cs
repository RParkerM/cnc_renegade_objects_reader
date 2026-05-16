using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;
using System.Reflection.Metadata;

namespace RenData.PhysDef;

[RegisterDefinition(ChunkId.PHYSICS_CHUNKID_STATICPHYSDEF)]
public partial class StaticPhysDefClass : PhysDefClass
{
    public StaticPhysDefClass()
    {
        IsNonOccluder = true;
        //EDITABLE_PARAM(StaticPhysDefClass, ParameterClass.TYPE_BOOL, IsNonOccluder);
    }

    // From DefinitionClass
    public override uint Get_Class_ID() => ClassId.CLASSID_STATICPHYSDEF;
    public override PersistClass Create()
    {
        //StaticPhysClass new_obj = NEW_REF(StaticPhysClass, ());
        //new_obj.Init(this);
        //return new_obj;
        throw new NotImplementedException();
    }

    // From PhysDefClass
    public override string Get_Type_Name() => "StaticPhysDef";
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
        csave.Begin_Chunk(STATICPHYSDEF_CHUNK_PHYSDEF);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(STATICPHYSDEF_CHUNK_VARIABLES);
        csave.WriteMicro(STATICPHYSDEF_VARIABLE_ISNONOCCLUDER, IsNonOccluder);
        csave.End_Chunk();
        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {

            switch (cload.Cur_Chunk_ID)
            {

                case STATICPHYSDEF_CHUNK_PHYSDEF:
                    base.Load(cload);
                    break;

                case STATICPHYSDEF_CHUNK_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case STATICPHYSDEF_VARIABLE_ISNONOCCLUDER:
                                cload.Read(ref IsNonOccluder);
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unhandled Chunk: 0x{0:X8} (depth {1}) in {2}", cload.Cur_Chunk_ID, cload.Cur_Chunk_Depth, nameof(StaticPhysDefClass));
                    break;
            }

            cload.Close_Chunk();
        }
        return true;
    }

    //	Editable interface requirements
    //DECLARE_EDITABLE(StaticPhysDefClass, PhysDefClass);

    protected bool IsNonOccluder;

    private const int STATICPHYSDEF_CHUNK_PHYSDEF = 0x01070002;
    private const int STATICPHYSDEF_CHUNK_VARIABLES = 0x01070003;

    private const int STATICPHYSDEF_VARIABLE_ISNONOCCLUDER = 0;
};

