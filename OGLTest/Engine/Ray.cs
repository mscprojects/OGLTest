using System.Collections.Generic;
using OpenTK;

namespace OGLTest
{
  public class Ray
  {
    public Vector3 Start { get; }
    public Vector3 Direction { get; }

    public Ray(Vector3 start, Vector3 direction)
    {
      Start = start;
      Direction = direction.Normalized();
    }

    public List<Vector3> GetPointsOnRay(float distanceBetween, float maxDistance)
    {
      var points = new List<Vector3>();

      var currentDistance = 0.0f;
      while (currentDistance < maxDistance)
      {
        points.Add(new Vector3(Start + Direction * currentDistance));
        currentDistance += distanceBetween;
      }

      return points;
    }
  }
}