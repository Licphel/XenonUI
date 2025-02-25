using KryptonM.Maths;

namespace XenonUI.Maths;

public class Transform
{

    public float m00 = 1.0f;
    public float m01;
    public float m02;
    public float m10;
    public float m11 = 1.0f;
    public float m12;

    public void Identity()
    {
        m00 = 1.0f;
        m01 = 0.0f;
        m02 = 0.0f;
        m10 = 0.0f;
        m11 = 1.0f;
        m12 = 0.0f;
    }

    public void Set(Transform other)
    {
        m00 = other.m00;
        m01 = other.m01;
        m02 = other.m02;
        m10 = other.m10;
        m11 = other.m11;
        m12 = other.m12;
    }

    public Transform Set(Matrix matrix)
    {
        m00 = matrix.m00;
        m01 = matrix.m01;
        m02 = matrix.m03;
        m10 = matrix.m10;
        m11 = matrix.m11;
        m12 = matrix.m13;
        return this;
    }

    public void Set(float x, float y, float deg, float scaleX, float scaleY)
    {
        m02 = x;
        m12 = y;
        if(deg == 0.0f)
        {
            m00 = scaleX;
            m01 = 0.0f;
            m10 = 0.0f;
            m11 = scaleY;
        }
        else
        {
            var sin = FloatMath.SinRad(deg);
            var cos = FloatMath.CosRad(deg);
            m00 = cos * scaleX;
            m01 = -sin * scaleY;
            m10 = sin * scaleX;
            m11 = cos * scaleY;
        }
    }

    public void ToOrtho(float left, float right, float bottom, float top)
    {
        var xOrtho = 2.0f / (right - left);
        var yOrtho = 2.0f / (top - bottom);
        m00 = xOrtho;
        m10 = 0.0f;
        m01 = 0.0f;
        m11 = yOrtho;
        m02 = 0.0f;
        m12 = 0.0f;
    }

    public void Mul(Transform other)
    {
        var tmp00 = m00 * other.m00 + m01 * other.m10;
        var tmp01 = m00 * other.m01 + m01 * other.m11;
        var tmp02 = m00 * other.m02 + m01 * other.m12 + m02;
        var tmp10 = m10 * other.m00 + m11 * other.m10;
        var tmp11 = m10 * other.m01 + m11 * other.m11;
        var tmp12 = m10 * other.m02 + m11 * other.m12 + m12;
        m00 = tmp00;
        m01 = tmp01;
        m02 = tmp02;
        m10 = tmp10;
        m11 = tmp11;
        m12 = tmp12;
    }

    public float Determinant()
    {
        return m00 * m11 - m01 * m10;
    }

    public void Invert()
    {
        var det = Determinant();
        var invDet = 1.0f / det;
        var tmp00 = m11;
        var tmp01 = -m01;
        var tmp02 = m01 * m12 - m11 * m02;
        var tmp10 = -m10;
        var tmp11 = m00;
        var tmp12 = m10 * m02 - m00 * m12;
        m00 = invDet * tmp00;
        m01 = invDet * tmp01;
        m02 = invDet * tmp02;
        m10 = invDet * tmp10;
        m11 = invDet * tmp11;
        m12 = invDet * tmp12;
    }

    public VaryingVector2 ApplyTo(VaryingVector2 vec)
    {
        var x = vec.x;
        var y = vec.y;
        vec.x = m00 * x + m01 * y + m02;
        vec.y = m10 * x + m11 * y + m12;
        return vec;
    }

    public Vector2 ApplyTo(ref Vector2 vec)
    {
        var x = vec.x;
        var y = vec.y;
        vec.x = m00 * x + m01 * y + m02;
        vec.y = m10 * x + m11 * y + m12;
        return vec;
    }

    public Transform Translate(float x, float y)
    {
        m02 += m00 * x + m01 * y;
        m12 += m10 * x + m11 * y;
        return this;
    }

    public Transform PreTranslate(float x, float y)
    {
        m02 += x;
        m12 += y;
        return this;
    }

    public Transform Scale(float scaleX, float scaleY)
    {
        m00 *= scaleX;
        m01 *= scaleY;
        m10 *= scaleX;
        m11 *= scaleY;
        return this;
    }

    public Transform PreScale(float scaleX, float scaleY)
    {
        m00 *= scaleX;
        m01 *= scaleX;
        m02 *= scaleX;
        m10 *= scaleY;
        m11 *= scaleY;
        m12 *= scaleY;
        return this;
    }

    public Transform Rotate(float radians)
    {
        if(radians == 0) return this;

        var cos = FloatMath.CosRad(radians);
        var sin = FloatMath.SinRad(radians);

        var tmp00 = m00 * cos + m01 * sin;
        var tmp01 = m00 * -sin + m01 * cos;
        var tmp10 = m10 * cos + m11 * sin;
        var tmp11 = m10 * -sin + m11 * cos;

        m00 = tmp00;
        m01 = tmp01;
        m10 = tmp10;
        m11 = tmp11;
        return this;
    }

}