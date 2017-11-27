using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace OGLTest
{
    class SandBlock : IBlock
    {
        private Vector3 _position;

        public SandBlock(int x, int y, int z)
        {
            _position = new Vector3(x, y, z);
        }
        public string texture()
        {
            return @"textures\blocks\sand.png";
        }

        public Vector3 position()
        {
            return _position;
        }
    }

    class DirtBlock : IBlock
    {
        private Vector3 _position;

        public DirtBlock(int x, int y, int z)
        {
            _position = new Vector3(x, y, z);
        }

        public string texture()
        {
            return @"textures\blocks\dirt.png";
        }

        public Vector3 position()
        {
            return _position;
        }
    }
}
