using System;
using OpenTK;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using opentk_example;

namespace Scene
{
  class Mesh
  {
    List<Vertex> _vertices;
    List<uint> _indices;
    List<Texture> _textures;
    public int VAO;
    private int VBO;
    private int EBO;
    public Mesh(List<Vertex> vertices, List<uint> indices, List<Texture> texures)
    {
      _vertices = vertices;
      _indices = indices;
      _textures = texures;
      SetupMesh();
    }

    public void Draw(Shader shader)
    {
      GL.BindVertexArray(VAO);
      GL.DrawElements(BeginMode.Triangles, _indices.Count, DrawElementsType.UnsignedInt, 0);
      GL.BindVertexArray(0);
    }

    private void SetupMesh()
    {
      VAO = GL.GenVertexArray();
      VBO = GL.GenBuffer();
      EBO = GL.GenBuffer();
      GL.BindVertexArray(VAO);

      GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
      GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Count * Vertex.SizeInBytes, _vertices.ToArray(), BufferUsageHint.StaticDraw);

      GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
      GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Count * sizeof(uint), _indices.ToArray(), BufferUsageHint.StaticDraw);

      GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);
      GL.EnableVertexAttribArray(0);

      GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 3 * sizeof(float));
      GL.EnableVertexAttribArray(1);

      GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 7 * sizeof(float));
      GL.EnableVertexAttribArray(2);

      GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 10 * sizeof(float));
      GL.EnableVertexAttribArray(3);

      GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 12 * sizeof(float));
      GL.EnableVertexAttribArray(4);

      GL.VertexAttribPointer(5, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 15 * sizeof(float));
      GL.EnableVertexAttribArray(5);

      GL.BindVertexArray(0);
    }

    public void UnLoad()
    {
      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      GL.DeleteBuffer(VBO);
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
      GL.DeleteBuffer(EBO);
    }
  }

  struct Texture
  {
    uint id;
    string type;
  };

  struct Vertex
  {
    public float positionX;
    public float positionY;
    public float positionZ;
    public float colorR;
    public float colorG;
    public float colorB;
    public float colorA;
    public float normalX;
    public float normalY;
    public float normalZ;
    public float textureX;
    public float textureY;
    public float tangentX;
    public float tangentY;
    public float tangentZ;
    public float bitangentX;
    public float bitangentY;
    public float bitangentZ;
    public const int SizeInBytes = 72;
  }
}

