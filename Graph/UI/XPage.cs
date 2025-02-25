using XenonUI.Core;
using XenonUI.Maths;

namespace XenonUI.Graph.UI;

public class XPage : XElement, XUIElementGroup
{

    private XButton closer;
    public Vector2 CloserOffset;

    public XUIElementManager Container = new XUIElementManager();

    private bool dragging;
    public Icon Icon;
    public int LabelH;
    private float lcx, lcy;
    public Lore Title;
    public Vector2 TitleOffset;

    public T Join<T>(T component) where T : XElement
    {
        return Container.Join(component);
    }

    public void Remove(XElement stru)
    {
        Container.Remove(stru);
    }

    public void Ascend(XElement stru)
    {
        Container.Ascend(stru);
    }

    public void UpdateComponents(VaryingVector2 cursor, Vector2 tls)
    {
        Container.UpdateComponents(cursor, tls);
    }

    public List<XElement> Values => Container.Values;

    public void SetCloseButton(XButton cls)
    {
        closer = cls;
        closer.OnLeftFired += () => Removed = true;
        closer.Bound.Locate(Bound.w + CloserOffset.x, Bound.h + CloserOffset.y);
        Join(closer);
    }

    public override void Update()
    {
        base.Update();

        UpdateComponents(Cursor, new Vector2(Bound.x, Bound.y));

        if(!IsExposed()) return;

        if(!dragging && KeyBind.MouseLeft.Pressed() && Bound.Contains(Cursor) && Cursor.y >= Bound.yprom - LabelH)
        {
            dragging = true;
            lcx = Cursor.x;
            lcy = Cursor.y;
            Parent.Ascend(this);
        }

        if(!KeyBind.MouseLeft.Holding())
        {
            dragging = false;
        }
        else if(dragging)
        {
            float nx = Cursor.x, ny = Cursor.y;

            Bound.Translate(nx - lcx, ny - lcy);

            lcx = nx;
            lcy = ny;
        }
    }

    public override void CollectTooltips(List<Lore> list)
    {
        base.CollectTooltips(list);

        foreach(XElement c in Container.Components)
        {
            c.Bound.Translate(Bound.x, Bound.y);
            if(c.Bound.Contains(Cursor))
                c.CollectTooltips(list);
            c.Bound.Translate(-Bound.x, -Bound.y);
        }
    }

    public override void Draw(Graphics graphics)
    {
        graphics.Draw(Icon, Bound);

        base.Draw(graphics);

        foreach(XElement c in Container.Components)
        {
            c.Bound.Translate(Bound.x, Bound.y);
            c.Draw(graphics);
            c.Bound.Translate(-Bound.x, -Bound.y);
        }

        graphics.Draw(Title, Bound.x + TitleOffset.x, Bound.yprom - LabelH + TitleOffset.y);
    }

}