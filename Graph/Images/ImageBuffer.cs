namespace XenonUI.Graph.Images;

public class ImageBuffer : Image
{

    public static ImageBuffer Img0x0 = new ImageBuffer(0, 0);

    private Image generatedSurfaceImage;
    
    public ImageBuffer(int w, int h)
    {
        IsSurface = false;
        Data = new byte[w * h * 4];
        Width = w;
        Height = h;
    }
    
    public override void InternalPerform(Graphics graphics, float x, float y, float w, float h)
    {
        if(generatedSurfaceImage == null)
        {
            if(Data.Length == 0)
                return;
            generatedSurfaceImage = GraphicsDevice.Current.NewImageSurface();
            GraphicsDevice.Current.ImageSurfaceData(Data, Width, Height);
        }
        graphics.DrawImage(generatedSurfaceImage, x, y, w, h);
    }

}