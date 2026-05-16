using RenData.ChunkIO;
using RenData.SaveLoad;
using RenData.IDs;

namespace RenData.Definitions;

public class ProjectorManagerDefClass
{

    public ProjectorManagerDefClass()
    {
        IsEnabled = false;
        IsPerspective = false;
        IsAdditive = false;
        IsAnimated = false;
        OrthoWidth = 10.0f;
        OrthoHeight = 10.0f;
        HorizontalFOV = WWMath.DEG_TO_RADF(10.0f);
        VerticalFOV = WWMath.DEG_TO_RADF(10.0f);
        NearZ = 5.0f;
        FarZ = 20.0f;
        Intensity = 1.0f;
    }

    public void Validate_Parameters()
    {
        if (HorizontalFOV <= 0.0f) { HorizontalFOV = WWMath.DEG_TO_RADF(10.0f); }
        if (VerticalFOV <= 0.0f) { VerticalFOV = WWMath.DEG_TO_RADF(10.0f); }
        if (OrthoWidth <= 0.0f) { OrthoWidth = 10.0f; }
        if (OrthoHeight <= 0.0f) { OrthoHeight = 10.0f; }
        if (NearZ < 0.0f) { NearZ = 0.0f; }
        if (FarZ < NearZ) { FarZ = NearZ + 10.0f; }
    }

    public bool Save(ChunkSaveClass csave)
    {

        Validate_Parameters();

        csave.Begin_Chunk(PROJECTORMANAGERDEF_CHUNK_VARIABLES);
        csave.WriteMicro(PROJECTORMANAGERDEF_VARIABLE_ISENABLED, IsEnabled);
        csave.WriteMicro(PROJECTORMANAGERDEF_VARIABLE_ISPERSPECTIVE, IsPerspective);
        csave.WriteMicro(PROJECTORMANAGERDEF_VARIABLE_ISADDITIVE, IsAdditive);
        csave.WriteMicro(PROJECTORMANAGERDEF_VARIABLE_ISANIMATED, IsAnimated);
        csave.WriteMicro(PROJECTORMANAGERDEF_VARIABLE_ORTHOWIDTH, OrthoWidth);
        csave.WriteMicro(PROJECTORMANAGERDEF_VARIABLE_ORTHOHEIGHT, OrthoHeight);
        csave.WriteMicro(PROJECTORMANAGERDEF_VARIABLE_HORIZONTALFOV, HorizontalFOV);
        csave.WriteMicro(PROJECTORMANAGERDEF_VARIABLE_VERTICALFOV, VerticalFOV);
        csave.WriteMicro(PROJECTORMANAGERDEF_VARIABLE_NEARZ, NearZ);
        csave.WriteMicro(PROJECTORMANAGERDEF_VARIABLE_FARZ, FarZ);
        ArgumentNullException.ThrowIfNull(TextureName);
        csave.WriteMicroString(PROJECTORMANAGERDEF_VARIABLE_TEXTURENAME, TextureName);
        ArgumentNullException.ThrowIfNull(BoneName);
        csave.WriteMicroString(PROJECTORMANAGERDEF_VARIABLE_BONENAME, BoneName);
        csave.WriteMicro(PROJECTORMANAGERDEF_VARIABLE_INTENSITY, Intensity);
        csave.End_Chunk();

        return true;
    }
    public bool Load(ChunkLoadClass cload) {
        while (cload.Open_Chunk())
        {

            switch (cload.Cur_Chunk_ID)
            {

                case PROJECTORMANAGERDEF_CHUNK_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case PROJECTORMANAGERDEF_VARIABLE_ISENABLED:
                                cload.Read(ref IsEnabled);
                                break;
                            case PROJECTORMANAGERDEF_VARIABLE_ISPERSPECTIVE:
                                cload.Read(ref IsPerspective);
                                break;
                            case PROJECTORMANAGERDEF_VARIABLE_ISADDITIVE:
                                cload.Read(ref IsAdditive);
                                break;
                            case PROJECTORMANAGERDEF_VARIABLE_ISANIMATED:
                                cload.Read(ref IsAnimated);
                                break;
                            case PROJECTORMANAGERDEF_VARIABLE_ORTHOWIDTH:
                                cload.Read(ref OrthoWidth);
                                break;
                            case PROJECTORMANAGERDEF_VARIABLE_ORTHOHEIGHT:
                                cload.Read(ref OrthoHeight);
                                break;
                            case PROJECTORMANAGERDEF_VARIABLE_HORIZONTALFOV:
                                cload.Read(ref HorizontalFOV);
                                break;
                            case PROJECTORMANAGERDEF_VARIABLE_VERTICALFOV:
                                cload.Read(ref VerticalFOV);
                                break;
                            case PROJECTORMANAGERDEF_VARIABLE_NEARZ:
                                cload.Read(ref NearZ);
                                break;
                            case PROJECTORMANAGERDEF_VARIABLE_FARZ:
                                cload.Read(ref FarZ);
                                break;
                            case PROJECTORMANAGERDEF_VARIABLE_TEXTURENAME:
                                cload.ReadMicroChunkWWString(out TextureName);
                                break;
                            case PROJECTORMANAGERDEF_VARIABLE_BONENAME:
                                cload.ReadMicroChunkWWString(out BoneName);
                                break;
                            case PROJECTORMANAGERDEF_VARIABLE_INTENSITY:
                                cload.Read(ref Intensity);
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unhandled Chunk: 0x{0:X8} (depth {1}) in {2}", cload.Cur_Chunk_ID, cload.Cur_Chunk_Depth, nameof(ProjectorManagerDefClass));
                    break;
            }

            cload.Close_Chunk();
        }
        return true;
    }


