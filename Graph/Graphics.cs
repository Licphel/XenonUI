using System.Runtime.CompilerServices;
using KryptonM;
using XenonUI.Graph.Images;
using XenonUI.Graph.Text;
using XenonUI.Maths;

namespace XenonUI.Graph;

public delegate void VertexAppender(Graphics graphics);

public delegate void UniformAppender(Graphics graphics);

public abstract unsafe class Graphics
{

    public static Graphics Global;

    public Color[] VertexColors = new Color[4];
    public Stack<Color[]> _colorStack = new Stack<Color[]>();

    private float InvTexHeight;
    private float InvTexWidth;

    public int MinVertexBufSize = 256;
    
    public Camera CameraNow;
    public Shader Program;
    public Image _ImageBinding;
    public Image ImagePureColor = ImageBuffer.Img0x0;
    public Image ImageMissing = ImageBuffer.Img0x0;
    public SDBuffer Buffer = new SDBuffer();

    public bool FlipX;
    public bool FlipY;

    public Font Font;
    public FontCarver FontCarver = new DefaultFontCarver();

    public MatrixStack MatrixStack = new MatrixStack();

    public UniformAppender UniformAppender;
    public VertexAppender[] VertAppenders;

    public Vector4 ViewportArray;

    public Graphics(int size)
    {
        NormalizeColor();
        //1 sprite - 4 vert - 6 ind
        Buffer.SetVertexCapacity(size);

        Vector2 vks = GraphicsDevice.Current.Size;
        ViewportArray = new Vector4(0, 0, vks.x, vks.y);
    }

    public void DrawRect(float x, float y, float width, float height)
    {
        DrawImage(ImagePureColor, x, y, width, height);
    }

    public void DrawRect(Rect dst)
    {
        DrawRect(dst.x, dst.y, dst.w, dst.h);
    }

    public void DrawImage(Image image, float x, float y, float width, float height, float srcX, float srcY,
        float srcWidth, float srcHeight)
    {
        if(image == null)
            image = ImageMissing;
        else if(!image.IsSurface)
        {
            DrawImage(image.GetTrueImageSurface(), x, y, width, height, srcX + image.U, srcY + image.V, srcWidth,
                srcHeight);
            return;
        }

        CheckCapacity();

        if(image != _ImageBinding)
        {
            InvTexWidth = 1f / image.Width;
            InvTexHeight = 1f / image.Height;
            Flush();
        }

        _ImageBinding = image;

        ref Transform topMatrix = ref MatrixStack.GetCombinedTransform();
        topMatrix.ApplyTo(x, y, out float x1, out float y1);
        topMatrix.ApplyTo(x, y + height, out float x2, out float y2);
        topMatrix.ApplyTo(x + width, y + height, out float x3, out float y3);
        topMatrix.ApplyTo(x + width, y, out float x4, out float y4);

        float u = srcX * InvTexWidth;
        float v = srcY * InvTexHeight;
        float u2 = (srcX + srcWidth) * InvTexWidth;
        float v2 = (srcY + srcHeight) * InvTexHeight;

        if(FlipX) (u, u2) = (u2, u);
        if(FlipY) (v, v2) = (v2, v);
        
        Buffer.Append(x3).Append(y3).Append(VertexColors[2]).Append(u2).Append(v);
        VertAppenders?[2]?.Invoke(this);
        Buffer.Append(x4).Append(y4).Append(VertexColors[3]).Append(u2).Append(v2);
        VertAppenders?[3]?.Invoke(this);
        Buffer.Append(x1).Append(y1).Append(VertexColors[0]).Append(u).Append(v2);
        VertAppenders?[0]?.Invoke(this);
        Buffer.Append(x2).Append(y2).Append(VertexColors[1]).Append(u).Append(v);
        VertAppenders?[1]?.Invoke(this);
        Buffer.NewIndex(6);
    }

    public void DrawImage(Image img, float x, float y, float sx, float sy, float sw, float sh)
    {
        DrawImage(img, x, y, sw, sh, sx, sy, sw, sh);
    }

    public void DrawImage(Image img, float x, float y, float w, float h)
    {
        DrawImage(img, x, y, w, h, 0, 0, img.Width, img.Height);
    }

    public void DrawImage(Image img, float x, float y)
    {
        DrawImage(img, x, y, img.Width, img.Height, 0, 0, img.Width, img.Height);
    }

    public void DrawImage(Image img, Rect dst, Rect src)
    {
        DrawImage(img, dst.x, dst.y, dst.w, dst.h, src.x, src.y, src.w, src.h);
    }

    public void DrawImage(Image img, Rect dst)
    {
        DrawImage(img, dst.x, dst.y, dst.w, dst.h);
    }

