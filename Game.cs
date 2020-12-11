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
    // Cena a ser desenhada
    public Scene.Scene scene;

    // A última posição do mouse para o controle da camera
    public Vector2 lastPos;
    // Estados dos botões pressionados
    public bool mPressed = false;
    public bool pPressed = false;
    // Fim dos estados dos botões pressionados
    public bool firstMove = true;

    // Representa o pós processamento da cena
    public PostProcessing postProcessing;
    public bool executepostProcessing = false;

    // Representa o skybox que deverá ser desenhado
    public Skybox skybox;

    public Game(int width, int height, string title) : base(width, height, new GraphicsMode(new ColorFormat(32), 16, 0, 4), title, GameWindowFlags.Default, DisplayDevice.Default, 4, 2, GraphicsContextFlags.ForwardCompatible) { }

    //Chamado a cada frame antes de desenhar a cena.
    //Aqui é consultado as entradas dos usuário.
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      if (!Focused) {
        return;
      }

      KeyboardState input = Keyboard.GetState();

      if (input.IsKeyDown(Key.Escape))
      {
        Exit();
      }

      if (input.IsKeyDown(Key.W)) 
      {
        scene._camera.Forward(e.Time);
      }

      if (input.IsKeyDown(Key.S)) 
      {
        scene._camera.Backward(e.Time);
      }

      if (input.IsKeyDown(Key.A)) 
      {
        scene._camera.Left(e.Time);
      }

      if (input.IsKeyDown(Key.D)) 
      {
        scene._camera.Right(e.Time);
      }

      if (input.IsKeyDown(Key.Space)) 
      {
        scene._camera.Up(e.Time);
      }

      if (input.IsKeyDown(Key.LShift)) 
      {
        scene._camera.Down(e.Time);
      }

      if (input.IsKeyDown(Key.M) && !mPressed) 
      {
        mPressed = true;
        scene._camera.PrintCameraInfo();
      }

      if (input.IsKeyUp(Key.M) && mPressed) 
      {
        mPressed = false;
      }

      if (input.IsKeyDown(Key.L) && !mPressed) 
      {
        mPressed = true;
        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
      }

      if (input.IsKeyUp(Key.L) && mPressed) 
      {
        mPressed = false;
      }

      if (input.IsKeyDown(Key.F) && !mPressed) 
      {
        mPressed = true;
        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
      }

      if (input.IsKeyUp(Key.F) && mPressed) 
      {
        mPressed = false;
      }

      if (input.IsKeyDown(Key.P) && !pPressed) 
      {
        pPressed = true;
        executepostProcessing = !executepostProcessing;
      }

      if (input.IsKeyUp(Key.P) && pPressed) 
      {
        pPressed = false;
      }

      var mouse = Mouse.GetState();

      if (firstMove) {
        lastPos = new Vector2(mouse.X, mouse.Y);
        firstMove = false;
      } else {
        var deltaX = mouse.X - lastPos.X;
        var deltaY = mouse.Y - lastPos.Y;

        scene._camera.pitch -= deltaY * scene._camera._sensitivity;
        scene._camera.yaw += deltaX * scene._camera._sensitivity;
        lastPos = new Vector2(mouse.X, mouse.Y);
      }

      base.OnUpdateFrame(e);
    }

    //Chamado apenas uma vez quando inicializa a janela
    protected override void OnLoad(EventArgs e)
    {
      // Carrega o arquivo da cena e consequentemente os modelos 3D listados no arquivo json
      scene = new Scene.Scene("scenes/scene.json");

      GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
      GL.Enable(EnableCap.Multisample);
      GL.Enable(EnableCap.DepthTest);
      GL.DepthFunc(DepthFunction.Less);
      GL.DepthMask(true);
      GL.DepthRange(scene._camera._near, scene._camera._far);

      // Carrega o shader de pós processamento para aplicar na cena quando a tecla P ser pressionada
      postProcessing = new PostProcessing(Width, Height, "./examples_shader/quad_shader.vert", "./examples_shader/quad_shader.frag");

      CursorVisible = false;
      CursorGrabbed = true;
      
      // Carrega o shader do skybox com as imagens skybox_[direcao].jpg
      skybox = new Skybox("skybox");

      base.OnLoad(e);
    }

    protected override void OnUnload(EventArgs e)
    {
      scene.UnLoad();
      base.OnUnload(e);
    }

    // A cada frame é chamado depois do OnUpdateFrame, aqui é desenhado a cena.
    protected override void OnRenderFrame(FrameEventArgs e)
    {
      GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
      GL.Enable(EnableCap.DepthTest);
      GL.DepthFunc(DepthFunction.Less);
      GL.DepthMask(true);
      GL.DepthRange(scene._camera._near, scene._camera._far);

      // Executando o postprocessing caso a tecla P tenha sido apertada no último frame
      if (executepostProcessing)
        postProcessing.preparePostProcessing();
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
      // Desenhando a cena.
      scene.Draw();

      // Desenhando o skybox
      skybox.Draw(scene._camera);

      // Executando o postprocessing caso a tecla P tenha sido apertada no último frame
      if (executepostProcessing)
        postProcessing.executePostProcessing();
      
      Context.SwapBuffers();
      base.OnRenderFrame(e);
    }

    protected override void OnResize(EventArgs e)
    {
      GL.Viewport(0, 0, Width, Height);
      scene._camera.Resize(Width, Height);
      base.OnResize(e);
    }

    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
      if (Focused)
      {
        Mouse.SetPosition(X + Width / 2f, Y + Height / 2f);
      }

      base.OnMouseMove(e);
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
      scene._camera.fov -= e.DeltaPrecise;
      base.OnMouseWheel(e);
    }
  }

}