    public bool IsEnabled;                     // should this object create a projector
    public bool IsPerspective;                 // is this a perspective projection
    public bool IsAdditive;                        // is this an additive projection
    public bool IsAnimated;                        // is this projector animated (attached to a bone that animates?)
    public float OrthoWidth;                       // width of the orthographic projection
    public float OrthoHeight;                  // height of the orthographic projection
    public float HorizontalFOV;                    // horizontal field of view
    public float VerticalFOV;                  // vertical field of view
    public float NearZ;                            // near clip plane
    public float FarZ;                             // far clip plane
    public float Intensity;                        // intensity of the projector
    public string? TextureName;                 // name of texture to project
    public string? BoneName;                        // name of the bone which should control the projector

    // Replaced enum with private const ints
    private const int PROJECTORMANAGERDEF_CHUNK_VARIABLES = 0x01110004;

    private const int PROJECTORMANAGERDEF_VARIABLE_ISENABLED = 0x00;
    private const int PROJECTORMANAGERDEF_VARIABLE_ISPERSPECTIVE = 0x01;
    private const int PROJECTORMANAGERDEF_VARIABLE_ISADDITIVE = 0x02;
    private const int PROJECTORMANAGERDEF_VARIABLE_ISANIMATED = 0x03;
    private const int PROJECTORMANAGERDEF_VARIABLE_ORTHOWIDTH = 0x04;
    private const int PROJECTORMANAGERDEF_VARIABLE_ORTHOHEIGHT = 0x05;
    private const int PROJECTORMANAGERDEF_VARIABLE_HORIZONTALFOV = 0x06;
    private const int PROJECTORMANAGERDEF_VARIABLE_VERTICALFOV = 0x07;
    private const int PROJECTORMANAGERDEF_VARIABLE_NEARZ = 0x08;
    private const int PROJECTORMANAGERDEF_VARIABLE_FARZ = 0x09;
    private const int PROJECTORMANAGERDEF_VARIABLE_TEXTURENAME = 0x0A;
    private const int PROJECTORMANAGERDEF_VARIABLE_BONENAME = 0x0B;
    private const int PROJECTORMANAGERDEF_VARIABLE_INTENSITY = 0x0C;
};
