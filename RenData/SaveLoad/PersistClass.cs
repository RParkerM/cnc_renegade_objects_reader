using RenData.ChunkIO;

namespace RenData.SaveLoad;
public abstract class PersistClass : PostLoadableClass
{
    public uint Old_Object_Pointer { get; init; }
    public abstract PersistFactoryClass Get_Factory();
    public virtual bool Load(ChunkLoadClass chunkLoad) { return true; }
    public virtual bool Save(ChunkSaveClass chunkSave) { return true; }
}
