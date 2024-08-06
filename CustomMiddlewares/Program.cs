using CustomMiddlewares.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//var options = builder.Configuration["HeaderOptions:AllowedHeaderValues"];
//var options2 = builder.Configuration.GetSection("HeaderOptions").Value;
// Hard Coded olarak kullan�m�
// middleware �al��mas� i�in option girdik.
builder.Services.Configure<HeaderOptions>(builder.Configuration.GetSection("HeaderOptions"));

// uygulama genelinde tek instance yeterlidir.
//builder.Services.AddSingleton<HeaderOptions>();



// Factory Tan�m� yapt���m�z i�in AddTransient yeni bir middleware instance ald�k
// ConfigureServices ile ilgi s�n�f�n instance g�nderece�iz
builder.Services.AddTransient<IPAddressFilteringMiddleware>();

// Konfig�rasyon ddosyas�nda okumad��m�z i�in ise elimiz ile yeni bir static instance tan�mlad�k buda configure services 2. kullan�m� oldu.
builder.Services.AddSingleton(new IPFilteringOptions
{
  BlackList = new HashSet<System.Net.IPAddress> { IPAddress.Parse("127.0.0.1"), },
  WhiteList = new HashSet<System.Net.IPAddress> { IPAddress.Parse("::1"), },
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
  // Sadece MVC gibi UI olan projlerde develop�ment mode tercih edilebilir.
  //app.UseDeveloperExceptionPage();
}

app.UseMiddleware<RequestHeaderMiddleware>();
app.UseMiddleware<IPAddressFilteringMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
