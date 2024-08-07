namespace CustomMiddlewares.Middlewares
{
  public class SecurityHeaderMiddleware
  {
    private readonly RequestDelegate _next;

    public SecurityHeaderMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self'; https://www.a.com; style-src 'self' ");

      context.Response.Headers.Add("X-Xss-Protection","1; mode=block");
      context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
      context.Response.Headers.Add("X-Frame-Options", "DENY");
      context.Response.Headers.Add("Referer-Policy", "no-referrer");
      context.Response.Headers.Add("Feature-Policy", "camera 'none;'" + "geolocation 'none'" + "microphone 'none';" + "usb 'none'");

      await _next(context);
    }
  }
}
