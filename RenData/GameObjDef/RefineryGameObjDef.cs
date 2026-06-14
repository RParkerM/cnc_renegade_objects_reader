using RenData.ChunkIO;
using RenData.Definitions;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.GameObjDef;

[RegisterDefinition(ChunkId.CHUNKID_GAME_OBJECT_DEF_REFINERY)]
public partial class RefineryGameObjDef : BuildingGameObjDef
{
    public RefineryGameObjDef()
    {
        UnloadTime = 0;
        FundsGathered = 0;
        FundsDistributedPerSec = 0;
        HarvesterDefID = 0;

        //EDITABLE_PARAM(RefineryGameObjDef, ParameterClass::TYPE_FLOAT, UnloadTime);
        //EDITABLE_PARAM(RefineryGameObjDef, ParameterClass::TYPE_FLOAT, FundsGathered);
        //EDITABLE_PARAM(RefineryGameObjDef, ParameterClass::TYPE_FLOAT, FundsDistributedPerSec);
        //#ifdef PARAM_EDITING_ON
        //    GenericDefParameterClass* param = new GenericDefParameterClass(&HarvesterDefID);
        //    param->Set_Class_ID(CLASSID_GAME_OBJECT_DEF_VEHICLE);
        //    param->Set_Name("Harvester");
        //    GENERIC_EDITABLE_PARAM(RefineryGameObjDef, param);
        //#endif
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GAME_OBJECT_DEF_REFINERY;

    public override PersistClass Create()
    {
        //RefineryGameObj* building = new RefineryGameObj;
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
        csave.WriteMicro(MICROCHUNKID_DEF_UNLOAD_TIME, UnloadTime);
        csave.WriteMicro(MICROCHUNKID_DEF_FUNDS_GATHERED, FundsGathered);
        csave.WriteMicro(MICROCHUNKID_DEF_FUNDS_PER_SEC, FundsDistributedPerSec);
        csave.WriteMicro(MICROCHUNKID_DEF_HARVESTER_DEFID, HarvesterDefID);
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
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case MICROCHUNKID_DEF_UNLOAD_TIME:
                                cload.Read(ref UnloadTime);
                                break;
                            case MICROCHUNKID_DEF_FUNDS_GATHERED:
                                cload.Read(ref FundsGathered);
                                break;
                            case MICROCHUNKID_DEF_FUNDS_PER_SEC:
                                cload.Read(ref FundsDistributedPerSec);
                                break;
                            case MICROCHUNKID_DEF_HARVESTER_DEFID:
                                cload.Read(ref HarvesterDefID);
                                break;
                            default:
                                Console.WriteLine("Unrecognized Refinery Def Variable chunkID\n");
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unrecognized Refinery Def chunkID\n");
                    break;
            }
            cload.Close_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    protected float UnloadTime;
    protected float FundsGathered;
    protected float FundsDistributedPerSec;
    protected int HarvesterDefID;


    private const uint CHUNKID_DEF_PARENT = 0x02200638;
    private const uint CHUNKID_DEF_VARIABLES = 0x02200639;

    private const byte MICROCHUNKID_DEF_UNLOAD_TIME = 1;
    private const byte MICROCHUNKID_DEF_FUNDS_GATHERED = 2;
    private const byte MICROCHUNKID_DEF_HARVESTER_DEFID = 3;
    private const byte MICROCHUNKID_DEF_FUNDS_PER_SEC = 4;
}
