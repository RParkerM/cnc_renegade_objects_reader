using RenData.ChunkIO;
using RenData.Definitions;
using System.Diagnostics;


public abstract class PhysDefClass : DefinitionClass
{
    public PhysDefClass()
    {
        ModelName = "null";
        IsPreLit = false;
        //FILENAME_PARAM(PhysDefClass, ModelName, "Westwood 3D Files", ".w3d");
    }

    // From PersistClass
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(PHYSDEF_CHUNK_DEFINITION);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(PHYSDEF_CHUNK_VARIABLES);
        ArgumentNullException.ThrowIfNull(ModelName);
        csave.WriteMicroString(PHYSDEF_VARIABLE_MODELNAME, ModelName);
        csave.WriteMicro(PHYSDEF_VARIABLE_ISPRELIT, IsPreLit);
        csave.End_Chunk();
        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {
                case PHYSDEF_CHUNK_DEFINITION:
                    base.Load(cload);
                    break;

                case PHYSDEF_CHUNK_VARIABLES:
                    Debug.Assert(cload.Cur_Chunk_ID == PHYSDEF_CHUNK_VARIABLES);
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            //OBSOLETE_MICRO_CHUNK(PHYSDEF_VARIABLE_FLAGS);
                            case PHYSDEF_VARIABLE_MODELNAME:
                                cload.ReadMicroChunkWWString(out ModelName);
                                break;
                            case PHYSDEF_VARIABLE_ISPRELIT:
                                cload.Read(ref IsPreLit);
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;
            }

            cload.Close_Chunk();
        }
        return true;
    }

    // PhysDef type filtering mechanism
    public virtual string Get_Type_Name() { return "PhysDef"; }
    public virtual bool Is_Type(string type_name)
    {
        return string.Compare(type_name, Get_Type_Name(), true) == 0;
    }

    // Validation methods
    public virtual bool Is_Valid_Config(ref string message)
    {
        bool retval = true;

        if (string.IsNullOrEmpty(ModelName))
        {
            message += "ModelName is invalid!\n";
            retval = false;
        }

        return retval;
    }

    // accessors
    public string Get_Model_Name()
    {
        ArgumentNullException.ThrowIfNull(ModelName);
        return ModelName;
    }

    public bool Get_Is_Pre_Lit() { return IsPreLit; }

    //	Editable interface requirements
    //DECLARE_EDITABLE(PhysDefClass, DefinitionClass);


    protected string? ModelName;
    protected bool IsPreLit;


    private const int PHYSDEF_CHUNK_DEFINITION = 0x055ffe07;
    private const int PHYSDEF_CHUNK_VARIABLES = 0x055ffe08;

    private const int PHYSDEF_VARIABLE_FLAGS = 0x00;
    private const int PHYSDEF_VARIABLE_MODELNAME = 0x01;
    private const int PHYSDEF_VARIABLE_ISPRELIT = 0x02;
};

