using Microsoft.EntityFrameworkCore;

namespace GestorIncidencias;

public static class ListaDeIncidencias
{
    public static async Task<List<IncidenciaEnLista>> Consultar(
        Datos datos,
        TimeProvider reloj,
        EstadoIncidencia? estado,
        PrioridadIncidencia? prioridad,
        int? zonaId,
        int? tecnicoId)
    {
        var incidencias = datos.Incidencias.Include(i => i.Zona).Where(i => !i.Archivada);

        if (estado is not null)
            incidencias = incidencias.Where(i => i.Estado == estado);
        if (prioridad is not null)
            incidencias = incidencias.Where(i => i.Prioridad == prioridad);
        if (zonaId is not null)
            incidencias = incidencias.Where(i => i.ZonaId == zonaId);
        if (tecnicoId is not null)
            incidencias = incidencias.Where(i => i.TecnicoId == tecnicoId);

        var hoy = DateOnly.FromDateTime(reloj.GetUtcNow().UtcDateTime);
        var lista = new List<IncidenciaEnLista>();
        foreach (var incidencia in await incidencias.OrderByDescending(i => i.FechaApertura).ToListAsync())
        {
            var tecnico = await datos.Tecnicos.FirstOrDefaultAsync(t => t.Id == incidencia.TecnicoId);
            lista.Add(new IncidenciaEnLista(
                incidencia.Id, incidencia.Titulo, incidencia.Prioridad, incidencia.Estado, incidencia.Zona?.Nombre,
                incidencia.FechaApertura, tecnico?.Nombre, incidencia.Plazo, incidencia.FueraDePlazo(hoy)));
        }

        return lista;
    }
}
