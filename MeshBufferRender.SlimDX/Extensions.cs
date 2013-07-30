using MeshBufferRender.Base;
using SlimDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDx = SlimDX;

namespace MeshBufferRender.SlimDX
{
    public static class Extensions
    {
        public static MeshBufferRender.Base.Vector3 ToVector3(this SlimDx.Vector3 vec)
        {
            return new MeshBufferRender.Base.Vector3(vec.X, vec.Y, vec.Z);
        }

        public static SlimDx.Vector3 ToSlimDXVector(this MeshBufferRender.Base.Vector3 vec)
        {
            return new SlimDx.Vector3((float)vec.X, (float)vec.Y, (float)vec.Z);
        }

        public static void Assert(this Result result)
        {
            if (!result.IsSuccess)
                throw new InvalidOperationException();
        }

        public static Matrix4x4 ToMatrix4x4(this Matrix matrix)
        {
            return new Matrix4x4()
            {
                M11 = matrix.M11,
                M12 = matrix.M12,
                M13 = matrix.M13,
                M14 = matrix.M14,
                M21 = matrix.M21,
                M22 = matrix.M22,
                M23 = matrix.M23,
                M24 = matrix.M24,
                M31 = matrix.M31,
                M32 = matrix.M32,
                M33 = matrix.M33,
                M34 = matrix.M34,
                M41 = matrix.M41,
                M42 = matrix.M42,
                M43 = matrix.M43,
                M44 = matrix.M44
            };
        }

        public static Matrix ToSlimDXMatrix(this Matrix4x4 matrix)
        {
            return new Matrix()
            {
                M11 = (float)matrix.M11,
                M12 = (float)matrix.M12,
                M13 = (float)matrix.M13,
                M14 = (float)matrix.M14,
                M21 = (float)matrix.M21,
                M22 = (float)matrix.M22,
                M23 = (float)matrix.M23,
                M24 = (float)matrix.M24,
                M31 = (float)matrix.M31,
                M32 = (float)matrix.M32,
                M33 = (float)matrix.M33,
                M34 = (float)matrix.M34,
                M41 = (float)matrix.M41,
                M42 = (float)matrix.M42,
                M43 = (float)matrix.M43,
                M44 = (float)matrix.M44
            };
        }

    }
}
