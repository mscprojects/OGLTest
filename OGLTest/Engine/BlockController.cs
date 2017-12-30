using OpenTK;

namespace OGLTest
{
  public class BlockController
  {
    private readonly World _world;

    public BlockController(World world)
    {
      _world = world;
    }

    public void DestroyBlock(Vector3 pos)
    {
      _world.DestroyBlock(pos);
    }
  }
}