using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using rt_1_in_one_week.raytrace.Scenes;
using ray_tracing_modules.Color;
using ray_tracing_modules.Process;
using ray_tracing_modules.RayTraycer.Model;
using RaytraceView.Prototype.ViewModel;

namespace RaytraceView.Prototype.Controllers
{
    public class RayTracer
    {
        private static RayTracer rayTracer;
        private RayTraceProcess rayTraceProcess;
        private readonly object rayTraceLock = new object();
        private double progress;
        private List<PixelData> pixelData = new List<PixelData>();
        private string[] scenes = new string[] { "Cover scene", "Materials", "Defocus Blur", "Test" };

        public string[] Scenes { get => scenes; }
        public RayTraceParameterModel Parameter { get; set; }

        public static RayTracer RayTracerSingleton()
        {
            if (rayTracer == null)
                rayTracer = new RayTracer();
            return rayTracer;
        }

        public RayTracer()
        {
            Parameter = new RayTraceParameterModel
            {
                SceneName = Scenes[0],
                Width = 600,
                Height = 300,
                Samples = 100,
                UpdateRate = 0.1
            };
        }

        ~RayTracer()
        {
            rayTracer?.TerminateRayTrace();
        }

        public void StartRayTrace()
        {
            TerminateRayTrace();
            pixelData = new List<PixelData>();
            var rayTraceConfguration = new RayTraceConfigurationModel
            {
                Width = Parameter.Width,
                Height = Parameter.Height,
                Samples = Parameter.Samples,
                UpdateRate = Parameter.UpdateRate,
            };
            var sceneType = Scenes.ToList().FindIndex((name) => Parameter.SceneName == name);
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
            var rayTracer = new rt_1_in_one_week.raytrace.RayTracer.RayTracer(scene);
            var rayTraceTarget = new RayTraceTargetAdapter
            (
                progress => this.progress = progress,
                (x, y, c) => SetPixel(x, y, ColorFactory.CreateSquare(c), rayTraceConfguration.Width, rayTraceConfguration.Height)
            );
            rayTraceProcess = new RayTraceProcess(rayTraceConfguration, rayTracer, rayTraceTarget);
            rayTraceProcess.StartAsync();
        }

        public void TerminateRayTrace()
        {
            rayTraceProcess?.WaitStop();
        }

        private void SetPixel(int x, int y, Color color, int w, int h)
        {
            var newPixelData = new PixelData
            {
                X = x,
                Y = h - 1 - y,
                R = color.R,
                G = color.G,
                B = color.B
            };
            lock (rayTraceLock)
            {
                pixelData.Add(newPixelData);
            }
        }

        public RayTraceImageDataModel GetPixelData()
        {
            List<PixelData> currentPixelData;
            lock (rayTraceLock)
            {
                currentPixelData = pixelData;
                pixelData = new List<PixelData>();
            }
            var imageDataModel = new RayTraceImageDataModel
            {
                PixelData = currentPixelData.ToArray(),
                Progress = progress
            };
            return imageDataModel;
        }
    }
}
