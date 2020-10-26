using System;
using System.Collections.Generic;
using janigoGL;
using Assimp;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Scene
{
  class Actor
  {
    List<Mesh> _meshes = new List<Mesh>();
    List<Texture> texturesLoaded = new List<Texture>();
    Transform _transform;
    Material _material;
    String directory;

    public Transform transform { set { _transform = value; } }
    public Material material { set { _material = value; } }

    public Actor(string path)
    {
      directory = path.Substring(0, path.LastIndexOf("/"));
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
      List<Texture> textures = new List<Texture>();
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

      if (amesh.MaterialIndex >= 0)
      {
        Assimp.Material material = scene.Materials[amesh.MaterialIndex];
        List<Texture> diffuseMaps = loadMaterialTextures(material, TextureType.Diffuse, "texture_diffuse");
        textures.InsertRange(textures.Count, diffuseMaps);
        List<Texture> specularMaps = loadMaterialTextures(material, TextureType.Specular, "texture_specular");
        textures.InsertRange(textures.Count, specularMaps);
      }

      return new Mesh(vertices, indices, textures);
    }

    public void Draw(Shader shader)
    {
      shader.SetMatrix4(_transform.LocalToWorldMatrix(), "modelMatrix");
      _material.LoadMaterial(shader);
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

    List<Texture> loadMaterialTextures(Assimp.Material mat, Assimp.TextureType type, string typeName)
    {
      List<Texture> textures = new List<Texture>();
      for (int i = 0; i < mat.GetMaterialTextureCount(type); i++)
      {
        bool skip = false;
        TextureSlot textureSlot;
        mat.GetMaterialTexture(type, i, out textureSlot);
        for (int j = 0; j < texturesLoaded.Count; j++)
        {

          if (texturesLoaded[j].path.CompareTo(textureSlot.FilePath) == 0)
          {
            skip = true;
            textures.Add(texturesLoaded[j]);
            break;
          }
        }

        if (!skip)
        {
          Texture texture = new Texture();
          texture.id = TextureFromFile(textureSlot.FilePath, directory);
          texture.type = typeName;
          texture.path = textureSlot.FilePath;
          textures.Add(texture);
          texturesLoaded.Add(texture);
        }

      }
      return textures;
    }

    public uint TextureFromFile(String filename, String directory)
    {
      filename = directory + '/' + filename;

      uint textureID;
      GL.GenTextures(1, out textureID);
      Image<Rgba32> image = Image.Load<Rgba32>(filename);

      if (image != null)
      {
        image.Mutate(x => x.Flip(FlipMode.Vertical));

        List<byte> pixels = new List<byte>();

        for (int y = 0; y < image.Height; y++)
        {
          Span<Rgba32> p = image.GetPixelRowSpan(y);
          for (int x = 0; x < image.Width; x++)
          {
            pixels.Add(p[x].R);
            pixels.Add(p[x].G);
            pixels.Add(p[x].B);
            pixels.Add(p[x].A);
          }
        }

        GL.BindTexture(TextureTarget.Texture2D, textureID);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)OpenTK.Graphics.OpenGL4.TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)OpenTK.Graphics.OpenGL4.TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        image.Dispose();
      }
      else
      {
        Console.WriteLine("Texture failed to load at path: " + filename);
      }

      return textureID;
    }
  }
}