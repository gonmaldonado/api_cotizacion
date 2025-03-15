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

app.UsePathBase("/apicotizacion");

// Habilitar Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // Ajustar ruta del JSON incluyendo la carpeta
    c.SwaggerEndpoint("/apicotizacion/swagger/v1/swagger.json", "API_COTIZACION V1");

    // Hacer que Swagger UI esté en /apicotizacion/swagger
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
