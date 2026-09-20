using RenData.ChunkIO;
using RenData.Definitions;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.EditorDef;

// Ported from Code/Tools/LevelEdit/PathfindStartDefinition.cpp
[RegisterDefinition(ChunkId.CHUNKID_PATHFIND_START_DEF)]
public partial class PathfindStartDefinitionClass : DefinitionClass
{
    public PathfindStartDefinitionClass()
    {
        m_GameObjectID = 0;
        // EDITABLE_PARAM( ParameterClass::TYPE_GAMEOBJDEFINITIONID, m_GameObjectID );
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_PATHFIND_START_DEF;

    public override PersistClass Create()
    {
        // return new PathfindStartNodeClass ();
        throw new NotImplementedException();
    }

    public override bool Save(ChunkSaveClass csave)
    {
        bool retval = true;

        csave.Begin_Chunk(CHUNKID_BASE_CLASS);
        retval &= base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_VARIABLES);
        csave.WriteMicro(VARID_GAME_OBJ_ID, m_GameObjectID);
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
                    Console.WriteLine("Unrecognized PathfindStartDef chunkID\n");
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
                case VARID_GAME_OBJ_ID:
                    cload.Read(ref m_GameObjectID);
                    break;
            }

            cload.Close_Micro_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    // PathfindStartDefinitionClass specific
    public int Get_Object_Type() => m_GameObjectID;

    private int m_GameObjectID;

    private const uint CHUNKID_VARIABLES = 0x00000100;
    private const uint CHUNKID_BASE_CLASS = 0x00000200;

    private const byte VARID_GAME_OBJ_ID = 0x01;
}
