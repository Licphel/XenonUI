using OpenTK.Windowing.GraphicsLibraryFramework;
using XenonUI.Graph;
using XenonUI.Maths;

namespace XenonUI.OpenGL;

public class OGL_GraphicsDevice : GraphicsDevice
{

	public unsafe Vector2 Size
	{
		get
		{
			GLFW.GetWindowSize(OGL.Window, out int x, out int y);
			return new Vector2(x, y);
		}
		set => GLFW.SetWindowSize(OGL.Window, (int) value.x, (int) value.y);
	}

	public unsafe Vector2 DeviceSize
	{
		get
		{
			VideoMode* vm = GLFW.GetVideoMode(GLFW.GetPrimaryMonitor());
			return new Vector2(vm->Width, vm->Height);
		}
	}

	public unsafe Vector2 Pos
	{
		get
		{
			GLFW.GetWindowPos(OGL.Window, out int x, out int y);
			return new Vector2(x, y);
		}
		set => GLFW.SetWindowPos(OGL.Window, (int) value.x, (int) value.y);
	}

	public unsafe bool Decorated
	{
		set => GLFW.SetWindowAttrib(OGL.Window, WindowAttribute.Decorated, value);
	}

	public unsafe void Maximize()
	{
		GLFW.MaximizeWindow(OGL.Window);
	}

	public unsafe string Title
	{
		set => GLFW.SetWindowTitle(OGL.Window, value);
	}

	public unsafe void SwapBuffer()
	{
		GLFW.SwapBuffers(OGL.Window);
	}

}
