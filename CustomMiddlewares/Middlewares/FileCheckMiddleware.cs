using System.Linq;

namespace CustomMiddlewares.Middlewares
{
  public class FileCheckMiddleware
  {
    private readonly RequestDelegate _next;

    public FileCheckMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      var blockedfileExtensions = new List<string>() { ".zip", ".exe", ".sql", ".dll", ".sh", ".cmd", ".bat",".jar",".rar" };

    

      if(context.Request.Method == HttpMethod.Post.ToString() && context.Request.ContentType != null && context.Request.ContentType.StartsWith("multipart/form-data")) {

        foreach (var file in context.Request.Form.Files)
        {
          var filePathExtension = Path.GetExtension(file.FileName);

          if (blockedfileExtensions.Contains(filePathExtension))
          {
            //context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { message = "yaksaklı dosya uzantıs" });
            return;
          }
        }

        await _next(context);

      }
      else
      {
        await _next(context);
      }

    

    } 
  }
}
