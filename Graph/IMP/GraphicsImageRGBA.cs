using XenonUI.Maths;

namespace XenonUI.Graph.IMP;

public unsafe class GraphicsImageRGBA : Graphics
{

    private readonly byte[] b;
    private readonly int w;
    private readonly int h;

    public GraphicsImageRGBA(byte[] b, int w, int h)
    {
        this.b = b;
        this.w = w;
        this.h = h;
    }

    public override bool SupportTransformation => false;

    public override void DrawImage(Image img, float x, float y, float width, float height, float srcX, float srcY,
        float srcWidth,
        float srcHeight)
    {
        if(Math.Abs(width - srcWidth) > 0.001f || Math.Abs(height - srcHeight) > 0.001f)
            throw new Exception("Cannot do scaling!");

        fixed(byte* src0 = img.Data)
        {
            fixed(byte* dst_ = b)
            {
                var dst0 = dst_ + (ulong)(y * w * 4 + x * 4);

                for(var x1 = 0; x1 < width; x1++)
                for(var y1 = 0; y1 < height; y1++)
                {
                    var src1 = src0 + (ulong)((srcX + x1) * srcWidth * 4 + (srcY + y1) * 4);
                    var dst1 = dst0 + (ulong)(y1 * width * 4 + x1 * 4);

                    dst1[0] = src1[0];
                    dst1[1] = src1[1];
                    dst1[2] = src1[2];
                    dst1[3] = src1[3];
                }
            }
        }
    }

    public override void Clear()
    {
        for(var i = 0; i < w * h * 4; i++) b[i] = 0;
    }

    public override void Flush()
    {
        //Nothing to do since the changes are instantly applied.
    }

    public override void UseShader(Shader program)
    {
        throw new NotImplementedException();
    }

    public override void UseDefaultShader()
    {
        throw new NotImplementedException();
    }

    public override void CheckTransformAndCap()
    {
        throw new NotImplementedException();
    }

    public override void Write(Vector2 vec)
    {
        throw new NotImplementedException();
    }

    public override void Write(Vector3 vec)
    {
        throw new NotImplementedException();
    }

    public override void Write(Vector4 vec)
    {
        throw new NotImplementedException();
    }

    public override void Write(params float[] arr)
    {
        throw new NotImplementedException();
    }

    public override void Write(float f1)
    {
        throw new NotImplementedException();
    }

    public override void Write(float f1, float f2)
    {
        throw new NotImplementedException();
    }

    public override void Write(float f1, float f2, float f3)
    {
        throw new NotImplementedException();
    }

    public override void WriteTransformed(Vector2 vec)
    {
        throw new NotImplementedException();
    }

    public override void NewVertex(int v)
    {
        throw new NotImplementedException();
    }

    public override void NewIndex(int v)
    {
        throw new NotImplementedException();
    }

}