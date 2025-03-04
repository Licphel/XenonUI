namespace XenonUI.Maths;

public class VaryingVector2
{

    public float x, y;

    public VaryingVector2()
    {
    }

    public VaryingVector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public VaryingVector2(IReadOnlyList<float> rvec)
    {
        x = rvec[0];
        y = rvec[1];
    }

    public int xi => (int)x;
    public int yi => (int)y;

    public float Len => Mathf.Sqrt(x * x + y * y);

    public VaryingVector2 FromDeg(float ln, float deg)
    {
        x = Mathf.CosDeg(deg) * ln;
        y = Mathf.SinDeg(deg) * ln;
        return this;
    }

    public VaryingVector2 FromRad(float ln, float rad)
    {
        x = Mathf.CosRad(rad) * ln;
        y = Mathf.SinRad(rad) * ln;
        return this;
    }

    public void Copy(VaryingVector2 vec)
    {
        x = vec.x;
        y = vec.y;
    }

    public static VaryingVector2 operator +(VaryingVector2 rvec1, VaryingVector2 rvec2)
    {
        rvec1.x += rvec2.x;
        rvec1.y += rvec2.y;
        return rvec1;
    }

    public static VaryingVector2 operator +(VaryingVector2 rvec1, Vector2 vector2)
    {
        rvec1.x += vector2.x;
        rvec1.y += vector2.y;
        return rvec1;
    }

    public static VaryingVector2 operator -(VaryingVector2 rvec1, VaryingVector2 rvec2)
    {
        rvec1.x -= rvec2.x;
        rvec1.y -= rvec2.y;
        return rvec1;
    }

    public static VaryingVector2 operator -(VaryingVector2 rvec1, Vector2 vector2)
    {
        rvec1.x -= vector2.x;
        rvec1.y -= vector2.y;
        return rvec1;
    }

    public static VaryingVector2 operator *(VaryingVector2 rvec1, VaryingVector2 rvec2)
    {
        rvec1.x *= rvec2.x;
        rvec1.y *= rvec2.y;
        return rvec1;
    }

    public static VaryingVector2 operator *(VaryingVector2 rvec1, Vector2 vector2)
    {
        rvec1.x *= vector2.x;
        rvec1.y *= vector2.y;
        return rvec1;
    }

    public static VaryingVector2 operator /(VaryingVector2 rvec1, VaryingVector2 rvec2)
    {
        rvec1.x /= rvec2.x;
        rvec1.y /= rvec2.y;
        return rvec1;
    }

    public static VaryingVector2 operator /(VaryingVector2 rvec1, Vector2 vector2)
    {
        rvec1.x /= vector2.x;
        rvec1.y /= vector2.y;
        return rvec1;
    }

    public static VaryingVector2 operator -(VaryingVector2 rvec)
    {
        rvec.x = -rvec.x;
        rvec.y = -rvec.y;
        return rvec;
    }

    public static explicit operator Vector2(VaryingVector2 v)
    {
        return new Vector2(v.x, v.y);
    }

}