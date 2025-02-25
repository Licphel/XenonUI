using XenonUI.Graph.IMP;

namespace XenonUI.Graph;

public class FontBitmap : Font
{

    public delegate int LocatePage(char ch);

    public float[] _GlyphWidth = new float[65536];
    public int[] _GlyphX = new int[65536];
    public int[] _GlyphY = new int[65536];
    public int _YSize;
    public LocatePage Locate;
    public Image[] texture;

    public override float LineH => _YSize;
    public override float ScaledAndBlankedLineH => LineH * Scale + Scale;

    public static Font Load(Image[] textures, LocatePage locator, int[] ghx, int[] ghy, float[] ghw, int size)
    {
        FontBitmap font = new FontBitmap();
        font.texture = textures;
        font.Locate = locator;
        font._GlyphX = ghx;
        font._GlyphY = ghy;
        font._GlyphWidth = ghw;
        font.Size = font._YSize = size;

        return font;
    }

    public override Glyph GetGlyph(char ch)
    {
        Glyph g = new Glyph();
        var page = Locate(ch);
        g.Src = ImageRegion.BySize(texture[page], _GlyphX[ch], _GlyphY[ch], _GlyphWidth[ch], _YSize);
        g.XMin = 0;
        g.XMax = _GlyphWidth[ch];
        g.YMin = 0;
        g.YMax = _YSize;
        g.Advance = g.Width;
        return g;
    }

}