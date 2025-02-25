using System.Reflection;
using KryptonM.IO;
using OpenTK;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;
using XenonUI.Core;
using XenonUI.Graph;
using XenonUI.Maths;

namespace XenonUI.OpenGL;

public class OGL
{

    public static unsafe Window* Window;

    public static OGL_Config Settings;
    public static int Pw, Ph;

    public static void LoadSettings(OGL_Config s)
    {
        Settings = s;
        Pw = (int)s.Size.x;
        Ph = (int)s.Size.y;
        Application.ScaledSize = new VaryingVector2(Pw, Ph);
    }

    public static unsafe void OpenWindow()
    {
        GLFW.Init();
        GLFW.SwapInterval(1);
        GLFW.DefaultWindowHints();
        GLFW.WindowHint(WindowHintBool.Decorated, Settings.Decorated);
        GLFW.WindowHint(WindowHintBool.Floating, Settings.Floating);
        GLFW.WindowHint(WindowHintBool.Resizable, Settings.Resizable);
        GLFW.WindowHint(WindowHintBool.Maximized, Settings.Maximized);
        GLFW.WindowHint(WindowHintBool.AutoIconify, Settings.AutoIconify);
        GLFW.WindowHint(WindowHintBool.FocusOnShow, true);
        GLFW.WindowHint(WindowHintBool.Visible, false);

        Window = GLFW.CreateWindow(Pw, Ph, Settings.Title, null, null);

        VideoMode* vm = GLFW.GetVideoMode(GLFW.GetPrimaryMonitor());
        var x = (vm->Width - Settings.Size.x) / 2;
        var y = (vm->Height - Settings.Size.y) / 2;
        GLFW.SetWindowPos(Window, (int)x, (int)y);

        if(Settings.Cursor != null)
        {
            ImageResult result = ImageResult.FromMemory(File.ReadAllBytes(Settings.Cursor.Path),
                ColorComponents.RedGreenBlueAlpha);

            fixed(byte* ptr = result.Data)
            {
                Image cimg = new Image(result.Width, result.Height, ptr);
                Cursor* cptr = GLFW.CreateCursor(cimg, Settings.Hotspot.xi, Settings.Hotspot.yi);
                GLFW.SetCursor(Window, cptr);
            }
        }

        if(Settings.Icons != null)
        {
            List<Image> img = new List<Image>();

            foreach(FileHandle icon in Settings.Icons)
            {
                ImageResult result =
                    ImageResult.FromMemory(File.ReadAllBytes(icon.Path), ColorComponents.RedGreenBlueAlpha);

                fixed(byte* ptr = result.Data)
                {
                    Image cimg = new Image(result.Width, result.Height, ptr);
                    img.Add(cimg);
                }
            }

            GLFW.SetWindowIcon(Window, new ReadOnlySpan<Image>(img.ToArray()));
        }

        if(Settings.Maximized) GLFW.MaximizeWindow(Window);

        GLFW.MakeContextCurrent(Window);
        GLFW.ShowWindow(Window);

        IBindingsContext ctx = new GLFWBindingsContext();
        Assembly assembly = Assembly.Load("OpenTK.Graphics");
        if(assembly == null)
            throw new NullReferenceException("Cannot find OpenTK.Graphics library.");
        Type type = assembly.GetType("OpenTK.Graphics.GLLoader");
        if(type == null)
            throw new NullReferenceException("Cannot find type OpenTK.Graphics.GLLoader.");
        MethodInfo method = type.GetMethod("LoadBindings");
        if(method == null)
            throw new NullReferenceException("Cannot find method OpenTK.Graphics.GLLoader.LoadBindings.");
        method.Invoke(null, [ctx]);

        OnLoad();
    }

    public static unsafe void OnLoad()
    {
        Time.SysNanotimer = () => GLFW.GetTime() * 1000_000_000;
        GraphicsDevice.Global = new OGL_GraphicsDevice();
        Keyboard.Global = new OGL_Keyboard();
        Graphics.Global = new OGL_Graphics(4096);

        Application.Update += GLFW.PollEvents;

        GLFW.SetWindowCloseCallback(Window, _ => { Application.Stop(); });
        GLFW.SetCharCallback(Window, (_, codepoint) =>
        {
            OGL_Keyboard state = (OGL_Keyboard)Keyboard.Global;
            if(state.PCcountdown <= 0)
                state.PileChars += (char)codepoint;
        });
        GLFW.SetKeyCallback(Window, (_, key, _, action, _) =>
        {
            OGL_Keyboard state = (OGL_Keyboard)Keyboard.Global;
            OGL_KeyBind observer = (OGL_KeyBind)state.Observe((int)key);
            OGL_KeyBind observer2 = (OGL_KeyBind)state.Observe((int)KeyID.ANY);
            switch(action)
            {
                case InputAction.Press:
                    observer.Fire(1);
                    observer2.Fire(1);
                    break;
                case InputAction.Repeat:
                    observer.Fire(2);
                    observer2.Fire(2);
                    break;
                case InputAction.Release:
                    observer.Consume();
                    observer2.Consume();
                    break;
            }
        });
        GLFW.SetMouseButtonCallback(Window, (_, key, action, _) =>
        {
            OGL_Keyboard state = (OGL_Keyboard)Keyboard.Global;
            OGL_KeyBind observer = (OGL_KeyBind)state.Observe((int)key);
            OGL_KeyBind observer2 = (OGL_KeyBind)state.Observe((int)KeyID.ANY);
            switch(action)
            {
                case InputAction.Press:
                    observer.Fire(1);
                    observer2.Fire(1);
                    break;
                case InputAction.Repeat:
                    observer.Fire(2);
                    observer2.Fire(2);
                    break;
                case InputAction.Release:
                    observer.Consume();
                    observer2.Consume();
                    break;
            }
        });
        GLFW.SetCursorPosCallback(Window, (_, x, y) =>
        {
            OGL_Keyboard state = (OGL_Keyboard)Keyboard.Global;
            state.Cursor.x = (float)x;
            state.Cursor.y = (float)(GraphicsDevice.Global.Size.y - y);
        });
        GLFW.SetScrollCallback(Window, (_, _, y) =>
        {
            OGL_Keyboard state = (OGL_Keyboard)Keyboard.Global;
            state.ScrollUABS = (float)y;
        });
        GLFW.SetWindowSizeCallback(Window, (_, width, height) =>
        {
            Application._CallResize();
            Pw = width;
            Ph = height;
        });
    }

}