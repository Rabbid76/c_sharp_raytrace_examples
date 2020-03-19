namespace ray_tracing_modules.RayTraycer.Interfaces
{
    public interface IRayTraceConfigurationModel
    {
        int Width { get; }
        int Height { get; }
        int Samples { get; }
        double UpdateRate { get; }
    }
}
