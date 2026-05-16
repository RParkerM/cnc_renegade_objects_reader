using RenData.ChunkIO;
using RenData.GameObjDef;
using RenData.IDs;
using RenData.SaveLoad;
using RenData.Types;

namespace RenData.Definitions;

[RegisterDefinition(ChunkId.CHUNKID_GAME_OBJECT_DEF_BUILDING)]
public partial class BuildingGameObjDef : DamageableGameObjDef
{
    public BuildingGameObjDef()
    {

        //            EDITABLE_PARAM(BuildingGameObjDef, ParameterClass::TYPE_STRING, MeshPrefix);

        //# ifdef PARAM_EDITING_ON
        //            int skin_type_counter;
        //            EnumParameterClass* mct_skin_param = new EnumParameterClass((int*)&MCTSkin);
        //            mct_skin_param->Set_Name("MCTSkin");
        //            for (skin_type_counter = 0; skin_type_counter < ArmorWarheadManager::Get_Num_Armor_Types(); skin_type_counter++)
        //            {
        //                mct_skin_param->Add_Value(ArmorWarheadManager::Get_Armor_Name(skin_type_counter), skin_type_counter);
        //            }
        //            GENERIC_EDITABLE_PARAM(BuildingGameObjDef, mct_skin_param);
        //#endif

        //            //
        //            //	Configure the building type parameter
        //            //
        //# ifdef PARAM_EDITING_ON
        //            EnumParameterClass* building_type_param = new EnumParameterClass((int*)&Type);
        //            building_type_param->Set_Name("Building Type");
        //            for (int index = TYPE_NONE; index < TYPE_COUNT; index++)
        //            {
        //                building_type_param->Add_Value(BULDING_TYPE_NAMES[index + 1], index);
        //            }
        //            GENERIC_EDITABLE_PARAM(BuildingGameObjDef, building_type_param);
        //#endif

        //            EDITABLE_PARAM(BuildingGameObjDef, ParameterClass::TYPE_STRINGSDB_ID, GDIDamageReportID);
        //            EDITABLE_PARAM(BuildingGameObjDef, ParameterClass::TYPE_STRINGSDB_ID, NodDamageReportID);
        //            EDITABLE_PARAM(BuildingGameObjDef, ParameterClass::TYPE_STRINGSDB_ID, GDIDestroyReportID);
        //            EDITABLE_PARAM(BuildingGameObjDef, ParameterClass::TYPE_STRINGSDB_ID, NodDestroyReportID);
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GAME_OBJECT_DEF_BUILDING;
    public override PersistClass Create()
    {
        //	BuildingGameObj* obj = new BuildingGameObj;
        //obj->Init( *this );
        //	return obj;
        throw new NotImplementedException();
    }
    public override bool Save(ChunkSaveClass csave)
    {

        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        ArgumentNullException.ThrowIfNull(MeshPrefix);
        csave.WriteMicroString(MICROCHUNKID_DEF_MESHPREFIX, MeshPrefix);
        csave.WriteMicro(MICROCHUNKID_DEF_MCTSKIN, MCTSkin);
        csave.WriteMicro(MICROCHUNKID_DEF_BUILDING_TYPE, Type);
        csave.WriteMicro(MICROCHUNKID_DEF_GDI_DAMAGE_REPORT_ID, GDIDamageReportID);
        csave.WriteMicro(MICROCHUNKID_DEF_NOD_DAMAGE_REPORT_ID, NodDamageReportID);
        csave.WriteMicro(MICROCHUNKID_DEF_GDI_DESTROY_REPORT_ID, GDIDestroyReportID);
        csave.WriteMicro(MICROCHUNKID_DEF_NOD_DESTROY_REPORT_ID, NodDestroyReportID);
        csave.End_Chunk();

        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        int legacy_team = -1;

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
                            case MICROCHUNKID_DEF_MESHPREFIX:
                                cload.ReadMicroChunkWWString(out MeshPrefix);
                                break;
                            case MICROCHUNKID_DEF_MCTSKIN:
                                cload.Read(ref MCTSkin);
                                break;
                            case MICROCHUNKID_DEF_BUILDING_TYPE:
                                cload.Read(ref Type);
                                break;
                            case LEGACY_MICROCHUNKID_DEF_BUILDING_TEAM:
                                cload.Read(ref legacy_team);
                                break;
                            case MICROCHUNKID_DEF_GDI_DAMAGE_REPORT_ID:
                                cload.Read(ref GDIDamageReportID);
                                break;
                            case MICROCHUNKID_DEF_NOD_DAMAGE_REPORT_ID:
                                cload.Read(ref NodDamageReportID);
                                break;
                            case MICROCHUNKID_DEF_GDI_DESTROY_REPORT_ID:
                                cload.Read(ref GDIDestroyReportID);
                                break;
                            case MICROCHUNKID_DEF_NOD_DESTROY_REPORT_ID:
                                cload.Read(ref NodDestroyReportID);
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

            if (legacy_team != -1)
            {
                DefaultPlayerType = (int)PlayerType.PLAYERTYPE_GDI;
                if (legacy_team == (int)BuildingConstants.LegacyBuildingTeam.LEGACY_TEAM_NOD)
                {
                    DefaultPlayerType = (int)PlayerType.PLAYERTYPE_NOD;
                }
            }
        }

        return true;
    }
    public override PersistFactoryClass Get_Factory() => _persistFactory;

