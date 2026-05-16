using RenData.ChunkIO;
using RenData.SaveLoad;
using System.Text;

namespace RenData.Definitions;


public class TempDefinition
{
    public TempDefinition(Definition def)
    {
        OldDefinition = def;
        DefinitionType = DefType.Old;
    }
    public TempDefinition(DefinitionClass def)
    {
        NewDefinition = def;
        DefinitionType = DefType.New;
    }
    public Definition? OldDefinition;
    public DefinitionClass? NewDefinition;
    public DefType DefinitionType;

    public enum DefType
    {
        Old,
        New
    }
}

public class DefinitionManager
{
    public const uint GetChunkId = 257;

    public enum ChunkId
    {
        WWAUDIO_BEGIN = 0x00030000,
    }

    public List<TempDefinition> Definitions = [];

    private readonly UnknownChunk variables = new();
    const uint VariablesChunkId = 256;
    const uint DefinitionsChunkId = 257;

    private readonly List<UnknownChunk> unknownChunks = [];

    public static readonly Dictionary<uint, string> RegisteredDefinitions = [];


    public void LoadVariables(ChunkLoadClass chunkLoad)
    {
        if (chunkLoad.Cur_Chunk_Length != 0)
        {
            Console.WriteLine($"Variables Chunk Length: {chunkLoad.Cur_Chunk_Length}");
        }
        variables.Load(chunkLoad);
        if (!chunkLoad.EndOfChunk)
        {
            Console.WriteLine($"End of Chunk not reached. Current Chunk ID: {chunkLoad.Cur_Chunk_ID} Size = {chunkLoad.Cur_Chunk_Length}");
        }
    }

    public void Load(ChunkLoadClass chunkLoad)
    {
        Console.WriteLine($"Loading Definition Manager");
        while (chunkLoad.Open_Chunk())
        {
            switch (chunkLoad.Cur_Chunk_ID)
            {
                case VariablesChunkId:
                    LoadVariables(chunkLoad);
                    break;
                case DefinitionsChunkId:
                    LoadDefinitions(chunkLoad);
                    break;
                default:
                    Console.WriteLine($"Unknown Chunk ID: {chunkLoad.Cur_Chunk_ID} Size = {chunkLoad.Cur_Chunk_Length}");
                    var unknownChunk = new UnknownChunk();
                    unknownChunk.Load(chunkLoad);
                    unknownChunks.Add(unknownChunk);
                    break;
            }
            chunkLoad.Close_Chunk();
        }
    }

    private void LoadDefinitions(ChunkLoadClass cload)
    {
        Dictionary<uint, int> definitionCounts = [];

        while (cload.Open_Chunk())
        {
            PersistFactoryClass? factory = SaveLoadSystemClass.Find_Persist_Factory(cload.Cur_Chunk_ID);
            definitionCounts.TryGetValue(cload.Cur_Chunk_ID, out int count);
            definitionCounts[cload.Cur_Chunk_ID] = count + 1;

            if (factory != null)
            {
                var def = factory.Load(cload);
                Definitions.Add(new TempDefinition((DefinitionClass)def));
            }
            else
            {
                var unknownDef = new UnknownDefinition();
                unknownDef.Load(cload);
                Definitions.Add(new TempDefinition(unknownDef));
            }

            if (!cload.EndOfChunk)
            {
                Console.WriteLine($"End of Chunk not reached. Current Chunk ID: {cload.Cur_Chunk_ID} Size = {cload.Cur_Chunk_Length}");
            }

            cload.Close_Chunk();
        }

        foreach (var kvp in definitionCounts)
        {
            var name = RegisteredDefinitions.ContainsKey(kvp.Key) ? $"{RegisteredDefinitions[kvp.Key]} {kvp.Key}" : kvp.Key.ToString();
            Console.WriteLine($"Loaded {kvp.Value} definitions of Chunk ID: {name} ({GetChunkTypeHex(kvp.Key)})");
        }

        if (!cload.EndOfChunk)
        {
            Console.WriteLine($"End of Chunk not reached. Current Chunk ID: {cload.Cur_Chunk_ID} Size = {cload.Cur_Chunk_Length}");
        }
    }

    public void Save(ChunkSaveClass chunkSave)
    {
        chunkSave.Begin_Chunk(VariablesChunkId);
        SaveVariables(chunkSave);
        chunkSave.End_Chunk();
        chunkSave.Begin_Chunk(DefinitionsChunkId);
        SaveDefinitions(chunkSave);
        chunkSave.End_Chunk();

        foreach (var unknownChunk in unknownChunks)
        {
            throw new InvalidOperationException("Cannot save unknown chunks in DefinitionManager.");
            //unknownChunk.Save(chunkSave);
        }
    }

    public void SaveDefinitions(ChunkSaveClass chunkSaveClass)
    {
        foreach (var definition in Definitions)
        {
            if(definition.DefinitionType == TempDefinition.DefType.Old) //is UnknownPersistDefinition persistClass)
            {
                var unknownDef = (UnknownDefinition)definition.OldDefinition!;
                chunkSaveClass.Begin_Chunk(unknownDef.ChunkId);
                unknownDef.Save(chunkSaveClass);
                chunkSaveClass.End_Chunk();
            }
            else
            {
                var def = (DefinitionClass)definition.NewDefinition!;
                chunkSaveClass.Begin_Chunk(def.Get_Class_ID());
                PersistFactory.Save(def, chunkSaveClass);
                chunkSaveClass.End_Chunk();
            }
        }
    }

    public bool SaveVariables(ChunkSaveClass chunkSave)
    {
        //variables.Save(chunkSave);
        return true;
    }

    string GetChunkTypeHex(uint ChunkType)
    {
        byte[] bytes = BitConverter.GetBytes(ChunkType);
        StringBuilder sb = new();
        foreach (byte b in bytes)
        {
            sb.Append($"{b:X2} ");
        }
        return sb.ToString();
    }
}