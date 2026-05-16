using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.GameObjDef;

[RegisterDefinition(ChunkId.CHUNKID_GAME_OBJECT_DEF_C4)]
public partial class C4GameObjDef : SimpleGameObjDef
{

    public C4GameObjDef()
    {
        ThrowVelocity = 5;
        //EDITABLE_PARAM(C4GameObjDef, ParameterClass::TYPE_FLOAT, ThrowVelocity);
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GAME_OBJECT_DEF_C4;
    public override PersistClass Create()
    {
        //C4GameObj* obj = new C4GameObj;
        //obj->Init(*this);
        //return obj;
        throw new NotImplementedException();
    }
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        PhysicalGameObjDefSave(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        csave.WriteMicro(MICROCHUNKID_DEF_THROW_VELOCITY, ThrowVelocity);
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
                    PhysicalGameObjDefLoad(cload);
                    break;

                case CHUNKID_DEF_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {

                            case MICROCHUNKID_DEF_THROW_VELOCITY:
                                cload.Read(ref ThrowVelocity);
                                break;
                            default:
                                Console.WriteLine($"Unhandled Micro Chunk:{cload.Cur_Micro_Chunk_ID}");
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

    //DECLARE_EDITABLE(C4GameObjDef, SimpleGameObjDef );

    protected float ThrowVelocity;

    private const int CHUNKID_DEF_PARENT = 930991700;
    private const int CHUNKID_DEF_VARIABLES = 930991701;

    private const int MICROCHUNKID_DEF_THROW_VELOCITY = 1;
};



//SimplePersistFactoryClass<C4GameObjDef, CHUNKID_GAME_OBJECT_DEF_C4> _C4GameObjDefPersistFactory;
//DECLARE_DEFINITION_FACTORY(C4GameObjDef, CLASSID_GAME_OBJECT_DEF_C4, "C4") _C4GameObjDefDefFactory;
