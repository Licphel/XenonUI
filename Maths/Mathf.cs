namespace XenonUI.Maths;

public class Mathf
{

    public const float DTR = (float)(Math.PI / 180F);
    public const float RTD = 1 / DTR;

    private static readonly float[] SinTable = new float[0x10000];

    static Mathf()
    {
        for(int i = 0; i < 65535; i++) SinTable[i] = (float)Math.Sin(i * Math.PI * 2D / 65536D);
    }

    public static float SafeDiv(float v1, float v2)
    {
        return v2 == 0 ? 0 : v1 / v2;
    }

    public static float Pow(float v, float d)
    {
        return (float)Math.Pow(v, d);
    }

    public static float Sqrt(float v)
    {
        return (float)Math.Sqrt(v);
    }

    public static float Log(float v)
    {
        return (float)Math.Log(v);
    }

    public static long Abs(long v)
    {
        return v > 0 ? v : -v;
    }

    public static float AtanRad(float y, float x)
    {
        return (float)Math.Atan2(y, x);
    }

    public static float AtanDeg(float y, float x)
    {
        return AtanRad(y, x) * RTD;
    }

    public static float SinRad(float v)
    {
        return SinTable[(int)(v * 10430.38f) & 0xffff];
    }

    public static float CosRad(float v)
    {
        return SinTable[(int)(v * 10430.38f + 16384f) & 0xffff];
    }

    public static float SinDeg(float v)
    {
        return SinRad(v * DTR);
    }

    public static float CosDeg(float v)
    {
        return CosRad(v * DTR);
    }

    public static float Rad(float v)
    {
        return v * DTR;
    }

    public static float Deg(float v)
    {
        return v * RTD;
    }

    public static int Ceiling(float v)
    {
        return (int)Math.Ceiling(v);
    }

    public static int FastFloor(float v)
    {
        int i = (int)v;
        return v >= i ? i : i - 1;
    }

    public static int Round(float v)
    {
        return (int)Math.Round(v);
    }

}