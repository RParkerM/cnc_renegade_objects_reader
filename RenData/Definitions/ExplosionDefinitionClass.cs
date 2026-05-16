using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.Definitions;

[RegisterDefinition(ChunkId.CHUNKID_EXPLOSION_DEF)]
public partial class ExplosionDefinitionClass : DefinitionClass
{
    public ExplosionDefinitionClass()
    {
        PhysDefID = 0;
        SoundDefID = 0;
        DamageRadius = 0;
        DamageStrength = 0;
        DamageWarhead = 0;
        DamageIsScaled = true;
        DecalSize = 10;
        AnimatedExplosion = true;
        CameraShakeIntensity = 0.0f;
        CameraShakeRadius = 25.0f;
        CameraShakeDuration = 1.5f;
        //{
        //# ifdef PARAM_EDITING_ON
        //            MODEL_DEF_PARAM(ExplosionDefinitionClass, PhysDefID, "TimedDecorationPhysDef");
        //            EDITABLE_PARAM(ExplosionDefinitionClass, ParameterClass::TYPE_SOUNDDEFINITIONID, SoundDefID);
        //            EDITABLE_PARAM(ExplosionDefinitionClass, ParameterClass::TYPE_FLOAT, DamageRadius);
        //            EDITABLE_PARAM(ExplosionDefinitionClass, ParameterClass::TYPE_FLOAT, DamageStrength);

        //            //	EDITABLE_PARAM( ExplosionDefinitionClass, ParameterClass::TYPE_INT,						DamageWarhead );
        //            EnumParameterClass param;
        //            param = new EnumParameterClass(DamageWarhead);
        //            param.Set_Name("Warhead");
        //            for (int i = 0; i < ArmorWarheadManager::Get_Num_Warhead_Types(); i++)
        //            {
        //                param.Add_Value(ArmorWarheadManager::Get_Warhead_Name(i), i);
        //            }
        //            GENERIC_EDITABLE_PARAM(ExplosionDefinitionClass, param)


        //        EDITABLE_PARAM(ExplosionDefinitionClass, ParameterClass::TYPE_BOOL, DamageIsScaled);
        //            EDITABLE_PARAM(ExplosionDefinitionClass, ParameterClass::TYPE_FILENAME, DecalFilename);
        //            EDITABLE_PARAM(ExplosionDefinitionClass, ParameterClass::TYPE_FLOAT, DecalSize);
        //            EDITABLE_PARAM(ExplosionDefinitionClass, ParameterClass::TYPE_BOOL, AnimatedExplosion);

        //            FLOAT_EDITABLE_PARAM(ExplosionDefinitionClass, CameraShakeIntensity, 0.0f, 1.0f);
        //            FLOAT_UNITS_PARAM(ExplosionDefinitionClass, CameraShakeRadius, 0.01f, 1000.0f, "meters");
        //            FLOAT_UNITS_PARAM(ExplosionDefinitionClass, CameraShakeDuration, 0.01f, 60.0f, "seconds");
        //#endif    //PARAM_EDITING_ON                    
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_DEF_EXPLOSION;
    public override PersistClass Create()
    {
        throw new NotImplementedException();
        //WWASSERT(0); return null; 
    }
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_EXPLOSION_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_EXPLOSION_DEF_VARIABLES);
        csave.WriteMicro(MICROCHUNKID_EXPLOSION_DEF_PHYS_DEF_ID, PhysDefID);
        csave.WriteMicro(MICROCHUNKID_EXPLOSION_DEF_SOUND_DEF_ID, SoundDefID);
        csave.WriteMicro(MICROCHUNKID_EXPLOSION_DEF_DAMAGE_RADIUS, DamageRadius);

