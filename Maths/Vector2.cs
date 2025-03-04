namespace XenonUI.Maths;

public struct Vector2
{

    public static readonly Vector2 Zero = new Vector2(0, 0);
    public static readonly Vector2 One = new Vector2(1, 1);

    public float x, y;

    public int xi => (int)x;
    public int yi => (int)y;

    public float Len => Mathf.Sqrt(x * x + y * y);

    public Vector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2(IReadOnlyList<float> vec)
    {
        x = vec[0];
        y = vec[1];
    }

    public Vector2 SetDeg(float ln, float deg)
    {
        x = Mathf.CosDeg(deg) * ln;
        y = Mathf.SinDeg(deg) * ln;
        return this;
    }

    public Vector2 SetRad(float ln, float rad)
    {
        x = Mathf.CosRad(rad) * ln;
        y = Mathf.SinRad(rad) * ln;
        return this;
    }

    public static Vector2 operator +(Vector2 vec1, Vector2 vector2)
    {
        return new Vector2(vec1.x + vector2.x, vec1.y + vector2.y);
    }

    public static Vector2 operator -(Vector2 vec1, Vector2 vector2)
    {
        return new Vector2(vec1.x - vector2.x, vec1.y - vector2.y);
    }

    public static Vector2 operator *(Vector2 vec1, Vector2 vector2)
    {
        return new Vector2(vec1.x * vector2.x, vec1.y * vector2.y);
    }

    public static Vector2 operator /(Vector2 vec1, Vector2 vector2)
    {
        return new Vector2(vec1.x / vector2.x, vec1.y / vector2.y);
    }

    public static Vector2 operator +(Vector2 vec1, float v)
    {
        return new Vector2(vec1.x + v, vec1.y + v);
    }

    public static Vector2 operator -(Vector2 vec1, float v)
    {
        return new Vector2(vec1.x - v, vec1.y - v);
    }

    public static Vector2 operator *(Vector2 vec1, float v)
    {
        return new Vector2(vec1.x * v, vec1.y * v);
    }

    public static Vector2 operator /(Vector2 vec1, float v)
    {
        return new Vector2(vec1.x / v, vec1.y / v);
    }

    public static Vector2 operator -(Vector2 vec)
    {
        return new Vector2(-vec.x, -vec.y);
    }

}