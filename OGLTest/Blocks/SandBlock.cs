namespace OGLTest
{
  class SandBlock : IBlock
  {
    public bool render()
    {
      return true;
    }

    public string texture()
    {
      return "sand.png";
    }
  }
}