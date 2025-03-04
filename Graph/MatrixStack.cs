using XenonUI.Maths;

namespace XenonUI.Graph;

public unsafe class MatrixStack
{

    public Transform Projection;
    public Transform[] ModelStack = new Transform[256];
    public int Len = 1;
    public ref Transform Top => ref ModelStack[Len - 1];
    public bool IsEmpty => Len == 1;

    public ref Transform GetCombinedTransform()
    {
        Transform c = Projection;
        c.Product(Top);
        return ref c;
    }

    public MatrixStack()
    {
        Clear();
    }

    public void Clear()
    {
        Len = 1;
        ModelStack[0].Identity();
    }

    public void Push()
    {
        Len++;
        _doCheck();
        ModelStack[Len - 1].Load(ModelStack[Len - 2]);
    }

    private void _doCheck()
    {
        if(IsEmpty)
            throw new Exception("Cannot modify the base matrix.");
    }

    public void Pop()
    {
        Len--;
    }

    public void Load(in Transform transform)
    {
        _doCheck();
        Top.Load(transform);
    }

    public void Rotate(Angle f)
    {
        _doCheck();
        Top.Rotate(f);
    }
    
    public void Rotate(Rotation r)
    {
        _doCheck();
        Top.Translate(r.cx, r.cy);
        Top.Rotate(r.Angle);
        Top.Translate(-r.cx, -r.cy);
    }
    
    public void Translate(float x, float y)
    {
        _doCheck();
        Top.Translate(x, y);
    }

    public void Scale(float x, float y)
    {
        _doCheck();
        Top.Scale(x, y);
    }
    
    public void Shear(float x, float y)
    {
        _doCheck();
        Top.Shear(x, y);
    }

}