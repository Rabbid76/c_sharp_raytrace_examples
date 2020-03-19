namespace ray_tracing_modules.RayTraycer.Interfaces
{
    public interface IRayTrace
    {
        bool KeepRendering { get; }
        double Progress { set; }
        void SetPixel(int x, int y, double r, double g, double b);
    }
}
