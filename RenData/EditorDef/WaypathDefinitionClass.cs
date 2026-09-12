using RenData.ChunkIO;
using RenData.Definitions;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.EditorDef;

// Ported from Code/Tools/LevelEdit/WaypathDefinition.cpp
[RegisterDefinition(ChunkId.CHUNKID_WAYPATH_DEF)]
public partial class WaypathDefinitionClass : DefinitionClass
{
    public WaypathDefinitionClass()
    {
        m_PassableObjects = 0;
        // ENUM_PARAM (m_PassableObjects, ("All", 1, "Humans", 2, "Ground Vehicles", 3, "Flying Vehicles", 4, NULL));
    }

    public override uint Get_Class_ID() => (uint)ClassId.CLASSID_WAYPATH;

    public override PersistClass Create()
    {
        // return new WaypathNodeClass ();
        throw new NotImplementedException();
    }

    public override bool Save(ChunkSaveClass csave)
    {
        bool retval = true;

        csave.Begin_Chunk(CHUNKID_BASE_CLASS);
        retval &= base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_VARIABLES);
        csave.WriteMicro(VARID_PASSABLE_OBJ_ID, m_PassableObjects);
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
                case CHUNKID_BASE_CLASS:
                    retval &= base.Load(cload);
                    break;

                case CHUNKID_VARIABLES:
                    retval &= Load_Variables(cload);
                    break;

                default:
                    Console.WriteLine("Unrecognized WaypathDef chunkID\n");
                    break;
            }

            cload.Close_Chunk();
        }

        return retval;
    }

    private bool Load_Variables(ChunkLoadClass cload)
    {
        //
        //	Loop through all the microchunks that define the variables
        //
        while (cload.Open_Micro_Chunk())
        {
            switch (cload.Cur_Micro_Chunk_ID)
            {
                case VARID_PASSABLE_OBJ_ID:
                    cload.Read(ref m_PassableObjects);
                    break;
            }

            cload.Close_Micro_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    private int m_PassableObjects;

    private const uint CHUNKID_VARIABLES = 0x00000100;
    private const uint CHUNKID_BASE_CLASS = 0x00000200;

    private const byte VARID_PASSABLE_OBJ_ID = 0x01;
}
