using System.Text;
using XenonUI.Core;
using XenonUI.Graph.Images;
using XenonUI.Graph.Text;

namespace XenonUI.Graph.UI;

public class XTextField : XElement
{

    public static KeyBind A_CODE = Keyboard.Global.Observe(KeyID.KEY_A);
    public static KeyBind C_CODE = Keyboard.Global.Observe(KeyID.KEY_C);
    public static KeyBind V_CODE = Keyboard.Global.Observe(KeyID.KEY_V);
    public static KeyBind LD_CODE = Keyboard.Global.Observe(KeyID.KEY_LEFT);
    public static KeyBind RD_CODE = Keyboard.Global.Observe(KeyID.KEY_RIGHT);

    public static int CURSOR_SHINE_TIME = Application.MaxTps / 3;
    private readonly StringBuilder text = new StringBuilder();

    private int clock;
    private bool cursorOn;
    public Icon[] Icons = new Icon[3];

    public string InputHint = "";
    private int pointer;
    public SideScroller Scroller = new SideScroller();
    private bool selectAll;
    public float TotalSize;
    private bool uppos;

    public string Text
    {
        get => text.ToString();
        set
        {
            text.Clear();
            text.Append(value);
            pointer = text.Length;
        }
    }

    public override void Update()
    {
        base.Update();

        clock++;

        cursorOn = false;

        if(!IsExposed()) return;

        Scroller.TotalSize = TotalSize;
        Scroller.Update(this);

        bool boundIn = Bound.Contains(Cursor);
        bool pressed = KeyBind.MouseLeft.Pressed();

        Keyboard input = Keyboard.Global;

        if(Highlighted == this)
        {
            //Common Input Operations
            string txt = input.Text;
            if(!string.IsNullOrEmpty(txt))
            {
                insert(txt);
                input.ConsumeTextInput();
            }

            //Process Clipboard Operations
            if(KeyBind.KeyCtrl.Holding())
            {
                if(V_CODE.Pressed()) insert(input.ClippedText);
                if(A_CODE.Pressed()) selectAll = !selectAll;
                if(C_CODE.Pressed() && selectAll)
                {
                    input.ClippedText = text.ToString();
                    selectAll = false;
                }
            }

            if(KeyBind.KeyEnter.Pressed()) insert("\n");
            //END

            //Pointer Move Operations
            if(LD_CODE.Pressed()) pointer = Math.Max(0, pointer - 1);
            if(RD_CODE.Pressed()) pointer = Math.Min(text.Length, pointer + 1);
            //Backspace Operations
            if(KeyBind.KeyBackspace.Pressed())
            {
                if(text.Length > 0 && pointer != 0)
                {
                    pointer = Math.Max(0, pointer - 1);
                    text.Remove(pointer, 1);
                }

                if(selectAll)
                {
                    pointer = 0;
                    text.Clear();
                    selectAll = false;
                }
            }
        }
    }

    private void insert(string txt)
    {
        text.Insert(pointer, txt);
        pointer += txt.Length;
        uppos = true;
    }

    public override void Draw(Graphics graphics)
    {
        if(Highlighted == this)
            graphics.DrawIcon(Icons[2], Bound);
        else if(cursorOn)
            graphics.DrawIcon(Icons[1], Bound);
        else
            graphics.DrawIcon(Icons[0], Bound);

        if(text.Length == 0)
            renderInBox(graphics, InputHint, new Color(1, 1, 1, 0.2f));
        else
            renderInBox(graphics, Text, selectAll ? new Color(0.2f, 0.6f, 1f) : new Color(1, 1, 1));
    }

    private void renderInBox(Graphics graphics, string textIn, Color color)
    {
        float x = Bound.x + Scroller.Outline;
        float y = Bound.yprom - graphics.Font.LineH - Scroller.Outline;

        graphics.Color4(color);
        TotalSize = graphics.Font.GetBounds(textIn, Bound.w - Scroller.Outline * 2).Height;

        if(uppos)
        {
            uppos = false;
            Scroller.TotalSize = TotalSize;
            Scroller.DownToGround(this);
        }

        float pos = Scroller.Pos;
        float o = Scroller.Outline;

        graphics.Scissor(Bound.x + o, Bound.y + o - 1, Bound.w - o * 2, Bound.h - o * 2 + 2);
        graphics.DrawText(textIn, Bound.x + o, Bound.yprom + pos - graphics.Font.LineH, Bound.w - o * 2);
        graphics.ScissorEnd();

        Scroller.Draw(graphics, this);

        if(Highlighted == this && clock % CURSOR_SHINE_TIME > CURSOR_SHINE_TIME / 2)
        {
            GlyphBounds bounds = graphics.Font.GetBounds(textIn.Substring(0, pointer), Bound.w - Scroller.Outline * 2);
            //do not use emptyDisplay
            x += bounds.LastWidth;
            y -= bounds.Height - o - graphics.Font.LineH - pos;

            graphics.NormalizeColor();
            if(Bound.Contains(x, y)) graphics.DrawRect(x, y, 1, graphics.Font.LineH);
        }

        graphics.NormalizeColor();
    }

}