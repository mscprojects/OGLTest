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
        private Camera _camera;
        private World _world;

        private const int _width = 800;
        private const int _height = 600;

        public MyWindow()
            : base(_width, _height, GraphicsMode.Default, "openGL test", GameWindowFlags.Default,
                DisplayDevice.Default, 4, 5, GraphicsContextFlags.Debug)
        {
            _camera = new Camera(_width, _height);
        }

        private static void MessageHandler(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
        {
            var debugMessage = Marshal.PtrToStringAnsi(message, length);
            Debug.WriteLine(debugMessage);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            HandleInput();
        }

        private void HandleInput()
        {
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();
            if (keyState.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            _camera.HandleKeyboard(keyState);
            _camera.HandleMouse(mouseState);
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
            GL.Enable(EnableCap.DepthTest);

            _program = new ShaderProgram();
            _program.AddShader(ShaderType.VertexShader, File.ReadAllText("vertex.s"));
            _program.AddShader(ShaderType.FragmentShader, File.ReadAllText("fragment.s"));
            _program.LinkProgram();

            _world = new World();
            
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            string oglVersion = GL.GetString(StringName.Version);
            Title = $"FPS: {1 / e.Time:F0}, Version: {oglVersion}";
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _program.Use();

            _program.SetInteger("texture0", 0);

            _program.setMat4("view", _camera.ViewMatrix);
            _program.setMat4("projection", _camera.ProjectionMatrix);

            _world.Render(_program);

            SwapBuffers();
        }
    }
}