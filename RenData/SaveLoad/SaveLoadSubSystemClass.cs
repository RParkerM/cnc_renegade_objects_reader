using RenData.ChunkIO;

namespace RenData.SaveLoad;

public abstract class SaveLoadSubSystemClass : PostLoadableClass
{
    protected SaveLoadSubSystemClass()
    {
        SaveLoadSystemClass.Register_Sub_System(this);
    }
    ~SaveLoadSubSystemClass()
    {
        SaveLoadSystemClass.Unregister_Sub_System(this);
    }
    public virtual uint Chunk_ID() => 0;
    internal virtual bool Contains_Data() => true;
    public abstract bool Save(ChunkSaveClass csave);
    public abstract bool Load(ChunkLoadClass cload);
    internal abstract string Name();

    internal SaveLoadSubSystemClass? NextSubSystem { get; set; } = null;
}
