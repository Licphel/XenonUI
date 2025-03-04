namespace XenonUI.Graph.Images;

public class Image : Icon
{

    public Image Reference;
    public byte[] Data;
    public virtual bool HasData => !IsSurface;
    public bool IsSurface;
    public float U, V;
    public bool IsFixedSize => true;
    public int Width { get; protected set; }
    public int Height { get; protected set; }

    /// <summary>
    /// Returns the source image at the end of the 'image chain'.
    /// For example, a subimage returns its parent image.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception">Cannot find a drawable source image</exception>
    public virtual Image GetTrueImageSurface()
    {
        if(IsSurface)
            return this;
        if(Reference == null)
            throw new Exception("Missing true image surface.");
        return Reference.GetTrueImageSurface();
    }

    public virtual Graphics CreateGraphics()
    {
        if(!HasData)
            throw new Exception("Image has no data.");
        //TODO to render on a general image
        throw new Exception($"Graphics is not available on {GetType().Name}.");
    }
    
    public static Image Subimage(Image tex, float u, float v, float uw, float vh)
    {
        Image part = new Image();
        part.Reference = tex.GetTrueImageSurface();
        part.U = u + tex.U;
        part.V = v + tex.V;
        part.Width = (int)uw;
        part.Height = (int)vh;
        return part;
    }

    public virtual void InternalPerform(Graphics graphics, float x, float y, float w, float h)
    {
        graphics.DrawImage(this, x, y, w, h);
    }

}