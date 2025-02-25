using KryptonM.Maths;
using XenonUI.Graph.IMP;

namespace XenonUI.Graph;

public class NinePatch : Icon
{

    private readonly ImageRegion B;
    private readonly ImageRegion C;
    private readonly ImageRegion L;
    private readonly ImageRegion LB;

    private readonly ImageRegion LT;
    private readonly ImageRegion R;
    private readonly ImageRegion RB;
    private readonly ImageRegion RT;

    private readonly float scale;
    private readonly ImageRegion T;
    private readonly float th;

    private readonly float tw;

    //Sometimes we don't want the texture parts to overlap because of w % tw != 0 or h % th != 0.
    //Generally, when the texture has transparent parts, turn it off.
    public bool AllowOverlapping = true;

    public NinePatch(ImageRegion texture) : this(texture, 1)
    {
    }

    public NinePatch(ImageRegion tex, float scale)
    {
        this.scale = scale;

        const float p13 = 1f / 3;
        const float p23 = 2f / 3;

        tw = p13 * tex.Width * scale;
        th = p13 * tex.Height * scale;

        LT = ImageRegion.ByPercentSize(tex, 0, 0, p13, p13);
        T = ImageRegion.ByPercentSize(tex, p13, 0, p13, p13);
        RT = ImageRegion.ByPercentSize(tex, p23, 0, p13, p13);
        L = ImageRegion.ByPercentSize(tex, 0, p13, p13, p13);
        C = ImageRegion.ByPercentSize(tex, p13, p13, p13, p13);
        R = ImageRegion.ByPercentSize(tex, p23, p13, p13, p13);
        LB = ImageRegion.ByPercentSize(tex, 0, p23, p13, p13);
        B = ImageRegion.ByPercentSize(tex, p13, p23, p13, p13);
        RB = ImageRegion.ByPercentSize(tex, p23, p23, p13, p13);
    }

    public void Draw(Graphics graphics, float x, float y, float w, float h)
    {
        var nw = CeilW(w);
        var nh = CeilH(h);
        var x2 = x + (AllowOverlapping ? w - tw : ActualW(w));
        var y2 = y + (AllowOverlapping ? h - th : ActualW(h));

        for(var i = 1; i < nw - 1; i++)
        for(var j = 1; j < nh - 1; j++)
            C.Draw(graphics, x + i * tw, y + j * th, tw, th); //center draw
        LB.Draw(graphics, x, y, tw, th);
        for(var i = 1; i < nh - 1; i++) L.Draw(graphics, x, y + i * th, tw, th); //left draw
        for(var i = 1; i < nw - 1; i++) B.Draw(graphics, x + i * tw, y, tw, th); //bottom draw
        RB.Draw(graphics, x2, y, tw, th);
        for(var i = 1; i < nh - 1; i++) R.Draw(graphics, x2, y + i * th, tw, th); //right draw
        LT.Draw(graphics, x, y2, tw, th);
        for(var i = 1; i < nw - 1; i++) T.Draw(graphics, x + i * tw, y2, tw, th); //top draw
        RT.Draw(graphics, x2, y2, tw, th);
    }

    public float CeilW(float mw)
    {
        return FloatMath.Ceiling(mw / tw);
    }

    public float CeilH(float mh)
    {
        return FloatMath.Ceiling(mh / th);
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