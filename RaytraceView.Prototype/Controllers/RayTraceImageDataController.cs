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

        [HttpPost]
        [Route("IsInvalidRayTraceModel")]
        public bool IsInvalidRayTraceModel(ViewModel.RayTraceParameterModel parameter)
        {
            return
                parameter.Width < 1 || parameter.Width > 4096 ||
                parameter.Height < 1 || parameter.Height > 4096 ||
                parameter.Samples < 1 || parameter.Samples > 1000000;
        }
    }
}
