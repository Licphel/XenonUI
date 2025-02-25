using KryptonM;
using KryptonM.IO;
using OpenTK.Graphics.OpenGL;
using StbImageSharp;
using XenonUI.Graph.IMP;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace XenonUI.OpenGL;

public unsafe class OGL_Image : ImageGBUF
{

    public ImageResult _N_RES;

    public int Id;
    public bool IsFB;

    static OGL_Image()
    {
        GL.Enable(EnableCap.Texture2d);
    }

    public OGL_Image(int id, int w, int h)
    {
        Id = id;
        Width = w;
        Height = h;
    }

    public OGL_Image()
    {
        Id = GL.GenTexture();

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2d, Id);

        NativeManager.I0.Remind(() => Dispose());
    }

    public void Upload(FileHandle handler)
    {
        ImageResult result = _N_RES =
            ImageResult.FromMemory(File.ReadAllBytes(handler.Path), ColorComponents.RedGreenBlueAlpha);
        fixed(byte* p = result.Data)
        {
            Upload((IntPtr)p, result.Width, result.Height, InternalFormat.Rgba, PixelFormat.Rgba);
        }

        Data = result.Data;
    }

    public void Upload(IntPtr bmap, int w, int h, InternalFormat f1 = InternalFormat.Rgba,
        PixelFormat f2 = PixelFormat.Bgra)
    {
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2d, Id);

        Width = w;
        Height = h;

        GL.TexImage2D(
            TextureTarget.Texture2d,
            0,
            f1,
            Width,
            Height,
            0,
            f2,
            PixelType.UnsignedByte,
            bmap
        );

        SetPixelRenderMode("Nearest|Nearest|Repeat|Repeat");
    }

    public void SetPixelRenderMode(string data)
    {
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2d, Id);

        var ss = data.Split('|');
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter,
            (int)(uint)Enum.Parse(typeof(TextureMinFilter), ss[0]));
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter,
            (int)(uint)Enum.Parse(typeof(TextureMagFilter), ss[1]));
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS,
            (int)(uint)Enum.Parse(typeof(TextureWrapMode), ss[2]));
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT,
            (int)(uint)Enum.Parse(typeof(TextureWrapMode), ss[3]));
    }

    public override void Dispose()
    {
        GL.DeleteTexture(Id);
    }

}