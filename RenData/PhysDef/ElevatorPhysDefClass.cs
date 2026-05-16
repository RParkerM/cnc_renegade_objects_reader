using RenData.ChunkIO;
using RenData.SaveLoad;
using RenData.IDs;
using System.Numerics;

namespace RenData.PhysDef;


[RegisterDefinition(ChunkId.PHYSICS_CHUNKID_ELEVATORPHYSDEF)]
public partial class ElevatorPhysDefClass : AccessiblePhysDefClass
{
    public ElevatorPhysDefClass()
    {
        // moved initializer from original free-function
        CallZones = new OBBoxClass[4];
        for (int index = 0; index < 4; index++)
        {
            CallZones[index] = new OBBoxClass();
            CallZones[index].Center = new Vector3(0, 0, 0);
            CallZones[index].Extent = new Vector3(1, 1, 1);
        }

        CloseDelay = 2.0f;
        DoorClosedTop_FrameNum = -1.0f;
        DoorOpeningBottom_FrameNum = -1.0f;
        ElevatorStartTop_FrameNum = -1.0f;
        ElevatorStoppedBottom_FrameNum = -1.0f;
        DoorOpenSoundDefID = 0;
        DoorCloseSoundDefID = 0;
        DoorUnlockSoundDefID = 0;
        DoorAccessDeniedSoundDefID = 0;
        ElevatorMovingSoundDefID = 0;

        //ZONE_PARAM(ElevatorPhysDefClass, CallZones[ZONE_LOWER_CALL], "LowerCallZone");
        //ZONE_PARAM(ElevatorPhysDefClass, CallZones[ZONE_LOWER_INSIDE], "LowerInsideZone");
        //ZONE_PARAM(ElevatorPhysDefClass, CallZones[ZONE_UPPER_CALL], "UpperCallZone");
        //ZONE_PARAM(ElevatorPhysDefClass, CallZones[ZONE_UPPER_INSIDE], "UpperInsideZone");
        //EDITABLE_PARAM(ElevatorPhysDefClass, ParameterClass.TYPE_FLOAT, CloseDelay);

        //PARAM_SEPARATOR(ElevatorPhysDefClass, "Frame Numbers");
        //EDITABLE_PARAM(ElevatorPhysDefClass, ParameterClass.TYPE_FLOAT, DoorClosedTop_FrameNum);
        //EDITABLE_PARAM(ElevatorPhysDefClass, ParameterClass.TYPE_FLOAT, DoorOpeningBottom_FrameNum);
        //EDITABLE_PARAM(ElevatorPhysDefClass, ParameterClass.TYPE_FLOAT, ElevatorStartTop_FrameNum);
        //EDITABLE_PARAM(ElevatorPhysDefClass, ParameterClass.TYPE_FLOAT, ElevatorStoppedBottom_FrameNum);

        //PARAM_SEPARATOR(ElevatorPhysDefClass, "Sounds");
        //EDITABLE_PARAM(ElevatorPhysDefClass, ParameterClass.TYPE_SOUNDDEFINITIONID, DoorOpenSoundDefID);
        //EDITABLE_PARAM(ElevatorPhysDefClass, ParameterClass.TYPE_SOUNDDEFINITIONID, DoorCloseSoundDefID);
        //EDITABLE_PARAM(ElevatorPhysDefClass, ParameterClass.TYPE_SOUNDDEFINITIONID, DoorUnlockSoundDefID);
        //EDITABLE_PARAM(ElevatorPhysDefClass, ParameterClass.TYPE_SOUNDDEFINITIONID, DoorAccessDeniedSoundDefID);
        //EDITABLE_PARAM(ElevatorPhysDefClass, ParameterClass.TYPE_SOUNDDEFINITIONID, ElevatorMovingSoundDefID);
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_ELEVATORPHYSDEF;
    public override string Get_Type_Name() { return "ElevatorPhysDef"; }
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
        // Original creation code (commented out for reference):
        // ElevatorPhysClass elevator = new ElevatorPhysClass;
        // elevator.Init(this);
        // return elevator;
        throw new NotImplementedException();
    }
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        csave.WriteMicro(MICROCHUNKID_DEF_CLOSE_DELAY, CloseDelay);
        //WRITE_MICRO_CHUNK( csave, MICROCHUNKID_DEF_LOCK_CODE, LockCode );
        csave.WriteMicro(MICROCHUNKID_DEF_LOWER_CALL_ZONE, CallZones[ZONE_LOWER_CALL]);
        csave.WriteMicro(MICROCHUNKID_DEF_LOWER_INSIDE_ZONE, CallZones[ZONE_LOWER_INSIDE]);
        csave.WriteMicro(MICROCHUNKID_DEF_UPPER_CALL_ZONE, CallZones[ZONE_UPPER_CALL]);
        csave.WriteMicro(MICROCHUNKID_DEF_UPPER_INSIDE_ZONE, CallZones[ZONE_UPPER_INSIDE]);
        csave.WriteMicro(MICROCHUNKID_DEF_DOORCLOSED_FRAMENUM, DoorClosedTop_FrameNum);
        csave.WriteMicro(MICROCHUNKID_DEF_DOOROPENING_FRAMENUM, DoorOpeningBottom_FrameNum);
        csave.WriteMicro(MICROCHUNKID_DEF_ELEVATOR_START_FRAMENUM, ElevatorStartTop_FrameNum);
        csave.WriteMicro(MICROCHUNKID_DEF_ELEVATOR_STOPPED_FRAMENUM, ElevatorStoppedBottom_FrameNum);
        csave.WriteMicro(MICROCHUNKID_DEF_DOOR_OPEN_SOUNDID, DoorOpenSoundDefID);
        csave.WriteMicro(MICROCHUNKID_DEF_DOOR_CLOSE_SOUNDID, DoorCloseSoundDefID);
        csave.WriteMicro(MICROCHUNKID_DEF_DOOR_UNLOCK_SOUNDID, DoorUnlockSoundDefID);
        csave.WriteMicro(MICROCHUNKID_DEF_DOOR_ACCESS_DENIED_SOUNDID, DoorAccessDeniedSoundDefID);
        csave.WriteMicro(MICROCHUNKID_DEF_ELEVATOR_MOVING_SOUNDID, ElevatorMovingSoundDefID);

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
                            case MICROCHUNKID_DEF_CLOSE_DELAY:
                                cload.Read(ref CloseDelay);
                                break;
                            case MICROCHUNKID_DEF_LOCK_CODE:
                                cload.Read(ref LockCode);
                                break;
                            case MICROCHUNKID_DEF_LOWER_CALL_ZONE:
                                cload.Read(ref CallZones[ZONE_LOWER_CALL]);
                                break;
                            case MICROCHUNKID_DEF_LOWER_INSIDE_ZONE:
                                cload.Read(ref CallZones[ZONE_LOWER_INSIDE]);
                                break;
                            case MICROCHUNKID_DEF_UPPER_CALL_ZONE:
                                cload.Read(ref CallZones[ZONE_UPPER_CALL]);
                                break;
                            case MICROCHUNKID_DEF_UPPER_INSIDE_ZONE:
                                cload.Read(ref CallZones[ZONE_UPPER_INSIDE]);
                                break;
                            case MICROCHUNKID_DEF_DOORCLOSED_FRAMENUM:
                                cload.Read(ref DoorClosedTop_FrameNum);
                                break;
                            case MICROCHUNKID_DEF_DOOROPENING_FRAMENUM:
                                cload.Read(ref DoorOpeningBottom_FrameNum);
                                break;
                            case MICROCHUNKID_DEF_ELEVATOR_START_FRAMENUM:
                                cload.Read(ref ElevatorStartTop_FrameNum);
                                break;
                            case MICROCHUNKID_DEF_ELEVATOR_STOPPED_FRAMENUM:
                                cload.Read(ref ElevatorStoppedBottom_FrameNum);
                                break;
                            case MICROCHUNKID_DEF_DOOR_OPEN_SOUNDID:
                                cload.Read(ref DoorOpenSoundDefID);
                                break;
                            case MICROCHUNKID_DEF_DOOR_CLOSE_SOUNDID:
                                cload.Read(ref DoorCloseSoundDefID);
                                break;
                            case MICROCHUNKID_DEF_DOOR_UNLOCK_SOUNDID:
                                cload.Read(ref DoorUnlockSoundDefID);
                                break;
                            case MICROCHUNKID_DEF_DOOR_ACCESS_DENIED_SOUNDID:
                                cload.Read(ref DoorAccessDeniedSoundDefID);
                                break;
                            case MICROCHUNKID_DEF_ELEVATOR_MOVING_SOUNDID:
                                cload.Read(ref ElevatorMovingSoundDefID);
                                break;

