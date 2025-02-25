using KryptonM.Maths;

namespace XenonUI.Maths;

public class Matrix
{

    public float m00;
    public float m01;
    public float m02;
    public float m03;
    public float m10;
    public float m11;
    public float m12;
    public float m13;
    public float m20;
    public float m21;
    public float m22;
    public float m23;
    public float m30;
    public float m31;
    public float m32;
    public float m33;

    public Matrix()
    {
        Identity();
    }

    public Matrix(Matrix src)
    {
        Load(src);
    }

    public Matrix Identity()
    {
        return Identity(this);
    }

    public static Matrix Identity(Matrix m)
    {
        m.m00 = 1.0F;
        m.m01 = 0.0F;
        m.m02 = 0.0F;
        m.m03 = 0.0F;
        m.m10 = 0.0F;
        m.m11 = 1.0F;
        m.m12 = 0.0F;
        m.m13 = 0.0F;
        m.m20 = 0.0F;
        m.m21 = 0.0F;
        m.m22 = 1.0F;
        m.m23 = 0.0F;
        m.m30 = 0.0F;
        m.m31 = 0.0F;
        m.m32 = 0.0F;
        m.m33 = 1.0F;
        return m;
    }

    public Matrix Zero()
    {
        return Zero(this);
    }

    public static Matrix Zero(Matrix m)
    {
        m.m00 = 0.0F;
        m.m01 = 0.0F;
        m.m02 = 0.0F;
        m.m03 = 0.0F;
        m.m10 = 0.0F;
        m.m11 = 0.0F;
        m.m12 = 0.0F;
        m.m13 = 0.0F;
        m.m20 = 0.0F;
        m.m21 = 0.0F;
        m.m22 = 0.0F;
        m.m23 = 0.0F;
        m.m30 = 0.0F;
        m.m31 = 0.0F;
        m.m32 = 0.0F;
        m.m33 = 0.0F;
        return m;
    }

    public Matrix Load(Matrix src)
    {
        return Load(src, this);
    }

    public static Matrix Load(Matrix src, Matrix dest)
    {
        if(dest == null) dest = new Matrix();

        dest.m00 = src.m00;
        dest.m01 = src.m01;
        dest.m02 = src.m02;
        dest.m03 = src.m03;
        dest.m10 = src.m10;
        dest.m11 = src.m11;
        dest.m12 = src.m12;
        dest.m13 = src.m13;
        dest.m20 = src.m20;
        dest.m21 = src.m21;
        dest.m22 = src.m22;
        dest.m23 = src.m23;
        dest.m30 = src.m30;
        dest.m31 = src.m31;
        dest.m32 = src.m32;
        dest.m33 = src.m33;
        return dest;
    }

    public static Matrix Add(Matrix left, Matrix right, Matrix dest)
    {
        if(dest == null) dest = new Matrix();

        dest.m00 = left.m00 + right.m00;
        dest.m01 = left.m01 + right.m01;
        dest.m02 = left.m02 + right.m02;
        dest.m03 = left.m03 + right.m03;
        dest.m10 = left.m10 + right.m10;
        dest.m11 = left.m11 + right.m11;
        dest.m12 = left.m12 + right.m12;
        dest.m13 = left.m13 + right.m13;
        dest.m20 = left.m20 + right.m20;
        dest.m21 = left.m21 + right.m21;
        dest.m22 = left.m22 + right.m22;
        dest.m23 = left.m23 + right.m23;
        dest.m30 = left.m30 + right.m30;
        dest.m31 = left.m31 + right.m31;
        dest.m32 = left.m32 + right.m32;
        dest.m33 = left.m33 + right.m33;
        return dest;
    }

    public static Matrix Sub(Matrix left, Matrix right, Matrix dest)
    {
        if(dest == null) dest = new Matrix();

        dest.m00 = left.m00 - right.m00;
        dest.m01 = left.m01 - right.m01;
        dest.m02 = left.m02 - right.m02;
        dest.m03 = left.m03 - right.m03;
        dest.m10 = left.m10 - right.m10;
        dest.m11 = left.m11 - right.m11;
        dest.m12 = left.m12 - right.m12;
        dest.m13 = left.m13 - right.m13;
        dest.m20 = left.m20 - right.m20;
        dest.m21 = left.m21 - right.m21;
        dest.m22 = left.m22 - right.m22;
        dest.m23 = left.m23 - right.m23;
        dest.m30 = left.m30 - right.m30;
        dest.m31 = left.m31 - right.m31;
        dest.m32 = left.m32 - right.m32;
        dest.m33 = left.m33 - right.m33;
        return dest;
    }

