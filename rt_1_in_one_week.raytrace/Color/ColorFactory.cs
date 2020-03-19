using System;
using rt_1_in_one_week.raytrace.Mathematics;

namespace rt_1_in_one_week.raytrace.Color
{
    public static class ColorFactory
    {
        public static System.Drawing.Color Create(Vec3 vector)
        {
            Func<double, int> toColor = (v) => (int)(Math.Min(v * 255.0, 255.0) + 0.5);
            return System.Drawing.Color.FromArgb(toColor(vector.X), toColor(vector.Y), toColor(vector.Z));
        }

        public static System.Drawing.Color CreateSquare(Vec3 vector)
        {
            Func<double, int> toColor = (v) => (int)(Math.Max(0.0, Math.Min(Math.Sqrt(v) * 255.0, 255.0)) + 0.5);
            return System.Drawing.Color.FromArgb(toColor(vector.X), toColor(vector.Y), toColor(vector.Z));
        }
    }
}
