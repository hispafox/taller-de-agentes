using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;

namespace Incidencias.Pruebas;

/// <summary>
/// La API en memoria, con su propia base de datos y un reloj parado en el 20 de noviembre de 2026.
/// </summary>
public class FabricaApi : WebApplicationFactory<Program>
{
    private readonly string fichero = Path.Combine(Path.GetTempPath(), $"incidencias-{Guid.NewGuid():N}.db");

    public FakeTimeProvider Reloj { get; } =
        new(new DateTimeOffset(2026, 11, 20, 10, 0, 0, TimeSpan.Zero));

    protected override void ConfigureWebHost(IWebHostBuilder constructor)
    {
        constructor.UseSetting("ConnectionStrings:Incidencias", $"Data Source={fichero}");
        constructor.ConfigureTestServices(servicios =>
            servicios.AddSingleton<TimeProvider>(Reloj));
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        SqliteConnection.ClearAllPools();
        File.Delete(fichero);
    }
}

/// <summary>
/// Cada prueba arranca con la carga de ejemplo recién hecha, así ninguna depende de otra.
/// </summary>
public abstract class PruebaDeApi : IDisposable
{
    protected readonly FabricaApi fabrica = new();
    protected readonly HttpClient cliente;

    protected PruebaDeApi() => cliente = fabrica.CreateClient();

    public void Dispose()
    {
        cliente.Dispose();
        fabrica.Dispose();
        GC.SuppressFinalize(this);
    }
}
