using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.Definitions;

[RegisterDefinition(ChunkId.CHUNKID_GLOBAL_SETTINGS_DEF_PURCHASE)]
public partial class PurchaseSettingsDefClass : DefinitionClass
{
    public const int TYPE_CLASSES = 0;
    public const int TYPE_VEHICLES = 1;
    public const int TYPE_EQUIPMENT = 2;
    public const int TYPE_SECRET_CLASSES = 3;
    public const int TYPE_SECRET_VEHICLES = 4;
    public const int TYPE_COUNT = 5;

    public const int TEAM_GDI = 0;
    public const int TEAM_NOD = 1;
    public const int TEAM_MUTANT_GDI = 2;
    public const int TEAM_MUTANT_NOD = 3;
    public const int TEAM_COUNT = 4;

    public const int MAX_ALTERNATES = 3;
    public const int MAX_ENTRIES = 10;

    public PurchaseSettingsDefClass()
    {
        Team = TEAM_GDI;
        Type = TYPE_CLASSES;

        CostList = new int[MAX_ENTRIES];
        DefinitionList = new int[MAX_ENTRIES];
        NameList = new int[MAX_ENTRIES];
        TextureList = new string[MAX_ENTRIES];
        AlternateDefinitionList = new int[MAX_ENTRIES, MAX_ALTERNATES];
        AlternateTextureList = new string[MAX_ENTRIES, MAX_ALTERNATES];

        for (int i = 0; i < MAX_ENTRIES; i++)
        {
            TextureList[i] = string.Empty;
            for (int j = 0; j < MAX_ALTERNATES; j++)
            {
                AlternateTextureList[i, j] = string.Empty;
            }
        }
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GLOBAL_SETTINGS_DEF_PURCHASE;

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
        csave.WriteMicro(VARID_TYPE, Type);

        for (int i = 0; i < MAX_ENTRIES; i++)
        {
            csave.WriteMicro(VARID_INDEX, i);
            csave.WriteMicro(VARID_COST, CostList[i]);
            csave.WriteMicro(VARID_DEFINITION, DefinitionList[i]);
            csave.WriteMicro(VARID_NAME, NameList[i]);
            csave.WriteMicroString(VARID_TEXTURE_NAME, TextureList[i]);

            for (int alt = 0; alt < MAX_ALTERNATES; alt++)
            {
                csave.WriteMicro(VARID_ALT_INDEX, alt);
                csave.WriteMicroString(VARID_ALT_TEXTURE_NAME, AlternateTextureList[i, alt]);
                csave.WriteMicro(VARID_ALT_DEFINITION, AlternateDefinitionList[i, alt]);
            }
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
                    Console.WriteLine("Unhandled PurchaseSettingsDefClass chunkID\n");
                    break;
            }
            cload.Close_Chunk();
        }

        return true;
    }

    private void Load_Variables(ChunkLoadClass cload)
    {
        int entryIndex = 0;
        int altIndex = 0;

        while (cload.Open_Micro_Chunk())
        {
            switch (cload.Cur_Micro_Chunk_ID)
            {
                case VARID_TEAM:
                    cload.Read(ref Team);
                    break;
                case VARID_TYPE:
                    cload.Read(ref Type);
                    break;
                case VARID_INDEX:
                    cload.Read(ref entryIndex);
                    break;
                case VARID_ALT_INDEX:
                    cload.Read(ref altIndex);
                    break;
                case VARID_COST:
                    if (entryIndex >= 0 && entryIndex < MAX_ENTRIES)
                        cload.Read(ref CostList[entryIndex]);
                    break;
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
                case VARID_ALT_DEFINITION:
                    if (entryIndex >= 0 && entryIndex < MAX_ENTRIES && altIndex >= 0 && altIndex < MAX_ALTERNATES)
                    {
                        int v = 0;
                        cload.Read(ref v);
                        AlternateDefinitionList[entryIndex, altIndex] = v;
                    }
                    break;
                case VARID_ALT_TEXTURE_NAME:
                    if (entryIndex >= 0 && entryIndex < MAX_ENTRIES && altIndex >= 0 && altIndex < MAX_ALTERNATES)
                        AlternateTextureList[entryIndex, altIndex] = cload.ReadMicroChunkWWString();
                    break;
                default:
                    Console.WriteLine("Unhandled PurchaseSettingsDefClass Variable chunkID\n");
                    break;
            }
            cload.Close_Micro_Chunk();
        }
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    protected int Team;
    protected int Type;
    protected int[] CostList;
    protected int[] DefinitionList;
    protected int[] NameList;
    protected string[] TextureList;
    protected int[,] AlternateDefinitionList;
    protected string[,] AlternateTextureList;


    private const uint CHUNKID_PARENT = 0x08071203;
    private const uint CHUNKID_VARIABLES = 0x08071204;

    private const byte VARID_TEAM = 1;
    private const byte VARID_TYPE = 2;
    private const byte XXX_VARID_ROW = 3;
    private const byte XXX_VARID_COL = 4;
    private const byte VARID_COST = 5;
    private const byte VARID_DEFINITION = 6;
    private const byte VARID_TEXTURE_NAME = 7;
    private const byte VARID_NAME = 8;
    private const byte VARID_INDEX = 9;
    private const byte VARID_ALT_INDEX = 10;
    private const byte VARID_ALT_TEXTURE_NAME = 11;
    private const byte VARID_ALT_DEFINITION = 12;
}
