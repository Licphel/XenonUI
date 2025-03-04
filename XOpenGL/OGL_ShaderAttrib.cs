using OpenTK.Graphics.OpenGL;
using XenonUI.Graph;

namespace XenonUI.XOpenGL;

public class OGL_ShaderAttrib : ShaderAttrib
{

    private readonly uint id;

    public OGL_ShaderAttrib(int id)
    {
        this.id = (uint)id;
        GL.EnableVertexAttribArray(this.id);
    }

    public void Pointer(ShaderAttrib.Type type, int components, int stride, int offset)
    {
        VertexAttribPointerType type1 = type switch
        {
            ShaderAttrib.Type.UByte => VertexAttribPointerType.UnsignedByte,
            ShaderAttrib.Type.Int => VertexAttribPointerType.Int,
            ShaderAttrib.Type.Float => VertexAttribPointerType.Float
        };
        GL.VertexAttribPointer(id, components, type1, false, stride, offset);
    }

}