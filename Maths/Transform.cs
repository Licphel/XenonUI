namespace XenonUI.Maths;

public struct Transform
{

    public float m00 = 1.0f;
    public float m01;
    public float m02;
    public float m10;
    public float m11 = 1.0f;
    public float m12;

    public Transform()
    {
        Identity();
    }

    public Transform Identity()
    {
        m00 = 1.0f;
        m01 = 0.0f;
        m02 = 0.0f;
        m10 = 0.0f;
        m11 = 1.0f;
        m12 = 0.0f;
        return this;
    }

    public Transform Load(in Transform other)
    {
        m00 = other.m00;
        m01 = other.m01;
        m02 = other.m02;
        m10 = other.m10;
        m11 = other.m11;
        m12 = other.m12;
        return this;
    }

    public Transform Orthographic(float left, float right, float bottom, float top)
    {
        float xOrtho = 2.0f / (right - left);
        float yOrtho = 2.0f / (top - bottom);
        m00 = xOrtho;
        m10 = 0.0f;
        m01 = 0.0f;
        m11 = yOrtho;
        m02 = 0.0f;
        m12 = 0.0f;
        return this;
    }

    public Transform Product(in Transform other)
    {
        float tmp00 = m00 * other.m00 + m01 * other.m10;
        float tmp01 = m00 * other.m01 + m01 * other.m11;
        float tmp02 = m00 * other.m02 + m01 * other.m12 + m02;
        float tmp10 = m10 * other.m00 + m11 * other.m10;
        float tmp11 = m10 * other.m01 + m11 * other.m11;
        float tmp12 = m10 * other.m02 + m11 * other.m12 + m12;
        m00 = tmp00;
        m01 = tmp01;
        m02 = tmp02;
        m10 = tmp10;
        m11 = tmp11;
        m12 = tmp12;
        return this;
    }

    public float Determinant()
    {
        return m00 * m11 - m01 * m10;
    }

    public Transform Invert()
    {
        float det = Determinant();
        float invDet = 1.0f / det;
        float tmp00 = m11;
        float tmp01 = -m01;
        float tmp02 = m01 * m12 - m11 * m02;
        float tmp10 = -m10;
        float tmp11 = m00;
        float tmp12 = m10 * m02 - m00 * m12;
        m00 = invDet * tmp00;
        m01 = invDet * tmp01;
        m02 = invDet * tmp02;
        m10 = invDet * tmp10;
        m11 = invDet * tmp11;
        m12 = invDet * tmp12;
        return this;
    }
    
    public void ApplyTo(float x, float y, out float xo, out float yo)
    {
        xo = m00 * x + m01 * y + m02;
        yo = m10 * x + m11 * y + m12;
    }

    public void ApplyTo(VaryingVector2 vec)
    {
        float x = vec.x;
        float y = vec.y;
        vec.x = m00 * x + m01 * y + m02;
        vec.y = m10 * x + m11 * y + m12;
    }

    public void ApplyTo(ref Vector2 vec)
    {
        float x = vec.x;
        float y = vec.y;
        vec.x = m00 * x + m01 * y + m02;
        vec.y = m10 * x + m11 * y + m12;
    }

    public Transform Translate(float x, float y)
    {
        m02 += m00 * x + m01 * y;
        m12 += m10 * x + m11 * y;
        return this;
    }
    
    public Transform Shear(float x, float y)
    {
        float tmp00 = m00 + y * m01;
        float tmp01 = m01 + x * m00;
        float tmp10 = m10 + y * m11;
        float tmp11 = m11 + x * m10;
        m00 = tmp00;
        m01 = tmp01;
        m10 = tmp10;
        m11 = tmp11;
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

    public Transform Rotate(Angle a)
    {
        float cos = Mathf.CosRad(a.Value);
        float sin = Mathf.SinRad(a.Value);

        float tmp00 = m00 * cos + m01 * sin;
        float tmp01 = m00 * -sin + m01 * cos;
        float tmp10 = m10 * cos + m11 * sin;
        float tmp11 = m10 * -sin + m11 * cos;

        m00 = tmp00;
        m01 = tmp01;
        m10 = tmp10;
        m11 = tmp11;
        return this;
    }

}