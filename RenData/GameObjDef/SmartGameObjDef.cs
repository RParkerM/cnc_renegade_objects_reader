using RenData.ChunkIO;

namespace RenData.GameObjDef;

public abstract class SmartGameObjDef : ArmedGameObjDef
{
    public SmartGameObjDef()
    {
        SightRange = 0;
        SightArc = WWMath.DEG_TO_RADF(0);
        ListenerScale = 1;
        //{
        //    EDITABLE_PARAM(SmartGameObjDef, ParameterClass::TYPE_FLOAT, SightRange);
        //    EDITABLE_PARAM(SmartGameObjDef, ParameterClass::TYPE_ANGLE, SightArc);
        //    EDITABLE_PARAM(SmartGameObjDef, ParameterClass::TYPE_FLOAT, ListenerScale);
        //    EDITABLE_PARAM(SmartGameObjDef, ParameterClass::TYPE_BOOL, IsStealthUnit);
        //}
    }

    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_ARMEDGAMEOBJ_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        csave.WriteMicro(MICROCHUNKID_DEF_SIGHT_RANGE, SightRange);
        csave.WriteMicro(MICROCHUNKID_DEF_SIGHT_ARC, SightArc);
        csave.WriteMicro(MICROCHUNKID_DEF_LISTENER_SCALE, ListenerScale);
        csave.WriteMicro(MICROCHUNKID_DEF_IS_STEALTH_UNIT, IsStealthUnit);
        csave.End_Chunk();

        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {

                case CHUNKID_DEF_ARMEDGAMEOBJ_PARENT:
                    base.Load(cload);
                    break;

                case CHUNKID_DEF_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {

                            case MICROCHUNKID_DEF_SIGHT_RANGE:
                                cload.Read(ref SightRange);
                                break;
                            case MICROCHUNKID_DEF_SIGHT_ARC:
                                cload.Read(ref SightArc);
                                break;
                            case MICROCHUNKID_DEF_LISTENER_SCALE:
                                cload.Read(ref ListenerScale);
                                break;
                            case LEGACY_MICROCHUNKID_DEF_INFO_ICON_TEXTURE_FILENAME:
                                InfoIconTextureFilename = cload.ReadMicroChunkWWString();
                                break;
                            case MICROCHUNKID_DEF_IS_STEALTH_UNIT:
                                cload.Read(ref IsStealthUnit);
                                break;

                            default:
                                Console.WriteLine($"Unrecognized SmartDef Variable chunkID {cload.Cur_Micro_Chunk_ID}");
                                break;

                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine($"Unrecognized SmartDef chunkID {cload.Cur_Chunk_ID}");
                    break;

            }
            cload.Close_Chunk();
        }

        return true;
    }

    //DECLARE_EDITABLE(SmartGameObjDef, ArmedGameObjDef );

    protected float SightRange;
    protected float SightArc;
    protected float ListenerScale;
    protected bool IsStealthUnit;

    private const int XXX_CHUNKID_DEF_PHYSICALGAMEOBJ_PARENT = 909991656;
    private const int CHUNKID_DEF_VARIABLES = 909991657;
    private const int CHUNKID_DEF_ARMEDGAMEOBJ_PARENT = 909991658;

    private const int MICROCHUNKID_DEF_SIGHT_RANGE = 9;
    private const int MICROCHUNKID_DEF_SIGHT_ARC = 10;
    private const int MICROCHUNKID_DEF_LISTENER_SCALE = 17;
    private const int LEGACY_MICROCHUNKID_DEF_INFO_ICON_TEXTURE_FILENAME = 18;
    private const int MICROCHUNKID_DEF_IS_STEALTH_UNIT = 19;
}
