namespace GestorIncidencias;

/// <summary>
/// Los errores de la API, todos con Problem Details y un texto que entiende quien usa la aplicación.
/// Un dato del cuerpo que no vale da 400; una ruta que apunta a algo que no existe, 404;
/// una petición que choca con el estado de la incidencia, 409.
/// </summary>
public static class Errores
{
    public static IResult ZonaNoExiste(int zonaId) => Results.Problem(
        title: "La zona no existe",
        detail: $"No hay ninguna zona con el identificador {zonaId}.",
        statusCode: StatusCodes.Status400BadRequest);

    public static IResult TecnicoNoExiste(int tecnicoId) => Results.Problem(
        title: "El técnico no existe",
        detail: $"No hay ningún técnico con el identificador {tecnicoId}.",
        statusCode: StatusCodes.Status400BadRequest);

    public static IResult TituloObligatorio() => Results.Problem(
        title: "Falta el título",
        detail: "Una incidencia necesita un título.",
        statusCode: StatusCodes.Status400BadRequest);

    public static IResult NombreObligatorio() => Results.Problem(
        title: "Falta el nombre",
        detail: "Un técnico necesita un nombre.",
        statusCode: StatusCodes.Status400BadRequest);

    public static IResult IncidenciaNoExiste(int id) => Results.Problem(
        title: "La incidencia no existe",
        detail: $"No hay ninguna incidencia con el identificador {id}.",
        statusCode: StatusCodes.Status404NotFound);

    public static IResult EstadoNoPermitido(string accion, EstadoIncidencia estado) => Results.Problem(
        title: "No se puede hacer en este estado",
        detail: $"No se puede {accion} una incidencia {estado.ToString().ToLowerInvariant()}.",
        statusCode: StatusCodes.Status409Conflict);
}
