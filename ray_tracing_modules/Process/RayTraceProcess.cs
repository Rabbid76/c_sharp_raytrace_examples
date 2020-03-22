using System;
using System.Threading.Tasks;
using ray_tracing_modules.Color;
using ray_tracing_modules.RayTraycer.Interfaces;

namespace ray_tracing_modules.Process
{
    public class RayTraceProcess
    {
        private IRayTraceConfigurationModel confguration;
        private IRayTracer rayTracer;
        private IRayTraceTarget target;
        private bool keepRendering = false;
        private Task rayTraceTask;

        public RayTraceProcess(IRayTraceConfigurationModel confguration, IRayTracer rayTracer, IRayTraceTarget target)
        {
            this.confguration = confguration;
            this.rayTracer = rayTracer;
            this.target = target;
        }

        ~RayTraceProcess()
        {
            WaitStop();
        }

        public void StartAsync() => rayTraceTask = RayTraceAsync();

        public void WaitStop()
        {
            Stop();
            rayTraceTask?.Wait();
        }

        public void Stop() => keepRendering = false;

        public async Task RayTraceAsync()
        {
            await Task.Run(() =>
            {
                RayTrace();
            }).ConfigureAwait(false);
        }

        public void RayTrace()
        {
            keepRendering = true;
            var cx = confguration.Width;
            var cy = confguration.Height;
            var no_samples = confguration.Samples;
            var update_rate = confguration.UpdateRate;
            int no_samples_outer = Math.Max(1, (int)(no_samples * update_rate + 0.5));
            int no_samples_inner = (no_samples + no_samples_outer - 1) / no_samples_outer;
            var sampler = new Random();
            var colorBuffer = new RGBdBuffer(cx, cy);
            int processed = 0;
            for (int outer_i = 0; keepRendering && (outer_i * no_samples_inner) < no_samples; ++outer_i)
            {
                foreach ((int x, int y) in new IterateBufferExp2(cx, cy))
                {
                    if (keepRendering == false)
                        break;

                    int no_start = no_samples_inner * outer_i;
                    int no_end = Math.Min(no_samples, no_start + no_samples_inner);
                    for (int sample_i = no_start; keepRendering && sample_i < no_end; ++sample_i)
                    {
                        (double dx, double dy) = ((double)x + sampler.NextDouble(), (double)y + sampler.NextDouble());
                        (double u, double v) = (dx / cx, dy / cy);
                        var sampleColor = rayTracer.RaytraceColor(u, v);
                        colorBuffer.Set(x, y, sampleColor, sample_i);
                    }

                    var color = colorBuffer.Get(x, y);
                    target.SetPixel(x, cy - y - 1, color);
                    target.Progress = (double)(++processed) / (double)(cx * cy * no_samples_outer);
                }
            }
        }
    }
}
