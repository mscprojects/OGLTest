using System;
using OpenTK.Graphics.OpenGL4;

namespace OGLTest
{
    internal class RenderObject : IDisposable
    {
        private bool _initialized;
        private int _verticesCount;
        private int _vertexArray;
        private int _buffer;

        public RenderObject(Vertex[] vertices)
        {
            _verticesCount = vertices.Length;

            _buffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _buffer);
            GL.BufferData(BufferTarget.ArrayBuffer, _verticesCount * Vertex.Size, vertices, BufferUsageHint.StaticDraw);

            _vertexArray = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArray);
            // Tell the shaders how the data is layed out
            // 1. Position
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, Vertex.Size, 0);
            GL.EnableVertexAttribArray(0);
            // 2. Color
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, Vertex.Size, 16);
            GL.EnableVertexAttribArray(1);
            // 3. Texture Coordinations
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Vertex.Size, 32);
            GL.EnableVertexAttribArray(2);

            _initialized = true;
        }

        public void Render()
        {
            GL.BindVertexArray(_vertexArray);
            GL.DrawArrays(PrimitiveType.Triangles, 0, _verticesCount);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_initialized)
                {
                    GL.DeleteVertexArray(_vertexArray);
                    GL.DeleteBuffer(_buffer);
                    _initialized = false;
                }
            }
        }
    }
}