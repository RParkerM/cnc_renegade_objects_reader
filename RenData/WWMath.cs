namespace RenData;

public static class WWMath
{
    //#define WWMATH_EPSILON		0.0001f
    //#define WWMATH_EPSILON2		WWMATH_EPSILON * WWMATH_EPSILON
    //#define WWMATH_PI				3.141592654f
    //#define WWMATH_FLOAT_MAX	(FLT_MAX)
    //#define WWMATH_FLOAT_MIN	(FLT_MIN)
    //#define WWMATH_SQRT2			1.414213562f
    //#define WWMATH_SQRT3			1.732050808f
    //#define WWMATH_OOSQRT2		0.707106781f
    //#define WWMATH_OOSQRT3		0.577350269f

    public const double WWMATH_PI = 3.141592654f;
    public static double RAD_TO_DEG(double x) => (x * 180.0 / WWMATH_PI);
    public static double DEG_TO_RAD(double x) => (x * WWMATH_PI / 180.0);

    public static float RAD_TO_DEGF(float x) => (x * 180.0f / (float)WWMATH_PI);
    public static float DEG_TO_RADF(float x) => (x * (float)WWMATH_PI / 180.0f);

    public static float Random_Float(float min, float max)
    {
        Random rand = new();
        return (float)(min + (rand.NextDouble() * (max - min)));
    }
}
