using ray_tracing_modules.Color;

namespace ray_tracing_modules.RayTraycer.Interfaces
{
    public interface IRayTraceTarget
    {
        double Progress { set; }
        void SetPixel(int x, int y, RGBColor color);
    }
}
