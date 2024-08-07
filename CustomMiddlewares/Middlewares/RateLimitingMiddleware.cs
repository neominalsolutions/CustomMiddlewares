using System.Collections.Concurrent;

namespace CustomMiddlewares.Middlewares
{
  public class RateLimitingMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly static ConcurrentDictionary<string, (DateTime,int)> RateLimit = new();
    private const int _breakMinute = 1;
    private readonly TimeSpan _breakDuration = TimeSpan.FromMinutes(_breakMinute);
    private const int maxLimit = 10; 

    public RateLimitingMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      var pathName = context.Request.Path;
     


      if (RateLimit.ContainsKey(pathName) && DateTime.Now - RateLimit[pathName].Item1 < _breakDuration)
      {
        var requestCount = RateLimit[pathName].Item2;

        if (RateLimit[pathName].Item2 >= maxLimit)
        {
          context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
          context.Response.Headers["X-Rate-Limit-Remaning-Count"] ="0";
          await context.Response.WriteAsJsonAsync(new { Message = $"{pathName} too many request" });
          return;
        }
        else
        {
          RateLimit[pathName] = (DateTime.Now, requestCount + 1);
          context.Response.Headers["X-Rate-Limit-Remaning-Count"] = (maxLimit - (requestCount + 1)).ToString();

          await _next(context);
        }
      }
      else
      {
       
    
        RateLimit[pathName] = (DateTime.Now, 1);
        context.Response.Headers["X-Rate-Limit-Remaning-Count"] = (maxLimit - 1).ToString();

        await _next(context);
      }
    }
  }
}
