using RenData.ChunkIO;
using RenData.GameObj;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.GameObjDef;

[RegisterDefinition(ChunkId.CHUNKID_GAME_OBJECT_DEF_POWERUP)]
public partial class PowerUpGameObjDef : SimpleGameObjDef
{
    public PowerUpGameObjDef()
    {
        //IsCaptureTheFlag( false ),
        GrantShieldType = 0;
        GrantShieldStrength = 0;
        GrantShieldStrengthMax = 0;
        GrantHealth = 0;
        GrantHealthMax = 0;
        GrantWeaponID = 0;
        GrantWeapon = true;
        GrantWeaponClips = false;
        GrantWeaponRounds = 0;
        Persistent = false;
        GrantKey = 0;
        GrantSoundID = 0;
        IdleSoundID = 0;
        AlwaysAllowGrant = false;
        //{
        //# ifdef PARAM_EDITING_ON
        //            //	EDITABLE_PARAM( PowerUpGameObjDef, ParameterClass::TYPE_INT,	GrantShieldType );
        //            EnumParameterClass param;
        //            param = new EnumParameterClass(GrantShieldType);
        //            param.Set_Name("GrantShieldType");
        //            for (int param_counter = 0; param_counter < ArmorWarheadManager::Get_Num_Armor_Types(); param_counter++)
        //            {
        //                param.Add_Value(ArmorWarheadManager::Get_Armor_Name(param_counter), param_counter);
        //            }
        //            GENERIC_EDITABLE_PARAM(PowerUpGameObjDef, param)


        //        EDITABLE_PARAM(PowerUpGameObjDef, ParameterClass::TYPE_FLOAT, GrantShieldStrength);
        //            EDITABLE_PARAM(PowerUpGameObjDef, ParameterClass::TYPE_FLOAT, GrantShieldStrengthMax);
        //            EDITABLE_PARAM(PowerUpGameObjDef, ParameterClass::TYPE_FLOAT, GrantHealth);
        //            EDITABLE_PARAM(PowerUpGameObjDef, ParameterClass::TYPE_FLOAT, GrantHealthMax);
        //            EDITABLE_PARAM(PowerUpGameObjDef, ParameterClass::TYPE_WEAPONOBJDEFINITIONID, GrantWeaponID);
        //            EDITABLE_PARAM(PowerUpGameObjDef, ParameterClass::TYPE_BOOL, GrantWeapon);
        //            EDITABLE_PARAM(PowerUpGameObjDef, ParameterClass::TYPE_BOOL, GrantWeaponClips);
        //            EDITABLE_PARAM(PowerUpGameObjDef, ParameterClass::TYPE_INT, GrantWeaponRounds);
        //            EDITABLE_PARAM(PowerUpGameObjDef, ParameterClass::TYPE_BOOL, Persistent);
        //            //EDITABLE_PARAM( PowerUpGameObjDef, ParameterClass::TYPE_BOOL,	false );//IsCaptureTheFlag );
        //            EDITABLE_PARAM(PowerUpGameObjDef, ParameterClass::TYPE_INT, GrantKey);
        //            EDITABLE_PARAM(PowerUpGameObjDef, ParameterClass::TYPE_BOOL, AlwaysAllowGrant);

        //            EDITABLE_PARAM(PowerUpGameObjDef, ParameterClass::TYPE_STRING, GrantAnimationName);
        //            EDITABLE_PARAM(PowerUpGameObjDef, ParameterClass::TYPE_STRING, IdleAnimationName);
        //            EDITABLE_PARAM(PowerUpGameObjDef, ParameterClass::TYPE_SOUNDDEFINITIONID, GrantSoundID);
        //            EDITABLE_PARAM(PowerUpGameObjDef, ParameterClass::TYPE_SOUNDDEFINITIONID, IdleSoundID);
    }
    public override uint Get_Class_ID() => ClassId.CLASSID_GAME_OBJECT_DEF_POWERUP;
    public override PersistClass Create()
    {
        //PowerUpGameObj obj = new PowerUpGameObj;
        //obj.Init(this);
        //return obj;
        throw new NotImplementedException();
    }
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        csave.WriteMicro(MICROCHUNKID_DEF_GRANT_SHIELD_TYPE, GrantShieldType);
        csave.WriteMicro(MICROCHUNKID_DEF_GRANT_SHIELD_STRENGTH, GrantShieldStrength);
        csave.WriteMicro(MICROCHUNKID_DEF_GRANT_SHIELD_STRENGTH_MAX, GrantShieldStrengthMax);
        csave.WriteMicro(MICROCHUNKID_DEF_GRANT_HEALTH, GrantHealth);
        csave.WriteMicro(MICROCHUNKID_DEF_GRANT_HEALTH_MAX, GrantHealthMax);
        csave.WriteMicro(MICROCHUNKID_DEF_GRANT_WEAPON_ID, GrantWeaponID);
        csave.WriteMicro(MICROCHUNKID_DEF_GRANT_WEAPON, GrantWeapon);
        csave.WriteMicro(MICROCHUNKID_DEF_GRANT_WEAPON_CLIPS, GrantWeaponClips);
        csave.WriteMicro(MICROCHUNKID_DEF_GRANT_WEAPON_ROUNDS, GrantWeaponRounds);
        csave.WriteMicro(MICROCHUNKID_DEF_PERSISTENT, Persistent);
        //WRITE_MICRO_CHUNK( csave, XXXMICROCHUNKID_DEF_IS_CAPTURE_THE_FLAG,		IsCaptureTheFlag );
        csave.WriteMicro(MICROCHUNKID_DEF_GRANT_KEY, GrantKey);
        csave.WriteMicro(MICROCHUNKID_DEF_GRANT_SOUNDID, GrantSoundID);
        csave.WriteMicro(MICROCHUNKID_DEF_IDLE_SOUNDID, IdleSoundID);
        ArgumentNullException.ThrowIfNull(GrantAnimationName);
        csave.WriteMicroString(MICROCHUNKID_DEF_GRANT_ANIMATION_NAME, GrantAnimationName);
        ArgumentNullException.ThrowIfNull(IdleAnimationName);
        csave.WriteMicroString(MICROCHUNKID_DEF_IDLE_ANIMATION_NAME, IdleAnimationName);
        csave.WriteMicro(MICROCHUNKID_DEF_ALWAYS_ALLOW_GRANT, AlwaysAllowGrant);
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
                            case MICROCHUNKID_DEF_GRANT_SHIELD_TYPE:
                                cload.Read(ref GrantShieldType);
                                break;
                            case MICROCHUNKID_DEF_GRANT_SHIELD_STRENGTH:
                                cload.Read(ref GrantShieldStrength);
                                break;
                            case MICROCHUNKID_DEF_GRANT_SHIELD_STRENGTH_MAX:
                                cload.Read(ref GrantShieldStrengthMax);
                                break;
                            case MICROCHUNKID_DEF_GRANT_HEALTH:
                                cload.Read(ref GrantHealth);
                                break;
                            case MICROCHUNKID_DEF_GRANT_HEALTH_MAX:
                                cload.Read(ref GrantHealthMax);
                                break;
                            case MICROCHUNKID_DEF_GRANT_WEAPON_ID:
                                cload.Read(ref GrantWeaponID);
                                break;
                            case MICROCHUNKID_DEF_GRANT_WEAPON:
                                cload.Read(ref GrantWeapon);
                                break;
                            case MICROCHUNKID_DEF_GRANT_WEAPON_CLIPS:
                                cload.Read(ref GrantWeaponClips);
                                break;
                            case MICROCHUNKID_DEF_GRANT_WEAPON_ROUNDS:
                                cload.Read(ref GrantWeaponRounds);
                                break;
                            case MICROCHUNKID_DEF_PERSISTENT:
                                cload.Read(ref Persistent);
                                break;
                            //READ_MICRO_CHUNK( cload, XXXMICROCHUNKID_DEF_IS_CAPTURE_THE_FLAG,			IsCaptureTheFlag );
                            case MICROCHUNKID_DEF_GRANT_KEY:
                                cload.Read(ref GrantKey);
                                break;

