using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshBufferRender.Base
{
    public static class Extensions
    {
        public static double DotProduct(this Vector3 a, Vector3 b)
        {
            return Vector3.DotProduct(a, b);
        }

        public static Vector3 CrossProduct(this Vector3 a, Vector3 b)
        {
            return Vector3.CrossProduct(a, b);
        }

        public static double Angle(this Vector3 a, Vector3 b)
        {
            return Vector3.Angle(a, b);
        }

    }
}
