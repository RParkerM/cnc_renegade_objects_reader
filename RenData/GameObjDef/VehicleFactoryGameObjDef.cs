using RenData.ChunkIO;
using RenData.Definitions;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.GameObjDef;

[RegisterDefinition(ChunkId.CHUNKID_GAME_OBJECT_DEF_VEHICLE_FACTORY)]
public partial class VehicleFactoryGameObjDef : BuildingGameObjDef
{
    public VehicleFactoryGameObjDef()
    {
        PadClearingWarhead = 25;
        TotalBuildingTime = 12.0f;
        //# ifdef PARAM_EDITING_ON
        //        EnumParameterClass* param = new EnumParameterClass(&PadClearingWarhead);
        //        param->Set_Name("Pad Clearing Warhead");
        //        for (int i = 0; i < ArmorWarheadManager::Get_Num_Warhead_Types(); i++)
        //        {
        //            param->Add_Value(ArmorWarheadManager::Get_Warhead_Name(i), i);
        //        }
        //        GENERIC_EDITABLE_PARAM(VehicleFactoryGameObjDef, param)
        //#endif

    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GAME_OBJECT_DEF_VEHICLE_FACTORY;
    public override PersistClass Create()
    {
        throw new NotImplementedException();
        //VehicleFactoryGameObj* building = new VehicleFactoryGameObj;
        //building->Init(*this);

        //return building;
    }
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        csave.WriteMicro(MICROCHUNKID_DEF_PADCLEARINGWARHEAD, PadClearingWarhead);
        csave.WriteMicro(MICROCHUNKID_DEF_TOTALBUILDINGTIME, TotalBuildingTime);
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
                    Load_Variables(cload);
                    break;

                default:
                    Console.WriteLine("Unrecognized Vehicle Factory Def chunkID\n");
                    break;

            }
            cload.Close_Chunk();
        }

        return true;
    }
    public override PersistFactoryClass Get_Factory() => _persistFactory;
    public int Get_Pad_Clearing_Warhead() { return PadClearingWarhead; }
    public float Get_Total_Building_Time() { return TotalBuildingTime; }

    //DECLARE_EDITABLE(VehicleFactoryGameObjDef, BuildingGameObjDef);

    protected void Load_Variables(ChunkLoadClass cload)
    {
        while (cload.Open_Micro_Chunk())
        {
            switch (cload.Cur_Micro_Chunk_ID)
            {

                case MICROCHUNKID_DEF_PADCLEARINGWARHEAD:
                    cload.Read(ref PadClearingWarhead);
                    break;
                case MICROCHUNKID_DEF_TOTALBUILDINGTIME:
                    cload.Read(ref TotalBuildingTime);
                    break;
                default:
                    Console.WriteLine("Unrecognized Vehicle Factory Def Variable chunkID\n");
                    break;
            }

            cload.Close_Micro_Chunk();
        }

        return;
    }

    protected int PadClearingWarhead;  // warhead used to destroy objects blocking construction area
    protected float TotalBuildingTime;	// total time for slowest vehicle to be constructed and driven out.

    private const int CHUNKID_DEF_PARENT = 0x02200638;
    private const int CHUNKID_DEF_VARIABLES = 0x02200639;

    private const int MICROCHUNKID_DEF_UNUSED = 1;
    private const int MICROCHUNKID_DEF_PADCLEARINGWARHEAD = 2;
    private const int MICROCHUNKID_DEF_TOTALBUILDINGTIME = 3;
};