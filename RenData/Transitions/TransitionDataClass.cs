using RenData.ChunkIO;
using System.Diagnostics;

namespace RenData.Transitions;

public class TransitionDataClass
{
    public TransitionDataClass()
    {
        Type = StyleType.LADDER_EXIT_TOP;
    }

    public bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_VARIABLES);

        csave.WriteMicro(MICROCHUNKID_TYPE, Type);
        csave.WriteMicro(MICROCHUNKID_ZONE, Zone);
        csave.WriteMicro(MICROCHUNKID_ENDING_TM, EndingTM);
        ArgumentNullException.ThrowIfNull(AnimationName);
        csave.WriteMicroString(MICROCHUNKID_ANIMATION_NAME, AnimationName);

        csave.End_Chunk();
        return true;
    }
    public bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {

                case CHUNKID_VARIABLES:
                    {
                        while (cload.Open_Micro_Chunk())
                        {
                            switch (cload.Cur_Micro_Chunk_ID)
                            {

                                case MICROCHUNKID_TYPE:
                                    cload.Read(ref Type);
                                    break;
                                case MICROCHUNKID_ZONE:
                                    cload.Read(ref Zone);
                                    break;
                                case MICROCHUNKID_ENDING_TM:
                                    cload.Read(ref EndingTM);
                                    break;
                                case MICROCHUNKID_ANIMATION_NAME:
                                    AnimationName = cload.ReadMicroChunkWWString();
                                    break;

                                default:
                                    Console.WriteLine($"Unrecognized Transition Variable chunkID {cload.Cur_Micro_Chunk_ID}");
                                    break;
                            }
                            cload.Close_Micro_Chunk();
                        }
                        break;
                    }

                default:
                    Console.WriteLine($"Unrecognized Transition chunkID {cload.Cur_Chunk_ID}");
                    break;

            }
            cload.Close_Chunk();
        }

        switch (Type)
        {
            case StyleType.LEGACY_VEHICLE_ENTER_0:
            case StyleType.LEGACY_VEHICLE_ENTER_1:
                Type = StyleType.VEHICLE_ENTER;
                break;
            case StyleType.LEGACY_VEHICLE_EXIT_0:
            case StyleType.LEGACY_VEHICLE_EXIT_1:
                Type = StyleType.VEHICLE_EXIT;
                break;
        }

        return true;
    }

    public enum StyleType
    {
        DISABLED = -1,
        LADDER_EXIT_TOP = 0,
        LADDER_EXIT_BOTTOM,
        LADDER_ENTER_TOP,
        LADDER_ENTER_BOTTOM,
        LEGACY_VEHICLE_ENTER_0,
        LEGACY_VEHICLE_ENTER_1,
        LEGACY_VEHICLE_EXIT_0,
        LEGACY_VEHICLE_EXIT_1,
        VEHICLE_ENTER,
        VEHICLE_EXIT,
        NUM_TRANSITION_TYPE
    }

    public static int Get_Num_Types() { return (int)StyleType.NUM_TRANSITION_TYPE; }
    public static string Get_Type_Name(StyleType type)
    {
        Debug.Assert(((int)type) < Get_Num_Types());
        return TransitionTypeNames[((int)type)];
    }

    public StyleType Get_Type() { return Type; }
    public void Set_Type(StyleType type) { Type = type; }

    public OBBoxClass Get_Zone() { return Zone; }
    public void Set_Zone(OBBoxClass zone) { Zone = zone; }

    public string Get_Animation_Name() { 
        ArgumentNullException.ThrowIfNull(AnimationName);
        return AnimationName; 
    }
    public void Set_Animation_Name(string name) { AnimationName = name; }

    public Matrix3D Get_Ending_TM() { return EndingTM; }
    public void Set_Ending_TM(Matrix3D tm) { EndingTM = tm; }

    private StyleType Type;
    private OBBoxClass Zone;
    private string? AnimationName;
    private Matrix3D EndingTM;

    private static readonly string[] TransitionTypeNames =
    [
        "LADDER_EXIT_TOP",
        "LADDER_EXIT_BOTTOM",
        "LADDER_ENTER_TOP",
        "LADDER_ENTER_BOTTOM",
        "LEGACY_VEHICLE_ENTER_0",
        "LEGACY_VEHICLE_ENTER_1",
        "LEGACY_VEHICLE_EXIT_0",
        "LEGACY_VEHICLE_EXIT_1",
        "VEHICLE_ENTER",
        "VEHICLE_EXIT",
    ];
    private const int CHUNKID_VARIABLES = 0x11051106;
    private const int MICROCHUNKID_TYPE = 1;
    private const int MICROCHUNKID_ZONE = 2;
    private const int MICROCHUNKID_ANIMATION_NAME = 3;
    private const int MICROCHUNKID_ENDING_TM = 4;
    private const int MICROCHUNKID_LADDER_INDEX = 5;
}
