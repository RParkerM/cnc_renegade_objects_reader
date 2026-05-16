using System;
using RenData.ChunkIO;
using RenData.SaveLoad;
using RenData.IDs;
using RenData.Dialogue;

namespace RenData.GameObjDef;

[RegisterDefinition(ChunkId.CHUNKID_GAME_OBJECT_DEF_SOLDIER)]
public partial class SoldierGameObjDef : SmartGameObjDef
{
    public SoldierGameObjDef()
    {
        TurnRate = WWMath.DEG_TO_RADF(360.0f);
        JumpVelocity = 2;
        SkeletonHeight = 0;
        SkeletonWidth = 0;
        UseInnateBehavior = true;
        InnateAggressiveness = 0.5f;
        InnateTakeCoverProbability = 0.5f;
        InnateIsStationary = false;
        HumanAnimOverrideDefID = 0;
        HumanLoiterCollectionDefID = 0;
        DeathSoundPresetID = 0;

        AllowInnateConversations = true;
        DialogList = [.. Enumerable.Range(0, 20).Select(_ => new DialogueClass())];
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GAME_OBJECT_DEF_SOLDIER;

    public override PersistClass Create()
    {
        // Original C++:
        // SoldierGameObj* obj = new SoldierGameObj;
        // obj.Init(*this);
        // return obj;
        throw new NotImplementedException();
    }

    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave); // SmartGameObjDef.Save(csave);
        csave.End_Chunk();

        for (int index = 0; index < DialogList.Count; index++)
        {
            csave.Begin_Chunk(CHUNKID_DEF_DIALOG_ENTRY);
            DialogList[index].Save(csave);
            csave.End_Chunk();
        }

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        csave.WriteMicro(MICROCHUNKID_DEF_TURN_RATE, TurnRate);
        csave.WriteMicro(MICROCHUNKID_DEF_JUMP_VELOCITY, JumpVelocity);
        csave.WriteMicro(MICROCHUNKID_DEF_SKELETON_HEIGHT, SkeletonHeight);
        csave.WriteMicro(MICROCHUNKID_DEF_SKELETON_WIDTH, SkeletonWidth);
        csave.WriteMicro(MICROCHUNKID_DEF_USE_INNATE_BEHAVIOR, UseInnateBehavior);
        //WRITE_MICRO_CHUNK( csave, MICROCHUNKID_DEF_USE_INNATE_CONVERSATIONS, UseInnateConversations );
        csave.WriteMicro(MICROCHUNKID_DEF_INNATE_AGGRESSIVENESS, InnateAggressiveness);
        csave.WriteMicro(MICROCHUNKID_DEF_INNATE_TAKE_COVER_PROB, InnateTakeCoverProbability);
        csave.WriteMicro(MICROCHUNKID_DEF_INNATE_IS_STATIONARY, InnateIsStationary);
        ArgumentNullException.ThrowIfNull(FirstPersonHands);
        csave.WriteMicroString(MICROCHUNKID_DEF_FIRST_PERSON_HANDS, FirstPersonHands);
        csave.WriteMicro(MICROCHUNKID_DEF_HUMAN_ANIM_OVERRIDE_DEF_ID, HumanAnimOverrideDefID);
        csave.WriteMicro(MICROCHUNKID_DEF_HUMAN_LOITER_COLLECTION_DEF_ID, HumanLoiterCollectionDefID);
        csave.WriteMicro(MICROCHUNKID_DEF_DEATH_SOUND_PRESET, DeathSoundPresetID);

        csave.End_Chunk();

