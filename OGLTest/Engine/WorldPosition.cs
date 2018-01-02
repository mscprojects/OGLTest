using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OGLTest.Engine
{
  public struct WorldPosition
  {
    public int X;
    public int Y;
    public int Z;

    public WorldPosition(Vector3 pos)
    {
      X = (int) pos.X;
      Y = (int) pos.Y;
      Z = (int) pos.Z;
    }

    public WorldPosition(int x, int y, int z)
    {
      X = x;
      Y = y;
      Z = z;
    }

    public static WorldPosition operator -(WorldPosition a, WorldPosition b)
    {
      return new WorldPosition
      {
        X = a.X - b.X,
        Y = a.Y - b.Y,
        Z = a.Z - b.Z
      };
    }

    public static WorldPosition operator +(WorldPosition a, WorldPosition b)
    {
      return new WorldPosition
      {
        X = a.X + b.X,
        Y = a.Y + b.Y,
        Z = a.Z + b.Z
      };
    }

    public Vector3 AsVector3()
    {
      return new Vector3(X, Y, Z);
    }

    public BBox BBox(Vector3 size)
    {
      var pos = AsVector3();
      return new BBox(pos, pos + size);
    }

  }
}
