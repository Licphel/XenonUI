namespace XenonUI.Graph.Images;

public abstract class ImageSurface : Image, IDisposable
{

    public ImageSurface()
    {
        IsSurface = true;
    }
    
    public abstract void Dispose();

}