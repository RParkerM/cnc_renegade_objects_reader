using RenData.ChunkIO;

namespace RenData.Dialogue;

public class DialogueClass
{
    public DialogueClass()
    {
    }
    public DialogueClass(DialogueClass src)
    {
        SilenceWeight = src.SilenceWeight;
        
        for (int index = 0; index < src.OptionList.Count; index++)
        {
            DialogueOptionClass option = new(src.OptionList[index]);
            OptionList.Add(option);
        }
    }
    ~DialogueClass()
    {
        Free_Options();
    }

    public List<DialogueOptionClass> Get_Option_List() { return OptionList; }
    public void Free_Options()
    {
        OptionList.Clear();
    }

    public float Get_Silence_Weight() { return SilenceWeight; }
    public void Set_Silence_Weight(float weight) { SilenceWeight = weight; }

    public int Get_Conversation()
    {
        int conv_id = 0;

        //
        //	Make a number we can use to index linearly into the option list 
        // to determine which one to use.
        //
        float total = SilenceWeight;
        for (int index = 0; index < OptionList.Count; index++)
        {
            total += OptionList[index].Get_Weight();
        }

        //
        //	Choose a random value in this linear range
        //
        float value = WWMath.Random_Float(0, total);

        //
        //	Now find the object this value corresponds to
        //
        float count = SilenceWeight;
        for (int index = 0; value > count && index < OptionList.Count; index++)
        {
            conv_id = OptionList[index].Get_Conversation_ID();
            count += OptionList[index].Get_Weight();
        }

        return conv_id;
    }

    //
    //	Save/load
    //
    public void Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DIALOGUE_VARIABLES);
        csave.WriteMicro(VARID_DIALOGUE_SILENCE, SilenceWeight);
        csave.End_Chunk();

        //
        //	Save the options
        //
        for (int index = 0; index < OptionList.Count; index++)
        {
            csave.Begin_Chunk(CHUNKID_DIALOGUE_OPTION);
            OptionList[index].Save(csave);
            csave.End_Chunk();
        }

        return;
    }
    public void Load(ChunkLoadClass cload)
    {
        Free_Options();

        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {

                case CHUNKID_DIALOGUE_VARIABLES:
                    Load_Variables(cload);
                    break;

                case CHUNKID_DIALOGUE_OPTION:
                    {
                        //
                        //	Create a new option object and add it to the list
                        //
                        DialogueOptionClass option = new();
                        option.Load(cload);
                        OptionList.Add(option);
                    }
                    break;
            }

            cload.Close_Chunk();
        }

        return;
    }


    public void Load_Variables(ChunkLoadClass cload)
    {
        while (cload.Open_Micro_Chunk())
        {
            switch (cload.Cur_Micro_Chunk_ID)
            {

                case VARID_DIALOGUE_SILENCE:
                    cload.Read(ref SilenceWeight);
                    break;
            }

            cload.Close_Micro_Chunk();
        }

        return;
    }

    protected List<DialogueOptionClass> OptionList = [];
    protected float SilenceWeight = 1;

    private const int CHUNKID_DIALOGUE_VARIABLES = 0x08040529;
    private const int CHUNKID_DIALOGUE_OPTION = 0x0804052A;

    private const int VARID_DIALOGUE_SILENCE = 0;

    public const int DIALOG_ON_TAKE_DAMAGE_FROM_FRIEND = 0;
    public const int DIALOG_ON_TAKE_DAMAGE_FROM_ENEMY = 1;
    public const int DIALOG_ON_DAMAGE_FRIEND = 2;
    public const int DIALOG_ON_DAMAGE_ENEMY = 3;
    public const int DIALOG_ON_KILLED_FRIEND = 4;
    public const int DIALOG_ON_KILLED_ENEMY = 5;
    public const int DIALOG_ON_SAW_FRIEND = 6;
    public const int DIALOG_ON_SAW_ENEMY = 7;
    public const int DIALOG_ON_OBSOLETE_01 = 8;
    public const int DIALOG_ON_OBSOLETE_02 = 9;
    public const int DIALOG_ON_DIE = 10;
    public const int DIALOG_ON_POKE_IDLE = 11;
    public const int DIALOG_ON_POKE_SEARCH = 12;
    public const int DIALOG_ON_POKE_COMBAT = 13;

    public const int DIALOG_STATE_FROM_IDLE_TO_COMBAT = 14;
    public const int DIALOG_STATE_FROM_IDLE_TO_SEARCH = 15;
    public const int DIALOG_STATE_FROM_SEARCH_TO_COMBAT = 16;
    public const int DIALOG_STATE_FROM_SEARCH_TO_IDLE = 17;
    public const int DIALOG_STATE_FROM_COMBAT_TO_SEARCH = 18;
    public const int DIALOG_STATE_FROM_COMBAT_TO_IDLE = 19;

    public const int DIALOG_MAX = 20;

    // Event names in a C# dictionary for easy lookup
    public static readonly Dictionary<int, string> DIALOG_EVENT_NAMES =
        new()
        {
        { DIALOG_ON_TAKE_DAMAGE_FROM_FRIEND, "TAKE_DAMAGE_FROM_FRIEND" },
        { DIALOG_ON_TAKE_DAMAGE_FROM_ENEMY, "TAKE_DAMAGE_FROM_ENEMY" },
        { DIALOG_ON_DAMAGE_FRIEND, "DAMAGE_FRIEND" },
        { DIALOG_ON_DAMAGE_ENEMY, "DAMAGE_ENEMY" },
        { DIALOG_ON_KILLED_FRIEND, "KILLED_FRIEND" },
        { DIALOG_ON_KILLED_ENEMY, "KILLED_ENEMY" },
        { DIALOG_ON_SAW_FRIEND, "SAW_FRIEND" },
        { DIALOG_ON_SAW_ENEMY, "SAW_ENEMY" },
        { DIALOG_ON_OBSOLETE_01, "OBSOLETE_01" },
        { DIALOG_ON_OBSOLETE_02, "OBSOLETE_02" },
        { DIALOG_ON_DIE, "DIE" },
        { DIALOG_ON_POKE_IDLE, "POKE_IDLE" },
        { DIALOG_ON_POKE_SEARCH, "POKE_SEARCH" },
        { DIALOG_ON_POKE_COMBAT, "POKE_COMBAT" },

        { DIALOG_STATE_FROM_IDLE_TO_COMBAT, "IDLE_TO_COMBAT" },
        { DIALOG_STATE_FROM_IDLE_TO_SEARCH, "IDLE_TO_SEARCH" },
        { DIALOG_STATE_FROM_SEARCH_TO_COMBAT, "SEARCH_TO_COMBAT" },
        { DIALOG_STATE_FROM_SEARCH_TO_IDLE, "SEARCH_TO_IDLE" },
        { DIALOG_STATE_FROM_COMBAT_TO_SEARCH, "COMBAT_TO_SEARCH" },
        { DIALOG_STATE_FROM_COMBAT_TO_IDLE, "COMBAT_TO_IDLE" }
    };
};
