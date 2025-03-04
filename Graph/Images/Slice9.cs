using XenonUI.Maths;

namespace XenonUI.Graph.Images;

public class Slice9 : Icon
{

    private readonly Image B;
    private readonly Image C;
    private readonly Image L;
    private readonly Image LB;

    private readonly Image LT;
    private readonly Image R;
    private readonly Image RB;
    private readonly Image RT;

    private readonly float scale;
    private readonly Image T;
    private readonly float th;

    private readonly float tw;

    //Sometimes we don't want the texture parts to overlap because of w % tw != 0 or h % th != 0.
    //Generally, when the texture has transparent parts, turn it off.
    public bool AllowOverlapping = true;

    public Slice9(Image texture) : this(texture, 1)
    {
    }

    public Slice9(Image tex, float scale)
    {
        this.scale = scale;

        const float p13 = 1f / 3;
        const float p23 = 2f / 3;

        tw = p13 * tex.Width * scale;
        th = p13 * tex.Height * scale;

        LT = ByPercentSize(tex, 0, 0, p13, p13);
        T = ByPercentSize(tex, p13, 0, p13, p13);
        RT = ByPercentSize(tex, p23, 0, p13, p13);
        L = ByPercentSize(tex, 0, p13, p13, p13);
        C = ByPercentSize(tex, p13, p13, p13, p13);
        R = ByPercentSize(tex, p23, p13, p13, p13);
        LB = ByPercentSize(tex, 0, p23, p13, p13);
        B = ByPercentSize(tex, p13, p23, p13, p13);
        RB = ByPercentSize(tex, p23, p23, p13, p13);
        
        static Image ByPercentSize(Image tex, float u, float v, float w, float h)
        {
            return Image.Subimage(tex, tex.Width * u, tex.Height * v, tex.Width * w, tex.Width * h);
        }
    }

    public bool IsFixedSize => false;
    public int Width => -1;
    public int Height => -1;

    public void InternalPerform(Graphics graphics, float x, float y, float w, float h)
    {
        float nw = CeilW(w);
        float nh = CeilH(h);
        float x2 = x + (AllowOverlapping ? w - tw : ActualW(w));
        float y2 = y + (AllowOverlapping ? h - th : ActualW(h));

        for(int i = 1; i < nw - 1; i++)
        for(int j = 1; j < nh - 1; j++)
            graphics.DrawImage(C, x + i * tw, y + j * th, tw, th);
        graphics.DrawImage(LB, x, y, tw, th);
        for(int i = 1; i < nh - 1; i++) 
            graphics.DrawImage(L, x, y + i * th, tw, th);
        for(int i = 1; i < nw - 1; i++) 
            graphics.DrawImage(B, x + i * tw, y, tw, th);
        graphics.DrawImage(RB, x2, y, tw, th);
        for(int i = 1; i < nh - 1; i++) 
            graphics.DrawImage(R, x2, y + i * th, tw, th);
        graphics.DrawImage(LT, x, y2, tw, th);
        for(int i = 1; i < nw - 1; i++) 
            graphics.DrawImage(T, x + i * tw, y2, tw, th);
        graphics.DrawImage(RT, x2, y2, tw, th);
    }

    public float CeilW(float mw)
    {
        return Mathf.Ceiling(mw / tw);
    }

    public float CeilH(float mh)
    {
        return Mathf.Ceiling(mh / th);
    }

    public float ActualW(float mw)
    {
        return tw * (CeilW(mw) - 1);
    }

    public float ActualH(float mh)
    {
        return th * (CeilH(mh) - 1);
    }

}