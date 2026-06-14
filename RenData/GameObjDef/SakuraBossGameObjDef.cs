using RenData.ChunkIO;
using RenData.Definitions;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.GameObjDef;

[RegisterDefinition(ChunkId.CHUNKID_GAME_OBJECT_DEF_SAKURA_BOSS)]
public partial class SakuraBossGameObjDef : VehicleGameObjDef
{
    public SakuraBossGameObjDef()
    {
        GattlingGunDefID = 0;
        RocketLauncherDefID = 0;
        GattlingGunRevSoundDefID = 0;
        RocketDestroyedExplosionID = 0;
        RocketDoorOpenSoundID = 0;
        RocketsDefense = new DefenseObjectDefClass();

        //PARAM_SEPARATOR(SakuraBossGameObjDef, "Rocket Launcher Defense Settings");
        //DEFENSEOBJECTDEF_EDITABLE_PARAMS(SakuraBossGameObjDef, RocketsDefense);
        //PARAM_SEPARATOR(SakuraBossGameObjDef, "");

        //PARAM_SEPARATOR(SakuraBossGameObjDef, "Weapons");
        //EDITABLE_PARAM(SakuraBossGameObjDef, ParameterClass::TYPE_WEAPONOBJDEFINITIONID, RocketLauncherDefID);
        //EDITABLE_PARAM(SakuraBossGameObjDef, ParameterClass::TYPE_WEAPONOBJDEFINITIONID, GattlingGunDefID);
        //EDITABLE_PARAM(SakuraBossGameObjDef, ParameterClass::TYPE_SOUNDDEFINITIONID, GattlingGunRevSoundDefID);
        //EDITABLE_PARAM(SakuraBossGameObjDef, ParameterClass::TYPE_SOUNDDEFINITIONID, RocketDoorOpenSoundID);
        //EDITABLE_PARAM(SakuraBossGameObjDef, ParameterClass::TYPE_EXPLOSIONDEFINITIONID, RocketDestroyedExplosionID);
        //PARAM_SEPARATOR(SakuraBossGameObjDef, "");
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GAME_OBJECT_DEF_SAKURA_BOSS;

    public override PersistClass Create()
    {
        //SakuraBossGameObj* obj = new SakuraBossGameObj;
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
        csave.WriteMicro(VARID_DEF_GATLING_DEF_ID, GattlingGunDefID);
        csave.WriteMicro(VARID_DEF_ROCKET_DEF_ID, RocketLauncherDefID);
        csave.WriteMicro(VARID_DEF_GATLING_REV_SOUND_ID, GattlingGunRevSoundDefID);
        csave.WriteMicro(VARID_DEF_ROCKET_DOOR_SOUND_ID, RocketDoorOpenSoundID);
        csave.WriteMicro(VARID_DEF_ROCKET_DESTROYED_EXPLOSION_ID, RocketDestroyedExplosionID);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_ROCKET_DEFENSEOBJ_DEF);
        RocketsDefense.Save(csave);
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
                            case VARID_DEF_GATLING_DEF_ID:
                                cload.Read(ref GattlingGunDefID);
                                break;
                            case VARID_DEF_ROCKET_DEF_ID:
                                cload.Read(ref RocketLauncherDefID);
                                break;
                            case VARID_DEF_GATLING_REV_SOUND_ID:
                                cload.Read(ref GattlingGunRevSoundDefID);
                                break;
                            case VARID_DEF_ROCKET_DOOR_SOUND_ID:
                                cload.Read(ref RocketDoorOpenSoundID);
                                break;
                            case VARID_DEF_ROCKET_DESTROYED_EXPLOSION_ID:
                                cload.Read(ref RocketDestroyedExplosionID);
                                break;
                            default:
                                Console.WriteLine("Unrecognized SakuraBossGameObjDef Variable chunkID\n");
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                case CHUNKID_DEF_ROCKET_DEFENSEOBJ_DEF:
                    RocketsDefense.Load(cload);
                    break;

                default:
                    Console.WriteLine("Unrecognized SakuraBossGameObjDef chunkID\n");
                    break;
            }
            cload.Close_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    protected int GattlingGunDefID;
    protected int RocketLauncherDefID;
    protected int GattlingGunRevSoundDefID;
    protected int RocketDestroyedExplosionID;
    protected int RocketDoorOpenSoundID;
    protected DefenseObjectDefClass RocketsDefense;


    private const uint CHUNKID_DEF_PARENT = 0x09070458;
    private const uint CHUNKID_DEF_VARIABLES = 0x09070459;
    private const uint CHUNKID_DEF_ROCKET_DEFENSEOBJ_DEF = 0x0907045A;

    private const byte VARID_DEF_GATLING_DEF_ID = 1;
    private const byte VARID_DEF_ROCKET_DEF_ID = 2;
    private const byte VARID_DEF_GATLING_REV_SOUND_ID = 3;
    private const byte VARID_DEF_ROCKET_DOOR_SOUND_ID = 4;
    private const byte VARID_DEF_ROCKET_DESTROYED_EXPLOSION_ID = 5;
}
