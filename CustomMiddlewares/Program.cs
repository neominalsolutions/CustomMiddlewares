using CustomMiddlewares.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var options = builder.Configuration["HeaderOptions:AllowedHeaderValues"];
// Hard Coded olarak kullanýmý
// middleware çalýþmasý için option girdik.
builder.Services.Configure<HeaderOptions>(builder.Configuration.GetSection("HeaderOptions"));

// uygulama genelinde tek instance yeterlidir.
//builder.Services.AddSingleton<HeaderOptions>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseMiddleware<RequestHeaderMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