    //DECLARE_EDITABLE(BuildingGameObjDef, DamageableGameObjDef );

    public void Set_Type(BuildingConstants.BuildingType type) { Type = type; }
    public BuildingConstants.BuildingType Get_Type() { return Type; }

    public string Get_Mesh_Prefix()
    {
        ArgumentNullException.ThrowIfNull(MeshPrefix);
        return MeshPrefix;
    }

    public int Get_Damage_Report(int team)
    {
        if ((int)PlayerType.PLAYERTYPE_GDI == team)
        {
            return GDIDamageReportID;
        }
        else if ((int)PlayerType.PLAYERTYPE_NOD == team)
        {
            return NodDamageReportID;
        }

        return 0;
    }
    public int Get_Destroy_Report(int team)
    {
        if ((int)PlayerType.PLAYERTYPE_GDI == team)
        {
            return GDIDestroyReportID;
        }
        else if ((int)PlayerType.PLAYERTYPE_NOD == team)
        {
            return NodDestroyReportID;
        }

        return 0;
    }


    protected string? MeshPrefix;
    protected ArmorType MCTSkin = 0;
    protected BuildingConstants.BuildingType Type = BuildingConstants.BuildingType.TYPE_NONE;

    protected int GDIDamageReportID = 0;
    protected int NodDamageReportID = 0;
    protected int GDIDestroyReportID = 0;
    protected int NodDestroyReportID = 0;

    private const uint CHUNKID_DEF_PARENT = 207011030;
    private const uint CHUNKID_DEF_VARIABLES = 207011031;

    // Microchunks (start at 1)
    private const uint MICROCHUNKID_DEF_MESHPREFIX = 1;
    private const uint MICROCHUNKID_DEF_MCTSKIN = 2;
    private const uint MICROCHUNKID_DEF_BUILDING_TYPE = 3;
    private const uint LEGACY_MICROCHUNKID_DEF_BUILDING_TEAM = 4;
    private const uint MICROCHUNKID_DEF_GDI_DAMAGE_REPORT_ID = 5;
    private const uint MICROCHUNKID_DEF_NOD_DAMAGE_REPORT_ID = 6;
    private const uint MICROCHUNKID_DEF_GDI_DESTROY_REPORT_ID = 7;
    private const uint MICROCHUNKID_DEF_NOD_DESTROY_REPORT_ID = 8;
};
