using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.GameObjDef;

[RegisterDefinition(ChunkId.CHUNKID_GAME_OBJECT_DEF_SPECIAL_EFFECTS)]
public partial class SpecialEffectsGameObjDef : PhysicalGameObjDef
{
    public SpecialEffectsGameObjDef()
    {
        SoundDefID = 0;

        //MODEL_DEF_PARAM(SpecialEffectsGameObjDef, PhysDefID, "TimedDecorationPhysDef");
        //EDITABLE_PARAM(SpecialEffectsGameObjDef, ParameterClass::TYPE_STRING, AnimationName);
        //EDITABLE_PARAM(SpecialEffectsGameObjDef, ParameterClass::TYPE_SOUNDDEFINITIONID, SoundDefID);
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GAME_OBJECT_DEF_SPECIAL_EFFECTS;

    public override PersistClass Create()
    {
        //SpecialEffectsGameObj* obj = new SpecialEffectsGameObj;
        //obj->Init(*this);
        //return obj;
        throw new NotImplementedException();
    }

    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        ArgumentNullException.ThrowIfNull(AnimationName);
        csave.WriteMicroString(VARID_DEF_ANIMATION_NAME, AnimationName);
        csave.WriteMicro(VARID_DEF_SOUNDID, SoundDefID);
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
                            case VARID_DEF_ANIMATION_NAME:
                                cload.ReadMicroChunkWWString(out AnimationName);
                                break;
                            case VARID_DEF_SOUNDID:
                                cload.Read(ref SoundDefID);
                                break;
                            default:
                                Console.WriteLine("Unrecognized SpecialEffectsGameObjDef Variable chunkID\n");
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unrecognized SpecialEffectsGameObjDef chunkID\n");
                    break;
            }
            cload.Close_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    protected string? AnimationName;
    protected int SoundDefID;


    private const uint CHUNKID_DEF_PARENT = 0x09010212;
    private const uint CHUNKID_DEF_VARIABLES = 0x09010213;

    private const byte VARID_DEF_ANIMATION_NAME = 1;
    private const byte VARID_DEF_SOUNDID = 2;
}

//SimplePersistFactoryClass<SpecialEffectsGameObjDef, CHUNKID_GAME_OBJECT_DEF_SPECIAL_EFFECTS> _SpecialEffectsGameObjDefPersistFactory;
//DECLARE_DEFINITION_FACTORY(SpecialEffectsGameObjDef, CLASSID_GAME_OBJECT_DEF_SPECIAL_EFFECTS, "Special Effects") _SpecialEffectsGameObjDefDefFactory;
