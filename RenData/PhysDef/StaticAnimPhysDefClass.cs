using RenData.ChunkIO;
using RenData.Definitions;
using RenData.IDs;
using RenData.SaveLoad;
using System.Reflection.Metadata;

namespace RenData.PhysDef;

[RegisterDefinition(ChunkId.PHYSICS_CHUNKID_STATICANIMPHYSDEF)]
public partial class StaticAnimPhysDefClass : StaticPhysDefClass
{

    public StaticAnimPhysDefClass()
    {
        ShadowDynamicObjs = false;
        ShadowIsAdditive = false;
        ShadowIgnoresZRotation = true;
        ShadowNearZ = 0.5f;
        ShadowFarZ = 5.0f;
        ShadowIntensity = 0.5f;
        DoesCollideInPathfind = false;
        IsCosmetic = false;

        // Add the misc flags to the editable interface
        //EDITABLE_PARAM(StaticAnimPhysDefClass, ParameterClass.TYPE_BOOL, IsCosmetic);
        //EDITABLE_PARAM(StaticAnimPhysDefClass, ParameterClass.TYPE_BOOL, DoesCollideInPathfind);

        // Make the animation manager variables editable
        //ANIMCOLLISIONMANAGERDEF_EDITABLE_PARAMS(StaticAnimPhysDefClass, AnimManagerDef);

        // Make the projector manager variables editable
        //PROJECTORMANAGERDEF_EDITABLE_PARAMS(StaticAnimPhysDefClass, ProjectorManagerDef);

        // make the shadow parameters editable
        //PARAM_SEPARATOR(StaticAnimPhysDefClass, "Shadow Settings");
        //EDITABLE_PARAM(StaticAnimPhysDefClass, ParameterClass.TYPE_BOOL, ShadowDynamicObjs);
        //EDITABLE_PARAM(StaticAnimPhysDefClass, ParameterClass.TYPE_BOOL, ShadowIsAdditive);
        //EDITABLE_PARAM(StaticAnimPhysDefClass, ParameterClass.TYPE_BOOL, ShadowIgnoresZRotation);
        //FLOAT_EDITABLE_PARAM(StaticAnimPhysDefClass, ShadowNearZ, 1.0f, 100.0f);
        //FLOAT_EDITABLE_PARAM(StaticAnimPhysDefClass, ShadowFarZ, 1.0f, 100.0f);
        //FLOAT_EDITABLE_PARAM(StaticAnimPhysDefClass, ShadowIntensity, 0.0f, 1.0f);
    }

    // From DefinitionClass
    public override uint Get_Class_ID() => ClassId.CLASSID_STATICANIMPHYSDEF;

    public override PersistClass Create()
    {
        throw new NotImplementedException();
        //StaticAnimPhysClass obj = NEW_REF(StaticAnimPhysClass, ());
        //obj.Init(this);
        //return obj;
    }

    // From PhysDefClass
    public override string Get_Type_Name() => "StaticAnimPhysDef";
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
        csave.Begin_Chunk(STATICANIMPHYSDEF_CHUNK_STATICPHYSDEF);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(STATICANIMPHYSDEF_CHUNK_PROJECTORMANAGERDEF);
        ProjectorManagerDef.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(STATICANIMPHYSDEF_CHUNK_ANIMMANAGERDEF);
        AnimManagerDef.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(STATICANIMPHYSDEF_CHUNK_VARIABLES);
        csave.WriteMicro(STATICANIMPHYSDEF_VARIABLE_SHADOWDYNAMICOBJS, ShadowDynamicObjs);
        csave.WriteMicro(STATICANIMPHYSDEF_VARIABLE_SHADOWISADDITIVE, ShadowIsAdditive);
        csave.WriteMicro(STATICANIMPHYSDEF_VARIABLE_SHADOWIGNORESZROTATION, ShadowIgnoresZRotation);
        csave.WriteMicro(STATICANIMPHYSDEF_VARIABLE_SHADOWFARZ, ShadowFarZ);
        csave.WriteMicro(STATICANIMPHYSDEF_VARIABLE_SHADOWINTENSITY, ShadowIntensity);
        csave.WriteMicro(STATICANIMPHYSDEF_VARIABLE_SHADOWNEARZ, ShadowNearZ);
        csave.WriteMicro(STATICANIMPHYSDEF_VARIABLE_COLLIDEINPATHFIND, DoesCollideInPathfind);
        csave.WriteMicro(STATICANIMPHYSDEF_VARIABLE_ISCOSMETIC, IsCosmetic);
        csave.End_Chunk();

