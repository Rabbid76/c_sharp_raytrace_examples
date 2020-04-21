using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ray_tracing_modules.Color;
using ray_tracing_modules.RayTraycer.Interfaces;

namespace ray_tracing_modules.Process
{
    public class RayTraceProcessAsyncEnumerable
    {
        readonly private RayTraceProcessEnumertor _process;
        readonly private IProgress<double> _progress;

        public RayTraceProcessAsyncEnumerable(
           IRayTraceConfigurationModel confguration, Func<double, double, RGBColor> background, IProgress<double> progress)
        {
            this._progress = progress;
            this._process = new RayTraceProcessEnumertor(confguration, background);
        }

        public bool KeepRendering { set => this._process.KeepRendering = value; }

        public async IAsyncEnumerable<RayTraceModel> RayTrace()
        {
            await Task.Delay(1);
            while (this._process.MoveNext())
            {
                var rayTraceData = this._process?.Current;
                if (rayTraceData != null)
                {
                    this._progress?.Report(rayTraceData.Progress);
                    yield return rayTraceData;
                }
            }
        }
    }
}
