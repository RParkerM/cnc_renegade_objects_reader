using RenData.ChunkIO;

namespace RenData.SaveLoad;

public class UnknownChunk : PersistClass
{
    public byte[] Data = [];
    public uint ChunkId { get; protected set; }
    public uint ChunkLength { get; protected set; }

    public override PersistFactoryClass Get_Factory()
    {
        throw new NotImplementedException();
    }

    public new void Load(ChunkLoadClass chunkLoad)
    {
        ChunkId = chunkLoad.Cur_Chunk_ID;
        ChunkLength = chunkLoad.Cur_Chunk_Length;
        Data = new byte[ChunkLength];
        chunkLoad.Read(Data.AsSpan());
    }

    public new void Save(ChunkSaveClass chunkSave)
    {
        chunkSave.Write(Data);
    }
}