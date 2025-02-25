namespace XenonUI.Graph.IMP;

public abstract class Image : ImageRegion, IDisposable
{

    public byte[] Data;

    public bool HasData = true;

    public Image()
    {
        //Set the region ref to itself.
        Src = this;
    }

    public abstract void Dispose();

    public virtual Graphics CreateGraphics()
    {
        if(!HasData)
            throw new Exception("This image cannot be edited.");
        return new GraphicsImageRGBA(Data, Width, Height);
    }

}