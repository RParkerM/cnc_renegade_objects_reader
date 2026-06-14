using System.Numerics;
using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.GameObjDef;

[RegisterDefinition(ChunkId.CHUNKID_GAME_OBJECT_DEF_DAMAGE_ZONE)]
public partial class DamageZoneGameObjDef : BaseGameObjDef
{
    public DamageZoneGameObjDef()
    {
        DamageRate = 10;
        DamageWarhead = 1;
        Color = new Vector3(0.7f, 0f, 0f);

        //#ifdef PARAM_EDITING_ON
        //    EDITABLE_PARAM(DamageZoneGameObjDef, ParameterClass::TYPE_FLOAT, DamageRate);
        //
        //    EnumParameterClass* param = new EnumParameterClass(&DamageWarhead);
        //    param->Set_Name("Damage Warhead");
        //    for (int i = 0; i < ArmorWarheadManager::Get_Num_Warhead_Types(); i++) {
        //        param->Add_Value(ArmorWarheadManager::Get_Warhead_Name(i), i);
        //    }
        //    GENERIC_EDITABLE_PARAM(DamageZoneGameObjDef, param);
        //
        //    EDITABLE_PARAM(DamageZoneGameObjDef, ParameterClass::TYPE_COLOR, Color);
        //#endif
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GAME_OBJECT_DEF_DAMAGE_ZONE;

    public override PersistClass Create()
    {
        //DamageZoneGameObj* zone = new DamageZoneGameObj;
        //zone->Init(*this);
        //return zone;
        throw new NotImplementedException();
    }

    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        csave.WriteMicro(MICROCHUNKID_DEF_ZONE_COLOR, Color);
        csave.WriteMicro(MICROCHUNKID_DEF_DAMAGE_RATE, DamageRate);
        csave.WriteMicro(MICROCHUNKID_DEF_DAMAGE_WARHEAD, DamageWarhead);
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
                            case MICROCHUNKID_DEF_DAMAGE_RATE:
                                cload.Read(ref DamageRate);
                                break;
                            case MICROCHUNKID_DEF_DAMAGE_WARHEAD:
                                cload.Read(ref DamageWarhead);
                                break;
                            default:
                                Console.WriteLine("Unrecognized DamageZoneDef Variable chunkID\n");
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unrecognized DamageZoneDef chunkID\n");
                    break;
            }
            cload.Close_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    public Vector3 Get_Color() { return Color; }

    //DECLARE_EDITABLE(DamageZoneGameObjDef, BaseGameObjDef);

    protected float DamageRate;
    protected int DamageWarhead;
    protected Vector3 Color;


    private const uint CHUNKID_DEF_PARENT = 626000947u;
    private const uint CHUNKID_DEF_VARIABLES = 626000948u;

    private const byte XXXMICROCHUNKID_DEF_DAMAGE_TYPE = 1;
    private const byte MICROCHUNKID_DEF_ZONE_COLOR = 2;
    private const byte MICROCHUNKID_DEF_DAMAGE_RATE = 3;
    private const byte MICROCHUNKID_DEF_DAMAGE_WARHEAD = 4;
}

//SimplePersistFactoryClass<DamageZoneGameObjDef, CHUNKID_GAME_OBJECT_DEF_DAMAGE_ZONE> _DamageZoneGameObjDefPersistFactory;
//DECLARE_DEFINITION_FACTORY(DamageZoneGameObjDef, CLASSID_GAME_OBJECT_DEF_DAMAGE_ZONE, "Damage Zone") _DamageZoneGameObjDefDefFactory;
