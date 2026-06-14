using RenData.ChunkIO;
using RenData.Definitions;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.GameObjDef;

[RegisterDefinition(ChunkId.CHUNKID_GAME_OBJECT_DEF_SOLDIER_FACTORY)]
public partial class SoldierFactoryGameObjDef : BuildingGameObjDef
{
    public SoldierFactoryGameObjDef()
    {
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GAME_OBJECT_DEF_SOLDIER_FACTORY;

    public override PersistClass Create()
    {
        //SoldierFactoryGameObj* building = new SoldierFactoryGameObj;
        //building->Init(*this);
        //return building;
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
                    Console.WriteLine("Unrecognized Soldier Factory Def chunkID\n");
                    break;
            }
            cload.Close_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;


    private const uint CHUNKID_DEF_PARENT = 0x02211153;
    private const uint CHUNKID_DEF_VARIABLES = 0x02211154;
}