    public static Matrix Mul(Matrix left, Matrix right, Matrix dest)
    {
        if(dest == null) dest = new Matrix();

        var m00 = left.m00 * right.m00 + left.m10 * right.m01 + left.m20 * right.m02 + left.m30 * right.m03;
        var m01 = left.m01 * right.m00 + left.m11 * right.m01 + left.m21 * right.m02 + left.m31 * right.m03;
        var m02 = left.m02 * right.m00 + left.m12 * right.m01 + left.m22 * right.m02 + left.m32 * right.m03;
        var m03 = left.m03 * right.m00 + left.m13 * right.m01 + left.m23 * right.m02 + left.m33 * right.m03;
        var m10 = left.m00 * right.m10 + left.m10 * right.m11 + left.m20 * right.m12 + left.m30 * right.m13;
        var m11 = left.m01 * right.m10 + left.m11 * right.m11 + left.m21 * right.m12 + left.m31 * right.m13;
        var m12 = left.m02 * right.m10 + left.m12 * right.m11 + left.m22 * right.m12 + left.m32 * right.m13;
        var m13 = left.m03 * right.m10 + left.m13 * right.m11 + left.m23 * right.m12 + left.m33 * right.m13;
        var m20 = left.m00 * right.m20 + left.m10 * right.m21 + left.m20 * right.m22 + left.m30 * right.m23;
        var m21 = left.m01 * right.m20 + left.m11 * right.m21 + left.m21 * right.m22 + left.m31 * right.m23;
        var m22 = left.m02 * right.m20 + left.m12 * right.m21 + left.m22 * right.m22 + left.m32 * right.m23;
        var m23 = left.m03 * right.m20 + left.m13 * right.m21 + left.m23 * right.m22 + left.m33 * right.m23;
        var m30 = left.m00 * right.m30 + left.m10 * right.m31 + left.m20 * right.m32 + left.m30 * right.m33;
        var m31 = left.m01 * right.m30 + left.m11 * right.m31 + left.m21 * right.m32 + left.m31 * right.m33;
        var m32 = left.m02 * right.m30 + left.m12 * right.m31 + left.m22 * right.m32 + left.m32 * right.m33;
        var m33 = left.m03 * right.m30 + left.m13 * right.m31 + left.m23 * right.m32 + left.m33 * right.m33;
        dest.m00 = m00;
        dest.m01 = m01;
        dest.m02 = m02;
        dest.m03 = m03;
        dest.m10 = m10;
        dest.m11 = m11;
        dest.m12 = m12;
        dest.m13 = m13;
        dest.m20 = m20;
        dest.m21 = m21;
        dest.m22 = m22;
        dest.m23 = m23;
        dest.m30 = m30;
        dest.m31 = m31;
        dest.m32 = m32;
        dest.m33 = m33;
        return dest;
    }

    public static Vector4 Transform(Matrix left, Vector4 right, ref Vector4 dest)
    {
        var x = left.m00 * right.x + left.m10 * right.y + left.m20 * right.z + left.m30 * right.w;
        var y = left.m01 * right.x + left.m11 * right.y + left.m21 * right.z + left.m31 * right.w;
        var z = left.m02 * right.x + left.m12 * right.y + left.m22 * right.z + left.m32 * right.w;
        var w = left.m03 * right.x + left.m13 * right.y + left.m23 * right.z + left.m33 * right.w;
        dest.x = x;
        dest.y = y;
        dest.z = z;
        dest.w = w;
        return dest;
    }

    public Matrix Transpose()
    {
        return Transpose(this);
    }

    public Matrix Translate(Vector2 vec)
    {
        return Translate(vec, this);
    }

    public Matrix Translate(Vector3 vec)
    {
        return Translate(vec, this);
    }

    public Matrix Scale(Vector3 vec)
    {
        return Scale(vec, this, this);
    }

