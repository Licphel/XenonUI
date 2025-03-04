using System.Runtime.InteropServices;
using FreeTypeSharp;
using KryptonM;
using KryptonM.IO;
using XenonUI.Graph.Text;
using Font = XenonUI.Graph.Text.Font;
using static FreeTypeSharp.FT;
using static FreeTypeSharp.FT_LOAD;
using static FreeTypeSharp.FT_Render_Mode_;
using Text_Font = XenonUI.Graph.Text.Font;

namespace XenonUI.XOpenGL;

public unsafe class OGL_FreetypeFont : Text_Font, IDisposable
{

    private readonly Dictionary<char, Glyph> Buf = new Dictionary<char, Glyph>();
    private FT_FaceRec_* face;
    private FT_LibraryRec_* lib;

    private uint resolution;

    public override float LineH => Size;
    public override float ScaledAndBlankedLineH => LineH * Scale + Scale;

    public static Font Load(FileHandle file, int resolution, int size)
    {
        FT_LibraryRec_* lib;
        FT_FaceRec_* face;
        FT_Init_FreeType(&lib);
        FT_New_Face(lib, (byte*)Marshal.StringToHGlobalAnsi(file.Path), 0, &face);
        FT_Select_Charmap(face, FT_Encoding_.FT_ENCODING_UNICODE);
        
        OGL_FreetypeFont font = new OGL_FreetypeFont { lib = lib, face = face, resolution = (uint)resolution, Size = size };
        NativeManager.I0.Remind(font.Dispose);
        return font;
    }

    public override Glyph GetGlyph(char ch)
    {
        if(Buf.TryGetValue(ch, out Glyph g1)) return g1;
        
        uint idx = FT_Get_Char_Index(face, ch);
        
        FT_Set_Pixel_Sizes(face, 0, resolution);
        FT_Load_Glyph(face, idx, FT_LOAD_DEFAULT);
        FT_Render_Glyph(face->glyph, FT_RENDER_MODE_NORMAL);
        FT_Bitmap_ m0 = face->glyph->bitmap;
        OGL_Image image = new OGL_Image();
        int len = (int)(m0.width * m0.rows * 4);
        byte* buf = stackalloc byte[len];
        for(int i = 0; i < len; i += 4)
        {
            byte grey = m0.buffer[i / 4];
            buf[i + 0] = 255;
            buf[i + 1] = 255;
            buf[i + 2] = 255;
            buf[i + 3] = grey;
        }

        image.Upload((IntPtr)buf,
            (int)face->glyph->bitmap.width,
            (int)face->glyph->bitmap.rows);

        image.SetPixelRenderMode("Linear|Linear|ClampToEdge|ClampToEdge");

        float ds = resolution / Size;

        Glyph g = new Glyph();
        g.Image = image;
        g.XMin = 0;
        g.XMax = face->glyph->metrics.width / ds / 64f;
        g.YMin = 0;
        g.YMax = face->glyph->metrics.height / ds / 64f;
        g.Advance = face->glyph->advance.x / ds / 64f;
        g.Bearing = face->glyph->metrics.horiBearingX / ds / 64f;
        g.Sink = face->glyph->metrics.horiBearingY / ds / 64f - g.Height - face->bbox.yMin / ds / 64f;

        Buf[ch] = g;
        
        return g;
    }

    public void Dispose()
    {
        FT_Done_Face(face);
        FT_Done_Library(lib);
    }

}