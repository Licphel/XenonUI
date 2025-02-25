using XenonUI.Core;
using XenonUI.Maths;

namespace XenonUI.Graph.UI;

//a special bounded struct: the Bound refers to its part "box", not including the text.
public class XCheckbox : XElement
{

	public bool IsOn;
	public bool ShouldShowCross;

	public Icon[] Icons = new Icon[3];
	public Lore DisplayedLore;
	public VaryingVector2 TextOffset = new VaryingVector2();

	public override void Update()
	{
		base.Update();

		if(KeyBind.MouseLeft.Pressed() && Bound.Contains(Cursor))
		{
			IsOn = !IsOn;
		}
	}

	public override void Draw(Graphics graphics)
	{
		base.Draw(graphics);

		if(IsOn)
		{
			graphics.Draw(Icons[1], Bound);
		}
		else
		{
			if(ShouldShowCross) 
				graphics.Draw(Icons[2], Bound);
			else
				graphics.Draw(Icons[0], Bound);
		}
		
		graphics.Draw(DisplayedLore, Bound.xprom + TextOffset.x, Bound.y + TextOffset.y);
	}

}
