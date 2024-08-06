using System.Net;

namespace CustomMiddlewares.Middlewares
{
  public class IPFilteringOptions
  {
    // Aynı referansa sahip değerleri kontrol etmek için HashSet 
    public HashSet<IPAddress> BlackList = new HashSet<IPAddress>();
    public HashSet<IPAddress> WhiteList = new HashSet<IPAddress>();
  }
  // BlackList ve WhiteList özelliklerine göre IP adres bloklaması yapacağımız middleware // ConfigureServices Yöntemi
  public class IPAddressFilteringMiddleware:IMiddleware
  {

    private readonly IPFilteringOptions _options;
    //private readonly RequestDelegate next;

    public IPAddressFilteringMiddleware(IPFilteringOptions options)
    {
      _options = options;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
      var IPAddress = context.Connection.RemoteIpAddress;
      var localIPAddress = context.Connection.LocalIpAddress;


      if (_options.BlackList.Any() && _options.BlackList.Contains(IPAddress))
      {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.WriteAsJsonAsync(new { Message = "Bu IP adresi Bloklandı" });
        return;
      }
      else if(_options.WhiteList.Any() && _options.WhiteList.Contains(IPAddress))
      {
        context.Response.StatusCode = StatusCodes.Status200OK;
        await next(context);
      }
      else
      {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.WriteAsJsonAsync(new { Message = "Bu IP adresi Bloklandı" });
        return;
      }
    }
  }
}
