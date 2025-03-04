using KryptonM;
using OpenTK.Graphics.OpenGL;
using XenonUI.Graph;

namespace XenonUI.XOpenGL;

public class OGL_Shader : Shader
{

    public int id;
    public Action<OGL_Shader> SetupDele;

    public OGL_Shader(Action<OGL_Shader> setup)
    {
        SetupDele = setup;
        NativeManager.I0.Remind(() =>
        {
            Unbind();
            GL.DeleteProgram(id);
        });
    }
    
    public ShaderUniform GetUniform(string name)
    {
        return new OGL_ShaderUniform(GL.GetUniformLocation(id, name));
    }

    public ShaderAttrib GetAttrib(string name)
    {
        return new OGL_ShaderAttrib(GL.GetAttribLocation(id, name));
    }

    public void Bind() => GL.UseProgram(id);
    public void Unbind() => GL.UseProgram(0);
    public void Setup() => SetupDele?.Invoke(this);

}