    public static Matrix Scale(Vector3 vec, Matrix src, Matrix dest)
    {
        if(dest == null) dest = new Matrix();

        dest.m00 = src.m00 * vec.x;
        dest.m01 = src.m01 * vec.x;
        dest.m02 = src.m02 * vec.x;
        dest.m03 = src.m03 * vec.x;
        dest.m10 = src.m10 * vec.y;
        dest.m11 = src.m11 * vec.y;
        dest.m12 = src.m12 * vec.y;
        dest.m13 = src.m13 * vec.y;
        dest.m20 = src.m20 * vec.z;
        dest.m21 = src.m21 * vec.z;
        dest.m22 = src.m22 * vec.z;
        dest.m23 = src.m23 * vec.z;
        return dest;
    }

    public Matrix Rotate(float angle, Vector3 axis)
    {
        return Rotate(angle, axis, this);
    }

    public Matrix Rotate(float angle, Vector3 axis, Matrix dest)
    {
        return Rotate(angle, axis, this, dest);
    }

    public static Matrix Rotate(float angle, Vector3 axis, Matrix src, Matrix dest)
    {
        if(dest == null) dest = new Matrix();

        var c = FloatMath.CosRad(angle);
        var s = FloatMath.SinRad(angle);
        var o = 1.0F - c;
        var xy = axis.x * axis.y;
        var yz = axis.y * axis.z;
        var xz = axis.x * axis.z;
        var xs = axis.x * s;
        var ys = axis.y * s;
        var zs = axis.z * s;
        var f00 = axis.x * axis.x * o + c;
        var f01 = xy * o + zs;
        var f02 = xz * o - ys;
        var f10 = xy * o - zs;
        var f11 = axis.y * axis.y * o + c;
        var f12 = yz * o + xs;
        var f20 = xz * o + ys;
        var f21 = yz * o - xs;
        var f22 = axis.z * axis.z * o + c;
        var t00 = src.m00 * f00 + src.m10 * f01 + src.m20 * f02;
        var t01 = src.m01 * f00 + src.m11 * f01 + src.m21 * f02;
        var t02 = src.m02 * f00 + src.m12 * f01 + src.m22 * f02;
        var t03 = src.m03 * f00 + src.m13 * f01 + src.m23 * f02;
        var t10 = src.m00 * f10 + src.m10 * f11 + src.m20 * f12;
        var t11 = src.m01 * f10 + src.m11 * f11 + src.m21 * f12;
        var t12 = src.m02 * f10 + src.m12 * f11 + src.m22 * f12;
        var t13 = src.m03 * f10 + src.m13 * f11 + src.m23 * f12;
        dest.m20 = src.m00 * f20 + src.m10 * f21 + src.m20 * f22;
        dest.m21 = src.m01 * f20 + src.m11 * f21 + src.m21 * f22;
        dest.m22 = src.m02 * f20 + src.m12 * f21 + src.m22 * f22;
        dest.m23 = src.m03 * f20 + src.m13 * f21 + src.m23 * f22;
        dest.m00 = t00;
        dest.m01 = t01;
        dest.m02 = t02;
        dest.m03 = t03;
        dest.m10 = t10;
        dest.m11 = t11;
        dest.m12 = t12;
        dest.m13 = t13;
        return dest;
    }

    public Matrix Translate(Vector3 vec, Matrix dest)
    {
        return Translate(vec, this, dest);
    }

    public static Matrix Translate(Vector3 vec, Matrix src, Matrix dest)
    {
        if(dest == null) dest = new Matrix();

        dest.m30 += src.m00 * vec.x + src.m10 * vec.y + src.m20 * vec.z;
        dest.m31 += src.m01 * vec.x + src.m11 * vec.y + src.m21 * vec.z;
        dest.m32 += src.m02 * vec.x + src.m12 * vec.y + src.m22 * vec.z;
        dest.m33 += src.m03 * vec.x + src.m13 * vec.y + src.m23 * vec.z;
        return dest;
    }

    public Matrix Translate(Vector2 vec, Matrix dest)
    {
        return Translate(vec, this, dest);
    }

