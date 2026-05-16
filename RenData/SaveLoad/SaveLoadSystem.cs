using RenData.ChunkIO;
using System.Diagnostics;

namespace RenData.SaveLoad;
public static class SaveLoadSystemClass
{
    //static PointerRemapClass PointerRemapper;
    public static List<PostLoadableClass> PostLoadList = [];

    private static readonly HashSet<SaveLoadSubSystemClass> SubSystemList = [];
    private static readonly HashSet<PersistFactoryClass> FactoryList = [];
    internal static bool Save(ChunkSaveClass csave, SaveLoadSubSystemClass subsystem) 
    {
        bool ok = true;
        if (subsystem.Contains_Data())
        {
            csave.Begin_Chunk(subsystem.Chunk_ID());
            ok = subsystem.Save(csave);
            csave.End_Chunk();
        }
        return ok;
    }

    public static bool Load(ChunkLoadClass cload, bool auto_post_load)
    {
        //WWLOG_PREPARE_TIME_AND_MEMORY("SaveLoadSystemClass::Load");
        //PointerRemapper.Reset();
        //WWLOG_INTERMEDIATE("PointerRemapper.Reset()");
        bool ok = true;

        // Load each chunk we encounter and link the manager into the PostLoad list
        while (cload.Open_Chunk())
        {
            SaveLoadStatus.Inc_Status_Count();     // Count the sub systems loaded
            SaveLoadSubSystemClass? sys = Find_Sub_System(cload.Cur_Chunk_ID);
            //WWLOG_INTERMEDIATE("Find_Sub_System");
            if (sys is not null)
            {
                //WWRELEASE_SAY(("			Name: %s\n",sys.Name()));
                SaveLoadStatus.INIT_SUB_STATUS(sys.Name());
                ok &= sys.Load(cload);
                //WWLOG_INTERMEDIATE(sys.Name());
            }
            cload.Close_Chunk();
        }

        // Process all of the pointer remap requests
        //PointerRemapper.Process();
        //WWLOG_INTERMEDIATE("PointerRemapper.Process()");
        //PointerRemapper.Reset();
        //WWLOG_INTERMEDIATE("PointerRemapper.Reset()");

        // Call PostLoad on each PersistClass that wanted post-load
        //if (auto_post_load)
        //{
        //    Post_Load_Processing(NULL);
        //}
        //WWLOG_INTERMEDIATE("PostLoadProcessing");

        return ok;
    }

    public static bool Post_Load_Processing(Action network_callback)
    {
        throw new NotImplementedException();
    }

    internal static void Register_Sub_System(SaveLoadSubSystemClass sys)
    {
        Debug.Assert(sys is not null);
        Link_Sub_System(sys);
    }

    internal static void Unregister_Sub_System(SaveLoadSubSystemClass sys)
    {
        Debug.Assert(sys is not null);
        Unlink_Sub_System(sys);
    }

    internal static SaveLoadSubSystemClass? Find_Sub_System(uint chunk_id)
    {
        return SubSystemList.FirstOrDefault(x => x.Chunk_ID() == chunk_id);
    }

    internal static void Link_Sub_System(SaveLoadSubSystemClass sys) { 
        if(SubSystemList.Any(x => x.Chunk_ID == sys.Chunk_ID))
        {
            throw new InvalidOperationException($"SubSystem with Chunk_ID {sys.Chunk_ID} already registered.");
        }
        SubSystemList.Add(sys);
    }
    internal static void Unlink_Sub_System(SaveLoadSubSystemClass sys) 
    { 
        SubSystemList.Remove(sys);
    }

    public static void Register_Persist_Factory(PersistFactoryClass factory)
    {
        Debug.Assert(factory is not null);
        Link_Factory(factory);
    }

    public static void Unregister_Persist_Factory(PersistFactoryClass factory)
    {
        Debug.Assert(factory is not null);
        Unlink_Factory(factory);
    }

    public static PersistFactoryClass? Find_Persist_Factory(uint chunk_id)
    {
        return FactoryList.FirstOrDefault(x => x.Chunk_ID() == chunk_id);
    }

    public static void Register_Post_Load_Callback(PostLoadableClass obj)
    {
        Debug.Assert(obj is not null);
        if (!obj.Is_Post_Load_Registered())
        {
            obj.Set_Post_Load_Registered(true);
            PostLoadList.Add(obj);
        }
    }

//    public static void Register_Pointer(void* old_pointer, void* new_pointer)
//    {
//        PointerRemapper.Register_Pointer(old_pointer, new_pointer);
//    }

//# ifdef WWDEBUG

//    public static void Request_Pointer_Remap(void** pointer_to_convert,const char* file,int line)
//{

//    PointerRemapper.Request_Pointer_Remap(pointer_to_convert, file, line);
//}

//public static void Request_Ref_Counted_Pointer_Remap(RefCountClass** pointer_to_convert,const char* file, int line)
//    {
//        PointerRemapper.Request_Ref_Counted_Pointer_Remap(pointer_to_convert, file, line);
//    }

//#else

//    public static void Request_Pointer_Remap(void** pointer_to_convert)
//    {
//        PointerRemapper.Request_Pointer_Remap(pointer_to_convert);
//    }

//    public static void Request_Ref_Counted_Pointer_Remap(RefCountClass** pointer_to_convert)
//    {
//        PointerRemapper.Request_Ref_Counted_Pointer_Remap(pointer_to_convert);
//    }

//#endif


    public static void Link_Factory(PersistFactoryClass fact)
    {
        Debug.Assert(fact is not null);
        if (fact is not null)
        {
            Debug.Assert(!FactoryList.Contains(fact));            // factories should never be registered twice!
            FactoryList.Add(fact);
        }
    }

    public static void Unlink_Factory(PersistFactoryClass fact)
    {
        Debug.Assert(fact is not null);
        FactoryList.Remove(fact);
    }

    //void Force_Link_WWSaveLoad(void)
    //{
    //    FORCE_LINK(Twiddler);
    //    return;
    //}
}


