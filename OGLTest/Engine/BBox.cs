using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace OGLTest.Engine
{
  public class BBox
  {

    public Vector3 Min { get; set; } = Vector3.Zero;
    public Vector3 Max { get; set; } = Vector3.One;

    public BBox()
    {
      
    }

    public BBox(Vector3 min, Vector3 max)
    {
      Min = min;
      Max = max;
    }

    public bool PointInside(Vector3 point)
    {
      return point.X > Min.X && point.X < Max.X &&
             point.Y > Min.Y && point.Y < Max.Y &&
             point.Z > Min.Z && point.Z < Max.Z;
    }

    public bool CollidesWith(BBox other)
    {
      return GetPoints().Any(other.PointInside);
    }

    public List<Vector3> GetPoints()
    {
      var pts = new List<Vector3>
      {
        new Vector3(Min.X, Min.Y, Min.Z),

        new Vector3(Max.X, Min.Y, Min.Z),
        new Vector3(Min.X, Max.Y, Min.Z),
        new Vector3(Min.X, Min.Y, Max.Z),

        new Vector3(Max.X, Min.Y, Max.Z),
        new Vector3(Max.X, Max.Y, Max.Z)
      };

      return pts;
    }

  }
}