using RenData.ChunkIO;
using RenData.SaveLoad;
using RenData.IDs;

namespace RenData.Definitions;

public class AnimCollisionManagerDefClass
{

    public AnimCollisionManagerDefClass()
    {
        CollisionMode = AnimCollisionManagerClass.COLLIDE_PUSH;
        AnimationMode = AnimCollisionManagerClass.ANIMATE_LOOP;
    }

    public void Validate_Parameters()
    {

        if (CollisionMode < 0) CollisionMode = 0;
        if (CollisionMode > AnimCollisionManagerClass.ANIMATE_MANUAL) CollisionMode = AnimCollisionManagerClass.ANIMATE_MANUAL;
    }

    public bool Save(ChunkSaveClass csave)
    {
        Validate_Parameters();

        csave.Begin_Chunk(ANIMCOLLISIONMANAGERDEF_CHUNK_VARIABLES);
        csave.WriteMicro(ANIMCOLLISIONMANAGERDEF_VARIABLE_COLLISIONMODE, CollisionMode);
        csave.WriteMicro(ANIMCOLLISIONMANAGERDEF_VARIABLE_ANIMATIONMODE, AnimationMode);
        ArgumentNullException.ThrowIfNull(AnimationName);
        csave.WriteMicroString(ANIMCOLLISIONMANAGERDEF_VARIABLE_ANIMATIONNAME, AnimationName);
        csave.End_Chunk();

        return true;
    }
    public bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {

            switch (cload.Cur_Chunk_ID)
            {

                case ANIMCOLLISIONMANAGERDEF_CHUNK_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case ANIMCOLLISIONMANAGERDEF_VARIABLE_COLLISIONMODE:
                                cload.Read(ref CollisionMode);
                                break;
                            case ANIMCOLLISIONMANAGERDEF_VARIABLE_ANIMATIONMODE:
                                cload.Read(ref AnimationMode);
                                break;
                            case ANIMCOLLISIONMANAGERDEF_VARIABLE_ANIMATIONNAME:
                                cload.ReadMicroChunkWWString(out AnimationName);
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unhandled Chunk: 0x{0:X8} (depth {1}) in {2}", cload.Cur_Chunk_ID, cload.Cur_Chunk_Depth, nameof(AnimCollisionManagerDefClass));
                    break;
            }

            cload.Close_Chunk();
        }
        return true;
    }


    public int CollisionMode;
    public int AnimationMode;
    public string? AnimationName;

    private const int ANIMCOLLISIONMANAGERDEF_CHUNK_VARIABLES = 525000306;

    private const int ANIMCOLLISIONMANAGERDEF_VARIABLE_COLLISIONMODE = 0x00;
    private const int ANIMCOLLISIONMANAGERDEF_VARIABLE_ANIMATIONMODE = 0x01;
    private const int ANIMCOLLISIONMANAGERDEF_VARIABLE_ANIMATIONNAME = 0x02;
};
