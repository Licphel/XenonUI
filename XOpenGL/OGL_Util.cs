using System.Numerics;
using OpenTK.Mathematics;
using XenonUI.Maths;
using Vector2 = System.Numerics.Vector2;
using Vector4 = XenonUI.Maths.Vector4;

namespace XenonUI.XOpenGL;

public class OGL_Util
{

    public static Color4<Rgba> ToColor(Vector4 vec)
    {
        return new Color4<Rgba>((int)(vec.x * 255), (int)(vec.y * 255), (int)(vec.z * 255), (int)(vec.w * 255));
    }

    public static Matrix4x4 ToM4(Transform t)
    {
        return new Matrix4x4(t.m00, t.m01, 0, t.m02, t.m10, t.m11, 0, t.m12, 0, 0, 1, 0, 0, 0, 0, 1);
    }

    public static Maths.Vector2 SysToVec2(Vector2i vec)
    {
        return new Maths.Vector2(vec.X, vec.Y);
    }

    public static Vector2 ToSysVec2(Maths.Vector2 vec)
    {
        return new Vector2(vec.x, vec.y);
    }

}