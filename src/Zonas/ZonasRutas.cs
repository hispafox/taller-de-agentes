using Microsoft.EntityFrameworkCore;

namespace GestorIncidencias;

public static class ZonasRutas
{
    public static void MapearZonas(this IEndpointRouteBuilder rutas)
    {
        rutas.MapGet("/zonas", async (Datos datos) =>
            await datos.Zonas.OrderBy(z => z.Nombre).ToListAsync());
    }
}
