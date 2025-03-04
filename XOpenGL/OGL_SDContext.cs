using OpenTK.Windowing.GraphicsLibraryFramework;
using XenonUI.Graph;
using Image = XenonUI.Graph.Images.Image;

namespace XenonUI.XOpenGL;

public unsafe class OGL_SDContext : SDContext
{

    public static OGL_SDContext Global = new OGL_SDContext();

    public override SDBuffer GetBufferFromType(Image img = null)
    {
        return null;
    }

    public override void DrawBuffer(SDBuffer buf)
    {
        if(buf.Size <= 0) return;

        //var shader = buf.Shader;

        //shader.Bind();
        
        //UniTexture.SetTexUnit(TextureID, 0);
        //UniProjection.SetMat4(Projection);
        //UniformAppender?.Invoke(this);

        //Vao.Bind();
        //Vbo.Bind();
        //Ebo.Bind();
        //Vbo.UpdateBuffer(0, Vertice0, NumVertices);

        //GL.DrawElements(PrimitiveType.Triangles, NumIndices, DrawElementsType.UnsignedInt, 0);

      
        //shader.Unbind();
    }

    public override void SwapDoubleBuffers()
    {
        GLFW.SwapBuffers(OGL.Window);
    }

    private static Shader Shader_Sample_Triangle;
    
    static OGL_SDContext()
    {
        const string vert = "#version 150 core\n" + "\n" +
                            "in vec2 i_position;\n" +
                            "in vec4 i_color;\n" +
                            "in vec2 i_texCoord;\n" +
                            "\n" +
                            "out vec4 o_color;\n" +
                            "out vec2 o_texCoord;\n" +
                            "\n" +
                            "uniform mat4 u_proj;\n" +
                            "\n" +
                            "void main() {\n" +
                            "    o_color = i_color;\n" +
                            "    o_texCoord = i_texCoord;\n" +
                            "\n" +
                            "    gl_Position = u_proj * vec4(i_position, 0.0, 1.0);\n" +
                            "}\n";
        const string frag = "#version 150 core\n" +
                            "\n" +
                            "in vec4 o_color;\n" +
                            "in vec2 o_texCoord;\n" +
                            "\n" +
                            "out vec4 fragColor;\n" +
                            "\n" +
                            "uniform sampler2D u_texture;\n" +
                            "\n" +
                            "void main() {\n" +
                            "    fragColor = o_color * texture(u_texture, o_texCoord);\n" +
                            "}";
        Shader_Sample_Triangle = OGL_ShaderFactory.Build(vert, frag, program =>
        {
            ShaderAttrib posAttrib = program.GetAttrib("i_position");
            ShaderAttrib colAttrib = program.GetAttrib("i_color");
            ShaderAttrib texAttrib = program.GetAttrib("i_texCoord");
            
            posAttrib.Pointer(ShaderAttrib.Type.Float, 2, 32, 0);
            colAttrib.Pointer(ShaderAttrib.Type.Float, 4, 32, 8);
            texAttrib.Pointer(ShaderAttrib.Type.Float, 2, 32, 24);
        });
    }

}