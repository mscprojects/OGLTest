using System.Collections.Generic;
using System.Linq;
using OGLTest.Engine;
using OpenTK;

namespace OGLTest
{
  public class CollisionController
  {
    private readonly World _world;

    public CollisionController(World world)
    {
      _world = world;
    }

    public bool CollidesWithWorld(BBox bbox)
    {
      var bboxPts = bbox.GetPoints();
      var bboxBlocks = new List<IBlock>(bboxPts.Select(pt => _world.BlockAtPosition(pt)));

      return bboxBlocks.Any(b => b.render());
    }

    public bool CollidesWithWorld(Ray ray, float maxDistance, out Vector3 hit)
    {
      var pointsOnRay = ray.GetPointsOnRay(0.1f, maxDistance);

      foreach (var point in pointsOnRay)
      {
        var blockAtPosition = _world.BlockAtPosition(point);
        if (blockAtPosition.render())
        {
          hit = point;
          return true;
        }
      }
      
      hit = Vector3.Zero;
      return false;
    }
  }
}