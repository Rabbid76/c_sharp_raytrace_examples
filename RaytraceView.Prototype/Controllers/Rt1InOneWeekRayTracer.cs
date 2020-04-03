using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using rt_1_in_one_week.raytrace.Scenes;
using rt_1_in_one_week.raytrace.RayTracer;
using ray_tracing_modules.Color;
using ray_tracing_modules.Process;
using ray_tracing_modules.RayTraycer.Model;
using RaytraceView.Prototype.ViewModel;

namespace RaytraceView.Prototype.Controllers
{
    public class Rt1InOneWeekRayTracer
    {
        private RayTraceProcess rayTraceProcess;
        private readonly object rayTraceLock = new object();
        private double progress;
        private List<PixelData> pixelData = new List<PixelData>();
        private static Rt1InOneWeekRayTracer rayTracer;

        public static Rt1InOneWeekRayTracer RayTracerSingleton()
        {
            if (rayTracer == null)
                rayTracer = new Rt1InOneWeekRayTracer();
            return rayTracer;
        }

        public Rt1InOneWeekRayTracer()
        { }

        ~Rt1InOneWeekRayTracer()
        {
            rayTracer?.TerminateRayTrace();
        }

        public void StartRayTrace()
        {
            TerminateRayTrace();
            pixelData = new List<PixelData>();
            var rayTraceConfguration = new RayTraceConfigurationModel
            {
                Width = 600,
                Height = 300,
                Samples = 1000,
                UpdateRate = 0.1,
            };
            var sceneType = 0;
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
                progress => this.progress = progress,
                (x, y, c) => SetPixel(x, y, ColorFactory.CreateSquare(c))
            );
            rayTraceProcess = new RayTraceProcess(rayTraceConfguration, rayTracer, rayTraceTarget);
            rayTraceProcess.StartAsync();
        }

        public void TerminateRayTrace()
        {
            rayTraceProcess?.WaitStop();
        }

        private void SetPixel(int x, int y, Color color)
        {
            var newPixelData = new PixelData
            {
                X = x,
                Y = 300 - 1 - y,
                R = color.R,
                G = color.G,
                B = color.B
            };
            lock (rayTraceLock)
            {
                pixelData.Add(newPixelData);
            }
        }

        public Rt1InOneWeekendImageDataModel GetPixelData()
        {
            List<PixelData> currentPixelData;
            lock (rayTraceLock)
            {
                currentPixelData = pixelData;
                pixelData = new List<PixelData>();
            }
            var imageDataModel = new Rt1InOneWeekendImageDataModel
            {
                PixelData = currentPixelData.ToArray(),
                Progress = progress
            };
            return imageDataModel;
        }
    }
}
