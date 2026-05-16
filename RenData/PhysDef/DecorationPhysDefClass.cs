using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.PhysDef;

[RegisterDefinition(ChunkId.PHYSICS_CHUNKID_DECOPHYSDEF)]
public partial class DecorationPhysDefClass : DynamicPhysDefClass
{
    public DecorationPhysDefClass() { }

    public override uint Get_Class_ID() => ClassId.CLASSID_DECOPHYSDEF;
    public override PersistClass Create()
    {
        //DecorationPhysClass new_obj = NEW_REF(DecorationPhysClass, ());
        //new_obj.Init(this);
        //return new_obj;
        throw new NotImplementedException();
    }

    // From PhysDefClass
    public override string Get_Type_Name() => "DecorationPhysDef";
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
        csave.Begin_Chunk(DECORATIONPHYSDEF_CHUNK_DYNAMICPHYSDEF);
        base.Save(csave);
        csave.End_Chunk();

        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {
                case DECORATIONPHYSDEF_CHUNK_PHYSDEF:
                    PhysDefClassLoad(cload);
                    break;

                case DECORATIONPHYSDEF_CHUNK_DYNAMICPHYSDEF:
                    base.Load(cload);
                    break;

                default:
                    Console.WriteLine("Unhandled Chunk: {cload.Cur_Chunk_ID} ");
                    break;
            }

            cload.Close_Chunk();
        }
        return true;
    }

    //	Editable interface requirements
    //DECLARE_EDITABLE(DecorationPhysDefClass, DynamicPhysDefClass);

    private const int DECORATIONPHYSDEF_CHUNK_PHYSDEF = 0x01070003;
    private const int DECORATIONPHYSDEF_CHUNK_DYNAMICPHYSDEF = 0x01070004;
};

//SimplePersistFactoryClass<DecorationPhysDefClass, PHYSICS_CHUNKID_DECOPHYSDEF>	_DecorationPhysDefFactory;
//DECLARE_DEFINITION_FACTORY(DecorationPhysDefClass, CLASSID_DECOPHYSDEF, "DecorationPhys") _DecorationPhysDefDefFactory;

