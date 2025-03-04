using System.Runtime.CompilerServices;
using KryptonM;
using OpenTK.Graphics.OpenGL;
using XenonUI.Graph;
using XenonUI.Graph.Images;
using XenonUI.Maths;

namespace XenonUI.XOpenGL;

public unsafe class OGL_Graphics : Graphics
{

    private static readonly Camera NullCam = new Camera();

    public OGL_Shader DefaultShader;
    public OGL_VBO<uint> Ebo;

    private ShaderUniform UniTexture;

    public OGL_VAO<byte, uint> Vao;
    public OGL_VBO<byte> Vbo;

    public OGL_Graphics(int size) : base(size)
    {
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        Ebo = new OGL_VBO<uint>(Buffer.Indices, Buffer.IndexCap, BufferTarget.ElementArrayBuffer, BufferUsage.DynamicDraw);
        Vbo = new OGL_VBO<byte>(Buffer.Vertices, Buffer.VertexCap, BufferTarget.ArrayBuffer, BufferUsage.DynamicDraw);
        Vao = new OGL_VAO<byte, uint>(Vbo, Ebo);

        DefaultShader = GetDefaultShader();

        Load(DefaultShader);

        UniTexture = DefaultShader.GetUniform("u_texture");
    }
    
    public void Load(OGL_Shader program)
    {
        Flush();

        Vao.Bind();

        Program = program;
        program.Bind();
        program.Setup();
    }

    public override void UseShader(Shader program)
    {
        Load((OGL_Shader)program);
    }

    public override void UseDefaultShader()
    {
        Load(DefaultShader);
    }

    public override void Flush()
    {
        if(Buffer.Size <= 0) return;

        Program.Bind();

        UniTexture.Setsm(0, _ImageBinding);
        UniformAppender?.Invoke(this);

        Vao.Bind();
        Vbo.Bind();
        Ebo.Bind();
        Vbo.UpdateBuffer(0, Buffer.Vertices0, Buffer.Size);

        GL.DrawElements(PrimitiveType.Triangles, Buffer.Index, DrawElementsType.UnsignedInt, 0);

        Program.Unbind();

        Buffer.Clear();
    }

    public override void Viewport(float x, float y, float w, float h)
    {
        GL.Viewport((int)x, (int)y, (int)w, (int)h);
        ViewportArray = new Vector4(x, y, w, h);
    }

    public override void Scissor(float x, float y, float w, float h)
    {
        Flush();
        float x0 = CameraNow.ToScrX(x, ViewportArray);
        float y0 = CameraNow.ToScrY(y, ViewportArray);
        float x1 = CameraNow.ToScrX(x + w, ViewportArray);
        float y1 = CameraNow.ToScrY(y + h, ViewportArray);
        GL.Scissor((int)x0 - 1, (int)y0 - 1, (int)(x1 - x0 + 1), (int)(y1 - y0 + 1));
        GL.Enable(EnableCap.ScissorTest);
    }

    public override void ScissorEnd()
    {
        Flush();
        GL.Disable(EnableCap.ScissorTest);
    }

    public override void Clear()
    {
        GL.ClearColor(OGL_Util.ToColor(OGL.Settings.ClearColor));
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    //-----

    private static OGL_Shader GetDefaultShader()
    {
        const string vert = "#version 150 core\n" +
                            "in vec2 i_position;\n" +
                            "in vec4 i_color;\n" +
                            "in vec2 i_texCoord;\n" +
                            "out vec4 o_color;\n" +
                            "out vec2 o_texCoord;\n" +
                            "void main() {\n" +
                            "    o_color = i_color;\n" +
                            "    o_texCoord = i_texCoord;\n" +
                            "    gl_Position = vec4(i_position, 0.0, 1.0);\n" +
                            "}\n";
        const string frag = "#version 150 core\n" +
                            "in vec4 o_color;\n" +
                            "in vec2 o_texCoord;\n" +
                            "out vec4 fragColor;\n" +
                            "uniform sampler2D u_texture;\n" +
                            "void main() {\n" +
                            "    fragColor = o_color * texture(u_texture, o_texCoord);\n" +
                            "}";
        return OGL_ShaderFactory.Build(vert, frag, program =>
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