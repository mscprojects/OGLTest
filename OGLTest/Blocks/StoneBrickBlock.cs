using OpenTK.Graphics;

namespace OGLTest
{
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
      return "hardened_clay_stained_cyan.png";
    }

    public Color4 color()
    {
      return _color;
    }
  }
}