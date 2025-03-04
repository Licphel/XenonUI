namespace XenonUI.Core;

public class KeyBindRemote : KeyBind
{

    private KeyBind observer;

    public KeyBindRemote(KeyID code)
    {
        Reset(code);
    }

    public void Consume() => observer.Consume();
    public int HoldTime() => observer.HoldTime();
    public bool Pressed() => observer.Pressed();
    public bool Holding() => observer.Holding();
    public bool DoublePressed() => observer.DoublePressed();
    public void Reset(KeyID code) => observer = Keyboard.Global.Observe(code);
    public void Reset(int code) => observer = Keyboard.Global.Observe(code);

}