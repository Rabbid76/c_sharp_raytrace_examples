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
    public class RayTraceImageDataController
    {
        private readonly ILogger<RayTraceImageDataController> logger;
        private readonly RayTracer rayTracer;

        public RayTraceImageDataController(ILogger<RayTraceImageDataController> logger)
        {
            this.logger = logger;
            rayTracer = RayTracer.RayTracerSingleton();
        }

        [HttpGet]
        public ViewModel.RayTraceImageDataModel Get()
        {
            var model = rayTracer?.GetPixelData();
            return model;
        }
    }
}
