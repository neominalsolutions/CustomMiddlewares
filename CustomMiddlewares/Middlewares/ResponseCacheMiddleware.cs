using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.IO.Pipelines;

namespace CustomMiddlewares.Middlewares
{
  // Get isteklerinde sadece çalışsın
  // Belirli bir süreşğine response cache alsın
  // Cahce attribute yoksa Resposne cachelemeden süreci devam ettirsin

  [AttributeUsage(AttributeTargets.Method)]
  public class CacheAttribute:Attribute
  {
    public int Duration { get; init; }

    public CacheAttribute(int duration)
    {
      Duration = duration;
    }
  }

  public class ResponseCacheMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;

    public ResponseCacheMiddleware(RequestDelegate next, IMemoryCache cache)
    {
      _next = next;
      _cache = cache;
    }

    public async Task InvokeAsync(HttpContext context)
    {

     


      var endpoint = context.GetEndpoint();
      var attributes = endpoint.Metadata.GetOrderedMetadata<CacheAttribute>();

      var path = context.Request.Path;


      if (attributes.Any())
      {
        var options = attributes.First();
        var responseBody = context.Response.Body;

        if(_cache.TryGetValue(path, out var cacheResponse))
        {
          context.Response.ContentType = "application/json";
          await context.Response.WriteAsJsonAsync(cacheResponse);
         
        }
        else
        {
          using (var stream = new MemoryStream())
          {
            // response body okuma işlemlerini memory stream içinde yaptık.
            // Context Response Body Stream'e kopyaladık
            context.Response.Body = stream;

            await _next(context);


            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var responseBodyString = new StreamReader(context.Response.Body).ReadToEnd();

            _cache.Set(path, responseBodyString, TimeSpan.FromMinutes(options.Duration));

          
            // streamdeki sonbilgiyi responseBody kopyaladık.
            await stream.CopyToAsync(responseBody);
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(responseBodyString);


          }
        }
      } 
      else
      {
        await _next(context);
      }
    }


  }
}
