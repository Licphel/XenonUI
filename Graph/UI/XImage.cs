namespace XenonUI.Graph.UI;

public class XImage : XElement
{

    public Icon Icon;

    public override void Draw(Graphics graphics)
    {
        graphics.Draw(Icon, Bound);
    }

}