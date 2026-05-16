using RenData.ChunkIO;
using RenData.SaveLoad;
using RenData.IDs;
using RenData.PhysDef;

namespace RenData.PhysDef;

[RegisterDefinition(ChunkId.PHYSICS_CHUNKID_ACCESSIBLEPHYSDEF)]
public partial class AccessiblePhysDefClass : StaticAnimPhysDefClass
{
    public AccessiblePhysDefClass()
    {
        LockCode = 0;
        //EDITABLE_PARAM(AccessiblePhysDefClass, ParameterClass.TYPE_INT, LockCode);
    }


    public override uint Get_Class_ID() => ClassId.CLASSID_ACCESSIBLEPHYSDEF;

    public override PersistClass Create()
    {
        // AccessiblePhysClass obj = NEW_REF(AccessiblePhysClass, ());
        // obj.Init(this);
        // return obj;
        throw new NotImplementedException();
    }


    public override string Get_Type_Name() { return "AccessiblePhysDefClass"; }
    public override bool Is_Type(string type_name)
    {
        bool retval = false;

        if (string.Compare(type_name, Get_Type_Name()) == 0) {
            retval = true;
        }

    else
        {
            retval = base.Is_Type(type_name);
        }

        return retval;
    }


    public override PersistFactoryClass Get_Factory() => _persistFactory;
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        Save_Variables(csave);
        csave.End_Chunk();
        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {

            switch (cload.Cur_Chunk_ID)
            {
                case CHUNKID_DEF_PARENT:
                    base.Load(cload);
                    break;

                case CHUNKID_DEF_VARIABLES:
                    Load_Variables(cload);
                    break;

                default:
                    Console.WriteLine("Unhandled Chunk: 0x{0:X8} (depth {1}) in {2}", cload.Cur_Chunk_ID, cload.Cur_Chunk_Depth, nameof(AccessiblePhysDefClass));
                    break;
            }

            cload.Close_Chunk();
        }

        return true;
    }


    //DECLARE_EDITABLE(AccessiblePhysDefClass, StaticAnimPhysDefClass);

	protected bool Save_Variables(ChunkSaveClass csave)
    {
        csave.WriteMicro(VARID_DEF_LOCKCODE, LockCode);
        return true;
    }
    protected bool Load_Variables(ChunkLoadClass cload)
    {
        while (cload.Open_Micro_Chunk())
        {
            switch (cload.Cur_Micro_Chunk_ID)
            {

                case VARID_DEF_LOCKCODE:
                    cload.Read(ref LockCode);
                    break;
            }

            cload.Close_Micro_Chunk();
        }

        return true;
    }

    protected int LockCode;

    private const uint CHUNKID_DEF_PARENT = 0x10311249u;
    private const uint CHUNKID_DEF_VARIABLES = 0x1031124Au;

    private const int VARID_DEF_LOCKCODE = 1;

    protected bool StaticAnimPhysDefClassLoad(ChunkLoadClass cload) => base.Load(cload);
    protected bool StaticAnimPhysDefClassSave(ChunkSaveClass csave) => base.Save(csave);
}

