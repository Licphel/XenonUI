using XenonUI.Core;
using XenonUI.Graph.IMP;
using XenonUI.Maths;

namespace XenonUI.Graph.UI;

public class XButton : XElement
{

	private bool cursorOn;

	//if it is true, this is a state-switching button.
	public bool IsSwitcher;
	public bool IsOn;

	public Icon[] Icons = new Icon[3];
	public Image Texture3Line;

	public Action OnLeftFired = () => { };
	public Action OnRightFired = () => { };

	private int pressDelay;
	public Lore Text;
	public VaryingVector2 TextOffset = new VaryingVector2();

	public static int DEFAULT_PRESS_DELAY => Application.Tps / 8;

	public override void Update()
	{
		base.Update();

		pressDelay--;

		cursorOn = false;

		if(Bound.Contains(Cursor) && IsExposed())
		{
			cursorOn = true;

			if(KeyBind.MouseLeft.Pressed())
			{
				OnLeftFired.Invoke();
				pressDelay = DEFAULT_PRESS_DELAY;
				IsOn = !IsOn;
			}

			if(KeyBind.MouseRight.Pressed())
			{
				OnRightFired.Invoke();
				pressDelay = DEFAULT_PRESS_DELAY;
			}
		}
	}

	public override void Draw(Graphics graphics)
	{
		if(Texture3Line == null)
		{
			if(pressDelay > 0 || (IsOn && IsSwitcher))
			{
				graphics.Draw(Icons[2], Bound);
			}
			else if(cursorOn)
			{
				graphics.Draw(Icons[1], Bound);
			}
			else
			{
				graphics.Draw(Icons[0], Bound);
			}

			graphics.Draw(Text, Bound.xcentral + TextOffset.x, Bound.y + TextOffset.y, Align.Center);
		}
		else
		{
			float sy;
			if(pressDelay > 0|| (IsOn && IsSwitcher))
			{
				sy = Texture3Line.Height / 3f * 2;
			}
			else if(cursorOn)
			{
				sy = Texture3Line.Height / 3f;
			}
			else
			{
				sy = 0;
			}

			graphics.DrawImage(Texture3Line, Bound, 0, sy, Texture3Line.Width, Texture3Line.Height / 3f);
			graphics.Draw(Text, Bound.xcentral + TextOffset.x, Bound.y + TextOffset.y, Align.Center);
		}
	}

}
