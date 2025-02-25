using KryptonM.Maths;
using OpenTK.Windowing.GraphicsLibraryFramework;
using XenonUI.Core;
using XenonUI.Maths;

namespace XenonUI.OpenGL;

public class OGL_Keyboard : Keyboard
{

    public Dictionary<int, OGL_KeyBind> Observers = new Dictionary<int, OGL_KeyBind>();
    public int PCcountdown;
    public string PileChars = "";
    public float ScrollUABS;

    public float Scroll
    {
        get => FloatMath.Abs(ScrollUABS);
        set => ScrollUABS = value;
    }

    public VaryingVector2 Cursor { get; } = new VaryingVector2();

    public KeyBind Observe(KeyID code)
    {
        return Observe((int)code);
    }

    public KeyBind Observe(int code)
    {
        if(!Observers.ContainsKey(code)) Observers[code] = new OGL_KeyBind();

        OGL_KeyBind obs = Observers[code];

        return obs;
    }

    public unsafe string ClippedText
    {
        get => GLFW.GetClipboardString(OGL.Window);
        set => GLFW.SetClipboardString(OGL.Window, value);
    }

    public string Text => PileChars;

    public void ConsumeTextInput()
    {
        PileChars = "";
        PCcountdown = 2;
    }

    public void ConsumeCursorScroll()
    {
        Scroll = 0;
    }

    public ScrollDirection ScrollDirection
    {
        get
        {
            if(ScrollUABS > 0) return ScrollDirection.UP;

            if(ScrollUABS < 0) return ScrollDirection.DOWN;

            return ScrollDirection.NONE;
        }
    }

    public void StartRoll()
    {
        PCcountdown--;
    }

    public void EndRoll()
    {
        OGL_KeyBind.InputCheckTicks++;
    }

}