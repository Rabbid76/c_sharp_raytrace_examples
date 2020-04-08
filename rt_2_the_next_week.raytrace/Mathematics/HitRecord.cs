using rt_2_the_next_week.raytrace.Interfaces;

namespace rt_2_the_next_week.raytrace.Mathematics
{
    public class HitRecord
    {
        private double _t;
        private double _u;
        private double _v;
        private Vec3 _p;
        private Vec3 _normal;
        private IMaterial _material;

        public double T { get => _t; set => _t = value; }
        public double U { get => _u; }
        public double V { get => _v; }
        public Vec3 P { get => _p; }
        public Vec3 Normal { get => _normal; }
        public IMaterial Material { get => _material; }

        public HitRecord(double t, double u, double v, Vec3 p, Vec3 normal, IMaterial material)
        {
            _t = t;
            _u = u;
            _v = v;
            _p = p;
            _normal = normal;
            _material = material;
        }

        public void InvertNormal() => _normal = -_normal;
        public void Displace(Vec3 offset) => _p += offset;
    }
}
