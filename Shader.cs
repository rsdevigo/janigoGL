using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace janigoGL
{
  class Shader : IDisposable
  {
    public int Handle;
    private bool disposedValue = false;

    Dictionary<string, int> _uniformLocations;


    public Shader(string vertexPath, string fragmentPath)
    {
      int VertexShader, FragmentShader;

      string VertexShaderSource;

      using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8))
      {
        VertexShaderSource = reader.ReadToEnd();
      }

      string FragmentShaderSource;

      using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8))
      {
        FragmentShaderSource = reader.ReadToEnd();
      }

      VertexShader = GL.CreateShader(ShaderType.VertexShader);
      GL.ShaderSource(VertexShader, VertexShaderSource);

      FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
      GL.ShaderSource(FragmentShader, FragmentShaderSource);

      GL.CompileShader(VertexShader);
      string infoLogVert = GL.GetShaderInfoLog(VertexShader);
      if (infoLogVert != System.String.Empty)
        System.Console.WriteLine(infoLogVert);

      GL.CompileShader(FragmentShader);

      string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

      if (infoLogFrag != System.String.Empty)
        System.Console.WriteLine(infoLogFrag);

      Handle = GL.CreateProgram();

      GL.AttachShader(Handle, VertexShader);
      GL.AttachShader(Handle, FragmentShader);

      GL.LinkProgram(Handle);

      GL.DetachShader(Handle, VertexShader);
      GL.DetachShader(Handle, FragmentShader);
      GL.DeleteShader(VertexShader);
      GL.DeleteShader(FragmentShader);

      GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

      // Next, allocate the dictionary to hold the locations.
      _uniformLocations = new Dictionary<string, int>();

      // Loop over all the uniforms,
      for (var i = 0; i < numberOfUniforms; i++)
      {
        // get the name of this uniform,
        var key = GL.GetActiveUniform(Handle, i, out _, out _);
        // get the location,
        var location = GL.GetUniformLocation(Handle, key);

        // and then add it to the dictionary.
        _uniformLocations.Add(key, location);
      }
    }

    ~Shader()
    {
      GL.DeleteProgram(Handle);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        GL.DeleteProgram(Handle);
        disposedValue = true;
      }
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    public void Use()
    {
      GL.UseProgram(Handle);
    }

    public void SetFloat(float v, string name)
    {
      GL.UseProgram(Handle);
      int location = GL.GetUniformLocation(Handle, name);
      if (location != -1)
        GL.Uniform1(location, v);
    }

    public void SetInt(int v, string name)
    {
      GL.UseProgram(Handle);
      int location = GL.GetUniformLocation(Handle, name);
      if (location != -1)
        GL.Uniform1(location, v);
    }

    public void SetVector3(Vector3 v, string name)
    {
      GL.UseProgram(Handle);
      int location = GL.GetUniformLocation(Handle, name);
      if (location != -1)
        GL.Uniform3(location, v);
    }

    public void SetMatrix4(Matrix4 v, string name)
    {
      GL.UseProgram(Handle);
      int location = GL.GetUniformLocation(Handle, name);
      if (location != -1)
        GL.UniformMatrix4(location, true, ref v);
    }


  }
}