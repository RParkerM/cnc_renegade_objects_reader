using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.Definitions;

[RegisterDefinition(ChunkId.CHUNKID_GLOBAL_SETTINGS_DEF_HUMAN_ANIM_OVERRIDE)]
public partial class HumanAnimOverrideDef : DefinitionClass
{
    public HumanAnimOverrideDef()
    {
        //EDITABLE_PARAM(HumanAnimOverrideDef, ParameterClass::TYPE_STRING, RunEmptyHands);
        //EDITABLE_PARAM(HumanAnimOverrideDef, ParameterClass::TYPE_STRING, WalkEmptyHands);
        //EDITABLE_PARAM(HumanAnimOverrideDef, ParameterClass::TYPE_STRING, RunAtChest);
        //EDITABLE_PARAM(HumanAnimOverrideDef, ParameterClass::TYPE_STRING, WalkAtChest);
        //EDITABLE_PARAM(HumanAnimOverrideDef, ParameterClass::TYPE_STRING, RunAtHip);
        //EDITABLE_PARAM(HumanAnimOverrideDef, ParameterClass::TYPE_STRING, WalkAtHip);
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GLOBAL_SETTINGS_DEF_HUMAN_ANIM_OVERRIDE;

    public override PersistClass? Create()
    {
        //WWASSERT(0);
        return null;
    }

    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_HAO_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_HAO_DEF_VARIABLES);
        ArgumentNullException.ThrowIfNull(RunEmptyHands);
        ArgumentNullException.ThrowIfNull(WalkEmptyHands);
        ArgumentNullException.ThrowIfNull(RunAtChest);
        ArgumentNullException.ThrowIfNull(WalkAtChest);
        ArgumentNullException.ThrowIfNull(RunAtHip);
        ArgumentNullException.ThrowIfNull(WalkAtHip);
        csave.WriteMicroString(MICROCHUNKID_HAO_DEF_RUN_EMPTY_HANDS, RunEmptyHands);
        csave.WriteMicroString(MICROCHUNKID_HAO_DEF_WALK_EMPTY_HANDS, WalkEmptyHands);
        csave.WriteMicroString(MICROCHUNKID_HAO_DEF_RUN_AT_CHEST, RunAtChest);
        csave.WriteMicroString(MICROCHUNKID_HAO_DEF_WALK_AT_CHEST, WalkAtChest);
        csave.WriteMicroString(MICROCHUNKID_HAO_DEF_RUN_AT_HIP, RunAtHip);
        csave.WriteMicroString(MICROCHUNKID_HAO_DEF_WALK_AT_HIP, WalkAtHip);
        csave.End_Chunk();

        return true;
    }

    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {
                case CHUNKID_HAO_DEF_PARENT:
                    base.Load(cload);
                    break;

                case CHUNKID_HAO_DEF_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case MICROCHUNKID_HAO_DEF_RUN_EMPTY_HANDS:
                                cload.ReadMicroChunkWWString(out RunEmptyHands);
                                break;
                            case MICROCHUNKID_HAO_DEF_WALK_EMPTY_HANDS:
                                cload.ReadMicroChunkWWString(out WalkEmptyHands);
                                break;
                            case MICROCHUNKID_HAO_DEF_RUN_AT_CHEST:
                                cload.ReadMicroChunkWWString(out RunAtChest);
                                break;
                            case MICROCHUNKID_HAO_DEF_WALK_AT_CHEST:
                                cload.ReadMicroChunkWWString(out WalkAtChest);
                                break;
                            case MICROCHUNKID_HAO_DEF_RUN_AT_HIP:
                                cload.ReadMicroChunkWWString(out RunAtHip);
                                break;
                            case MICROCHUNKID_HAO_DEF_WALK_AT_HIP:
                                cload.ReadMicroChunkWWString(out WalkAtHip);
                                break;
                            default:
                                Console.WriteLine("Unhandled HumanAnimOverrideDef Variable chunkID\n");
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unhandled HumanAnimOverrideDef chunkID\n");
                    break;
            }
            cload.Close_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    protected string? RunEmptyHands;
    protected string? WalkEmptyHands;
    protected string? RunAtChest;
    protected string? WalkAtChest;
    protected string? RunAtHip;
    protected string? WalkAtHip;


    private const uint CHUNKID_HAO_DEF_PARENT = 726011912;
    private const uint CHUNKID_HAO_DEF_VARIABLES = 726011913;

    private const byte MICROCHUNKID_HAO_DEF_RUN_EMPTY_HANDS = 1;
    private const byte MICROCHUNKID_HAO_DEF_WALK_EMPTY_HANDS = 2;
    private const byte MICROCHUNKID_HAO_DEF_RUN_AT_CHEST = 3;
    private const byte MICROCHUNKID_HAO_DEF_WALK_AT_CHEST = 4;
    private const byte MICROCHUNKID_HAO_DEF_RUN_AT_HIP = 5;
    private const byte MICROCHUNKID_HAO_DEF_WALK_AT_HIP = 6;
}
