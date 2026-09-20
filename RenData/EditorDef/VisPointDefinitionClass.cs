using RenData.ChunkIO;
using RenData.Definitions;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.EditorDef;

// Ported from Code/Tools/LevelEdit/VisPointDefinition.cpp
[RegisterDefinition(ChunkId.CHUNKID_VIS_POINT_DEF)]
public partial class VisPointDefinitionClass : DefinitionClass
{
    public VisPointDefinitionClass()
    {
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_VIS_POINT_DEF;

    public override PersistClass Create()
    {
        // return new VisPointNodeClass ();
        throw new NotImplementedException();
    }

    public override bool Save(ChunkSaveClass csave)
    {
        bool retval = true;

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
                case CHUNKID_BASE_CLASS:
                    retval &= base.Load(cload);
                    break;

                default:
                    Console.WriteLine("Unrecognized VisPointDef chunkID\n");
                    break;
            }

            cload.Close_Chunk();
        }

        return retval;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    private const uint CHUNKID_VARIABLES = 0x00000100;
    private const uint CHUNKID_BASE_CLASS = 0x00000200;
}
