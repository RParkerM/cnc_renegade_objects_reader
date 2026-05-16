using RenData.ChunkIO;
using RenData.SaveLoad;
using RenData.IDs;
using RenData.Definitions;

namespace RenData.PhysDef;

[RegisterDefinition(ChunkId.PHYSICS_CHUNKID_DAMAGEABLESTATICPHYSDEF)]
public partial class DamageableStaticPhysDefClass : StaticAnimPhysDefClass
{
    public DamageableStaticPhysDefClass()
    {
        KilledExplosion = 0;
        LiveLoopStart = 0;
        LiveLoopEnd = 0;
        LiveTwitchStart = 0;
        LiveTwitchEnd = 0;
        DeathTransitionStart = 0;
        DeathTransitionEnd = 0;
        DeadLoopStart = 0;
        DeadLoopEnd = 0;
        DeadTwitchStart = 0;
        DeadTwitchEnd = 0;
        PlayTwitchesToCompletion = false;
        DefenseObjectDef = new DefenseObjectDefClass();

        //PARAM_SEPARATOR(DamageableStaticPhysDefClass, "Damage Behavior Controls");

        //EDITABLE_PARAM(DamageableStaticPhysDefClass, ParameterClass.TYPE_EXPLOSIONDEFINITIONID, KilledExplosion);

        //INT_EDITABLE_PARAM(DamageableStaticPhysDefClass, LiveLoopStart, 0, 999);
        //INT_EDITABLE_PARAM(DamageableStaticPhysDefClass, LiveLoopEnd, 0, 999);
        //INT_EDITABLE_PARAM(DamageableStaticPhysDefClass, LiveTwitchStart, 0, 999);
        //INT_EDITABLE_PARAM(DamageableStaticPhysDefClass, LiveTwitchEnd, 0, 999);
        //INT_EDITABLE_PARAM(DamageableStaticPhysDefClass, DeathTransitionStart, 0, 999);
        //INT_EDITABLE_PARAM(DamageableStaticPhysDefClass, DeathTransitionEnd, 0, 999);
        //INT_EDITABLE_PARAM(DamageableStaticPhysDefClass, DeadLoopStart, 0, 999);
        //INT_EDITABLE_PARAM(DamageableStaticPhysDefClass, DeadLoopEnd, 0, 999);
        //INT_EDITABLE_PARAM(DamageableStaticPhysDefClass, DeadTwitchStart, 0, 999);
        //INT_EDITABLE_PARAM(DamageableStaticPhysDefClass, DeadTwitchEnd, 0, 999);
        //EDITABLE_PARAM(DamageableStaticPhysDefClass, ParameterClass.TYPE_BOOL, PlayTwitchesToCompletion);

        //DEFENSEOBJECTDEF_EDITABLE_PARAMS(DamageableStaticPhysDefClass, DefenseObjectDef);
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_DAMAGEABLESTATICPHYSDEF;
    public override string Get_Type_Name() { return "DamageableStaticPhysDef"; }
    public override bool Is_Type(string type_name)
    {
        if (string.Compare(type_name, Get_Type_Name()) == 0)
        {
            return true;
        }
        else
        {
            return base.Is_Type(type_name);
        }
    }
    public override PersistClass Create()
    {
        // DamageableStaticPhysClass obj = new DamageableStaticPhysClass;
        // obj.Init(this);
        // return obj;
        throw new NotImplementedException();
    }

    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(DAMAGEABLESTATICPHYSDEF_CHUNK_STATICANIMPHYSDEF);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(DAMAGEABLESTATICPHYSDEF_CHUNK_VARIABLES);
        csave.WriteMicro(DAMAGEABLESTATICPHYSDEF_VARIABLE_KILLEDEXPLOSION, KilledExplosion);
        csave.WriteMicro(DAMAGEABLESTATICPHYSDEF_VARIABLE_LIVELOOPSTART, LiveLoopStart);
        csave.WriteMicro(DAMAGEABLESTATICPHYSDEF_VARIABLE_LIVELOOPEND, LiveLoopEnd);
        csave.WriteMicro(DAMAGEABLESTATICPHYSDEF_VARIABLE_LIVETWITCHSTART, LiveTwitchStart);
        csave.WriteMicro(DAMAGEABLESTATICPHYSDEF_VARIABLE_LIVETWITCHEND, LiveTwitchEnd);
        csave.WriteMicro(DAMAGEABLESTATICPHYSDEF_VARIABLE_DEATHTRANSITIONSTART, DeathTransitionStart);
        csave.WriteMicro(DAMAGEABLESTATICPHYSDEF_VARIABLE_DEATHTRANSITIONEND, DeathTransitionEnd);
        csave.WriteMicro(DAMAGEABLESTATICPHYSDEF_VARIABLE_DEADLOOPSTART, DeadLoopStart);
        csave.WriteMicro(DAMAGEABLESTATICPHYSDEF_VARIABLE_DEADLOOPEND, DeadLoopEnd);
        csave.WriteMicro(DAMAGEABLESTATICPHYSDEF_VARIABLE_DEADTWITCHSTART, DeadTwitchStart);
        csave.WriteMicro(DAMAGEABLESTATICPHYSDEF_VARIABLE_DEADTWITCHEND, DeadTwitchEnd);
        csave.WriteMicro(DAMAGEABLESTATICPHYSDEF_VARIABLE_PLAYTWITCHESTOCOMPLETION, PlayTwitchesToCompletion);
        csave.End_Chunk();

        csave.Begin_Chunk(DAMAGEABLESTATICPHYSDEF_CHUNK_DEFENSEOBJECTDEF);
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

                case DAMAGEABLESTATICPHYSDEF_CHUNK_STATICANIMPHYSDEF:
                    base.Load(cload);
                    break;

                case DAMAGEABLESTATICPHYSDEF_CHUNK_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case DAMAGEABLESTATICPHYSDEF_VARIABLE_KILLEDEXPLOSION:
                                cload.Read(ref KilledExplosion);
                                break;
                            case DAMAGEABLESTATICPHYSDEF_VARIABLE_LIVELOOPSTART:
                                cload.Read(ref LiveLoopStart);
                                break;
                            case DAMAGEABLESTATICPHYSDEF_VARIABLE_LIVELOOPEND:
                                cload.Read(ref LiveLoopEnd);
                                break;
                            case DAMAGEABLESTATICPHYSDEF_VARIABLE_LIVETWITCHSTART:
                                cload.Read(ref LiveTwitchStart);
                                break;
                            case DAMAGEABLESTATICPHYSDEF_VARIABLE_LIVETWITCHEND:
                                cload.Read(ref LiveTwitchEnd);
                                break;
                            case DAMAGEABLESTATICPHYSDEF_VARIABLE_DEATHTRANSITIONSTART:
                                cload.Read(ref DeathTransitionStart);
                                break;
                            case DAMAGEABLESTATICPHYSDEF_VARIABLE_DEATHTRANSITIONEND:
                                cload.Read(ref DeathTransitionEnd);
                                break;
                            case DAMAGEABLESTATICPHYSDEF_VARIABLE_DEADLOOPSTART:
                                cload.Read(ref DeadLoopStart);
                                break;
                            case DAMAGEABLESTATICPHYSDEF_VARIABLE_DEADLOOPEND:
                                cload.Read(ref DeadLoopEnd);
                                break;
                            case DAMAGEABLESTATICPHYSDEF_VARIABLE_DEADTWITCHSTART:
                                cload.Read(ref DeadTwitchStart);
                                break;
                            case DAMAGEABLESTATICPHYSDEF_VARIABLE_DEADTWITCHEND:
                                cload.Read(ref DeadTwitchEnd);
                                break;
                            case DAMAGEABLESTATICPHYSDEF_VARIABLE_PLAYTWITCHESTOCOMPLETION:
                                cload.Read(ref PlayTwitchesToCompletion);
                                break;
                            default:
                                Console.WriteLine("Unrecognized DamageableStaticPhysDef Variable chunkID: 0x{0:X8} (depth {1}) in {2}", cload.Cur_Micro_Chunk_ID, cload.Cur_Chunk_Depth, nameof(DamageableStaticPhysDefClass));
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                case DAMAGEABLESTATICPHYSDEF_CHUNK_DEFENSEOBJECTDEF:
                    DefenseObjectDef.Load(cload);
                    break;

                default:
                    Console.WriteLine("Unrecognized DamageableStaticPhysDef chunkID: 0x{0:X8} (depth {1}) in {2}", cload.Cur_Chunk_ID, cload.Cur_Chunk_Depth, nameof(DamageableStaticPhysDefClass));
                    break;

            }
            cload.Close_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    //DECLARE_EDITABLE(DamageableStaticPhysDefClass, StaticAnimPhysDefClass);


