using System;
using System.Threading.Tasks;
using rt_1_in_one_week.ViewModel;
using rt_1_in_one_week.raytrace.Scenes;
using rt_1_in_one_week.raytrace.RayTracer;
using ray_tracing_modules.Color;
using ray_tracing_modules.Process;
using ray_tracing_modules.RayTraycer.Model;

namespace rt_1_in_one_week.Model
{
    public class RayTraceModel
    {
        private RayTraceViewModel rayTraceViewModel;
        private Task rayTraceTask;
        private RayTraceProcess rayTraceProcess;

        public RayTraceModel()
        { }

        public RayTraceViewModel ViewModel 
        {
            set { rayTraceViewModel = value; }
        }

        public void StartRayTrace()
        {
            var rayTraceConfguration = new RayTraceConfigurationModel
            {
                Width = rayTraceViewModel.BitmapWidth,
                Height = rayTraceViewModel.BitmapHeight,
                Samples = rayTraceViewModel.RenderSamples,
                UpdateRate = rayTraceViewModel.RenderUpdateRate,
            };
            var sceneType = Int32.Parse(rayTraceViewModel.CurrentScene.Number);
            var aspect = (double)rayTraceConfguration.Width / (double)rayTraceConfguration.Height;
            IScene scene;
            switch (sceneType)
            {
                default:
                case 0: scene = new CoverScene(aspect); break;
                case 1: scene = new MaterialsScene(aspect); break;
                case 2: scene = new DefocusBlurScene(aspect); break;
                case 3: scene = new TestScene(aspect); break;
            }
            var rayTracer = new RayTracer(scene);
            var rayTraceTarget = new RayTraceTargetAdapter
            (
                progress => rayTraceViewModel.Progress = progress,
                (x, y, c) => rayTraceViewModel.SetBitmapPixel(x, y, ColorFactory.CreateSquare(c))
            );
            rayTraceProcess = new RayTraceProcess(rayTraceConfguration, rayTracer, rayTraceTarget);
            rayTraceTask = rayTraceProcess.RayTraceAsync();
        }

        public void TerminateRayTrace()
        {
            if (rayTraceProcess != null)
                rayTraceProcess.Stop();
            if (rayTraceTask != null)
                rayTraceTask.Wait();
        }
    }
}
