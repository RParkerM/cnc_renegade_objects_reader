using RenData.IDs;
using RenData.ChunkIO;
using System.Diagnostics;
using RenData.SaveLoad;

namespace RenData.Definitions;

public class DefinitionMgrClass : SaveLoadSubSystemClass
{
    public static DefinitionMgrClass _TheDefinitionMgr { get; } = new();

    public override uint Chunk_ID() => ChunkId.CHUNKID_SAVELOAD_DEFMGR;

    ~DefinitionMgrClass()
    {
        Free_Definitions();
    }

    public static DefinitionClass? Find_Definition(uint id, bool twiddle = true)
    {
        DefinitionClass? definition = null;

        int lower_index = 0;
        int upper_index = _DefinitionCount - 1;
        int index = upper_index / 2;
        bool keep_going = (_DefinitionCount > 0);

        //
        //	Binary search the list until we've found the definition
        //
        while (keep_going)
        {
            DefinitionClass curr_def = _SortedDefinitionArray[index];
            Debug.Assert(curr_def is not null);

            //
            //	Is this the definition we are looking for?
            //
            if (curr_def.Get_ID() == id)
            {
                definition = _SortedDefinitionArray[index];
                keep_going = false;
            }
            else if (upper_index <= lower_index + 1)
            {

                //
                //	When the window get's too small, our divide by two won't catch
                // both entries, so just go ahead and do them both now.
                //
                keep_going = false;
                if (_SortedDefinitionArray[lower_index].Get_ID() == id)
                {
                    definition = _SortedDefinitionArray[lower_index];
                }
                else if (_SortedDefinitionArray[upper_index].Get_ID() == id)
                {
                    definition = _SortedDefinitionArray[upper_index];
                }

            }
            else
            {

                //
                //	Cut our 'window' in half
                //
                if (id > curr_def.Get_ID())
                {
                    lower_index = index;
                    index += (upper_index - index) / 2;
                }
                else
                {
                    upper_index = index;
                    index -= (index - lower_index) / 2;
                }
            }
        }

        //
        //	Should we twiddle this definition? (Twiddling refers to our randomizing
        //	framework for definitions)
        //
        if (twiddle &&
                definition is not null &&
                definition.Get_Class_ID() == ClassId.CLASSID_TWIDDLERS)
        {
            definition = ((TwiddlerClass)definition).Twiddle();
        }

        return definition;
    }

