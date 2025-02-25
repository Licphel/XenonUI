using System.Runtime.InteropServices;
using FreeTypeSharp;
using KryptonM;
using KryptonM.IO;
using XenonUI.Graph;
using Font = XenonUI.Graph.Font;
using Graph_Font = XenonUI.Graph.Font;
using static FreeTypeSharp.FT;
using static FreeTypeSharp.FT_LOAD;
using static FreeTypeSharp.FT_Render_Mode_;

namespace XenonUI.OpenGL;

public unsafe class OGL_FT_Font : Graph_Font
{

    private readonly Dictionary<char, Glyph> Buf = new Dictionary<char, Glyph>();
    private FT_FaceRec_* face, face1;

    private uint resolution;

    public override float LineH => Size;
    public override float ScaledAndBlankedLineH => LineH * Scale + Scale;

    public static Font Load(FileHandle[] files, int resolution, int size)
    {
        if(files.Length > 2)
            Logger.Fatal("Too many face fallbacks.");
        FT_LibraryRec_* lib;
        FT_FaceRec_* face;
        FT_Init_FreeType(&lib);
        FT_New_Face(lib, (byte*)Marshal.StringToHGlobalAnsi(files[0].Path), 0, &face);
        FT_Select_Charmap(face, FT_Encoding_.FT_ENCODING_UNICODE);
        FT_LibraryRec_* lib1;
        FT_FaceRec_* face1 = null;
        if(files.Length > 1)
        {
            FT_Init_FreeType(&lib1);
            FT_New_Face(lib1, (byte*)Marshal.StringToHGlobalAnsi(files[1].Path), 0, &face1);
            FT_Select_Charmap(face1, FT_Encoding_.FT_ENCODING_UNICODE);
        }

        return new OGL_FT_Font { face = face, face1 = face1, resolution = (uint)resolution, Size = size };
    }

    public override Glyph GetGlyph(char ch)
    {
        if(Buf.TryGetValue(ch, out Glyph g1)) return g1;

        FT_FaceRec_* face = this.face;
        var idx = FT_Get_Char_Index(face, ch);

        if(idx == 0) face = face1; //fallback

        FT_Set_Pixel_Sizes(face, 0, resolution);
        FT_Load_Glyph(face, idx, FT_LOAD_DEFAULT);
        FT_Render_Glyph(face->glyph, FT_RENDER_MODE_NORMAL);
        FT_Bitmap_ m0 = face->glyph->bitmap;
        OGL_Image image = new OGL_Image();
        var len = (int)(m0.width * m0.rows * 4);
        var buf = stackalloc byte[len];
        for(var i = 0; i < len; i += 4)
        {
            var grey = m0.buffer[i / 4];
            buf[i + 0] = 255;
            buf[i + 1] = 255;
            buf[i + 2] = 255;
            buf[i + 3] = grey;
        }

        image.Upload((IntPtr)buf,
            (int)face->glyph->bitmap.width,
            (int)face->glyph->bitmap.rows);

        image.SetPixelRenderMode("Linear|Linear|Repeat|Repeat");

        var ds = resolution / Size;

        Glyph g = new Glyph();
        g.Src = image;
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

}