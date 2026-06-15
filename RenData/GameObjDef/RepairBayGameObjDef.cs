using RenData.ChunkIO;
using RenData.Definitions;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.GameObjDef;

[RegisterDefinition(ChunkId.CHUNKID_GAME_OBJECT_DEF_REPAIR_BAY)]
public partial class RepairBayGameObjDef : BuildingGameObjDef
{
    public RepairBayGameObjDef()
    {
        RepairPerSec = 0;
        RepairingStaticAnimDefID = 0;

        //EDITABLE_PARAM(RepairBayGameObjDef, ParameterClass::TYPE_FLOAT, RepairPerSec);
        //#ifdef PARAM_EDITING_ON
        //    GenericDefParameterClass* param = new GenericDefParameterClass(&RepairingStaticAnimDefID);
        //    param->Set_Class_ID(CLASSID_TILE);
        //    param->Set_Name("Repairing Static Anim Type");
        //    GENERIC_EDITABLE_PARAM(RepairBayGameObjDef, param);
        //#endif
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GAME_OBJECT_DEF_REPAIR_BAY;

    public override PersistClass Create()
    {
        //RepairBayGameObj* building = new RepairBayGameObj;
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
        csave.WriteMicro(MICROCHUNKID_DEF_REPAIR_PER_SEC, RepairPerSec);
        csave.WriteMicro(MICROCHUNKID_DEF_REPARING_STATICANIM_DEFID, RepairingStaticAnimDefID);
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
                            case MICROCHUNKID_DEF_REPAIR_PER_SEC:
                                cload.Read(ref RepairPerSec);
                                break;
                            case MICROCHUNKID_DEF_REPARING_STATICANIM_DEFID:
                                cload.Read(ref RepairingStaticAnimDefID);
                                break;
                            default:
                                Console.WriteLine("Unrecognized RepairBay Def Variable chunkID\n");
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unrecognized RepairBay Def chunkID\n");
                    break;
            }
            cload.Close_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    protected float RepairPerSec;
    protected int RepairingStaticAnimDefID;


    private const uint CHUNKID_DEF_PARENT = 0x02200638;
    private const uint CHUNKID_DEF_VARIABLES = 0x02200639;

    private const byte MICROCHUNKID_DEF_REPAIR_PER_SEC = 1;
    private const byte MICROCHUNKID_DEF_REPARING_STATICANIM_DEFID = 2;
}
