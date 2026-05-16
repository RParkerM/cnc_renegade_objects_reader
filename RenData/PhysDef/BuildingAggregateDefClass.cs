using RenData.ChunkIO;
using RenData.SaveLoad;
using RenData.IDs;
using System.Diagnostics;

namespace RenData.PhysDef;

[RegisterDefinition(ChunkId.PHYSICS_CHUNKID_BUILDINGAGGREGATEDEF)]
public partial class BuildingAggregateDefClass : StaticAnimPhysDefClass
{
    public BuildingAggregateDefClass()
    {
        AnimLogicMode = ANIM_LOGIC_LOOP;
        IsMCT = false;

        Frame0 = new int[BuildingStateClass.STATE_COUNT];
        Frame1 = new int[BuildingStateClass.STATE_COUNT];
        AnimationEnabled = new bool[BuildingStateClass.STATE_COUNT];

        for (int i = 0; i < BuildingStateClass.STATE_COUNT; i++)
        {
            Frame0[i] = 0;
            Frame1[i] = 0;
            AnimationEnabled[i] = true;
        }

        //# ifdef PARAM_EDITING_ON
        //    PARAM_SEPARATOR(BuildingAggregateDefClass, "Building Behavior Settings");

        //    EnumParameterClass anim_logic_param = new EnumParameterClass(AnimLogicMode);
        //    anim_logic_param.Set_Name("AnimLogicMode");
        //    anim_logic_param.Add_Value("ANIM_LOGIC_LINEAR", ANIM_LOGIC_LINEAR);
        //    anim_logic_param.Add_Value("ANIM_LOGIC_LOOP", ANIM_LOGIC_LOOP);
        //    anim_logic_param.Add_Value("ANIM_LOGIC_SEQUENCE", ANIM_LOGIC_SEQUENCE);
        //    GENERIC_EDITABLE_PARAM(BuildingAggregateDefClass, anim_logic_param);

        //    //EDITABLE_PARAM(BuildingAggregateDefClass, ParameterClass.TYPE_BOOL, IsMCT);

        //    for (i = 0; i < BuildingStateClass.STATE_COUNT; i++)
        //    {
        //        NAMED_EDITABLE_PARAM(BuildingAggregateDefClass, ParameterClass.TYPE_INT, Frame0[i], "Frame0");
        //        NAMED_EDITABLE_PARAM(BuildingAggregateDefClass, ParameterClass.TYPE_INT, Frame1[i], "Frame1");
        //        NAMED_EDITABLE_PARAM(BuildingAggregateDefClass, ParameterClass.TYPE_BOOL, AnimationEnabled[i], "AnimationEnabled");
        //    }
        //#endif
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_BUILDINGAGGREGATEDEF;
    public override string Get_Type_Name() { return "BuildingAggregateDef"; }
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
        // BuildingAggregateClass obj = new BuildingAggregateClass;
        // obj.Init(this);
        // return obj;
        throw new NotImplementedException();
    }
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(BAGDEF_CHUNK_STATICANIMPHYS);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(BAGDEF_CHUNK_VARIABLES);
        csave.WriteMicro(BAGDEF_VARIABLE_ANIMLOGICMODE, AnimLogicMode);
        csave.WriteMicro(BAGDEF_VARIABLE_ISMCT, IsMCT);
        csave.End_Chunk();