    public static DefinitionClass? Find_Named_Definition(string name, bool twiddle = true)
    {
        DefinitionClass? definition = null;

        //
        //	Loop through all the definitions and see if we can
        // find the one with the requested name
        //
        for (int index = 0; index < _DefinitionCount; index++)
        {
            DefinitionClass curr_def = _SortedDefinitionArray[index];

            //
            //	Is this the definition we were looking for?
            //
            if (curr_def is not null && curr_def.Get_Name() == name)
            {
                definition = curr_def;
                break;
            }
        }

        //
        //	Should we twiddle this definition? (Twiddling refers to our randomizing
        //	framework for definitions)
        //
        if (twiddle &&
                definition is not null &&
                definition.Get_Class_ID() == ClassId.CLASSID_TWIDDLERS)
        {
            definition = ((TwiddlerClass)definition).Twiddle();
        }

        return definition;
    }
    public static DefinitionClass? Find_Typed_Definition(string name, uint class_id, bool twiddle = true)
    {
        //
        //	Sanity check
        //
        if (DefinitionHash is null)
        {
            Trace.WriteLine(("DefinitionMgrClass::Find_Typed_Definition () failed due to a null DefinitionHash. {0}\n", name));
            return null;
        }

        DefinitionClass? definition = null;

        // Check the hash table first. The hash table is built as we need the definitions, so if definition is not
        // in the table, it will be added there.

        //
        // TSS null deref on this sucker 08/03/01
        //
        Debug.Assert(DefinitionHash is not null);

        string lower_case_name = name.ToLower();
        DefinitionHash.TryGetValue(lower_case_name, out var defs);

        if (defs is not null)
        {
            for (int i = 0; i < defs.Count; ++i)
            {
                DefinitionClass curr_def = defs[i];
                Debug.Assert(curr_def is not null);
                uint curr_class_id = curr_def.Get_Class_ID();
                if ((curr_class_id == class_id) ||
                        (ClassId.SuperClassID_From_ClassID(curr_class_id) == class_id) ||
                        (twiddle && (curr_def.Get_Class_ID() == ClassId.CLASSID_TWIDDLERS)))
                {
                    definition = curr_def;
                    break;
                }
            }
        }

        //
        //	Loop through all the definitions and see if we can
        // find the one with the requested name
        //
        if (definition is null)
        {
            for (int index = 0; index < _DefinitionCount; index++)
            {
                DefinitionClass curr_def = _SortedDefinitionArray[index];
                if (curr_def is not null)
                {

                    //
                    //	Is this the correct class of definition?
                    //
                    uint curr_class_id = curr_def.Get_Class_ID();
                    if ((curr_class_id == class_id) ||
                            (ClassId.SuperClassID_From_ClassID(curr_class_id) == class_id) ||
                            (twiddle && (curr_def.Get_Class_ID() == ClassId.CLASSID_TWIDDLERS)))
                    {
                        //
                        //	Is this the definition we were looking for?
                        //
                        if (string.Equals(curr_def.Get_Name(), name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            definition = curr_def;
                            // Add the definition to the hash table, so that it can be quickly accessed the next time it is needed.
                            if (defs is null)
                            {
                                defs = [];
                                DefinitionHash.Add(lower_case_name, defs);
                            }
                            defs.Add(definition);
                            break;
                        }
                    }
                }
            }
        }

        //
        //	Should we twiddle this definition? (Twiddling refers to our randomizing
        //	framework for definitions)
        //
        if (twiddle &&
                definition is not null &&
                definition.Get_Class_ID() == ClassId.CLASSID_TWIDDLERS)
        {
            definition = ((TwiddlerClass)definition).Twiddle();
        }

        return definition;
    }
    public static void List_Available_Definitions()
    {
        //
        //	Loop through all the definitions and print the definition name
        //
        Trace.WriteLine(("Available definitions:\n"));
        for (int index = 0; index < _DefinitionCount; index++)
        {
            DefinitionClass curr_def = _SortedDefinitionArray[index];
            if (curr_def is not null)
            {
                Trace.WriteLine(("  >{0}<\n", curr_def.Get_Name()));
            }
        }

        return;
    }
    public static void List_Available_Definitions(int superclass_id)
    {
        //
        //	Loop through all the definitions and print the definition name
        //
        Trace.WriteLine(("Available superclass definitions for 0x{0,8:X}\n", superclass_id));
        DefinitionClass? definition;
        for (definition = Get_First((uint)superclass_id, ID_SUPERCLASS);
                definition is not null;
                definition = Get_Next(definition, (uint)superclass_id, ID_SUPERCLASS))
        {
            Trace.WriteLine(("  >{0}<\n", definition.Get_Name()));
        }

        return;
    }
    public static uint Get_New_ID(uint class_id)
    {
        uint idrange_start = (class_id - ClassId.DEF_CLASSID_START) * IDRANGE_PER_CLASS;
        uint idrange_end = (idrange_start + IDRANGE_PER_CLASS);

        uint new_id = idrange_start + 1;

        //
        //	Try to find the first empty slot in this ID range
        //
        for (int index = 0; index < _DefinitionCount; index++)
        {
            DefinitionClass definition = _SortedDefinitionArray[index];
            if (definition is not null)
            {

                //
                //	Get this definition's ID
                //
                uint curr_id = definition.Get_ID();

                //
                //	Is this id in the range we are looking for?
                //
                if (curr_id >= idrange_start && curr_id < idrange_end)
                {

                    bool is_ok = false;
                    if (index < _DefinitionCount - 1)
                    {

                        //
                        //	Check to see if the next definition in our array leaves a hole in the
                        // ID range.
                        //
                        DefinitionClass next_definition = _SortedDefinitionArray[index + 1];
                        if (next_definition is not null && next_definition.Get_ID() > (curr_id + 1))
                        {
                            is_ok = true;
                        }

                    }
                    else
                    {
                        is_ok = true;
                    }

                    //
                    //	Return the new ID
                    //
                    if (is_ok)
                    {
                        new_id = curr_id + 1;
                        break;
                    }
                }
            }
        }

        return new_id;
    }

    public static void Register_Definition(DefinitionClass definition)
    {
        Debug.Assert(definition is not null);
        if (definition is not null && definition.m_DefinitionMgrLink == -1 && definition.Get_ID() != 0)
        {
            //
            //	Make sure the definition array is large enough
            //
            Prepare_Definition_Array();

            //
            //	Calculate where in the list we should insert this definition
            //
            uint id = definition.Get_ID();
            int lower_index = 0;
            int upper_index = _DefinitionCount - 1;
            int index = upper_index / 2;
            int insert_index = _DefinitionCount;
            bool keep_going = (_DefinitionCount > 0);
            bool is_valid = true;

            while (keep_going)
            {

                DefinitionClass curr_def = _SortedDefinitionArray[index];
                Debug.Assert(curr_def is not null);

                //
                //	Check to make sure we aren't trying to register a definition
                // that has the same ID as a definition that is already in the list.
                //
                if (curr_def.Get_ID() == id)
                {
                    insert_index = index;
                    keep_going = false;
                    is_valid = false;
                }
                else
                {

                    //
                    //	Cut our 'window' in half
                    //
                    if (id > curr_def.Get_ID())
                    {
                        lower_index = index;
                        index += (upper_index - index) / 2;
                    }
                    else
                    {
                        upper_index = index;
                        index -= (index - lower_index) / 2;
                    }

                    //
                    //	If we've narrowed down the window to 2 entries, then quick check
                    // the different possibilities.
                    //
                    if (upper_index <= lower_index + 1)
                    {
                        if (_SortedDefinitionArray[upper_index].Get_ID() <= id)
                        {
                            insert_index = upper_index + 1;
                        }
                        else if (_SortedDefinitionArray[lower_index].Get_ID() <= id)
                        {
                            insert_index = upper_index;
                        }
                        else
                        {
                            insert_index = lower_index;
                        }
                        keep_going = false;
                    }
                }
            }

            //Debug.Assert (is_valid);
            if (is_valid)
            {

                //
                //	Re-index all the definitions that got bumped one cell due to this insertion.
                //
                for (index = _DefinitionCount - 1; index >= insert_index; index--)
                {
                    _SortedDefinitionArray[index + 1] = _SortedDefinitionArray[index];
                    _SortedDefinitionArray[index + 1].m_DefinitionMgrLink = index + 1;
                }

                //
                //	Insert this definition into the list
                //
                definition.m_DefinitionMgrLink = insert_index;
                _SortedDefinitionArray[insert_index] = definition;
                _DefinitionCount++;
            }
        }

        return;
    }
    public static void Unregister_Definition(DefinitionClass definition)
    {
        Debug.Assert(definition is not null);
        //Debug.Assert (definition.m_DefinitionMgrLink >= 0 && definition.m_DefinitionMgrLink < _DefinitionCount);

        if (definition is not null && definition.m_DefinitionMgrLink != -1)
        {
            //
            //	Re-index the definitions that come after this definition in the list
            //
            for (int index = definition.m_DefinitionMgrLink; index < _DefinitionCount - 1; index++)
            {
                _SortedDefinitionArray[index] = _SortedDefinitionArray[index + 1];
                _SortedDefinitionArray[index].m_DefinitionMgrLink = index;
            }

            _SortedDefinitionArray.RemoveAt(_DefinitionCount - 1);
            definition.m_DefinitionMgrLink = -1;
            _DefinitionCount--;
        }

        return;
    }

    public static DefinitionClass? Get_First()
    {
        DefinitionClass? definition = null;
        if (_DefinitionCount > 0)
        {
            definition = _SortedDefinitionArray[0];
        }

        return definition;
    }
    public static DefinitionClass? Get_First(uint id, int type = ID_CLASS)
    {
        DefinitionClass? definition = null;

        //
        //	Loop through all the definitions and find the first
        // one that belongs to the requested class
        //
        for (int index = 0;
                (definition is null) && (index < _DefinitionCount);
                index++)
        {
            DefinitionClass curr_def = _SortedDefinitionArray[index];
            if (curr_def is not null)
            {

                //
                //	Is this the definition we were looking for?
                //
                if ((type == ID_SUPERCLASS) &&
                        (ClassId.SuperClassID_From_ClassID(curr_def.Get_Class_ID()) == id))
                {
                    definition = curr_def;
                }
                else if ((type == ID_CLASS) &&
                                (curr_def.Get_Class_ID() == id))
                {
                    definition = curr_def;
                }
            }
        }

        return definition;
    }
    public static DefinitionClass? Get_Next(DefinitionClass curr_def)
    {
        Debug.Assert(curr_def is not null);
        DefinitionClass? definition = null;

        int index = curr_def.m_DefinitionMgrLink + 1;
        if (index < _DefinitionCount)
        {
            definition = _SortedDefinitionArray[index];
        }

        return definition;
    }
    public static DefinitionClass? Get_Next(DefinitionClass curr_def, uint id, int type = ID_CLASS)
    {
        DefinitionClass? definition = null;

        //
        //	Loop through all the definitions and find the first
        // one that belongs to the requested class
        //
        for (int index = curr_def.m_DefinitionMgrLink + 1;
                (definition is null) && (index < _DefinitionCount);
                index++)
        {
            curr_def = _SortedDefinitionArray[index];
            if (curr_def is not null)
            {

                //
                //	Is this the definition we were looking for?
                //
                if ((type == ID_SUPERCLASS) &&
                        (ClassId.SuperClassID_From_ClassID(curr_def.Get_Class_ID()) == id))
                {
                    definition = curr_def;
                }
                else if ((type == ID_CLASS) &&
                                (curr_def.Get_Class_ID() == id))
                {
                    definition = curr_def;
                }
            }
        }

        return definition;
    }

    public static void Free_Definitions()
    {
        // Clear the hash table
        foreach (var kvp in DefinitionHash)
        {
            var defs = kvp.Value;
            defs?.Clear();

        }
        DefinitionHash.Clear();

        //
        //	Free each of the definition objects
        //
        _SortedDefinitionArray.Clear();
        _fileOrderedItems.Clear();

        _DefinitionCount = 0;
        return;
    }

    internal override bool Contains_Data() => true;
    public override bool Save(ChunkSaveClass csave)
    {
        // TODO: Investigate if following line is necessary
        //WWMEMLOG(MEM_GAMEDATA);

        bool retval = true;

        //
        //	Create a chunk to contain the class variables we need to serialize.
        //
        csave.Begin_Chunk(CHUNKID_VARIABLES);
        Save_Variables(csave);
        csave.End_Chunk();

        //
        //	Have the base class write the objects to their own chunk.
        //
        csave.Begin_Chunk(CHUNKID_OBJECTS);
        retval &= Save_Objects(csave);
        csave.End_Chunk();

        return retval;
    }

    public override bool Load(ChunkLoadClass cload)
    {
        // TODO: Investigate if following line is necessary
        //WWMEMLOG(MEM_GAMEDATA);
        bool retval = true;

        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {

                //
                //	If this is the chunk that contains the class variables, then
                // loop through and read each microchunk
                //
                case CHUNKID_VARIABLES:
                    retval &= Load_Variables(cload);
                    break;

                //
                //	Load all the definition objects from this chunk
                //
                case CHUNKID_OBJECTS:
                    retval &= Load_Objects(cload);
                    break;
            }

            cload.Close_Chunk();
        }

        return retval;
    }
    internal override string Name() { return "DefinitionMgrClass"; }
    protected bool Save_Objects(ChunkSaveClass csave)
    {
        // Save in original file order, interleaving known and unknown definitions.
        foreach (var item in _fileOrderedItems)
        {
            switch (item)
            {
                case DefinitionClass def when def.Is_Save_Enabled():
                    csave.Begin_Chunk(def.Get_Factory().Chunk_ID());
                    def.Get_Factory().Save(csave, def);
                    csave.End_Chunk();
                    break;
                case UnknownChunk unk:
                    csave.Begin_Chunk(unk.ChunkId);
                    unk.Save(csave);
                    csave.End_Chunk();
                    break;
            }
        }

        return true;
    }

