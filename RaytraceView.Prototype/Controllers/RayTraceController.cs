using System;
using System.IO;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RaytraceView.Prototype.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RayTraceController : ControllerBase
    {
        private readonly ILogger<RayTraceController> logger;
        private readonly RayTracer rayTracer;

        public RayTraceController(ILogger<RayTraceController> logger)
        {
            this.logger = logger;
            rayTracer = RayTracer.RayTracerSingleton();
            rayTracer?.StartRayTrace();
        }

        [HttpPut]
        public IActionResult Put(ViewModel.RayTraceParameterModel parameter)
        {
            rayTracer.Parameter = parameter;
            rayTracer?.StartRayTrace();
            return Accepted();
        }

        [HttpGet]
        public ViewModel.RayTraceModel Get()
        {
            var image = TestImage();
            var imageBytes = ImageToByteArray(image);
            var imageString = ByteArrayToString(imageBytes);
            var parameter = new ViewModel.RayTraceParameterModel
            {
                SceneName = rayTracer.Parameter.SceneName,
                Width = rayTracer.Parameter.Width,
                Height = rayTracer.Parameter.Height,
                Samples = rayTracer.Parameter.Samples,
                UpdateRate = rayTracer.Parameter.UpdateRate
            };
            var model = new ViewModel.RayTraceModel
            {
                Title = "Ray Tracing in One Weekend",
                ImagePng = imageString,
                Scenes = rayTracer.Scenes,
                Parameter = parameter
            };
            return model;
        }

        private Image TestImage()
        {
            int cx = rayTracer.Parameter.Width;
            int cy = rayTracer.Parameter.Height;
            var bitmap = new Bitmap(cx, cy);
            for (int x = 0; x < cx; ++x)
            {
                for (int y = 0; y < cy; ++y)
                {
                    double u = (double)x / cx;
                    double v = (double)y / cy;
                    double t = (1 - v * u) * 0.4 + 0.3;
                    Color c = RGBColor(t, t, t);
                    bitmap.SetPixel(x, y, c);
                }
            }
            return bitmap;
        }

        private static System.Drawing.Color RGBColor(double r, double g, double b)
        {
            Func<double, int> toColor = (v) => (int)(Math.Min(v * 255.0, 255.0) + 0.5);
            return System.Drawing.Color.FromArgb(toColor(r), toColor(g), toColor(b));
        }

        private static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        private static string ByteArrayToString(byte[] imageBytes) => System.Convert.ToBase64String(imageBytes);
    }
}
