
using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;
namespace RenData.GameObjDef;

[RegisterDefinition(ChunkId.CHUNKID_GAME_OBJECT_DEF_SAMSITE)]
public partial class SAMSiteGameObjDef : SmartGameObjDef
{
    public SAMSiteGameObjDef()
    {
        //MODEL_DEF_PARAM(SAMSiteGameObjDef, PhysDefID, "DecorationPhysDef");
    }
    public override uint Get_Class_ID() => ClassId.CLASSID_GAME_OBJECT_DEF_SAMSITE;
    public override PersistClass Create()
    {
        //SAMSiteGameObj obj = new SAMSiteGameObj;
        //obj.Init(this);
        //return obj;
        throw new NotImplementedException();
    }
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
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

                default:
                    Console.WriteLine("Unrecognized SimpleDef chunkID\n");
                    break;

            }
            cload.Close_Chunk();
        }

        return true;
    }
    public override PersistFactoryClass Get_Factory() => _persistFactory;

    //DECLARE_EDITABLE(SAMSiteGameObjDef, SmartGameObjDef);

    const int CHUNKID_DEF_PARENT = 930991800;
};


//SimplePersistFactoryClass<SAMSiteGameObjDef, CHUNKID_GAME_OBJECT_DEF_SAMSITE>	_SAMSiteGameObjDefPersistFactory;
//DECLARE_DEFINITION_FACTORY(SAMSiteGameObjDef, CLASSID_GAME_OBJECT_DEF_SAMSITE, "SAMSite") _SAMSiteGameObjDefDefFactory;

