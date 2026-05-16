using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;
using RenData.Types;

namespace RenData.GameObjDef;

[RegisterDefinition(ChunkId.CHUNKID_GAME_OBJECT_DEF_SIMPLE)]
public partial class SimpleGameObjDef : PhysicalGameObjDef
{

    public SimpleGameObjDef()
    {
        IsEditorObject = false;
        IsHiddenObject = false;
        PlayerTerminalType = PlayerTerminalClassType.TYPE_NONE;
            //            MODEL_DEF_PARAM(SimpleGameObjDef, PhysDefID, "PhysDef");
            //            EDITABLE_PARAM(SimpleGameObjDef, ParameterClass::TYPE_BOOL, IsEditorObject);
            //            EDITABLE_PARAM(SimpleGameObjDef, ParameterClass::TYPE_BOOL, IsHiddenObject);

            //# ifdef	PARAM_EDITING_ON

            //            //
            //            //	Configure the orator types parameter
            //            //
            //            EnumParameterClass* pt_type_param = new EnumParameterClass((int*)&PlayerTerminalType);
            //            pt_type_param->Set_Name("Player Terminal Type");
            //            pt_type_param->Add_Value("<None>", PlayerTerminalClass::TYPE_NONE);
            //            pt_type_param->Add_Value("GDI", PlayerTerminalClass::TYPE_GDI);
            //            pt_type_param->Add_Value("NOD", PlayerTerminalClass::TYPE_NOD);
            //            pt_type_param->Add_Value("Mutant", PlayerTerminalClass::TYPE_MUTANT);
            //            GENERIC_EDITABLE_PARAM(SimpleGameObjDef, pt_type_param);
            //#endif
    }


    public override uint Get_Class_ID() => ClassId.CLASSID_GAME_OBJECT_DEF_SIMPLE;
    public override PersistClass Create()
    {
        //SimpleGameObj* obj = new SimpleGameObj;
        //obj->Init(*this);
        //return obj;
        throw new NotImplementedException();
    }
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        csave.WriteMicro(MICROCHUNKID_DEF_IS_EDITOR_OBJECT, IsEditorObject);
        csave.WriteMicro(MICROCHUNKID_DEF_IS_HIDDEN_OBJECT, IsHiddenObject);
        csave.WriteMicro(MICROCHUNKID_DEF_PLAYER_TERM_TYPE, PlayerTerminalType);
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

                            case MICROCHUNKID_DEF_IS_EDITOR_OBJECT:
                                cload.Read(ref IsEditorObject);
                                break;
                            case MICROCHUNKID_DEF_IS_HIDDEN_OBJECT:
                                cload.Read(ref IsHiddenObject);
                                break;
                            case MICROCHUNKID_DEF_PLAYER_TERM_TYPE:
                                cload.Read(ref PlayerTerminalType);
                                break;

                            default:
                                Console.WriteLine("Unrecognized SimpleDef Variable chunkID\n");
                                break;

                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unrecognized SimpleDef chunkID\n");
                    break;

            }
            cload.Close_Chunk();
        }
        return true;
    }
    public override PersistFactoryClass Get_Factory() => _persistFactory;

    //DECLARE_EDITABLE(SimpleGameObjDef, PhysicalGameObjDef );

    // Accessors
    public PlayerTerminalClassType Get_Player_Terminal_Type() { return PlayerTerminalType; }
    public bool Get_Is_Editor_Object() { return IsEditorObject; }

    protected bool IsEditorObject;
    protected bool IsHiddenObject;

    // See playerterminal.h
    protected PlayerTerminalClassType PlayerTerminalType;

    protected bool PhysicalGameObjDefLoad(ChunkLoadClass cload) => base.Load(cload);
    protected bool PhysicalGameObjDefSave(ChunkSaveClass csave) => base.Save(csave);

    private const uint CHUNKID_DEF_PARENT = 930991656;
    private const uint CHUNKID_DEF_VARIABLES = 930991657;

    private const uint MICROCHUNKID_DEF_IS_EDITOR_OBJECT = 1;
    private const uint MICROCHUNKID_DEF_IS_HIDDEN_OBJECT = 2;
    private const uint MICROCHUNKID_DEF_PLAYER_TERM_TYPE = 3;
};

//SimplePersistFactoryClass<SimpleGameObjDef, CHUNKID_GAME_OBJECT_DEF_SIMPLE> _SimpleGameObjDefPersistFactory;
//DECLARE_DEFINITION_FACTORY(SimpleGameObjDef, CLASSID_GAME_OBJECT_DEF_SIMPLE, "Simple") _SimpleGameObjDefDefFactory;




