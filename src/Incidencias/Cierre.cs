namespace GestorIncidencias;

/// <summary>
/// El técnico resuelve y la oficina cierra después de revisar el parte.
/// Cada método dice si el cambio de estado se puede hacer.
/// </summary>
public static class Cierre
{
    public static bool Resolver(Incidencia incidencia, DateTime ahora)
    {
        if (incidencia.Estado != EstadoIncidencia.Abierta)
            return false;

        incidencia.Estado = EstadoIncidencia.Resuelta;
        incidencia.FechaResolucion = ahora;
        return true;
    }

    public static bool Cerrar(Incidencia incidencia, DateTime ahora)
    {
        if (incidencia.Estado != EstadoIncidencia.Resuelta)
            return false;

        incidencia.Estado = EstadoIncidencia.Cerrada;
        incidencia.FechaCierre = ahora;
        return true;
    }

    public static bool Reabrir(Incidencia incidencia)
    {
        if (incidencia.Estado == EstadoIncidencia.Abierta)
            return false;

        incidencia.Estado = EstadoIncidencia.Abierta;
        incidencia.FechaResolucion = null;
        incidencia.FechaCierre = null;
        incidencia.Archivada = false;
        return true;
    }
}
