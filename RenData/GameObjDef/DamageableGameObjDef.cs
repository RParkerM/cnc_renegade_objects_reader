using RenData.Types;
using RenData.ChunkIO;
using RenData.Definitions;

namespace RenData.GameObjDef;

public abstract class DamageableGameObjDef : ScriptableGameObjDef
{
    public DamageableGameObjDef()
    {
        TranslatedNameID = 0;
        EncyclopediaType = EncyclopediaMgrClass.TYPE.TYPE_UNKNOWN;
        EncyclopediaID = 0;
        NotTargetable = false;
        DefaultPlayerType = (int)PlayerType.PLAYERTYPE_NEUTRAL;
    }
    //        DEFENSEOBJECTDEF_EDITABLE_PARAMS(DamageableGameObjDef, DefenseObjectDef);
    //        EDITABLE_PARAM(DamageableGameObjDef, ParameterClass::TYPE_STRINGSDB_ID, TranslatedNameID);
    //        FILENAME_PARAM(DamageableGameObjDef, InfoIconTextureFilename, "InfoIconTextureFilename", ".TGA");

    //# ifdef	PARAM_EDITING_ON
    //        EnumParameterClass* param = new EnumParameterClass((int*)&EncyclopediaType);
    //        param->Set_Name("Encyclopedia Type");
    //        param->Add_Value("<NA>", 0);
    //        param->Add_Value("Character", EncyclopediaMgrClass::TYPE_CHARACTER);
    //        param->Add_Value("Weapon", EncyclopediaMgrClass::TYPE_WEAPON);
    //        param->Add_Value("Vehicle", EncyclopediaMgrClass::TYPE_VEHICLE);
    //        param->Add_Value("Building", EncyclopediaMgrClass::TYPE_BUILDING);
    //        GENERIC_EDITABLE_PARAM(DamageableGameObjDef, param)


    //    param = new EnumParameterClass(&DefaultPlayerType);
    //        param->Set_Name("PlayerType");
    //        param->Add_Value("Mutant", PLAYERTYPE_MUTANT);
    //        param->Add_Value("Unteamed", PLAYERTYPE_NEUTRAL);
    //        param->Add_Value("Renegade", PLAYERTYPE_RENEGADE);
    //        param->Add_Value("Nod", PLAYERTYPE_NOD);
    //        param->Add_Value("GDI", PLAYERTYPE_GDI);
    //        GENERIC_EDITABLE_PARAM(DamageableGameObjDef, param)
    //#endif

    //	EDITABLE_PARAM(DamageableGameObjDef, ParameterClass::TYPE_INT, EncyclopediaID);
    //        EDITABLE_PARAM(DamageableGameObjDef, ParameterClass::TYPE_BOOL, NotTargetable);
    //        return;

    protected bool ScriptableGameObjDefLoad(ChunkLoadClass cload)
    {
        return base.Load(cload);
    }
    protected bool ScriptableGameObjDefSave(ChunkSaveClass csave)
    {
        return base.Save(csave);
    }

    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        csave.WriteMicro(MICROCHUNKID_DEF_TRANSLATED_NAME_ID, TranslatedNameID);
        if (InfoIconTextureFilename is null) throw new InvalidOperationException("InfoIconTextureFilename is null");
        csave.WriteMicroString(MICROCHUNKID_DEF_INFO_ICON_TEXTURE_FILENAME, InfoIconTextureFilename);
        csave.WriteMicro(MICROCHUNKID_DEF_ENCY_TYPE, (uint)EncyclopediaType);
        csave.WriteMicro(MICROCHUNKID_DEF_ENCY_ID, EncyclopediaID);
        csave.WriteMicro(MICROCHUNKID_DEF_NOT_TARGETABLE, NotTargetable);
        csave.WriteMicro(MICROCHUNKID_DEF_DEFAULT_PLAYER_TYPE, DefaultPlayerType);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_DEFENSEOBJECTDEF);
        DefenseObjectDef.Save(csave);
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

                            case MICROCHUNKID_DEF_TRANSLATED_NAME_ID:
                                cload.Read(ref TranslatedNameID);
                                break;
                            case MICROCHUNKID_DEF_INFO_ICON_TEXTURE_FILENAME:
                                InfoIconTextureFilename = cload.ReadMicroChunkWWString();
                                break;
                            case MICROCHUNKID_DEF_ENCY_TYPE:
                                cload.Read(ref EncyclopediaType);
                                break;
                            case MICROCHUNKID_DEF_ENCY_ID:
                                cload.Read(ref EncyclopediaID);
                                break;
                            case MICROCHUNKID_DEF_NOT_TARGETABLE:
                                cload.Read(ref NotTargetable);
                                break;
                            case MICROCHUNKID_DEF_DEFAULT_PLAYER_TYPE:
                                cload.Read(ref DefaultPlayerType);
                                break;

                            default:
                                Console.WriteLine($"Unhandled MicroChunk:%d File:%s Line: {cload.Cur_Micro_Chunk_ID}");
                                break;

                    }
                    cload.Close_Micro_Chunk();
                    }
                    break;


                case CHUNKID_DEF_DEFENSEOBJECTDEF:
                    DefenseObjectDef.Load(cload);
                    break;

                default:
                    Console.WriteLine($"Unhandled Chunk:%d File:%s Line:   {cload.Cur_Chunk_ID}");
                    break;

            }
            cload.Close_Chunk();
        }
        return true;
    }

    //DECLARE_EDITABLE(DamageableGameObjDef, ScriptableGameObjDef );

    public int Get_Name_ID() { return TranslatedNameID; }

    //	//
    //	//	Encyclopedia information
    //	//
    //	EncyclopediaMgrClass::TYPE Get_Encyclopedia_Type(void) const  { return EncyclopediaType; }
    //	int Get_Encyclopedia_ID(void) const        { return EncyclopediaID; }

    //	//
    //	//	Icon information
    //	//
    //	const StringClass &						Get_Icon_Filename (void) const		{ return InfoIconTextureFilename; }
    //	int Get_Translated_Name_ID(void) const { return TranslatedNameID; }

    //	const DefenseObjectDefClass &			Get_DefenseObjectDef( void ) const	{ return DefenseObjectDef; }

    public int Get_Default_Player_Type() { return DefaultPlayerType; }

    protected DefenseObjectDefClass DefenseObjectDef = new();
    protected string? InfoIconTextureFilename;
    protected int TranslatedNameID;
    protected EncyclopediaMgrClass.TYPE EncyclopediaType;
    protected int EncyclopediaID;
    protected bool NotTargetable;
    protected int DefaultPlayerType;

    private const int CHUNKID_DEF_PARENT = 207011205;
    private const int CHUNKID_DEF_VARIABLES = 207011206;
    private const int CHUNKID_DEF_DEFENSEOBJECTDEF = 207011207;
    private const int MICROCHUNKID_DEF_TRANSLATED_NAME_ID = 1;
    private const int MICROCHUNKID_DEF_INFO_ICON_TEXTURE_FILENAME = 2;
    private const int MICROCHUNKID_DEF_ENCY_TYPE = 3;
    private const int MICROCHUNKID_DEF_ENCY_ID = 4;
    private const int MICROCHUNKID_DEF_NOT_TARGETABLE = 5;
    private const int MICROCHUNKID_DEF_DEFAULT_PLAYER_TYPE = 6;

}
