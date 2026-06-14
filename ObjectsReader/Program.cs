using ObjectsReader.Managers;
using RenData.ChunkIO;
using RenData.Definitions;
using RenData.SaveLoad;

//string filename = "C:\\Westwood\\tools\\leveledit\\TestDamageMods\\presets\\objects.ddb";
//string filename = "D:\\Westwood\\tools\\leveledit\\Stock\\presets\\objects.ddb";
#if true
string inputFilename = "objects.ddb";
string outputFilename = "objects2.ddb";
#else
string inputFilename = "objects2.ddb";
string outputFilename = "objects3.ddb";
#endif

string filename = $"D:\\Program Files (x86)\\Steam\\steamapps\\common\\Command & Conquer Renegade - OW3D\\LevelEditor\\stock\\presets\\{inputFilename}";
var fileInfo = new FileInfo(filename);


var chunkLoad = new ChunkLoadClass(new FileStream(filename, FileMode.Open, FileAccess.Read));

var defMgr = new DefinitionMgrClass();
var presetMgr = new PresetManager();

var presetMgrChunk = new UnknownChunk();

List<UnknownChunk> unknownChunks = [];

while (chunkLoad.Open_Chunk())
{
    //Console.WriteLine($"Opened Chunk: {chunkLoad.Cur_Chunk_ID} Size = {chunkLoad.Cur_Chunk_Length}");
    switch (chunkLoad.Cur_Chunk_ID)
    {
        case DefinitionMgrClass.GetChunkId:
            Console.WriteLine($"Opened DefinitinoManager Chunk");
            defMgr.Load(chunkLoad);
            break;
        case PresetManager.GetChunkId:
            Console.WriteLine($"Opened Preset Manager Chunk");
            presetMgrChunk.Load(chunkLoad);
            break;
        default:
            Console.WriteLine($"Unknown Chunk Type: {chunkLoad.Cur_Chunk_ID}");
            var unknownChunk = new UnknownChunk();
            unknownChunk.Load(chunkLoad);
            unknownChunks.Add(unknownChunk);
            break;
    }
    if (!chunkLoad.EndOfChunk)
    {
        Console.WriteLine($"End of Chunk not reached. Current Chunk ID: {chunkLoad.Cur_Chunk_ID} Size = {chunkLoad.Cur_Chunk_Length}");
    }

    chunkLoad.Close_Chunk();
}

var filename2 = fileInfo.FullName.Replace(inputFilename, outputFilename);
var newFile = new FileStream(filename2, FileMode.Create, FileAccess.Write);
var chunkSave = new ChunkSaveClass(newFile);

chunkSave.Begin_Chunk(DefinitionMgrClass.GetChunkId);
defMgr.Save(chunkSave);
Console.WriteLine("Definition Manager Saved");
chunkSave.End_Chunk();

chunkSave.Begin_Chunk(PresetManager.GetChunkId);
presetMgrChunk.Save(chunkSave);
chunkSave.End_Chunk();

foreach (var unknownChunk in unknownChunks)
{
    unknownChunk.Save(chunkSave);
}

//defMgr.Save(chunkSave);
//presetMgrChunk.Save(chunkSave);
Console.WriteLine("Done.");


//enum ChunkId
//{
//    CHUNKID_SAVELOAD_DEFMGR = 257,
//    CHUNKID_PRESETMGR = 327688
//}