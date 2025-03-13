using api_cotizacion.Interfaces;
using api_cotizacion.services;

var builder = WebApplication.CreateBuilder(args);

// Registra la interfaz y su implementaci�n
builder.Services.AddScoped<IBancoNacionService, BancoNacionService>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API_COTIZACION V1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
