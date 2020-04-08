using System;
using rt_2_the_next_week.raytrace.Interfaces;

namespace rt_2_the_next_week.raytrace.Mathematics
{
    public class Camera
        : ICamera
    {
        static Random _sampler = new Random();

        Vec3 _lower_left_corner = new Vec3(-2, -1, -1);
        Vec3 _horizontal = new Vec3(4, 0, 0);
        Vec3 _vertical = new Vec3(0, 2, 0);
        Vec3 _origin = new Vec3(0);
        Vec3 _w = new Vec3(0);
        Vec3 _u = new Vec3(0);
        Vec3 _v = new Vec3(0);
        double _lense_radius = 0;
        double _time0 = 0;
        double _time1 = 0;

        public static Camera CreateByVerticalFiled(double vfov, double aspect)
        {
            double theta = vfov * Math.PI / 180;
            double half_height = Math.Tan(theta / 2);
            double half_width = half_height * aspect;
            return new Camera(
                Vec3.Create(-half_width, -half_height, -1),
                Vec3.Create(2*half_width, 0, 0),
                Vec3.Create(0, 2*half_height, 0),
                Vec3.Create(0.0), 0, 0, 0);
        }

        public static Camera CreateLookAt(Vec3 lookFrom, Vec3 lookAt, Vec3 vup, double vfov, double aspect, double aderpture = 0, double fous_dist=1, double t0 = 0, double t1 = 0)
        {
            double theta = vfov * Math.PI / 180;
            double half_height = Math.Tan(theta / 2);
            double half_width = half_height * aspect;
            Vec3 w = (lookFrom - lookAt).Normalized();
            Vec3 u = Vec3.Cross(vup, w);
            Vec3 v = Vec3.Cross(w, u);
            return new Camera(
                lookFrom - u * half_width * fous_dist - v * half_height * fous_dist - w * fous_dist,
                u * half_width * 2 * fous_dist,
                v * half_height * 2 * fous_dist,
                lookFrom.Clone(),
                aderpture / 2, t0, t1);
        }

        public Camera(Vec3 lower_left_corner, Vec3 horizontal, Vec3 vertical, Vec3 origin, double lense_radius, double t0, double t1)
        {
            _lower_left_corner = lower_left_corner;
            _horizontal = horizontal;
            _vertical = vertical;
            _origin = origin;
            _lense_radius = lense_radius;
            _u = _horizontal.Normalized();
            _v = _vertical.Normalized();
            _w = (origin - lower_left_corner - horizontal * 0.5 - vertical * 0.5).Normalized();
            (_time0, _time1) = (t0, t1);
        }

        public Ray Get(double u, double v)
        {
            var rd = Sample.RandomInUnitSphere() * _lense_radius;
            var offset = u * rd.X + v * rd.Y;
            var time = _time0 + _sampler.NextDouble() + (_time1 - _time0);
            var ray = new Ray(_origin + offset, _lower_left_corner + _horizontal * u + _vertical * v - _origin - offset, time);
            return ray;
        }
    }
}
