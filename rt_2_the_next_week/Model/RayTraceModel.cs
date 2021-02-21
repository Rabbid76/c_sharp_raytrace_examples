using System;
using rt_2_the_next_week.ViewModel;
using ray_tracing_modules.Color;
using ray_tracing_modules.Process;
using ray_tracing_modules.RayTraycer.Model;
using rt_2_the_next_week.raytrace.Interfaces;
using rt_2_the_next_week.raytrace.Scenes;
using rt_2_the_next_week.raytrace.RayTracer;

namespace rt_2_the_next_week.Model
{
    public class RayTraceModel
    {
        private RayTraceViewModel rayTraceViewModel;
        private RayTraceProcessHandler rayTraceProcess;

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
                case 0: scene = new CoverSceneRT1(aspect); break;
                case 1: scene = new MaterialsScene(aspect); break;
                case 2: scene = new DefocusBlurScene(aspect); break;
                case 3: scene = new TestScene1(aspect); break;
                case 4: scene = new TestScene2(aspect); break;
                case 5: scene = new CoverSceneRT1Motion(aspect); break;
                case 6: scene = new CheckerTextureScene(aspect); break;
                case 7: scene = new NoiseTextureScene(aspect); break;
                case 8: scene = new GlobeScene(aspect); break;
                case 9: scene = new SimpleLightScene(aspect); break;
                case 10: scene = new RoomScene(aspect); break;
                case 11: scene = new VolumeScene(aspect); break;
                case 12: scene = new CoverSceneRT2(aspect); break;
            }
            var rayTracer = new RayTracer(scene);
            var rayTraceTarget = new RayTraceTargetAdapter
            (
                progress => rayTraceViewModel.Progress = progress,
                (x, y, c) => rayTraceViewModel.SetBitmapPixel(x, y, ColorFactory.CreateSquare(c))
            );
            rayTraceProcess = new RayTraceProcessHandler(rayTraceConfguration, rayTracer, rayTraceTarget);
            rayTraceProcess.StartAsync();
        }

        public void TerminateRayTrace()
        {
            rayTraceProcess?.WaitStop();
        }
    }
}
