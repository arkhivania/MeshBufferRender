using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshBufferRender.Base
{
    public class Matrix4x4
    {
        public double M11;
        public double M12;
        public double M13;
        public double M14;

        public double M21;
        public double M22;
        public double M23;
        public double M24;

        public double M31;
        public double M32;
        public double M33;
        public double M34;

        public double M41;
        public double M42;
        public double M43;
        public double M44;

        public Matrix4x4()
            : this(0)
        {

        }

        public Matrix4x4(double i)
        {
            M11 = M22 = M33 = i;
            M44 = 1;
            M12 = M13 = M14 = M21 = M23 = M24 = M31 = M32 = M34 = M41 = M42 = M43 = 0;
        }

        public Matrix4x4(double m11, double m12, double m13, double m14,
                         double m21, double m22, double m23, double m24,
                         double m31, double m32, double m33, double m34,
                         double m41, double m42, double m43, double m44)
        {
            M11 = m11; M12 = m12; M13 = m13; M14 = m14;
            M21 = m21; M22 = m22; M23 = m23; M24 = m24;
            M31 = m31; M32 = m32; M33 = m33; M34 = m34;
            M41 = m41; M42 = m42; M43 = m43; M44 = m44;
        }

        public Matrix4x4(double m11, double m12, double m13,
                         double m21, double m22, double m23,
                         double m31, double m32, double m33,
                         double m41, double m42, double m43)
        {
            M11 = m11; M12 = m12; M13 = m13; M14 = 0;
            M21 = m21; M22 = m22; M23 = m23; M24 = 0;
            M31 = m31; M32 = m32; M33 = m33; M34 = 0;
            M41 = m41; M42 = m42; M43 = m43; M44 = 1;
        }

        private double Det3x3(double m11, double m12, double m13, double m21, double m22, double m23, double m31, double m32, double m33)
        {
            return m11 * (m22 * m33 - m23 * m32) - m12 * (m21 * m33 - m31 * m23) + m13 * (m21 * m32 - m31 * m22);
        }

        public double Determinant
        {
            get
            {
                return M11 * Det3x3(M22, M23, M24, M32, M33, M34, M42, M43, M44) -
                       M12 * Det3x3(M21, M23, M24, M31, M33, M34, M41, M43, M44) +
                       M13 * Det3x3(M21, M22, M24, M31, M32, M34, M41, M42, M44) -
                       M14 * Det3x3(M21, M22, M23, M31, M32, M33, M41, M42, M43);
            }
        }

        public double[] ToDouble16()
        {
            return new double[] { M11, M12, M13, M14, 
                                  M21, M22, M23, M24, 
                                  M31, M32, M33, M34, 
                                  M41, M42, M43, M44 };
        }

        public float[] ToFloat16()
        {
            return new float[] { (float)M11, (float)M12, (float)M13, (float)M14, 
                                 (float)M21, (float)M22, (float)M23, (float)M24, 
                                 (float)M31, (float)M32, (float)M33, (float)M34, 
                                 (float)M41, (float)M42, (float)M43, (float)M44 };
        }

        public Matrix4x4 Transpose()
        {
            var res = new Matrix4x4(0);

            res.M11 = M11;
            res.M12 = M21;
            res.M13 = M31;
            res.M14 = M41;

            res.M21 = M12;
            res.M22 = M22;
            res.M23 = M32;
            res.M24 = M42;

            res.M31 = M13;
            res.M32 = M23;
            res.M33 = M33;
            res.M34 = M43;

            res.M41 = M14;
            res.M42 = M24;
            res.M43 = M34;
            res.M44 = M44;

            return res;
        }

        public Matrix4x4 Invert()
        {
            var d = Determinant;
            if (d == 0) return new Matrix4x4(double.NaN, double.NaN, double.NaN, double.NaN,
                                             double.NaN, double.NaN, double.NaN, double.NaN,
                                             double.NaN, double.NaN, double.NaN, double.NaN,
                                             double.NaN, double.NaN, double.NaN, double.NaN);
            var res = new Matrix4x4(0);

            res.M11 = Det3x3(M22, M23, M24, M32, M33, M34, M42, M43, M44) / d;
            res.M21 = -Det3x3(M21, M23, M24, M31, M33, M34, M41, M43, M44) / d;
            res.M31 = Det3x3(M21, M22, M24, M31, M32, M34, M41, M42, M44) / d;
            res.M41 = -Det3x3(M21, M22, M23, M31, M32, M33, M41, M42, M43) / d;

            res.M12 = -Det3x3(M12, M13, M14, M32, M33, M34, M42, M43, M44) / d;
            res.M22 = Det3x3(M11, M13, M14, M31, M33, M34, M41, M43, M44) / d;
            res.M32 = -Det3x3(M11, M12, M14, M31, M32, M34, M41, M42, M44) / d;
            res.M42 = Det3x3(M11, M12, M13, M31, M32, M33, M41, M42, M43) / d;

            res.M13 = Det3x3(M12, M13, M14, M22, M23, M24, M42, M43, M44) / d;
            res.M23 = -Det3x3(M11, M13, M14, M21, M23, M24, M41, M43, M44) / d;
            res.M33 = Det3x3(M11, M12, M14, M21, M22, M24, M41, M42, M44) / d;
            res.M43 = -Det3x3(M11, M12, M13, M21, M22, M23, M41, M42, M43) / d;

            res.M14 = -Det3x3(M12, M13, M14, M22, M23, M24, M32, M33, M34) / d;
            res.M24 = Det3x3(M11, M13, M14, M21, M23, M24, M31, M33, M34) / d;
            res.M34 = -Det3x3(M11, M12, M14, M21, M22, M24, M31, M32, M34) / d;
            res.M44 = Det3x3(M11, M12, M13, M21, M22, M23, M31, M32, M33) / d;

            return res;
        }

        public static Matrix4x4 operator *(Matrix4x4 a, Matrix4x4 b)
        {
            return new Matrix4x4(a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31 + a.M14 * b.M41,
                                  a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32 + a.M14 * b.M42,
                                  a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33 + a.M14 * b.M43,
                                  a.M11 * b.M14 + a.M12 * b.M24 + a.M13 * b.M34 + a.M14 * b.M44,

                                  a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31 + a.M24 * b.M41,
                                  a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32 + a.M24 * b.M42,
                                  a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33 + a.M24 * b.M43,
                                  a.M21 * b.M14 + a.M22 * b.M24 + a.M23 * b.M34 + a.M24 * b.M44,

                                  a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31 + a.M34 * b.M41,
                                  a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32 + a.M34 * b.M42,
                                  a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33 + a.M34 * b.M43,
                                  a.M31 * b.M14 + a.M32 * b.M24 + a.M33 * b.M34 + a.M34 * b.M44,

                                  a.M41 * b.M11 + a.M42 * b.M21 + a.M43 * b.M31 + a.M44 * b.M41,
                                  a.M41 * b.M12 + a.M42 * b.M22 + a.M43 * b.M32 + a.M44 * b.M42,
                                  a.M41 * b.M13 + a.M42 * b.M23 + a.M43 * b.M33 + a.M44 * b.M43,
                                  a.M41 * b.M14 + a.M42 * b.M24 + a.M43 * b.M34 + a.M44 * b.M44);
        }

        public static Matrix4x4 operator +(Matrix4x4 a, Matrix4x4 b)
        {
            return new Matrix4x4(a.M11 + b.M11, a.M12 + b.M12, a.M13 + b.M13, a.M14 + b.M14,
                                 a.M21 + b.M21, a.M22 + b.M22, a.M23 + b.M23, a.M24 + b.M24,
                                 a.M31 + b.M31, a.M32 + b.M32, a.M33 + b.M33, a.M34 + b.M34,
                                 a.M41 + b.M41, a.M42 + b.M42, a.M43 + b.M43, a.M44 + b.M44);
        }

        public static Matrix4x4 operator -(Matrix4x4 a, Matrix4x4 b)
        {
            return new Matrix4x4(a.M11 - b.M11, a.M12 - b.M12, a.M13 - b.M13, a.M14 - b.M14,
                                 a.M21 - b.M21, a.M22 - b.M22, a.M23 - b.M23, a.M24 - b.M24,
                                 a.M31 - b.M31, a.M32 - b.M32, a.M33 - b.M33, a.M34 - b.M34,
                                 a.M41 - b.M41, a.M42 - b.M42, a.M43 - b.M43, a.M44 - b.M44);
        }

        public static Matrix4x4 operator *(Matrix4x4 a, double b)
        {
            return new Matrix4x4(a.M11 * b, a.M12 * b, a.M13 * b, a.M14 * b,
                                 a.M21 * b, a.M22 * b, a.M23 * b, a.M24 * b,
                                 a.M31 * b, a.M32 * b, a.M33 * b, a.M34 * b,
                                 a.M41 * b, a.M42 * b, a.M43 * b, a.M44 * b);
        }

        public static Matrix4x4 operator *(double a, Matrix4x4 b)
        {
            return new Matrix4x4(b.M11 * a, b.M12 * a, b.M13 * a, b.M14 * a,
                                 b.M21 * a, b.M22 * a, b.M23 * a, b.M24 * a,
                                 b.M31 * a, b.M32 * a, b.M33 * a, b.M34 * a,
                                 b.M41 * a, b.M42 * a, b.M43 * a, b.M44 * a);
        }

        public void Add(Matrix4x4 a)
        {
            M11 += a.M11; M12 += a.M12; M13 += a.M13; M14 += a.M14;
            M21 += a.M21; M22 += a.M22; M23 += a.M23; M24 += a.M24;
            M31 += a.M31; M32 += a.M32; M33 += a.M33; M34 += a.M34;
            M41 += a.M41; M42 += a.M42; M43 += a.M43; M44 += a.M44;
        }

        public void Subtract(Matrix4x4 a)
        {
            M11 -= a.M11; M12 -= a.M12; M13 -= a.M13; M14 -= a.M14;
            M21 -= a.M21; M22 -= a.M22; M23 -= a.M23; M24 -= a.M24;
            M31 -= a.M31; M32 -= a.M32; M33 -= a.M33; M34 -= a.M34;
            M41 -= a.M41; M42 -= a.M42; M43 -= a.M43; M44 -= a.M44;
        }

        public Matrix4x4 Clone()
        {
            return new Matrix4x4(M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44);
        }


        public void TransformPoints(Vector3[] points)
        {
            for (int i = 0; i < points.Length; i++)
                points[i] = TransformPoint(points[i]);
        }

        public Vector3[] TransformedPoints(Vector3[] points)
        {
            var res = new Vector3[points.Length];
            for (int i = 0; i < points.Length; i++)
                res[i] = TransformPoint(points[i]);
            return res;
        }

        public Vector3 TransformPoint(Vector3 p)
        {
            return p * this;
        }


        public Vector3[] TransformedVectors(Vector3[] points)
        {
            var res = new Vector3[points.Length];
            for (int i = 0; i < points.Length; i++)
                res[i] = TransformVector(points[i]);
            return res;
        }

        public void TransformVectors(Vector3[] points)
        {
            for (int i = 0; i < points.Length; i++)
                points[i] = TransformVector(points[i]);
        }

        public Vector3 TransformVector(Vector3 p)
        {
            return new Vector3(p.X * M11 + p.Y * M21 + p.Z * M31,
                               p.X * M12 + p.Y * M22 + p.Z * M32,
                               p.X * M13 + p.Y * M23 + p.Z * M33);
        }


        public static Matrix4x4 Rotate(double rotate_x, double rotate_y, double rotate_z)
        {
            Matrix4x4 rX = new Matrix4x4(1, 0, 0,
                                         0, System.Math.Cos(rotate_x), -System.Math.Sin(rotate_x),
                                         0, System.Math.Sin(rotate_x), System.Math.Cos(rotate_x),
                                         0, 0, 0);

            Matrix4x4 rY = new Matrix4x4(System.Math.Cos(rotate_y), 0, -System.Math.Sin(rotate_y),
                                         0, 1, 0,
                                         System.Math.Sin(rotate_y), 0, System.Math.Cos(rotate_y),
                                         0, 0, 0);

            Matrix4x4 rZ = new Matrix4x4(System.Math.Cos(rotate_z), -System.Math.Sin(rotate_z), 0,
                                         System.Math.Sin(rotate_z), System.Math.Cos(rotate_z), 0,
                                         0, 0, 1,
                                         0, 0, 0);

            return rX * rY * rZ;
        }

        public static Matrix4x4 Translate(double translate_x, double translate_y, double translate_z)
        {
            return new Matrix4x4(1, 0, 0, 0, 1, 0, 0, 0, 1, translate_x, translate_y, translate_z);
        }

        public static Matrix4x4 Translate(Vector3 d)
        {
            return new Matrix4x4(1, 0, 0, 0, 1, 0, 0, 0, 1, d.X, d.Y, d.Z);
        }

        public static Matrix4x4 Scale(double scale_x, double scale_y, double scale_z)
        {
            return new Matrix4x4(scale_x, 0, 0, 0, scale_y, 0, 0, 0, scale_z, 0, 0, 0);
        }

        public static Matrix4x4 Scale(double scale)
        {
            return new Matrix4x4(scale, 0, 0, 0, scale, 0, 0, 0, scale, 0, 0, 0);
        }

        public static Matrix4x4 RotateMatrixFromTwoVectors(Vector3 v_src, Vector3 v_dst)
        {
            var l1 = v_src.Length;
            var l2 = v_dst.Length;
            if (l1 == 0 || l2 == 0) return null;
            var v1 = v_src / l1;
            var v2 = v_dst / l2;
            var v3 = v1 + v2;
            if (v3.LengthSquare == 0) return new Matrix4x4(-1);
            v3 = v3.Normalize();

            var n = Vector3.CrossProduct(v3, v1);
            double W = Vector3.DotProduct(v1, v3);

            double xx = n.X * n.X;
            double xy = n.X * n.Y;
            double xz = n.X * n.Z;
            double xw = n.X * W;

            double yy = n.Y * n.Y;
            double yz = n.Y * n.Z;
            double yw = n.Y * W;

            double zz = n.Z * n.Z;
            double zw = n.Z * W;

            return new Matrix4x4(1 - 2 * (yy + zz), 2 * (xy - zw), 2 * (xz + yw),
                                 2 * (xy + zw), 1 - 2 * (xx + zz), 2 * (yz - xw),
                                 2 * (xz - yw), 2 * (yz + xw), 1 - 2 * (xx + yy),
                                 0, 0, 0);
        }

        public static Matrix4x4 RotationAxis(Vector3 axis, double angle)
        {
            axis = axis.Normalize();
            return RotationAxis(axis.X, axis.Y, axis.Z, angle);
        }
        private static Matrix4x4 RotationAxis(double x, double y, double z, double angle)
        {
            var cos = System.Math.Cos(angle);
            var icos = 1 - cos;
            var sin = System.Math.Sin(angle);

            return new Matrix4x4(cos + icos * x * x, icos * x * y + sin * z, icos * x * z - sin * y,
                                 icos * x * y - sin * z, cos + icos * y * y, icos * y * z + sin * x,
                                 icos * x * z + sin * y, icos * y * z - sin * x, cos + icos * z * z,
                                 0, 0, 0);
        }

        public static Matrix4x4 LookAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            Vector3 zaxis = (target - eye).Normalize();
            Vector3 xaxis = Vector3.CrossProduct(up, zaxis).Normalize();
            Vector3 yaxis = Vector3.CrossProduct(zaxis, xaxis);

            return new Matrix4x4(xaxis.X, yaxis.X, zaxis.X,
                xaxis.Y, yaxis.Y, zaxis.Y,
                xaxis.Z, yaxis.Z, zaxis.Z,
                -Vector3.DotProduct(xaxis, eye), -Vector3.DotProduct(yaxis, eye), -Vector3.DotProduct(zaxis, eye));
        }

        public static Matrix4x4 PerspectiveFovLH(double fieldOfViewY, double aspectRatio, double znearPlane, double zfarPlane)
        {
            double h = 1.0 / System.Math.Tan(fieldOfViewY / 2);
            double w = h / aspectRatio;
            double iz = 1.0 / (zfarPlane - znearPlane);

            return new Matrix4x4(w, 0, 0, 0,
                                 0, h, 0, 0,
                                 0, 0, zfarPlane * iz, 1,
                                 0, 0, -znearPlane * zfarPlane * iz, 0);
        }

        public static Matrix4x4 PerspectiveFovLH(double fieldOfViewY, double aspectRatio)
        {
            double h = 1.0 / System.Math.Tan(fieldOfViewY / 2);
            double w = h / aspectRatio;
            return new Matrix4x4(w, 0, 0, 0,
                                 0, h, 0, 0,
                                 0, 0, 1, 1,
                                 0, 0, -1, 0);
        }
    }

}
