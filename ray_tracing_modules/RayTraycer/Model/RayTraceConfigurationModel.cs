using ray_tracing_modules.RayTraycer.Interfaces;

namespace ray_tracing_modules.RayTraycer.Model
{
    public class RayTraceConfigurationModel
        : IRayTraceConfigurationModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Samples { get; set; }
        public double UpdateRate { get; set; }
    }
}
