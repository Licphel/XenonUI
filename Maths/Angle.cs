namespace XenonUI.Maths;

public readonly struct Angle
{

    public readonly float Value;

    public Angle(float value)
    {
        Value = value;
    }

    public static Angle Radian(float v)
    {
        return new Angle(v);
    }
    
    public static Angle Degree(float v)
    {
        return new Angle(Mathf.Rad(v));
    }

}