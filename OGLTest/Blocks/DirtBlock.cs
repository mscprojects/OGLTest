using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace OGLTest
{
  class DirtBlock : IBlock
    {
        public bool render()
        {
            return true;
        }

        public string texture()
        {
            return "hardened_clay_stained_green.png";
        }
    }
}
