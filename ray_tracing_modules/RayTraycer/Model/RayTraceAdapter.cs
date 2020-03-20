using System;
using ray_tracing_modules.Color;
using ray_tracing_modules.RayTraycer.Interfaces;

namespace ray_tracing_modules.RayTraycer.Model
{
    public class RayTraceTargetAdapter
        : IRayTraceTarget
    {
        private Action<double> progress;
        private Action<int, int, RGBColor> setPixel;

        public RayTraceTargetAdapter(Action<double> progress, Action<int, int, RGBColor> setPixel)
        {
            this.progress = progress;
            this.setPixel = setPixel;
        }

        public double Progress { set => progress(value); }
        public void SetPixel(int x, int y, RGBColor color) => setPixel(x, y, color);
    }
}
