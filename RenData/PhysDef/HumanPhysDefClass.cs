using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.PhysDef;

[RegisterDefinition(ChunkId.PHYSICS_CHUNKID_HUMANPHYSDEF)]
public partial class HumanPhysDefClass : Phys3DefClass
{
    public HumanPhysDefClass() { }

    public override uint Get_Class_ID() => ClassId.CLASSID_HUMANPHYSDEF;
    public override PersistClass Create()
    {
        //HumanPhysClass obj = NEW_REF(HumanPhysClass, ());
        //obj.Init(this);
        //return obj;
        throw new NotImplementedException();
    }
    public override string Get_Type_Name() { return "HumanPhysDef"; }
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
        csave.Begin_Chunk(HUMANPHYSDEF_CHUNK_PHYS3DEF);
        base.Save(csave);
        csave.End_Chunk();

        // no variables for now
        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {
                case HUMANPHYSDEF_CHUNK_PHYS3DEF:
                    base.Load(cload);
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
    //DECLARE_EDITABLE(HumanPhysDefClass, Phys3DefClass);

    private const int HUMANPHYSDEF_CHUNK_PHYS3DEF = 0x00516000;
};
