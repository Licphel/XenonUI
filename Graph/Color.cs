namespace XenonUI.Graph;

public struct Color
{

	/// <summary>
	///     It should be over 0 and less than 1, but sometimes, when colors are merging, it can overstep.
	/// </summary>
	public float R, G, B, A;

    public Color()
    {
        R = 1;
        G = 1;
        B = 1;
        A = 1;
    }

    public Color(float r, float g, float b, float a = 1)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public Color(Color c0, float a = 1)
    {
        R = c0.R;
        G = c0.G;
        B = c0.B;
        A = a;
    }

    public byte ByteR => (byte)(R * 255);
    public byte ByteG => (byte)(G * 255);
    public byte ByteB => (byte)(B * 255);
    public byte ByteA => (byte)(A * 255);

    public static Color operator *(Color c1, Color c2)
    {
        return new Color(c1.R * c2.R, c1.G * c2.G, c1.B * c2.B, c1.A * c2.A);
    }

    public static Color operator +(Color c1, Color c2)
    {
        return new Color(c1.R + c2.R, c1.G + c2.G, c1.B + c2.B, c1.A + c2.A);
    }

    public static Color operator /(Color c1, Color c2)
    {
        return new Color(c1.R / c2.R, c1.G / c2.G, c1.B / c2.B, c1.A / c2.A);
    }

    public static Color operator -(Color c1, Color c2)
    {
        return new Color(c1.R - c2.R, c1.G - c2.G, c1.B - c2.B, c1.A - c2.A);
    }

    public static Color operator !(Color c)
    {
        return new Color(1 - c.R, 1 - c.G, 1 - c.B, c.A);
    }

    public static Color operator *(Color c, float v)
    {
        return new Color(c.R * v, c.G * v, c.B * v, c.A);
    }

    public static Color operator +(Color c, float v)
    {
        return new Color(c.R + v, c.G + v, c.B + v, c.A);
    }

    public static Color operator /(Color c, float v)
    {
        return new Color(c.R / v, c.G / v, c.B / v, c.A);
    }

    public static Color operator -(Color c, float v)
    {
        return new Color(c.R - v, c.G - v, c.B - v, c.A);
    }

    public static Color HsvToRgb(float hue, float saturation, float value)
    {
        var i = (int)(hue * 6) % 6;
        var f = hue * 6 - i;
        var f1 = value * (1 - saturation);
        var f2 = value * (1 - f * saturation);
        var f3 = value * (1 - (1 - f) * saturation);
        float f4 = 0;
        float f5 = 0;
        float f6 = 0;

        switch(i)
        {
            case 0:
                f4 = value;
                f5 = f3;
                f6 = f1;
                break;
            case 1:
                f4 = f2;
                f5 = value;
                f6 = f1;
                break;
            case 2:
                f4 = f1;
                f5 = value;
                f6 = f3;
                break;
            case 3:
                f4 = f1;
                f5 = f2;
                f6 = value;
                break;
            case 4:
                f4 = f3;
                f5 = f1;
                f6 = value;
                break;
            case 5:
                f4 = value;
                f5 = f1;
                f6 = f2;
                break;
        }

        return new Color(f4, f5, f6);
    }

}