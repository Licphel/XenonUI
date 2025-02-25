namespace XenonUI.Graph.UI;

public class XTextView : XElement
{

    public Lore DisplayedLore;
    public SideScroller Scroller = new SideScroller();
    public float TotalSize;

    public override void Update()
    {
        base.Update();

        if(!IsExposed()) return;

        Scroller.TotalSize = TotalSize;
        Scroller.Update(this);
    }

    public override void Draw(Graphics graphics)
    {
        TotalSize = graphics.Font.GetBounds(DisplayedLore, Bound.w - 2).Height;

        var pos = Scroller.Pos;
        var o = Scroller.Outline;

        graphics.Scissor(Bound.x + o, Bound.y + o - 1, Bound.w - o * 2, Bound.h - o * 2 + 2);
        graphics.Draw(DisplayedLore, Bound.x + o, Bound.yprom + pos - graphics.Font.LineH, Bound.w - o * 2);
        graphics.ScissorEnd();

        Scroller.Draw(graphics, this);
    }

}