    public static Matrix Translate(Vector2 vec, Matrix src, Matrix dest)
    {
        if(dest == null) dest = new Matrix();

        dest.m30 += src.m00 * vec.x + src.m10 * vec.y;
        dest.m31 += src.m01 * vec.x + src.m11 * vec.y;
        dest.m32 += src.m02 * vec.x + src.m12 * vec.y;
        dest.m33 += src.m03 * vec.x + src.m13 * vec.y;
        return dest;
    }

    public Matrix Transpose(Matrix dest)
    {
        return Transpose(this, dest);
    }

    public static Matrix Transpose(Matrix src, Matrix dest)
    {
        if(dest == null) dest = new Matrix();

        var m00 = src.m00;
        var m01 = src.m10;
        var m02 = src.m20;
        var m03 = src.m30;
        var m10 = src.m01;
        var m11 = src.m11;
        var m12 = src.m21;
        var m13 = src.m31;
        var m20 = src.m02;
        var m21 = src.m12;
        var m22 = src.m22;
        var m23 = src.m32;
        var m30 = src.m03;
        var m31 = src.m13;
        var m32 = src.m23;
        var m33 = src.m33;
        dest.m00 = m00;
        dest.m01 = m01;
        dest.m02 = m02;
        dest.m03 = m03;
        dest.m10 = m10;
        dest.m11 = m11;
        dest.m12 = m12;
        dest.m13 = m13;
        dest.m20 = m20;
        dest.m21 = m21;
        dest.m22 = m22;
        dest.m23 = m23;
        dest.m30 = m30;
        dest.m31 = m31;
        dest.m32 = m32;
        dest.m33 = m33;
        return dest;
    }

    public float Determinant()
    {
        var f = m00 * (m11 * m22 * m33 + m12 * m23 * m31 + m13 * m21 * m32 - m13 * m22 * m31 - m11 * m23 * m32 -
                       m12 * m21 * m33);
        f -= m01 * (m10 * m22 * m33 + m12 * m23 * m30 + m13 * m20 * m32 - m13 * m22 * m30 - m10 * m23 * m32 -
                    m12 * m20 * m33);
        f += m02 * (m10 * m21 * m33 + m11 * m23 * m30 + m13 * m20 * m31 - m13 * m21 * m30 - m10 * m23 * m31 -
                    m11 * m20 * m33);
        f -= m03 * (m10 * m21 * m32 + m11 * m22 * m30 + m12 * m20 * m31 - m12 * m21 * m30 - m10 * m22 * m31 -
                    m11 * m20 * m32);
        return f;
    }

    private static float Determinant3x3(float t00, float t01, float t02, float t10, float t11, float t12, float t20,
        float t21, float t22)
    {
        return t00 * (t11 * t22 - t12 * t21) + t01 * (t12 * t20 - t10 * t22) + t02 * (t10 * t21 - t11 * t20);
    }

    public Matrix Invert()
    {
        return Invert(this, this);
    }

