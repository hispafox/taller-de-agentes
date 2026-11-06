using Microsoft.EntityFrameworkCore;

namespace GestorIncidencias;

public static class ListaDeIncidencias
{
    public static async Task<List<IncidenciaEnLista>> Consultar(
        Datos datos,
        EstadoIncidencia? estado,
        PrioridadIncidencia? prioridad,
        int? zonaId)
    {
        var incidencias = datos.Incidencias.Where(i => !i.Archivada);

        if (estado is not null)
            incidencias = incidencias.Where(i => i.Estado == estado);
        if (prioridad is not null)
            incidencias = incidencias.Where(i => i.Prioridad == prioridad);
        if (zonaId is not null)
            incidencias = incidencias.Where(i => i.ZonaId == zonaId);

        return await incidencias
            .OrderByDescending(i => i.FechaApertura)
            .Select(i => new IncidenciaEnLista(i.Id, i.Titulo, i.Prioridad, i.Estado, i.Zona!.Nombre, i.FechaApertura))
            .ToListAsync();
    }
}
