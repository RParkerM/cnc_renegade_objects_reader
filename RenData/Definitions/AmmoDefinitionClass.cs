using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;
using RenData.Types;
using System.Numerics;

namespace RenData.Definitions;

[RegisterDefinition(ChunkId.CHUNKID_AMMO_DEF)]
public partial class AmmoDefinitionClass : DefinitionClass
{
    public enum Type
    {
        AMMO_TYPE_NORMAL,
        AMMO_TYPE_C4_REMOTE,
        AMMO_TYPE_C4_TIMED,
        AMMO_TYPE_C4_PROXIMITY,
    };

    public AmmoDefinitionClass() { }

    //SimplePersistFactoryClass<AmmoDefinitionClass, CHUNKID_AMMO_DEF> _AmmoDefPersistFactory;
    //DECLARE_DEFINITION_FACTORY(AmmoDefinitionClass, CLASSID_DEF_AMMO, "Ammo") _AmmoDefDefFactory;
    public override uint Get_Class_ID() => (uint)ClassId.CLASSID_DEF_AMMO;
    public override PersistClass Create()
    {
        throw new NotImplementedException();
    }
    public override bool Save(ChunkSaveClass csave)
    {

        csave.Begin_Chunk(CHUNKID_AMMO_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_AMMO_DEF_VARIABLES);
        ArgumentNullException.ThrowIfNull(ModelFilename);
        csave.WriteMicroString(MICROCHUNKID_AMMO_DEF_MODEL, ModelFilename);
        //		WRITE_MICRO_CHUNK(			 csave, 	MICROCHUNKID_AMMO_DEF_WEAPON_DEF_ID,			WeaponDefID );
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_WARHEAD, Warhead);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_DAMAGE, Damage);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_RANGE, Range);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_VELOCITY, Velocity);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_GRAVITY, Gravity);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_ELASTICITY, Elasticity);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_RATE_OF_FIRE, RateOfFire);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_SPRAY_ANGLE, SprayAngle);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_SPRAY_COUNT, SprayCount);
        ArgumentNullException.ThrowIfNull(TrailEmitter);
        csave.WriteMicroString(MICROCHUNKID_AMMO_DEF_TRAIL_EMITTER, TrailEmitter);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_AQUIRE_TIME, AquireTime);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_BURST_DELAY_TIME, BurstDelayTime);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_BURST_MAX, BurstMax);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_SOFT_PIERCE_LIMIT, SoftPierceLimit);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_TURN_RATE, TurnRate);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_TIME_ACTIVATED, TimeActivated);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_TERRAIN_ACTIVATED, TerrainActivated);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_IS_TRACKING, IsTracking);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_EFFECTIVE_RANGE, EffectiveRange);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_EXPLOSION_DEF_ID, ExplosionDefID);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_RANDOM_TRACKING_SCALE, RandomTrackingScale);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_DISPLAY_LASER, DisplayLaser);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_CHARGE_TIME, ChargeTime);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_CONTINUOUS_SOUND_DEF_ID, ContinuousSoundDefID);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_FIRE_SOUND_DEFID, FireSoundDefID);
        ArgumentNullException.ThrowIfNull(ContinuousEmitterName);
        csave.WriteMicroString(MICROCHUNKID_AMMO_DEF_CONTINUOUS_EMITTER_NAME, ContinuousEmitterName);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_MAX_BOUNCES, MaxBounces);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_SPRAY_BULLET_COST, SprayBulletCost);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_AMMO_TYPE, AmmoType);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_C4_TRIGGER_TIME_1, C4TriggerTime1);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_C4_TRIGGER_TIME_2, C4TriggerTime2);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_C4_TRIGGER_TIME_3, C4TriggerTime3);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_C4_TRIGGER_RANGE_1, C4TriggerRange1);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_C4_TRIGGER_RANGE_2, C4TriggerRange2);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_C4_TRIGGER_RANGE_3, C4TriggerRange3);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_C4_TIMING_SOUND_1_ID, C4TimingSound1ID);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_C4_TIMING_SOUND_2_ID, C4TimingSound2ID);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_C4_TIMING_SOUND_3_ID, C4TimingSound3ID);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_ALIASED_SPEED, AliasedSpeed);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_HITTER_TYPE, HitterType);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_BEACON_DEFID, BeaconDefID);

        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_BEAM_ENABLED, BeamEnabled);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_BEAM_COLOR, BeamColor);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_BEAM_TIME, BeamTime);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_BEAM_WIDTH, BeamWidth);
        ArgumentNullException.ThrowIfNull(BeamTexture);
        csave.WriteMicroString(MICROCHUNKID_AMMO_DEF_BEAM_TEXTURE, BeamTexture);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_BEAM_SUBDIVISION_ENABLED, BeamSubdivisionEnabled);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_BEAM_SUBDIVISION_SCALE, BeamSubdivisionScale);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_BEAM_SUBDIVISION_FROZEN, BeamSubdivisionFrozen);
        csave.WriteMicro(MICROCHUNKID_AMMO_DEF_BEAM_END_CAPS, BeamEndCaps);
        csave.WriteMicro(MICROCHUNKID_AMMO_ICON_NAME_ID, IconNameID);
        ArgumentNullException.ThrowIfNull(IconTextureName);
        csave.WriteMicroString(MICROCHUNKID_AMMO_ICON_TEXTURE_NAME, IconTextureName);

        csave.WriteMicro(MICROCHUNKID_AMMO_ICON_TEXTURE_UV, (RectClassStruct)IconTextureUV);
        csave.WriteMicro(MICROCHUNKID_AMMO_ICON_OFFSET, IconOffset);
        csave.WriteMicro(MICROCHUNKID_AMMO_GRENADE_SAFETY_TIME, GrenadeSafetyTime);

        csave.End_Chunk();

        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {

                case CHUNKID_AMMO_DEF_PARENT:
                    base.Load(cload);
                    break;

                case CHUNKID_AMMO_DEF_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case MICROCHUNKID_AMMO_DEF_MODEL:
                                cload.ReadMicroChunkWWString(out ModelFilename);
                                break;
                            case MICROCHUNKID_AMMO_DEF_WARHEAD:
                                cload.Read(ref Warhead);
                                break;
                            case MICROCHUNKID_AMMO_DEF_DAMAGE:
                                cload.Read(ref Damage);
                                break;
                            case MICROCHUNKID_AMMO_DEF_RANGE:
                                cload.Read(ref Range);
                                break;
                            case MICROCHUNKID_AMMO_DEF_VELOCITY:
                                cload.Read(ref Velocity);
                                break;
                            case MICROCHUNKID_AMMO_DEF_GRAVITY:
                                cload.Read(ref Gravity);
                                break;
                            case MICROCHUNKID_AMMO_DEF_ELASTICITY:
                                cload.Read(ref Elasticity);
                                break;
                            case MICROCHUNKID_AMMO_DEF_RATE_OF_FIRE:
                                cload.Read(ref RateOfFire);
                                break;
                            case MICROCHUNKID_AMMO_DEF_SPRAY_ANGLE:
                                cload.Read(ref SprayAngle);
                                break;
                            case MICROCHUNKID_AMMO_DEF_SPRAY_COUNT:
                                cload.Read(ref SprayCount);
                                break;
                            case MICROCHUNKID_AMMO_DEF_TRAIL_EMITTER:
                                cload.ReadMicroChunkWWString(out TrailEmitter);
                                break;
                            case MICROCHUNKID_AMMO_DEF_AQUIRE_TIME:
                                cload.Read(ref AquireTime);
                                break;
                            case MICROCHUNKID_AMMO_DEF_BURST_DELAY_TIME:
                                cload.Read(ref BurstDelayTime);
                                break;
                            case MICROCHUNKID_AMMO_DEF_BURST_MAX:
                                cload.Read(ref BurstMax);
                                break;
                            case MICROCHUNKID_AMMO_DEF_SOFT_PIERCE_LIMIT:
                                cload.Read(ref SoftPierceLimit);
                                break;
                            case MICROCHUNKID_AMMO_DEF_TURN_RATE:
                                cload.Read(ref TurnRate);
                                break;
                            case MICROCHUNKID_AMMO_DEF_TIME_ACTIVATED:
                                cload.Read(ref TimeActivated);
                                break;
                            case MICROCHUNKID_AMMO_DEF_TERRAIN_ACTIVATED:
                                cload.Read(ref TerrainActivated);
                                break;
                            case MICROCHUNKID_AMMO_DEF_IS_TRACKING:
                                cload.Read(ref IsTracking);
                                break;
                            case MICROCHUNKID_AMMO_DEF_EFFECTIVE_RANGE:
                                cload.Read(ref EffectiveRange);
                                break;
                            case MICROCHUNKID_AMMO_DEF_EXPLOSION_DEF_ID:
                                cload.Read(ref ExplosionDefID);
                                break;
                            case MICROCHUNKID_AMMO_DEF_RANDOM_TRACKING_SCALE:
                                cload.Read(ref RandomTrackingScale);
                                break;
                            case MICROCHUNKID_AMMO_DEF_DISPLAY_LASER:
                                cload.Read(ref DisplayLaser);
                                break;
                            case MICROCHUNKID_AMMO_DEF_CHARGE_TIME:
                                cload.Read(ref ChargeTime);
                                break;
                            case MICROCHUNKID_AMMO_DEF_CONTINUOUS_SOUND_DEF_ID:
                                cload.Read(ref ContinuousSoundDefID);
                                break;
                            case MICROCHUNKID_AMMO_DEF_FIRE_SOUND_DEFID:
                                cload.Read(ref FireSoundDefID);
                                break;
                            case MICROCHUNKID_AMMO_DEF_CONTINUOUS_EMITTER_NAME:
                                cload.ReadMicroChunkWWString(out ContinuousEmitterName);
                                break;
                            case MICROCHUNKID_AMMO_DEF_MAX_BOUNCES:
                                cload.Read(ref MaxBounces);
                                break;
                            case MICROCHUNKID_AMMO_DEF_SPRAY_BULLET_COST:
                                cload.Read(ref SprayBulletCost);
                                break;
                            case MICROCHUNKID_AMMO_DEF_AMMO_TYPE:
                                cload.Read(ref AmmoType);
                                break;
                            case MICROCHUNKID_AMMO_DEF_C4_TRIGGER_TIME_1:
                                cload.Read(ref C4TriggerTime1);
                                break;
                            case MICROCHUNKID_AMMO_DEF_C4_TRIGGER_TIME_2:
                                cload.Read(ref C4TriggerTime2);
                                break;
                            case MICROCHUNKID_AMMO_DEF_C4_TRIGGER_TIME_3:
                                cload.Read(ref C4TriggerTime3);
                                break;
                            case MICROCHUNKID_AMMO_DEF_C4_TRIGGER_RANGE_1:
                                cload.Read(ref C4TriggerRange1);
                                break;
                            case MICROCHUNKID_AMMO_DEF_C4_TRIGGER_RANGE_2:
                                cload.Read(ref C4TriggerRange2);
                                break;
                            case MICROCHUNKID_AMMO_DEF_C4_TRIGGER_RANGE_3:
                                cload.Read(ref C4TriggerRange3);
                                break;
                            case MICROCHUNKID_AMMO_DEF_C4_TIMING_SOUND_1_ID:
                                cload.Read(ref C4TimingSound1ID);
                                break;
                            case MICROCHUNKID_AMMO_DEF_C4_TIMING_SOUND_2_ID:
                                cload.Read(ref C4TimingSound2ID);
                                break;
                            case MICROCHUNKID_AMMO_DEF_C4_TIMING_SOUND_3_ID:
                                cload.Read(ref C4TimingSound3ID);
                                break;
                            case MICROCHUNKID_AMMO_DEF_ALIASED_SPEED:
                                cload.Read(ref AliasedSpeed);
                                break;
                            case MICROCHUNKID_AMMO_DEF_HITTER_TYPE:
                                cload.Read(ref HitterType);
                                break;
                            case MICROCHUNKID_AMMO_DEF_BEACON_DEFID:
                                cload.Read(ref BeaconDefID);
                                break;
                            case MICROCHUNKID_AMMO_DEF_BEAM_ENABLED:
                                cload.Read(ref BeamEnabled);
                                break;
                            case MICROCHUNKID_AMMO_DEF_BEAM_COLOR:
                                cload.Read(ref BeamColor);
                                break;
                            case MICROCHUNKID_AMMO_DEF_BEAM_TIME:
                                cload.Read(ref BeamTime);
                                break;
                            case MICROCHUNKID_AMMO_DEF_BEAM_WIDTH:
                                cload.Read(ref BeamWidth);
                                break;
                            case MICROCHUNKID_AMMO_DEF_BEAM_TEXTURE:
                                cload.ReadMicroChunkWWString(out BeamTexture);
                                break;
                            case MICROCHUNKID_AMMO_DEF_BEAM_SUBDIVISION_ENABLED:
                                cload.Read(ref BeamSubdivisionEnabled);
                                break;
                            case MICROCHUNKID_AMMO_DEF_BEAM_SUBDIVISION_SCALE:
                                cload.Read(ref BeamSubdivisionScale);
                                break;
                            case MICROCHUNKID_AMMO_DEF_BEAM_SUBDIVISION_FROZEN:
                                cload.Read(ref BeamSubdivisionFrozen);
                                break;
                            case MICROCHUNKID_AMMO_DEF_BEAM_END_CAPS:
                                cload.Read(ref BeamEndCaps);
                                break;
                            case MICROCHUNKID_AMMO_ICON_NAME_ID:
                                cload.Read(ref IconNameID);
                                break;
                            case MICROCHUNKID_AMMO_ICON_TEXTURE_NAME:
                                cload.ReadMicroChunkWWString(out IconTextureName);
                                break;
                            case MICROCHUNKID_AMMO_ICON_TEXTURE_UV:
                                RectClassStruct rect = new();
                                cload.Read(ref rect);
                                IconTextureUV.Set(rect);
                                break;
                            case MICROCHUNKID_AMMO_ICON_OFFSET:
                                cload.Read(ref IconOffset);
                                break;
                            case MICROCHUNKID_AMMO_GRENADE_SAFETY_TIME:
                                cload.Read(ref GrenadeSafetyTime);
                                break;


                            default:
                                Console.WriteLine("Unrecognized AmmoDef Variable chunkID\n");
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unrecognized AmmoDef chunkID\n");
                    break;

            }
            cload.Close_Chunk();
        }

        // Init the model name from the model filename
        //::Get_Render_Obj_Name_From_Filename(ModelName, ModelFilename);

        if (string.IsNullOrEmpty(ModelName))
        {
            ModelName = "NULL";
        }

        return true;
    }
    public override PersistFactoryClass Get_Factory() => _persistFactory;

    //DECLARE_EDITABLE(AmmoDefinitionClass, DefinitionClass);

    //bool operator ==( const AmmoDefinitionClass & vector) const { return false; }
    //bool operator !=( const AmmoDefinitionClass & vector) const    { return true; }

    public string? ModelFilename;
    public string? ModelName;
    public int Warhead;
    public float Damage;
    public float Range;
    public float Velocity;
    public float Gravity;
    public float Elasticity;
    public float RateOfFire;
    public float SprayAngle;
    public int SprayCount;
    public string? TrailEmitter;
    public float AquireTime;
    public float BurstDelayTime;
    public int BurstMax;
    public int SoftPierceLimit;
    public float TurnRate;
    public bool TimeActivated;
    public bool TerrainActivated;
    public bool IsTracking;
    public float EffectiveRange;
    public int ExplosionDefID;
    public float RandomTrackingScale;
    public bool DisplayLaser;
    public float ChargeTime;
    public int ContinuousSoundDefID;
    public int FireSoundDefID;
    public string? ContinuousEmitterName;
    public int MaxBounces;
    public int SprayBulletCost;
    public int AmmoType;
    public float C4TriggerTime1;
    public float C4TriggerTime2;
    public float C4TriggerTime3;
    public float C4TriggerRange1;
    public float C4TriggerRange2;
    public float C4TriggerRange3;
    public int C4TimingSound1ID;
    public int C4TimingSound2ID;
    public int C4TimingSound3ID;
    public float AliasedSpeed;
    public int HitterType;
    public int BeaconDefID;

    public bool BeamEnabled;                       // enable/disable usage of beam for instant bullets
    public Vector3 BeamColor;
    public float BeamTime;                         // seconds
    public float BeamWidth;                            // meters
    public bool BeamEndCaps;                       // put end-caps on the beam?
    public string? BeamTexture;                        // texture filename
    public bool BeamSubdivisionEnabled;            // boolean; we use fixed subdivision count
    public float BeamSubdivisionScale;         // normalized amplitude.
    public bool BeamSubdivisionFrozen;         // animated or frozen subdivision

    public int IconNameID;
    public string? IconTextureName;
    public RectClass IconTextureUV = new();
    public Vector2 IconOffset;

    public float GrenadeSafetyTime;

    const int CHUNKID_AMMO_DEF_VARIABLES = 1206091429;
    const int CHUNKID_AMMO_DEF_PARENT = 1206091430;

    const int XXXMICROCHUNKID_AMMO_DEF_WEAPON_DEF_ID = 1;
    const int MICROCHUNKID_AMMO_DEF_MODEL = 2;
    const int MICROCHUNKID_AMMO_DEF_WARHEAD = 3;
    const int MICROCHUNKID_AMMO_DEF_DAMAGE = 4;
    const int MICROCHUNKID_AMMO_DEF_RANGE = 5;
    const int MICROCHUNKID_AMMO_DEF_VELOCITY = 6;
    const int MICROCHUNKID_AMMO_DEF_GRAVITY = 7;
    const int XXXMICROCHUNKID_AMMO_DEF_ELASTICITY = 8;
    const int XXXMICROCHUNKID_AMMO_DEF_CLIP_SIZE = 9;
    const int MICROCHUNKID_AMMO_DEF_SPRAY_ANGLE = 10;
    const int MICROCHUNKID_AMMO_DEF_SPRAY_COUNT = 11;
    const int MICROCHUNKID_AMMO_DEF_TRAIL_EMITTER = 12;
    const int MICROCHUNKID_AMMO_DEF_AQUIRE_TIME = 13;
    const int MICROCHUNKID_AMMO_DEF_BURST_DELAY_TIME = 14;
    const int MICROCHUNKID_AMMO_DEF_BURST_MAX = 15;
    const int XXXXMICROCHUNKID_AMMO_DEF_EXPLOSION = 16;
    const int MICROCHUNKID_AMMO_DEF_SOFT_PIERCE_LIMIT = 17;
    const int MICROCHUNKID_AMMO_DEF_TURN_RATE = 18;
    const int MICROCHUNKID_AMMO_DEF_TIME_ACTIVATED = 19;
    const int MICROCHUNKID_AMMO_DEF_TERRAIN_ACTIVATED = 20;
    const int MICROCHUNKID_AMMO_DEF_IS_TRACKING = 21;
    const int XXXMICROCHUNKID_AMMO_DEF_ACTIVATE_SOUND_RADIUS = 22;
    const int MICROCHUNKID_AMMO_DEF_EFFECTIVE_RANGE = 23;
    const int MICROCHUNKID_AMMO_DEF_EXPLOSION_DEF_ID = 24;
    const int MICROCHUNKID_AMMO_DEF_RANDOM_TRACKING_SCALE = 25;
    const int MICROCHUNKID_AMMO_DEF_DISPLAY_LASER = 26;
    const int MICROCHUNKID_AMMO_DEF_CHARGE_TIME = 27;
    const int MICROCHUNKID_AMMO_DEF_CONTINUOUS_SOUND_DEF_ID = 28;
    const int MICROCHUNKID_AMMO_DEF_CONTINUOUS_EMITTER_NAME = 29;
    const int MICROCHUNKID_AMMO_DEF_MAX_BOUNCES = 30;
    const int MICROCHUNKID_AMMO_DEF_SPRAY_BULLET_COST = 31;
    const int MICROCHUNKID_AMMO_DEF_AMMO_TYPE = 32;
    const int XXXMICROCHUNKID_AMMO_DEF_C4_DETONATE_SOUND_ID = 33;
    const int XXXMICROCHUNKID_AMMO_DEF_C4_TIMING_SOUND_ID = 34;
    const int MICROCHUNKID_AMMO_DEF_C4_TRIGGER_TIME_1 = 35;
    const int MICROCHUNKID_AMMO_DEF_C4_TRIGGER_TIME_2 = 36;
    const int MICROCHUNKID_AMMO_DEF_C4_TRIGGER_TIME_3 = 37;
    const int MICROCHUNKID_AMMO_DEF_C4_TRIGGER_RANGE_1 = 38;
    const int MICROCHUNKID_AMMO_DEF_C4_TRIGGER_RANGE_2 = 39;
    const int MICROCHUNKID_AMMO_DEF_C4_TRIGGER_RANGE_3 = 40;
    const int MICROCHUNKID_AMMO_DEF_C4_TIMING_SOUND_1_ID = 41;
    const int MICROCHUNKID_AMMO_DEF_C4_TIMING_SOUND_2_ID = 42;
    const int MICROCHUNKID_AMMO_DEF_C4_TIMING_SOUND_3_ID = 43;
    const int MICROCHUNKID_AMMO_DEF_ELASTICITY = 44;
    const int MICROCHUNKID_AMMO_DEF_ALIASED_SPEED = 45;
    const int MICROCHUNKID_AMMO_DEF_HITTER_TYPE = 46;
    const int MICROCHUNKID_AMMO_DEF_BEACON_DEFID = 47;
    const int MICROCHUNKID_AMMO_DEF_RATE_OF_FIRE = 48;

    const int MICROCHUNKID_AMMO_DEF_BEAM_ENABLED = 49;
    const int MICROCHUNKID_AMMO_DEF_BEAM_COLOR = 50;
    const int MICROCHUNKID_AMMO_DEF_BEAM_TIME = 51;
    const int MICROCHUNKID_AMMO_DEF_BEAM_WIDTH = 52;
    const int MICROCHUNKID_AMMO_DEF_BEAM_TEXTURE = 53;
    const int MICROCHUNKID_AMMO_DEF_BEAM_SUBDIVISION_ENABLED = 54;
    const int MICROCHUNKID_AMMO_DEF_BEAM_SUBDIVISION_SCALE = 55;
    const int MICROCHUNKID_AMMO_DEF_BEAM_SUBDIVISION_FROZEN = 56;
    const int MICROCHUNKID_AMMO_DEF_BEAM_END_CAPS = 57;
    const int MICROCHUNKID_AMMO_ICON_NAME_ID = 58;
    const int MICROCHUNKID_AMMO_ICON_TEXTURE_NAME = 59;
    const int MICROCHUNKID_AMMO_ICON_TEXTURE_UV = 60;
    const int MICROCHUNKID_AMMO_ICON_OFFSET = 61;
    const int MICROCHUNKID_AMMO_DEF_FIRE_SOUND_DEFID = 62;
    const int MICROCHUNKID_AMMO_GRENADE_SAFETY_TIME = 63;
};



