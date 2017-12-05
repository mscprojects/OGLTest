using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace OGLTest
{
    internal class ShaderProgram
    {
        private readonly List<int> _usedShaders = new List<int>();
        private readonly int _program;

        public ShaderProgram()
        {
            _program = GL.CreateProgram();
        }

        public void AddShader(ShaderType type, string source)
        {
            var shader = GL.CreateShader(type);
            _usedShaders.Add(shader);

            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);

            var info = GL.GetShaderInfoLog(shader);
            Debug.WriteLine(info);

            GL.AttachShader(_program, shader);
        }

        public void LinkProgram()
        {
            GL.LinkProgram(_program);

            foreach (var usedShader in _usedShaders)
            {
                GL.DetachShader(_program, usedShader);
                GL.DeleteShader(usedShader);
            }
        }

        public void Use()
        {
            GL.UseProgram(_program);
        }

        public void Delete()
        {
            GL.DeleteProgram(_program);
        }

        public void SetInteger(string uniformName, int i)
        {
            var location = GL.GetUniformLocation(_program, uniformName);
            GL.Uniform1(location, i);
        }

        public void setMat4(int location, Matrix4 mat4)
        {
            GL.UniformMatrix4(location, false, ref mat4);
        }

        public void setMat4(string uniformName, Matrix4 mat4)
        {
            var location = GL.GetUniformLocation(_program, uniformName);
            GL.UniformMatrix4(location, false, ref mat4);
        }
    }
}