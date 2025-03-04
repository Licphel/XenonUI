namespace XenonUI.Graph.Images;

public unsafe class ImageAtlas
{
    
    private int cx, cy;
    private byte[] data;
    private ImageSurface surface;
    private int mh;
    private int p0, p1;

    public ImageAtlas(int size = 4096)
    {
        p0 = p1 = size;
    }

    public Image Accept(Image img)
    {
        if(surface == null || data == null)
            throw new Exception("Image atlas has not begun.");
        
        if(cx + img.Width >= p0)
        {
            cy += mh;
            mh = 0;
            cx = 0;
        }
        
        mh = Math.Max(img.Height, mh);
        Image part = Image.Subimage(surface, cx, cy, img.Width, img.Height);
        
        fixed(byte* src = img.Data)
        {
            fixed(byte* dst = data)
            {
                byte* dst0 = dst + cy * p0 * 4 + cx * 4;

                for(int x = 0; x < img.Width; x++)
                for(int y = 0; y < img.Height; y++)
                {
                    byte* src1 = src + y * img.Width * 4 + x * 4;

                    byte* dst1 = dst0 + y * p0 * 4;
                    dst1 += x * 4;

                    dst1[0] = src1[0];
                    dst1[1] = src1[1];
                    dst1[2] = src1[2];
                    dst1[3] = src1[3];
                }
            }
        }

        cx += img.Width;
        return part;
    }

    public void Begin()
    {
        surface = GraphicsDevice.Current.NewImageSurface();
        data = new byte[p0 * p1 * 4];
    }

    public void End()
    {
        if(surface == null || data == null)
            throw new Exception("Image atlas has not begun.");
        
        GraphicsDevice.Current.ImageSurfaceData(data, p0, p1, surface);
    }

}