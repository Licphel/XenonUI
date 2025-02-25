using XenonUI.Core;

namespace XenonUI.Graph.UI;

public class SideScroller
{

	private float bx, by, bw, bh;
	private float lcy, sp0;
	private float prevPos;
	private float startPos;

	public bool IsDragging;
	public float Speed = 250;
	private float accels;
	public float TotalSize;
	public float Outline;
	public float Pos => Time.PartialTicks * (startPos - prevPos) + prevPos;

	public SideScroller(float otl = 0)
	{
		Outline = otl;
		startPos = -Outline;
	}

	public void UpToTop(XElement c)
	{
		startPos = -Outline;
		ClampPos(c);
		prevPos = startPos;
	}

	public void DownToGround(XElement c)
	{
		startPos = TotalSize - c.Bound.h + Outline * 2;
		ClampPos(c);
		prevPos = startPos;
	}

	protected void ClampPos(XElement c)
	{
		//A minimum offset.
		if(startPos <= -Outline)
		{
			startPos = -Outline;
		}

		if(startPos - TotalSize - Outline >= -c.Bound.h)
		{
			startPos = TotalSize - c.Bound.h + Outline;
		}

		if(TotalSize + Outline * 2 < c.Bound.h)
		{
			startPos = -Outline;
		}
	}

	public void Update(XElement c)
	{
		Keyboard input = Keyboard.Global;

		float scr = input.Scroll;

		prevPos = startPos;

		if(c.Bound.Contains(c.Cursor) && scr != 0)
		{
			switch(input.ScrollDirection)
			{
				case ScrollDirection.UP:
					accels -= Speed;
					break;
				case ScrollDirection.DOWN:
					accels += Speed;
					break;
			}
			input.ConsumeCursorScroll();
		}

		startPos += accels * Time.Delta;
		accels *= 0.75f;
		ClampPos(c);

		if(KeyBind.MouseLeft.Holding())
		{
			if(!IsDragging)
			{
				float mx = c.Cursor.x;
				float my = c.Cursor.y;

				if(mx >= bx - 1 && mx <= bx + bw + 1 && my >= by - 1 && my <= by + bh + 1)
				{
					IsDragging = true;
					lcy = c.Cursor.y;
					sp0 = startPos;
				}
			}
		}
		else
		{
			IsDragging = false;
		}

		if(IsDragging)
		{
			startPos = sp0 - (c.Cursor.y - lcy) / c.Bound.h * TotalSize;
			ClampPos(c);
		}
	}

	public void Draw(Graphics graphics, XElement c)
	{
		float per = (c.Bound.h - Outline * 2) / TotalSize;
		if(per > 1)
		{
			per = 1;
		}

		float scrollPer = System.Math.Abs(Pos) / TotalSize;
		float h = c.Bound.h * per;
		float oh = scrollPer * c.Bound.h;

		bh = h;
		bx = c.Bound.xprom - 5;
		by = c.Bound.yprom - oh - h;
		bw = 2;

		if(per < 1)
		{
			graphics.DrawRect(bx, by, bw, bh);
		}
	}

}