    public static IReadOnlyList<object> FileOrderedItems => _fileOrderedItems;
    private static readonly List<object> _fileOrderedItems = [];  // DefinitionClass | UnknownChunk, in file order

    private readonly List<UnknownChunk> _unknownDefinitions = [];
    protected bool Load_Objects(ChunkLoadClass cload)
    {
        bool retval = true;

        _fileOrderedItems.Clear();
        Dictionary<uint, int> definitionCounts = [];

        while (cload.Open_Chunk())
        {
            //Console.WriteLine($"Loading definition chunk ID: {cload.Cur_Chunk_ID}");
            definitionCounts.TryGetValue(cload.Cur_Chunk_ID, out int count);
            definitionCounts[cload.Cur_Chunk_ID] = count + 1;
            //
            //	Load this definition from the chunk (if possible)
            //
            PersistFactoryClass? factory = SaveLoadSystemClass.Find_Persist_Factory(cload.Cur_Chunk_ID);
            if (factory is not null)
            {

                DefinitionClass definition = (DefinitionClass)factory.Load(cload);
                if (definition is not null)
                {

                    //
                    //	Add this definition to our array
                    //
                    Prepare_Definition_Array();
                    _SortedDefinitionArray.Add(definition);
                    _fileOrderedItems.Add(definition);
                    _DefinitionCount++;
                    var id = definition.Get_ID();
                }
            }
            else
            {
                UnknownChunk unknownDef = new();
                unknownDef.Load(cload);
                _unknownDefinitions.Add(unknownDef);
                _fileOrderedItems.Add(unknownDef);
            }

            cload.Close_Chunk();
        }

        //
        //	Sort the definitions
        //
        _SortedDefinitionArray.Sort(fnCompareDefinitionsCallback);

        //
        //	Assign a mgr link to each definition
        //
        for (int index = 0; index < _DefinitionCount; index++)
        {
            _SortedDefinitionArray[index].m_DefinitionMgrLink = index;
        }

        int totalDefinitions = 0;
        int parseDefinitions = 0;

        var sortedDefinitionCounts = definitionCounts.OrderBy(kvp => kvp.Key);
        foreach (var kvp in sortedDefinitionCounts)
        {
            var name = RegisteredDefinitions.TryGetValue(kvp.Key, out string? value) ? $"{value} {kvp.Key}" : kvp.Key.ToString();
            Console.WriteLine($"Loaded {kvp.Value} definitions of Chunk ID: {name}");
            if (value is not null) parseDefinitions += kvp.Value;
            totalDefinitions += kvp.Value;
        }

        Console.WriteLine($"Loaded {totalDefinitions} definitions, with {parseDefinitions} successfully parsed and {totalDefinitions - parseDefinitions} unknown.");

        return retval;
    }
    protected bool Save_Variables(ChunkSaveClass csave)
    {
        bool retval = true;
        return retval;
    }
    protected bool Load_Variables(ChunkLoadClass cload)
    {
        bool retval = true;

        while (cload.Open_Micro_Chunk())
        {
            switch (cload.Cur_Micro_Chunk_ID)
            {

                case VARID_NEXTDEFID:
                    break;
            }

            cload.Close_Micro_Chunk();
        }

        return retval;
    }

