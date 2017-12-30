using System;

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
  }
}