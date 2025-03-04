using KryptonM;
using OpenTK.Graphics.OpenGL;

namespace XenonUI.XOpenGL;

public class OGL_VBO<T> where T : unmanaged
{

    private readonly BufferTarget type;

    public int Id;

    public unsafe OGL_VBO(T* data, int len, BufferTarget target, BufferUsage hint)
    {
        type = target;
        Id = GL.GenBuffer();

        Bind();

        GL.BufferData(type, len * sizeof(T), (IntPtr)data, hint);

        NativeManager.I0.Remind(() => GL.DeleteBuffer(Id));
    }

    public void Bind()
    {
        GL.BindBuffer(type, Id);
    }

    public void Unbind()
    {
        GL.BindBuffer(type, 0);
    }

    public unsafe void UpdateBuffer(nint offset, T* data, int len)
    {
        GL.BufferSubData(type, offset, len * sizeof(T), (IntPtr)data);
    }

}