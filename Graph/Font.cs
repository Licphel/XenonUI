namespace XenonUI.Graph;

public abstract class Font
{

	public static string ASCII = "!@#$%^&*()_+-=[]{}|\\;':\"<>,./?~`ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
	
	public float Scale = 1f;
	public float Size;
	
	public abstract Glyph GetGlyph(char ch);
	public abstract float LineH { get; }
	public abstract float ScaledAndBlankedLineH { get; }

	public GlyphBounds GetBounds(string text, float maxWidth = int.MaxValue)
	{
		float width = 0;
		float lineWidth = 0;
		float height = 0;
		float lineHeight = ScaledAndBlankedLineH;
		bool needNewLine = false;

		for(int i = 0; i < text.Length; i++)
		{
			char c = text[i];

			if(c == '\n' || needNewLine)
			{
				height += lineHeight;
				width = System.Math.Max(lineWidth, width);
				lineWidth = 0;
				needNewLine = false;
				continue;
			}

			float w = GetGlyph(c).Advance * Scale;

			if(lineWidth + w >= maxWidth)
			{
				needNewLine = true;
				i -= 2;
				continue;
			}

			lineWidth += w;
		}

		height += lineHeight;
		width = System.Math.Max(lineWidth, width);

		return new GlyphBounds(text, width, height, lineWidth);
	}

	public GlyphBounds GetBounds(Lore text, float maxWidth = int.MaxValue)
	{
		return GetBounds(text.Summary, maxWidth);
	}

}