using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RaytraceView.Prototype.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Rt1InOneWeekendImageDataController
    {
        private readonly ILogger<Rt1InOneWeekendImageDataController> logger;
        private readonly Rt1InOneWeekRayTracer rayTracer;

        public Rt1InOneWeekendImageDataController(ILogger<Rt1InOneWeekendImageDataController> logger)
        {
            this.logger = logger;
            rayTracer = Rt1InOneWeekRayTracer.RayTracerSingleton();
        }

        [HttpGet]
        public ViewModel.Rt1InOneWeekendImageDataModel Get()
        {
            var model = new ViewModel.Rt1InOneWeekendImageDataModel
            {
                PixelData = rayTracer?.GetPixelData(),
            };
            return model;
        }
    }
}
