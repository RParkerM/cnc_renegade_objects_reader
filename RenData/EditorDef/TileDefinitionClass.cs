using RenData.ChunkIO;
using RenData.Definitions;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.EditorDef;

// Ported from Code/Tools/LevelEdit/TileDefinition.cpp
[RegisterDefinition(ChunkId.CHUNKID_TILE_DEF)]
public partial class TileDefinitionClass : DefinitionClass
{
    public TileDefinitionClass()
    {
        m_PhysDefID = 0;
        // MODEL_DEF_PARAM (m_PhysDefID, "StaticPhysDef");
    }

    public override uint Get_Class_ID() => (uint)ClassId.CLASSID_TILE;

    public override PersistClass Create()
    {
        // return new TileNodeClass ();
        throw new NotImplementedException();
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

                default:
                    Console.WriteLine("Unrecognized TileDef chunkID\n");
                    break;
            }

            cload.Close_Chunk();
        }

        return retval;
    }

    private bool Save_Variables(ChunkSaveClass csave)
    {
        csave.WriteMicro(VARID_PHYS_DEF_ID, m_PhysDefID);
        return true;
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
                case VARID_PHYS_DEF_ID:
                    cload.Read(ref m_PhysDefID);
                    break;
            }

            cload.Close_Micro_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    // Tile definition specific
    public int Get_Phys_Def_ID() => m_PhysDefID;

    private int m_PhysDefID;

    private const uint CHUNKID_VARIABLES = 0x00000100;
    private const uint CHUNKID_BASE_CLASS = 0x00000200;

    private const byte VARID_XXX_MODEL_NAME = 0x01;
    private const byte VARID_PHYS_DEF_ID = 0x02;
}
