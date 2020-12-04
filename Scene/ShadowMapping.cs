using janigoGL;
using OpenTK.Graphics.OpenGL4;
using OpenTK;
using System;


namespace Scene {
    class ShadowMapping {
        FramebufferManager fmanager;
        Shader shader;

        public ShadowMapping(int w, int h) {
            fmanager = new FramebufferManager(w, h, FramebufferManagerMode.SHADOW_BUFFER);
            shader = new Shader("./shadow_shader.vert", "./shadow_shader.frag");
        }

        public void CreateShadowMap(Camera _camera) {
            shader.Use();
            shader.SetMatrix4(Matrix4.CreateOrthographicOffCenter(-10.0f, 10.0f, -10.0f, 10.0f, 1.0f, 7.5f), "projectionMatrix");
        }
    }
}