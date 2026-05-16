using System;
using System.Numerics;
using RenData.ChunkIO;
using RenData.SaveLoad;
using RenData.IDs;
using RenData.Definitions;
using RenData.Sound;

namespace RenData;

[RegisterDefinition(ChunkId.CHUNKID_WWAUDIO_BEGIN)]
public partial class AudibleSoundDefinitionClass : DefinitionClass
{
    public AudibleSoundDefinitionClass()
    {
        m_Priority = 0.5f;
        m_Volume = 1.0f;
        m_Pan = 0.5f;
        m_LoopCount = 1;
        m_DropOffRadius = 40.0f;
        m_LogicalDropOffRadius = -1.0f;
        m_MaxVolRadius = 20.0f;
        m_Is3D = true;
        m_Type = AudibleSoundClass.TYPE_SOUND_EFFECT;
        m_LogicalTypeMask = 0;
        m_LogicalNotifyDelay = 2;
        m_CreateLogical = false;
        m_AttenuationSphereColor = new Vector3(0, 0.75f, 0.75f);
        m_StartOffset = 0;
        m_PitchFactor = 1.0f;
        m_PitchFactorRandomizer = 0.0f;
        m_VolumeRandomizer = 0.0f;
        m_VirtualChannel = 0;

        // NAMED_EDITABLE_PARAM(AudibleSoundDefinitionClass, ParameterClass.TYPE_SOUND_FILENAME, m_Filename, "Filename");
        // INT_EDITABLE_PARAM(AudibleSoundDefinitionClass, m_VirtualChannel, 0, 100);
        // NAMED_EDITABLE_PARAM(AudibleSoundDefinitionClass, ParameterClass.TYPE_FLOAT, m_DropOffRadius, "Drop-off Radius");
        // NAMED_EDITABLE_PARAM(AudibleSoundDefinitionClass, ParameterClass.TYPE_FLOAT, m_MaxVolRadius, "Max-Vol Radius");
        // NAMED_EDITABLE_PARAM(AudibleSoundDefinitionClass, ParameterClass.TYPE_BOOL, m_Is3D, "Is 3D Sound");
        // INT_EDITABLE_PARAM(AudibleSoundDefinitionClass, m_LoopCount, 0, 1000000);
        // FLOAT_EDITABLE_PARAM(AudibleSoundDefinitionClass, m_Volume, 0, 1.0F);
        // NAMED_FLOAT_UNITS_PARAM(AudibleSoundDefinitionClass, m_VolumeRandomizer, 0, 1.0F, "", "Volume Random (+/-)");
        // FLOAT_EDITABLE_PARAM(AudibleSoundDefinitionClass, m_Pan, 0, 1.0F);
        // FLOAT_EDITABLE_PARAM(AudibleSoundDefinitionClass, m_Priority, 0, 1.0F);

        // ENUM_PARAM(AudibleSoundDefinitionClass, m_Type, ("Sound Effect", AudibleSoundClass.TYPE_SOUND_EFFECT,
        //     "Music", AudibleSoundClass.TYPE_MUSIC,
        //     "Dialog", AudibleSoundClass.TYPE_DIALOG,
        //     "Cinematic", AudibleSoundClass.TYPE_CINEMATIC, 0));

        // FLOAT_EDITABLE_PARAM(AudibleSoundDefinitionClass, m_StartOffset, 0, 1.0F);
        // FLOAT_EDITABLE_PARAM(AudibleSoundDefinitionClass, m_PitchFactor, 0, 1.0F);
        // NAMED_FLOAT_UNITS_PARAM(AudibleSoundDefinitionClass, m_PitchFactorRandomizer, 0, 1.0F, "", "Pitch Factor Random (+/-)");
        // NAMED_EDITABLE_PARAM(AudibleSoundDefinitionClass, ParameterClass.TYPE_STRING, m_DisplayText, "Display Text");

        //
        //	Logical sound params
        //
        // NAMED_EDITABLE_PARAM(AudibleSoundDefinitionClass, ParameterClass.TYPE_BOOL, m_CreateLogical, "Create Logical Sound");
        // NAMED_EDITABLE_PARAM(AudibleSoundDefinitionClass, ParameterClass.TYPE_FLOAT, m_LogicalDropOffRadius, "Logical Drop-off Radius");
        // NAMED_EDITABLE_PARAM(AudibleSoundDefinitionClass, ParameterClass.TYPE_FLOAT, m_LogicalNotifyDelay, "Logical Notif Delay");

        //#ifdef	PARAM_EDITING_ON
        //
        //	Configure the logical type mask enumeration
        //
        //EnumParameterClass* param = new EnumParameterClass(&m_LogicalTypeMask);
        //param.Set_Name("Logical Type");
        //int count = WWAudioClass.Get_Instance().Get_Logical_Type_Count();
        //for (int index = 0; index < count; index++) {
        //	string display_name(0, true);
        //	int id = WWAudioClass.Get_Instance().Get_Logical_Type(index, display_name);
        //	param.Add_Value(display_name, id);
        //}
        //GENERIC_EDITABLE_PARAM(AudibleSoundDefinitionClass, param);
        //#endif

        // NAMED_EDITABLE_PARAM(AudibleSoundDefinitionClass, ParameterClass.TYPE_COLOR, m_AttenuationSphereColor, "Sphere Color");

        return;
    }

