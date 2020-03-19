using System;
using ray_tracing_modules.RayTraycer.Interfaces;

namespace ray_tracing_modules.RayTraycer.Model
{
    public class RayTraceAdapter
        : IRayTrace
    {
        private bool keepRendering = true;
        private Action<double> progress;
        private Action<int, int, double, double, double> setPixel;

        public RayTraceAdapter(Action<double> progress, Action<int, int, double, double, double> setPixel)
        {
            this.progress = progress;
            this.setPixel = setPixel;
        }

        public bool KeepRendering { get => keepRendering; set => keepRendering = value; }
        public double Progress { set => progress(value); }
        public void SetPixel(int x, int y, double r, double g, double b) => setPixel(x, y, r, g, b);
    }
}
