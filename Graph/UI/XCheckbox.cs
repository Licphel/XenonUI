using XenonUI.Core;
using XenonUI.Graph.Images;
using XenonUI.Graph.Text;
using XenonUI.Maths;

namespace XenonUI.Graph.UI;

//a special bounded struct: the Bound refers to its part "box", not including the text.
public class XCheckbox : XElement
{

    public Lore DisplayedLore;

    public Icon[] Icons = new Icon[3];

    public bool IsOn;
    public bool ShouldShowCross;
    public VaryingVector2 TextOffset = new VaryingVector2();

    public override void Update()
    {
        base.Update();

        if(KeyBind.MouseLeft.Pressed() && Bound.Contains(Cursor)) IsOn = !IsOn;
    }

    public override void Draw(Graphics graphics)
    {
        base.Draw(graphics);

        if(IsOn)
        {
            graphics.DrawIcon(Icons[1], Bound);
        }
        else
        {
            if(ShouldShowCross)
                graphics.DrawIcon(Icons[2], Bound);
            else
                graphics.DrawIcon(Icons[0], Bound);
        }

        graphics.DrawLore(DisplayedLore, Bound.xprom + TextOffset.x, Bound.y + TextOffset.y);
    }

}