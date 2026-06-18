using System.Numerics;
using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;
using RenData.Types;

namespace RenData.Definitions;

[RegisterDefinition(ChunkId.CHUNKID_GLOBAL_SETTINGS_DEF_EVA)]
public partial class EvaSettingsDefClass : DefinitionClass
{
    public EvaSettingsDefClass()
    {
        ObjectivesScreenRect = new RectClass(0.063f, 0.25f, 0.938f, 0.75f);
        ObjectivesTextRect = new RectClass(0.1f, 0.260f, 0.906f, 0.555f);
        ObjectivesEndcapUVRect = new RectClass(0, 67, 8, 127);
        ObjectivesFadeoutUVRect = new RectClass(41, 34, 127, 65);
        ObjectivesBackgroundUVRect = new RectClass(2, 1, 126, 32);
        ObjectivesTextureSize = new Vector2(128, 128);

        MessagesScreenRect = new RectClass(0.116f, 0.021f, 0.938f, 0.208f);
        MessagesTextRect = new RectClass(0.147f, 0.031f, 0.906f, 0.198f);
        MessagesEndcapUVRect = new RectClass(0, 67, 8, 127);
        MessagesFadeoutUVRect = new RectClass(41, 34, 127, 65);
        MessagesBackgroundUVRect = new RectClass(2, 1, 126, 32);
        MessagesTextureSize = new Vector2(128, 128);
        MessagesIconPos = new Vector2(0.016f, 0.021f);
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GLOBAL_SETTINGS_DEF_EVA;

    public override PersistClass? Create()
    {
        //WWASSERT(0);
        return null;
    }

    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(CHUNKID_PARENT);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_VARIABLES);
        csave.WriteMicro(VARID_OBJECTIVESSCREENRECT, (RectClassStruct)ObjectivesScreenRect);
        csave.WriteMicro(VARID_OBJECTIVESTEXTRECT, (RectClassStruct)ObjectivesTextRect);
        csave.WriteMicro(VARID_OBJECTIVESENDCAPUVRECT, (RectClassStruct)ObjectivesEndcapUVRect);
        csave.WriteMicro(VARID_OBJECTIVESFADEOUTUVRECT, (RectClassStruct)ObjectivesFadeoutUVRect);
        csave.WriteMicro(VARID_OBJECTIVESBACKGROUNDUVRECT, (RectClassStruct)ObjectivesBackgroundUVRect);
        csave.WriteMicro(VARID_OBJECTIVESTEXTURESIZE, ObjectivesTextureSize);

        csave.WriteMicro(VARID_MESSAGESSCREENRECT, (RectClassStruct)MessagesScreenRect);
        csave.WriteMicro(VARID_MESSAGESTEXTRECT, (RectClassStruct)MessagesTextRect);
        csave.WriteMicro(VARID_MESSAGESENDCAPUVRECT, (RectClassStruct)MessagesEndcapUVRect);
        csave.WriteMicro(VARID_MESSAGESFADEOUTUVRECT, (RectClassStruct)MessagesFadeoutUVRect);
        csave.WriteMicro(VARID_MESSAGESBACKGROUNDUVRECT, (RectClassStruct)MessagesBackgroundUVRect);
        csave.WriteMicro(VARID_MESSAGESTEXTURESIZE, MessagesTextureSize);
        csave.WriteMicro(VARID_MESSAGESICONPOS, MessagesIconPos);
        csave.End_Chunk();

        return true;
    }

    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {
                case CHUNKID_PARENT:
                    base.Load(cload);
                    break;

                case CHUNKID_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        RectClassStruct rc = default;
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case VARID_OBJECTIVESSCREENRECT: cload.Read(ref rc); ObjectivesScreenRect.Set(rc); break;
                            case VARID_OBJECTIVESTEXTRECT: cload.Read(ref rc); ObjectivesTextRect.Set(rc); break;
                            case VARID_OBJECTIVESENDCAPUVRECT: cload.Read(ref rc); ObjectivesEndcapUVRect.Set(rc); break;
                            case VARID_OBJECTIVESFADEOUTUVRECT: cload.Read(ref rc); ObjectivesFadeoutUVRect.Set(rc); break;
                            case VARID_OBJECTIVESBACKGROUNDUVRECT: cload.Read(ref rc); ObjectivesBackgroundUVRect.Set(rc); break;
                            case VARID_OBJECTIVESTEXTURESIZE: cload.Read(ref ObjectivesTextureSize); break;

                            case VARID_MESSAGESSCREENRECT: cload.Read(ref rc); MessagesScreenRect.Set(rc); break;
                            case VARID_MESSAGESTEXTRECT: cload.Read(ref rc); MessagesTextRect.Set(rc); break;
                            case VARID_MESSAGESENDCAPUVRECT: cload.Read(ref rc); MessagesEndcapUVRect.Set(rc); break;
                            case VARID_MESSAGESFADEOUTUVRECT: cload.Read(ref rc); MessagesFadeoutUVRect.Set(rc); break;
                            case VARID_MESSAGESBACKGROUNDUVRECT: cload.Read(ref rc); MessagesBackgroundUVRect.Set(rc); break;
                            case VARID_MESSAGESTEXTURESIZE: cload.Read(ref MessagesTextureSize); break;
                            case VARID_MESSAGESICONPOS: cload.Read(ref MessagesIconPos); break;

                            default:
                                Console.WriteLine("Unhandled EvaSettingsDefClass Variable chunkID\n");
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unhandled EvaSettingsDefClass chunkID\n");
                    break;
            }
            cload.Close_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    protected RectClass ObjectivesScreenRect;
    protected RectClass ObjectivesTextRect;
    protected RectClass ObjectivesEndcapUVRect;
    protected RectClass ObjectivesFadeoutUVRect;
    protected RectClass ObjectivesBackgroundUVRect;
    protected Vector2 ObjectivesTextureSize;

    protected RectClass MessagesScreenRect;
    protected RectClass MessagesTextRect;
    protected RectClass MessagesEndcapUVRect;
    protected RectClass MessagesFadeoutUVRect;
    protected RectClass MessagesBackgroundUVRect;
    protected Vector2 MessagesTextureSize;
    protected Vector2 MessagesIconPos;


    private const uint CHUNKID_PARENT = 803001812;
    private const uint CHUNKID_VARIABLES = 803001813;

    private const byte VARID_OBJECTIVESSCREENRECT = 1;
    private const byte VARID_OBJECTIVESTEXTRECT = 2;
    private const byte VARID_OBJECTIVESENDCAPUVRECT = 3;
    private const byte VARID_OBJECTIVESFADEOUTUVRECT = 4;
    private const byte VARID_OBJECTIVESBACKGROUNDUVRECT = 5;
    private const byte VARID_OBJECTIVESTEXTURESIZE = 6;

    private const byte VARID_MESSAGESSCREENRECT = 7;
    private const byte VARID_MESSAGESTEXTRECT = 8;
    private const byte VARID_MESSAGESENDCAPUVRECT = 9;
    private const byte VARID_MESSAGESFADEOUTUVRECT = 10;
    private const byte VARID_MESSAGESBACKGROUNDUVRECT = 11;
    private const byte VARID_MESSAGESTEXTURESIZE = 12;
    private const byte VARID_MESSAGESICONPOS = 13;
}
