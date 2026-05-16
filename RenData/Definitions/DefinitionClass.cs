using RenData.ChunkIO;
using RenData.SaveLoad;

namespace RenData.Definitions;
public abstract class DefinitionClass : EditableClass
{
    /////////////////////////////////////////////////////////////////////
    //	Editable interface requirements
    /////////////////////////////////////////////////////////////////////
    //DECLARE_EDITABLE(DefinitionClass, EditableClass);


    // Type identification
    public abstract uint Get_Class_ID();
    public abstract PersistClass? Create();

    // User data support
    public uint Get_User_Data() { return m_GenericUserData; }
    public void Set_User_Data(uint data) { m_GenericUserData = data; }

    // Save support
    public bool Is_Save_Enabled() { return m_SaveEnabled; }
    public void Enable_Save(bool onoff) { m_SaveEnabled = onoff; }

    //	Protected member data
    public int m_DefinitionMgrLink;

    private string m_Name;
    private uint m_ID;
    private uint m_GenericUserData;
    private bool m_SaveEnabled;

    public DefinitionClass()
    {
        m_Name = string.Empty;
        m_ID = 0;
        m_GenericUserData = 0;
        m_SaveEnabled = true;
        m_DefinitionMgrLink = -1;
    }

    public virtual string Get_Name()
    {
        return m_Name;
    }

    public virtual void Set_Name(string new_name)
    {
        m_Name = new_name;
    }

    public virtual uint Get_ID()
    {
        return m_ID;
    }

    public virtual bool Is_Valid_Config(string message)
    {
        return true;
    }

    private const int CHUNKID_VARIABLES = 0x00000100;



    private const int VARID_INSTANCEID = 0x01;
    private const int XXX_VARID_PARENTID = 0x02;
    private const int VARID_NAME = 0x03;

    public override bool Save(ChunkSaveClass csave)
    {
        bool retval = true;

        csave.Begin_Chunk(CHUNKID_VARIABLES);
        retval &= Save_Variables(csave);
        csave.End_Chunk();

        return retval;
    }

    public override bool Load(ChunkLoadClass cload)
    {
        bool retval = true;

        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {
                case CHUNKID_VARIABLES:
                    Load_Variables(cload);
                    break;
            }

            cload.Close_Chunk();
        }
        //Console.WriteLine($"Loaded Definition: {Get_Name()}.");

        return retval;
    }


    private bool Save_Variables(ChunkSaveClass csave)
    {
        bool retval = true;

        csave.WriteMicro(VARID_INSTANCEID, m_ID);

        // TODO: test is this is correct port
        // originally WRITE_MICRO_CHUNK_WWSTRING
        csave.WriteMicroString(VARID_NAME, m_Name);
        return retval;
    }


    private bool Load_Variables(ChunkLoadClass cload)
    {
        bool retval = true;

        //
        //	Loop through all the microchunks that define the variables
        //
        while (cload.Open_Micro_Chunk())
        {
            switch (cload.Cur_Micro_Chunk_ID)
            {
                case VARID_INSTANCEID:
                    cload.Read(ref m_ID);
                    break;
                case VARID_NAME:
                    m_Name = cload.ReadMicroChunkWWString();
                    //READ_MICRO_CHUNK_WWSTRING(cload, VARID_NAME, m_Name)
                    break;
            }

            cload.Close_Micro_Chunk();
        }
        return retval;
    }


    public void Set_ID(uint id)
    {
        m_ID = id;

        //
        //	If we are registered with the definition manager, then we need to
        // re-link ourselves back into the list
        //
        if (m_DefinitionMgrLink != -1)
        {
            DefinitionMgrClass.Unregister_Definition(this);
            DefinitionMgrClass.Register_Definition(this);
        }

        return;
    }
}
