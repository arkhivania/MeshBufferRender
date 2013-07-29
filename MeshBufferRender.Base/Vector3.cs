using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshBufferRender.Base
{
    public struct Vector3
    {
        public double X;

        public double Y;

        public double Z;

        public Vector3(double x, double y, double z)
        {
            X = x; Y = y; Z = z;
        }

        public static Vector3 operator *(Vector3 v, Matrix4x4 m)
        {
            double f = v.X * m.M14 + v.Y * m.M24 + v.Z * m.M34 + 1 * m.M44;
            if (f != 1 && f != 0)
                return new Vector3((v.X * m.M11 + v.Y * m.M21 + v.Z * m.M31 + m.M41) / f,
                                   (v.X * m.M12 + v.Y * m.M22 + v.Z * m.M32 + m.M42) / f,
                                   (v.X * m.M13 + v.Y * m.M23 + v.Z * m.M33 + m.M43) / f);
            else
                return new Vector3(v.X * m.M11 + v.Y * m.M21 + v.Z * m.M31 + m.M41,
                                   v.X * m.M12 + v.Y * m.M22 + v.Z * m.M32 + m.M42,
                                   v.X * m.M13 + v.Y * m.M23 + v.Z * m.M33 + m.M43);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector3 operator -(Vector3 a)
        {
            return new Vector3(-a.X, -a.Y, -a.Z);
        }

        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return (a.X == b.X && a.Y == b.Y && a.Z == b.Z);
        }

        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return (a.X != b.X || a.Y != b.Y || a.Z != b.Z);
        }

        public static Vector3 operator *(Vector3 a, double b)
        {
            return new Vector3(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vector3 operator *(double b, Vector3 a)
        {
            return new Vector3(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vector3 operator /(Vector3 a, double b)
        {
            return new Vector3(a.X / b, a.Y / b, a.Z / b);
        }

        public void Add(Vector3 a)
        {
            X += a.X; Y += a.Y; Z += a.Z;
        }

        public void Subtract(Vector3 a)
        {
            X -= a.X; Y -= a.Y; Z -= a.Z;
        }

        public void Multiply(double a)
        {
            X *= a; Y *= a; Z *= a;
        }

        public void Divide(double a)
        {
            X /= a; Y /= a; Z /= a;
        }

        public static double DotProduct(Vector3 a, Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public static Vector3 CrossProduct(Vector3 a, Vector3 b)
        {
            return new Vector3(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        }

        public static double Angle(Vector3 a, Vector3 b)
        {
            var aLength = a.Length;
            var bLength = b.Length;
            if (aLength * bLength == 0) throw new ArgumentException("a or b equal to 0");
            var c = a.DotProduct(b) / (aLength * bLength);
            return System.Math.Acos(c);
        }

        public Vector3 Normalize()
        {
            var l = this.Length;
            return l > 0 ? new Vector3(X / l, Y / l, Z / l) : this;
        }


        public double Length
        {
            get
            {
                return System.Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }

        public double LengthSquare
        {
            get
            {
                return X * X + Y * Y + Z * Z;
            }
        }

        public static Vector3 VectorWeightSumm(Vector3 v1, double weight1, Vector3 v2, double weight2)
        {
            var w = weight1 + weight2;
            if (w == 0) return (v1 + v2) / 2;
            return (v1 * weight1 + v2 * weight2) / w;
        }

        public static Vector3 VectorWeightSumm(Vector3 v1, Vector3 v2, double w)
        {
            return v1 * (1 - w) + v2 * w;
        }

        public bool Validate()
        {
            if (double.IsNaN(X) || double.IsNaN(Y) || double.IsNaN(Z) ||
                double.IsInfinity(X) || double.IsInfinity(Y) || double.IsInfinity(Z)) return false;
            return true;
        }

        public static Vector3 NaN
        {
            get
            {
                return new Vector3(double.NaN, double.NaN, double.NaN);
            }
        }

        public override string ToString()
        {
            return X.ToString() + "; " + Y.ToString() + "; " + Z.ToString();
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector3)
            {
                Vector3 v2 = (Vector3)obj;

                return X == v2.X && Y == v2.Y && Z == v2.Z;
            }
            return false;
        }
    }

}
