using Assistt.BLL.Layer;
using System.ComponentModel.DataAnnotations;

namespace CustomMiddlewares.Middlewares
{
  // Global Error uygulama genelinde yönetip logladığımız middleware
  public class ErrorHandlingMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
      _next = next;
      _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      var endpoint = context.GetEndpoint();

      // Validation Exception A 422 status code indicates that the server was unable to process the request because it contains invalid data. kodu döndürebilir.
      try
      {
        await _next(context);
      }
      catch(CustomException ex)
      {
        _logger.LogError(ex,ex.Message);
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new { message = ex.Message });

        return;
      }
      catch(ValidationException ex) // Validation Exception
      {
        _logger.LogError(ex, ex.Message);
        context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
        await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        return;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, ex.Message);
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new { message = "Uygulamada beklemedik bir hata oluştu" });
        return;
      }
    }
  }
}