        for (int i = 0; i < BuildingStateClass.STATE_COUNT; i++)
        {
            Save_State_Animation_Data(csave, i);
        }

        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {

                case BAGDEF_CHUNK_STATICANIMPHYS:
                    base.Load(cload);
                    break;

                case BAGDEF_CHUNK_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case BAGDEF_VARIABLE_ANIMLOGICMODE:
                                cload.Read(ref AnimLogicMode);
                                break;
                            case BAGDEF_VARIABLE_ISMCT:
                                cload.Read(ref IsMCT);
                                break;
                            default:
                                Console.WriteLine("Unrecognized BuildingAggregateDef Variable chunkID: 0x{0:X8} (depth {1}) in {2}", cload.Cur_Chunk_ID, cload.Cur_Chunk_Depth, nameof(BuildingAggregateDefClass));
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                case BAGDEF_CHUNK_HEALTH100_POWERON_VARIABLES:
                case BAGDEF_CHUNK_HEALTH75_POWERON_VARIABLES:
                case BAGDEF_CHUNK_HEALTH50_POWERON_VARIABLES:
                case BAGDEF_CHUNK_HEALTH25_POWERON_VARIABLES:
                case BAGDEF_CHUNK_DESTROYED_POWERON_VARIABLES:
                case BAGDEF_CHUNK_HEALTH100_POWEROFF_VARIABLES:
                case BAGDEF_CHUNK_HEALTH75_POWEROFF_VARIABLES:
                case BAGDEF_CHUNK_HEALTH50_POWEROFF_VARIABLES:
                case BAGDEF_CHUNK_HEALTH25_POWEROFF_VARIABLES:
                case BAGDEF_CHUNK_DESTROYED_POWEROFF_VARIABLES:
                    Load_State_Animation_Data(cload, (int)(cload.Cur_Chunk_ID - BAGDEF_CHUNK_HEALTH100_POWERON_VARIABLES));
                    break;

                default:
                    Console.WriteLine("Unrecognized BuildingAggregateDef chunkID: 0x{0:X8} (depth {1}) in {2}", cload.Cur_Chunk_ID, cload.Cur_Chunk_Depth, nameof(BuildingAggregateDefClass));
                    break;

            }
            cload.Close_Chunk();
        }

        return true;
    }
    public override PersistFactoryClass Get_Factory() => _persistFactory;

    //DECLARE_EDITABLE(BuildingAggregateDefClass, StaticAnimPhysDefClass);


    protected bool Save_State_Animation_Data(ChunkSaveClass csave, int state_index)
    {
        Debug.Assert(state_index >= 0);
        Debug.Assert(state_index < BuildingStateClass.STATE_COUNT);

        csave.Begin_Chunk(BAGDEF_CHUNK_HEALTH100_POWERON_VARIABLES + (uint)state_index);
        csave.WriteMicro(BAGDEF_VARIABLE_FRAME0, Frame0[state_index]);
        csave.WriteMicro(BAGDEF_VARIABLE_FRAME1, Frame1[state_index]);
        csave.WriteMicro(BAGDEF_VARIABLE_ANIMATIONENABLED, AnimationEnabled[state_index]);
        csave.End_Chunk();
        return true;
    }

    protected bool Load_State_Animation_Data(ChunkLoadClass cload, int state_index)
    {
        Debug.Assert(state_index >= 0);
        Debug.Assert(state_index < BuildingStateClass.STATE_COUNT);

        while (cload.Open_Micro_Chunk())
        {
            switch (cload.Cur_Micro_Chunk_ID)
            {
                case BAGDEF_VARIABLE_FRAME0:
                    cload.Read(ref Frame0[state_index]);
                    break;
                case BAGDEF_VARIABLE_FRAME1:
                    cload.Read(ref Frame1[state_index]);
                    break;
                case BAGDEF_VARIABLE_ANIMATIONENABLED:
                    cload.Read(ref AnimationEnabled[state_index]);
                    break;
                default:
                    Console.WriteLine("Unrecognized BuildingAggregateDef Variable chunkID: 0x{0:X8} (depth {1}) in {2}", cload.Cur_Chunk_ID, cload.Cur_Chunk_Depth, nameof(BuildingAggregateDefClass));
                    break;
            }
            cload.Close_Micro_Chunk();
        }
        return true;
    }



    protected const int ANIM_LOGIC_LINEAR = 0;
    protected const int ANIM_LOGIC_LOOP = 1;
    protected const int ANIM_LOGIC_SEQUENCE = 2;

    protected int AnimLogicMode;
    protected bool IsMCT;

    protected int[] Frame0;
    protected int[] Frame1;
    protected bool[] AnimationEnabled;

    private const uint BAGDEF_CHUNK_STATICANIMPHYS = 8281441u;
    private const uint BAGDEF_CHUNK_VARIABLES = 8281442u;

    private const uint BAGDEF_CHUNK_HEALTH100_POWERON_VARIABLES = 8281443u;
    private const uint BAGDEF_CHUNK_HEALTH75_POWERON_VARIABLES = 8281444u;
    private const uint BAGDEF_CHUNK_HEALTH50_POWERON_VARIABLES = 8281445u;
    private const uint BAGDEF_CHUNK_HEALTH25_POWERON_VARIABLES = 8281446u;
    private const uint BAGDEF_CHUNK_DESTROYED_POWERON_VARIABLES = 8281447u;
    private const uint BAGDEF_CHUNK_HEALTH100_POWEROFF_VARIABLES = 8281448u;
    private const uint BAGDEF_CHUNK_HEALTH75_POWEROFF_VARIABLES = 8281449u;
    private const uint BAGDEF_CHUNK_HEALTH50_POWEROFF_VARIABLES = 8281450u;
    private const uint BAGDEF_CHUNK_HEALTH25_POWEROFF_VARIABLES = 8281451u;
    private const uint BAGDEF_CHUNK_DESTROYED_POWEROFF_VARIABLES = 8281452u;

    private const int BAGDEF_VARIABLE_ANIMLOGICMODE = 0;
    private const int BAGDEF_VARIABLE_FRAME0 = 1;
    private const int BAGDEF_VARIABLE_FRAME1 = 2;
    private const int BAGDEF_VARIABLE_ANIMATIONENABLED = 3;
    private const int BAGDEF_VARIABLE_ISMCT = 4;
}