//AmmoDefinitionClass::AmmoDefinitionClass(void) :
//	Warhead(0),
//	Damage(1),
//	Range(10),
//	EffectiveRange(10),
//	Velocity(1),
//	Gravity(0),
//	Elasticity(1.0f),
//	RateOfFire(1),
//	SprayAngle(0),
//	SprayCount(1),
//	AquireTime(0),
//	BurstDelayTime(0),
//	BurstMax(0),
//	SoftPierceLimit(0),
//	TurnRate(0),
//	TimeActivated(false),
//	TerrainActivated(false),
//	IsTracking(false),
////	WeaponDefID( 0 ),
//	ExplosionDefID(0),
//	RandomTrackingScale(0),
//	DisplayLaser(false),
//	ChargeTime(0),
//	ContinuousSoundDefID(0),
//	FireSoundDefID(0),
//	MaxBounces(0),
//	SprayBulletCost(1),
//	AmmoType(AMMO_TYPE_NORMAL),
//	BeaconDefID(0),
//	C4TriggerTime1(0),
//	C4TriggerTime2(0),
//	C4TriggerTime3(0),
//	C4TriggerRange1(0),
//	C4TriggerRange2(0),
//	C4TriggerRange3(0),
//	C4TimingSound1ID(0),
//	C4TimingSound2ID(0),
//	C4TimingSound3ID(0),
//	AliasedSpeed(0),
//	HitterType(SurfaceEffectsManager::HITTER_TYPE_BULLET),
//	BeamEnabled(false),
//	BeamColor(1, 1, 1),
//	BeamTime(0.3f),
//	BeamWidth(0.25f),
//	BeamEndCaps(true),
//	BeamSubdivisionEnabled(false),
//	BeamSubdivisionScale(1.0f),
//	BeamSubdivisionFrozen(false),
//	IconNameID(0),
//	IconTextureUV(0, 0, 0, 0),
//	IconOffset(0, 0),
//	GrenadeSafetyTime(0)
//{
//# ifdef	PARAM_EDITING_ON
//    int i;
//    EnumParameterClass* param;
//    param = new EnumParameterClass(&AmmoType);
//    param->Set_Name("Ammo Type");
//    param->Add_Value("Normal", AmmoDefinitionClass::AMMO_TYPE_NORMAL);
//    param->Add_Value("Remote C4", AmmoDefinitionClass::AMMO_TYPE_C4_REMOTE);
//    param->Add_Value("Timed C4", AmmoDefinitionClass::AMMO_TYPE_C4_TIMED);
//    param->Add_Value("Proximity C4", AmmoDefinitionClass::AMMO_TYPE_C4_PROXIMITY);
//    GENERIC_EDITABLE_PARAM(AmmoDefinitionClass, param)

