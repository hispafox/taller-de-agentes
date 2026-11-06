using Microsoft.EntityFrameworkCore;

namespace GestorIncidencias;

public static class IncidenciasRutas
{
    public static void MapearIncidencias(this IEndpointRouteBuilder rutas)
    {
        var incidencias = rutas.MapGroup("/incidencias");

        incidencias.MapGet("/", ListaDeIncidencias.Consultar);
        incidencias.MapGet("/buscar", BusquedaServicio.Buscar);
        incidencias.MapGet("/{id:int}", Leer);
        incidencias.MapPost("/", Abrir);
        incidencias.MapPut("/{id:int}", Editar);
        incidencias.MapPost("/{id:int}/resolver", Resolver);
        incidencias.MapPost("/{id:int}/cerrar", Cerrar);
        incidencias.MapPost("/{id:int}/reabrir", Reabrir);
        incidencias.MapPost("/{id:int}/archivar", Archivar);
        incidencias.MapPost("/archivar-cerradas", ArchivarCerradas);
    }

    private static async Task<IResult> Leer(int id, Datos datos)
    {
        var incidencia = await Buscar(datos, id);
        return incidencia is null
            ? Errores.IncidenciaNoExiste(id)
            : Results.Ok(IncidenciaDetalle.Desde(incidencia));
    }

    private static async Task<IResult> Abrir(DatosDeIncidencia cuerpo, Datos datos, TimeProvider reloj)
    {
        if (await Validar(cuerpo, datos) is { } error)
            return error;

        var incidencia = new Incidencia
        {
            Titulo = cuerpo.Titulo!,
            Descripcion = cuerpo.Descripcion,
            Observaciones = cuerpo.Observaciones,
            Prioridad = cuerpo.Prioridad,
            Estado = EstadoIncidencia.Abierta,
            ZonaId = cuerpo.ZonaId,
            FechaApertura = reloj.GetUtcNow().UtcDateTime,
        };
        datos.Incidencias.Add(incidencia);
        await datos.SaveChangesAsync();

        var guardada = await Buscar(datos, incidencia.Id);
        return Results.Created($"/incidencias/{incidencia.Id}", IncidenciaDetalle.Desde(guardada!));
    }

    private static async Task<IResult> Editar(int id, DatosDeIncidencia cuerpo, Datos datos)
    {
        var incidencia = await Buscar(datos, id);
        if (incidencia is null)
            return Errores.IncidenciaNoExiste(id);
        if (incidencia.Estado == EstadoIncidencia.Cerrada)
            return Errores.EstadoNoPermitido("editar", incidencia.Estado);
        if (await Validar(cuerpo, datos) is { } error)
            return error;

        incidencia.Titulo = cuerpo.Titulo!;
        incidencia.Descripcion = cuerpo.Descripcion;
        incidencia.Observaciones = cuerpo.Observaciones;
        incidencia.Prioridad = cuerpo.Prioridad;
        incidencia.ZonaId = cuerpo.ZonaId;
        await datos.SaveChangesAsync();

        var guardada = await Buscar(datos, id);
        return Results.Ok(IncidenciaDetalle.Desde(guardada!));
    }

    private static Task<IResult> Resolver(int id, Datos datos, TimeProvider reloj) =>
        CambiarEstado(id, datos, "resolver", i => Cierre.Resolver(i, reloj.GetUtcNow().UtcDateTime));

    private static Task<IResult> Cerrar(int id, Datos datos, TimeProvider reloj) =>
        CambiarEstado(id, datos, "cerrar", i => Cierre.Cerrar(i, reloj.GetUtcNow().UtcDateTime));

    private static Task<IResult> Reabrir(int id, Datos datos) =>
        CambiarEstado(id, datos, "reabrir", Cierre.Reabrir);

    private static Task<IResult> Archivar(int id, Datos datos) =>
        CambiarEstado(id, datos, "archivar", ArchivadoServicio.Archivar);

    private static async Task<IResult> ArchivarCerradas(Datos datos, TimeProvider reloj) =>
        Results.Ok(new { archivadas = await ArchivadoServicio.ArchivarCerradas(datos, reloj) });

    private static async Task<IResult> CambiarEstado(int id, Datos datos, string accion, Func<Incidencia, bool> cambio)
    {
        var incidencia = await Buscar(datos, id);
        if (incidencia is null)
            return Errores.IncidenciaNoExiste(id);
        if (!cambio(incidencia))
            return Errores.EstadoNoPermitido(accion, incidencia.Estado);

        await datos.SaveChangesAsync();
        return Results.Ok(IncidenciaDetalle.Desde(incidencia));
    }

    private static Task<Incidencia?> Buscar(Datos datos, int id) =>
        datos.Incidencias.Include(i => i.Zona).FirstOrDefaultAsync(i => i.Id == id);

    private static async Task<IResult?> Validar(DatosDeIncidencia cuerpo, Datos datos)
    {
        if (string.IsNullOrWhiteSpace(cuerpo.Titulo))
            return Errores.TituloObligatorio();
        if (cuerpo.ZonaId is { } zonaId && !await datos.Zonas.AnyAsync(z => z.Id == zonaId))
            return Errores.ZonaNoExiste(zonaId);
        return null;
    }
}
