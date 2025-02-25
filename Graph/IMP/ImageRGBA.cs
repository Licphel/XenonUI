using System.Runtime.InteropServices;

namespace XenonUI.Graph.IMP;

public unsafe class ImageRGBA : Image
{
    
    public ImageRGBA(int w, int h)
    {
        HasData = true;
        Data = new byte[w * h * 4];
        Width = w;
        Height = h;
    }

    public override void Dispose()
    {
    }

    public override void Draw(Graphics graphics, float x, float y, float w, float h)
    {
        throw new NotImplementedException("Cannot directly draw ImageRGBA to the screen.");
    }

}