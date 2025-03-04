using XenonUI.Graph.Images;
using XenonUI.Maths;

namespace XenonUI.Graph;

public interface ShaderUniform
{
    
    void Seti(int v);
    void Setf(float v);
    void Setf(Vector2 v);
    void Setf(Vector3 v);
    void Setf(Vector4 v);
    void Setf(Transform v);
    void Setsm(int unit, Image img);

}