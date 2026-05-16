using System.Diagnostics;

namespace RenData.SaveLoad;
internal static class SaveLoadStatus
{
    const int MAX_STATUS_TEXT_ID = 2;

    public static void INIT_STATUS(string t) => Set_Status_Text(t, 0);
    public static void INIT_SUB_STATUS(string t) => Set_Status_Text(t, 1);

    private static object text_mutex = new object();
    private static List<string> status_text = new(MAX_STATUS_TEXT_ID);

    public static void Set_Status_Text(string text, int id)
    {
        lock (text_mutex)
        {
            Debug.Assert(id < MAX_STATUS_TEXT_ID);
            status_text[id] = text;
            if (id == 0) status_text[1] = "";
        }
    }

    public static void Get_Status_Text(ref string text, int id)
    {
        lock (text_mutex)
        {
            Debug.Assert(id < MAX_STATUS_TEXT_ID);
            text = status_text[id];
        }
    }

    private static int status_count;
    public static void Reset_Status_Count()
    {
        status_count = 0;
    }

    public static void Inc_Status_Count()
    {
        status_count++;
    }

    public static int Get_Status_Count()
    {
        return status_count;
    }
}
