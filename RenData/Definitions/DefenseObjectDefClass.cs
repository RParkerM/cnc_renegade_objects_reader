using RenData.ChunkIO;

namespace RenData.Definitions;

public class DefenseObjectDefClass
{
    public DefenseObjectDefClass()
    {
        Health = 100.0f;
        HealthMax = 100.0f;
        Skin = 0;
        ShieldStrength = 0;
        ShieldStrengthMax = 0;
        ShieldType = 0;
        DamagePoints = 0;
        DeathPoints = 0;
    }
    public bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(DEFENSEOBJECTDEF_CHUNK_VARIABLES);
        csave.WriteMicro(DEFENSEOBJECTDEF_VARIABLE_HEALTH, Health);
        csave.WriteMicro(DEFENSEOBJECTDEF_VARIABLE_HEALTHMAX, HealthMax);
        ///TODO: Change this to use ArmorWarheadManager
        //int skin_save_id = ArmorWarheadManager::Get_Armor_Save_ID(Skin);
        //WRITE_MICRO_CHUNK(csave, DEFENSEOBJECTDEF_VARIABLE_SKIN, skin_save_id);
        csave.WriteMicro(DEFENSEOBJECTDEF_VARIABLE_SKIN, Skin);

        csave.WriteMicro(DEFENSEOBJECTDEF_VARIABLE_SHIELDSTRENGTH, ShieldStrength);
        csave.WriteMicro(DEFENSEOBJECTDEF_VARIABLE_SHIELDSTRENGTHMAX, ShieldStrengthMax);

        ///TODO: Change this to use ArmorWarheadManager
        //int shield_save_id = ArmorWarheadManager::Get_Armor_Save_ID(ShieldType);
        //WRITE_MICRO_CHUNK(csave, DEFENSEOBJECTDEF_VARIABLE_SHIELDTYPE, shield_save_id);
        csave.WriteMicro(DEFENSEOBJECTDEF_VARIABLE_SHIELDTYPE, ShieldType);
        csave.WriteMicro(DEFENSEOBJECTDEF_VARIABLE_DAMAGE_POINTS, DamagePoints);
        csave.WriteMicro(DEFENSEOBJECTDEF_VARIABLE_DEATH_POINTS, DeathPoints);
        csave.End_Chunk();
        return true;
    }
    public bool Load(ChunkLoadClass cload)
    {
        //TODO: Change this to use ArmorWarheadManager
        //int skin_save_id = -2;
        //int shield_save_id = -2;
        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {
                case DEFENSEOBJECTDEF_CHUNK_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case DEFENSEOBJECTDEF_VARIABLE_HEALTH:
                                cload.Read(ref Health);
                                break;
                            case DEFENSEOBJECTDEF_VARIABLE_HEALTHMAX:
                                cload.Read(ref HealthMax);
                                break;
                            case DEFENSEOBJECTDEF_VARIABLE_SKIN:
                                // TODO: Use ArmorWarheadManager
                                //cload.Read(ref skin_save_id);
                                cload.Read(ref Skin);
                                break;
                            case DEFENSEOBJECTDEF_VARIABLE_SHIELDSTRENGTH:
                                cload.Read(ref ShieldStrength);
                                break;
                            case DEFENSEOBJECTDEF_VARIABLE_SHIELDSTRENGTHMAX:
                                cload.Read(ref ShieldStrengthMax);
                                break;
                            case DEFENSEOBJECTDEF_VARIABLE_SHIELDTYPE:
                                // TODO: Use ArmorWarheadManager
                                //cload.Read(ref shield_save_id);
                                cload.Read(ref ShieldType);
                                break;
                            case DEFENSEOBJECTDEF_VARIABLE_DAMAGE_POINTS:
                                cload.Read(ref DamagePoints);
                                break;
                            case DEFENSEOBJECTDEF_VARIABLE_DEATH_POINTS:
                                cload.Read(ref DeathPoints);
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine($"Unhandled Chunk in DefenseObjectDefClass: {cload.Cur_Chunk_ID}");
                    break;
            }

            cload.Close_Chunk();
        }
        //TODO: Change this to use ArmorWarheadManager
        //Skin = ArmorWarheadManager::Find_Armor_Save_ID(skin_save_id);
        //ShieldType = ArmorWarheadManager::Find_Armor_Save_ID(shield_save_id);
        return true;
    }

    public float Health;
    public float HealthMax;
    public uint Skin;
    public float ShieldStrength;
    public float ShieldStrengthMax;
    public uint ShieldType;
    public float DamagePoints;
    public float DeathPoints;

    private const int DEFENSEOBJECTDEF_CHUNK_VARIABLES = 7311607;
    private const int DEFENSEOBJECTDEF_VARIABLE_HEALTH = 0x00;
    private const int DEFENSEOBJECTDEF_VARIABLE_HEALTHMAX = 0x01;
    private const int DEFENSEOBJECTDEF_VARIABLE_SKIN = 0x02;
    private const int DEFENSEOBJECTDEF_VARIABLE_SHIELDSTRENGTH = 0x03;
    private const int DEFENSEOBJECTDEF_VARIABLE_SHIELDSTRENGTHMAX = 0x04;
    private const int DEFENSEOBJECTDEF_VARIABLE_SHIELDTYPE = 0x05;
    private const int DEFENSEOBJECTDEF_VARIABLE_DAMAGE_POINTS = 0x06;
    private const int DEFENSEOBJECTDEF_VARIABLE_DEATH_POINTS = 0x07;
}
