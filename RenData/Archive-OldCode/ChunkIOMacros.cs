namespace ObjectsReader.Macros;

internal class ChunkIOMacros
{
    // /*
    //** Like READ_MICRO_CHUNK but reads items straight into the data safe.
    //*/
    //#define READ_SAFE_MICRO_CHUNK(cload,id,var,type)								\
    //	case (id):	{                                                     \
    //		void* temp_read_buffer_on_the_stack = _alloca(sizeof(type));	\
    //		cload.Read(temp_read_buffer_on_the_stack, sizeof(type));       \
    //		var = *((type*) temp_read_buffer_on_the_stack);                 \
    //		break;                                                         \
    //	}

    //#define READ_MICRO_CHUNK_STRING(cload,id,var,size)		\
    //	case (id):	WWASSERT(cload.Cur_Micro_Chunk_Length() <= size); cload.Read(var, cload.Cur_Micro_Chunk_Length()); break;	\

    //#define READ_MICRO_CHUNK_WWSTRING(cload,id,var)		\
    //	case (id):	cload.Read(var.Get_Buffer(cload.Cur_Micro_Chunk_Length()),cload.Cur_Micro_Chunk_Length()); break;	\

    //#define READ_MICRO_CHUNK_WIDESTRING(cload,id,var)		\
    //	case (id):	cload.Read(var.Get_Buffer((cload.Cur_Micro_Chunk_Length()+1)/2),cload.Cur_Micro_Chunk_Length()); break;	\

    ///*
    //** These load macros make it easier to add extra code to a specifc case
    //*/
    //#define LOAD_MICRO_CHUNK(cload,var)						\
    //	cload.Read(&var,sizeof(var)); \

    //#define LOAD_MICRO_CHUNK_WWSTRING(cload,var)		\
    //	cload.Read(var.Get_Buffer(cload.Cur_Micro_Chunk_Length()),cload.Cur_Micro_Chunk_Length());	\

    //#define LOAD_MICRO_CHUNK_WIDESTRING(cload,var)		\
    //	cload.Read(var.Get_Buffer((cload.Cur_Micro_Chunk_Length()+1)/2),cload.Cur_Micro_Chunk_Length());	\


    /*
** WRITE_MICRO_CHUNK	- use this one-line macro to easily make a micro chunk for an individual variable.
** Note that you should always wrap your micro-chunks inside a normal chunk.
** Example:
**
**	csave.Begin_Chunk(PHYSGRID_CHUNK_VARIABLES);
**	WRITE_MICRO_CHUNK(csave,PHYSGRID_VARIABLE_VERSION,version);
**	WRITE_MICRO_CHUNK(csave,PHYSGRID_VARIABLE_DUMMYVISID,DummyVisId);
**	WRITE_MICRO_CHUNK(csave,PHYSGRID_VARIABLE_BASEVISID,BaseVisId);
**	csave.End_Chunk();
*/
//#define WRITE_MICRO_CHUNK(csave,id,var) { \
//    csave.Begin_Micro_Chunk(id); \
//	csave.Write(&var,sizeof(var)); \
//	csave.End_Micro_Chunk(); }

//#define WRITE_SAFE_MICRO_CHUNK(csave,id,var,type) { \
//csave.Begin_Micro_Chunk(id);		\
//	type data = (type)var;				\
//	csave.Write(&data,sizeof(data)); \
//	csave.End_Micro_Chunk(); }

//#define WRITE_MICRO_CHUNK_STRING(csave,id,var) { \
//	csave.Begin_Micro_Chunk(id); \
//	csave.Write(var, strlen(var) + 1); \
//	csave.End_Micro_Chunk(); }

//#define WRITE_MICRO_CHUNK_WWSTRING(csave,id,var) { \
//	csave.Begin_Micro_Chunk(id); \
//	csave.Write((const TCHAR*)var, var.Get_Length () + 1); \
//	csave.End_Micro_Chunk(); }

//#define WRITE_MICRO_CHUNK_WIDESTRING(csave,id,var) { \
//	csave.Begin_Micro_Chunk(id); \
//	csave.Write((const WCHAR*)var, (var.Get_Length() + 1) * 2); \
//	csave.End_Micro_Chunk(); }
}