                            case MICROCHUNKID_DEF_GRANT_SOUNDID:
                                cload.Read(ref GrantSoundID);
                                break;
                            case MICROCHUNKID_DEF_IDLE_SOUNDID:
                                cload.Read(ref IdleSoundID);
                                break;
                            case MICROCHUNKID_DEF_GRANT_ANIMATION_NAME:
                                cload.ReadMicroChunkWWString(out GrantAnimationName);
                                break;
                            case MICROCHUNKID_DEF_IDLE_ANIMATION_NAME:
                                cload.ReadMicroChunkWWString(out IdleAnimationName);
                                break;
                            case MICROCHUNKID_DEF_ALWAYS_ALLOW_GRANT:
                                cload.Read(ref AlwaysAllowGrant);
                                break;

                            default:
                                Console.WriteLine("Unhandled Micro Chunk:{cload.Cur_Micro_Chunk_ID()}");
                                break;

                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unhandled Chunk:{cload.Cur_Chunk_ID()}");
                    break;

            }
            cload.Close_Chunk();
        }

        return true;
    }
    public override PersistFactoryClass Get_Factory() => _persistFactory;

    //DECLARE_EDITABLE(PowerUpGameObjDef, SimpleGameObjDef);

    // Grant returns true if anything was granted that the grantee didn't already have
    public bool Grant(SmartGameObj obj, PowerUpGameObj? p_powerup = null, bool hud_display = true)
    {


        //        bool PowerUpGameObjDef::Grant(SmartGameObj obj, PowerUpGameObj p_powerup, bool hud_display)
        //{
        //            int no_grant_message = 0;

        //            bool granted = false;

        //            WWASSERT(CombatManager::I_Am_Server());

        //            DefenseObjectClass defense = obj.Get_Defense_Object();
        //            // Grant the shield
        //            if (GrantShieldType != 0)
        //            {
        //                if (GrantShieldType > (int)defense.Get_Shield_Type())
        //                {
        //                    defense.Set_Shield_Type(GrantShieldType);
        //                    granted = true;
        //                }
        //                else
        //                {
        //                    no_grant_message = IDS_M00DSGN_DSGN1015I1DSGN_TXT; //"You are already at full shield."
        //                }
        //            }

        //            if (GrantShieldStrengthMax != 0)
        //            {
        //                float add = GrantShieldStrengthMax(float)obj.Get_Definition().Get_DefenseObjectDef().ShieldStrengthMax;

        //                switch (CombatManager::Get_Difficulty_Level())
        //                {
        //                    case 0: add = 2.0f; break;
        //                    case 2: add = 0.75f; break;
        //                }
        //                ;

        //                // Round up to next int
        //                add = (int)(add + 0.95f);

        //                defense.Set_Shield_Strength_Max(defense.Get_Shield_Strength_Max() + add);
        //                granted = true;

        //                if (hud_display  obj == COMBAT_STAR) {
        //                    HUDClass::Add_Shield_Upgrade_Grant(add);
        //                }

        //            }

        //            if (GrantShieldStrength != 0)
        //            {
        //                if ((defense.Get_Shield_Strength() < defense.Get_Shield_Strength_Max()))
        //                {
        //                    defense.Add_Shield_Strength(GrantShieldStrength);
        //                    granted = true;

        //                    if (obj == COMBAT_STAR)
        //                    {
        //                        Vector3 pos;
        //                        obj.Get_Position(pos);
        //                        DIAG_LOG(("AMPU", "%1.2f; %1.2f; %1.2f; %1.2f; %1.2f", pos.X, pos.Y, pos.Z, defense.Get_Shield_Strength(), defense.Get_Health()));
        //                    }
        //                }
        //                else
        //                {
        //                    no_grant_message = IDS_M00DSGN_DSGN1015I1DSGN_TXT; //"You are already at full shield."
        //                }
        //            }

        //            if (granted  hud_display) {
        //                if (obj == COMBAT_STAR)
        //                {
        //                    if (GrantShieldStrengthMax == 0)
        //                    {
        //                        HUDClass::Add_Shield_Grant(GrantShieldStrength);
        //                    }
        //                }
        //            }

        //            // Grant the Health
        //            if (GrantHealthMax != 0)
        //            {
        //                float add = GrantHealthMax(float)obj.Get_Definition().Get_DefenseObjectDef().HealthMax;

        //                switch (CombatManager::Get_Difficulty_Level())
        //                {
        //                    case 0: add = 2.0f; break;
        //                    case 2: add = 0.75f; break;
        //                }
        //                ;

        //                // Round up to next int
        //                add = (int)(add + 0.95f);

        //                defense.Set_Health_Max(defense.Get_Health_Max() + add);
        //                granted = true;

        //                if (hud_display  obj == COMBAT_STAR) {
        //                    HUDClass::Add_Health_Upgrade_Grant(add);
        //                }
        //            }

        //            if (GrantHealth != 0)
        //            {
        //                if (defense.Get_Health() < defense.Get_Health_Max())
        //                {
        //                    defense.Add_Health(GrantHealth);
        //                    granted = true;

        //                    if (obj == COMBAT_STAR  hud_display) {
        //                        if (GrantHealthMax == 0)
        //                        {
        //                            HUDClass::Add_Health_Grant(GrantHealth);
        //                        }
        //                    }

        //                    if (obj == COMBAT_STAR)
        //                    {
        //                        Vector3 pos;
        //                        obj.Get_Position(pos);
        //                        DIAG_LOG(("HEPU", "%1.2f; %1.2f; %1.2f; %1.2f; %1.2f", pos.X, pos.Y, pos.Z, defense.Get_Shield_Strength(), defense.Get_Health()));
        //                    }
        //                }
        //                else
        //                {
        //                    no_grant_message = IDS_M00DSGN_DSGN1014I1DSGN_TXT; //"You are already at full health."
        //                }
        //            }

        //            // Grant the Weapon
        //            if (GrantWeaponID != 0)
        //            {

        //                if ((GrantWeapon!obj.Get_Weapon_Bag().Is_Weapon_Owned(GrantWeaponID)) ||
        //                    (!obj.Get_Weapon_Bag().Is_Ammo_Full(GrantWeaponID))) {

        //                    if (obj == COMBAT_STAR  hud_display) {
        //                        if (GrantWeapon!obj.Get_Weapon_Bag().Is_Weapon_Owned(GrantWeaponID)) {
        //                            HUDClass::Add_Powerup_Weapon(GrantWeaponID, GrantWeaponRounds);
        //                        }

        //                else
        //                        {
        //                            if (!obj.Get_Weapon_Bag().Is_Ammo_Full(GrantWeaponID))
        //                            {
        //                                HUDClass::Add_Powerup_Ammo(GrantWeaponID, GrantWeaponRounds);
        //                            }
        //                        }
        //                    }

        //                    obj.Get_Weapon_Bag().Add_Weapon(GrantWeaponID, GrantWeaponRounds, GrantWeapon);
        //                    granted = true;

        //                    if (obj == COMBAT_STAR)
        //                    {
        //                        Vector3 pos;
        //                        obj.Get_Position(pos);
        //                        char grant_name = "";
        //                        WeaponDefinitionClass def = WeaponManager::Find_Weapon_Definition(GrantWeaponID);
        //                        if (def)
        //                        {
        //                            grant_name = def.Get_Name();
        //                        }
        //                        char weapon_name = "";
        //                        int ammo = 0;
        //                        if (obj.Get_Weapon())
        //                        {
        //                            weapon_name = obj.Get_Weapon().Get_Definition().Get_Name();
        //                            ammo = obj.Get_Weapon().Get_Total_Rounds();
        //                        }
        //                        DIAG_LOG(("WEPU", "%1.2f; %1.2f; %1.2f; %s; %d; %s; %d", pos.X, pos.Y, pos.Z, grant_name, GrantWeaponRounds, weapon_name, ammo));
        //                    }
        //                }

        //        else
        //                {
        //                    no_grant_message = IDS_M00DSGN_DSGN1016I1DSGN_TXT; //"Your weapon is full."
        //                }

        //            }
        //            else if (GrantWeaponClips)
        //            {

        //                //
        //                //	Loop over all the weapons in the owner's bag
        //                //
        //                WeaponBagClass weapon_bag = obj.Get_Weapon_Bag();
        //                for (int weapon_index = 0; weapon_index < weapon_bag.Get_Count(); weapon_index++)
        //                {
        //                    WeaponClass weapon = weapon_bag.Peek_Weapon(weapon_index);
        //                    if (weapon != null  weapon.Get_Definition().CanReceiveGenericCnCAmmo) {

        //                    //
        //                    //	Grant "x" number of clips to the weapon
        //                    //
        //                    int clip_rounds = weapon.Get_Definition().ClipSize;
        //                    weapon.Add_Rounds(clip_rounds  GrantWeaponRounds);
        //                }
        //            }
        //        }


        //        // Grant the key
        //        if (GrantKey != 0)
        //        {
        //            SoldierGameObj soldier = obj.As_SoldierGameObj();
        //            if (soldier soldier.Is_Human_Controlled()) {
        //                if (!soldier.Has_Key(GrantKey))
        //                {
        //                    soldier.Give_Key(GrantKey);
        //                    granted = true;
        //                }
        //            }
        //            if (obj == COMBAT_STAR)
        //            {
        //                Vector3 pos;
        //                obj.Get_Position(pos);
        //                DIAG_LOG(("KEPU", "%1.2f; %1.2f; %1.2f; %d", pos.X, pos.Y, pos.Z, GrantKey));
        //            }

        //            if (granted  hud_display obj == COMBAT_STAR) {
        //                HUDClass::Add_Key_Grant(GrantKey);
        //            }
        //        }


        //    /
        //    // Handle Capture the Flag
        //    if (IsCaptureTheFlag  p_powerup != null  obj.As_SoldierGameObj() != null ) {
        //            CombatManager::Soldier_Contacts_Flag(obj.As_SoldierGameObj(), p_powerup);
        //            granted = true;
        //        }

        //    /


        //    if (AlwaysAllowGrant)
        //        {
        //            granted = true;
        //        }

        //        if (granted  p_powerup != null) {
        //            p_powerup.Set_State(PowerUpGameObj::STATE_GRANTING);

        //            //
        //            //	Reveal this object to the player
        //            //
        //            if (COMBAT_STAR == obj)
        //            {
        //                EncyclopediaMgrClass::Reveal_Object(p_powerup);
        //            }
        //        }

        //        // Stats
        //        if (granted  obj.Get_Player_Data()) {
        //            obj.Get_Player_Data().Stats_Add_Powerup();
        //        }

        //        if (!granted(COMBAT_STAR == obj)  no_grant_message != 0) {
        //            HUDInfo::Set_HUD_Help_Text(TRANSLATE(no_grant_message), Vector3(0, 1, 0));

        //        }

        //        return granted;
        //    }

        throw new NotImplementedException();
    }

    public int Get_Grant_Weapon_ID() { return GrantWeaponID; }

    protected int GrantShieldType;
    protected float GrantShieldStrength;
    protected float GrantShieldStrengthMax;
    protected float GrantHealth;
    protected float GrantHealthMax;
    protected int GrantWeaponID;
    protected bool GrantWeapon;
    protected int GrantWeaponRounds;
    protected bool GrantWeaponClips;
    protected bool Persistent;
    //protected bool											IsCaptureTheFlag;
    protected int GrantKey;
    protected bool AlwaysAllowGrant;

    protected int GrantSoundID;
    protected string? GrantAnimationName;
    protected int IdleSoundID;
    protected string? IdleAnimationName;

    private const uint CHUNKID_DEF_PARENT = 909991656;
    private const uint CHUNKID_DEF_VARIABLES = 909991657;

    private const uint XXXMICROCHUNKID_DEF_PARAMETERS = 1;
    private const uint MICROCHUNKID_DEF_PERSISTENT = 2;
    private const uint MICROCHUNKID_DEF_GRANT_SHIELD_TYPE = 3;
    private const uint MICROCHUNKID_DEF_GRANT_SHIELD_STRENGTH = 4;
    private const uint XXXMICROCHUNKID_DEF_GRANT_SHIELD_STRENGTH_MAX = 5;
    private const uint MICROCHUNKID_DEF_GRANT_HEALTH = 6;
    private const uint XXXMICROCHUNKID_DEF_GRANT_HEALTH_MAX = 7;
    private const uint MICROCHUNKID_DEF_GRANT_WEAPON_ID = 8;
    private const uint MICROCHUNKID_DEF_GRANT_WEAPON = 9;
    private const uint MICROCHUNKID_DEF_GRANT_WEAPON_ROUNDS = 10;
    private const uint XXXMICROCHUNKID_DEF_IS_CAPTURE_THE_FLAG = 11;
    private const uint XXXMICROCHUNKID_DEF_GRANT_KEY_MASK = 12;

    private const uint MICROCHUNKID_DEF_GRANT_ANIMATION_NAME = 13;
    private const uint MICROCHUNKID_DEF_GRANT_SOUNDID = 14;
    private const uint MICROCHUNKID_DEF_IDLE_ANIMATION_NAME = 15;
    private const uint MICROCHUNKID_DEF_IDLE_SOUNDID = 16;
    private const uint MICROCHUNKID_DEF_GRANT_KEY = 17;
    private const uint MICROCHUNKID_DEF_ALWAYS_ALLOW_GRANT = 18;
    private const uint MICROCHUNKID_DEF_GRANT_WEAPON_CLIPS = 19;

    private const uint MICROCHUNKID_DEF_GRANT_SHIELD_STRENGTH_MAX = 20;
    private const uint MICROCHUNKID_DEF_GRANT_HEALTH_MAX = 21;

};

//SimplePersistFactoryClass<PowerUpGameObjDef, CHUNKID_GAME_OBJECT_DEF_POWERUP> _PowerUpGameObjDefPersistFactory;
//DECLARE_DEFINITION_FACTORY(PowerUpGameObjDef, CLASSID_GAME_OBJECT_DEF_POWERUP, "PowerUp") _PowerUpGameObjDefDefFactory;



