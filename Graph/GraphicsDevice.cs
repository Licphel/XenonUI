using KryptonM.IO;
using XenonUI.Graph.Images;
using XenonUI.Graph.Text;
using XenonUI.Maths;
using XenonUI.XOpenGL;

namespace XenonUI.Graph;

/// <summary>
/// The only class refers back to the implementations.
/// Generally, a user shouldn't directly call the low-level classes, but use this device instead.
/// </summary>
public abstract class GraphicsDevice
{

    public static GraphicsDevice Current;
    
    public const string OpenGL = "OpenGL";
    public const string Vulkan = "Vulkan";

    public string Name = "Unknown";

    public static GraphicsDevice GetLocalDevice(string name)
    {
        if(Current != null)
        {
            if(Current.Name != name)
                throw new Exception("Current device is invalid.");
            return Current;
        }
        switch(name)
        {
            case OpenGL:
                return Current = new OGL_GraphicsDevice { Name = OpenGL };
            case Vulkan:
                throw new NotImplementedException();
        }

        throw new Exception("Unknown backend name.");
    }

    public abstract SDContext Context { get; }

    public abstract ImageSurface ImageSurfaceData(byte[] data, int w, int h, ImageSurface dst = null);
    public abstract ImageSurface NewImageSurface();
    public abstract ImageSurface NewImageSurface(FileHandle file);

    public abstract Font NewFont(FileHandle file, int resolution, int size);
    
    public abstract string Title { set; }
    public abstract Vector2 Size { get; set; }
    public abstract Vector2 DeviceSize { get; }
    public abstract Vector2 Pos { get; set; }
    public abstract bool Decorated { set; }
    
    public abstract void Maximize();
    
}