namespace XenonUI.Core;

public interface KeyBind
{

    public static KeyBindRemote Any = new KeyBindRemote(KeyID.ANY);
    public static KeyBindRemote MouseLeft = new KeyBindRemote(KeyID.BUTTON_LEFT);
    public static KeyBindRemote MouseRight = new KeyBindRemote(KeyID.BUTTON_RIGHT);
    public static KeyBindRemote MouseMid = new KeyBindRemote(KeyID.BUTTON_MIDDLE);
    public static KeyBindRemote KeySpace = new KeyBindRemote(KeyID.KEY_SPACE);
    public static KeyBindRemote KeyShift = new KeyBindRemote(KeyID.KEY_LEFT_SHIFT);
    public static KeyBindRemote KeyCtrl = new KeyBindRemote(KeyID.KEY_LEFT_CONTROL);
    public static KeyBindRemote KeyAlt = new KeyBindRemote(KeyID.KEY_LEFT_ALT);
    public static KeyBindRemote KeyEnter = new KeyBindRemote(KeyID.KEY_ENTER);
    public static KeyBindRemote KeyBackspace = new KeyBindRemote(KeyID.KEY_BACKSPACE);

    void Consume();

    int HoldTime();

    bool Pressed();

    bool Holding();

    bool DoublePressed();

}