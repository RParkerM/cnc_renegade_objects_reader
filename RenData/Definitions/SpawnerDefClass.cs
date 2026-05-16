using System.Diagnostics;
using RenData.ChunkIO;
using RenData.SaveLoad;
using RenData.IDs;
using RenData.Definitions;

namespace RenData;

[RegisterDefinition(ChunkId.CHUNKID_SPAWNER_DEF)]
public partial class SpawnerDefClass : DefinitionClass
{
    public SpawnerDefClass()
    {
        PlayerType = (int)Types.PlayerType.PLAYERTYPE_NEUTRAL; 
        SpawnMax = -1; // unlimited
        SpawnDelay = 10f;
        SpawnDelayVariation = 0f;
        IsPrimary = false;
        IsSoldierStartup = false;
        PostVisualSpawnDelay = 0f;
        SpecialEffectsObjID = 0;
        GotoSpawnerPos = false;
        GotoSpawnerPosPriority = 30f;
        TeleportFirstSpawn = true;
        StartsDisabled = false;
        KillHibernatingSpawn = true;
        ApplySpawnMaterialEffect = true;
        IsMultiplayWeaponSpawner = false;
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_SPAWNER_DEF;

    public override PersistClass Create()
    {
        // SpawnerClass* obj = new SpawnerClass;
        // obj.Init(*this);
        // return obj;
        throw new NotImplementedException();
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory; // _persistFactory provided by source generator

    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        for (int i = 0; i < SpawnDefinitionIDList.Count; i++)
        {
            // assuming WriteMicro exists and accepts int id
            csave.WriteMicro(MICROCHUNKID_DEF_DEFINITION_ID, SpawnDefinitionIDList[i]);
        }
        csave.WriteMicro(MICROCHUNKID_DEF_PLAYER_TYPE, PlayerType);
        csave.WriteMicro(MICROCHUNKID_DEF_SPAWN_MAX, SpawnMax);
        csave.WriteMicro(MICROCHUNKID_DEF_SPAWN_DELAY, SpawnDelay);
        csave.WriteMicro(MICROCHUNKID_DEF_SPAWN_DELAY_VARIATION, SpawnDelayVariation);
        csave.WriteMicro(MICROCHUNKID_DEF_IS_PRIMARY, IsPrimary);
        csave.WriteMicro(MICROCHUNKID_DEF_IS_SOLDIER_STARTUP, IsSoldierStartup);

        csave.WriteMicro(MICROCHUNKID_DEF_SPECIAL_EFFECTS_OBJ_ID, SpecialEffectsObjID);
        csave.WriteMicro(MICROCHUNKID_DEF_POST_EFFECT_SPAWN_DELAY, PostVisualSpawnDelay);
        csave.WriteMicro(MICROCHUNKID_DEF_GOTO_SPAWNER_POS, GotoSpawnerPos);
        csave.WriteMicro(MICROCHUNKID_DEF_GOTO_SPAWNER_POS_PRIORITY, GotoSpawnerPosPriority);
        csave.WriteMicro(MICROCHUNKID_DEF_TELEPORT_FIRST_SPAWN, TeleportFirstSpawn);
        csave.WriteMicro(MICROCHUNKID_DEF_STARTS_DISABLED, StartsDisabled);

        csave.WriteMicro(MICROCHUNKID_DEF_KILL_HIBERNATING_SPAWN, KillHibernatingSpawn);
        csave.WriteMicro(MICROCHUNKID_DEF_APPLY_SPAWN_MATERIAL_EFFECT, ApplySpawnMaterialEffect);
        csave.WriteMicro(MICROCHUNKID_DEF_IS_MULTIPLAY_WEAPON_SPAWNER, IsMultiplayWeaponSpawner);

        Debug.Assert(ScriptNameList.Count == ScriptParameterList.Count);
        for (int i = 0; i < ScriptNameList.Count; i++)
        {
            ArgumentNullException.ThrowIfNull(ScriptNameList[i]);
            csave.WriteMicroString(MICROCHUNKID_DEF_SCRIPT_NAME, ScriptNameList[i]);
            ArgumentNullException.ThrowIfNull(ScriptParameterList[i]);
            csave.WriteMicroString(MICROCHUNKID_DEF_SCRIPT_PARAMETERS, ScriptParameterList[i]);
        }

        csave.End_Chunk();

        return true;
    }

    public override bool Load(ChunkLoadClass cload)
    {
        Debug.Assert(ScriptNameList.Count == ScriptParameterList.Count);
        Debug.Assert(SpawnDefinitionIDList.Count == 0);
        string str;

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
                            case MICROCHUNKID_DEF_DEFINITION_ID:
                                {
                                    int def_id = 0;
                                    cload.Read(ref def_id);
                                    SpawnDefinitionIDList.Add(def_id);
                                }
                                break;

                            case MICROCHUNKID_DEF_PLAYER_TYPE:
                                cload.Read(ref PlayerType);
                                break;
                            case MICROCHUNKID_DEF_SPAWN_MAX:
                                cload.Read(ref SpawnMax);
                                break;
                            case MICROCHUNKID_DEF_SPAWN_DELAY:
                                cload.Read(ref SpawnDelay);
                                break;
                            case MICROCHUNKID_DEF_SPAWN_DELAY_VARIATION:
                                cload.Read(ref SpawnDelayVariation);
                                break;
                            case MICROCHUNKID_DEF_IS_PRIMARY:
                                cload.Read(ref IsPrimary);
                                break;
                            case MICROCHUNKID_DEF_IS_SOLDIER_STARTUP:
                                cload.Read(ref IsSoldierStartup);
                                break;
                            case MICROCHUNKID_DEF_POST_EFFECT_SPAWN_DELAY:
                                cload.Read(ref PostVisualSpawnDelay);
                                break;
                            case MICROCHUNKID_DEF_SPECIAL_EFFECTS_OBJ_ID:
                                cload.Read(ref SpecialEffectsObjID);
                                break;
                            case MICROCHUNKID_DEF_GOTO_SPAWNER_POS:
                                cload.Read(ref GotoSpawnerPos);
                                break;
                            case MICROCHUNKID_DEF_GOTO_SPAWNER_POS_PRIORITY:
                                cload.Read(ref GotoSpawnerPosPriority);
                                break;
                            case MICROCHUNKID_DEF_TELEPORT_FIRST_SPAWN:
                                cload.Read(ref TeleportFirstSpawn);
                                break;
                            case MICROCHUNKID_DEF_STARTS_DISABLED:
                                cload.Read(ref StartsDisabled);
                                break;
                            case MICROCHUNKID_DEF_KILL_HIBERNATING_SPAWN:
                                cload.Read(ref KillHibernatingSpawn);
                                break;
                            case MICROCHUNKID_DEF_APPLY_SPAWN_MATERIAL_EFFECT:
                                cload.Read(ref ApplySpawnMaterialEffect);
                                break;
                            case MICROCHUNKID_DEF_IS_MULTIPLAY_WEAPON_SPAWNER:
                                cload.Read(ref IsMultiplayWeaponSpawner);
                                break;

                            case MICROCHUNKID_DEF_SCRIPT_NAME:
                                cload.ReadMicroChunkWWString(out str);
                                ScriptNameList.Add(str);
                                break;

                            case MICROCHUNKID_DEF_SCRIPT_PARAMETERS:
                                cload.ReadMicroChunkWWString(out str);
                                ScriptParameterList.Add(str);
                                break;

                            default:
                                Console.WriteLine($"Unhandled Micro Chunk:{cload.Cur_Micro_Chunk_ID}");
                                break;
                        }

                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine($"Unhandled Chunk:{cload.Cur_Chunk_ID}");
                    break;
            }

