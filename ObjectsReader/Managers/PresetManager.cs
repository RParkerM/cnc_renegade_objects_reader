
//using RenData.ChunkIO;
//using RenData.SaveLoad;

namespace ObjectsReader.Managers;

internal class PresetManager
{
    enum ChunkId
    {
        VARIABLES = 0x00000100,
        PRESETS,
        EMBEDDED_NODE_DATA,
        PRESET_ID,
        NODE_LIST,
        PRESET = 327687,
        CHUNKID_PRESETMGR = 327688
    };

    /// <summary>
    /// 327688 - CHUNKID_PRESETMGR
    /// </summary>
    public const uint GetChunkId = (uint)ChunkId.CHUNKID_PRESETMGR;

    //private List<Preset> presets = [];

    //public void Load(ChunkLoadClass chunkLoad)
    //{
    //    while (chunkLoad.Open_Chunk())
    //    {
    //        switch (chunkLoad.Cur_Chunk_ID)
    //        {
    //            case (uint)ChunkId.PRESETS:
    //                LoadPresets(chunkLoad);
    //                break;
    //            case (uint)ChunkId.EMBEDDED_NODE_DATA:
    //                LoadEmbeddedNodeData(chunkLoad);
    //                break;
    //        }
    //        if (!chunkLoad.EndOfChunk)
    //        {
    //            Console.WriteLine($"End of Chunk not reached. Current Chunk ID: {chunkLoad.Cur_Chunk_ID} Size = {chunkLoad.Cur_Chunk_Length}");
    //        }
    //        chunkLoad.Close_Chunk();
    //    }
    //}

    //private void LoadPresets(ChunkLoadClass chunkLoad)
    //{
    //    Dictionary<uint, int> presetCounts = [];
    //    while (chunkLoad.Open_Chunk())
    //    {
    //        switch (chunkLoad.Cur_Chunk_ID)
    //        {
    //            case (uint)ChunkId.PRESET:
    //                var preset = PersistFactory.Load<Preset>(chunkLoad);
    //                preset.Load(chunkLoad);
    //                presets.Add(preset);
    //                break;
    //            default:
    //                Console.WriteLine($"Unknown Preset Chunk ID: {chunkLoad.Cur_Chunk_ID} Size {chunkLoad.Cur_Chunk_Length}");
    //                break;
    //        }
    //        if (!presetCounts.TryGetValue(chunkLoad.Cur_Chunk_ID, out int value))
    //        {
    //            presetCounts[chunkLoad.Cur_Chunk_ID] = 0;
    //        }
    //        presetCounts[chunkLoad.Cur_Chunk_ID]++;

    //        chunkLoad.Close_Chunk();
    //    }
    //    Console.WriteLine("Preset Counts:");
    //    foreach (var kvp in presetCounts)
    //    {
    //        Console.WriteLine($"{kvp.Key} : {kvp.Value}");
    //    }

    //}

    //private void LoadEmbeddedNodeData(ChunkLoadClass chunkLoad)
    //{
    //    Dictionary<uint, int> nodeCounts = [];
    //    while (chunkLoad.Open_Chunk())
    //    {
    //        if (!nodeCounts.TryGetValue(chunkLoad.Cur_Chunk_ID, out int value))
    //        {
    //            nodeCounts[chunkLoad.Cur_Chunk_ID] = 0;
    //        }
    //        nodeCounts[chunkLoad.Cur_Chunk_ID]++;

    //        chunkLoad.Close_Chunk();
    //    }
    //    if (nodeCounts.Count != 0)
    //    {
    //        Console.WriteLine("WARNING: EMBEDDED NODE DATA FOUND IN PRESET MANAGER: UNIMPLEMENTED");
    //    }
    //    foreach (var kvp in nodeCounts)
    //    {
    //        Console.WriteLine($"{kvp.Key} : {kvp.Value}");
    //    }
    //}
}
