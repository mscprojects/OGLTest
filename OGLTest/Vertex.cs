using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics;

namespace OGLTest
{
    internal struct Vertex
    {
        public const int Size = 4 * 4 
            + 4 * 4 
            + 2 * 4;

        public Vector4 _position;
        public Color4 _color;
        public Vector2 _textureCoordinates;

        public Vertex(Vector4 position, Color4 color, Vector2 textureCoordinates)
        {
            _position = position;
            _color = color;
            _textureCoordinates = textureCoordinates;
        }
    }
}