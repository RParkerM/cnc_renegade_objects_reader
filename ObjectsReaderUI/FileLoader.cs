using RenData.ChunkIO;
using RenData.Definitions;
using RenData.SaveLoad;

namespace ObjectsReaderUI;

public record TopLevelChunk(string Label, uint ChunkId, object Data);

public static class FileLoader
{
    private const uint PresetManagerChunkId = 327688;

    public static List<TopLevelChunk> Load(string path)
    {
        var result = new List<TopLevelChunk>();

        DefinitionMgrClass.Free_Definitions();

        var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        var chunkLoad = new ChunkLoadClass(stream);
        var defMgr = new DefinitionMgrClass();

        while (chunkLoad.Open_Chunk())
        {
            uint id = chunkLoad.Cur_Chunk_ID;
            switch (id)
            {
                case DefinitionMgrClass.GetChunkId:
                    defMgr.Load(chunkLoad);
                    result.Add(new TopLevelChunk("DefinitionManager", id, defMgr));
                    break;
                case PresetManagerChunkId:
                    var presetChunk = new UnknownChunk();
                    presetChunk.Load(chunkLoad);
                    result.Add(new TopLevelChunk("PresetManager", id, presetChunk));
                    break;
                default:
                    var unknown = new UnknownChunk();
                    unknown.Load(chunkLoad);
                    result.Add(new TopLevelChunk($"Unknown (0x{id:X8})", id, unknown));
                    break;
            }
            chunkLoad.Close_Chunk();
        }

        stream.Close();
        return result;
    }

    /// <summary>
    /// Writes the loaded chunks back out in their original file order. Mirrors the
    /// round-trip save in the console tool: each top-level chunk re-serializes
    /// itself, so an unedited file round-trips bit-for-bit.
    /// </summary>
    public static void Save(string path, IEnumerable<TopLevelChunk> chunks)
    {
        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        var chunkSave = new ChunkSaveClass(stream);

        foreach (var chunk in chunks)
        {
            chunkSave.Begin_Chunk(chunk.ChunkId);
            switch (chunk.Data)
            {
                case DefinitionMgrClass defMgr:
                    defMgr.Save(chunkSave);
                    break;
                case UnknownChunk unknown:
                    unknown.Save(chunkSave);
                    break;
            }
            chunkSave.End_Chunk();
        }

        stream.Flush();
    }
}
