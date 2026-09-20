using RenData.ChunkIO;
using RenData.Definitions;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.EditorDef;

// Ported from Code/Tools/LevelEdit/editoronlydefinition.cpp
[RegisterDefinition(ChunkId.CHUNKID_EDITOR_ONLY_OBJECTS_DEF)]
public partial class EditorOnlyDefinitionClass : DefinitionClass
{
    public EditorOnlyDefinitionClass()
    {
        // FILENAME_PARAM (ModelName, "Westwood 3D Files", ".w3d");
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_EDITOR_ONLY_OBJECTS;

    public override PersistClass Create()
    {
        // return new EditorOnlyObjectNodeClass ();
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
                    Console.WriteLine("Unrecognized EditorOnlyDef chunkID\n");
                    break;
            }

            cload.Close_Chunk();
        }

        return retval;
    }

    private bool Save_Variables(ChunkSaveClass csave)
    {
        csave.WriteMicroString(VARID_MODEL_NAME, ModelName);
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
                case VARID_MODEL_NAME:
                    ModelName = cload.ReadMicroChunkWWString();
                    break;
            }

            cload.Close_Micro_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    public string Get_Model_Name() => ModelName;

    private string ModelName = string.Empty;

    private const uint CHUNKID_VARIABLES = 0x00000100;
    private const uint CHUNKID_BASE_CLASS = 0x00000200;

    private const byte VARID_MODEL_NAME = 0x01;
}
