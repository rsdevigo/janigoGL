using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL4;
using System;
using janigoGL;

namespace Scene {
    class FramebufferManager {
        public uint frameBuffer;
        public uint texture;
        private int width;
        private int height;
        public Shader shader;
        public float[] quadVertices = { // vertex attributes for a quad that fills the entire screen in Normalized Device Coordinates.
            // positions   // texCoords
            -1.0f,  1.0f,  0.0f, 1.0f,
            -1.0f, -1.0f,  0.0f, 0.0f,
            1.0f, -1.0f,  1.0f, 0.0f,

            -1.0f,  1.0f,  0.0f, 1.0f,
            1.0f, -1.0f,  1.0f, 0.0f,
            1.0f,  1.0f,  1.0f, 1.0f
        };

        public uint quadVAO;
        public uint quadVBO;
        public FramebufferManager(int Width, int Height, string vertexShaderPath, string fragShaderPath, FramebufferManagerMode mode = FramebufferManagerMode.COLOR_BUFFER) {
            shader = new Shader(vertexShaderPath, fragShaderPath);
            width = Width;
            height = Height;
            GL.GenFramebuffers(1, out frameBuffer);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);
            
            GenerateBuffer(mode);
            
            if(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
                Console.WriteLine("ERROR::FRAMEBUFFER:: Framebuffer is not complete!");
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            SetupQuadMesh();
        }

        public FramebufferManager(int Width, int Height, FramebufferManagerMode mode = FramebufferManagerMode.COLOR_BUFFER) {
            width = Width;
            height = Height;
            GL.GenFramebuffers(1, out frameBuffer);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);
            
            GenerateBuffer(mode);
            
            if(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
                Console.WriteLine("ERROR::FRAMEBUFFER:: Framebuffer is not complete!");
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            SetupQuadMesh();
        }

        public void Draw() {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Disable(EnableCap.DepthTest);
            shader.Use();
            GL.BindVertexArray(quadVAO);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }

        public void SetupQuadMesh() {
            GL.GenVertexArrays(1, out quadVAO);
            GL.GenBuffers(1, out quadVBO);
            GL.BindVertexArray(quadVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, quadVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, quadVertices.Length * sizeof(float), quadVertices, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
        }

        public void GenerateBuffer(FramebufferManagerMode mode) {
            GL.GenTextures(1, out texture);
            IntPtr a = new IntPtr();
            GL.BindTexture(TextureTarget.Texture2D, texture);
            switch(mode) {
                case FramebufferManagerMode.COLOR_BUFFER:
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, a);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                    GL.BindTexture(TextureTarget.Texture2D, 0);
                    GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, texture, 0);

                    uint rbo;

                    GL.GenRenderbuffers(1, out rbo);
                    GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rbo);
                    GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, width, height);
                    GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
                    GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, rbo);
                    break;
                case FramebufferManagerMode.SHADOW_BUFFER:
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, width, height, 0, PixelFormat.DepthComponent, PixelType.UnsignedByte, a);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)OpenTK.Graphics.OpenGL4.TextureWrapMode.Repeat);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)OpenTK.Graphics.OpenGL4.TextureWrapMode.Repeat);
                    GL.BindTexture(TextureTarget.Texture2D, 0);
                    GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, texture, 0);
                    GL.DrawBuffer(0);
                    GL.ReadBuffer(0);
                break;
            }
        }
    }

    enum FramebufferManagerMode {
        COLOR_BUFFER = 0,
        SHADOW_BUFFER
    }
}
