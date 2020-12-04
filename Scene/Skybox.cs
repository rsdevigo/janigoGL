using janigoGL;
using OpenTK.Graphics.OpenGL4;
using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using OpenTK;
namespace Scene {
    class Skybox {
        private Shader shader;
        private uint textureID;
        string[] faces = {
            "right.jpg",
            "left.jpg",
            "top.jpg",
            "bottom.jpg",
            "front.jpg",
            "back.jpg"
        };
        private uint cubeVAO;
        private uint cubeVBO;
        private float[] skyboxVertices = {
            // positions          
            -1.0f,  1.0f, -1.0f,
            -1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,
            1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,

            -1.0f, -1.0f,  1.0f,
            -1.0f, -1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f,  1.0f,
            -1.0f, -1.0f,  1.0f,

            1.0f, -1.0f, -1.0f,
            1.0f, -1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f,  1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,

            -1.0f, -1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f, -1.0f,  1.0f,
            -1.0f, -1.0f,  1.0f,

            -1.0f,  1.0f, -1.0f,
            1.0f,  1.0f, -1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f, -1.0f,

            -1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f,  1.0f,
            1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f,  1.0f,
            1.0f, -1.0f,  1.0f
        };
        public Skybox(string filename) {
            SetupCubeMesh();
            LoadCubemap(filename);
            shader = new Shader("./skybox_shader.vert", "./skybox_shader.frag");
        }

        public void Draw(Camera _camera) {
            GL.DepthFunc(DepthFunction.Lequal);
            GL.DepthMask(false);
            shader.Use();
            shader.SetMatrix4(new Matrix4(new Matrix3(_camera._viewMatrix)), "viewMatrix");
            shader.SetMatrix4(_camera._projectionMatrix, "projectionMatrix");
            GL.BindVertexArray(cubeVAO);
            GL.ActiveTexture(TextureUnit.Texture0);
            shader.SetInt(0, "skybox");
            GL.BindTexture(TextureTarget.TextureCubeMap, textureID);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            GL.BindVertexArray(0);
            GL.DepthFunc(DepthFunction.Less);
            GL.DepthMask(true);
        }

        void LoadCubemap(string filename)
        {
            GL.GenTextures(1, out textureID);
            GL.BindTexture(TextureTarget.TextureCubeMap, textureID);
            
            for (int i = 0; i < faces.Length; i++)
            {
                LoadImage(filename+"_"+faces[i], i);
            }

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)OpenTK.Graphics.OpenGL4.TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)OpenTK.Graphics.OpenGL4.TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)OpenTK.Graphics.OpenGL4.TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        } 

        public void LoadImage (string filename, int i) {
            Image<Rgba32> image = Image.Load<Rgba32>(filename);

            if (image != null)
            {
                //image.Mutate(x => x.Flip(FlipMode.Vertical));

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
                TextureTarget target = (TextureTarget.TextureCubeMapPositiveX+i);
                GL.TexImage2D(target, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
                image.Dispose();
            }
            else
            {
                Console.WriteLine("Texture failed to load at path: " + filename);
            }
        }

        public void SetupCubeMesh() {
            GL.GenVertexArrays(1, out cubeVAO);
            GL.GenBuffers(1, out cubeVBO);
            GL.BindVertexArray(cubeVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, cubeVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, skyboxVertices.Length * sizeof(float), skyboxVertices, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.BindVertexArray(0);
        }
    }
}