                            default:
                                Console.WriteLine("Unrecognized ElevatorPhys Variable chunkID: 0x{0:X8} (depth {1}) in {2}", cload.Cur_Chunk_ID, cload.Cur_Chunk_Depth, nameof(ElevatorPhysDefClass));
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unrecognized ElevatorPhys chunkID: 0x{0:X8} (depth {1}) in {2}", cload.Cur_Chunk_ID, cload.Cur_Chunk_Depth, nameof(ElevatorPhysDefClass));
                    break;

            }
            cload.Close_Chunk();
        }

        return true;
    }
    public override PersistFactoryClass Get_Factory() => _persistFactory;

    public OBBoxClass Get_Zone(ELEVATOR_ZONE id) { return CallZones[(int)id]; }

    //DECLARE_EDITABLE(ElevatorPhysDefClass, AccessiblePhysDefClass);

    
    OBBoxClass[] CallZones;
    float CloseDelay;

    float DoorClosedTop_FrameNum;
    float DoorOpeningBottom_FrameNum;
    float ElevatorStartTop_FrameNum;
    float ElevatorStoppedBottom_FrameNum;

    int DoorOpenSoundDefID;
    int DoorCloseSoundDefID;
    int DoorUnlockSoundDefID;
    int DoorAccessDeniedSoundDefID;
    int ElevatorMovingSoundDefID;

    public enum ELEVATOR_ZONE
    {
        ZONE_LOWER_CALL = 0,
        ZONE_LOWER_INSIDE,
        ZONE_UPPER_CALL,
        ZONE_UPPER_INSIDE,
        ZONE_MAX
    }

    // Provide constants matching original C-style names for existing code that indexes with them
    private const int ZONE_LOWER_CALL = 0;
    private const int ZONE_LOWER_INSIDE = 1;
    private const int ZONE_UPPER_CALL = 2;
    private const int ZONE_UPPER_INSIDE = 3;
    private const int ZONE_MAX = 4;

    // Replaced file-scope enum with private consts scoped to the class
    private const uint CHUNKID_DEF_OLD_PARENT = 714001418u;
    private const uint CHUNKID_DEF_VARIABLES = 714001419u;
    private const uint CHUNKID_DEF_PARENT = 714001420u;

    private const int MICROCHUNKID_DEF_CLOSE_DELAY = 1;
    private const int MICROCHUNKID_DEF_LOCK_CODE = 2;
    private const int MICROCHUNKID_DEF_UPPER_CALL_ZONE = 3;
    private const int MICROCHUNKID_DEF_UPPER_INSIDE_ZONE = 4;
    private const int MICROCHUNKID_DEF_LOWER_CALL_ZONE = 5;
    private const int MICROCHUNKID_DEF_LOWER_INSIDE_ZONE = 6;

    private const int MICROCHUNKID_DEF_DOORCLOSED_FRAMENUM = 7;
    private const int MICROCHUNKID_DEF_DOOROPENING_FRAMENUM = 8;
    private const int MICROCHUNKID_DEF_ELEVATOR_START_FRAMENUM = 9;
    private const int MICROCHUNKID_DEF_ELEVATOR_STOPPED_FRAMENUM = 10;
    private const int MICROCHUNKID_DEF_DOOR_OPEN_SOUNDID = 11;
    private const int MICROCHUNKID_DEF_DOOR_CLOSE_SOUNDID = 12;
    private const int MICROCHUNKID_DEF_DOOR_UNLOCK_SOUNDID = 13;
    private const int MICROCHUNKID_DEF_DOOR_ACCESS_DENIED_SOUNDID = 14;
    private const int MICROCHUNKID_DEF_ELEVATOR_MOVING_SOUNDID = 15;
}


