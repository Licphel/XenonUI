using KryptonM.IO;
using OpenTK.Graphics.OpenGL;

namespace XenonUI.XOpenGL;

public class OGL_ShaderFactory
{

    public static OGL_Shader Build(FileHandle fileNoSuffix, Action<OGL_Shader> shaderSetup = null)
    {
        string path = fileNoSuffix.Path;
        try
        {
            //if not using this way to read, it won't keep "\n", which shaders needs.
            string vert = StringIO.Read(new FileHandleImpl(path + ".vert"));
            string frag = StringIO.Read(new FileHandleImpl(path + ".frag"));
            return Build(vert, frag, shaderSetup);
        }
        catch(IOException)
        {
            throw new FileNotFoundException("Not found: " + path);
        }
    }

    public static OGL_Shader Build(string vertex, string fragment, Action<OGL_Shader> shaderSetup = null)
    {
        OGL_Shader shader = new OGL_Shader(shaderSetup);
        shader.id = GL.CreateProgram();

        int vert = BuildShaderPart(vertex, ShaderType.VertexShader);
        int frag = BuildShaderPart(fragment, ShaderType.FragmentShader);

        GL.AttachShader(shader.id, vert);
        GL.AttachShader(shader.id, frag);

        GL.BindFragDataLocation(shader.id, 0, "fragColor");

        GL.LinkProgram(shader.id);

        GL.DetachShader(shader.id, vert);
        GL.DetachShader(shader.id, frag);

        GL.ValidateProgram(shader.id);

        GL.DeleteShader(vert);
        GL.DeleteShader(frag);

        return shader;
    }

    private static int BuildShaderPart(string code, ShaderType type)
    {
        int id = GL.CreateShader(type);

        if(id == 0) throw new Exception("Fail to create shader type :" + type);

        GL.ShaderSource(id, code);
        GL.CompileShader(id);

        GL.GetShaderInfoLog(id, out string txt);

        if(!string.IsNullOrEmpty(txt)) throw new Exception("GLERR => " + txt);

        return id;
    }

}