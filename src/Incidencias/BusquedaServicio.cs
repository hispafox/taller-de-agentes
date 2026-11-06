using Microsoft.EntityFrameworkCore;

namespace GestorIncidencias;

public static class BusquedaServicio
{
    public static async Task<List<IncidenciaEnLista>> Buscar(Datos datos, string texto)
    {
        var patron = $"%{texto}%";

        return await datos.Incidencias
            .Where(i => EF.Functions.Like(i.Titulo, patron) || EF.Functions.Like(i.Descripcion!, patron))
            .OrderByDescending(i => i.FechaApertura)
            .Select(i => new IncidenciaEnLista(i.Id, i.Titulo, i.Prioridad, i.Estado, i.Zona!.Nombre, i.FechaApertura))
            .ToListAsync();
    }
}
