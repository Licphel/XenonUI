using XenonUI.Core;
using XenonUI.Graph.Images;
using XenonUI.Graph.Text;
using XenonUI.Maths;

namespace XenonUI.Graph.UI;

public class XButton : XElement
{

    private bool cursorOn;

    public Icon[] Icons = new Icon[3];
    public bool IsOn;

    //if it is true, this is a state-switching button.
    public bool IsSwitcher;

    public Action OnLeftFired = () => { };
    public Action OnRightFired = () => { };

    private int pressDelay;
    public Lore Text;
    public VaryingVector2 TextOffset = new VaryingVector2();
    public Image Texture3Line;

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
                graphics.DrawIcon(Icons[2], Bound);
            else if(cursorOn)
                graphics.DrawIcon(Icons[1], Bound);
            else
                graphics.DrawIcon(Icons[0], Bound);

            graphics.DrawLore(Text, Bound.xcentral + TextOffset.x, Bound.y + TextOffset.y, FontAlign.Center);
        }
        else
        {
            float sy;
            if(pressDelay > 0 || (IsOn && IsSwitcher))
                sy = Texture3Line.Height / 3f * 2;
            else if(cursorOn)
                sy = Texture3Line.Height / 3f;
            else
                sy = 0;

            graphics.DrawImage(Texture3Line, Bound, 0, sy, Texture3Line.Width, Texture3Line.Height / 3f);
            graphics.DrawLore(Text, Bound.xcentral + TextOffset.x, Bound.y + TextOffset.y, FontAlign.Center);
        }
    }

}