using System;

namespace ray_tracing_modules.Color
{
    public static class ColorFactory
    {
        public static System.Drawing.Color Create(RGBColor color)
        {
            Func<double, int> toColor = (v) => (int)(Math.Min(v * 255.0, 255.0) + 0.5);
            return System.Drawing.Color.FromArgb(toColor(color.R), toColor(color.G), toColor(color.B));
        }

        public static System.Drawing.Color CreateSquare(RGBColor color)
        {
            Func<double, int> toColor = (v) => (int)(Math.Max(0.0, Math.Min(Math.Sqrt(v) * 255.0, 255.0)) + 0.5);
            return System.Drawing.Color.FromArgb(toColor(color.R), toColor(color.G), toColor(color.B));
        }
    }
}
