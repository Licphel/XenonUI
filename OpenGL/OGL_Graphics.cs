using System.Runtime.CompilerServices;
using OpenTK.Graphics.OpenGL;
using XenonUI.Core;
using XenonUI.Graph;
using XenonUI.Graph.IMP;
using XenonUI.Maths;

namespace XenonUI.OpenGL;

public unsafe class OGL_Graphics : Graphics
{

	private static readonly Camera NullCam = new Camera();

	public uint* Indices;
	public int Indlen;
	private float InvTexHeight;
	private float InvTexWidth;

	public int MinVertexBufSize = 256;
	public int NumIndices;
	public int NumVertices;

	public OGL_Shader DefaultShader;

	private int TextureID;
	private ShaderUniform UniProjection;
	private ShaderUniform UniTexture;

	public OGL_VAO<float, uint> Vao;
	public OGL_VBO<float> Vbo;
	public OGL_VBO<uint> Ebo;
	
	public float* Vertices;
	public float* Vertice0;
	public int Verlen;

	public OGL_Graphics(int size)
	{
		NormalizeColor();
		//1 sprite - 4 vert - 6 ind
		Vertice0 = Vertices = NativeMem.MemReallocate(Vertices, Verlen = size, 0);
		Indices = NativeMem.MemReallocate(Indices, Indlen = size / 2 * 3, 0);

		for(uint i = 0, k = 0; i < size / 4; i += 6, k += 4)
		{
			Indices[i + 0] = 0 + k;
			Indices[i + 1] = 1 + k;
			Indices[i + 2] = 3 + k;
			Indices[i + 3] = 1 + k;
			Indices[i + 4] = 2 + k;
			Indices[i + 5] = 3 + k;
		}

		GL.Enable(EnableCap.Blend);
		GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

		Ebo = new OGL_VBO<uint>(Indices, Indlen, BufferTarget.ElementArrayBuffer, BufferUsage.DynamicDraw);
		Vbo = new OGL_VBO<float>(Vertices, Verlen, BufferTarget.ArrayBuffer, BufferUsage.DynamicDraw);
		Vao = new OGL_VAO<float, uint>(Vbo, Ebo);

		DefaultShader = GetDefaultShader();

		Load(DefaultShader);

		UniTexture = DefaultShader.GetUniform("u_texture");

		//Get as default
		ViewportArray = new Vector4(0, 0, GraphicsDevice.Global.Size.x, GraphicsDevice.Global.Size.y);
	}

	public void Load(OGL_Shader program)
	{
		Flush();

		Vao.Bind();

		Program = program;
		program.Bind();
		program.Setup();

		UniProjection = program.GetUniform("u_proj");
		UniProjection.SetMat4(Projection);
	}

	public override void UseShader(Shader program)
	{
		Load((OGL_Shader) program);
	}

	public override void UseDefaultShader()
	{
		Load(DefaultShader);
	}

