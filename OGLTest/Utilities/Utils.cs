using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OGLTest.Utilities
{
  // TODO: Find a better name for this class
  class Utils
  {
    public static float Floor(float f)
    {
      return (float)Math.Floor(f);
    }

    public static float Sin(float degrees)
    {
      return (float)Math.Sin(MathHelper.DegreesToRadians(degrees));
    }

    public static float Cos(float degrees)
    {
      return (float)Math.Cos(MathHelper.DegreesToRadians(degrees));
    }

    public static Vector3 TruncateComponents(Vector3 vec)
    {
      return new Vector3((int)vec.X, (int)vec.Y, (int)vec.Z);
    }

    public static Vector3 GetMiddlePoint(IEnumerable<Vector3> points)
    {
      Vector3 sum = Vector3.Zero;
      int num = 0;
      foreach (var pt in points)
      {
        sum += pt;
        num++;
      }
      return sum / num;
    }
  }
}
