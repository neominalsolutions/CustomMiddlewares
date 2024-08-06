using Assistt.BLL.Layer.Services;
using CustomMiddlewares.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CustomMiddlewares.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class WeatherForecastController : ControllerBase
  {
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IOptions<HeaderOptions> _options1;
    private readonly ISampleService _sampleService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptions<HeaderOptions> options1, ISampleService sampleService)
    {
      _logger = logger;
      _options1 = options1;
      _sampleService = sampleService;

    }

    [HttpGet]
    //[ErrorHandler]
    //[Cache(Duration=50)]
    public async Task<IActionResult> Get()
    {
      await _sampleService.ExecuteAsync(new Assistt.BLL.Layer.Models.SampleEntity { Name = null });

      return Ok();
    } 

  
  }
}