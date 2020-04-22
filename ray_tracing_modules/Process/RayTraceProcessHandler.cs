using System;
using System.Threading.Tasks;
using ray_tracing_modules.Color;
using ray_tracing_modules.RayTraycer.Interfaces;

namespace ray_tracing_modules.Process
{
    public class RayTraceProcessHandler
    {
        private IRayTraceConfigurationModel confguration;
        private IRayTracer rayTracer;
        private IRayTraceTarget target;
        private Task rayTraceTask;
        //private RayTraceProcessEnumertor enumertator;
        private RayTraceProcessAsyncEnumerable enumerable;

        public RayTraceProcessHandler(IRayTraceConfigurationModel confguration, IRayTracer rayTracer, IRayTraceTarget target)
        {
            this.confguration = confguration;
            this.rayTracer = rayTracer;
            this.target = target;
        }

        ~RayTraceProcessHandler()
        {
            WaitStop();
        }

        public void StartAsync() => rayTraceTask = RayTraceAsync();

        public void WaitStop()
        {
            Stop();
            rayTraceTask?.Wait();
        }

        public void Stop()
        {
            //if (enumertator != null)
            //    enumertator.KeepRendering = false;
            enumerable?.CancelRendering();
        }

        public async Task RayTraceAsync()
        {
            /*
            enumertator = new RayTraceProcessEnumertor(this.confguration, rayTracer.RaytraceColor);
            await Task.Run(() =>
            {
                while (target != null && enumertator != null ? enumertator.MoveNext() : false)
                {
                    var rayTraceData = enumertator?.Current;
                    if (rayTraceData != null)
                    {
                        this.target.SetPixel(rayTraceData.X, rayTraceData.Y, rayTraceData.Color);
                        this.target.Progress = rayTraceData.Progress;
                    }
                }
                enumertator = null;
            }).ConfigureAwait(false);
            */

            enumerable = new RayTraceProcessAsyncEnumerable(this.confguration, rayTracer.RaytraceColor, null);
            await Task.Run(async () =>
            {
                await foreach (var rayTraceData in enumerable.RayTrace())
                {
                    target.SetPixel(rayTraceData.X, rayTraceData.Y, rayTraceData.Color);
                    this.target.Progress = rayTraceData.Progress;
                }

            }).ConfigureAwait(false);

            /*
            var progress = new Progress<double>((value) => this.target.Progress = value);
            enumerable = new RayTraceProcessAsyncEnumerable(this.confguration, rayTracer.RaytraceColor, progress);
            await Task.Run(async () =>
            {
                await foreach (var rayTraceData in enumerable.RayTrace())
                {
                    target.SetPixel(rayTraceData.X, rayTraceData.Y, rayTraceData.Color);
                }

            }).ConfigureAwait(false);
            */
        }
    }
}
