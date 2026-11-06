using Microsoft.EntityFrameworkCore;

namespace GestorIncidencias;

public static class BusquedaServicio
{
    public static async Task<List<IncidenciaEnLista>> Buscar(Datos datos, TimeProvider reloj, string texto)
    {
        var patron = $"%{texto}%";
        var hoy = DateOnly.FromDateTime(reloj.GetUtcNow().UtcDateTime);

        return await datos.Incidencias
            .Where(i => EF.Functions.Like(i.Titulo, patron) || EF.Functions.Like(i.Descripcion!, patron))
            .OrderByDescending(i => i.FechaApertura)
            .Select(i => new IncidenciaEnLista(
                i.Id, i.Titulo, i.Prioridad, i.Estado, i.Zona!.Nombre, i.FechaApertura,
                i.Tecnico!.Nombre, i.Plazo, i.Plazo < hoy && i.Estado != EstadoIncidencia.Cerrada))
            .ToListAsync();
    }
}
