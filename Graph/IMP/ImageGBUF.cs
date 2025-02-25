namespace XenonUI.Graph.IMP;

public abstract unsafe class ImageGBUF : Image
{

	public ImageGBUF()
	{
		HasData = false;
	}
	
	public override void Draw(Graphics graphics, float x, float y, float w, float h)
	{
		graphics.DrawImage(this, x, y, w, h);
	}

}
