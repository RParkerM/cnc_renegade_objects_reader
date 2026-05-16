using RenData.ChunkIO;
using System.Diagnostics;

namespace RenData.SaveLoad;
public static class PersistFactory
{
    public const uint CHUNKID_OBJPOINTER = 0x00100100;
    public const uint CHUNKID_OBJDATA = 0x00100101;

    public static T Load<T>(ChunkLoadClass chunkLoad) where T : PersistClass, new()
    {
        chunkLoad.Open_Chunk();
        Debug.Assert(chunkLoad.Cur_Chunk_ID == CHUNKID_OBJPOINTER);

        uint oldObj = 0;
        /// What do we do with this?
        chunkLoad.Read(ref oldObj);
        chunkLoad.Close_Chunk();

        //Console.WriteLine($"Object Pointer: {oldObj}");

        chunkLoad.Open_Chunk();
        Debug.Assert(chunkLoad.Cur_Chunk_ID == CHUNKID_OBJDATA);
        var new_obj = new T
        {
            Old_Object_Pointer = oldObj
        };
        new_obj.Load(chunkLoad);
        chunkLoad.Close_Chunk();

        return new_obj;
    }

    public static void Save<T>(T obj, ChunkSaveClass chunkSave) where T : PersistClass
    {
        chunkSave.Begin_Chunk(CHUNKID_OBJPOINTER);
        chunkSave.SimpleWrite(obj.Old_Object_Pointer);
        chunkSave.End_Chunk();
        chunkSave.Begin_Chunk(CHUNKID_OBJDATA);
        obj.Save(chunkSave);
        chunkSave.End_Chunk();
    }
}

public class SimplePersistFactoryClass<T>(uint ChunkId) : PersistFactoryClass where T : PersistClass, new()
{
    public override uint Chunk_ID() => ChunkId;
    public override PersistClass Load(ChunkLoadClass cload)
    {
        cload.Open_Chunk();
        Debug.Assert(cload.Cur_Chunk_ID == SIMPLEFACTORY_CHUNKID_OBJPOINTER);

        uint oldObj = 0;
        cload.Read(ref oldObj);
        cload.Close_Chunk();

        cload.Open_Chunk();
        Debug.Assert(cload.Cur_Chunk_ID == SIMPLEFACTORY_CHUNKID_OBJDATA);
        var new_obj = new T
        {
            Old_Object_Pointer = oldObj
        };
        new_obj.Load(cload);
        cload.Close_Chunk();

        //SaveLoadSystemClass::Register_Pointer(old_obj, new_obj);
        return new_obj;
    }
    public override void Save(ChunkSaveClass csave, PersistClass obj)
    {
        Debug.Assert(obj is T, "Object type mismatch in SimplePersistFactoryClass");
        csave.Begin_Chunk(SIMPLEFACTORY_CHUNKID_OBJPOINTER);
        csave.SimpleWrite(obj.Old_Object_Pointer);
        csave.End_Chunk();
        csave.Begin_Chunk(SIMPLEFACTORY_CHUNKID_OBJDATA);
        obj.Save(csave);
        csave.End_Chunk();
    }

    public const uint SIMPLEFACTORY_CHUNKID_OBJPOINTER = 0x00100100;
    public const uint SIMPLEFACTORY_CHUNKID_OBJDATA = 0x00100101;
}

public abstract class PersistFactoryClass
{
    public virtual uint Chunk_ID() => 0;
    protected PersistFactoryClass()
    {
        SaveLoadSystemClass.Register_Persist_Factory(this);
    }
    ~PersistFactoryClass()
    {
        SaveLoadSystemClass.Unregister_Persist_Factory(this);
    }

    public abstract PersistClass Load(ChunkLoadClass cload);
    public abstract void Save(ChunkSaveClass csave, PersistClass obj);

    public PersistFactoryClass? NextFactory;
}
