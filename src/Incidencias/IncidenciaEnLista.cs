namespace GestorIncidencias;

public record IncidenciaEnLista(
    int Id,
    string Titulo,
    PrioridadIncidencia Prioridad,
    EstadoIncidencia Estado,
    string? Zona,
    DateTime FechaApertura);

public record IncidenciaDetalle(
    int Id,
    string Titulo,
    string? Descripcion,
    string? Observaciones,
    PrioridadIncidencia Prioridad,
    EstadoIncidencia Estado,
    int? ZonaId,
    string? Zona,
    DateTime FechaApertura,
    DateTime? FechaResolucion,
    DateTime? FechaCierre,
    bool Archivada)
{
    public static IncidenciaDetalle Desde(Incidencia i) => new(
        i.Id, i.Titulo, i.Descripcion, i.Observaciones, i.Prioridad, i.Estado,
        i.ZonaId, i.Zona?.Nombre, i.FechaApertura, i.FechaResolucion, i.FechaCierre, i.Archivada);
}

public record DatosDeIncidencia(
    string? Titulo,
    string? Descripcion,
    string? Observaciones,
    PrioridadIncidencia Prioridad,
    int? ZonaId);
