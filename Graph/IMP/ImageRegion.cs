namespace XenonUI.Graph.IMP;

public class ImageRegion : Icon
{

	public Image Src;
	public float U, V;
	public int Width, Height;

	public virtual void Draw(Graphics graphics, float x, float y, float w, float h)
	{
		graphics.DrawImage(Src, x, y, w, h, U, V, Width, Height);
	}

	public static ImageRegion BySize(ImageRegion tex, float u, float v, float uw, float vh)
	{
		ImageRegion part = new ImageRegion();
		part.Src = tex.Src;
		part.U = u + tex.U;
		part.V = v + tex.V;
		part.Width = (int) uw;
		part.Height = (int) vh;
		return part;
	}

	public static ImageRegion ByVerts(ImageRegion tex, float u, float v, float u2, float v2)
	{
		return BySize(tex, u, v, u2 - u, v2 - v);
	}

	public static ImageRegion ByPercentSize(ImageRegion tex, float u, float v, float w, float h)
	{
		return BySize(tex, tex.Width * u, tex.Height * v, tex.Width * w, tex.Width * h);
	}

	public static ImageRegion ByPercentVerts(ImageRegion tex, float u, float v, float u2, float v2)
	{
		return ByVerts(tex, tex.Width * u, tex.Height * v, tex.Width * u2, tex.Width * v2);
	}

}
