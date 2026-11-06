using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Incidencias.Pruebas;

public class MigracionesPruebas : IDisposable
{
    private readonly string fichero = Path.Combine(Path.GetTempPath(), $"migracion-{Guid.NewGuid():N}.db");

    private DbContextOptions<Datos> Opciones =>
        new DbContextOptionsBuilder<Datos>().UseSqlite($"Data Source={fichero}").Options;

    [Fact]
    public void Una_incidencia_se_puede_guardar_solo_con_el_titulo()
    {
        using var datos = new Datos(Opciones);
        var migrador = datos.GetService<IMigrator>();

        migrador.Migrate("Inicial");
        datos.Database.ExecuteSql($"INSERT INTO Incidencias (Titulo) VALUES ('Fuga en el garaje')");
        migrador.Migrate();

        var incidencia = datos.Incidencias.Single();
        Assert.Equal("Fuga en el garaje", incidencia.Titulo);
        Assert.Equal(EstadoIncidencia.Abierta, incidencia.Estado);
        Assert.Equal(PrioridadIncidencia.Normal, incidencia.Prioridad);
    }

    [Fact]
    public void C06_La_migracion_conserva_las_incidencias_que_ya_existen()
    {
        using var datos = new Datos(Opciones);
        var migrador = datos.GetService<IMigrator>();

        migrador.Migrate("Archivado");  // la última migración antes del cambio
        datos.Database.ExecuteSql($"INSERT INTO Incidencias (Titulo) VALUES ('Fuga en el garaje')");
        migrador.Migrate();             // hasta la última

        var incidencia = datos.Incidencias.Single();
        Assert.Equal("Fuga en el garaje", incidencia.Titulo);
        Assert.Null(incidencia.TecnicoId);
        Assert.Null(incidencia.Plazo);
    }

    [Fact]
    public void El_modelo_no_tiene_cambios_sin_migracion()
    {
        using var datos = new Datos(Opciones);

        Assert.False(datos.Database.HasPendingModelChanges());
    }

    public void Dispose()
    {
        SqliteConnection.ClearAllPools();
        File.Delete(fichero);
        GC.SuppressFinalize(this);
    }
}
