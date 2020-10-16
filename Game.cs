using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL4;
using System;
using Scene;

namespace janigoGL
{
  class Game : GameWindow
  {
    private double _time;
    public Scene.Scene scene;
    public Matrix4 modelMatrix = Matrix4.CreateScale(0.5f) * Matrix4.CreateRotationX(45) * Matrix4.CreateRotationY(45) * Matrix4.CreateTranslation(Vector3.Zero);
    public Game(int width, int height, string title) : base(width, height, GraphicsMode.Default, title, GameWindowFlags.Default, DisplayDevice.Default, 4, 2, GraphicsContextFlags.ForwardCompatible) { }
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      KeyboardState input = Keyboard.GetState();

      if (input.IsKeyDown(Key.Escape))
      {
        Exit();
      }
      base.OnUpdateFrame(e);
    }

    protected override void OnLoad(EventArgs e)
    {
      scene = new Scene.Scene("scenes/scene.json");
      GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
      GL.Enable(EnableCap.DepthTest);
      GL.DepthFunc(DepthFunction.Less);
      GL.DepthMask(true);
      GL.DepthRange(scene._camera._near, scene._camera._far);
      base.OnLoad(e);
    }

    protected override void OnUnload(EventArgs e)
    {
      scene.UnLoad();
      base.OnUnload(e);
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      _time += 10 * e.Time;
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
      scene.Draw();
      Context.SwapBuffers();
      base.OnRenderFrame(e);
    }

    protected override void OnResize(EventArgs e)
    {
      GL.Viewport(0, 0, Width, Height);
      scene._camera.Resize(Width, Height);
      base.OnResize(e);
    }
  }

}