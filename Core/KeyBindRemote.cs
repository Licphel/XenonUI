namespace XenonUI.Core;

public class KeyBindRemote : KeyBind
{

	private KeyBind observer;

	public KeyBindRemote(KeyID code)
	{
		Reset(code);
	}

	public void Consume()
	{
		observer.Consume();
	}

	public int HoldTime()
	{
		return observer.HoldTime();
	}

	public bool Pressed()
	{
		return observer.Pressed();
	}

	public bool Holding()
	{
		return observer.Holding();
	}

	public bool DoublePressed()
	{
		return observer.DoublePressed();
	}

	public void Reset(KeyID code)
	{
		observer = Keyboard.Global.Observe(code);
	}

	public void Reset(int code)
	{
		observer =  Keyboard.Global.Observe(code);
	}

}
