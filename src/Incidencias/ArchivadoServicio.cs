using Microsoft.EntityFrameworkCore;

namespace GestorIncidencias;

public static class ArchivadoServicio
{
    public const int DiasHastaArchivar = 90;

    public static bool Archivar(Incidencia incidencia)
    {
        if (incidencia.Estado != EstadoIncidencia.Cerrada)
            return false;

        incidencia.Archivada = true;
        return true;
    }

    public static async Task<int> ArchivarCerradas(Datos datos, TimeProvider reloj)
    {
        var limite = reloj.GetUtcNow().UtcDateTime.AddDays(-DiasHastaArchivar);

        return await datos.Incidencias
            .Where(i => i.Estado == EstadoIncidencia.Cerrada && !i.Archivada && i.FechaCierre <= limite)
            .ExecuteUpdateAsync(cambio => cambio.SetProperty(i => i.Archivada, true));
    }
}