        csave.WriteMicro(MICROCHUNKID_EXPLOSION_DEF_DAMAGE_STRENGTH, DamageStrength);
        csave.WriteMicro(MICROCHUNKID_EXPLOSION_DEF_DAMAGE_WARHEAD, DamageWarhead);
        csave.WriteMicro(MICROCHUNKID_EXPLOSION_DEF_DAMAGE_IS_SCALED, DamageIsScaled);
        ArgumentNullException.ThrowIfNull(DecalFilename);
        csave.WriteMicroString(MICROCHUNKID_EXPLOSION_DEF_DECAL_FILENAME, DecalFilename);
        csave.WriteMicro(MICROCHUNKID_EXPLOSION_DEF_DECAL_SIZE, DecalSize);
        csave.WriteMicro(MICROCHUNKID_EXPLOSION_DEF_ANIMATED_EXPLOLSION, AnimatedExplosion);
        csave.WriteMicro(MICROCHUNKID_EXPLOSION_DEF_CAMERASHAKEINTENSITY, CameraShakeIntensity);
        csave.WriteMicro(MICROCHUNKID_EXPLOSION_DEF_CAMERASHAKERADIUS, CameraShakeRadius);
        csave.WriteMicro(MICROCHUNKID_EXPLOSION_DEF_CAMERASHAKEDURATION, CameraShakeDuration);
        csave.End_Chunk();

        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {

                case CHUNKID_EXPLOSION_DEF_PARENT:
                    base.Load(cload);
                    break;

                case CHUNKID_EXPLOSION_DEF_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case MICROCHUNKID_EXPLOSION_DEF_PHYS_DEF_ID:
                                cload.Read(ref PhysDefID);
                                break;
                            case MICROCHUNKID_EXPLOSION_DEF_SOUND_DEF_ID:
                                cload.Read(ref SoundDefID);
                                break;
                            case MICROCHUNKID_EXPLOSION_DEF_DAMAGE_RADIUS:
                                cload.Read(ref DamageRadius);
                                break;
                        case MICROCHUNKID_EXPLOSION_DEF_DAMAGE_STRENGTH:
                                cload.Read(ref DamageStrength);
                                break;
                            case MICROCHUNKID_EXPLOSION_DEF_DAMAGE_WARHEAD:
                                cload.Read(ref DamageWarhead);
                                break;
                            case MICROCHUNKID_EXPLOSION_DEF_DAMAGE_IS_SCALED:
                                cload.Read(ref DamageIsScaled);
                                break;
                            case MICROCHUNKID_EXPLOSION_DEF_DECAL_FILENAME:
                                cload.ReadMicroChunkWWString(out DecalFilename);
                                break;
                            case MICROCHUNKID_EXPLOSION_DEF_DECAL_SIZE:
                                cload.Read(ref DecalSize);
                                break;
                            case MICROCHUNKID_EXPLOSION_DEF_ANIMATED_EXPLOLSION:
                                cload.Read(ref AnimatedExplosion);
                                break;
                            case MICROCHUNKID_EXPLOSION_DEF_CAMERASHAKEINTENSITY:
                                cload.Read(ref CameraShakeIntensity);
                                break;
                            case MICROCHUNKID_EXPLOSION_DEF_CAMERASHAKERADIUS:
                                cload.Read(ref CameraShakeRadius);
                                break;
                            case MICROCHUNKID_EXPLOSION_DEF_CAMERASHAKEDURATION:
                                cload.Read(ref CameraShakeDuration);
                                break;
                            default:
                                Console.WriteLine("Unrecognized ExplosionDef Variable chunkID\n");
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unrecognized ExplosionDef chunkID\n");
                    break;

            }
            cload.Close_Chunk();
        }

        return true;
    }
    public override PersistFactoryClass Get_Factory() => _persistFactory;

    //DECLARE_EDITABLE(ExplosionDefinitionClass, DefinitionClass);

    public int PhysDefID;
    public int SoundDefID;
    public float DamageRadius;
    public float DamageStrength;
    public int DamageWarhead;
    public bool DamageIsScaled;
    public string? DecalFilename;
    public float DecalSize;
    public bool AnimatedExplosion;

    public float CameraShakeIntensity;
    public float CameraShakeRadius;
    public float CameraShakeDuration;



    private const uint CHUNKID_EXPLOSION_DEF_VARIABLES = 54264661;   // from octal 0317001525
    private const uint CHUNKID_EXPLOSION_DEF_PARENT = 54264662;

    // Microchunks (start at 1 and auto-increment)
    private const uint MICROCHUNKID_EXPLOSION_DEF_PHYS_DEF_ID = 1;
    private const uint MICROCHUNKID_EXPLOSION_DEF_SOUND_DEF_ID = 2;
    private const uint MICROCHUNKID_EXPLOSION_DEF_DAMAGE_RADIUS = 3;
    private const uint MICROCHUNKID_EXPLOSION_DEF_DAMAGE_STRENGTH = 4;
    private const uint MICROCHUNKID_EXPLOSION_DEF_DAMAGE_WARHEAD = 5;
    private const uint MICROCHUNKID_EXPLOSION_DEF_DAMAGE_IS_SCALED = 6;
    private const uint MICROCHUNKID_EXPLOSION_DEF_DECAL_FILENAME = 7;
    private const uint MICROCHUNKID_EXPLOSION_DEF_DECAL_SIZE = 8;
    private const uint XXXMICROCHUNKID_EXPLOSION_DEF_ANIMATION = 9;
    private const uint MICROCHUNKID_EXPLOSION_DEF_ANIMATED_EXPLOLSION = 10;
    private const uint MICROCHUNKID_EXPLOSION_DEF_CAMERASHAKEINTENSITY = 11;
    private const uint MICROCHUNKID_EXPLOSION_DEF_CAMERASHAKERADIUS = 12;
    private const uint MICROCHUNKID_EXPLOSION_DEF_CAMERASHAKEDURATION = 13;

};

//SimplePersistFactoryClass<ExplosionDefinitionClass, CHUNKID_EXPLOSION_DEF>	_ExplosionDefPersistFactory;
//DECLARE_DEFINITION_FACTORY(ExplosionDefinitionClass, CLASSID_DEF_EXPLOSION, "Explosion") _ExplosionDefDefFactory;

