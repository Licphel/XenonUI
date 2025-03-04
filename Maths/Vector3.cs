namespace XenonUI.Maths;

public struct Vector3
{

    public static Vector3 Zero = new Vector3(0, 0, 0);
    public static Vector3 One = new Vector3(1, 1, 1);
    
    public float x, y, z;
    
    public int xi => (int)x;
    public int yi => (int)y;
    public int zi => (int)z;

    public float Len => Mathf.Sqrt(x * x + y * y + z * z);

    public Vector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    public Vector3(Vector2 v, float z)
    {
        x = v.x;
        y = v.y;
        this.z = z;
    }

    public Vector3(IReadOnlyList<float> list) : this(list[0], list[1], list[2])
    {
    }

    public static Vector3 operator +(Vector3 vec1, Vector3 vec2)
    {
        return new Vector3(vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z);
    }

    public static Vector3 operator -(Vector3 vec1, Vector3 vec2)
    {
        return new Vector3(vec1.x - vec2.x, vec1.y - vec2.y, vec1.z - vec2.z);
    }

    public static Vector3 operator *(Vector3 vec1, Vector3 vec2)
    {
        return new Vector3(vec1.x * vec2.x, vec1.y * vec2.y, vec1.z * vec2.z);
    }

    public static Vector3 operator /(Vector3 vec1, Vector3 vec2)
    {
        return new Vector3(vec1.x / vec2.x, vec1.y / vec2.y, vec1.z / vec2.z);
    }

    public static Vector3 operator +(Vector3 vec1, float v)
    {
        return new Vector3(vec1.x + v, vec1.y + v, vec1.z + v);
    }

    public static Vector3 operator -(Vector3 vec1, float v)
    {
        return new Vector3(vec1.x - v, vec1.y - v, vec1.z - v);
    }

    public static Vector3 operator *(Vector3 vec1, float v)
    {
        return new Vector3(vec1.x * v, vec1.y * v, vec1.z * v);
    }

    public static Vector3 operator /(Vector3 vec1, float v)
    {
        return new Vector3(vec1.x / v, vec1.y / v, vec1.z / v);
    }

    public static Vector3 operator -(Vector3 vec)
    {
        return new Vector3(-vec.x, -vec.y, -vec.z);
    }

}