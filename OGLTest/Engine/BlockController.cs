using OGLTest.Engine;
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

    public void DestroyBlock(WorldPosition pos)
    {
      _world.DestroyBlock(pos);
    }
  }
}