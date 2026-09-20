using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.Definitions;

[RegisterDefinition(ChunkId.CHUNKID_GLOBAL_SETTINGS_DEF_CHAR_CLASS)]
public partial class CharacterClassSettingsDefClass : DefinitionClass
{
    public const int CLASS_MINIGUNNER = 0;
    public const int CLASS_ROCKET_SOLDIER = 1;
    public const int CLASS_GRENADIER = 2;
    public const int CLASS_ENGINEER = 3;
    public const int CLASS_FLAME_THROWER = 4;
    public const int CLASS_MUTANT = 5;
    public const int CLASS_COUNT = 6;

    public const int RANK_ENLISTED = 0;
    public const int RANK_OFFICER = 1;
    public const int RANK_SPECIAL_FORCES = 2;
    public const int RANK_BOSS = 3;
    public const int RANK_COUNT = 4;

    public const int TEAM_GDI = 0;
    public const int TEAM_NOD = 1;
    public const int TEAM_COUNT = 2;

    public CharacterClassSettingsDefClass()
    {
        CostTable = new int[CLASS_COUNT, RANK_COUNT, TEAM_COUNT];
        DefinitionTable = new int[CLASS_COUNT, RANK_COUNT, TEAM_COUNT];
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GLOBAL_SETTINGS_DEF_CHAR_CLASS;

    public override PersistClass? Create()
    {
        //WWASSERT(0);
        return null;
    }

    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_VARIABLES);

        // GDI Cost (24 entries, IDs 1-24)
        WriteCost(csave, 1, CLASS_MINIGUNNER, TEAM_GDI);
        WriteCost(csave, 5, CLASS_ROCKET_SOLDIER, TEAM_GDI);
        WriteCost(csave, 9, CLASS_GRENADIER, TEAM_GDI);
        WriteCost(csave, 13, CLASS_ENGINEER, TEAM_GDI);
        WriteCost(csave, 17, CLASS_FLAME_THROWER, TEAM_GDI);
        WriteCost(csave, 21, CLASS_MUTANT, TEAM_GDI);

        // NOD Cost (24 entries, IDs 25-48)
        WriteCost(csave, 25, CLASS_MINIGUNNER, TEAM_NOD);
        WriteCost(csave, 29, CLASS_ROCKET_SOLDIER, TEAM_NOD);
        WriteCost(csave, 33, CLASS_GRENADIER, TEAM_NOD);
        WriteCost(csave, 37, CLASS_ENGINEER, TEAM_NOD);
        WriteCost(csave, 41, CLASS_FLAME_THROWER, TEAM_NOD);
        WriteCost(csave, 45, CLASS_MUTANT, TEAM_NOD);

        // GDI DefID (24 entries, IDs 49-72)
        WriteDef(csave, 49, CLASS_MINIGUNNER, TEAM_GDI);
        WriteDef(csave, 53, CLASS_ROCKET_SOLDIER, TEAM_GDI);
        WriteDef(csave, 57, CLASS_GRENADIER, TEAM_GDI);
        WriteDef(csave, 61, CLASS_ENGINEER, TEAM_GDI);
        WriteDef(csave, 65, CLASS_FLAME_THROWER, TEAM_GDI);
        WriteDef(csave, 69, CLASS_MUTANT, TEAM_GDI);

        // NOD DefID (24 entries, IDs 73-96)
        WriteDef(csave, 73, CLASS_MINIGUNNER, TEAM_NOD);
        WriteDef(csave, 77, CLASS_ROCKET_SOLDIER, TEAM_NOD);
        WriteDef(csave, 81, CLASS_GRENADIER, TEAM_NOD);
        WriteDef(csave, 85, CLASS_ENGINEER, TEAM_NOD);
        WriteDef(csave, 89, CLASS_FLAME_THROWER, TEAM_NOD);
        WriteDef(csave, 93, CLASS_MUTANT, TEAM_NOD);

        csave.End_Chunk();

        return true;
    }

    private void WriteCost(ChunkSaveClass csave, int baseId, int charClass, int team)
    {
        for (int rank = 0; rank < RANK_COUNT; rank++)
        {
            csave.WriteMicro((byte)(baseId + rank), CostTable[charClass, rank, team]);
        }
    }

    private void WriteDef(ChunkSaveClass csave, int baseId, int charClass, int team)
    {
        for (int rank = 0; rank < RANK_COUNT; rank++)
        {
            csave.WriteMicro((byte)(baseId + rank), DefinitionTable[charClass, rank, team]);
        }
    }

    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {
                case CHUNKID_PARENT:
                    base.Load(cload);
                    break;

                case CHUNKID_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        int id = (int)cload.Cur_Micro_Chunk_ID;
                        // ID 1-48 = Cost (24 GDI then 24 NOD), 49-96 = Def (24 GDI then 24 NOD)
                        // Within each block: 6 classes × 4 ranks = 24, class index = (offset / 4), rank = (offset % 4)
                        if (id >= 1 && id <= 96)
                        {
                            int idx = id - 1; // 0..95
                            bool isDef = idx >= 48;
                            int blockIdx = idx % 48; // 0..47 within cost/def
                            int team = blockIdx / 24; // 0=GDI, 1=NOD
                            int withinTeam = blockIdx % 24;
                            int charClass = withinTeam / 4;
                            int rank = withinTeam % 4;

                            int value = 0;
                            cload.Read(ref value);
                            if (isDef)
                                DefinitionTable[charClass, rank, team] = value;
                            else
                                CostTable[charClass, rank, team] = value;
                        }
                        else
                        {
                            Console.WriteLine("Unhandled CharacterClassSettingsDefClass Variable chunkID\n");
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unhandled CharacterClassSettingsDefClass chunkID\n");
                    break;
            }
            cload.Close_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    public int Get_Cost(int charClass, int charRank, int charTeam)
    {
        if (charClass >= 0 && charClass < CLASS_COUNT &&
            charRank >= 0 && charRank < RANK_COUNT &&
            charTeam >= 0 && charTeam < TEAM_COUNT)
        {
            return CostTable[charClass, charRank, charTeam];
        }
        return -1;
    }

    public int Get_Definition(int charClass, int charRank, int charTeam)
    {
        if (charClass >= 0 && charClass < CLASS_COUNT &&
            charRank >= 0 && charRank < RANK_COUNT &&
            charTeam >= 0 && charTeam < TEAM_COUNT)
        {
            return DefinitionTable[charClass, charRank, charTeam];
        }
        return -1;
    }

    // CostTable[class][rank][team], DefinitionTable[class][rank][team]
    protected int[,,] CostTable;
    protected int[,,] DefinitionTable;


    private const uint CHUNKID_PARENT = 0x12021027;
    private const uint CHUNKID_VARIABLES = 0x12021028;
}