            cload.Close_Chunk();
        }

        Debug.Assert(ScriptNameList.Count == ScriptParameterList.Count);
        return true;
    }

    public List<int> Get_Spawn_Definition_ID_List() => SpawnDefinitionIDList;
    public int Get_Player_Type() => PlayerType;
    public bool Is_Multiplay_Weapon_Spawner() => IsMultiplayWeaponSpawner;


    protected List<int> SpawnDefinitionIDList = [];
    protected int PlayerType;

    protected int SpawnMax;
    protected float SpawnDelay;
    protected float SpawnDelayVariation;

    protected bool IsPrimary;
    protected bool IsSoldierStartup;
    protected bool GotoSpawnerPos;
    protected float GotoSpawnerPosPriority;
    protected bool TeleportFirstSpawn;

    protected int SpecialEffectsObjID;
    protected float PostVisualSpawnDelay;

    protected bool StartsDisabled;
    protected bool KillHibernatingSpawn;
    protected bool ApplySpawnMaterialEffect;
    protected bool IsMultiplayWeaponSpawner;

    protected List<string> ScriptNameList = [];
    protected List<string> ScriptParameterList = [];

    private const int CHUNKID_DEF_PARENT = 1013991542;
    private const int CHUNKID_DEF_VARIABLES = 1013991543;

    private const int MICROCHUNKID_DEF_DEFINITION_ID = 1;
    private const int XXXMICROCHUNKID_DEF_IS_COMMANDO_STARTING_POINT = 2;
    private const int MICROCHUNKID_DEF_PLAYER_TYPE = 3;
    private const int MICROCHUNKID_DEF_SPAWN_MAX = 4;
    private const int MICROCHUNKID_DEF_SPAWN_DELAY = 5;
    private const int XXXMICROCHUNKID_DEF_AUTO_SPAWN_RADIUS = 6;
    private const int XXXMICROCHUNKID_DEF_ONE_AT_A_TIME = 7;
    private const int MICROCHUNKID_DEF_SPAWN_DELAY_VARIATION = 8;
    private const int MICROCHUNKID_DEF_IS_PRIMARY = 9;
    private const int XXXMICROCHUNKID_DEF_EFFECT_MODEL_NAME = 10;
    private const int XXXMICROCHUNKID_DEF_EFFECT_ANIMATION_NAME = 11;
    private const int XXXMICROCHUNKID_DEF_SOUND_ID = 12;
    private const int MICROCHUNKID_DEF_POST_EFFECT_SPAWN_DELAY = 13;
    private const int MICROCHUNKID_DEF_SPECIAL_EFFECTS_OBJ_ID = 14;
    private const int MICROCHUNKID_DEF_IS_SOLDIER_STARTUP = 15;
    private const int MICROCHUNKID_DEF_GOTO_SPAWNER_POS = 16;
    private const int MICROCHUNKID_DEF_TELEPORT_FIRST_SPAWN = 17;
    private const int MICROCHUNKID_DEF_SCRIPT_NAME = 18;
    private const int MICROCHUNKID_DEF_SCRIPT_PARAMETERS = 19;
    private const int MICROCHUNKID_DEF_STARTS_DISABLED = 20;
    private const int MICROCHUNKID_DEF_KILL_HIBERNATING_SPAWN = 21;
    private const int MICROCHUNKID_DEF_GOTO_SPAWNER_POS_PRIORITY = 22;
    private const int MICROCHUNKID_DEF_APPLY_SPAWN_MATERIAL_EFFECT = 23;
    private const int MICROCHUNKID_DEF_IS_MULTIPLAY_WEAPON_SPAWNER = 24;
}

