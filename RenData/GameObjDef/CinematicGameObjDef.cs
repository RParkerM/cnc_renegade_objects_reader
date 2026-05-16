using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.GameObjDef;

[RegisterDefinition(ChunkId.CHUNKID_GAME_OBJECT_DEF_CINEMATIC)]
public partial class CinematicGameObjDef : ArmedGameObjDef
{
    public CinematicGameObjDef()
    {
        SoundDefID = 0;
        AutoFireWeapon = false;
        DestroyAfterAnimation = true;
        CameraRelative = false;
        //{
        //            MODEL_DEF_PARAM(CinematicGameObjDef, PhysDefID, "DynamicAnimPhysDef");
        //            EDITABLE_PARAM(CinematicGameObjDef, ParameterClass::TYPE_SOUNDDEFINITIONID, SoundDefID);
        //            EDITABLE_PARAM(CinematicGameObjDef, ParameterClass::TYPE_STRING, SoundBoneName);
        //            FILENAME_PARAM(CinematicGameObjDef, AnimationName, "Animation", ".W3D");
        //            EDITABLE_PARAM(CinematicGameObjDef, ParameterClass::TYPE_BOOL, AutoFireWeapon);
        //            EDITABLE_PARAM(CinematicGameObjDef, ParameterClass::TYPE_BOOL, DestroyAfterAnimation);
        //            EDITABLE_PARAM(CinematicGameObjDef, ParameterClass::TYPE_BOOL, CameraRelative);
        //        }
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GAME_OBJECT_DEF_CINEMATIC;
    public override PersistClass Create()
    {
        //CinematicGameObj obj = new CinematicGameObj;
        //obj.Init(this);
        //return obj;
        throw new NotImplementedException();
    }
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        csave.WriteMicro(MICROCHUNKID_DEF_SOUND_DEF_ID, SoundDefID);
        ArgumentNullException.ThrowIfNull(SoundBoneName);
        csave.WriteMicroString(MICROCHUNKID_DEF_SOUND_BONE_NAME, SoundBoneName);
        ArgumentNullException.ThrowIfNull(AnimationName);
        csave.WriteMicroString(XXX_MICROCHUNKID_DEF_ANIMATION_NAME, AnimationName);
        csave.WriteMicro(MICROCHUNKID_DEF_AUTO_FIRE_WEAPON, AutoFireWeapon);
        csave.WriteMicro(MICROCHUNKID_DEF_DESTROY_AFTER_ANIMATION, DestroyAfterAnimation);
        csave.WriteMicro(MICROCHUNKID_DEF_CAMERA_RELATIVE, CameraRelative);
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
                            case MICROCHUNKID_DEF_SOUND_DEF_ID:
                                cload.Read(ref SoundDefID);
                                break;
                            case MICROCHUNKID_DEF_SOUND_BONE_NAME:
                                cload.ReadMicroChunkWWString(out SoundBoneName);
                                break;
                            case XXX_MICROCHUNKID_DEF_ANIMATION_NAME:
                                cload.ReadMicroChunkWWString(out AnimationName);
                                break;
                            case MICROCHUNKID_DEF_AUTO_FIRE_WEAPON:
                                cload.Read(ref AutoFireWeapon);
                                break;
                            case MICROCHUNKID_DEF_DESTROY_AFTER_ANIMATION:
                                cload.Read(ref DestroyAfterAnimation);
                                break;
                            case MICROCHUNKID_DEF_CAMERA_RELATIVE:
                                cload.Read(ref CameraRelative);
                                break;
                            default:
                                Console.WriteLine("Unrecognized CinematicDef Variable chunkID\n");
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unrecognized CinematicDef chunkID\n");
                    break;
            }
            cload.Close_Chunk();
        }

        return true;
    }
    public override PersistFactoryClass Get_Factory() => _persistFactory;

    //DECLARE_EDITABLE(CinematicGameObjDef, ArmedGameObjDef);

    protected int SoundDefID;
    protected string? SoundBoneName;
    protected string? AnimationName;
    protected bool AutoFireWeapon;
    protected bool DestroyAfterAnimation;
    protected bool CameraRelative;


    private const int CHUNKID_DEF_PARENT = 418001957;
    private const int CHUNKID_DEF_VARIABLES = 418001958;

    private const int MICROCHUNKID_DEF_SOUND_DEF_ID = 1;
    private const int MICROCHUNKID_DEF_SOUND_BONE_NAME = 2;
    private const int XXX_MICROCHUNKID_DEF_ANIMATION_NAME = 3;
    private const int MICROCHUNKID_DEF_AUTO_FIRE_WEAPON = 4;
    private const int MICROCHUNKID_DEF_DESTROY_AFTER_ANIMATION = 5;
    private const int MICROCHUNKID_DEF_CAMERA_RELATIVE = 6;
};

//SimplePersistFactoryClass<CinematicGameObjDef, CHUNKID_GAME_OBJECT_DEF_CINEMATIC> _CinematicGameObjDefPersistFactory;
//DECLARE_DEFINITION_FACTORY(CinematicGameObjDef, CLASSID_GAME_OBJECT_DEF_CINEMATIC, "Cinematic") _CinematicGameObjDefDefFactory;