////	EDITABLE_PARAM( AmmoDefinitionClass, ParameterClass::TYPE_WEAPONOBJDEFINITIONID,	WeaponDefID);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FILENAME, ModelFilename);

//    //	EDITABLE_PARAM( AmmoDefinitionClass, ParameterClass::TYPE_INT,			Warhead);
//    param = new EnumParameterClass(&Warhead);
//    param->Set_Name("Warhead");
//    for (i = 0; i < ArmorWarheadManager::Get_Num_Warhead_Types(); i++)
//    {
//        param->Add_Value(ArmorWarheadManager::Get_Warhead_Name(i), i);
//    }
//    GENERIC_EDITABLE_PARAM(AmmoDefinitionClass, param)


//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FLOAT, Damage);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FLOAT, Range);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FLOAT, EffectiveRange);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FLOAT, Velocity);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FLOAT, Gravity);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FLOAT, Elasticity);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FLOAT, RateOfFire);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_ANGLE, SprayAngle);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_INT, SprayCount);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_INT, SprayBulletCost);
//    //	EDITABLE_PARAM( AmmoDefinitionClass, ParameterClass::TYPE_FILENAME,	TrailEmitter);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FLOAT, AquireTime);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FLOAT, BurstDelayTime);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_INT, BurstMax);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_INT, SoftPierceLimit);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_ANGLE, TurnRate);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_BOOL, TimeActivated);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_BOOL, TerrainActivated);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_BOOL, IsTracking);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_EXPLOSIONDEFINITIONID, ExplosionDefID);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FLOAT, RandomTrackingScale);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_BOOL, DisplayLaser);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FLOAT, ChargeTime);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_SOUNDDEFINITIONID, ContinuousSoundDefID);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_SOUNDDEFINITIONID, FireSoundDefID);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FILENAME, ContinuousEmitterName);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_INT, MaxBounces);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FLOAT, C4TriggerTime1);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FLOAT, C4TriggerTime2);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FLOAT, C4TriggerTime3);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FLOAT, C4TriggerRange1);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FLOAT, C4TriggerRange2);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FLOAT, C4TriggerRange3);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_SOUNDDEFINITIONID, C4TimingSound1ID);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_SOUNDDEFINITIONID, C4TimingSound2ID);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_SOUNDDEFINITIONID, C4TimingSound3ID);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FLOAT, AliasedSpeed);

