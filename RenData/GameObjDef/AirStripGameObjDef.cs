using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.GameObjDef;

[RegisterDefinition(ChunkId.CHUNKID_GAME_OBJECT_DEF_AIRSTRIP)]
public partial class AirStripGameObjDef : VehicleFactoryGameObjDef
{
    public AirStripGameObjDef()
    {
        CinematicLengthToDropOff = 0;
        CinematicLengthToVehicleDisplay = 0;
        CinematicDefID = 0;
        CinematicSlotIndex = 0;
        //            //
        //            //	Editable support
        //            //		
        //            EDITABLE_PARAM(AirStripGameObjDef, ParameterClass::TYPE_FLOAT, CinematicLengthToDropOff);
        //            EDITABLE_PARAM(AirStripGameObjDef, ParameterClass::TYPE_INT, CinematicSlotIndex);
        //            EDITABLE_PARAM(AirStripGameObjDef, ParameterClass::TYPE_FLOAT, CinematicLengthToVehicleDisplay);

        //# ifdef PARAM_EDITING_ON
        //            GenericDefParameterClass* param = new GenericDefParameterClass(&CinematicDefID);
        //            param->Set_Class_ID(CLASSID_GAME_OBJECT_DEF_SIMPLE);
        //            param->Set_Name("Drop-Off Cinematic");
        //            GENERIC_EDITABLE_PARAM(AirStripGameObjDef, param)
        //	#endif //PARAM_EDITING_ON

        //	return;
    }
    public override uint Get_Class_ID() => ClassId.CLASSID_GAME_OBJECT_DEF_AIRSTRIP;
    public override PersistClass Create()
    {
        throw new NotImplementedException();
        //AirStripGameObj* building = new AirStripGameObj;
        //building->Init(*this);

        //return building;
    }
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);

        csave.WriteMicro(MICROCHUNKID_DEF_CINEMATIC_LENGTH_TO_DROPOFF, CinematicLengthToDropOff);
        csave.WriteMicro(MICROCHUNKID_DEF_CINEMATIC_DEFID, CinematicDefID);
        csave.WriteMicro(MICROCHUNKID_DEF_CINEMATIC_SLOT_INDEX, CinematicSlotIndex);
        csave.WriteMicro(MICROCHUNKID_DEF_DISPLAY_VEHICLE_TIME, CinematicLengthToVehicleDisplay);

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
                    Console.WriteLine("Unrecognized AirStrip Def chunkID\n");
                    break;

            }
            cload.Close_Chunk();
        }

        return true;
    }
    public override PersistFactoryClass Get_Factory() => _persistFactory;

    //DECLARE_EDITABLE(AirStripGameObjDef, VehicleFactoryGameObjDef);

    protected new void Load_Variables(ChunkLoadClass cload)
    {
        while (cload.Open_Micro_Chunk())
        {
            switch (cload.Cur_Micro_Chunk_ID)
            {
                case MICROCHUNKID_DEF_CINEMATIC_LENGTH_TO_DROPOFF:
                    cload.Read(ref CinematicLengthToDropOff);
                    break;
                case MICROCHUNKID_DEF_CINEMATIC_DEFID:
                    cload.Read(ref CinematicDefID);
                    break;
                case MICROCHUNKID_DEF_CINEMATIC_SLOT_INDEX:
                    cload.Read(ref CinematicSlotIndex);
                    break;
                case MICROCHUNKID_DEF_DISPLAY_VEHICLE_TIME:
                    cload.Read(ref CinematicLengthToVehicleDisplay);
                    break;
                default:
                    Console.WriteLine("Unrecognized AirStrip Def Variable chunkID\n");
                    break;
            }

            cload.Close_Micro_Chunk();
        }
    }

    protected int CinematicDefID;
    protected int CinematicSlotIndex;
    protected float CinematicLengthToDropOff;
    protected float CinematicLengthToVehicleDisplay;

    private const int CHUNKID_DEF_PARENT = 0x02200638;
    private const int CHUNKID_DEF_VARIABLES = 0x02200639;
    private const int MICROCHUNKID_DEF_CINEMATIC_DEFID = 1;
    private const int MICROCHUNKID_DEF_CINEMATIC_LENGTH_TO_DROPOFF = 2;
    private const int MICROCHUNKID_DEF_CINEMATIC_SLOT_INDEX = 3;
    private const int MICROCHUNKID_DEF_DISPLAY_VEHICLE_TIME = 4;
    private const int CHUNKID_PARENT = 0x0219043;
    private const int CHUNKID_VARIABLES = 0x0219044;
    private const int MICROCHUNKID_UNUSED = 1;
};

//SimplePersistFactoryClass<AirStripGameObjDef, CHUNKID_GAME_OBJECT_DEF_AIRSTRIP> _AirStripGameObjDefPersistFactory;
//SimplePersistFactoryClass<AirStripGameObj, CHUNKID_GAME_OBJECT_AIRSTRIP> _AirStripGameObjPersistFactory;
//DECLARE_DEFINITION_FACTORY(AirStripGameObjDef, CLASSID_GAME_OBJECT_DEF_AIRSTRIP, "Airstrip")   _AirStripGameObjDefDefFactory;
