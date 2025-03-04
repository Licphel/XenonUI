using XenonUI.Graph.Images;

namespace XenonUI.Graph.Text;

public struct Glyph
{

    public Image Image;
    public float XMin, XMax;
    public float YMin, YMax;
    public float Advance;
    public float Bearing;
    public float Sink;

    public float Width => XMax - XMin;
    public float Height => YMax - YMin;

    public static Glyph operator *(Glyph g, float m)
    {
        g.XMin *= m;
        g.XMax *= m;
        g.YMin *= m;
        g.YMax *= m;
        g.Advance *= m;
        g.Bearing *= m;
        g.Sink *= m;
        return g;
    }

}