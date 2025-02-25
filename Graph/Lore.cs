using System.Text;
using XenonUI.Core;
using XenonUI.Maths;

namespace XenonUI.Graph;

public struct Lore
{

	public List<(LoreStyle, Func<string>)> Content = new List<(LoreStyle, Func<string>)>();
	public LoreStyle _LatestSP;

	public Lore() {}

	public string Summary
	{
		get
		{
			var sb = new StringBuilder();
			foreach(var s in Content)
				sb.Append(s.Item2());
			return sb.ToString();
		}
	}

	public static Lore Dynamic(Func<string> text)
	{
		Lore lr = new Lore();
		lr.Content.Add((lr._LatestSP = new LoreStyle(), text));
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

	public Lore Style(LoreStyle sp)
	{
		int i = Content.Count - 1;
		Content[i] = (sp, Content[i].Item2);
		return this;
	}

	public Lore Paint(Color c)
	{
		_LatestSP.Color = c;
		return this;
	}

	public Lore Paint(float r, float g, float b, float a = 1)
	{
		_LatestSP.Color = new Color(r, g, b, a);
		return this;
	}
	
	public Lore Outline(Color c)
	{
		_LatestSP.Outline = true;
		_LatestSP.OutlineColor = c;
		return this;
	}

	public Lore Outline(float r, float g, float b, float a = 1)
	{
		_LatestSP.Outline = true;
		_LatestSP.OutlineColor = new Color(r, g, b, a);
		return this;
	}
	
	public Lore Bold()
	{
		_LatestSP.Bold = true;
		return this;
	}
	
	public Lore Italic()
	{
		_LatestSP.Italic = true;
		return this;
	}
	
	public Lore Deleted()
	{
		_LatestSP.Deleted = true;
		return this;
	}
	
	public Lore Underlined()
	{
		_LatestSP.Underlined = true;
		return this;
	}
	
	public Lore Combine(Lore l1)
	{
		foreach(var v in l1.Content)
		{
			Content.Add(v);
		}
		return this;
	}

}