        return true;
    }
    public override bool Load(ChunkLoadClass cload) {
        while (cload.Open_Chunk())
        {

            switch (cload.Cur_Chunk_ID)
            {

                case STATICANIMPHYSDEF_CHUNK_STATICPHYSDEF:
                    base.Load(cload);
                    break;

                case STATICANIMPHYSDEF_CHUNK_PROJECTORMANAGERDEF:
                    ProjectorManagerDef.Load(cload);
                    break;

                case STATICANIMPHYSDEF_CHUNK_ANIMMANAGERDEF:
                    AnimManagerDef.Load(cload);
                    break;

                case STATICANIMPHYSDEF_CHUNK_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            // NOTE: these two variables/microchunks have been moved into the animation manager.
                            case STATICANIMPHYSDEF_VARIABLE_COLLISIONMODE:
                                cload.Read(ref AnimManagerDef.CollisionMode);
                                break;
                            case STATICANIMPHYSDEF_VARIABLE_ANIMATIONNAME:
                                cload.ReadMicroChunkWWString(out AnimManagerDef.AnimationName);
                                break;

                            case STATICANIMPHYSDEF_VARIABLE_SHADOWDYNAMICOBJS:
                                cload.Read(ref ShadowDynamicObjs);
                                break;
                            case STATICANIMPHYSDEF_VARIABLE_SHADOWISADDITIVE:
                                cload.Read(ref ShadowIsAdditive);
                                break;
                            case STATICANIMPHYSDEF_VARIABLE_SHADOWIGNORESZROTATION:
                                cload.Read(ref ShadowIgnoresZRotation);
                                break;
                            case STATICANIMPHYSDEF_VARIABLE_SHADOWFARZ:
                                cload.Read(ref ShadowFarZ);
                                break;
                            case STATICANIMPHYSDEF_VARIABLE_SHADOWINTENSITY:
                                cload.Read(ref ShadowIntensity);
                                break;
                            case STATICANIMPHYSDEF_VARIABLE_SHADOWNEARZ:
                                cload.Read(ref ShadowNearZ);
                                break;
                            case STATICANIMPHYSDEF_VARIABLE_COLLIDEINPATHFIND:
                                cload.Read(ref DoesCollideInPathfind);
                                break;
                            case STATICANIMPHYSDEF_VARIABLE_ISCOSMETIC:
                                cload.Read(ref IsCosmetic);
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unhandled Chunk: 0x{0:X8} (depth {1}) in {2}", cload.Cur_Chunk_ID, cload.Cur_Chunk_Depth, nameof(StaticAnimPhysDefClass));
                    break;
            }

            cload.Close_Chunk();
        }
        return true;
    }

    //	Editable interface requirements
    //DECLARE_EDITABLE(StaticAnimPhysDefClass, StaticPhysDefClass);

    // Accessors
    public bool Shadow_Dynamic_Objs() { return ShadowDynamicObjs; }
    public bool Shadow_Is_Additive() { return ShadowIsAdditive; }
    public bool Shadow_Ignores_Z_Rotation() { return ShadowIgnoresZRotation; }
    public float Shadow_NearZ() { return ShadowNearZ; }
    public float Shadow_FarZ() { return ShadowFarZ; }
    public float Shadow_Intensity() { return ShadowIntensity; }

    public bool Does_Collide_In_Pathfind() { return DoesCollideInPathfind; }

    protected bool IsCosmetic;

    // Animation and animated collision support
    protected AnimCollisionManagerDefClass AnimManagerDef = new();

    // Animated projector support
    protected ProjectorManagerDefClass ProjectorManagerDef = new();

    // Static shadow support 
    protected bool ShadowDynamicObjs;
    protected bool ShadowIsAdditive;
    protected bool ShadowIgnoresZRotation;
    protected float ShadowNearZ;
    protected float ShadowFarZ;
    protected float ShadowIntensity;

    // Pathfind support
    protected bool DoesCollideInPathfind;

    // Replaced file-scope enum with private const ints scoped to the class
    private const uint STATICANIMPHYSDEF_CHUNK_STATICPHYSDEF = 0x055110100u; // (parent class)
    private const uint STATICANIMPHYSDEF_CHUNK_PROJECTORMANAGERDEF = 0x055110101u;
    private const uint STATICANIMPHYSDEF_CHUNK_VARIABLES = 0x055110102u;
    private const uint STATICANIMPHYSDEF_CHUNK_ANIMMANAGERDEF = 0x055110103u;

    private const int STATICANIMPHYSDEF_VARIABLE_COLLISIONMODE = 0x00;
    private const int STATICANIMPHYSDEF_VARIABLE_SHADOWDYNAMICOBJS = 0x01;
    private const int STATICANIMPHYSDEF_VARIABLE_SHADOWISADDITIVE = 0x02;
    private const int STATICANIMPHYSDEF_VARIABLE_SHADOWIGNORESZROTATION = 0x03;
    private const int STATICANIMPHYSDEF_VARIABLE_SHADOWFARZ = 0x04;
    private const int STATICANIMPHYSDEF_VARIABLE_SHADOWINTENSITY = 0x05;
    private const int STATICANIMPHYSDEF_VARIABLE_SHADOWNEARZ = 0x06;
    private const int STATICANIMPHYSDEF_VARIABLE_ANIMATIONNAME = 0x07;
    private const int STATICANIMPHYSDEF_VARIABLE_COLLIDEINPATHFIND = 0x08;
    private const int STATICANIMPHYSDEF_VARIABLE_ISCOSMETIC = 0x09;
};

