using Assistt.BLL.Layer.Services;
using CustomMiddlewares.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(opt =>
{
  opt.AddServerHeader = false;
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddCors(opt =>
{
  opt.AddPolicy("Default", policy =>
  {
    //{
    //  policy.WithHeaders("GET", "POST");
    //  policy.WithHeaders("");
    policy.WithOrigins("100.100.10.10", "www.a.com");
  });

  opt.AddPolicy("Default2", policy =>
  {
    //{
    //  policy.WithHeaders("GET", "POST");
    //  policy.WithHeaders("");
    policy.WithOrigins("100.100.10.10", "www.a.com");
  });
});


//var options = builder.Configuration["HeaderOptions:AllowedHeaderValues"];
//var options2 = builder.Configuration.GetSection("HeaderOptions").Value;
// Hard Coded olarak kullan�m�
// middleware �al��mas� i�in option girdik.
builder.Services.Configure<HeaderOptions>(builder.Configuration.GetSection("HeaderOptions"));

// uygulama genelinde tek instance yeterlidir.
//builder.Services.AddSingleton<HeaderOptions>();



// Factory Tan�m� yapt���m�z i�in AddTransient yeni bir middleware instance ald�k
// ConfigureServices ile ilgi s�n�f�n instance g�nderece�iz
//builder.Services.AddTransient<IPAddressFilteringMiddleware>();

// Konfig�rasyon ddosyas�nda okumad��m�z i�in ise elimiz ile yeni bir static instance tan�mlad�k buda configure services 2. kullan�m� oldu.
builder.Services.AddSingleton(new IPFilteringOptions
{
  BlackList = new HashSet<System.Net.IPAddress> { IPAddress.Parse("127.0.0.1"), },
  WhiteList = new HashSet<System.Net.IPAddress> { IPAddress.Parse("::1"), },
});


builder.Services.AddScoped<ISampleService, SampleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
  // Sadece MVC gibi UI olan projlerde develop�ment mode tercih edilebilir.
  //app.UseDeveloperExceptionPage();
}




app.UseHsts();



//app.UseMiddleware<RequestHeaderMiddleware>();
//app.UseMiddleware<IPAddressFilteringMiddleware>();
//app.UseMiddleware<ErrorHandlingMiddleware>();
//app.UseMiddleware<AttributeBasedMiddleware>();

// Kendi hata midllewarelerinizi b�t�n middleware �zerinde tan�mlay�n.
//app.UseMiddleware<ErrorHandlingMiddleware>();
//app.UseMiddleware<ResponseCacheMiddleware>();
//app.UseMiddleware<FileCheckMiddleware>();

//app.UseMiddleware<CircuitBrakerMiddleware>();
//app.UseMiddleware<RateLimitingMiddleware>();



app.UseHttpsRedirection();
//app.UseMiddleware<SecurityHeaderMiddleware>();
app.UseCors("Default");

// Di�er Middlewareler


app.UseAuthorization();

app.MapControllers();

//app.Use(async (context, next) =>
//{
//  if (context.Request.Path.StartsWithSegments("/api/clients"))
//  {
//    app.UseCors("Default");

//    await next();
//  }

//});


//app.MapWhen(x => x.Request.Path.StartsWithSegments("/api/clients"), async (app1) =>
//{

//  app1.UseCors("Default2");

//  Console.WriteLine("Buras�");

//  app.Run();



//});


app.Run();
