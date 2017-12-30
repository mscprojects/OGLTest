using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace OGLTest
{
    class MyWindow : GameWindow
    {
        private ShaderProgram _program;
        private Player _player;
        private World _world;

        private const int _width = 800;
        private const int _height = 600;

        public MyWindow()
            : base(_width, _height, GraphicsMode.Default, "openGL test", GameWindowFlags.Default,
                DisplayDevice.Default, 4, 5, GraphicsContextFlags.Debug)
        {
            var playerCamera = new Camera(_width, _height);
            _player = new Player(playerCamera);
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
            _player.Update(e.Time);
        }

        public override void Exit()
        {
            _program.Delete();
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

            _program = new ShaderProgram();
            _program.AddShader(ShaderType.VertexShader, File.ReadAllText("vertex.s"));
            _program.AddShader(ShaderType.FragmentShader, File.ReadAllText("fragment.s"));
            _program.LinkProgram();

            _world = new World();

            GL.PatchParameter(PatchParameterInt.PatchVertices, 3);

            _lightningCube = new DebugDrawCube(_lightSource, _lightSource + Vector3.One);
        }

        private Vector3 _lightSource = new Vector3(50, 100, 0);
        private Vector3 _lightColor = new Vector3(1, 1, 1);
        private DebugDrawCube _lightningCube;

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            string oglVersion = GL.GetString(StringName.Version);
            Title = $"FPS: {1 / e.Time:F0}, Version: {oglVersion}";
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _program.Use();
            
            _program.setMat4("view", _player.Camera.ViewMatrix);
            _program.setMat4("projection", _player.Camera.ProjectionMatrix);
            _program.setMat4("model", Matrix4.Identity);

            _program.setVec3("light_color", _lightColor);
            _program.setVec3("light_position", _lightSource);
            _program.setVec3("camera_position", _player.Position);

            GL.Enable(EnableCap.DepthTest);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            _world.Render(_program);

            GL.Disable(EnableCap.DepthTest);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            _lightningCube.Render();

            SwapBuffers();
        }
    }
}