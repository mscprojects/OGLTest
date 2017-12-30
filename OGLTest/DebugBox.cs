using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace OGLTest
{
    public class DebugDrawCube
    {
        private Vector3 _min, _max;
        private RenderObject _renderObject;

        public Color4 Color { get; set; } = Color4.White;

        public DebugDrawCube(Vector3 point1, Vector3 point2)
        {
            if (point1.Length > point2.Length)
            {
                var tmp = point1;
                point1 = point2;
                point2 = tmp;
            }

            _max = point2;
            _min = point1;
            CreateMesh();
        }

        private void CreateMesh()
        {
            var scale = new Vector3
            {
                X = _max.X - _min.X,
                Y = _max.Y - _min.Y,
                Z = _max.Z - _min.Z
            };

            var cubeMesh = new CubeMesh
            {
                Position = _min,
                Scale = scale,
                Color = Color
            };

            var cubeVertices = cubeMesh.CreateMesh();
            _renderObject = new RenderObject(cubeVertices.ToArray());
        }


        public void Render()
        {
            var oldPolygonMode = GL.GetInteger(GetPName.PolygonMode);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            var whiteTexture = Texture.White();
            whiteTexture.Bind(TextureUnit.Texture0);

            _renderObject.Render();
            GL.PolygonMode(MaterialFace.FrontAndBack, (PolygonMode) oldPolygonMode);
        }
    }
}