using System.Text;
using KryptonM.IDM;
using XenonUI.Graph.Images;

namespace XenonUI.Graph.Text;

public struct Lore
{

    public List<(LoreStyle, object)> Content = new List<(LoreStyle, object)>();
    public LoreStyle _LatestStyle;

    public Lore()
    {
    }

    public string Summary
    {
        get
        {
            StringBuilder sb = new StringBuilder();
            foreach((LoreStyle, object) s in Content)
            {
                switch(s.Item2)
                {
                    case Func<string> fn:
                        sb.Append(fn());
                        break;
                    case Icon _:
                        sb.Append("[Icon]");
                        break;
                }
            }
            return sb.ToString();
        }
    }

    public static Lore Dynamic(Func<string> text)
    {
        Lore lr = new Lore();
        lr.Content.Add((lr._LatestStyle = new LoreStyle(), text));
        return lr;
    }

    public static Lore Literal(string text = "")
    {
        return Dynamic(() => text);
    }

    public static Lore Translate(string text, params string[] repmt)
    {
        return Dynamic(() => I18N.GetText(text, repmt));
    }
    
    public static Lore Icon(Icon icon)
    {
        Lore lr = new Lore();
        lr.Content.Add((lr._LatestStyle = new LoreStyle(), icon));
        return lr;
    }

    public Lore Style(LoreStyle sp)
    {
        int i = Content.Count - 1;
        Content[i] = (sp, Content[i].Item2);
        return this;
    }

    public Lore Paint(Color c)
    {
        _LatestStyle.Color = c;
        return this;
    }

    public Lore Paint(float r, float g, float b, float a = 1)
    {
        _LatestStyle.Color = new Color(r, g, b, a);
        return this;
    }

    public Lore Outline(Color c)
    {
        _LatestStyle.Outline = true;
        _LatestStyle.OutlineColor = c;
        return this;
    }

    public Lore Outline(float r, float g, float b, float a = 1)
    {
        _LatestStyle.Outline = true;
        _LatestStyle.OutlineColor = new Color(r, g, b, a);
        return this;
    }

    public Lore Bold()
    {
        _LatestStyle.Bold = true;
        return this;
    }

    public Lore Italic()
    {
        _LatestStyle.Italic = true;
        return this;
    }

    public Lore Deleted()
    {
        _LatestStyle.Deleted = true;
        return this;
    }

    public Lore Underlined()
    {
        _LatestStyle.Underlined = true;
        return this;
    }

    public Lore Combine(Lore l1)
    {
        foreach((LoreStyle, object) v in l1.Content) 
            Content.Add(v);
        return this;
    }

}