namespace GestorIncidencias;

public enum EstadoIncidencia
{
    Abierta,
    Resuelta,
    Cerrada,
}

public enum PrioridadIncidencia
{
    Normal,
    Alta,
    Urgente,
}

public class Incidencia
{
    public int Id { get; set; }
    public required string Titulo { get; set; }
    public string? Descripcion { get; set; }
    public string? Observaciones { get; set; }
    public PrioridadIncidencia Prioridad { get; set; }
    public EstadoIncidencia Estado { get; set; }
    public DateTime FechaApertura { get; set; }
    public DateTime? FechaResolucion { get; set; }
    public DateTime? FechaCierre { get; set; }
    public bool Archivada { get; set; }
    public int? ZonaId { get; set; }
    public Zona? Zona { get; set; }
}
