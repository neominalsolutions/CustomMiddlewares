namespace CustomMiddlewares.Middlewares
{
  // Class veya method üzerinde bu attribute kullanılabilir.
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
  public class SampleAttribute:Attribute
  {}

  public class AttributeBasedMiddleware
  {
    private readonly RequestDelegate _next;

    public AttributeBasedMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      var endpoint = context.GetEndpoint();
      var attributes = endpoint.Metadata.GetOrderedMetadata<SampleAttribute>();

      if (attributes.Any())
      {
        // ilgili middleware işlemleri
        await _next(context);
      }
      else
      {
        await context.Response.WriteAsync("Attribute Kullanılmamış");
        return;
      }

    }
  }
}
