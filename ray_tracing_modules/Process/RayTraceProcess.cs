using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ray_tracing_modules.Color;
using ray_tracing_modules.RayTraycer.Interfaces;

namespace ray_tracing_modules.Process
{
    public class RayTraceProcess
    {
        private bool _keepRendering = false;

        private IRayTraceConfigurationModel _confguration;
        Func<double, double, RGBColor> _background;
        IProgress<double> _progress;

        public RayTraceProcess(
           IRayTraceConfigurationModel confguration, Func<double, double, RGBColor> background, IProgress<double> progress)
        {
            this._confguration = confguration;
            this._background = background;
            this._progress = progress;
        }

        public bool KeepRendering { set => _keepRendering = value; }
       
        public async IAsyncEnumerable<RayTraceModel> RayTrace()
        {
            await Task.Delay(1);
            
            this._keepRendering = true;
            var cx = this._confguration.Width;
            var cy = this._confguration.Height;
            var no_samples = this._confguration.Samples;
            var update_rate = this._confguration.UpdateRate;
            int no_samples_outer = Math.Max(1, (int)(no_samples * update_rate + 0.5));
            int no_samples_inner = (no_samples + no_samples_outer - 1) / no_samples_outer;
            var sampler = new Random();
            var colorBuffer = new RGBdBuffer(cx, cy);
            int processed = 0;
            for (int outer_i = 0; this._keepRendering && (outer_i * no_samples_inner) < no_samples; ++outer_i)
            {
                foreach ((int x, int y) in new IterateBufferExp2(cx, cy))
                {
                    if (this._keepRendering == false)
                        break;

                    int no_start = no_samples_inner * outer_i;
                    int no_end = Math.Min(no_samples, no_start + no_samples_inner);
                    for (int sample_i = no_start; this._keepRendering && sample_i < no_end; ++sample_i)
                    {
                        (double dx, double dy) = ((double)x + sampler.NextDouble(), (double)y + sampler.NextDouble());
                        (double u, double v) = (dx / cx, dy / cy);
                        var sampleColor = this._background(u, v);
                        colorBuffer.Set(x, y, sampleColor, sample_i);
                    }

                    var color = colorBuffer.Get(x, y);
                    yield return new RayTraceModel { X = x, Y = y, Color = color };
                    var progress_value = (double)(++processed) / (double)(cx * cy * no_samples_outer);
                    this._progress.Report(progress_value);
                }
            }
        }
    }
}
