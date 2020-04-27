using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ray_tracing_modules.Color;
using ray_tracing_modules.RayTraycer.Interfaces;

namespace ray_tracing_modules.Process
{
    public class RayTraceProcessAsyncEnumerable
    {
        readonly private RayTraceProcessEnumertor _process;
        readonly private IProgress<double> _progress;
        readonly private CancellationTokenSource _cancelationSource = new CancellationTokenSource();

        public RayTraceProcessAsyncEnumerable(
           IRayTraceConfigurationModel confguration, Func<double, double, RGBColor> background, IProgress<double> progress)
        {
            this._progress = progress;
            this._process = new RayTraceProcessEnumertor(confguration, background);
        }

        public void CancelRendering() => _cancelationSource.Cancel();

        public CancellationToken CancellationToken { get => _cancelationSource.Token; }

        public async IAsyncEnumerable<RayTraceModel> RayTrace(CancellationToken cancellationToken)
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
                if (cancellationToken.IsCancellationRequested)
                    break;
            }
        }
    }
}
