using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics;

namespace OGLTest
{
    internal struct Vertex
    {
        public const int Size = 3 * 4 
            + 3 * 4 
            + 4 * 4
            + 2 * 4;

        public Vector3 _position;
        public Vector3 _normal;

        public Color4 _color;
        public Vector2 _textureCoordinates;

        public Vertex(Vector3 position, Vector3 normal, Color4 color, Vector2 textureCoordinates)
        {
            _position = position;
            _normal = normal;
            _color = color;
            _textureCoordinates = textureCoordinates;
        }
    }
}