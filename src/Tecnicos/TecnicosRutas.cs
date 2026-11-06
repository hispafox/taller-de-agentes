using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace GestorIncidencias;

public record TecnicoNuevo(string? Nombre);

public static class TecnicosRutas
{
    public static void MapearTecnicos(this IEndpointRouteBuilder rutas)
    {
        // SQLite ordena por bytes y dejaría a Óscar el último: el orden va en memoria, con el del español.
        var orden = StringComparer.Create(CultureInfo.GetCultureInfo("es-ES"), ignoreCase: true);

        rutas.MapGet("/tecnicos", async (Datos datos) =>
            (await datos.Tecnicos.Where(t => t.Activo).ToListAsync()).OrderBy(t => t.Nombre, orden).ToList());

        rutas.MapPost("/tecnicos", async (TecnicoNuevo cuerpo, Datos datos) =>
        {
            if (string.IsNullOrWhiteSpace(cuerpo.Nombre))
                return Errores.NombreObligatorio();

            var tecnico = new Tecnico { Nombre = cuerpo.Nombre };
            datos.Tecnicos.Add(tecnico);
            await datos.SaveChangesAsync();
            return Results.Created($"/tecnicos/{tecnico.Id}", tecnico);
        });
    }
}
