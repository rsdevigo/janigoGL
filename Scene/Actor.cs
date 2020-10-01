using System;
using System.Collections.Generic;
using opentk_example;
using Assimp;

namespace Scene
{
  class Actor
  {
    List<Mesh> _meshes = new List<Mesh>();
    Transform _transform;

    public Transform transform { set { _transform = value; } }

    public Actor(string path)
    {
      LoadActor(path);
    }

    private void LoadActor(string path)
    {
      AssimpContext context = new AssimpContext();
      Assimp.Scene scene = context.ImportFile(path,
        PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs |
        PostProcessSteps.GenerateSmoothNormals | PostProcessSteps.CalculateTangentSpace
      );

      if (scene == null || scene.RootNode == null || scene.SceneFlags == SceneFlags.Incomplete)
      {
        Console.WriteLine("Houve um erro ao carregar o ator");
        return;
      }

      ProcessNode(scene.RootNode, scene);
    }

    private void ProcessNode(Assimp.Node node, Assimp.Scene scene)
    {
      for (int i = 0; i < node.MeshCount; i++)
      {
        Assimp.Mesh amesh = scene.Meshes[node.MeshIndices[i]];
        _meshes.Add(ProcessMesh(amesh, scene));
      }

      for (int i = 0; i < node.ChildCount; i++)
      {
        ProcessNode(node.Children[i], scene);
      }
    }

    private Mesh ProcessMesh(Assimp.Mesh amesh, Assimp.Scene scene)
    {
      List<Vertex> vertices = new List<Vertex>();
      List<uint> indices = new List<uint>();
      Random r = new Random();
      for (int i = 0; i < amesh.VertexCount; i++)
      {
        Vertex vertex = new Vertex();
        vertex.positionX = amesh.Vertices[i].X;
        vertex.positionY = amesh.Vertices[i].Y;
        vertex.positionZ = amesh.Vertices[i].Z;
        if (amesh.HasNormals)
        {
          vertex.normalX = amesh.Normals[i].X;
          vertex.normalY = amesh.Normals[i].Y;
          vertex.normalZ = amesh.Normals[i].Z;
        }

        if (amesh.HasTextureCoords(0))
        {
          vertex.textureX = amesh.TextureCoordinateChannels[0][i].X;
          vertex.textureY = amesh.TextureCoordinateChannels[0][i].Y;
          vertex.bitangentX = amesh.BiTangents[i].X;
          vertex.bitangentY = amesh.BiTangents[i].Y;
          vertex.bitangentZ = amesh.BiTangents[i].Z;
          vertex.tangentX = amesh.Tangents[i].X;
          vertex.tangentY = amesh.Tangents[i].Y;
          vertex.tangentZ = amesh.Tangents[i].Z;
        }
        else
        {
          vertex.textureX = 0f;
          vertex.textureY = 0f;
        }
        vertex.colorA = (float)r.NextDouble();
        vertex.colorR = (float)r.NextDouble();
        vertex.colorB = (float)r.NextDouble();
        vertex.colorG = 1f;
        vertices.Add(vertex);
      }

      for (int i = 0; i < amesh.FaceCount; i++)
      {
        Assimp.Face face = amesh.Faces[i];
        for (int j = 0; j < face.IndexCount; j++)
        {

          indices.Add((uint)face.Indices[j]);
        }
      }

      return new Mesh(vertices, indices, new List<Texture>());
    }

    public void Draw(Shader shader)
    {
      shader.SetMatrix4(_transform.LocalToWorldMatrix(), "modelMatrix");
      _meshes.ForEach(mesh =>
      {
        mesh.Draw(shader);
      });
    }

    public void UnLoad()
    {
      _meshes.ForEach(mesh =>
      {
        mesh.UnLoad();
      });
    }
  }
}