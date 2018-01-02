using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using OGLTest.DebugUtilities;
using OGLTest.Renderer;
using OGLTest.Utilities;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;


// TODO:
// 1. Get a messaging system across the whole system for lower coupling
// 2. Give each block a coordinate (int!)
// 3. Proper disposal of RenderObjects

namespace OGLTest
{
  class MyWindow : GameWindow
  {
    private CollisionController _collisionController;
    private BlockController _blockController;

    private ShaderProgram _program;
    private ShaderProgram _skyboxProgram;
    private ShaderProgram _debugProgram;

    private Player _player;
    private World _world;
    private Skybox _skybox;

    private const int _width = 800;
    private const int _height = 600;

    public MyWindow()
        : base(_width, _height, GraphicsMode.Default, "openGL test", GameWindowFlags.Default,
            DisplayDevice.Default, 4, 5, GraphicsContextFlags.Debug)
    {
    }

    private static void MessageHandler(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
    {
      var debugMessage = Marshal.PtrToStringAnsi(message, length);
      Debug.WriteLine(debugMessage);
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);

      var keyState = Keyboard.GetState();
      var mouseState = Mouse.GetState();
      if (keyState.IsKeyDown(Key.Escape))
      {
        Exit();
      }

      _player.HandleKeyboard(keyState);
      _player.HandleMouse(mouseState);
      _player.Update((float) e.Time);

      _world.Update((float) e.Time);

      if (Focused)
      {
        var centerPoint = new Point(_width / 2, _height / 2);
        centerPoint = PointToScreen(centerPoint);
        OpenTK.Input.Mouse.SetPosition(centerPoint.X, centerPoint.Y);
      }

      DebugDrawManager.Update();
    }

    public override void Exit()
    {
      _program.Delete();
      _debugProgram.Delete();
      _skyboxProgram.Delete();

      base.Exit();
    }

    protected override void OnClosed(EventArgs e)
    {
      base.OnClosed(e);
      Exit();
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      GL.Enable(EnableCap.DebugOutput);
      GL.Enable(EnableCap.DebugOutputSynchronous);
      int[] arr = { };
      GL.DebugMessageControl(DebugSourceControl.DontCare, DebugTypeControl.DontCare, DebugSeverityControl.DontCare, 0, arr, true);
      GL.DebugMessageCallback(MessageHandler, IntPtr.Zero);

      GL.ClearColor(Color4.Black);

      GameState.Initialize();

      _program = new ShaderProgram();
      _program.AddShader(ShaderType.VertexShader, ReadShader("vertex.s"));
      _program.AddShader(ShaderType.FragmentShader, ReadShader("fragment.s"));
      _program.LinkProgram();

      _skyboxProgram = new ShaderProgram();
      _skyboxProgram.AddShader(ShaderType.VertexShader, ReadShader("skybox_vertex.s"));
      _skyboxProgram.AddShader(ShaderType.FragmentShader, ReadShader("skybox_fragment.s"));
      _skyboxProgram.LinkProgram();

      _debugProgram = new ShaderProgram();
      _debugProgram.AddShader(ShaderType.VertexShader, ReadShader("debug_vertex.s"));
      _debugProgram.AddShader(ShaderType.FragmentShader, ReadShader("debug_fragment.s"));
      _debugProgram.LinkProgram();

      _world = new World();
      _collisionController = new CollisionController(_world);
      _blockController = new BlockController(_world);

      var playerCamera = new Camera(_width, _height);
      _player = new Player(playerCamera, _collisionController, _blockController);
      _skybox = new Skybox();

      GL.PatchParameter(PatchParameterInt.PatchVertices, 3);

    }

    private static string ReadShader(string shaderName)
    {
      return File.ReadAllText(@"..\..\Shaders\" + shaderName);
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);

      string oglVersion = GL.GetString(StringName.Version);
      Title = $"FPS: {1 / e.Time:F0}, Version: {oglVersion}";
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      _skyboxProgram.Use();
      _skyboxProgram.setMat4("view", _player.Camera.ViewMatrix.ClearTranslation());
      _skyboxProgram.setMat4("projection", _player.Camera.ProjectionMatrix);

      GL.DepthMask(false);
      GL.Disable(EnableCap.DepthTest);
      GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
      _skybox.Render();

      _program.Use();
      _program.setMat4("view", _player.Camera.ViewMatrix);
      _program.setMat4("projection", _player.Camera.ProjectionMatrix);
      _program.setMat4("model", Matrix4.Identity);

      _program.setVec3("light_color", Vector3.One);
      _program.setVec3("light_position", new Vector3(0, 10, 50));
      _program.setVec3("camera_position", _player.Position);

      GL.DepthMask(true);
      GL.Enable(EnableCap.DepthTest);
      GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
      _world.Render();

      _debugProgram.Use();
      _debugProgram.setMat4("view", _player.Camera.ViewMatrix);
      _debugProgram.setMat4("projection", _player.Camera.ProjectionMatrix);
      _debugProgram.setMat4("model", Matrix4.Identity);

      GL.Disable(EnableCap.DepthTest);
      GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

      DebugDrawManager.Render();

      SwapBuffers();
    }
  }
}