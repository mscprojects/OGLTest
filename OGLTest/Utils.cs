using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OGLTest
{
    class Utils
    {
        public static float Sin(float degrees)
        {
            return (float) Math.Sin(MathHelper.DegreesToRadians(degrees));
        }

        public static float Cos(float degrees)
        {
            return (float) Math.Cos(MathHelper.DegreesToRadians(degrees));
        }
    }
}
