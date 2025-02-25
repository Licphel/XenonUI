namespace XenonUI.Graph.IMP;

public abstract unsafe class Image : ImageRegion, IDisposable
{

    public bool HasData = true;
    public byte[] Data;
    
    public Image()
    {
        //Set the region ref to itself.
        Src = this;
    }
   
    public virtual Graphics CreateGraphics()
    {
        if(!HasData)
            throw new Exception("This image cannot be edited.");
        return new GraphicsImageRGBA(Data, Width, Height);
    }

    public abstract void Dispose();

}