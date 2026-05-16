namespace RenData.Sound;

public class AudibleSoundClass
{
    // Sound types converted from enum to public constant integers
    public const int TYPE_MUSIC = 0;
    public const int TYPE_SOUND_EFFECT = 1;
    public const int TYPE_DIALOG = 2;
    public const int TYPE_CINEMATIC = 3;
    public const int TYPE_COUNT = 4;

    // Sound states converted from enum to public constant integers
    public const int STATE_STOPPED = 0;
    public const int STATE_PLAYING = 1;
    public const int STATE_PAUSED = 2;
    public const int STATE_COUNT = 3;
}
