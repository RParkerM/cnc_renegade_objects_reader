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
}
