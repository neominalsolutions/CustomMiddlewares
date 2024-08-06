using CustomMiddlewares.Dtos;
using CustomMiddlewares.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomMiddlewares.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TestsController : ControllerBase
  {
    //[Sample]
    [HttpGet]
    [Cache(duration:5)]
    public IActionResult Get()
    {
      var data = new List<SampleResponse> { new SampleResponse { Text = "Sample-1" }, new SampleResponse { Text = "Sample-2" } };

      return Ok(data);
    }

    [HttpPost]
    public IActionResult Post([FromBody] ProductDto request)
    {
      
      return Ok();
    }
  }
}
