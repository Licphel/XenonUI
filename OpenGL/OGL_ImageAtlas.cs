using OpenTK.Platform;
using XenonIO.IO;
using XenonUI.Core;
using XenonUI.Graph;
using XenonUI.Graph.IMP;
using Bitmap = System.Drawing.Bitmap;
using InternalFormat = OpenTK.Graphics.OpenGL.InternalFormat;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace XenonUI.OpenGL;

public unsafe class OGL_ImageAtlas : ImageAtlas
{

	private readonly int powerx = 12;
	private readonly int powery = 12;
	private int p0, p1;
	private int cx, cy;
	private int mh;

	private OGL_Image glt;
	private Image img;
	private Graphics g;

	public ImageRegion Accept(Image tex)
	{
		if(cx + tex.Width >= p0)
		{
			cy += mh;
			mh = 0;
			cx = 0;
		}
		OGL_Image gltin = (OGL_Image) tex;
		mh = System.Math.Max(tex.Height, mh);
		ImageRegion part = ImageRegion.BySize(glt, cx, cy, tex.Width, tex.Height);
		//g.DrawImage(tex, cx, cy + tex.Height, tex.Width, tex.Height);
		
		fixed(byte* b1 = tex.Data)
		{
			fixed(byte* map = img.Data)
			{
				byte* dst0 = map + cy * p0 * 4 + cx * 4;
				
				for(int x = 0; x < tex.Width; x++)
				{
					for(int y = 0; y < tex.Height; y++)
					{
						byte* p1 = b1 + y * tex.Width * 4 + x * 4;
				
						byte* dst1 = dst0 + y * p0 * 4;
						dst1 += x * 4;

						dst1[0] = p1[0];
						dst1[1] = p1[1];
						dst1[2] = p1[2];
						dst1[3] = p1[3];
					}
				}
			}
		}
		
		cx += tex.Width;
		return part;
	}

	public void Begin()
	{
		glt = new OGL_Image();
		p0 = (int) System.Math.Pow(2, powerx);
		p1 = (int) System.Math.Pow(2, powery);
		img = new ImageRGBA(p0, p1);
		g = img.CreateGraphics();
	}

	public unsafe void End()
	{
		fixed(byte* p = img.Data)
		{
			glt.Upload((IntPtr)p, p0, p1, InternalFormat.Rgba, PixelFormat.Rgba);
		}
		img.Dispose();
	}

}
