using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;
using RenData.Transitions;

namespace RenData.GameObjDef;

[RegisterDefinition(ChunkId.CHUNKID_GAME_OBJECT_DEF_TRANSITION)]
public partial class TransitionGameObjDef : BaseGameObjDef
{
    public TransitionGameObjDef()
    {
    }

    ~TransitionGameObjDef()
    {
        Free_Transition_List();
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GAME_OBJECT_DEF_TRANSITION;
    public override PersistClass Create()
    {

        //TransitionGameObj obj = new TransitionGameObj;
        //obj.Init(this);
        //return obj;
        throw new NotImplementedException();
    }
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();


        for (int index = 0; index < Transitions.Count(); index++)
        {
            TransitionDataClass transition = Transitions[index];
            if (transition != null)
            {
                csave.Begin_Chunk(CHUNKID_DEF_TRANSITION);
                transition.Save(csave);
                csave.End_Chunk();
            }
        }

        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        Free_Transition_List();

        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {

                case CHUNKID_DEF_PARENT:
                    base.Load(cload);
                    break;

                case CHUNKID_DEF_TRANSITION:
                    {
                        TransitionDataClass transition = new TransitionDataClass();
                        transition.Load(cload);
                        Transitions.Add(transition);
                    }
                    break;

                default:
                    Console.WriteLine("Unrecognized TransitionDef chunkID\n");
                    break;

            }
            cload.Close_Chunk();
        }

        return true;
    }
    public override PersistFactoryClass Get_Factory() => _persistFactory;

    List<TransitionDataClass> Get_Transition_List() { return Transitions; }
    void Free_Transition_List()
    {
        Transitions.Clear();
    }


    protected List<TransitionDataClass> Transitions = [];


    private const int CHUNKID_DEF_PARENT = 1111991201;
    private const int CHUNKID_DEF_TRANSITION = 1111991202;
};

//DECLARE_DEFINITION_FACTORY(TransitionGameObjDef, CLASSID_GAME_OBJECT_DEF_TRANSITION, "Transition") _TransitionGameObjDefDefFactory;
