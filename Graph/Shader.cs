using XenonUI.Maths;

namespace XenonUI.Graph;

public interface Shader
{

    void Setup();

    void Bind();

    void Unbind();

    ShaderUniform GetUniform(string name);

    ShaderAttrib GetAttrib(string name);

}