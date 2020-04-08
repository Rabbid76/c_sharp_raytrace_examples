using System;

namespace rt_2_the_next_week.raytrace.Mathematics
{
    public static class Function
    {
        public static double Schlick(double cosine, double ref_idx)
        {
            double r0 = (1 - ref_idx) / (1 + ref_idx);
            r0 = r0 * r0;
            return r0 + (1 - r0) * Math.Pow(1 - cosine, 5);
        }

        public static (double u, double v) GetSphereUV(Vec3 p)
        {
            double phi = Math.Atan2(p.Z, p.X);
            double theta = Math.Asin(p.Y);
            double u = 1 - (phi + Math.PI) / (2 * Math.PI);
            double v = (theta + Math.PI / 2) / Math.PI;
            return (u, v);
        }
    }
}