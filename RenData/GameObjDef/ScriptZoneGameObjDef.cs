using RenData.ChunkIO;
using RenData.SaveLoad;
using RenData.IDs;
using System.Numerics;
using RenData.Types.ZoneConstants;

namespace RenData.GameObjDef;

[RegisterDefinition(ChunkId.CHUNKID_GAME_OBJECT_DEF_SCRIPT_ZONE)]
public partial class ScriptZoneGameObjDef : ScriptableGameObjDef
{
    public ScriptZoneGameObjDef()
    {
        // Editable params and parameter editor code omitted/commented out
        //EDITABLE_PARAM(ScriptZoneGameObjDef, ParameterClass.TYPE_COLOR, Color);
        //EDITABLE_PARAM(ScriptZoneGameObjDef, ParameterClass.TYPE_BOOL, CheckStarsOnly);
        //EDITABLE_PARAM(ScriptZoneGameObjDef, ParameterClass.TYPE_BOOL, IsEnvironmentZone);

        // Configure the zone type parameter
        //#ifdef PARAM_EDITING_ON
        // EnumParameterClass* zone_type_param = new EnumParameterClass((int*)&ZoneType);
        // zone_type_param.Set_Name("Zone Type");
        // for (int index = 0; index < TYPE_COUNT; index++) {
        //     zone_type_param.Add_Value(ZONE_TYPE_NAMES[index], index);
        // }
        // GENERIC_EDITABLE_PARAM(ScriptZoneGameObjDef, zone_type_param);
        //#endif
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GAME_OBJECT_DEF_SCRIPT_ZONE;

    public override PersistClass Create()
    {
        // ScriptZoneGameObj* obj = new ScriptZoneGameObj;
        // obj.Init(*this);
        // return obj;
        throw new NotImplementedException();
    }

    public override bool Save(ChunkSaveClass csave)
    {
        // Save parent chunk
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        // Save variables chunk
        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        csave.WriteMicro(MICROCHUNKID_DEF_ZONE_COLOR, Color);
        csave.WriteMicro(MICROCHUNKID_DEF_CHECK_STARS_ONLY, CheckStarsOnly);
        csave.WriteMicro(MICROCHUNKID_DEF_ZONE_TYPE, _zoneType);
        csave.WriteMicro(MICROCHUNKID_DEF_IS_ENVIRONMENT_ZONE, IsEnvironmentZone);
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
                            case MICROCHUNKID_DEF_ZONE_COLOR:
                                cload.Read(ref Color);
                                break;
                            case MICROCHUNKID_DEF_CHECK_STARS_ONLY:
                                cload.Read(ref CheckStarsOnly);
                                break;
                            case MICROCHUNKID_DEF_ZONE_TYPE:
                                cload.Read(ref _zoneType);
                                break;
                            case MICROCHUNKID_DEF_IS_ENVIRONMENT_ZONE:
                                cload.Read(ref IsEnvironmentZone);
                                break;

                            default:
                                Console.WriteLine("Unrecognized ZoneDef Variable chunkID\n");
                                break;
                        }

                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unrecognized ZoneDef chunkID\n");
                    break;
            }

            cload.Close_Chunk();
        }

        return true;
    }

    public virtual bool Is_Valid_Config(ref string message) { return true; }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    // Accessors
    public Vector3 Get_Color() { return Color; }
    public int Get_Type() { return _zoneType; }


    // Note: renamed field to avoid name clash with the ZoneType class
    protected int _zoneType = ZoneType.TYPE_DEFAULT;
    protected Vector3 Color = new(0f, 0.7f, 0f);
    protected bool IsCTFZone;
    protected bool CheckStarsOnly = true;
    protected bool IsEnvironmentZone = false;


    private const uint XXXCHUNKID_DEF_PARENT_OLD = 1111991132u;
    private const uint CHUNKID_DEF_VARIABLES = 1111991133u;
    private const uint CHUNKID_DEF_PARENT = 1111991134u;

    private const byte MICROCHUNKID_DEF_IS_CTF_ZONE = 1;
    private const byte MICROCHUNKID_DEF_ZONE_COLOR = 2;
    private const byte MICROCHUNKID_DEF_CHECK_STARS_ONLY = 3;
    private const byte MICROCHUNKID_DEF_ZONE_TYPE = 4;
    private const byte MICROCHUNKID_DEF_IS_ENVIRONMENT_ZONE = 5;
}
