using RenData.ChunkIO;
using RenData.IDs;
using RenData.PhysDef;
using RenData.SaveLoad;

namespace RenData;



[RegisterDefinition(ChunkId.PHYSICS_CHUNKID_DOORPHYSDEF)]
public partial class DoorPhysDefClass : AccessiblePhysDefClass
{
    public DoorPhysDefClass()
    {
        CloseDelay = 2.0f;
        TriggerZone1 = new OBBoxClass(); // original: Vector3(0,0,0) to Vector3(1,1,1)
        TriggerZone2 = new OBBoxClass(); // original: Vector3(0,0,0) to Vector3(1,1,1)
        OpenSoundDefID = 0;
        CloseSoundDefID = 0;
        UnlockSoundDefID = 0;
        AccessDeniedSoundDefID = 0;
        DoorOpensForVehicles = false;

        //ZONE_PARAM(DoorPhysDefClass, TriggerZone1, "TriggerZone1");
        //ZONE_PARAM(DoorPhysDefClass, TriggerZone2, "TriggerZone2");
        //EDITABLE_PARAM(DoorPhysDefClass, ParameterClass.TYPE_FLOAT, CloseDelay);
        //EDITABLE_PARAM(DoorPhysDefClass, ParameterClass.TYPE_SOUNDDEFINITIONID, OpenSoundDefID);
        //EDITABLE_PARAM(DoorPhysDefClass, ParameterClass.TYPE_SOUNDDEFINITIONID, CloseSoundDefID);
        //EDITABLE_PARAM(DoorPhysDefClass, ParameterClass.TYPE_SOUNDDEFINITIONID, UnlockSoundDefID);
        //EDITABLE_PARAM(DoorPhysDefClass, ParameterClass.TYPE_SOUNDDEFINITIONID, AccessDeniedSoundDefID);
        //EDITABLE_PARAM(DoorPhysDefClass, ParameterClass.TYPE_BOOL, DoorOpensForVehicles);
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_DOORPHYSDEF;
    public override string Get_Type_Name() { return "DoorPhysDef"; }
    public override bool Is_Type(string type_name)
    {
        if (string.Compare(type_name, Get_Type_Name()) == 0)
        {
            return true;
        }
        else
        {
            return base.Is_Type(type_name);
        }
    }
    public override PersistClass Create()
    {
        // DoorPhysClass door = new DoorPhysClass;
        // door.Init(this);
        // return door;
        throw new NotImplementedException();
    }
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        //		WRITE_MICRO_CHUNK( csave, MICROCHUNKID_DEF_TRIGGER_RADIUS, TriggerRadius );
        csave.WriteMicro(MICROCHUNKID_DEF_CLOSE_DELAY, CloseDelay);
        csave.WriteMicro(MICROCHUNKID_DEF_TRIGGER_ZONE1, TriggerZone1);
        csave.WriteMicro(MICROCHUNKID_DEF_TRIGGER_ZONE2, TriggerZone2);
        csave.WriteMicro(MICROCHUNKID_DEF_OPEN_SOUND_DEF_ID, OpenSoundDefID);
        csave.WriteMicro(MICROCHUNKID_DEF_CLOSE_SOUND_DEF_ID, CloseSoundDefID);
        csave.WriteMicro(MICROCHUNKID_DEF_UNLOCK_SOUND_DEF_ID, UnlockSoundDefID);
        csave.WriteMicro(MICROCHUNKID_DEF_ACCESS_DENIED_SOUND_DEF_ID, AccessDeniedSoundDefID);
        csave.WriteMicro(MICROCHUNKID_DEF_DOOR_OPENS_FOR_VEHICLES, DoorOpensForVehicles);
        csave.End_Chunk();

        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {

                case CHUNKID_DEF_OLD_PARENT:
                    StaticAnimPhysDefClassLoad(cload);
                    break;

                case CHUNKID_DEF_PARENT:
                    base.Load(cload);
                    break;

                case CHUNKID_DEF_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            //                        READ_MICRO_CHUNK( cload, MICROCHUNKID_DEF_TRIGGER_RADIUS, TriggerRadius );
                            case MICROCHUNKID_DEF_CLOSE_DELAY:
                                cload.Read(ref CloseDelay);
                                break;
                            case MICROCHUNKID_DEF_TRIGGER_ZONE1:
                                cload.Read(ref TriggerZone1);
                                break;
                            case MICROCHUNKID_DEF_TRIGGER_ZONE2:
                                cload.Read(ref TriggerZone2);
                                break;
                            case MICROCHUNKID_DEF_OLD_LOCK_CODE:
                                cload.Read(ref LockCode);
                                break;
                            case MICROCHUNKID_DEF_OPEN_SOUND_DEF_ID:
                                cload.Read(ref OpenSoundDefID);
                                break;
                            case MICROCHUNKID_DEF_CLOSE_SOUND_DEF_ID:
                                cload.Read(ref CloseSoundDefID);
                                break;
                            case MICROCHUNKID_DEF_UNLOCK_SOUND_DEF_ID:
                                cload.Read(ref UnlockSoundDefID);
                                break;
                            case MICROCHUNKID_DEF_ACCESS_DENIED_SOUND_DEF_ID:
                                cload.Read(ref AccessDeniedSoundDefID);
                                break;
                            case MICROCHUNKID_DEF_DOOR_OPENS_FOR_VEHICLES:
                                cload.Read(ref DoorOpensForVehicles);
                                break;

                            default:
                                Console.WriteLine("Unrecognized DoorPhys Variable chunkID: 0x{0:X8} (depth {1}) in {2}", cload.Cur_Chunk_ID, cload.Cur_Chunk_Depth, nameof(DoorPhysDefClass));
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unrecognized DoorPhys chunkID: 0x{0:X8} (depth {1}) in {2}", cload.Cur_Chunk_ID, cload.Cur_Chunk_Depth, nameof(DoorPhysDefClass));
                    break;

            }
            cload.Close_Chunk();
        }

