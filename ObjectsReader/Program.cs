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

Console.WriteLine("Done.");

newFile.Flush();
newFile.Close();

var originalBytes = File.ReadAllBytes(filename);
var writtenBytes = File.ReadAllBytes(filename2);

if (originalBytes.SequenceEqual(writtenBytes))
{
    Console.WriteLine("Files are bit identical.");
}
else
{
    Console.WriteLine($"Files differ! Original: {originalBytes.Length} bytes, Written: {writtenBytes.Length} bytes");

    Console.WriteLine("\n--- Chunk-level diff ---");
    using var s1 = new FileStream(filename, FileMode.Open, FileAccess.Read);
    using var s2 = new FileStream(filename2, FileMode.Open, FileAccess.Read);
    ChunkDiff(s1, s2, s1.Length, s2.Length, "root");
}


//enum ChunkId
//{
//    CHUNKID_SAVELOAD_DEFMGR = 257,
//    CHUNKID_PRESETMGR = 327688
//}

// ── helpers ──────────────────────────────────────────────────────────────────

static bool ChunkDiff(Stream s1, Stream s2, long end1, long end2, string path)
{
    int idx = 0;

    while (s1.Position < end1 || s2.Position < end2)
    {
        if (s1.Position >= end1)
        {
            Console.WriteLine($"  {path}[{idx}]: original has no more chunks; written still has data");
            return false;
        }
        if (s2.Position >= end2)
        {
            Console.WriteLine($"  {path}[{idx}]: written has no more chunks; original still has data");
            return false;
        }

        var h1 = ReadChunkHeader(s1);
        var h2 = ReadChunkHeader(s2);

        // Try to resolve a human-readable name for the chunk ID
        string name1 = DefinitionMgrClass.RegisteredDefinitions.TryGetValue(h1.Id, out var rn1) ? rn1 : $"0x{h1.Id:X8}";
        string name2 = DefinitionMgrClass.RegisteredDefinitions.TryGetValue(h2.Id, out var rn2) ? rn2 : $"0x{h2.Id:X8}";

        if (h1.Id != h2.Id)
        {
            Console.WriteLine($"  {path}[{idx}]: chunk ID mismatch — original={name1}, written={name2}");
            return false;
        }

        string loc = $"{path}[{idx}:{name1}]";

        if (h1.HasChildren != h2.HasChildren)
        {
            Console.WriteLine($"  {loc}: contains_chunks flag mismatch — original={h1.HasChildren}, written={h2.HasChildren}");
            s1.Seek(h1.DataSize, SeekOrigin.Current);
            s2.Seek(h2.DataSize, SeekOrigin.Current);
            idx++;
            continue;
        }

        if (h1.DataSize != h2.DataSize)
        {
            Console.WriteLine($"  {loc}: size mismatch — original={h1.DataSize} bytes, written={h2.DataSize} bytes");
            s1.Seek(h1.DataSize, SeekOrigin.Current);
            s2.Seek(h2.DataSize, SeekOrigin.Current);
            idx++;
            continue;
        }

        if (h1.HasChildren)
        {
            long ce1 = s1.Position + h1.DataSize;
            long ce2 = s2.Position + h2.DataSize;
            ChunkDiff(s1, s2, ce1, ce2, loc);
            // Seek past any unread remainder (defensive)
            s1.Position = ce1;
            s2.Position = ce2;
        }
        else
        {
            byte[] d1 = new byte[h1.DataSize];
            byte[] d2 = new byte[h2.DataSize];
            s1.ReadExactly(d1);
            s2.ReadExactly(d2);

            if (!d1.AsSpan().SequenceEqual(d2))
            {
                int diff = 0;
                while (d1[diff] == d2[diff]) diff++;
                int printEnd = Math.Min(diff + 16, d1.Length);
                Console.WriteLine($"  {loc}: content differs at byte +{diff} (0x{diff:X})");
                Console.WriteLine($"    original: {Convert.ToHexString(d1[diff..printEnd])}");
                Console.WriteLine($"    written:  {Convert.ToHexString(d2[diff..printEnd])}");
            }
        }

        idx++;
    }

    return true;
}

static (uint Id, uint DataSize, bool HasChildren) ReadChunkHeader(Stream s)
{
    Span<byte> buf = stackalloc byte[8];
    s.ReadExactly(buf);
    uint id      = BitConverter.ToUInt32(buf[..4]);
    uint rawSize = BitConverter.ToUInt32(buf[4..]);
    return (id, rawSize & 0x7FFFFFFFu, (rawSize & 0x80000000u) != 0);
}
