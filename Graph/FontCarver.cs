using XenonUI.Graph.IMP;

namespace XenonUI.Graph;

public abstract class FontCarver
{

	public int MaxDrawableChars = int.MaxValue;

	public abstract void Draw(Graphics graphics, Font font, string text, float x, float y, float maxw);

	public abstract void Draw(Graphics graphics, Font font, Lore text, float x, float y, float maxw);

}

public class DefaultFontCarver : FontCarver
{

	public override void Draw(Graphics graphics, Font font, string text, float x, float y, float maxw)
	{
		if(string.IsNullOrWhiteSpace(text))
		{
			return;
		}

		float fontHeight = font.ScaledAndBlankedLineH;

		float drawX = x;
		float drawY = y;

		bool newLine = false;

		for(int i = 0; i < text.Length && i < MaxDrawableChars; i++)
		{
			char ch = text[i];

			if(ch == '\n' || newLine)
			{
				drawY -= fontHeight;
				drawX = x;
				newLine = false;
				continue;
			}

			var g = font.GetGlyph(ch) * font.Scale;
		
			if(drawX - x + g.Advance >= maxw)
			{
				newLine = true;
				i -= 2;
				continue;
			}

			ImageRegion map = g.Src;
			
			graphics.Draw(map, drawX + g.Bearing, drawY + g.Sink, g.Width, g.Height);
			drawX += g.Advance;
		}
	}

	public override void Draw(Graphics graphics, Font font, Lore text, float x, float y, float maxw)
	{
		graphics.PushColor();

		float fontHeight = font.ScaledAndBlankedLineH;
		
		float drawX = x;
		float drawY = y;

		bool newLine = false;

		foreach(var v in text.Content)
		{
			LoreStyle sp = v.Item1;
			string text0 = v.Item2();
			graphics.Color4(sp.Color);

			for(int i = 0; i < text0.Length && i < MaxDrawableChars; i++)
			{
				char ch = text0[i];
				var g = font.GetGlyph(ch) * font.Scale;
				float w = g.Width;
				float h = g.Height;
				char ch1 = ch;

				if(ch == '\n' || newLine)
				{
					drawY -= fontHeight;
					drawX = x;
					newLine = false;
					continue;
				}

				if(drawX - x + w >= maxw)
				{
					newLine = true;
					i -= 2;
					continue;
				}

				ImageRegion map = g.Src;
				float gw = map.Width, gh = map.Height;
				
				if(!sp.Bold && !sp.Outline)
				{
					DrawOnce(graphics, map, sp, drawX, drawY, w, h, gw, gh);
				}
				else
				{
					graphics.PushColor();
					if(sp.Outline)
					{
						graphics.Color4(sp.OutlineColor);
					}
					DrawOnce(graphics, map, sp, drawX + g.Bearing + 0.5f, drawY + g.Sink + 0.25f, w, h, gw, gh);
					DrawOnce(graphics, map, sp, drawX + g.Bearing - 0.5f, drawY + g.Sink + 0.25f, w, h, gw, gh);
					DrawOnce(graphics, map, sp, drawX + g.Bearing + 0.5f, drawY + g.Sink - 0.25f, w, h, gw, gh);
					DrawOnce(graphics, map, sp, drawX + g.Bearing - 0.5f, drawY + g.Sink - 0.25f, w, h, gw, gh);

					graphics.PopColor();
					DrawOnce(graphics, map, sp, drawX + g.Bearing, drawY + g.Sink, w, h, gw, gh);
				}

				if(sp.Underlined)
				{
					graphics.DrawRect(drawX, drawY, w, 1);
				}
				if(sp.Deleted)
				{
					graphics.DrawRect(drawX, drawY + fontHeight / 2f - 2 * font.Scale, w, 1);
				}
				
				drawX += g.Advance;
			}
		}

		graphics.PopColor();

		return;

		static void DrawOnce(Graphics graphics, ImageRegion map, LoreStyle sp, float drawX, float drawY, float w, float h, float gw, float gh)
		{
			float gx = map.U, gy = map.V;
			if(sp.Italic)
			{
				graphics.DrawImage(map.Src, drawX, drawY, w, h / 3f, gx, gy + gh / 3f * 2f, gw, gh / 3f);
				graphics.DrawImage(map.Src, drawX + 0.5f, drawY + h / 3f, w, h / 3f, gx, gy + gh / 3f, gw, gh / 3f);
				graphics.DrawImage(map.Src, drawX + 1f, drawY + h / 3f * 2, w, h / 3f, gx, gy, gw, gh / 3f);
			}
			else
			{
				graphics.DrawImage(map.Src, drawX, drawY, w, h, gx, gy, gw, gh);
			}
		}
	}

}