        return true;
    }
    public override PersistFactoryClass Get_Factory() => _persistFactory;

    //DECLARE_EDITABLE(DoorPhysDefClass, AccessiblePhysDefClass);

    public OBBoxClass Get_Trigger_Zone1() { return TriggerZone1; }
    public OBBoxClass Get_Trigger_Zone2() { return TriggerZone2; }


    public bool Is_Vehicle_Door() { return DoorOpensForVehicles; }

    protected OBBoxClass TriggerZone1;
    protected OBBoxClass TriggerZone2;

    protected float CloseDelay;
    protected int OpenSoundDefID;
    protected int CloseSoundDefID;
    protected int UnlockSoundDefID;
    protected int AccessDeniedSoundDefID;
    protected bool DoorOpensForVehicles;

    private const uint CHUNKID_DEF_OLD_PARENT = 320001902u;
    private const uint CHUNKID_DEF_VARIABLES = 320001903u;
    private const uint CHUNKID_DEF_PARENT = 320001904u;

    private const int XXXMICROCHUNKID_DEF_TRIGGER_RADIUS = 1;
    private const int MICROCHUNKID_DEF_CLOSE_DELAY = 2;
    private const int MICROCHUNKID_DEF_TRIGGER_ZONE1 = 3;
    private const int MICROCHUNKID_DEF_OLD_LOCK_CODE = 4;
    private const int MICROCHUNKID_DEF_OPEN_SOUND_DEF_ID = 5;
    private const int MICROCHUNKID_DEF_CLOSE_SOUND_DEF_ID = 6;
    private const int MICROCHUNKID_DEF_UNLOCK_SOUND_DEF_ID = 7;
    private const int MICROCHUNKID_DEF_ACCESS_DENIED_SOUND_DEF_ID = 8;
    private const int MICROCHUNKID_DEF_TRIGGER_ZONE2 = 9;
    private const int MICROCHUNKID_DEF_DOOR_OPENS_FOR_VEHICLES = 10;
}
