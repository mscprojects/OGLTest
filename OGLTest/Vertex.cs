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

        private readonly Vector4 _position;
        private readonly Color4 _color;
        private readonly Vector2 _textureCoordinations;

        public Vertex(Vector4 position, Color4 color, Vector2 textureCoordinations)
        {
            _position = position;
            _color = color;
            _textureCoordinations = textureCoordinations;
        }
    }
}