    public static Matrix Invert(Matrix src, Matrix dest)
    {
        var determinant = src.Determinant();
        if(determinant != 0.0F)
        {
            if(dest == null) dest = new Matrix();

            var determinant_inv = 1.0F / determinant;
            var t00 = Determinant3x3(src.m11, src.m12, src.m13, src.m21, src.m22, src.m23, src.m31, src.m32,
                src.m33);
            var t01 = -Determinant3x3(src.m10, src.m12, src.m13, src.m20, src.m22, src.m23, src.m30, src.m32,
                src.m33);
            var t02 = Determinant3x3(src.m10, src.m11, src.m13, src.m20, src.m21, src.m23, src.m30, src.m31,
                src.m33);
            var t03 = -Determinant3x3(src.m10, src.m11, src.m12, src.m20, src.m21, src.m22, src.m30, src.m31,
                src.m32);
            var t10 = -Determinant3x3(src.m01, src.m02, src.m03, src.m21, src.m22, src.m23, src.m31, src.m32,
                src.m33);
            var t11 = Determinant3x3(src.m00, src.m02, src.m03, src.m20, src.m22, src.m23, src.m30, src.m32,
                src.m33);
            var t12 = -Determinant3x3(src.m00, src.m01, src.m03, src.m20, src.m21, src.m23, src.m30, src.m31,
                src.m33);
            var t13 = Determinant3x3(src.m00, src.m01, src.m02, src.m20, src.m21, src.m22, src.m30, src.m31,
                src.m32);
            var t20 = Determinant3x3(src.m01, src.m02, src.m03, src.m11, src.m12, src.m13, src.m31, src.m32,
                src.m33);
            var t21 = -Determinant3x3(src.m00, src.m02, src.m03, src.m10, src.m12, src.m13, src.m30, src.m32,
                src.m33);
            var t22 = Determinant3x3(src.m00, src.m01, src.m03, src.m10, src.m11, src.m13, src.m30, src.m31,
                src.m33);
            var t23 = -Determinant3x3(src.m00, src.m01, src.m02, src.m10, src.m11, src.m12, src.m30, src.m31,
                src.m32);
            var t30 = -Determinant3x3(src.m01, src.m02, src.m03, src.m11, src.m12, src.m13, src.m21, src.m22,
                src.m23);
            var t31 = Determinant3x3(src.m00, src.m02, src.m03, src.m10, src.m12, src.m13, src.m20, src.m22,
                src.m23);
            var t32 = -Determinant3x3(src.m00, src.m01, src.m03, src.m10, src.m11, src.m13, src.m20, src.m21,
                src.m23);
            var t33 = Determinant3x3(src.m00, src.m01, src.m02, src.m10, src.m11, src.m12, src.m20, src.m21,
                src.m22);
            dest.m00 = t00 * determinant_inv;
            dest.m11 = t11 * determinant_inv;
            dest.m22 = t22 * determinant_inv;
            dest.m33 = t33 * determinant_inv;
            dest.m01 = t10 * determinant_inv;
            dest.m10 = t01 * determinant_inv;
            dest.m20 = t02 * determinant_inv;
            dest.m02 = t20 * determinant_inv;
            dest.m12 = t21 * determinant_inv;
            dest.m21 = t12 * determinant_inv;
            dest.m03 = t30 * determinant_inv;
            dest.m30 = t03 * determinant_inv;
            dest.m13 = t31 * determinant_inv;
            dest.m31 = t13 * determinant_inv;
            dest.m32 = t23 * determinant_inv;
            dest.m23 = t32 * determinant_inv;
            return dest;
        }

        return null;
    }

    public Matrix Negate(Matrix dest)
    {
        return Negate(this, dest);
    }

    public static Matrix Negate(Matrix src, Matrix dest)
    {
        if(dest == null) dest = new Matrix();

        dest.m00 = -src.m00;
        dest.m01 = -src.m01;
        dest.m02 = -src.m02;
        dest.m03 = -src.m03;
        dest.m10 = -src.m10;
        dest.m11 = -src.m11;
        dest.m12 = -src.m12;
        dest.m13 = -src.m13;
        dest.m20 = -src.m20;
        dest.m21 = -src.m21;
        dest.m22 = -src.m22;
        dest.m23 = -src.m23;
        dest.m30 = -src.m30;
        dest.m31 = -src.m31;
        dest.m32 = -src.m32;
        dest.m33 = -src.m33;
        return dest;
    }

    public void Orthographic(float left, float right, float bottom, float top, float near, float far)
    {
        var x = 2 / (right - left);
        var y = 2 / (top - bottom);
        var z = -2 / (far - near);

        var tx = -(right + left) / (right - left);
        var ty = -(top + bottom) / (top - bottom);
        var tz = -(far + near) / (far - near);

        m00 = x;
        m10 = 0;
        m20 = 0;
        m30 = 0;
        m01 = 0;
        m11 = y;
        m21 = 0;
        m31 = 0;
        m02 = 0;
        m12 = 0;
        m22 = z;
        m32 = 0;
        m03 = tx;
        m13 = ty;
        m23 = tz;
        m33 = 1;
    }

    public void ToAffine(Transform transform)
    {
        m00 = transform.m00;
        m10 = transform.m10;
        m20 = 0;
        m30 = 0;
        m01 = transform.m01;
        m11 = transform.m11;
        m21 = 0;
        m31 = 0;
        m02 = 0;
        m12 = 0;
        m22 = 1;
        m32 = 0;
        m03 = transform.m02;
        m13 = transform.m12;
        m23 = 0;
        m33 = 1;
    }

}