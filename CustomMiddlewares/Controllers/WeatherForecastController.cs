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

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptions<HeaderOptions> options1)
    {
      _logger = logger;
      _options1 = options1;

    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
      throw new Exception("Hata");

      return Ok();
    } 

  
  }
}