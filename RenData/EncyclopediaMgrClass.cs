namespace RenData;

public class EncyclopediaMgrClass// : SaveLoadSubSystemClass
{
    public enum TYPE
    {
        TYPE_UNKNOWN = -1,
        TYPE_CHARACTER = 0,
        TYPE_WEAPON,
        TYPE_VEHICLE,
        TYPE_BUILDING,
        TYPE_COUNT

    }

    //	///////////////////////////////////////////////////////////////////
    //	//	Public methods
    //	///////////////////////////////////////////////////////////////////

    //	//
    //	//	Initialization methods
    //	//
    //	static void Initialize(void);
    //static void Shutdown(void);

    ////
    ////	Content control
    ////
    //static bool Reveal_Object(DamageableGameObj* game_obj);
    //static bool Reveal_Object(TYPE type, int object_id);
    //static bool Is_Object_Revealed(TYPE type, int object_id);

    //static void Reveal_Objects(TYPE type);
    //static void Reveal_All_Objects(void);
    //static void Hide_Objects(TYPE type);
    //static void Hide_All_Objects(void);

    ////
    ////	Memory persistence
    ////
    //static void Store_Data(void);
    //static void Restore_Data(void);

    ////
    ////	UI
    ////
    //static void Display_Event_UI(void);

    ////
    ////	Inherited
    ////
    //uint32 Chunk_ID(void) const           { return CHUNKID_ENCYCLOPEDIAMGR; }

    //private:

    //	///////////////////////////////////////////////////////////////////
    //	//	Private methods
    //	///////////////////////////////////////////////////////////////////

    //	//
    //	//	Inherited
    //	//
    //	bool Save(ChunkSaveClass &csave);
    //bool Load(ChunkLoadClass &cload);
    //const char* Name (void) const                   { return "EncyclopediaMgrClass"; }

    //	//							
    //	//	Save load support	
    //	//							
    //	void Load_Variables(ChunkLoadClass &cload);

    ////
    ////	Initialization support
    ////
    //static void Build_Bit_Vector(TYPE type);

    /////////////////////////////////////////////////////////////////////
    ////	Private member data
    /////////////////////////////////////////////////////////////////////	
    //static BooleanVectorClass KnownObjectVector[TYPE_COUNT];
    //static BooleanVectorClass CopyOfKnownObjectVector[TYPE_COUNT];
}