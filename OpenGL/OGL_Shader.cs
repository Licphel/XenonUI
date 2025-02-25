﻿using System.Numerics;
using OpenTK.Graphics.OpenGL;
using XenonIO.IO;
using XenonUI.Core;
using XenonUI.Graph;
using XenonUI.Maths;

namespace XenonUI.OpenGL;

public class OGL_Shader : Shader
{

	public int id;
	public Setup SetupDele;

	public OGL_Shader(Setup setup)
	{
		SetupDele = setup;
		NativeManager.I0.Remind(Release);
	}

	public ShaderAttribute GetAttribute(string attr)
	{
		return new ShaderAttribute(GL.GetAttribLocation(id, attr));
	}

	public ShaderUniform GetUniform(string name)
	{
		return new ShaderUniform(GL.GetUniformLocation(id, name));
	}

	public override void Bind()
	{
		GL.UseProgram(id);
	}

	public override void Unbind()
	{
		GL.UseProgram(0);
	}

	public override void Setup()
	{
		SetupDele?.Invoke(this);
	}

	public void Release()
	{
		Unbind();
		GL.DeleteProgram(id);
	}

}

public struct ShaderAttribute
{

	private readonly uint id;

	public ShaderAttribute(int id)
	{
		this.id = (uint) id;
	}

	public void Ptr(VertexAttribPointerType type, int sizeb, int strideb, int offb)
	{
		GL.VertexAttribPointer(id, sizeb, type, false, strideb, offb);
	}

	public void PtrFloat(int size, int stride, int offset)
	{
		GL.VertexAttribPointer(id, size, VertexAttribPointerType.Float, false,
			stride * sizeof(float), offset * sizeof(float));
	}

	public void PtrByte(int size, int stride, int offset)
	{
		GL.VertexAttribPointer(id, size, VertexAttribPointerType.UnsignedByte, false,
			stride * sizeof(byte), offset * sizeof(byte));
	}

	public void Enable()
	{
		GL.EnableVertexAttribArray(id);
	}

}

public struct ShaderUniform
{

	private readonly int id;

	public ShaderUniform(int id)
	{
		this.id = id;
	}

	public unsafe void SetMat4(Matrix matrix)
	{
		Matrix4x4 m4 = OGL_Util.ToM4(matrix);
		GL.UniformMatrix4fv(id, 1, true, (float*) &m4);
	}

	public void Set1(double i)
	{
		GL.Uniform1d(id, i);
	}

	public void Set2(double x, double y)
	{
		GL.Uniform2d(id, x, y);
	}

	public void Set3(double x, double y, double z)
	{
		GL.Uniform3d(id, x, y, z);
	}

	public void Set4(double x, double y, double z, double w)
	{
		GL.Uniform4d(id, x, y, z, w);
	}

	public void SetTexUnit(int i, int unit)
	{
		GL.ActiveTexture((TextureUnit) ((int) TextureUnit.Texture0 + unit));
		GL.BindTexture(TextureTarget.Texture2d, i);
		GL.Uniform1i(id, unit);
		GL.ActiveTexture(TextureUnit.Texture0);
	}

}

public delegate void Setup(OGL_Shader program);

public class ShaderBuilds
{

	public static OGL_Shader Build(FileHandle fileNoSuffix, Setup shaderSetup = null)
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

	public static OGL_Shader Build(string vertex, string fragment, Setup shaderSetup = null)
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

		if(id == 0)
		{
			throw new Exception("Fail to create shader type :" + type);
		}

		GL.ShaderSource(id, code);
		GL.CompileShader(id);

		GL.GetShaderInfoLog(id, out string txt);

		if(!string.IsNullOrEmpty(txt))
		{
			throw new Exception("GLERR => " + txt);
		}

		return id;
	}

}
