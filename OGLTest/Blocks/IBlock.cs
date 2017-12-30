using OpenTK;
using OpenTK.Graphics;

namespace OGLTest
{
  public interface IBlock
  {
    bool render();
    string texture();
  }
}