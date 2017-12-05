using OpenTK;
using OpenTK.Graphics;

namespace OGLTest
{
    internal interface IBlock
    {
        bool render();
        string texture();
        Color4 color();
    }
}