using KryptonM.IO;
using OpenTK.Windowing.GraphicsLibraryFramework;
using XenonUI.Graph;
using XenonUI.Graph.Images;
using XenonUI.Graph.Text;
using XenonUI.Maths;

namespace XenonUI.XOpenGL;

public unsafe class OGL_GraphicsDevice : GraphicsDevice
{

    public override SDContext Context => OGL_SDContext.Global;

    public override ImageSurface ImageSurfaceData(byte[] data, int w, int h, ImageSurface dst = null)
    {
        OGL_Image img;
        if(dst == null)
            img = new OGL_Image();
        else
            img = (OGL_Image)dst;
        fixed(byte* ptr = data)
            img.Upload((IntPtr)ptr, w, h);
        return img;
    }

    public override ImageSurface NewImageSurface()
    {
        return new OGL_Image();
    }

    public override ImageSurface NewImageSurface(FileHandle file)
    {
        OGL_Image img = new OGL_Image();
        img.Upload(file);
        return img;
    }

    public override Font NewFont(FileHandle file, int resolution, int size)
    {
        return OGL_FreetypeFont.Load(file, resolution, size);
    }
    
    public override Vector2 Size
    {
        get
        {
            GLFW.GetWindowSize(OGL.Window, out int x, out int y);
            return new Vector2(x, y);
        }
        set => GLFW.SetWindowSize(OGL.Window, (int)value.x, (int)value.y);
    }

    public override Vector2 DeviceSize
    {
        get
        {
            VideoMode* vm = GLFW.GetVideoMode(GLFW.GetPrimaryMonitor());
            return new Vector2(vm->Width, vm->Height);
        }
    }

    public override Vector2 Pos
    {
        get
        {
            GLFW.GetWindowPos(OGL.Window, out int x, out int y);
            return new Vector2(x, y);
        }
        set => GLFW.SetWindowPos(OGL.Window, (int)value.x, (int)value.y);
    }

    public override bool Decorated
    {
        set => GLFW.SetWindowAttrib(OGL.Window, WindowAttribute.Decorated, value);
    }

    public override void Maximize()
    {
        GLFW.MaximizeWindow(OGL.Window);
    }

    public override string Title
    {
        set => GLFW.SetWindowTitle(OGL.Window, value);
    }

}