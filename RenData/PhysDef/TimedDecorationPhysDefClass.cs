using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.PhysDef;

[RegisterDefinition(ChunkId.PHYSICS_CHUNKID_TIMEDDECOPHYSDEF)]
public partial class TimedDecorationPhysDefClass : DecorationPhysDefClass
{
    public TimedDecorationPhysDefClass()
    {
        Lifetime = 2.0f;
        //# ifdef PARAM_EDITING_ON
        // make our parameters editable!
        //FLOAT_EDITABLE_PARAM(TimedDecorationPhysDefClass, Lifetime, 0.01f, 100.0f);
        //#endif
    }

    // From DefinitionClass
    public override uint Get_Class_ID() => ClassId.CLASSID_TIMEDDECOPHYSDEF;

    public override PersistClass Create()
    {
        throw new NotImplementedException();
        //TimedDecorationPhysClass new_obj = NEW_REF(TimedDecorationPhysClass, ());
        //new_obj.Init(this);
        //return new_obj;
    }

    // From PhysDefClass
    public override string Get_Type_Name() => "TimedDecorationPhysDef";
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

    // From PersistClass
    public override PersistFactoryClass Get_Factory() => _persistFactory;
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(TIMEDDECORATIONPHYSDEF_CHUNK_DECORATIONPHYSDEF);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(TIMEDDECORATIONPHYSDEF_CHUNK_VARIABLES);
        csave.WriteMicro(TIMEDDECORATIONPHYSDEF_VARIABLE_LIFETIME, Lifetime);
        csave.End_Chunk();

        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {

            switch (cload.Cur_Chunk_ID)
            {

                case TIMEDDECORATIONPHYSDEF_CHUNK_DECORATIONPHYSDEF:
                    base.Load(cload);
                    break;

                case TIMEDDECORATIONPHYSDEF_CHUNK_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case TIMEDDECORATIONPHYSDEF_VARIABLE_LIFETIME:
                                cload.Read(ref Lifetime);
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    System.Console.WriteLine("Unhandled Chunk: 0x{0:X8} (depth {1}) in {2}", cload.Cur_Chunk_ID, cload.Cur_Chunk_Depth, nameof(TimedDecorationPhysDefClass));
                    break;
            }

            cload.Close_Chunk();
        }
        return true;
    }

    // accessors
    float Get_Lifetime() { return Lifetime; }

    //	Editable interface requirements
    //DECLARE_EDITABLE(TimedDecorationPhysDefClass, DecorationPhysDefClass);


    protected float Lifetime;

    private const int TIMEDDECORATIONPHYSDEF_CHUNK_DECORATIONPHYSDEF = 0x01170003;          // (parent class)
    private const int TIMEDDECORATIONPHYSDEF_CHUNK_VARIABLES = 0x01170004;

    private const int TIMEDDECORATIONPHYSDEF_VARIABLE_LIFETIME = 0x00;
};

