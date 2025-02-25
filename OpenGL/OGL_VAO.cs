using OpenTK.Graphics.OpenGL;
using XenonUI.Core;

namespace XenonUI.OpenGL;

public class OGL_VAO<T, I>
    where T : unmanaged
    where I : unmanaged
{

    public int Id;

    public OGL_VAO(OGL_VBO<T> vbo, OGL_VBO<I> ebo)
    {
        Id = GL.GenVertexArray();

        Bind();
        vbo.Bind();
        ebo?.Bind();

        NativeManager.I0.Remind(() => GL.DeleteVertexArray(Id));
    }

    public void Bind()
    {
        GL.BindVertexArray(Id);
    }

    public void Unbind()
    {
        GL.BindVertexArray(0);
    }

}