    public void DrawImage(Image img, Rect dst, float sx, float sy, float sw, float sh)
    {
        DrawImage(img, dst.x, dst.y, dst.w, dst.h, sx, sy, sw, sh);
    }

    public void DrawImage(Image img, float x, float y, float w, float h, Rect src)
    {
        DrawImage(img, x, y, w, h, src.x, src.y, src.w, src.h);
    }

    public void DrawIcon(Icon icon, Rect dst)
    {
        icon?.InternalPerform(this, dst.x, dst.y, dst.w, dst.h);
    }

    public void DrawIcon(Icon icon, float x, float y, float width, float height)
    {
        icon?.InternalPerform(this, x, y, width, height);
    }

    public virtual void DrawText(string text, float x, float y, float maxWidth = int.MaxValue)
    {
        FontCarver.Draw(this, Font, text, x, y, maxWidth);
    }

    public void DrawText(string text, float x, float y, FontAlign align, float maxWidth = int.MaxValue)
    {
        GlyphBounds bounds = Font.GetBounds(text);

        switch(align)
        {
            case FontAlign.Left:
                DrawText(text, x, y, maxWidth);
                break;
            case FontAlign.Right:
                DrawText(text, x - bounds.Width, y, maxWidth);
                break;
            case FontAlign.Center:
                DrawText(text, x - bounds.Width / 2f, y, maxWidth);
                break;
        }
    }

    public virtual void DrawLore(Lore text, float x, float y, float maxWidth = int.MaxValue)
    {
        FontCarver.Draw(this, Font, text, x, y, maxWidth);
    }

    public void DrawLore(Lore text, float x, float y, FontAlign align, float maxWidth = int.MaxValue)
    {
        if(text.Content.Count == 0) return;

        GlyphBounds bounds = Font.GetBounds(text);

        switch(align)
        {
            case FontAlign.Left:
                DrawLore(text, x, y, maxWidth);
                break;
            case FontAlign.Right:
                DrawLore(text, x - bounds.Width, y, maxWidth);
                break;
            case FontAlign.Center:
                DrawLore(text, x - bounds.Width / 2.0F, y, maxWidth);
                break;
        }
    }

    public abstract void Clear();
    public abstract void Flush();

    public virtual void Viewport(float x, float y, float w, float h) => throw new NotImplementedException();

    public void Viewport(Vector4 viewport)
    {
        Viewport(viewport.x, viewport.y, viewport.z, viewport.w);
    }

    public virtual void Scissor(float x, float y, float w, float h) => throw new NotImplementedException();

    public void Scissor(Vector4 rect)
    {
        Scissor(rect.x, rect.y, rect.z, rect.w);
    }

    public virtual void ScissorEnd() => throw new NotImplementedException();

    public virtual void UseCamera(Camera camera)
    {
        MatrixStack.Projection = camera.CombinedTransform;
        CameraNow = camera;
    }

    public abstract void UseShader(Shader program);
    public abstract void UseDefaultShader();

    public void CheckCapacity()
    {
        if(Buffer.VertexCap - Buffer.Size < MinVertexBufSize)
            Flush();
    }

    public void Color4(float r, float g, float b, float a = 1) =>
        VertexColors[0] = VertexColors[1] = VertexColors[2] = VertexColors[3] = new Color(r, g, b, a);

    public void Color4(Color color) => VertexColors[0] = VertexColors[1] = VertexColors[2] = VertexColors[3] = color;

    public void Merge4(float r, float g, float b, float a = 1)
    {
        VertexColors[0] *= new Color(r, g, b, a);
        VertexColors[1] *= new Color(r, g, b, a);
        VertexColors[2] *= new Color(r, g, b, a);
        VertexColors[3] *= new Color(r, g, b, a);
    }

    public void Merge4(Vector3 vector3, float a = 1)
    {
        VertexColors[0] *= new Color(vector3.x, vector3.y, vector3.z, a);
        VertexColors[1] *= new Color(vector3.x, vector3.y, vector3.z, a);
        VertexColors[2] *= new Color(vector3.x, vector3.y, vector3.z, a);
        VertexColors[3] *= new Color(vector3.x, vector3.y, vector3.z, a);
    }

    public void Merge4(Color color)
    {
        VertexColors[0] *= color;
        VertexColors[1] *= color;
        VertexColors[2] *= color;
        VertexColors[3] *= color;
    }

    public void NormalizeColor() => VertexColors[0] = VertexColors[1] = VertexColors[2] = VertexColors[3] = new Color(1, 1, 1);
    public void PushColor() => _colorStack.Push(new[] { VertexColors[0], VertexColors[1], VertexColors[2], VertexColors[3] });
    public void PopColor() => VertexColors = _colorStack.Pop();

}