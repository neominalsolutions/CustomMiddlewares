using Microsoft.Extensions.Options;

namespace CustomMiddlewares.Middlewares
{
  // Senaryo bazı durumlar client ile server haberleşirken bazı özel header değerleri kullanabilir
  // ilgili endpointler bu header değerlerine sahip clientlar tarafında tetiklenebilir bunun dışındaki tüm istekler için bir yetkilendirme hatası fırlatılabilir.

  public class HeaderOptions
  {
    public List<string> AllowedHeaderValues = new();
  }
  public class RequestHeaderMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly IOptions<HeaderOptions> _options;

    public RequestHeaderMiddleware(RequestDelegate next, IOptions<HeaderOptions> headerOptions)
    {
      _next = next;
      _options = headerOptions;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      var headerValue = context.Request.Headers["X-Client"].FirstOrDefault();

     
        // Request izin verilen header değerlerine sahip mi?
        if (!string.IsNullOrEmpty(headerValue) && _options.Value.AllowedHeaderValues.Contains(headerValue))
        { //süreci devam ettir.
          await _next(context);
        }
        else
        {
          context.Response.StatusCode = StatusCodes.Status403Forbidden;
          await context.Response.WriteAsJsonAsync(new { Message = "Header is not valid" });
          return;
        }
      
    }
  }
}
