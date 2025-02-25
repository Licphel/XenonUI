using XenonIO.IO;
using XenonUI.Maths;

namespace XenonUI.OpenGL;

public class OGL_Config
{

	public static int MipmapLevel = 0;
	public bool AutoIconify = true;

	public Vector4 ClearColor = new Vector4(0, 0, 0, 1);
	public FileHandle Cursor;
	public bool Decorated = true;
	public bool Floating = false;
	public Vector2 Hotspot = new Vector2(0, 0);
	public FileHandle[] Icons;
	public bool Maximized = false;
	public bool Resizable = true;

	public Vector2 Size = new Vector2(128, 128);

	public string Title = "Kinetic OpenGL";

}
