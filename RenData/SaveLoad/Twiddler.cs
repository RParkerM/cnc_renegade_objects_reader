using RenData.ChunkIO;
using RenData.Definitions;
using RenData.IDs;

namespace RenData.SaveLoad;

[RegisterDefinition(ChunkId.CHUNKID_TWIDDLER)]
public partial class TwiddlerClass : DefinitionClass
{
    //DECLARE_EDITABLE(TwiddlerClass, DefinitionClass);
    //DECLARE_DEFINITION_FACTORY(TwiddlerClass, CLASSID_TWIDDLERS, "Twiddler")    _TwiddlerFactory;

    public TwiddlerClass()
    {
        //CLASSID_DEFIDLIST_PARAM(TwiddlerClass, m_DefinitionList, 0, m_IndirectClassID, "Preset List");
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_TWIDDLERS;
    public override PersistClass? Create()
    {
        PersistClass? retval = null;

        //
        //	Pick a random definition
        //
        DefinitionClass? definition = Twiddle();
        if (definition != null)
        {

            //
            //	Indirect the creation to the definition we randomly selected
            //
            retval = definition.Create();

        }

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
                    retval &= Load_Variables(cload);
                    break;

                case CHUNKID_BASE_CLASS:
                    retval &= base.Load(cload);
                    break;
            }

            cload.Close_Chunk();
        }

        return retval;
    }
    public override bool Save(ChunkSaveClass csave)
    {
        bool retval = true;

        csave.Begin_Chunk(CHUNKID_VARIABLES);
        retval &= Save_Variables(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_BASE_CLASS);
        retval &= base.Save(csave);
        csave.End_Chunk();

        return retval;
    }

    //SimplePersistFactoryClass<TwiddlerClass> _TwiddlerPersistFactory = new(ChunkId.CHUNKID_TWIDDLER);
    public override PersistFactoryClass Get_Factory()
    {
        return _persistFactory;
    }


    public virtual DefinitionClass? Twiddle()
    {

        DefinitionClass? definition = null;

        if (m_DefinitionList.Count > 0)
        {
            //
            //	Get a random index into our definition list
            //

            var randomizer = new Random(Environment.TickCount);
            int index = randomizer.Next(0, m_DefinitionList.Count);

            //
            //	Lookup the definition this entry represents
            //
            int def_id = m_DefinitionList[index];
            definition = DefinitionMgrClass.Find_Definition((uint)def_id);
        }

        return definition;
    }
    public virtual uint Get_Indirect_Class_ID() => m_IndirectClassID;
    public virtual void Set_Indirect_Class_ID(uint class_id) { m_IndirectClassID = class_id; }

    private bool Save_Variables(ChunkSaveClass csave)
    {
        csave.WriteMicro(VARID_INDIRECT_CLASSID, m_IndirectClassID);


        for (int index = 0; index < m_DefinitionList.Count(); index++)
        {
            //
            //	Save this definition ID to the chunk
            //
            int def_id = m_DefinitionList[index];
            csave.WriteMicro(VARID_DEFINTION_ID, def_id);
        }

        return true;
    }

    private bool Load_Variables(ChunkLoadClass cload)
    {
        //
        //	Start fresh
        //
        m_DefinitionList.Clear();

        //
        //	Loop through all the microchunks that define the variables
        //
        while (cload.Open_Micro_Chunk())
        {
            switch (cload.Cur_Micro_Chunk_ID)
            {
                case VARID_INDIRECT_CLASSID:
                    cload.Read(ref m_IndirectClassID);
                    break;


                case VARID_DEFINTION_ID:
                    //
                    //	Read the definition ID from the chunk and add it
                    // to our list
                    //
                    int def_id = 0;
                    cload.Read(ref def_id);
                    m_DefinitionList.Add(def_id);
                    break;
            }

            cload.Close_Micro_Chunk();
        }

        return true;
    }

    private uint m_IndirectClassID = 0;
    private List<int> m_DefinitionList = [];

    const int CHUNKID_VARIABLES = 0x00000100;
    const int CHUNKID_BASE_CLASS = 0x00000200;
    const int VARID_DEFINTION_ID = 0x01;
    const int VARID_INDIRECT_CLASSID = 0x02;
};

