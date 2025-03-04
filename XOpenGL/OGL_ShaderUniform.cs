using System.Numerics;
using OpenTK.Graphics.OpenGL;
using XenonUI.Graph;
using XenonUI.Graph.Images;
using XenonUI.Maths;
using Vector2 = XenonUI.Maths.Vector2;
using Vector3 = XenonUI.Maths.Vector3;
using Vector4 = XenonUI.Maths.Vector4;

namespace XenonUI.XOpenGL;

public unsafe class OGL_ShaderUniform : ShaderUniform
{

    private readonly int id;

    public OGL_ShaderUniform(int id)
    {
        this.id = id;
    }

    public void Seti(int v)
    {
        GL.Uniform1i(id, v);
    }

    public void Setf(float v)
    {
        GL.Uniform1f(id, v);
    }
    
    public void Setf(Vector2 v)
    {
        GL.Uniform2f(id, v.x, v.y);
    }

    public void Setf(Vector3 v)
    {
        GL.Uniform3f(id, v.x, v.y, v.z);
    }

    public void Setf(Vector4 v)
    {
        GL.Uniform4f(id, v.x, v.y, v.z, v.w);
    }

    public void Setf(Transform v)
    {
        Matrix4x4 m4 = OGL_Util.ToM4(v);
        GL.UniformMatrix4fv(id, 1, true, (float*)&m4);
    }

    public void Setsm(int unit, Image img)
    {
        GL.ActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + unit));
        GL.BindTexture(TextureTarget.Texture2d, ((OGL_Image) img).Id);
        GL.Uniform1i(id, unit);
        GL.ActiveTexture(TextureUnit.Texture0);
    }

}