    private static readonly Dictionary<string, List<DefinitionClass>> DefinitionHash = [];
    private static void Prepare_Definition_Array() { }
    private static int fnCompareDefinitionsCallback(DefinitionClass definition1, DefinitionClass definition2)
    {
        Debug.Assert(definition1 is not null);
        Debug.Assert(definition2 is not null);

        //
        //	Sort the definitions based on ID
        //
        int result;
        if (definition1.Get_ID() > definition2.Get_ID())
        {
            result = 1;
        }
        else if (definition1.Get_ID() < definition2.Get_ID())
        {
            result = -1;
        }
        else
        {
            result = 0;
        }

        return result;
    }


    private static readonly List<DefinitionClass> _SortedDefinitionArray = [];
    private static int _DefinitionCount = 0;

    private const int DEFINTION_LIST_GROW_SIZE = 1000;
    private const uint IDRANGE_PER_CLASS = 10000;

    private const int CHUNKID_VARIABLES = 0x00000100;
    private const int CHUNKID_OBJECTS = 0x00000101;
    private const int CHUNKID_OBJECT = 0x00000102;
    private const int VARID_NEXTDEFID = 0x01;

    private const int ID_CLASS = 1;
    private const int ID_SUPERCLASS = 2;

    public static readonly Dictionary<uint, string> RegisteredDefinitions = [];

    public const uint GetChunkId = ChunkId.CHUNKID_SAVELOAD_DEFMGR;
};


