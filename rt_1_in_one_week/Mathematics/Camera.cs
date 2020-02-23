using System;
using System.Collections.Generic;
using System.Text;

namespace rt_1_in_one_week.Mathematics
{
    public class Camera
        : ICamera
    {
        Vec3 _lower_left_corner = new Vec3(-2, -1, -1);
        Vec3 _horizontal = new Vec3(4, 0, 0);
        Vec3 _vertical = new Vec3(0, 2, 0);
        Vec3 _origin = new Vec3(0, 0, 0);

        public Camera(Vec3 lower_left_corner, Vec3 horizontal, Vec3 vertical, Vec3 origin)
        {
            _lower_left_corner = lower_left_corner;
            _horizontal = horizontal;
            _vertical = vertical;
            _origin = origin;
        }

        public Ray Get(double u, double v) => new Ray(_origin, _lower_left_corner + _horizontal * u + _vertical * v);
    }
}
