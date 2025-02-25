using XenonUI.Maths;

namespace XenonUI.Core;

public interface Keyboard
{

	public static Keyboard Global;

	VaryingVector2 Cursor { get; }
	ScrollDirection ScrollDirection { get; }
	float Scroll { get; }
	string ClippedText { get; set; }
	string Text { get; }

	KeyBind Observe(KeyID code);

	KeyBind Observe(int code);

	void ConsumeTextInput();

	void ConsumeCursorScroll();

	void StartRoll();

	void EndRoll();

}

public enum ScrollDirection
{

	NONE,
	UP,
	DOWN

}
