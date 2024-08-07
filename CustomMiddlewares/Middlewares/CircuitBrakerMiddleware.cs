using System.Collections.Concurrent;
using System.Net;

namespace CustomMiddlewares.Middlewares
{
 
  // İstinai durumlarda hataları request path üzerinden yakalaıyp gereksiz istelerin handle ettiğimiz kısım.
  public class CircuitBrakerMiddleware
  {
   
    private readonly static ConcurrentDictionary<string, DateTime> Failures = new();
    private readonly RequestDelegate _next;
    // 1 dakika boyunca gelen hatalı istekleri kesiceğiz.
    private const int _breakMinute = 1;
    private readonly TimeSpan _breakDuration = TimeSpan.FromMinutes(_breakMinute);

    public CircuitBrakerMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      var pathName = context.Request.Path;

      if(Failures.ContainsKey(pathName) && DateTime.Now - Failures[pathName] < _breakDuration)
      {
        context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
        await context.Response.WriteAsJsonAsync(new { Message = $"{pathName} yapılan istek kesintiye uğratıldı {_breakMinute} kadar bekleyiniz" });
        return;
      }

      try
      {
        await _next(context);
      }
      catch (Exception)
      {
        Failures[pathName] = DateTime.Now;
      }
    }
  }
}
