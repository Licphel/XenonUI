using XenonUI.Graph.Images;

namespace XenonUI.Graph.UI;

public class XImage : XElement
{

    public Icon Icon;

    public override void Draw(Graphics graphics)
    {
        graphics.DrawIcon(Icon, Bound);
    }

}