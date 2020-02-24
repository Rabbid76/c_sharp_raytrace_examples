using static System.Math;

namespace rt_1_in_one_week.Mathematics
{
    public struct Vec3
    {
        private double[] _v;

        public double this[int index] { get => _v[index]; set => _v[index] = value; }
        public double X { get => _v[0]; set => _v[0] = value; }
        public double Y { get => _v[1]; set => _v[1] = value; }
        public double Z { get => _v[2]; set => _v[2] = value; }

        public Vec3(double val) { _v = new double[3] { val, val, val }; }
        public Vec3(double x, double y, double z) { _v = new double[3] { x, y, z }; }
        public Vec3(double[] v) { _v = new double[3] { v[0], v[1], v[2] }; }
        public Vec3(Vec3 v) { _v = (double[])v._v.Clone(); }

        public Vec3 Clone() => new Vec3(_v);

        public static Vec3 Create(double val) => new Vec3(val);
        public static Vec3 Create(double x, double y, double z) => new Vec3(x, y, z);
        public static Vec3 Create(double[] v) => new Vec3(v);
        public static Vec3 Create(Vec3 v) => new Vec3(v);

        public static Vec3 operator -(Vec3 a) => new Vec3(-a.X, -a.Y, -a.Z);

        public static Vec3 operator +(Vec3 a, double b) => new Vec3(a.X + b, a.Y + b, a.Z + b);
        public static Vec3 operator -(Vec3 a, double b) => new Vec3(a.X - b, a.Y - b, a.Z - b);
        public static Vec3 operator *(Vec3 a, double b) => new Vec3(a.X * b, a.Y * b, a.Z * b);
        public static Vec3 operator /(Vec3 a, double b) => new Vec3(a.X / b, a.Y / b, a.Z / b);

        public static Vec3 operator + (Vec3 a, Vec3 b) => new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vec3 operator -(Vec3 a, Vec3 b) => new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vec3 operator *(Vec3 a, Vec3 b) => new Vec3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        public static Vec3 operator /(Vec3 a, Vec3 b) => new Vec3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);

        /// <summary>
        /// [Euclidean distance](https://en.wikipedia.org/wiki/Euclidean_distance)
        /// </summary>
        public double LengthSquare { get => X * X + Y * Y + Z * Z; }
        public double Length { get => Sqrt(LengthSquare); }
        public Vec3 Normalize() { this /= Length; return this; }
        public Vec3 Normalized() => this / Length;
        public double DistanceToSquare(Vec3 b) => (this - b).LengthSquare;
        public double DistanceTo(Vec3 b) => (this - b).Length;
        static public Vec3 Normalize(Vec3 a) => a.Normalized();
        public static double DistanceSquare(Vec3 a, Vec3 b) => (a - b).LengthSquare;
        public static double Distance(Vec3 a, Vec3 b) => (a - b).Length;

        /// <summary>
        /// [Dot product](https://en.wikipedia.org/wiki/Dot_product)
        /// </summary>
        public double Dot(Vec3 b) => X * b.X + Y * b.Y + Z * b.Z;
        public double DotSelf() => X * X + Y * Y + Z * Z; 
        public static double Dot(Vec3 a, Vec3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

        /// <summary>
        /// [Cross product](https://en.wikipedia.org/wiki/Cross_product)
        /// </summary>
        public Vec3 Cross(Vec3 b) { this = Cross(this, b); return this; }
        public static Vec3 Cross(Vec3 a, Vec3 b)
        {
            return new Vec3(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X); 
        }

        /// <summary>
        /// [Reflection](https://en.wikipedia.org/wiki/Reflection_(mathematics))
        /// </summary>
        public Vec3 Reflect(Vec3 n) { this = this - n * Dot(n) * 2; return this; }
        public static Vec3 Reflect(Vec3 v, Vec3 n) { return v - n * Dot(v, n) * 2; }

        /// <summary>
        /// Refract [Snell's law](https://en.wikipedia.org/wiki/Snell%27s_law)
        /// </summary>
        public bool Refract(Vec3 n, double ni_over_nt)
        {
            Vec3 refracted;
            if (Refract(this, n, ni_over_nt, out refracted))
            {
                this = refracted;
                return true;
            }
            return false;
        }

        public static bool Refract(Vec3 v, Vec3 n, double ni_over_nt, out Vec3 refracted)
        {
            Vec3 uv = v.Normalized();
            double dt = uv.Dot(n);
            double discriminante = 1 - ni_over_nt * ni_over_nt * (1 - dt * dt);
            if (discriminante > 0)
            {
                refracted = (uv - n * dt) * ni_over_nt - n * Sqrt(discriminante);
                return true;
            }
            refracted = Create(0);
            return false;
        }
    }
}
