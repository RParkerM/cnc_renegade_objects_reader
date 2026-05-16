//using RenData.ChunkIO;
//using RenData.SaveLoad;

//namespace ObjectsReader.Managers;
//#warning Review implementation
//internal class Preset : PersistClass
//{
//    public bool m_IsTemporary { get; private set; } = false;
//    public uint m_DefinitionID { get; private set; } = 0;
//    public List<string> m_ManualDependencies { get; private set; } = [];
//    public string m_Comments { get; private set; } = string.Empty;
//    public uint m_ParentPtr { get; private set; } = 0;
//    public uint m_ParentID { get; private set; } = 0;

//    public uint old_object_ptr { get; private set; } = 0;
//    public uint Old_Object_Pointer { get; init; }

//    public void Load(ChunkLoadClass chunkLoad)
//    {
//        while (chunkLoad.Open_Chunk())
//        {
//            switch (chunkLoad.Cur_Chunk_ID)
//            {
//                case (uint)ChunkId.COMMENTS:
//                    m_Comments = chunkLoad.ReadWWString();
//                    break;
//                case (uint)ChunkId.VARIABLES:
//                    LoadVariables(chunkLoad);
//                    break;
//                default:
//                    Console.WriteLine($"Unknown Preset Chunk ID: {chunkLoad.Cur_Chunk_ID} Size {chunkLoad.Cur_Chunk_Length}");
//                    break;
//            }
//            if (!chunkLoad.EndOfChunk)
//            {
//                Console.WriteLine($"Missed End of Chunk: {chunkLoad.Cur_Chunk_ID} Size {chunkLoad.Cur_Chunk_Length}");
//            }
//            chunkLoad.Close_Chunk();
//        }
//    }

//    private void LoadPreset(ChunkLoadClass chunkLoad)
//    {
        

//    }


//    private bool LoadVariables(ChunkLoadClass chunkLoad)
//    {
//        {
//            m_DefinitionID = 0;
//            int old_this_ptr = 0;
//            m_ManualDependencies.Clear();

//            //
//            //	Loop through all the microchunks that define the variables
//            //
//            while (chunkLoad.Open_Micro_Chunk())
//            {
//                switch (chunkLoad.Cur_Micro_Chunk_ID)
//                {
//                    case (uint)MicroChunkId.VARID_DEFINITIONID:
//                        /// TODO FIX THIS
//                        m_DefinitionID = chunkLoad.Read<uint>();
//                        break;
//                    case (uint)MicroChunkId.VARID_ISTEMPORARY:
//                        m_IsTemporary = chunkLoad.Read<bool>();
//                        break;
//                    case (uint)MicroChunkId.VARID_COMMENTS:
//                        m_Comments = chunkLoad.ReadMicroChunkWWString();
//                        break;
//                    case (uint)MicroChunkId.VARID_PARENTPTR:
//                        m_ParentPtr = chunkLoad.Read<uint>();
//                        break;
//                    case (uint)MicroChunkId.VARID_THISPTR:
//                        old_this_ptr = chunkLoad.Read<int>();
//                        break;
//                    case (uint)MicroChunkId.VARID_PARENT_ID:
//                        m_ParentID = chunkLoad.Read<uint>();
//                        break;
//                    case (uint)MicroChunkId.VARID_MANUALDEPENDENCY:
//                        var filename = chunkLoad.ReadMicroChunkWWString();
//                        m_ManualDependencies.Add(filename);

//                        break;
//                    default:
//                        Console.WriteLine($"Unknown MicroChunk Id{chunkLoad.Cur_Micro_Chunk_ID} Size {chunkLoad.Cur_Micro_Chunk_Length}");
//                        break;
//                }

//                chunkLoad.Close_Micro_Chunk();
//            }

//            ////
//            ////	Check for a recursive linkage (could happen with temp presets if the
//            //// user deletes his registry)...
//            ////
//            //if (m_ParentID == Get_ID())
//            //{
//            //    m_IsValid = false;
//            //}

//            ////
//            ////	Handle pointer remapping.  LEGACY CODE -- IS NOW OBSOLETE.
//            ////
//            //WWASSERT(old_this_ptr != NULL);
//            //SaveLoadSystemClass::Register_Pointer(old_this_ptr, this);
//            //if (m_ParentPtr != NULL)
//            //{
//            //    REQUEST_POINTER_REMAP((void**)&m_Parent);
//            //}

//            ////
//            ////	Lookup the definition pointer from the definition id
//            ////
//            //WWASSERT(m_DefinitionID != 0);
//            //m_Definition = DefinitionMgrClass::Find_Definition(m_DefinitionID, false);

//            ////
//            ////	Associate this preset with the definition
//            ////
//            //if (m_Definition != NULL)
//            //{
//            //    m_Definition->Set_User_Data((uint32)this);
//            //}

//            //if (m_DefinitionID == 0 || m_Definition == NULL)
//            //{
//            //    int test = 0;
//            //}

//            return true;
//        }
//    }

//    //void PersistClass.Load(ChunkLoadClass chunkLoad)
//    //{
//    //    throw new NotImplementedException();
//    //}

//    public void Save(ChunkSaveClass chunkSave)
//    {
//        throw new NotImplementedException();
//    }

//    enum ChunkId
//    {
//        VARIABLES = 0x00000100,
//        COMMENTS,
//        XX_CHUNKID_NODES
//    };

//    enum MicroChunkId
//    {
//        VARID_DEFINITIONID = 0x01,
//        VARID_ISTEMPORARY,
//        VARID_COMMENTS,
//        VARID_PARENTPTR,
//        VARID_THISPTR,
//        XXX_VARID_FILEDEPENDENCY,
//        VARID_PARENT_ID,
//        VARID_MANUALDEPENDENCY
//    };
//}
