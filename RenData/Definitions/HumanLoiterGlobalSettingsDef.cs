using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.Definitions;

[RegisterDefinition(ChunkId.CHUNKID_GLOBAL_SETTINGS_DEF_HUMAN_LOITER)]
public partial class HumanLoiterGlobalSettingsDef : DefinitionClass
{
    public HumanLoiterGlobalSettingsDef()
    {
        ActivationDelay = 20;
        LoiterFrequency = 10;
        LoiterAnimList = [];

        //EDITABLE_PARAM(HumanLoiterGlobalSettingsDef, ParameterClass::TYPE_FLOAT, ActivationDelay);
        //EDITABLE_PARAM(HumanLoiterGlobalSettingsDef, ParameterClass::TYPE_FLOAT, LoiterFrequency);
        //EDITABLE_PARAM(HumanLoiterGlobalSettingsDef, ParameterClass::TYPE_FILENAMELIST, LoiterAnimList);
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GLOBAL_SETTINGS_DEF_HUMAN_LOITER;

    public override PersistClass? Create()
    {
        //WWASSERT(0);
        return null;
    }

    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_HL_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_HL_DEF_VARIABLES);
        csave.WriteMicro(MICROCHUNKID_HL_DEF_ACTIVATION_DELAY, ActivationDelay);
        csave.WriteMicro(MICROCHUNKID_HL_DEF_LOITER_FREQUENCY, LoiterFrequency);

        foreach (var entry in LoiterAnimList)
        {
            csave.WriteMicroString(MICROCHUNKID_HL_DEF_LOITER_ANIM_LIST_ENTRY, entry);
        }
        csave.End_Chunk();

        return true;
    }

    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {
                case CHUNKID_HL_DEF_PARENT:
                    base.Load(cload);
                    break;

                case CHUNKID_HL_DEF_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case MICROCHUNKID_HL_DEF_ACTIVATION_DELAY:
                                cload.Read(ref ActivationDelay);
                                break;
                            case MICROCHUNKID_HL_DEF_LOITER_FREQUENCY:
                                cload.Read(ref LoiterFrequency);
                                break;
                            case MICROCHUNKID_HL_DEF_LOITER_ANIM_LIST_ENTRY:
                                LoiterAnimList.Add(cload.ReadMicroChunkWWString());
                                break;
                            default:
                                Console.WriteLine("Unhandled HumanLoiter Variable chunkID\n");
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unhandled HumanLoiter chunkID\n");
                    break;
            }
            cload.Close_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    protected float ActivationDelay;
    protected float LoiterFrequency;
    protected List<string> LoiterAnimList;


    private const uint CHUNKID_HL_DEF_PARENT = 803001812;
    private const uint CHUNKID_HL_DEF_VARIABLES = 803001813;

    private const byte MICROCHUNKID_HL_DEF_ACTIVATION_DELAY = 1;
    private const byte MICROCHUNKID_HL_DEF_LOITER_FREQUENCY = 2;
    private const byte MICROCHUNKID_HL_DEF_LOITER_ANIM_LIST_ENTRY = 3;
}