/*
Original C++-style free functions and enum (commented out):

ElevatorPhysDefClass.ElevatorPhysDefClass() :
	AccessiblePhysDefClass(),
	CloseDelay(2),
	/LowerCallZone(Vector3( 0,0,0), Vector3( 1,1,1 ) ),
	LowerInsideZone(Vector3( 0,0,0), Vector3( 1,1,1 ) ),
	UpperCallZone(Vector3( 0,0,0), Vector3( 1,1,1 ) ),
	UpperInsideZone(Vector3( 0,0,0), Vector3( 1,1,1 ) ),/
	DoorClosedTop_FrameNum(-1),
	DoorOpeningBottom_FrameNum(-1),
	ElevatorStartTop_FrameNum(-1),
	ElevatorStoppedBottom_FrameNum(-1),
	DoorOpenSoundDefID(0),
	DoorCloseSoundDefID(0),
	DoorUnlockSoundDefID(0),
	DoorAccessDeniedSoundDefID(0),
	ElevatorMovingSoundDefID(0)
{
    for (int index = 0; index < ZONE_MAX; index++)
    {
        CallZones[index].Center = Vector3(0, 0, 0);
        CallZones[index].Extent = Vector3(1, 1, 1);
    }

    ZONE_PARAM(ElevatorPhysDefClass, CallZones[ZONE_LOWER_CALL], "LowerCallZone");
    ZONE_PARAM(ElevatorPhysDefClass, CallZones[ZONE_LOWER_INSIDE], "LowerInsideZone");
    ZONE_PARAM(ElevatorPhysDefClass, CallZones[ZONE_UPPER_CALL], "UpperCallZone");
    ZONE_PARAM(ElevatorPhysDefClass, CallZones[ZONE_UPPER_INSIDE], "UpperInsideZone");
    //EDITABLE_PARAM(ElevatorPhysDefClass, ParameterClass.TYPE_FLOAT, CloseDelay);

    PARAM_SEPARATOR(ElevatorPhysDefClass, "Frame Numbers");
    //EDITABLE_PARAM(ElevatorPhysDefClass, ParameterClass.TYPE_FLOAT, DoorClosedTop_FrameNum);
    //EDITABLE_PARAM(ElevatorPhysDefClass, ParameterClass.TYPE_FLOAT, DoorOpeningBottom_FrameNum);
    //EDITABLE_PARAM(ElevatorPhysDefClass, ParameterClass.TYPE_FLOAT, ElevatorStartTop_FrameNum);
    //EDITABLE_PARAM(ElevatorPhysDefClass, ParameterClass.TYPE_FLOAT, ElevatorStoppedBottom_FrameNum);

    PARAM_SEPARATOR(ElevatorPhysDefClass, "Sounds");
    //EDITABLE_PARAM(ElevatorPhysDefClass, ParameterClass.TYPE_SOUNDDEFINITIONID, DoorOpenSoundDefID);
    //EDITABLE_PARAM(ElevatorPhysDefClass, ParameterClass.TYPE_SOUNDDEFINITIONID, DoorCloseSoundDefID);
    //EDITABLE_PARAM(ElevatorPhysDefClass, ParameterClass.TYPE_SOUNDDEFINITIONID, DoorUnlockSoundDefID);
    //EDITABLE_PARAM(ElevatorPhysDefClass, ParameterClass.TYPE_SOUNDDEFINITIONID, DoorAccessDeniedSoundDefID);
    //EDITABLE_PARAM(ElevatorPhysDefClass, ParameterClass.TYPE_SOUNDDEFINITIONID, ElevatorMovingSoundDefID);
}

PersistClass ElevatorPhysDefClass.Create()
{
    ElevatorPhysClass elevator = new ElevatorPhysClass;
    elevator.Init(this);
    return elevator;
}

enum {
    CHUNKID_DEF_OLD_PARENT = 714001418,
    CHUNKID_DEF_VARIABLES,
    CHUNKID_DEF_PARENT,

    MICROCHUNKID_DEF_CLOSE_DELAY = 1,
    MICROCHUNKID_DEF_LOCK_CODE,
    MICROCHUNKID_DEF_UPPER_CALL_ZONE,
    MICROCHUNKID_DEF_UPPER_INSIDE_ZONE,
    MICROCHUNKID_DEF_LOWER_CALL_ZONE,
    MICROCHUNKID_DEF_LOWER_INSIDE_ZONE,

    MICROCHUNKID_DEF_DOORCLOSED_FRAMENUM,
    MICROCHUNKID_DEF_DOOROPENING_FRAMENUM,
    MICROCHUNKID_DEF_ELEVATOR_START_FRAMENUM,
    MICROCHUNKID_DEF_ELEVATOR_STOPPED_FRAMENUM,
    MICROCHUNKID_DEF_DOOR_OPEN_SOUNDID,
    MICROCHUNKID_DEF_DOOR_CLOSE_SOUNDID,
    MICROCHUNKID_DEF_DOOR_UNLOCK_SOUNDID,
    MICROCHUNKID_DEF_DOOR_ACCESS_DENIED_SOUNDID,
    MICROCHUNKID_DEF_ELEVATOR_MOVING_SOUNDID
};
*/
