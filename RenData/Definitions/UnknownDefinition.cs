using RenData.ChunkIO;
using RenData.SaveLoad;

namespace RenData.Definitions;
public class UnknownDefinition : Definition
{
    public byte[] Data = [];

    public override PersistFactoryClass Get_Factory()
    {
        throw new NotImplementedException();
    }

    public override bool Load(ChunkLoadClass chunkLoad)
    {
        ChunkId = chunkLoad.Cur_Chunk_ID;
        ChunkLength = chunkLoad.Cur_Chunk_Length;
        Data = new byte[ChunkLength];
        chunkLoad.Read(Data.AsSpan());
        return true;

    }
    public override bool Save(ChunkSaveClass chunkSave)
    {
        chunkSave.Write(Data);
        return true;
    }
}
