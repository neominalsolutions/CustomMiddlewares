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
// Hard Coded olarak kullanýmý
// middleware çalýþmasý için option girdik.
builder.Services.Configure<HeaderOptions>(builder.Configuration.GetSection("HeaderOptions"));

// uygulama genelinde tek instance yeterlidir.
//builder.Services.AddSingleton<HeaderOptions>();



// Factory Tanýmý yaptýðýmýz için AddTransient yeni bir middleware instance aldýk
// ConfigureServices ile ilgi sýnýfýn instance göndereceðiz
builder.Services.AddTransient<IPAddressFilteringMiddleware>();

// Konfigürasyon ddosyasýnda okumadðýmýz için ise elimiz ile yeni bir static instance tanýmladýk buda configure services 2. kullanýmý oldu.
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
  // Sadece MVC gibi UI olan projlerde developöment mode tercih edilebilir.
  //app.UseDeveloperExceptionPage();
}

app.UseMiddleware<RequestHeaderMiddleware>();
app.UseMiddleware<IPAddressFilteringMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