	public override void DrawImage(Image image, float x, float y, float width, float height, float srcX, float srcY,
	                          float srcWidth, float srcHeight)
	{
		image = image ?? TexMissing;

		OGL_Image glt = (OGL_Image) image;

		CheckTransformAndCap();

		int id = glt.Id;

		if(id != TextureID)
		{
			InvTexWidth = 1f / image.Width;
			InvTexHeight = 1f / image.Height;
			Flush();
		}

		TextureID = id;

		float x1;
		float y1;
		float x2;
		float y2;
		float x3;
		float y3;
		float x4;
		float y4;

		if(!Matrices.IsEmpty)
		{
			x1 = x + Transform.m02;
			y1 = y + Transform.m12;
			x2 = x + Transform.m01 * height + Transform.m02;
			y2 = y + Transform.m11 * height + Transform.m12;
			x3 = x + Transform.m00 * width + Transform.m01 * height + Transform.m02;
			y3 = y + Transform.m10 * width + Transform.m11 * height + Transform.m12;
			x4 = x + Transform.m00 * width + Transform.m02;
			y4 = y + Transform.m10 * width + Transform.m12;
		}
		else
		{
			x1 = x;
			y1 = y;
			x2 = x;
			y2 = y + height;
			x3 = x + width;
			y3 = y + height;
			x4 = x + width;
			y4 = y;
		}

		float u = srcX * InvTexWidth;
		float v = srcY * InvTexHeight;
		float u2 = (srcX + srcWidth) * InvTexWidth;
		float v2 = (srcY + srcHeight) * InvTexHeight;

		if(FlipX)
		{
			(u, u2) = (u2, u);
		}

		if(FlipY)
		{
			(v, v2) = (v2, v);
		}

		if(glt.IsFB)
		{
			(v, v2) = (v2, v);
		}

		{
			//RT
			*Vertices++ = x3;
			*Vertices++ = y3;
			*Vertices++ = _colors[2].R;
			*Vertices++ = _colors[2].G;
			*Vertices++ = _colors[2].B;
			*Vertices++ = _colors[2].A;
			*Vertices++ = u2;
			*Vertices++ = v;

			VertAppenders?[2]?.Invoke(this);

			//RD
			*Vertices++ = x4;
			*Vertices++ = y4;
			*Vertices++ = _colors[3].R;
			*Vertices++ = _colors[3].G;
			*Vertices++ = _colors[3].B;
			*Vertices++ = _colors[3].A;
			*Vertices++ = u2;
			*Vertices++ = v2;

			VertAppenders?[3]?.Invoke(this);

			//LD
			*Vertices++ = x1;
			*Vertices++ = y1;
			*Vertices++ = _colors[0].R;
			*Vertices++ = _colors[0].G;
			*Vertices++ = _colors[0].B;
			*Vertices++ = _colors[0].A;
			*Vertices++ = u;
			*Vertices++ = v2;

			VertAppenders?[0]?.Invoke(this);

			//LT
			*Vertices++ = x2;
			*Vertices++ = y2;
			*Vertices++ = _colors[1].R;
			*Vertices++ = _colors[1].G;
			*Vertices++ = _colors[1].B;
			*Vertices++ = _colors[1].A;
			*Vertices++ = u;
			*Vertices++ = v;

			VertAppenders?[1]?.Invoke(this);
		}

		NewVertex(32);
		NewIndex(6);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void CheckTransformAndCap()
	{
		if(Verlen - (Vertices - Vertice0) < MinVertexBufSize)
		{
			Flush();
		}

		if(Matrices.Changed)
		{
			Transform.Set(Matrices.Top);
			Matrices.Changed = false;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void Write(float v)
	{
		*Vertices++ = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void Write(float v1, float v2)
	{
		*Vertices++ = v1;
		*Vertices++ = v2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void Write(float v1, float v2, float v3)
	{
		*Vertices++ = v1;
		*Vertices++ = v2;
		*Vertices++ = v3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void Write(Vector2 vec)
	{
		*Vertices++ = vec.x;
		*Vertices++ = vec.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void Write(Vector3 vec)
	{
		*Vertices++ = vec.x;
		*Vertices++ = vec.y;
		*Vertices++ = vec.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void Write(Vector4 vec)
	{
		*Vertices++ = vec.x;
		*Vertices++ = vec.y;
		*Vertices++ = vec.z;
		*Vertices++ = vec.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void Write(params float[] arr)
	{
		for(int i = 0; i < arr.Length; i++)
		{
			*Vertices++ = arr[i];
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void WriteTransformed(Vector2 vec)
	{
		Vector2 vecTrf = Transform.ApplyTo(ref vec);

		*Vertices++ = vecTrf.x;
		*Vertices++ = vecTrf.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void NewVertex(int v)
	{
		NumVertices += v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void NewIndex(int v)
	{
		NumIndices += v;
	}

	public override void Flush()
	{
		if(NumVertices <= 0)
		{
			return;
		}

		Program.Bind();

		UniTexture.SetTexUnit(TextureID, 0);
		UniProjection.SetMat4(Projection);
		UniformAppender?.Invoke(this);

		Vao.Bind();
		Vbo.Bind();
		Ebo.Bind();
		Vbo.UpdateBuffer(0, Vertice0, NumVertices);

		GL.DrawElements(PrimitiveType.Triangles, NumIndices, DrawElementsType.UnsignedInt, 0);

		Program.Unbind();
		
		NumVertices = 0;
		NumIndices = 0;
		Vertices = Vertice0;
	}

	public override bool SupportTransformation => true;

	public override void Viewport(float x, float y, float w, float h)
	{
		GL.Viewport((int) x, (int) y, (int) w, (int) h);
		ViewportArray = new Vector4(x, y, w, h);
	}

	public override void Scissor(float x, float y, float w, float h)
	{
		Flush();
		float x0 = CameraNow.ToScrX(x, ViewportArray);
		float y0 = CameraNow.ToScrY(y, ViewportArray);
		float x1 = CameraNow.ToScrX(x + w, ViewportArray);
		float y1 = CameraNow.ToScrY(y + h, ViewportArray);
		GL.Scissor((int) x0 - 1, (int) y0 - 1, (int) (x1 - x0 + 1), (int) (y1 - y0 + 1));
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

	public override void UseCamera(Camera camera)
	{
		Flush();
		Projection.ToAffine(camera.CombinedTransform);
		UniProjection.SetMat4(Projection);
		CameraNow = camera;
	}

	public override void EndCamera(Camera camera)
	{
		NullCam.Viewport.Set(0, 0, GraphicsDevice.Global.Size.x, GraphicsDevice.Global.Size.y);
		NullCam.ToCenter();
		NullCam.Push();
		UseCamera(NullCam);
	}

	//-----

	private static OGL_Shader GetDefaultShader()
	{
		const string vert = "#version 150 core\n" +
		                    "\n" +
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
		return ShaderBuilds.Build(vert, frag, program =>
		{
			ShaderAttribute posAttrib = program.GetAttribute("i_position");
			posAttrib.Enable();
			ShaderAttribute colAttrib = program.GetAttribute("i_color");
			colAttrib.Enable();
			ShaderAttribute texAttrib = program.GetAttribute("i_texCoord");
			texAttrib.Enable();

			posAttrib.Ptr(VertexAttribPointerType.Float, 2, 32, 0);
			colAttrib.Ptr(VertexAttribPointerType.Float, 4, 32, 8);
			texAttrib.Ptr(VertexAttribPointerType.Float, 2, 32, 24);
		});
	}

}
