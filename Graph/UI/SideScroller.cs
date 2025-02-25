using XenonUI.Core;

namespace XenonUI.Graph.UI;

public class SideScroller
{

    private float accels;

    private float bx, by, bw, bh;

    public bool IsDragging;
    private float lcy, sp0;
    public float Outline;
    private float prevPos;
    public float Speed = 250;
    private float startPos;
    public float TotalSize;

    public SideScroller(float otl = 0)
    {
        Outline = otl;
        startPos = -Outline;
    }

    public float Pos => Time.PartialTicks * (startPos - prevPos) + prevPos;

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
        if(startPos <= -Outline) startPos = -Outline;

        if(startPos - TotalSize - Outline >= -c.Bound.h) startPos = TotalSize - c.Bound.h + Outline;

        if(TotalSize + Outline * 2 < c.Bound.h) startPos = -Outline;
    }

    public void Update(XElement c)
    {
        Keyboard input = Keyboard.Global;

        var scr = input.Scroll;

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
                var mx = c.Cursor.x;
                var my = c.Cursor.y;

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
        var per = (c.Bound.h - Outline * 2) / TotalSize;
        if(per > 1) per = 1;

        var scrollPer = Math.Abs(Pos) / TotalSize;
        var h = c.Bound.h * per;
        var oh = scrollPer * c.Bound.h;

        bh = h;
        bx = c.Bound.xprom - 5;
        by = c.Bound.yprom - oh - h;
        bw = 2;

        if(per < 1) graphics.DrawRect(bx, by, bw, bh);
    }

}