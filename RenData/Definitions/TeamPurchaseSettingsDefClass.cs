using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.Definitions;

[RegisterDefinition(ChunkId.CHUNKID_GLOBAL_SETTINGS_DEF_TEAM_PURCHASE)]
public partial class TeamPurchaseSettingsDefClass : DefinitionClass
{
    public const int TEAM_GDI = 0;
    public const int TEAM_NOD = 1;
    public const int TEAM_COUNT = 2;
    public const int MAX_ENTRIES = 4;

    public TeamPurchaseSettingsDefClass()
    {
        Team = TEAM_GDI;
        BeaconNameID = 0;
        BeaconDefinitionID = 0;
        BeaconCost = 0;
        SupplyNameID = 0;

        DefinitionList = new int[MAX_ENTRIES];
        NameList = new int[MAX_ENTRIES];
        TextureList = new string[MAX_ENTRIES];
        for (int i = 0; i < MAX_ENTRIES; i++) TextureList[i] = string.Empty;
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GLOBAL_SETTINGS_DEF_TEAM_PURCHASE;

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

        csave.WriteMicro(VARID_TEAM, Team);
        csave.WriteMicro(VARID_BEACON_DEFINITION, BeaconDefinitionID);
        csave.WriteMicro(VARID_BEACON_NAME, BeaconNameID);
        csave.WriteMicro(VARID_BEACON_COST, BeaconCost);
        ArgumentNullException.ThrowIfNull(BeaconTextureName);
        csave.WriteMicroString(VARID_BEACON_TEXTURE_NAME, BeaconTextureName);
        csave.WriteMicro(VARID_SUPPLY_NAME, SupplyNameID);
        ArgumentNullException.ThrowIfNull(SupplyTextureName);
        csave.WriteMicroString(VARID_SUPPLY_TEXTURE_NAME, SupplyTextureName);

        for (int i = 0; i < MAX_ENTRIES; i++)
        {
            csave.WriteMicro(VARID_INDEX, i);
            csave.WriteMicro(VARID_DEFINITION, DefinitionList[i]);
            csave.WriteMicro(VARID_NAME, NameList[i]);
            csave.WriteMicroString(VARID_TEXTURE_NAME, TextureList[i]);
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
                case CHUNKID_PARENT:
                    base.Load(cload);
                    break;

                case CHUNKID_VARIABLES:
                    Load_Variables(cload);
                    break;

                default:
                    Console.WriteLine("Unhandled TeamPurchaseSettingsDefClass chunkID\n");
                    break;
            }
            cload.Close_Chunk();
        }

        return true;
    }

    private void Load_Variables(ChunkLoadClass cload)
    {
        int entryIndex = 0;

        while (cload.Open_Micro_Chunk())
        {
            switch (cload.Cur_Micro_Chunk_ID)
            {
                case VARID_TEAM: cload.Read(ref Team); break;
                case VARID_BEACON_DEFINITION: cload.Read(ref BeaconDefinitionID); break;
                case VARID_BEACON_NAME: cload.Read(ref BeaconNameID); break;
                case VARID_BEACON_COST: cload.Read(ref BeaconCost); break;
                case VARID_BEACON_TEXTURE_NAME: BeaconTextureName = cload.ReadMicroChunkWWString(); break;
                case VARID_SUPPLY_NAME: cload.Read(ref SupplyNameID); break;
                case VARID_SUPPLY_TEXTURE_NAME: SupplyTextureName = cload.ReadMicroChunkWWString(); break;
                case VARID_INDEX: cload.Read(ref entryIndex); break;
                case VARID_DEFINITION:
                    if (entryIndex >= 0 && entryIndex < MAX_ENTRIES)
                        cload.Read(ref DefinitionList[entryIndex]);
                    break;
                case VARID_NAME:
                    if (entryIndex >= 0 && entryIndex < MAX_ENTRIES)
                        cload.Read(ref NameList[entryIndex]);
                    break;
                case VARID_TEXTURE_NAME:
                    if (entryIndex >= 0 && entryIndex < MAX_ENTRIES)
                        TextureList[entryIndex] = cload.ReadMicroChunkWWString();
                    break;
                default:
                    Console.WriteLine("Unhandled TeamPurchaseSettingsDefClass Variable chunkID\n");
                    break;
            }
            cload.Close_Micro_Chunk();
        }
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    protected int Team;
    protected int BeaconDefinitionID;
    protected int BeaconNameID;
    protected int BeaconCost;
    protected string? BeaconTextureName;
    protected int SupplyNameID;
    protected string? SupplyTextureName;
    protected int[] DefinitionList;
    protected int[] NameList;
    protected string[] TextureList;


    private const uint CHUNKID_PARENT = 0x10231215;
    private const uint CHUNKID_VARIABLES = 0x10231216;

    private const byte VARID_TEAM = 1;
    private const byte VARID_DEFINITION = 2;
    private const byte VARID_TEXTURE_NAME = 3;
    private const byte VARID_NAME = 4;
    private const byte VARID_INDEX = 5;
    private const byte VARID_BEACON_DEFINITION = 6;
    private const byte VARID_BEACON_NAME = 7;
    private const byte VARID_BEACON_TEXTURE_NAME = 8;
    private const byte VARID_BEACON_COST = 9;
    private const byte VARID_SUPPLY_NAME = 10;
    private const byte VARID_SUPPLY_TEXTURE_NAME = 11;
}
