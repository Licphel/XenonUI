namespace XenonUI.Graph.Text;

public struct GlyphBounds
{

    public string Sequence;
    public float Width;
    public float Height;
    public float LastWidth;

    public GlyphBounds(string sequence, float w, float h, float lw)
    {
        Sequence = sequence;
        Width = w;
        Height = h;
        LastWidth = lw;
    }

}