//    param = new EnumParameterClass(&HitterType);
//    param->Set_Name("HitterType");
//    for (i = 0; i < SurfaceEffectsManager::Num_Hitter_Types(); i++)
//    {
//        param->Add_Value(SurfaceEffectsManager::Hitter_Type_Name(i), i);
//    }
//    GENERIC_EDITABLE_PARAM(AmmoDefinitionClass, param)


//    GenericDefParameterClass* beacon_param = new GenericDefParameterClass(&BeaconDefID);
//    beacon_param->Set_Class_ID(CLASSID_GAME_OBJECT_DEF_BEACON);
//    beacon_param->Set_Name("Beacon Object");
//    GENERIC_EDITABLE_PARAM(AmmoDefinitionClass, beacon_param);

//    /*
//     ** Beam effect parameters
//     */
//    PARAM_SEPARATOR(AmmoDefinitionClass, "Instant Bullet Beam Effects");

//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_BOOL, BeamEnabled);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_VECTOR3, BeamColor);
//    FLOAT_UNITS_PARAM(AmmoDefinitionClass, BeamTime, 0.01f, 10.0f, "seconds");
//    FLOAT_UNITS_PARAM(AmmoDefinitionClass, BeamWidth, 0.01f, 10.0f, "meters");
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_BOOL, BeamEndCaps);
//    FILENAME_PARAM(AmmoDefinitionClass, BeamTexture, "Texture filename", ".tga");
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_BOOL, BeamSubdivisionEnabled);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_BOOL, BeamSubdivisionFrozen);
//    FLOAT_EDITABLE_PARAM(AmmoDefinitionClass, BeamSubdivisionScale, 0.01f, 10.0f);

//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_STRINGSDB_ID, IconNameID);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FILENAME, IconTextureName);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_RECT, IconTextureUV);
//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_VECTOR2, IconOffset);

//    EDITABLE_PARAM(AmmoDefinitionClass, ParameterClass::TYPE_FLOAT, GrenadeSafetyTime);

//#endif
//}

///*
//**
//*/
