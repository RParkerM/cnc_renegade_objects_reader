using RenData.ChunkIO;

namespace RenData.GameObjDef;

public abstract class ArmedGameObjDef : PhysicalGameObjDef
{
    public ArmedGameObjDef()
    {
        WeaponTiltRate = 1.0f;
        WeaponTiltMin = -10000.0f;
        WeaponTiltMax = 10000.0f;
        WeaponTurnRate = 1.0f;
        WeaponTurnMin = -10000.0f;
        WeaponTurnMax = 10000.0f;
        WeaponError = 0;
        WeaponDefID = 0;
        SecondaryWeaponDefID = 0;
        WeaponRounds = -1;
        //{
        //    EDITABLE_PARAM(ArmedGameObjDef, ParameterClass::TYPE_ANGLE, WeaponTiltRate);
        //    EDITABLE_PARAM(ArmedGameObjDef, ParameterClass::TYPE_ANGLE, WeaponTiltMin);
        //    EDITABLE_PARAM(ArmedGameObjDef, ParameterClass::TYPE_ANGLE, WeaponTiltMax);
        //    EDITABLE_PARAM(ArmedGameObjDef, ParameterClass::TYPE_ANGLE, WeaponTurnRate);
        //    EDITABLE_PARAM(ArmedGameObjDef, ParameterClass::TYPE_ANGLE, WeaponTurnMin);
        //    EDITABLE_PARAM(ArmedGameObjDef, ParameterClass::TYPE_ANGLE, WeaponTurnMax);
        //    EDITABLE_PARAM(ArmedGameObjDef, ParameterClass::TYPE_ANGLE, WeaponError);
        //    EDITABLE_PARAM(ArmedGameObjDef, ParameterClass::TYPE_WEAPONOBJDEFINITIONID, WeaponDefID);
        //    EDITABLE_PARAM(ArmedGameObjDef, ParameterClass::TYPE_INT, WeaponRounds);
        //    EDITABLE_PARAM(ArmedGameObjDef, ParameterClass::TYPE_WEAPONOBJDEFINITIONID, SecondaryWeaponDefID);
        //}
    }

    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        csave.WriteMicro(MICROCHUNKID_DEF_WEAPON_TILT_RATE, WeaponTiltRate);
        csave.WriteMicro(MICROCHUNKID_DEF_WEAPON_TILT_MIN, WeaponTiltMin);
        csave.WriteMicro(MICROCHUNKID_DEF_WEAPON_TILT_MAX, WeaponTiltMax);
        csave.WriteMicro(MICROCHUNKID_DEF_WEAPON_TURN_RATE, WeaponTurnRate);
        csave.WriteMicro(MICROCHUNKID_DEF_WEAPON_TURN_MIN, WeaponTurnMin);
        csave.WriteMicro(MICROCHUNKID_DEF_WEAPON_TURN_MAX, WeaponTurnMax);
        csave.WriteMicro(MICROCHUNKID_DEF_WEAPON_DEF_ID, WeaponDefID);
        csave.WriteMicro(MICROCHUNKID_DEF_SECONDARY_WEAPON_DEF_ID, SecondaryWeaponDefID);
        csave.WriteMicro(MICROCHUNKID_DEF_WEAPON_ROUNDS, WeaponRounds);
        csave.WriteMicro(MICROCHUNKID_DEF_WEAPON_ERROR, WeaponError);
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

                            case MICROCHUNKID_DEF_WEAPON_TILT_RATE:
                                cload.Read(ref WeaponTiltRate);
                                break;
                            case MICROCHUNKID_DEF_WEAPON_TILT_MIN:
                                cload.Read(ref WeaponTiltMin);
                                break;
                            case MICROCHUNKID_DEF_WEAPON_TILT_MAX:
                                cload.Read(ref WeaponTiltMax);
                                break;
                            case MICROCHUNKID_DEF_WEAPON_TURN_RATE:
                                cload.Read(ref WeaponTurnRate);
                                break;
                            case MICROCHUNKID_DEF_WEAPON_TURN_MIN:
                                cload.Read(ref WeaponTurnMin);
                                break;
                            case MICROCHUNKID_DEF_WEAPON_TURN_MAX:
                                cload.Read(ref WeaponTurnMax);
                                break;
                            case MICROCHUNKID_DEF_WEAPON_DEF_ID:
                                cload.Read(ref WeaponDefID);
                                break;
                            case MICROCHUNKID_DEF_SECONDARY_WEAPON_DEF_ID:
                                cload.Read(ref SecondaryWeaponDefID);
                                break;
                            case MICROCHUNKID_DEF_WEAPON_ROUNDS:
                                cload.Read(ref WeaponRounds);
                                break;
                            case MICROCHUNKID_DEF_WEAPON_ERROR:
                                cload.Read(ref WeaponError);
                                break;

                            default:
                                Console.WriteLine($"Unrecognized ArmedDef Variable chunkID {cload.Cur_Micro_Chunk_ID}");
                                break;

                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine($"Unrecognized ArmedDef chunkID {cload.Cur_Chunk_ID}");
                    break;

            }
            cload.Close_Chunk();
        }

        return true;
    }

    protected float WeaponTiltRate;
    protected float WeaponTiltMin;
    protected float WeaponTiltMax;
    protected float WeaponTurnRate;
    protected float WeaponTurnMin;
    protected float WeaponTurnMax;
    protected float WeaponError;

    protected int WeaponDefID;
    protected int SecondaryWeaponDefID;
    protected int WeaponRounds;

    private const int CHUNKID_DEF_PARENT = 418001829;
    private const int CHUNKID_DEF_VARIABLES = 418001830;
    private const int MICROCHUNKID_DEF_WEAPON_TILT_RATE = 1;
    private const int MICROCHUNKID_DEF_WEAPON_TILT_MIN = 2;
    private const int MICROCHUNKID_DEF_WEAPON_TILT_MAX = 3;
    private const int MICROCHUNKID_DEF_WEAPON_TURN_RATE = 4;

    private const int MICROCHUNKID_DEF_WEAPON_TURN_MIN = 5;
    private const int MICROCHUNKID_DEF_WEAPON_TURN_MAX = 6;
    private const int XXXMICROCHUNKID_DEF_PRIMARY_ROUNDS = 7;
    private const int XXXMICROCHUNKID_DEF_PRIMARY_AMMO_WEAPON_DEF_ID = 8;
    private const int XXXMICROCHUNKID_DEF_SECONDARY_AMMO_WEAPON_DEF_ID = 9;
    private const int XXXMICROCHUNKID_DEF_SECONDARY_ROUNDS = 10;
    private const int MICROCHUNKID_DEF_WEAPON_DEF_ID = 11;
    private const int MICROCHUNKID_DEF_WEAPON_ROUNDS = 12;
    private const int MICROCHUNKID_DEF_WEAPON_ERROR = 13;
    private const int MICROCHUNKID_DEF_SECONDARY_WEAPON_DEF_ID = 14;
}
