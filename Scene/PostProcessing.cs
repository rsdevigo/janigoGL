using OpenTK.Graphics.OpenGL4;
using System;
using janigoGL;
namespace Scene {
    class PostProcessing {
        private Scene scene;
        private FramebufferManager fmanager;
        public PostProcessing(int width, int height, string vertexShaderPath, string fragShaderPath) {
            fmanager = new FramebufferManager(width, height, vertexShaderPath, fragShaderPath);
        }

        public void preparePostProcessing() {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fmanager.frameBuffer);
        }

        public void executePostProcessing() {
            fmanager.Draw();
        }
    }
}