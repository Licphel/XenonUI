namespace XenonUI.Maths;

public ref struct Rotation
{

    public float cx, cy;
    public Angle Angle;

    public Rotation(float cx, float cy, Angle a)
    {
        this.cx = cx;
        this.cy = cy;
        Angle = a;
    }
    
    public static Rotation Get(Rect rect, Angle a)
    {
        return new Rotation(rect.xcentral, rect.ycentral, a);
    }
    
    public static Rotation Get(Vector2 vec, Angle a)
    {
        return new Rotation(vec.x, vec.y, a);
    }
    
    public static Rotation Get(float x, float y, Angle a)
    {
        return new Rotation(x, y, a);
    }

}