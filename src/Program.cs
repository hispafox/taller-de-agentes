using System.Text.Json.Serialization;
using GestorIncidencias;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddProblemDetails();
// Un cuerpo que no se puede leer es un 400 también en desarrollo, y sale con Problem Details.
builder.Services.Configure<RouteHandlerOptions>(opciones => opciones.ThrowOnBadRequest = false);
builder.Services.AddDbContext<Datos>((servicios, opciones) =>
    opciones.UseSqlite(servicios.GetRequiredService<IConfiguration>().GetConnectionString("Incidencias")));
builder.Services.ConfigureHttpJsonOptions(opciones =>
    opciones.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

using (var ambito = app.Services.CreateScope())
{
    var datos = ambito.ServiceProvider.GetRequiredService<Datos>();
    await datos.Database.MigrateAsync();
    await CargaDeEjemplo.Aplicar(datos, ambito.ServiceProvider.GetRequiredService<TimeProvider>());
}

app.UseExceptionHandler();
app.UseStatusCodePages();

app.MapearIncidencias();
app.MapearZonas();

app.Run();
