using RenData.ChunkIO;
using RenData.SaveLoad;

namespace RenData.Definitions;
public abstract class Definition : PersistClass
{
    public uint ChunkId { get; protected set; }
    public uint ChunkLength { get; protected set; }
    // TODO Consider whether Old_Object_Pointer is needed. If not necessary, put justification here.
    //public uint Old_Object_Pointer { get; init; }

    public override abstract bool Load(ChunkLoadClass chunkLoad);
    public override abstract bool Save(ChunkSaveClass chunkSave);
}
