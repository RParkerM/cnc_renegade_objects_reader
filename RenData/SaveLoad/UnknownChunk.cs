using RenData.ChunkIO;

namespace RenData.SaveLoad;

public class UnknownChunk : PersistClass
{
    public byte[] Data = [];
    public uint ChunkId { get; protected set; }
    public uint ChunkLength { get; protected set; }
    public bool ContainsChunks { get; protected set; }

    public override PersistFactoryClass Get_Factory()
    {
        throw new NotImplementedException();
    }

    public new void Load(ChunkLoadClass chunkLoad)
    {
        ChunkId = chunkLoad.Cur_Chunk_ID;
        ChunkLength = chunkLoad.Cur_Chunk_Length;
        ContainsChunks = chunkLoad.Contains_Chunks;
        Data = new byte[ChunkLength];
        chunkLoad.Read(Data.AsSpan());
    }

    public new void Save(ChunkSaveClass chunkSave)
    {
        if (ContainsChunks)
            WriteSubChunks(chunkSave, Data);
        else
            chunkSave.Write(Data);
    }

    // Recursively re-emit inner sub-chunks so ChunkSaveClass sets contains_chunks automatically.
    private static void WriteSubChunks(ChunkSaveClass chunkSave, byte[] data)
    {
        int pos = 0;
        while (pos + 8 <= data.Length)
        {
            uint id      = BitConverter.ToUInt32(data, pos);     pos += 4;
            uint rawSize = BitConverter.ToUInt32(data, pos);     pos += 4;
            bool hasChildren = (rawSize & 0x80000000u) != 0;
            int  dataSize    = (int)(rawSize & 0x7FFFFFFFu);

            chunkSave.Begin_Chunk(id);
            if (hasChildren)
                WriteSubChunks(chunkSave, data[pos..(pos + dataSize)]);
            else
                chunkSave.Write(data[pos..(pos + dataSize)]);
            chunkSave.End_Chunk();

            pos += dataSize;
        }
    }
}