    protected int KilledExplosion;

    protected int LiveLoopStart;
    protected int LiveLoopEnd;
    protected int LiveTwitchStart;
    protected int LiveTwitchEnd;
    protected int DeathTransitionStart;
    protected int DeathTransitionEnd;
    protected int DeadLoopStart;
    protected int DeadLoopEnd;
    protected int DeadTwitchStart;
    protected int DeadTwitchEnd;
    protected bool PlayTwitchesToCompletion;
    protected DefenseObjectDefClass DefenseObjectDef;

    private const uint DAMAGEABLESTATICPHYSDEF_CHUNK_STATICANIMPHYSDEF = 7311734;
    private const uint DAMAGEABLESTATICPHYSDEF_CHUNK_VARIABLES = 7311735;
    private const uint DAMAGEABLESTATICPHYSDEF_CHUNK_DEFENSEOBJECTDEF = 7311736;

    private const int DAMAGEABLESTATICPHYSDEF_VARIABLE_KILLEDEXPLOSION = 0x00;
    private const int DAMAGEABLESTATICPHYSDEF_VARIABLE_RESETAFTERANIM = 0x01;
    private const int DAMAGEABLESTATICPHYSDEF_VARIABLE_LIVELOOPSTART = 0x02;
    private const int DAMAGEABLESTATICPHYSDEF_VARIABLE_LIVELOOPEND = 0x03;
    private const int DAMAGEABLESTATICPHYSDEF_VARIABLE_LIVETWITCHSTART = 0x04;
    private const int DAMAGEABLESTATICPHYSDEF_VARIABLE_LIVETWITCHEND = 0x05;
    private const int DAMAGEABLESTATICPHYSDEF_VARIABLE_DEATHTRANSITIONSTART = 0x06;
    private const int DAMAGEABLESTATICPHYSDEF_VARIABLE_DEATHTRANSITIONEND = 0x07;
    private const int DAMAGEABLESTATICPHYSDEF_VARIABLE_DEADLOOPSTART = 0x08;
    private const int DAMAGEABLESTATICPHYSDEF_VARIABLE_DEADLOOPEND = 0x09;
    private const int DAMAGEABLESTATICPHYSDEF_VARIABLE_DEADTWITCHSTART = 0x0A;
    private const int DAMAGEABLESTATICPHYSDEF_VARIABLE_DEADTWITCHEND = 0x0B;
    private const int DAMAGEABLESTATICPHYSDEF_VARIABLE_PLAYTWITCHESTOCOMPLETION = 0x0C;
}
