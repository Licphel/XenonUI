namespace XenonUI.Maths;

public struct Vector4
{

    public static Vector4 Zero = new Vector4(0, 0, 0, 0);
    public static Vector4 One = new Vector4(1, 1, 1, 1);

    public float x, y, z, w;
   
    public int xi => (int)x;
    public int yi => (int)y;
    public int zi => (int)z;
    public int wi => (int)w;

    public float Len => Mathf.Sqrt(x * x + y * y + z * z + w * w);

    public Vector4(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public Vector4(Vector3 v, float w)
    {
        x = v.x;
        y = v.y;
        z = v.z;
        this.w = w;
    }

    public Vector4(IReadOnlyList<float> list) : this(list[0], list[1], list[2], list[3])
    {
    }

    public static Vector4 operator +(Vector4 vec1, Vector4 vec2)
    {
        return new Vector4(vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z, vec1.w + vec2.w);
    }

    public static Vector4 operator -(Vector4 vec1, Vector4 vec2)
    {
        return new Vector4(vec1.x - vec2.x, vec1.y - vec2.y, vec1.z - vec2.z, vec1.w - vec2.w);
    }

    public static Vector4 operator *(Vector4 vec1, Vector4 vec2)
    {
        return new Vector4(vec1.x * vec2.x, vec1.y * vec2.y, vec1.z * vec2.z, vec1.w * vec2.w);
    }

    public static Vector4 operator /(Vector4 vec1, Vector4 vec2)
    {
        return new Vector4(vec1.x / vec2.x, vec1.y / vec2.y, vec1.z / vec2.z, vec1.w / vec2.w);
    }

    public static Vector4 operator +(Vector4 vec1, float v)
    {
        return new Vector4(vec1.x + v, vec1.y + v, vec1.z + v, vec1.w + v);
    }

    public static Vector4 operator -(Vector4 vec1, float v)
    {
        return new Vector4(vec1.x - v, vec1.y - v, vec1.z - v, vec1.w - v);
    }

    public static Vector4 operator *(Vector4 vec1, float v)
    {
        return new Vector4(vec1.x * v, vec1.y * v, vec1.z * v, vec1.w * v);
    }

    public static Vector4 operator /(Vector4 vec1, float v)
    {
        return new Vector4(vec1.x / v, vec1.y / v, vec1.z / v, vec1.w / v);
    }

    public static Vector4 operator -(Vector4 vec)
    {
        return new Vector4(-vec.x, -vec.y, -vec.z, -vec.w);
    }

}