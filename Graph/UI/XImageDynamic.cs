using XenonUI.Graph.Images;

namespace XenonUI.Graph.UI;

public class XImageDynamic : XElement
{

    public enum Style
    {

        UpShrink,
        DownShrink,
        LeftShrink,
        RightShrink,
        Vanish

    }

    public Style _style;
    public float Progress;

    public Image texp;

    public XImageDynamic(Image texp, Style style)
    {
        this.texp = texp;
        _style = style;
    }

    public override void Draw(Graphics graphics)
    {
        float p = Progress;
        switch(_style)
        {
            case Style.RightShrink:
                graphics.DrawImage(texp.GetTrueImageSurface(), Bound.x, Bound.y, Bound.w * p, Bound.h, texp.U, texp.V, texp.Width * p,
                    texp.Height);
                break;
            case Style.LeftShrink:
                graphics.DrawImage(texp.GetTrueImageSurface(), Bound.x + Bound.w * (1 - p), Bound.y, Bound.w * p, Bound.h,
                    texp.U + texp.Width * (1 - p), texp.V, texp.Width * p, texp.Height);
                break;
            case Style.UpShrink:
                graphics.DrawImage(texp.GetTrueImageSurface(), Bound.x, Bound.y, Bound.w, Bound.h * p, texp.U,
                    texp.V + texp.Height * (1 - p), texp.Width, texp.Height * p);
                break;
            case Style.DownShrink:
                graphics.DrawImage(texp.GetTrueImageSurface(), Bound.x, Bound.y + Bound.h * (1 - p), Bound.w, Bound.h * p, texp.U, texp.V,
                    texp.Width, texp.Height * p);
                break;
            case Style.Vanish:
                graphics.Color4(1, 1, 1, p);
                graphics.DrawImage(texp, Bound);
                graphics.NormalizeColor();
                break;
        }
    }

}