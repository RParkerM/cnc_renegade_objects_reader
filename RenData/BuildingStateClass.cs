using System.Diagnostics;

namespace RenData;

public abstract class BuildingStateClass
{
    public const int HEALTH100_POWERON = 0;
    public const int HEALTH75_POWERON = 1;
    public const int HEALTH50_POWERON = 2;
    public const int HEALTH25_POWERON = 3;
    public const int DESTROYED_POWERON = 4;

    public const int HEALTH100_POWEROFF = 5;
    public const int HEALTH75_POWEROFF = 6;
    public const int HEALTH50_POWEROFF = 7;
    public const int HEALTH25_POWEROFF = 8;
    public const int DESTROYED_POWEROFF = 9;

    public const int STATE_COUNT = 10;

    public const int HEALTH_100 = 0;
    public const int HEALTH_75 = 1;
    public const int HEALTH_50 = 2;
    public const int HEALTH_25 = 3;
    public const int HEALTH_0 = 4;

    public static int Get_Health_State(int building_state)
    {
        int state = building_state;
        if (state >= HEALTH100_POWEROFF)
        {
            state -= HEALTH100_POWEROFF;
        }
        return state;
    }
    public static int Percentage_To_Health_State(float health)
    {
        if (health <= 0.0f)
        {
            return HEALTH_0;
        }
        if (health <= 25.0f)
        {
            return HEALTH_25;
        }
        if (health <= 50.0f)
        {
            return HEALTH_50;
        }
        if (health <= 75.0f)
        {
            return HEALTH_75;
        }
        return HEALTH_100;
    }

    public static bool Is_Power_On(int building_state)
    {
        return (building_state < HEALTH100_POWEROFF);
    }
    public static int Enable_Power(int input_state, bool onoff)
    {
        if (onoff)
        {
            return _EquivalentPowerOnState[input_state];
        }
        else
        {
            return _EquivalentPowerOffState[input_state];
        }
    }

    public static int Compose_State(int health_state, bool power_onoff)
    {
        int state = health_state;
        if (power_onoff == false)
        {
            state += HEALTH100_POWEROFF;
        }
        return state;
    }

    public static string Get_State_Name(int state)
    {
        Debug.Assert(state >= 0);
        Debug.Assert(state < STATE_COUNT);

        return _StateNames[state];
    }


    private static int[] _EquivalentPowerOnState =
    {
    HEALTH100_POWERON,
    HEALTH75_POWERON,
    HEALTH50_POWERON,
    HEALTH25_POWERON,
    DESTROYED_POWERON,

    HEALTH100_POWERON,
    HEALTH75_POWERON,
    HEALTH50_POWERON,
    HEALTH25_POWERON,
    DESTROYED_POWERON,
    };

    private static int[] _EquivalentPowerOffState =
    {
    HEALTH100_POWEROFF,
    HEALTH75_POWEROFF,
    HEALTH50_POWEROFF,
    HEALTH25_POWEROFF,
    DESTROYED_POWEROFF,

    HEALTH100_POWEROFF,
    HEALTH75_POWEROFF,
    HEALTH50_POWEROFF,
    HEALTH25_POWEROFF,
    DESTROYED_POWEROFF,
    };

    private static string[] _StateNames =
    {
    "Building State: Health 100%, Power ON",
    "Building State: Health 75%, Power ON",
    "Building State: Health 50%, Power ON",
    "Building State: Health 25%, Power ON",
    "Building State: Destroyed, Power ON",
    "Building State: Health 100%, Power OFF",
    "Building State: Health 75%, Power OFF",
    "Building State: Health 50%, Power OFF",
    "Building State: Health 25%, Power OFF",
    "Building State: Destroyed, Power OFF",
    };


};
