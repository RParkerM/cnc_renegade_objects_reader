using System;
using System.Collections.Generic;
using System.Text;

namespace RenData.PhysDef;

public abstract class MotorVehicle
{
    public static float RPM_TO_RADS(float rpm)
    {
        return (float)((rpm) * (2.0f * WWMath.WWMATH_PI) / 60.0f);
    }

    public static float RADS_TO_RPM(float rads)
    {
        return (float)(rads * 60.0f / (2.0f * WWMath.WWMATH_PI));
    }
}
