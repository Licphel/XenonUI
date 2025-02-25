using XenonUI.Graph.IMP;
using XenonUI.Maths;

namespace XenonUI.Graph;

public delegate void VertexAppender(Graphics graphics);
public delegate void UniformAppender(Graphics graphics);

public abstract class Graphics
{

    public static Graphics Global;
    
    public bool FlipX;
    public bool FlipY;
    public Font Font;
    public FontCarver FontCarver = new DefaultFontCarver();
    public Color[] _colors = new Color[4];
    public Stack<Color[]> _colorStack = new Stack<Color[]>();
    public Shader Program;
    public Matrix Projection = new Matrix();
    public MatrixStack Matrices = new MatrixStack();
    public Camera CameraNow;
    public Image Texfil;
    public Image TexMissing;
    public Transform Transform = new Transform();
    public UniformAppender UniformAppender;
    public VertexAppender[] VertAppenders;
    public Vector4 ViewportArray;
    
    public void DrawRect(float x, float y, float width, float height) => DrawImage(Texfil, x, y, width, height);
    public void DrawRect(Rect dst) => DrawRect(dst.x, dst.y, dst.w, dst.h);
    
    public abstract void DrawImage(Image img, float x, float y, float width, float height, float srcX, float srcY, float srcWidth, float srcHeight);
    public void DrawImage(Image img, float x, float y, float sx, float sy, float sw, float sh) => DrawImage(img, x, y, sw, sh, sx, sy, sw, sh);
    public void DrawImage(Image img, float x, float y, float w, float h) => DrawImage(img, x, y, w, h, 0, 0, img.Width, img.Height);
    public void DrawImage(Image img, float x, float y) => DrawImage(img, x, y, img.Width, img.Height, 0, 0, img.Width, img.Height);
    public void DrawImage(Image img, Rect dst, Rect src) => DrawImage(img, dst.x, dst.y, dst.w, dst.h, src.x, src.y, src.w, src.h);
    public void DrawImage(Image img, Rect dst) => DrawImage(img, dst.x, dst.y, dst.w, dst.h);
    public void DrawImage(Image img, Rect dst, float sx, float sy, float sw, float sh) => DrawImage(img, dst.x, dst.y, dst.w, dst.h, sx, sy, sw, sh);
    public void DrawImage(Image img, float x, float y, float w, float h, Rect src) => DrawImage(img, x, y, w, h, src.x, src.y, src.w, src.h);

    public virtual void Draw(string text, float x, float y, float maxWidth = int.MaxValue) => FontCarver.Draw(this, Font, text, x, y, maxWidth);

    public void Draw(Icon icon, Rect dst) => icon.Draw(this, dst.x, dst.y, dst.w, dst.h);
    public void Draw(Icon icon, float x, float y, float width, float height) => icon.Draw(this, x, y, width, height);

    public void Draw(string text, float x, float y, Align align, float maxWidth = int.MaxValue)
    {
        GlyphBounds bounds = Font.GetBounds(text);

        switch(align)
        {
            case Align.Left:
                Draw(text, x, y, maxWidth);
                break;
            case Align.Right:
                Draw(text, x - bounds.Width, y, maxWidth);
                break;
            case Align.Center:
                Draw(text, x - bounds.Width / 2f, y, maxWidth);
                break;
        }
    }

    public virtual void Draw(Lore text, float x, float y, float maxWidth = int.MaxValue) => FontCarver.Draw(this, Font, text, x, y, maxWidth);

    public void Draw(Lore text, float x, float y, Align align, float maxWidth = int.MaxValue)
    {
        if(text.Content.Count == 0) return;

        GlyphBounds bounds = Font.GetBounds(text);

        switch(align)
        {
            case Align.Left:
                Draw(text, x, y, maxWidth);
                break;
            case Align.Right:
                Draw(text, x - bounds.Width, y, maxWidth);
                break;
            case Align.Center:
                Draw(text, x - bounds.Width / 2.0F, y, maxWidth);
                break;
        }
    }
    
    public abstract void Clear();
    public abstract void Flush();
    
    public abstract bool SupportTransformation { get; }
    
    public virtual void Viewport(float x, float y, float w, float h) => throw new NotImplementedException();
    public void Viewport(Vector4 viewport) => Viewport(viewport.x, viewport.y, viewport.z, viewport.w);
    
    public virtual void Scissor(float x, float y, float w, float h) => throw new NotImplementedException();
    public void Scissor(Vector4 rect) => Scissor(rect.x, rect.y, rect.z, rect.w);
    public virtual void ScissorEnd() => throw new NotImplementedException();
    
    public virtual void UseCamera(Camera camera) => throw new NotImplementedException();
    public virtual void EndCamera(Camera camera) => throw new NotImplementedException();
    
    public abstract void UseShader(Shader program);
    public abstract void UseDefaultShader();
    
    public abstract void CheckTransformAndCap();

    public abstract void Write(Vector2 vec);
    public abstract void Write(Vector3 vec);
    public abstract void Write(Vector4 vec);
    public abstract void Write(params float[] arr);
    public abstract void Write(float f1);
    public abstract void Write(float f1, float f2);
    public abstract void Write(float f1, float f2, float f3);
    public abstract void WriteTransformed(Vector2 vec);
    public abstract void NewVertex(int v);
    public abstract void NewIndex(int v);
    
    public void Color4(float r, float g, float b, float a = 1)
    {
        _colors[0] = _colors[1] = _colors[2] = _colors[3] = new Color(r, g, b, a);
    }

    public void Color4(Color color)
    {
        _colors[0] = _colors[1] = _colors[2] = _colors[3] = color;
    }

    public void Merge4(float r, float g, float b, float a = 1)
    {
        _colors[0] *= new Color(r, g, b, a);
        _colors[1] *= new Color(r, g, b, a);
        _colors[2] *= new Color(r, g, b, a);
        _colors[3] *= new Color(r, g, b, a);
    }

    public void Merge4(Vector3 vector3, float a = 1)
    {
        _colors[0] *= new Color(vector3.x, vector3.y, vector3.z, a);
        _colors[1] *= new Color(vector3.x, vector3.y, vector3.z, a);
        _colors[2] *= new Color(vector3.x, vector3.y, vector3.z, a);
        _colors[3] *= new Color(vector3.x, vector3.y, vector3.z, a);
    }

    public void Merge4(Color color)
    {
        _colors[0] *= color;
        _colors[1] *= color;
        _colors[2] *= color;
        _colors[3] *= color;
    }

    public void NormalizeColor()
    {
        _colors[0] = _colors[1] = _colors[2] = _colors[3] = new Color(1, 1, 1, 1);
    }

    public void PushColor()
    {
        _colorStack.Push(new Color[] {_colors[0], _colors[1], _colors[2], _colors[3]});
    }

    public void PopColor()
    {
        _colors = _colorStack.Pop();
    }
    
}