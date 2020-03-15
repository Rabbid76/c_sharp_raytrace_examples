using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RaytraceView.Prototype.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Rt1InOneWeekendController : ControllerBase
    {
        private readonly ILogger<Rt1InOneWeekendController> _logger;

        public Rt1InOneWeekendController(ILogger<Rt1InOneWeekendController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ViewModel.Rt1InOneWeekend Get()
        {
            var image = TestImage();
            var imageBytes = ImageToByteArray(image);
            var imageString = ByteArrayToString(imageBytes);

            var model = new ViewModel.Rt1InOneWeekend
            {
                Title = "Ray Tracing in One Weekend",
                ImagePng = imageString
            };
            return model;
        }

        private Image TestImage()
        {
            int cx = 100;
            int cy = 100;
            var bitmap = new Bitmap(cx, cy);
            for (int x = 0; x < cx; ++x)
            {
                for (int y = 0; y < cy; ++y)
                {
                    double u = (double)x / cx;
                    double v = (double)y / cy;
                    Color c = RGBColor(u, v, (1-u)*(1-v));
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