    // From DefinitionClass
    public override uint Get_Class_ID() => ClassId.CLASSID_SOUND;

    // From PersistClass
    public override PersistFactoryClass Get_Factory() => _persistFactory;

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
                    Console.WriteLine($"Unhandled Chunk: 0x{cload.Cur_Chunk_ID:X8} (depth {cload.Cur_Chunk_Depth}) in {nameof(AudibleSoundDefinitionClass)}");
                    break;
            }

            cload.Close_Chunk();
        }

        return retval;
    }

    public override PersistClass? Create()
    {
        // Original implementation returned Create_Sound(CLASSID_3D);
        // PersistClass* AudibleSoundDefinitionClass.Create() 
        // {
        //     return Create_Sound(CLASSID_3D);
        // }
        throw new NotImplementedException();
    }

    public AudibleSoundClass? Create_Sound(int classid_hint)
    {
        // Original C++ implementation (kept as comment reference):
        /*
        AudibleSoundClass* new_sound = null;

        // If this is a relative path, strip it off and assume
        // the current directory is set correctly.
        string real_filename(m_Filename, true);
         char* dir_delimiter = .strrchr(m_Filename, '\\');
        if (dir_delimiter != null && m_Filename.Get_Length() > 2 && m_Filename[1] != ':') {
            real_filename = (dir_delimiter + 1);
        }

        // Should we create a 2D or 3D sound?
        if (m_Is3D && classid_hint != CLASSID_2D) {
            new_sound = WWAudioClass.Get_Instance().Create_3D_Sound(real_filename, classid_hint);
        }
        else {
            new_sound = WWAudioClass.Get_Instance().Create_Sound_Effect(real_filename);
        }

        // Did we successfully create the sound?
        if (new_sound != null) {
            // Configure the sound
            new_sound.Set_Type((AudibleSoundClass.SOUND_TYPE)m_Type);
            new_sound.Set_Priority(m_Priority);
            new_sound.Set_Loop_Count(m_LoopCount);
            new_sound.Set_DropOff_Radius(m_DropOffRadius);
            new_sound.Set_Definition((AudibleSoundDefinitionClass*)this);
            new_sound.Set_Start_Offset(m_StartOffset);
            new_sound.Set_Virtual_Channel(m_VirtualChannel);

            // Randomize the volume and pitch as necessary
            float volume = m_Volume;
            if (m_VolumeRandomizer != 0) {
                volume += WWMath.Random_Float(-m_VolumeRandomizer, m_VolumeRandomizer);
            }

            float pitch_factor = m_PitchFactor;
            if (m_PitchFactor != 0) {
                pitch_factor += WWMath.Random_Float(-m_PitchFactorRandomizer, m_PitchFactorRandomizer);
            }

            // Set the volume and pitch
            new_sound.Set_Volume(volume);
            new_sound.Set_Pitch_Factor(pitch_factor);

            if (new_sound.As_Sound3DClass() != null) {
                ((Sound3DClass*)new_sound).Set_Max_Vol_Radius(m_MaxVolRadius);
            }
        }

        return new_sound;
        */

        throw new NotImplementedException();
    }

    public void Initialize_From_Sound(AudibleSoundClass? sound)
    {
        // Original implementation (commented for reference):
        /*
        if (sound != null) {
            Sound3DClass* sound_3d = sound.As_Sound3DClass();

            // Choose defaults for the values that we can't get from
            // the sound.
            m_LogicalDropOffRadius = -1.0F;
            m_LogicalNotifyDelay = 2;
            m_LogicalTypeMask = 0;
            m_CreateLogical = false;
            m_Pan = 0.5F;
            m_DisplayText = "";

            // Copy the values that we can from the sound object
            m_Filename = sound.Get_Filename();
            m_DropOffRadius = sound.Get_DropOff_Radius();
            m_Priority = sound.Peek_Priority();
            m_Is3D = (sound_3d != null);
            m_Type = sound.Get_Type();
            m_LoopCount = sound.Get_Loop_Count();
            m_Volume = sound.Get_Volume();
            m_StartOffset = sound.Get_Start_Offset();
            m_PitchFactor = sound.Get_Pitch_Factor();
            m_VirtualChannel = sound.Get_Virtual_Channel();

            if (sound_3d != null) {
                m_MaxVolRadius = sound_3d.Get_Max_Vol_Radius();
            }
        }
        */

        throw new NotImplementedException();
    }

    // Accessors
    public string Get_Filename() => m_Filename ?? string.Empty;
    public string Get_Display_Text() => m_DisplayText ?? string.Empty;
    public float Get_Max_Vol_Radius() => m_MaxVolRadius;
    public float Get_DropOff_Radius() => m_DropOffRadius;
    public Vector3 Get_Sphere_Color() => m_AttenuationSphereColor;
    public float Get_Volume() => m_Volume;
    public float Get_Volume_Randomizer() => m_VolumeRandomizer;
    public float Get_Start_Offset() => m_StartOffset;
    public float Get_Pitch_Factor() => m_PitchFactor;
    public float Get_Pitch_Factor_Randomizer() => m_PitchFactorRandomizer;
    public int Get_Virtual_Channel() => m_VirtualChannel;

    public void Set_Volume(float volume) { m_Volume = volume; }
    public void Set_Volume_Randomizer(float value) { m_VolumeRandomizer = value; }
    public void Set_Max_Vol_Radius(float radius) { m_MaxVolRadius = radius; }
    public void Set_DropOff_Radius(float radius) { m_DropOffRadius = radius; }
    public void Set_Start_Offset(float offset) { m_StartOffset = offset; }
    public void Set_Pitch_Factor(float factor) { m_PitchFactor = factor; }
    public void Set_Pitch_Factor_Randomizer(float value) { m_PitchFactorRandomizer = value; }
    public void Set_Virtual_Channel(int channel) { m_VirtualChannel = channel; }

    // Logical sound creation
    public LogicalSoundClass? Create_Logical()
    {
        throw new NotImplementedException();
        //LogicalSoundClass? logical_sound = null;

        //if (m_CreateLogical)
        //{
        //    logical_sound = new LogicalSoundClass();
        //    logical_sound.Set_Type_Mask(m_LogicalTypeMask);
        //    logical_sound.Set_Notify_Delay(m_LogicalNotifyDelay);
        //    logical_sound.Set_Single_Shot(m_LoopCount != 0);

        //    if (m_LogicalDropOffRadius < 0)
        //    {
        //        logical_sound.Set_DropOff_Radius(m_DropOffRadius);
        //    }
        //    else
        //    {
        //        logical_sound.Set_DropOff_Radius(m_LogicalDropOffRadius);
        //    }
        //}

        //return logical_sound;
    }

    // Private helper methods for save/load
    private bool Save_Variables(ChunkSaveClass csave)
    {
        // Save the audible variables
        csave.WriteMicro(VARID_PRIORITY, m_Priority);
        csave.WriteMicro(VARID_VOLUME, m_Volume);
        csave.WriteMicro(VARID_PAN, m_Pan);
        csave.WriteMicro(VARID_LOOP_COUNT, m_LoopCount);
        csave.WriteMicro(VARID_DROP_OFF, m_DropOffRadius);
        csave.WriteMicro(VARID_MAX_VOL, m_MaxVolRadius);
        csave.WriteMicro(VARID_TYPE, m_Type);
        csave.WriteMicro(VARID_IS3D, m_Is3D);
        ArgumentNullException.ThrowIfNull(m_Filename);
					csave.WriteMicroString(VARID_FILENAME, m_Filename);
        ArgumentNullException.ThrowIfNull(m_DisplayText);
					csave.WriteMicroString(VARID_DISPLAY_TEXT, m_DisplayText);
        csave.WriteMicro(VARID_START_OFFSET, m_StartOffset);
        csave.WriteMicro(VARID_PITCH_FACTOR, m_PitchFactor);
        csave.WriteMicro(VARID_PITCH_FACTOR_RND, m_PitchFactorRandomizer);
        csave.WriteMicro(VARID_VOLUME_RND, m_VolumeRandomizer);
        csave.WriteMicro(VARID_VIRTUAL_CHANNEL, m_VirtualChannel);

        // Save the logical variables
        csave.WriteMicro(VARID_LOGICAL_MASK, m_LogicalTypeMask);
        csave.WriteMicro(VARID_LOGICAL_DELAY, m_LogicalNotifyDelay);
        csave.WriteMicro(VARID_CREATE_LOGICAL, m_CreateLogical);
        csave.WriteMicro(VARID_LOGICAL_DROP_OFF, m_LogicalDropOffRadius);
        csave.WriteMicro(VARID_SPHERE_COLOR, m_AttenuationSphereColor);

        return true;
    }

    private bool Load_Variables(ChunkLoadClass cload)
    {
        while (cload.Open_Micro_Chunk())
        {
            switch (cload.Cur_Micro_Chunk_ID)
            {
                case VARID_PRIORITY:
                    cload.Read(ref m_Priority);
                    break;
                case VARID_VOLUME:
                    cload.Read(ref m_Volume);
                    break;
                case VARID_PAN:
                    cload.Read(ref m_Pan);
                    break;
                case VARID_LOOP_COUNT:
                    cload.Read(ref m_LoopCount);
                    break;
                case VARID_DROP_OFF:
                    cload.Read(ref m_DropOffRadius);
                    break;
                case VARID_MAX_VOL:
                    cload.Read(ref m_MaxVolRadius);
                    break;
                case VARID_TYPE:
                    cload.Read(ref m_Type);
                    break;
                case VARID_IS3D:
                    cload.Read(ref m_Is3D);
                    break;
                case VARID_FILENAME:
					cload.ReadMicroChunkWWString(out m_Filename);
                    break;
                case VARID_DISPLAY_TEXT:
					cload.ReadMicroChunkWWString(out m_DisplayText);
                    break;
                case VARID_LOGICAL_MASK:
                    cload.Read(ref m_LogicalTypeMask);
                    break;
                case VARID_LOGICAL_DELAY:
                    cload.Read(ref m_LogicalNotifyDelay);
                    break;
                case VARID_CREATE_LOGICAL:
                    cload.Read(ref m_CreateLogical);
                    break;
                case VARID_LOGICAL_DROP_OFF:
                    cload.Read(ref m_LogicalDropOffRadius);
                    break;
                case VARID_SPHERE_COLOR:
                    cload.Read(ref m_AttenuationSphereColor);
                    break;
                case VARID_START_OFFSET:
                    cload.Read(ref m_StartOffset);
                    break;
                case VARID_PITCH_FACTOR:
                    cload.Read(ref m_PitchFactor);
                    break;
                case VARID_PITCH_FACTOR_RND:
                    cload.Read(ref m_PitchFactorRandomizer);
                    break;
                case VARID_VOLUME_RND:
                    cload.Read(ref m_VolumeRandomizer);
                    break;
                case VARID_VIRTUAL_CHANNEL:
                    cload.Read(ref m_VirtualChannel);
                    break;
                default:
                    Console.WriteLine($"Unhandled Micro Chunk:{cload.Cur_Micro_Chunk_ID}");
                    break;
            }

            cload.Close_Micro_Chunk();
        }

        return true;
    }

    // Protected member data (converted from C++ private section)
    protected float m_Priority;
    protected float m_Volume;
    protected float m_VolumeRandomizer;
    protected float m_Pan;
    protected int m_LoopCount;
    protected int m_VirtualChannel;
    protected float m_DropOffRadius;
    protected float m_MaxVolRadius;
    protected bool m_Is3D;
    protected string? m_Filename;
    protected int m_Type;
    protected string? m_DisplayText;
    protected float m_StartOffset;
    protected float m_PitchFactor;
    protected float m_PitchFactorRandomizer;

    protected int m_LogicalTypeMask;
    protected float m_LogicalNotifyDelay;
    protected float m_LogicalDropOffRadius;
    protected bool m_CreateLogical;

    // Misc UI info
    protected Vector3 m_AttenuationSphereColor;

    private const uint CHUNKID_VARIABLES = 0x00000100u;
    private const uint CHUNKID_BASE_CLASS = 0x00000200u;

    private const int VARID_UNUSED1 = 0x01;
    private const int VARID_UNUSED2 = 0x02;
    private const int VARID_PRIORITY = 0x03;
    private const int VARID_VOLUME = 0x04;
    private const int VARID_PAN = 0x05;
    private const int VARID_LOOP_COUNT = 0x06;
    private const int VARID_DROP_OFF = 0x07;
    private const int VARID_MAX_VOL = 0x08;
    private const int VARID_TYPE = 0x09;
    private const int VARID_IS3D = 0x0A;
    private const int VARID_FILENAME = 0x0B;
    private const int VARID_DISPLAY_TEXT = 0x0C;
    private const int VARID_LOGICAL_MASK = 0x0D;
    private const int VARID_LOGICAL_DELAY = 0x0E;
    private const int VARID_CREATE_LOGICAL = 0x0F;
    private const int VARID_LOGICAL_DROP_OFF = 0x10;
    private const int VARID_SPHERE_COLOR = 0x11;
    private const int VARID_START_OFFSET = 0x12;
    private const int VARID_PITCH_FACTOR = 0x13;
    private const int VARID_PITCH_FACTOR_RND = 0x14;
    private const int VARID_VOLUME_RND = 0x15;
    private const int VARID_VIRTUAL_CHANNEL = 0x16;
}


