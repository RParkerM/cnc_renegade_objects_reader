using RenData.ChunkIO;
using RenData.Definitions;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.EditorDef;

// Ported from Code/Tools/LevelEdit/TerrainDefinition.cpp
[RegisterDefinition(ChunkId.CHUNKID_TERRAIN_DEF)]
public partial class TerrainDefinitionClass : DefinitionClass
{
    public TerrainDefinitionClass()
    {
        // FILENAME_PARAM (m_ModelName, "Westwood 3D Files", ".w3d");
        // FILENAME_PARAM (m_LightFilename, "Westwood Light Database", ".wlt");
    }

    public override uint Get_Class_ID() => (uint)ClassId.CLASSID_TERRAIN;

    public override PersistClass Create()
    {
        // return new TerrainNodeClass ();
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
                    Console.WriteLine("Unrecognized TerrainDef chunkID\n");
                    break;
            }

            cload.Close_Chunk();
        }

        return retval;
    }

    private bool Save_Variables(ChunkSaveClass csave)
    {
        //
        //	Write the list of distances to the chunk
        //
        foreach (uint distance in m_DistanceList)
        {
            csave.WriteMicro(VARID_LOD_DIST, distance);
        }

        csave.WriteMicroString(VARID_MODEL_NAME, m_ModelName);
        csave.WriteMicroString(VARID_LIGHT_FILENAME, m_LightFilename);
        return true;
    }

    private bool Load_Variables(ChunkLoadClass cload)
    {
        // Start fresh
        m_DistanceList.Clear();

        //
        //	Loop through all the microchunks that define the variables
        //
        while (cload.Open_Micro_Chunk())
        {
            switch (cload.Cur_Micro_Chunk_ID)
            {
                case VARID_LOD_DIST:
                    {
                        uint distance = 0;
                        cload.Read(ref distance);
                        m_DistanceList.Add(distance);
                    }
                    break;

                case VARID_MODEL_NAME:
                    m_ModelName = cload.ReadMicroChunkWWString();
                    break;

                case VARID_LIGHT_FILENAME:
                    m_LightFilename = cload.ReadMicroChunkWWString();
                    break;
            }

            cload.Close_Micro_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    // Terrain definition specific
    public string Get_Model_Name() => m_ModelName;
    public void Set_Model_Name(string filename) => m_ModelName = filename;

    public string Get_Light_Filename() => m_LightFilename;
    public void Set_Light_Filename(string filename) => m_LightFilename = filename;

    private readonly List<uint> m_DistanceList = [];
    private string m_ModelName = string.Empty;
    private string m_LightFilename = string.Empty;

    private const uint CHUNKID_VARIABLES = 0x00000100;
    private const uint CHUNKID_BASE_CLASS = 0x00000200;

    private const byte VARID_LOD_DIST = 0x01;
    private const byte VARID_MODEL_NAME = 0x02;
    private const byte VARID_LIGHT_FILENAME = 0x03;
}
