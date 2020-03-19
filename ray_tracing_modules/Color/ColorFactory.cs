using System;

namespace ray_tracing_modules.RayTraycer.Color
{
    public static class ColorFactory
    {
        public static System.Drawing.Color Create(double r, double g, double b)
        {
            Func<double, int> toColor = (v) => (int)(Math.Min(v * 255.0, 255.0) + 0.5);
            return System.Drawing.Color.FromArgb(toColor(r), toColor(g), toColor(b));
        }

        public static System.Drawing.Color CreateSquare(double r, double g, double b)
        {
            Func<double, int> toColor = (v) => (int)(Math.Max(0.0, Math.Min(Math.Sqrt(v) * 255.0, 255.0)) + 0.5);
            return System.Drawing.Color.FromArgb(toColor(r), toColor(g), toColor(b));
        }
    }
}
