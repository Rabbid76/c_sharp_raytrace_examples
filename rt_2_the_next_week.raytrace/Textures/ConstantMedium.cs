using System;
using rt_2_the_next_week.raytrace.Interfaces;
using rt_2_the_next_week.raytrace.Mathematics;
using rt_2_the_next_week.raytrace.Materials;

namespace rt_2_the_next_week.raytrace.Textures
{
    public class ConstantMedium
        : IHitable
    {
        static Random _sampler = new Random();
        IHitable _boundary;
        double _density;
        IMaterial _phaseFunction;

        public ConstantMedium(IHitable boundary, double density, ITexture albedo)
        {
            _boundary = boundary;
            _density = density;
            _phaseFunction = new Isotropic(albedo);
        }

        public bool BoundingBox(double t0, double t1, out AABB box)
        {
            return _boundary.BoundingBox(t0, t1, out box);
        }

        public bool Hit(Ray r, double t_min, double t_max, out HitRecord rec)
        {
            bool enableDebug = false;
            bool db = enableDebug && _sampler.Next() < 0.00001;

            HitRecord rec1, rec2;
            if (_boundary.Hit(r, double.MinValue, double.MaxValue, out rec))
            {
                rec1 = rec;
                if (_boundary.Hit(r, rec1.T + 0.0001, double.MaxValue, out rec2))
                {
                    if (enableDebug)
                        Console.WriteLine($"t0 t1 {rec1.T} {rec2.T}");

                    if (rec1.T < t_min) rec1.T = t_min;
                    if (rec2.T > t_max) rec2.T = t_max;

                    if (rec1.T >= rec2.T)
                        return false;
                    if (rec1.T < 0)
                        rec1.T = 0;

                    double distance_inside_boundary = (rec2.T - rec1.T) * r.Direction.Length;
                    double hit_distance = -(1 / _density) * Math.Log(_sampler.NextDouble());

                    if (hit_distance < distance_inside_boundary)
                    {

                        var t = rec1.T + hit_distance / r.Direction.Length;
                        var p = r.PointAt(rec.T);

                        if (enableDebug)
                            Console.WriteLine($"hit_distance {hit_distance}; rec.T {rec.T}; rectP {rec.P}");

                        var n = Vec3.Create(1, 0, 0); // arbitrary
                        rec = new HitRecord(t, 0, 0, p, n, _phaseFunction);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
