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

// Habilitar Swagger
app.UseSwagger();

if (!builder.Configuration.GetValue<bool>("ProdEnvironment"))
{
    app.UseSwaggerUI(c =>
    {
        var swaggerJsonUrl = "/swagger/v1/swagger.json";
        c.SwaggerEndpoint(swaggerJsonUrl, "API_COTIZACION V1");
        c.RoutePrefix = "swagger";
        c.DefaultModelsExpandDepth(-1);
    });

}
else
{
    app.UsePathBase("/apicotizacion");
    app.UseSwaggerUI(c =>
    {
        var swaggerJsonUrl = "/apicotizacion/swagger/v1/swagger.json";
        c.SwaggerEndpoint(swaggerJsonUrl, "API_COTIZACION V1");
        c.DefaultModelsExpandDepth(-1);
    });

}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
