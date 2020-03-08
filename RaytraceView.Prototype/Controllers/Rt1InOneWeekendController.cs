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
  public class Rt1InOneWeekendController : ControllerBase
  {
    private readonly ILogger<Rt1InOneWeekendController> _logger;

    public Rt1InOneWeekendController(ILogger<Rt1InOneWeekendController> logger)
    {
      _logger = logger;
    }

    [HttpGet]
    public Rt1InOneWeekend Get()
    {
      return new Rt1InOneWeekend { Title = "Ray Tracing in One Weekend" };
    }
  }
}
