using System.Numerics;
using RenData.ChunkIO;
using RenData.Definitions;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.EditorDef;

// Ported from Code/Tools/LevelEdit/LightDefinition.cpp
[RegisterDefinition(ChunkId.CHUNKID_LIGHT_DEF)]
public partial class LightDefinitionClass : DefinitionClass
{
    // LightClass::LightType
    public const int POINT = 0;
    public const int DIRECTIONAL = 1;
    public const int SPOT = 2;

    public LightDefinitionClass()
    {
        m_CastsShadows = false;
        m_AmbientColor = new Vector3(0.7f, 0.7f, 0.7f);
        m_DiffuseColor = new Vector3(0.7f, 0.7f, 0.7f);
        m_SpecularColor = new Vector3(0.7f, 0.7f, 0.7f);
        m_Intensity = 1.0f;
        m_FarRadiusInner = 10.0f;
        m_FarRadiusOuter = 20.0f;
        m_SpotDir = new Vector3(0, 0, 0);
        m_SpotAngle = 0;
        m_SpotExp = 0;
        m_LightType = POINT;
    }

    public override uint Get_Class_ID() => (uint)ClassId.CLASSID_LIGHT;

    public override PersistClass Create()
    {
        // return new LightNodeClass ();
        throw new NotImplementedException();
    }

    public override bool Save(ChunkSaveClass csave)
    {
        bool retval = true;

        csave.Begin_Chunk(CHUNKID_VARIABLES);
        retval &= Save_Variables(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_BASE_CLASS);
        retval &= base.Save(csave);
        csave.End_Chunk();

        return retval;
    }

    public override bool Load(ChunkLoadClass cload)
    {
        bool retval = true;

        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {
                case CHUNKID_VARIABLES:
                    retval &= Load_Variables(cload);
                    break;

                case CHUNKID_BASE_CLASS:
                    retval &= base.Load(cload);
                    break;

                default:
                    Console.WriteLine("Unrecognized LightDef chunkID\n");
                    break;
            }

            cload.Close_Chunk();
        }

        return retval;
    }

    private bool Save_Variables(ChunkSaveClass csave)
    {
        csave.WriteMicro(VARID_CASTS_SHADOWS, m_CastsShadows);
        csave.WriteMicro(VARID_AMBIENT_COLOR, m_AmbientColor);
        csave.WriteMicro(VARID_DIFFUSE_COLOR, m_DiffuseColor);
        csave.WriteMicro(VARID_SPECULAR_COLOR, m_SpecularColor);
        csave.WriteMicro(VARID_INTENSITY, m_Intensity);
        csave.WriteMicro(VARID_INNER_RADIUS, m_FarRadiusInner);
        csave.WriteMicro(VARID_OUTER_RADIUS, m_FarRadiusOuter);
        csave.WriteMicro(VARID_LIGHT_TYPE, m_LightType);
        csave.WriteMicro(VARID_LIGHT_SPOT_ANGLE, m_SpotAngle);
        csave.WriteMicro(VARID_LIGHT_SPOT_EXPONENT, m_SpotExp);
        csave.WriteMicro(VARID_LIGHT_SPOT_DIRECTION, m_SpotDir);
        return true;
    }

    private bool Load_Variables(ChunkLoadClass cload)
    {
        //
        //	Loop through all the microchunks that define the variables
        //
        while (cload.Open_Micro_Chunk())
        {
            switch (cload.Cur_Micro_Chunk_ID)
            {
                case VARID_CASTS_SHADOWS:
                    cload.Read(ref m_CastsShadows);
                    break;
                case VARID_AMBIENT_COLOR:
                    cload.Read(ref m_AmbientColor);
                    break;
                case VARID_DIFFUSE_COLOR:
                    cload.Read(ref m_DiffuseColor);
                    break;
                case VARID_SPECULAR_COLOR:
                    cload.Read(ref m_SpecularColor);
                    break;
                case VARID_INTENSITY:
                    cload.Read(ref m_Intensity);
                    break;
                case VARID_INNER_RADIUS:
                    cload.Read(ref m_FarRadiusInner);
                    break;
                case VARID_OUTER_RADIUS:
                    cload.Read(ref m_FarRadiusOuter);
                    break;
                case VARID_LIGHT_TYPE:
                    cload.Read(ref m_LightType);
                    break;
                case VARID_LIGHT_SPOT_ANGLE:
                    cload.Read(ref m_SpotAngle);
                    break;
                case VARID_LIGHT_SPOT_EXPONENT:
                    cload.Read(ref m_SpotExp);
                    break;
                case VARID_LIGHT_SPOT_DIRECTION:
                    cload.Read(ref m_SpotDir);
                    break;
            }

            cload.Close_Micro_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    // Light definition specific
    public bool Does_Cast_Shadows() => m_CastsShadows;
    public Vector3 Get_Ambient_Color() => m_AmbientColor;
    public Vector3 Get_Diffuse_Color() => m_DiffuseColor;
    public Vector3 Get_Specular_Color() => m_SpecularColor;
    public float Get_Intensity() => m_Intensity;
    public float Get_Inner_Radius() => m_FarRadiusInner;
    public float Get_Outer_Radius() => m_FarRadiusOuter;
    public int Get_Light_Type() => m_LightType;
    public Vector3 Get_Spot_Direction() => m_SpotDir;
    public float Get_Spot_Angle() => m_SpotAngle;
    public float Get_Spot_Exponent() => m_SpotExp;

    private Vector3 m_AmbientColor;
    private Vector3 m_DiffuseColor;
    private Vector3 m_SpecularColor;
    private Vector3 m_SpotDir;
    private float m_SpotAngle;
    private float m_SpotExp;
    private float m_Intensity;
    private float m_FarRadiusInner;
    private float m_FarRadiusOuter;
    private bool m_CastsShadows;
    private int m_LightType;

    private const uint CHUNKID_VARIABLES = 0x00000100;
    private const uint CHUNKID_BASE_CLASS = 0x00000200;

    private const byte VARID_CASTS_SHADOWS = 0x01;
    private const byte VARID_AMBIENT_COLOR = 0x02;
    private const byte VARID_DIFFUSE_COLOR = 0x03;
    private const byte VARID_SPECULAR_COLOR = 0x04;
    private const byte VARID_INTENSITY = 0x05;
    private const byte VARID_INNER_RADIUS = 0x06;
    private const byte VARID_OUTER_RADIUS = 0x07;
    private const byte VARID_LIGHT_TYPE = 0x08;
    private const byte VARID_LIGHT_SPOT_ANGLE = 0x09;
    private const byte VARID_LIGHT_SPOT_EXPONENT = 0x0A;
    private const byte VARID_LIGHT_SPOT_DIRECTION = 0x0B;
}
