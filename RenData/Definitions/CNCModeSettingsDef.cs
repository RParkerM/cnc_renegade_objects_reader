using RenData.ChunkIO;
using RenData.IDs;
using RenData.SaveLoad;

namespace RenData.Definitions;

[RegisterDefinition(ChunkId.CHUNKID_GLOBAL_SETTINGS_DEF_CNCMODE)]
public partial class CNCModeSettingsDef : DefinitionClass
{
    public const int NUM_TEAMS = 2;
    public const int NUM_RADIO_CMDS = 30;

    public CNCModeSettingsDef()
    {
        AnnouncementInterval = 30;

        mPowerOfflineID = new int[NUM_TEAMS];
        mPurchaseCanceledID = new int[NUM_TEAMS];
        mInsufficientFundsID = new int[NUM_TEAMS];
        mConstructingID = new int[NUM_TEAMS];
        mUnitReadyID = new int[NUM_TEAMS];
        mIonBeaconDeployedID = new int[NUM_TEAMS];
        mIonBeaconDisarmedID = new int[NUM_TEAMS];
        mIonBeaconWarningID = new int[NUM_TEAMS];
        mNukeBeaconDeployedID = new int[NUM_TEAMS];
        mNukeBeaconDisarmedID = new int[NUM_TEAMS];
        mNukeBeaconWarningID = new int[NUM_TEAMS];

        mRadioCmds = new int[NUM_RADIO_CMDS];
        mRadioCmdIcons = new string[NUM_RADIO_CMDS];
        for (int i = 0; i < NUM_RADIO_CMDS; i++) mRadioCmdIcons[i] = string.Empty;
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_GLOBAL_SETTINGS_DEF_CNCMODE;

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

        csave.WriteMicro(VARID_DEF_ANNOUNCEMENT_INTERVAL, AnnouncementInterval);

        csave.WriteMicro(VARID_DEF_NOD_POWER_OFFLINE_ID, mPowerOfflineID[0]);
        csave.WriteMicro(VARID_DEF_GDI_POWER_OFFLINE_ID, mPowerOfflineID[1]);
        csave.WriteMicro(VARID_DEF_NOD_PURCHASE_CANCELED_ID, mPurchaseCanceledID[0]);
        csave.WriteMicro(VARID_DEF_GDI_PURCHASE_CANCELED_ID, mPurchaseCanceledID[1]);
        csave.WriteMicro(VARID_DEF_NOD_INSUFFICIENT_FUNDS_ID, mInsufficientFundsID[0]);
        csave.WriteMicro(VARID_DEF_GDI_INSUFFICIENT_FUNDS_ID, mInsufficientFundsID[1]);
        csave.WriteMicro(VARID_DEF_NOD_UNIT_READY_ID, mUnitReadyID[0]);
        csave.WriteMicro(VARID_DEF_GDI_UNIT_READY_ID, mUnitReadyID[1]);

        for (int i = 0; i < NUM_RADIO_CMDS; i++)
        {
            csave.WriteMicro((byte)(VARID_DEF_RADIO_CMD_01 + i), mRadioCmds[i]);
            csave.WriteMicroString((byte)(VARID_DEF_RADIO_ICON_01 + i), mRadioCmdIcons[i]);
        }

        csave.WriteMicro(VARID_DEF_NOD_ION_BEACON_DEPLOYED_ID, mIonBeaconDeployedID[0]);
        csave.WriteMicro(VARID_DEF_GDI_ION_BEACON_DEPLOYED_ID, mIonBeaconDeployedID[1]);
        csave.WriteMicro(VARID_DEF_NOD_ION_BEACON_DISARMED_ID, mIonBeaconDisarmedID[0]);
        csave.WriteMicro(VARID_DEF_GDI_ION_BEACON_DISARMED_ID, mIonBeaconDisarmedID[1]);
        csave.WriteMicro(VARID_DEF_NOD_ION_BEACON_WARNING_ID, mIonBeaconWarningID[0]);
        csave.WriteMicro(VARID_DEF_GDI_ION_BEACON_WARNING_ID, mIonBeaconWarningID[1]);

        csave.WriteMicro(VARID_DEF_NOD_NUKE_BEACON_DEPLOYED_ID, mNukeBeaconDeployedID[0]);
        csave.WriteMicro(VARID_DEF_GDI_NUKE_BEACON_DEPLOYED_ID, mNukeBeaconDeployedID[1]);
        csave.WriteMicro(VARID_DEF_NOD_NUKE_BEACON_DISARMED_ID, mNukeBeaconDisarmedID[0]);
        csave.WriteMicro(VARID_DEF_GDI_NUKE_BEACON_DISARMED_ID, mNukeBeaconDisarmedID[1]);
        csave.WriteMicro(VARID_DEF_NOD_NUKE_BEACON_WARNING_ID, mNukeBeaconWarningID[0]);
        csave.WriteMicro(VARID_DEF_GDI_NUKE_BEACON_WARNING_ID, mNukeBeaconWarningID[1]);

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
                        int id = (int)cload.Cur_Micro_Chunk_ID;
                        if (id == VARID_DEF_ANNOUNCEMENT_INTERVAL) cload.Read(ref AnnouncementInterval);
                        else if (id == VARID_DEF_NOD_POWER_OFFLINE_ID) cload.Read(ref mPowerOfflineID[0]);
                        else if (id == VARID_DEF_GDI_POWER_OFFLINE_ID) cload.Read(ref mPowerOfflineID[1]);
                        else if (id == VARID_DEF_NOD_PURCHASE_CANCELED_ID) cload.Read(ref mPurchaseCanceledID[0]);
                        else if (id == VARID_DEF_GDI_PURCHASE_CANCELED_ID) cload.Read(ref mPurchaseCanceledID[1]);
                        else if (id == VARID_DEF_NOD_INSUFFICIENT_FUNDS_ID) cload.Read(ref mInsufficientFundsID[0]);
                        else if (id == VARID_DEF_GDI_INSUFFICIENT_FUNDS_ID) cload.Read(ref mInsufficientFundsID[1]);
                        else if (id == VARID_DEF_NOD_UNIT_READY_ID) cload.Read(ref mUnitReadyID[0]);
                        else if (id == VARID_DEF_GDI_UNIT_READY_ID) cload.Read(ref mUnitReadyID[1]);
                        else if (id >= VARID_DEF_RADIO_CMD_01 && id < VARID_DEF_RADIO_CMD_01 + NUM_RADIO_CMDS)
                            cload.Read(ref mRadioCmds[id - VARID_DEF_RADIO_CMD_01]);
                        else if (id >= VARID_DEF_RADIO_ICON_01 && id < VARID_DEF_RADIO_ICON_01 + NUM_RADIO_CMDS)
                            mRadioCmdIcons[id - VARID_DEF_RADIO_ICON_01] = cload.ReadMicroChunkWWString();
                        else if (id == VARID_DEF_NOD_ION_BEACON_DEPLOYED_ID) cload.Read(ref mIonBeaconDeployedID[0]);
                        else if (id == VARID_DEF_GDI_ION_BEACON_DEPLOYED_ID) cload.Read(ref mIonBeaconDeployedID[1]);
                        else if (id == VARID_DEF_NOD_ION_BEACON_DISARMED_ID) cload.Read(ref mIonBeaconDisarmedID[0]);
                        else if (id == VARID_DEF_GDI_ION_BEACON_DISARMED_ID) cload.Read(ref mIonBeaconDisarmedID[1]);
                        else if (id == VARID_DEF_NOD_ION_BEACON_WARNING_ID) cload.Read(ref mIonBeaconWarningID[0]);
                        else if (id == VARID_DEF_GDI_ION_BEACON_WARNING_ID) cload.Read(ref mIonBeaconWarningID[1]);
                        else if (id == VARID_DEF_NOD_NUKE_BEACON_DEPLOYED_ID) cload.Read(ref mNukeBeaconDeployedID[0]);
                        else if (id == VARID_DEF_GDI_NUKE_BEACON_DEPLOYED_ID) cload.Read(ref mNukeBeaconDeployedID[1]);
                        else if (id == VARID_DEF_NOD_NUKE_BEACON_DISARMED_ID) cload.Read(ref mNukeBeaconDisarmedID[0]);
                        else if (id == VARID_DEF_GDI_NUKE_BEACON_DISARMED_ID) cload.Read(ref mNukeBeaconDisarmedID[1]);
                        else if (id == VARID_DEF_NOD_NUKE_BEACON_WARNING_ID) cload.Read(ref mNukeBeaconWarningID[0]);
                        else if (id == VARID_DEF_GDI_NUKE_BEACON_WARNING_ID) cload.Read(ref mNukeBeaconWarningID[1]);
                        else Console.WriteLine("Unhandled CNCModeSettingsDef Variable chunkID\n");
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unhandled CNCModeSettingsDef chunkID\n");
                    break;
            }
            cload.Close_Chunk();
        }

        return true;
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;

    protected int AnnouncementInterval;

    protected int[] mPowerOfflineID;
    protected int[] mPurchaseCanceledID;
    protected int[] mInsufficientFundsID;
    protected int[] mConstructingID;
    protected int[] mUnitReadyID;
    protected int[] mIonBeaconDeployedID;
    protected int[] mIonBeaconDisarmedID;
    protected int[] mIonBeaconWarningID;
    protected int[] mNukeBeaconDeployedID;
    protected int[] mNukeBeaconDisarmedID;
    protected int[] mNukeBeaconWarningID;

    protected int[] mRadioCmds;
    protected string[] mRadioCmdIcons;


    private const uint CHUNKID_PARENT = 803001812;
    private const uint CHUNKID_VARIABLES = 803001813;

    private const byte VARID_DEF_ANNOUNCEMENT_INTERVAL = 1;
    private const byte VARID_DEF_NOD_POWER_OFFLINE_ID = 2;
    private const byte VARID_DEF_GDI_POWER_OFFLINE_ID = 3;
    private const byte VARID_DEF_NOD_PURCHASE_CANCELED_ID = 4;
    private const byte VARID_DEF_GDI_PURCHASE_CANCELED_ID = 5;
    private const byte VARID_DEF_NOD_INSUFFICIENT_FUNDS_ID = 6;
    private const byte VARID_DEF_GDI_INSUFFICIENT_FUNDS_ID = 7;
    private const byte VARID_DEF_NOD_UNIT_READY_ID = 8;
    private const byte VARID_DEF_GDI_UNIT_READY_ID = 9;

    private const byte VARID_DEF_RADIO_CMD_01 = 10; // through 39 (30 entries)

    private const byte VARID_DEF_NOD_ION_BEACON_DEPLOYED_ID = 40;
    private const byte VARID_DEF_GDI_ION_BEACON_DEPLOYED_ID = 41;
    private const byte VARID_DEF_NOD_ION_BEACON_DISARMED_ID = 42;
    private const byte VARID_DEF_GDI_ION_BEACON_DISARMED_ID = 43;
    private const byte VARID_DEF_NOD_ION_BEACON_WARNING_ID = 44;
    private const byte VARID_DEF_GDI_ION_BEACON_WARNING_ID = 45;

    private const byte VARID_DEF_NOD_NUKE_BEACON_DEPLOYED_ID = 46;
    private const byte VARID_DEF_GDI_NUKE_BEACON_DEPLOYED_ID = 47;
    private const byte VARID_DEF_NOD_NUKE_BEACON_DISARMED_ID = 48;
    private const byte VARID_DEF_GDI_NUKE_BEACON_DISARMED_ID = 49;
    private const byte VARID_DEF_NOD_NUKE_BEACON_WARNING_ID = 50;
    private const byte VARID_DEF_GDI_NUKE_BEACON_WARNING_ID = 51;

    private const byte VARID_DEF_RADIO_ICON_01 = 52; // through 81 (30 entries)
}
