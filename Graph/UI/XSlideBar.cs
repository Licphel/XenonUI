using System.Globalization;
using XenonUI.Core;
using XenonUI.Graph.Images;
using XenonUI.Graph.Text;
using XenonUI.Maths;

namespace XenonUI.Graph.UI;

public delegate string TextConvert(float value);

public class XSlideBar : XElement
{

    private readonly XButton decrease;
    private readonly XButton increase;
    private Rect decRect, incRect;

    public Lore? Display = null;

    public Icon Icon;
    private int scrollBuf;
    public TextConvert TextRelinker = f => f.ToString("%.2f", CultureInfo.InvariantCulture);

    public float Value, MaxValue, MinValue, StepValue;

    public XSlideBar(XButton dec, XButton inc)
    {
        decrease = dec;
        increase = inc;
        decrease.OnLeftFired += () =>
        {
            Value -= StepValue;
            Check();
        };
        increase.OnLeftFired += () =>
        {
            Value += StepValue;
            Check();
        };
        decRect = decrease.Bound;
        incRect = increase.Bound;
    }

    public void Correct()
    {
        decRect.Locate(Bound.x, Bound.y);
        incRect.Locate(Bound.xprom - incRect.w, Bound.y);
    }

    private void Check()
    {
        //avoid floating value
        if(Value < MinValue - 0.0001f) Value = MaxValue;
        if(Value > MaxValue + 0.0001f) Value = MinValue;
    }

    public override void Update()
    {
        base.Update();

        decrease.Update();
        increase.Update();

        Keyboard input = Keyboard.Global;

        float scr = input.Scroll;

        if(Bound.Contains(Cursor) && IsExposed() && scrollBuf < 0 && scr != 0)
        {
            if(input.ScrollDirection == ScrollDirection.UP)
            {
                Value += StepValue;
                Check();
            }
            else if(input.ScrollDirection == ScrollDirection.DOWN)
            {
                Value -= StepValue;
                Check();
            }

            scrollBuf = 2;
            input.ConsumeCursorScroll();
        }

        scrollBuf--;

        decrease.Update();
        increase.Update();
    }

    public override void Draw(Graphics graphics)
    {
        graphics.DrawIcon(Icon, Bound);

        string special = TextRelinker.Invoke(Value);
        Lore? drw = Display == null ? Lore.Literal(special) : Display?.Combine(Lore.Literal(": " + special));

        graphics.DrawLore((Lore)drw, Bound.xcentral, Bound.y + 4, FontAlign.Center);

        decrease.Draw(graphics);
        increase.Draw(graphics);
    }

}