using System.Runtime.InteropServices;
using KryptonM;
using KryptonM.IO;
using OpenTK.Graphics.OpenGL;
using StbImageSharp;
using XenonUI.Graph.Images;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace XenonUI.XOpenGL;

public unsafe class OGL_Image : ImageSurface
{
    
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

    public void Upload(FileHandle handle)
    {
        if(!handle.Exists)
            throw new FileNotFoundException($"File {handle.Path} does not exist.");
        
        ImageResult result = ImageResult.FromMemory(File.ReadAllBytes(handle.Path), ColorComponents.RedGreenBlueAlpha);
        fixed(byte* p = result.Data)
        {
            Upload((IntPtr)p, result.Width, result.Height);
        }
    }

    public void Upload(IntPtr bmap, int w, int h)
    {
        Data = new byte[w * h * 4];
        if(bmap != IntPtr.Zero)
            Marshal.Copy(bmap, Data, 0, Data.Length);
        
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2d, Id);

        Width = w;
        Height = h;

        GL.TexImage2D(
            TextureTarget.Texture2d,
            0,
            InternalFormat.Rgba,
            Width,
            Height,
            0,
            PixelFormat.Rgba,
            PixelType.UnsignedByte,
            bmap
        );

        SetPixelRenderMode("Nearest|Nearest|Repeat|Repeat");
    }

    public void SetPixelRenderMode(string data)
    {
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2d, Id);

        string[] ss = data.Split('|');
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