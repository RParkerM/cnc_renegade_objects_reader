using RenData.ChunkIO;

namespace RenData.PhysDef;

public abstract class DynamicPhysDefClass : PhysDefClass
{
    public DynamicPhysDefClass() { }

    // From PersistClass
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(DYNAMICPHYSDEF_CHUNK_PHYSDEF);
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

                case DYNAMICPHYSDEF_CHUNK_PHYSDEF:
                    base.Load(cload);
                    break;
            }

            cload.Close_Chunk();
        }
        return true;
    }

    // From PhysDefClass
    public override string Get_Type_Name() { return "DynamicPhysDef"; }
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

    // Validation methods
    public override bool Is_Valid_Config(string message)
    {
        return base.Is_Valid_Config(message);
    }

    //	Editable interface requirements
    //DECLARE_EDITABLE(DynamicPhysDefClass, PhysDefClass);

    protected bool PhysDefClassLoad(ChunkLoadClass cload) => base.Load(cload);
    protected bool PhysDefClassSave(ChunkSaveClass csave) => base.Save(csave);

    private const int DYNAMICPHYSDEF_CHUNK_PHYSDEF = 813001104;
};
