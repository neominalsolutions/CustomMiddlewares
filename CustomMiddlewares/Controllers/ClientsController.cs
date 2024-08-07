using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomMiddlewares.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ClientsController : ControllerBase
  {
    [HttpGet]
    public IActionResult Get()
    {
      return Ok();
    }
  }
}
