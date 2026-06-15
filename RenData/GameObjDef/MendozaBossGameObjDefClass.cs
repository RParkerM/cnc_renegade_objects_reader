using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.GameObjDef;

[RegisterDefinition(ChunkId.CHUNKID_GAME_OBJECT_DEF_MENDOZA_BOSS)]
public partial class MendozaBossGameObjDefClass : SoldierGameObjDef
{
    public MendozaBossGameObjDefClass()
    {
        UseInnateBehavior = false;

        //PARAM_SEPARATOR(MendozaBossGameObjDefClass, "Boss Settings");
        //PARAM_SEPARATOR(MendozaBossGameObjDefClass, "");
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GAME_OBJECT_DEF_MENDOZA_BOSS;

    public override PersistClass Create()
    {
        //MendozaBossGameObjClass* obj = new MendozaBossGameObjClass;
        //obj->Init(*this);
        //return obj;
        throw new NotImplementedException();
    }

    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        csave.End_Chunk();

        return true;
    }

    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {
                case CHUNKID_DEF_PARENT:
                    base.Load(cload);
                    break;

                case CHUNKID_DEF_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unrecognized MendozaBossGameObjDefClass chunkID\n");
                    break;
            }
            cload.Close_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;


    private const uint CHUNKID_DEF_PARENT = 0x09230242;
    private const uint CHUNKID_DEF_VARIABLES = 0x09230243;
}
