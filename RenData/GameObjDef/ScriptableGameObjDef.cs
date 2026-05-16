using RenData.ChunkIO;
using System.Diagnostics;

namespace RenData.GameObjDef;

public abstract class ScriptableGameObjDef : BaseGameObjDef
{
    const int CHUNKID_DEF_PARENT = 627001056;
    const int CHUNKID_DEF_VARIABLES = 627001057;
    const int XXX_MICROCHUNKID_DEF_TYPE = 1;
    const int MICROCHUNKID_DEF_SCRIPT_NAME = 2;
    const int MICROCHUNKID_DEF_SCRIPT_PARAMETERS = 3;

    public ScriptableGameObjDef()
    {
        //SCRIPTLIST_PARAM(ScriptableGameObjDef, "Scripts", ScriptNameList, ScriptParameterList);
    }

    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_DEF_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DEF_VARIABLES);
        Debug.Assert(ScriptNameList.Count == ScriptParameterList.Count);
        for (int i = 0; i < ScriptNameList.Count; i++)
        {
            csave.WriteMicroString(MICROCHUNKID_DEF_SCRIPT_NAME, ScriptNameList[i]);
            csave.WriteMicroString(MICROCHUNKID_DEF_SCRIPT_PARAMETERS, ScriptParameterList[i]);
        }
        csave.End_Chunk();

        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        Debug.Assert(ScriptNameList.Count == ScriptParameterList.Count);
        string str;
        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {
                case CHUNKID_DEF_PARENT:
                    base.Load(cload);
                    break;

                case CHUNKID_DEF_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {

                            case MICROCHUNKID_DEF_SCRIPT_NAME:
                                str = cload.ReadMicroChunkWWString();
                                ScriptNameList.Add(str);
                                break;

                            case MICROCHUNKID_DEF_SCRIPT_PARAMETERS:
                                str = cload.ReadMicroChunkWWString();
                                ScriptParameterList.Add(str);
                                break;

                            default:
                                Console.WriteLine($"Unhandled Chunk:{cload.Cur_Chunk_ID} File:%s Line:%d\r\n");
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine($"Unhandled Chunk:{cload.Cur_Chunk_ID} File:%s Line:%d\r\n");
                    break;

            }
            cload.Close_Chunk();
        }
        Debug.Assert(ScriptNameList.Count == ScriptParameterList.Count);
        return true;
    }

    //DECLARE_EDITABLE(ScriptableGameObjDef, BaseGameObjDef );

    protected List<string> ScriptNameList = [];
    protected List<string> ScriptParameterList = [];
}
