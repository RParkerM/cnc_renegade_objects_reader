using RenData.ChunkIO;
using RenData.SaveLoad;
using RenData.IDs;
using RenData.Definitions;

namespace RenData.PhysDef;

[RegisterDefinition(ChunkId.PHYSICS_CHUNKID_DYNAMICANIMPHYSDEF)]
public partial class DynamicAnimPhysDefClass : DecorationPhysDefClass
{
    public DynamicAnimPhysDefClass()
    {
        // moved initializer from original free-function
        AnimManagerDef = new AnimCollisionManagerDefClass();
        CastsShadows = false;
        ShadowNearZ = -1.0f;
        ShadowFarZ = -1.0f;

        // Make the animation manager variables editable
        //ANIMCOLLISIONMANAGERDEF_EDITABLE_PARAMS(DynamicAnimPhysDefClass, AnimManagerDef);
        //PARAM_SEPARATOR(DynamicAnimPhysDefClass, "Shadow Settings");
        //EDITABLE_PARAM(DynamicAnimPhysDefClass, ParameterClass.TYPE_BOOL, CastsShadows);
        //FLOAT_UNITS_PARAM(DynamicAnimPhysDefClass, ShadowNearZ, -1.0f, 1000.0f, "meters (-1 for default)")
        //FLOAT_UNITS_PARAM(DynamicAnimPhysDefClass, ShadowFarZ, -1.0f, 1000.0f, "meters (-1 for default)")
    }

    // From DefinitionClass
    public override uint Get_Class_ID() => ClassId.CLASSID_DYNAMICANIMPHYSDEF;

    public override PersistClass Create()
    {
        // Original creation code (commented out and left as a reference):
        // DynamicAnimPhysClass obj = NEW_REF(DynamicAnimPhysClass, ());
        // obj.Init(this);
        // return obj;
        throw new NotImplementedException();
    }

    // From PhysDefClass
    public override string Get_Type_Name() => "DynamicAnimPhysDef";
    public override bool Is_Type(string type_name)
    {
        if (string.Compare(type_name, Get_Type_Name()) == 0)
        {
            return true;
        }
        else
        {
            return base.Is_Type(type_name);
        }
    }

    // From PersistClass
    public override PersistFactoryClass Get_Factory() => _persistFactory;

    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(DYNAMICANIMPHYSDEF_CHUNK_DECOPHYSDEF);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(DYNAMICANIMPHYSDEF_CHUNK_ANIMMANAGERDEF);
        AnimManagerDef.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(DYNAMICANIMPHYSDEF_CHUNK_VARIABLES);
        csave.WriteMicro(DYNAMICANIMPHYSDEF_VARIABLE_CASTSSHADOWS, CastsShadows);
        csave.WriteMicro(DYNAMICANIMPHYSDEF_VARIABLE_SHADOWNEARZ, ShadowNearZ);
        csave.WriteMicro(DYNAMICANIMPHYSDEF_VARIABLE_SHADOWFARZ, ShadowFarZ);
        csave.End_Chunk();

        return true;
    }

    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {

            switch (cload.Cur_Chunk_ID)
            {

                case DYNAMICANIMPHYSDEF_CHUNK_DECOPHYSDEF:
                    base.Load(cload);
                    break;

                case DYNAMICANIMPHYSDEF_CHUNK_ANIMMANAGERDEF:
                    AnimManagerDef.Load(cload);
                    break;

                case DYNAMICANIMPHYSDEF_CHUNK_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case DYNAMICANIMPHYSDEF_VARIABLE_CASTSSHADOWS:
                                cload.Read(ref CastsShadows);
                                break;
                            case DYNAMICANIMPHYSDEF_VARIABLE_SHADOWNEARZ:
                                cload.Read(ref ShadowNearZ);
                                break;
                            case DYNAMICANIMPHYSDEF_VARIABLE_SHADOWFARZ:
                                cload.Read(ref ShadowFarZ);
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unhandled Chunk: 0x{0:X8} (depth {1}) in {2}", cload.Cur_Chunk_ID, cload.Cur_Chunk_Depth, nameof(DynamicAnimPhysDefClass));
                    break;
            }

            cload.Close_Chunk();
        }
        return true;
    }


    //	Editable interface requirements
    //DECLARE_EDITABLE(DynamicAnimPhysDefClass, DecorationPhysDefClass);


    // Animation and animated collision support
    protected AnimCollisionManagerDefClass AnimManagerDef;

    protected bool CastsShadows;
    protected float ShadowNearZ;
    protected float ShadowFarZ;

    // Replaced file-scope enum with private consts scoped to the class
    // Octal literals converted to hexadecimal equivalents
    private const uint DYNAMICANIMPHYSDEF_CHUNK_DECOPHYSDEF = 0xAB00CEu; // (parent class) == octal 052600316
    private const uint DYNAMICANIMPHYSDEF_CHUNK_ANIMMANAGERDEF = 0xAB00CFu; // == octal 052600317
    private const uint DYNAMICANIMPHYSDEF_CHUNK_VARIABLES = 0xAB00D0u; // == octal 052600318

    private const int DYNAMICANIMPHYSDEF_VARIABLE_CASTSSHADOWS = 0x01;
    private const int DYNAMICANIMPHYSDEF_VARIABLE_SHADOWNEARZ = 0x02;
    private const int DYNAMICANIMPHYSDEF_VARIABLE_SHADOWFARZ = 0x03;
}



// The following original C++-style free-function constructor and Create() implementation
// have been commented out because their logic was moved/recorded in the class above.

/*
DynamicAnimPhysDefClass.DynamicAnimPhysDefClass() :
	CastsShadows(false),
	ShadowNearZ(-1.0f),
	ShadowFarZ(-1.0f)
{
    // Make the animation manager variables editable
    //ANIMCOLLISIONMANAGERDEF_EDITABLE_PARAMS(DynamicAnimPhysDefClass, AnimManagerDef);
    //PARAM_SEPARATOR(DynamicAnimPhysDefClass, "Shadow Settings");
    //EDITABLE_PARAM(DynamicAnimPhysDefClass, ParameterClass.TYPE_BOOL, CastsShadows);
    //FLOAT_UNITS_PARAM(DynamicAnimPhysDefClass, ShadowNearZ, -1.0f, 1000.0f, "meters (-1 for default)")
    //FLOAT_UNITS_PARAM(DynamicAnimPhysDefClass, ShadowFarZ, -1.0f, 1000.0f, "meters (-1 for default)")
}

PersistClass DynamicAnimPhysDefClass.Create()
{
    DynamicAnimPhysClass obj = NEW_REF(DynamicAnimPhysClass, ();
    obj.Init(this);
    return obj;
}
*/



