﻿using XenonUI.Core;
using XenonUI.Maths;

namespace XenonUI.Graph.UI;

public abstract class XElement
{

    public static XElement Highlighted;

    public Rect Bound;
    public VaryingVector2 Cursor = new VaryingVector2();
    public List<Lore> DefaultAttachedTooltips = new List<Lore>();
    public XUIElementGroup Parent;
    public bool Removed;

    public virtual void Update()
    {
    }

    public virtual void Draw(Graphics graphics)
    {
    }

    public virtual void CollectTooltips(List<Lore> list)
    {
        list.AddRange(DefaultAttachedTooltips);
    }

    public bool IsExposed()
    {
        if(Parent == null) return true;

        var idx = Parent.Values.IndexOf(this);

        for(var i = 0; i < Parent.Values.Count; i++)
        {
            XElement c = Parent.Values[i];
            if(c.Bound.Contains(Cursor) && i > idx) return false;
        }

        //if is in a window
        if(Parent is XPage win)
        {
            var idx1 = win.Parent.Values.IndexOf(win);
            //check in screen other windows.
            for(var i1 = 0; i1 < win.Parent.Values.Count; i1++)
            {
                XElement c1 = win.Parent.Values[i1];
                if(c1.Bound.Contains(Cursor) && i1 < idx1 && c1 is XPage) return false;
            }
        }

        return true;
    }

    public static void GetHighlight(XUIElementGroup group, bool isRoot = true)
    {
        if(!KeyBind.MouseLeft.Pressed()) return;

        if(isRoot) Highlighted = null;

        foreach(XElement c in group.Values)
            if(c.Bound.Contains(Keyboard.Global.Cursor) && c.IsExposed())
            {
                Highlighted = c;
                if(c is XUIElementGroup g1) GetHighlight(g1, false);
            }
    }

}