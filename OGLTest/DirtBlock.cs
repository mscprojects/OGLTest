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
    class AirBlock : IBlock
    {
        public bool render()
        {
            return false;
        }

        public string texture()
        {
            throw new NotImplementedException();
        }

        public Color4 color()
        {
            throw new NotImplementedException();
        }
    }

    class SandBlock : IBlock
    {
        private Color4 _color;

        public SandBlock()
        {
            _color = new Color4(255, 255, 0, 255);
        }

        public bool render()
        {
            return true;
        }

        public string texture()
        {
            return "sand.png";
        }

        public Color4 color()
        {
            return _color;
        }
    }

    class DirtBlock : IBlock
    {
        private Color4 _color;

        public DirtBlock()
        {
            _color = new Color4(0, 0, 255, 255);
        }

        public bool render()
        {
            return true;
        }

        public string texture()
        {
            return "dirt.png";
        }

        public Color4 color()
        {
            return _color;
        }
    }

    class StoneBrickBlock : IBlock
    {
        private Color4 _color;

        public StoneBrickBlock()
        {
            _color = new Color4(0, 0, 255, 255);
        }

        public bool render()
        {
            return true;
        }

        public string texture()
        {
            return "stonebrick.png";
        }

        public Color4 color()
        {
            return _color;
        }
    }
}
