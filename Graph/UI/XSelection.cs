using XenonUI.Core;
using XenonUI.Maths;

namespace XenonUI.Graph.UI;

public class XSelection : XElement
{

    public static KeyBind CL_CODE = Keyboard.Global.Observe(KeyID.BUTTON_LEFT);
    public static KeyBind CR_CODE = Keyboard.Global.Observe(KeyID.BUTTON_RIGHT);

    public List<Entry> Entries = new List<Entry>();

    public float EntryH;
    public SideScroller Scroller = new SideScroller();
    protected float TotalSize => Entries.Count * (EntryH + 1);

    public void Add(Entry entry)
    {
        Entries.Add(entry);
        entry.Index = Entries.IndexOf(entry);
        entry.Parent = this;
        entry.Cursor = Cursor;
        entry.Activate();
    }

    public override void Update()
    {
        base.Update();

        foreach(Entry t in Entries)
        {
            t.Update();
            if(t.IsHovering() && KeyBind.MouseLeft.Pressed()) t.Pressed();
        }

        if(!IsExposed()) return;

        Scroller.TotalSize = TotalSize;
        Scroller.Update(this);
    }

    public override void Draw(Graphics graphics)
    {
        renderBasement(graphics);

        graphics.Scissor(Bound.x - Scroller.Outline, Bound.y - Scroller.Outline,
            Bound.w + Scroller.Outline * 2, Bound.h + Scroller.Outline * 2);
        renderEntries(graphics);
        graphics.ScissorEnd();

        renderOverlay(graphics);
    }

    public override void CollectTooltips(List<Lore> list)
    {
        base.CollectTooltips(list);

        foreach(Entry t in Entries)
            if(t.IsHovering())
                t.CollectTooltips(list);
    }

    protected void renderEntries(Graphics graphics)
    {
        var per = Bound.h / TotalSize;
        if(per > 1) per = 1;

        var pos = Scroller.Pos;

        for(var i = 0; i < Entries.Count; i++) Entries[i].Correct(pos);
        for(var i = 0; i < Entries.Count; i++) Entries[i].Draw(graphics);

        Scroller.Draw(graphics, this);
    }

    protected void renderBasement(Graphics graphics)
    {
    }

    protected void renderOverlay(Graphics graphics)
    {
    }

    public abstract class Entry
    {

        public XSelection Parent;

        public float w, w0, h;
        public float x;
        public float y;
        protected float y0;

        public int Index { get; set; }
        public VaryingVector2 Cursor { get; set; }

        public virtual void Activate()
        {
            h = Parent.EntryH;
            w0 = w = Parent.Bound.w;
        }

        public virtual void Correct(float posOffset)
        {
            y0 = y = Parent.Bound.yprom - (Index + 1) * (h + 1);
            x = Parent.Bound.x + 1;
            y = y0 + posOffset;
            w = w0 - Parent.Scroller.Outline;
        }

        public virtual void Update()
        {
        }

        public virtual void CollectTooltips(List<Lore> list)
        {
        }

        public virtual void Draw(Graphics graphics)
        {
        }

        public virtual void Pressed()
        {
        }

        public virtual bool IsHovering()
        {
            if(Parent.Scroller.IsDragging) return false; //Avoid accidental action
            var mx = Cursor.x;
            var my = Cursor.y;
            return mx >= x && mx <= x + w && my >= y && my <= y + h && Parent.Bound.Contains(mx, my);
        }

    }

}