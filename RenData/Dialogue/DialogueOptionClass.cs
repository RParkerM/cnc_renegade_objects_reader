using RenData.ChunkIO;
using RenData.SaveLoad;
using RenData.IDs;

namespace RenData.Dialogue;

public class DialogueOptionClass
{

    public DialogueOptionClass()
    {
    }
    public DialogueOptionClass(DialogueOptionClass src)
    {
        Weight = src.Weight;
        ConversationID = src.ConversationID;
    }

    public int Get_Conversation_ID() { return ConversationID; }
    public float Get_Weight() { return Weight; }

    public void Set_Conversation_ID(int id) { ConversationID = id; }
    public void Set_Weight(float weight) { Weight = weight; }

    //
    //	Save/load
    //
    public void Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_OPTION_VARIABLES);
        csave.WriteMicro(VARID_WEIGHT, Weight);
        csave.WriteMicro(VARID_CONVERSATION_ID, ConversationID);
        csave.End_Chunk();

        return;
    }
    public void Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {

                case CHUNKID_OPTION_VARIABLES:
                    Load_Variables(cload);
                    break;
            }

            cload.Close_Chunk();
        }

        return;
    }

	protected void Load_Variables(ChunkLoadClass cload)
    {
        while (cload.Open_Micro_Chunk())
        {
            switch (cload.Cur_Micro_Chunk_ID)
            {

                case VARID_WEIGHT:
                    cload.Read(ref Weight);
                    break;
                case VARID_CONVERSATION_ID:
                    cload.Read(ref ConversationID);
                    break;
            }

            cload.Close_Micro_Chunk();
        }

        return;
    }

    protected float Weight = 1;
    protected int ConversationID = 0;


    // Converted enums -> private const fields
    private const uint CHUNKID_OPTION_VARIABLES = 0x08040528u;
    private const uint CHUNKID_DIALOGUE_VARIABLES = 0x08040529u;
    private const uint CHUNKID_DIALOGUE_OPTION = 0x0804052Au;

    private const int VARID_WEIGHT = 0;
    private const int XXX_VARID_REMARK_TEXT_ID = 1;
    private const int VARID_CONVERSATION_ID = 2;

    private const int VARID_DIALOGUE_SILENCE = 0;
}
