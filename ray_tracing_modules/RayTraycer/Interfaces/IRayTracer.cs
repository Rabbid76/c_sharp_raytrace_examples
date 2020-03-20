using ray_tracing_modules.Color;

namespace ray_tracing_modules.RayTraycer.Interfaces
{
    public interface IRayTracer
    {
        RGBColor RaytraceColor(double u, double v);
    }
}