        return true;
    }

    public override bool Load(ChunkLoadClass cload)
    {
        int dialog_index = 0;

        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {

                case CHUNKID_DEF_PARENT:
                    base.Load(cload); // SmartGameObjDef.Load(cload);
                    break;

                case CHUNKID_DEF_DIALOG_ENTRY:
                    if (dialog_index < DialogList.Count)
                    {
                        DialogList[dialog_index++].Load(cload);
                    }
                    break;

                case CHUNKID_DEF_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case MICROCHUNKID_DEF_TURN_RATE:
                                cload.Read(ref TurnRate);
                                break;
                            case MICROCHUNKID_DEF_JUMP_VELOCITY:
                                cload.Read(ref JumpVelocity);
                                break;
                            case MICROCHUNKID_DEF_SKELETON_HEIGHT:
                                cload.Read(ref SkeletonHeight);
                                break;
                            case MICROCHUNKID_DEF_SKELETON_WIDTH:
                                cload.Read(ref SkeletonWidth);
                                break;
                            case MICROCHUNKID_DEF_USE_INNATE_BEHAVIOR:
                                cload.Read(ref UseInnateBehavior);
                                break;
                            //READ_MICRO_CHUNK( cload, MICROCHUNKID_DEF_USE_INNATE_CONVERSATIONS, UseInnateConversations );
                            case MICROCHUNKID_DEF_INNATE_AGGRESSIVENESS:
                                cload.Read(ref InnateAggressiveness);
                                break;
                            case MICROCHUNKID_DEF_INNATE_TAKE_COVER_PROB:
                                cload.Read(ref InnateTakeCoverProbability);
                                break;
                            case MICROCHUNKID_DEF_INNATE_IS_STATIONARY:
                                cload.Read(ref InnateIsStationary);
                                break;
                            case MICROCHUNKID_DEF_FIRST_PERSON_HANDS:
                                cload.ReadMicroChunkWWString(out FirstPersonHands);
                                break;
                            case MICROCHUNKID_DEF_ORATOR_TYPE:
                                cload.Read(ref OratorType);
                                break;
                            case MICROCHUNKID_DEF_HUMAN_ANIM_OVERRIDE_DEF_ID:
                                cload.Read(ref HumanAnimOverrideDefID);
                                break;
                            case MICROCHUNKID_DEF_HUMAN_LOITER_COLLECTION_DEF_ID:
                                cload.Read(ref HumanLoiterCollectionDefID);
                                break;
                            case MICROCHUNKID_DEF_DEATH_SOUND_PRESET:
                                cload.Read(ref DeathSoundPresetID);
                                break;

                            default:
                                Console.WriteLine("Unrecognized SoldierDef Variable microchunkID\n");
                                break;

                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unrecognized SoldierDef chunkID\n");
                    break;

            }
            cload.Close_Chunk();
        }

        return true;
    }

    public List<DialogueClass> Get_Dialog_List() { return DialogList; }

    public override PersistFactoryClass Get_Factory() => _persistFactory!;

    protected float TurnRate;
    protected float JumpVelocity;
    protected float SkeletonHeight;
    protected float SkeletonWidth;
    protected bool UseInnateBehavior;
    protected float InnateAggressiveness;
    protected float InnateTakeCoverProbability;
    protected bool InnateIsStationary;
    protected List<DialogueClass> DialogList;
    protected string FirstPersonHands = string.Empty;
    protected int HumanAnimOverrideDefID;
    protected int HumanLoiterCollectionDefID;
    protected int DeathSoundPresetID;

    private const int CHUNKID_DEF_PARENT = 909991656;
    private const int CHUNKID_DEF_VARIABLES = 909991657;
    private const int CHUNKID_DEF_DIALOG_ENTRY = 909991658;

    private const int MICROCHUNKID_DEF_TURN_RATE = 1;
    private const int MICROCHUNKID_DEF_JUMP_VELOCITY = 2;
    private const int MICROCHUNKID_DEF_SKELETON_HEIGHT = 3;
    private const int MICROCHUNKID_DEF_SKELETON_WIDTH = 4;
    private const int MICROCHUNKID_DEF_USE_INNATE_BEHAVIOR = 5;
    private const int MICROCHUNKID_DEF_INNATE_AGGRESSIVENESS = 6;
    private const int MICROCHUNKID_DEF_INNATE_TAKE_COVER_PROB = 7;
    private const int XXXMICROCHUNKID_DEF_INNATE_ESCORT_ID = 8;
    private const int XXXMICROCHUNKID_DEF_INNATE_ESCORT_RANGE = 9;
    private const int MICROCHUNKID_DEF_FIRST_PERSON_HANDS = 10;
    private const int XXXMICROCHUNKID_DEF_CORPSE_PERSIST_TIME = 11;
    private const int MICROCHUNKID_DEF_USE_INNATE_CONVERSATIONS = 12;
    private const int MICROCHUNKID_DEF_INNATE_IS_STATIONARY = 13;
    private const int MICROCHUNKID_DEF_ORATOR_TYPE = 14;
    private const int MICROCHUNKID_DEF_HUMAN_ANIM_OVERRIDE_DEF_ID = 15;
    private const int MICROCHUNKID_DEF_DEATH_SOUND_PRESET = 16;
    private const int MICROCHUNKID_DEF_HUMAN_LOITER_COLLECTION_DEF_ID = 17;
}
