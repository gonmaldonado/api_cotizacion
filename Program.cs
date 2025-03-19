using api_cotizacion.Interfaces;
using api_cotizacion.services;

var builder = WebApplication.CreateBuilder(args);

// Registra la interfaz y su implementación
builder.Services.AddScoped<IBancoNacionService, BancoNacionService>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Detecta si está en desarrollo o producción
var isDevelopment = app.Environment.IsDevelopment();

// Si está en producción, usa el prefijo "/apicotizacion"
if (!isDevelopment)
{
    app.UsePathBase("/apicotizacion");
}

// Habilitar Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // Ajustar ruta del JSON dependiendo del entorno
    var swaggerJsonUrl = isDevelopment ? "/swagger/v1/swagger.json" : "/apicotizacion/swagger/v1/swagger.json";
    c.SwaggerEndpoint(swaggerJsonUrl, "API_COTIZACION V1");

    // Hacer que Swagger UI esté en la ruta correcta
    c.RoutePrefix = isDevelopment ? "swagger" : "apicotizacion/swagger";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
