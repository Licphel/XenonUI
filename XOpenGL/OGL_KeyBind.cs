using XenonUI.Core;

namespace XenonUI.XOpenGL;

public class OGL_KeyBind : KeyBind
{

    public static long InputCheckTicks;
    private byte Press;

    private long PressOccur = -1;

    public void Consume()
    {
        Press = 0;
        PressOccur = -1;
    }

    public int HoldTime()
    {
        if(Press == 0) return 0;

        return (int)(InputCheckTicks - PressOccur - 1);
    }

    public bool Pressed()
    {
        return Press != 0 && (PressOccur == InputCheckTicks || HoldTime() > Application.MaxTps * 2);
    }

    public bool Holding()
    {
        return Press != 0;
    }

    public bool DoublePressed()
    {
        return Press == 2;
    }

    public void Fire(byte i)
    {
        Press = i;
        PressOccur = InputCheckTicks;
    }

}