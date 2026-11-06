using Microsoft.EntityFrameworkCore;

namespace GestorIncidencias;

public record Asignacion(int? TecnicoId, DateOnly? Plazo);

public record TecnicoResumen(int Id, string Nombre);

public record IncidenciaAsignada(int Id, TecnicoResumen? Tecnico, DateOnly? Plazo, bool FueraDePlazo);

public static class AsignacionRutas
{
    public static async Task<IResult> Asignar(int id, Asignacion cuerpo, Datos datos, TimeProvider reloj)
    {
        var incidencia = await datos.Incidencias.FirstOrDefaultAsync(i => i.Id == id);
        if (incidencia is null)
            return Errores.IncidenciaNoExiste(id);
        if (incidencia.Estado == EstadoIncidencia.Cerrada)
            return Errores.EstadoNoPermitido("asignar", incidencia.Estado);

        Tecnico? tecnico = null;
        if (cuerpo.TecnicoId is { } tecnicoId)
        {
            tecnico = await datos.Tecnicos.FirstOrDefaultAsync(t => t.Id == tecnicoId);
            if (tecnico is null)
                return Errores.TecnicoNoExiste(tecnicoId);
        }

        incidencia.TecnicoId = cuerpo.TecnicoId;
        incidencia.Plazo = cuerpo.Plazo;
        await datos.SaveChangesAsync();

        var hoy = DateOnly.FromDateTime(reloj.GetUtcNow().UtcDateTime);
        var resumen = tecnico is null ? null : new TecnicoResumen(tecnico.Id, tecnico.Nombre);
        return Results.Ok(new IncidenciaAsignada(id, resumen, incidencia.Plazo, incidencia.FueraDePlazo(hoy)));
    }
}
