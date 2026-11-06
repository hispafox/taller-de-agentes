using Microsoft.EntityFrameworkCore;

namespace GestorIncidencias;

/// <summary>
/// Datos de ejemplo para clase, siempre los mismos. Las fechas van relativas a hoy.
/// La incidencia 1 sigue abierta: las pruebas y los ejemplos del curso parten de ella.
/// </summary>
public static class CargaDeEjemplo
{
    public static async Task Aplicar(Datos datos, TimeProvider reloj)
    {
        if (await datos.Zonas.AnyAsync())
            return;

        var hoy = reloj.GetUtcNow().UtcDateTime;
        var norte = new Zona { Nombre = "Norte" };
        var centro = new Zona { Nombre = "Centro" };
        var sur = new Zona { Nombre = "Sur" };
        datos.Zonas.AddRange(norte, centro, sur);

        var rafa = new Tecnico { Nombre = "Rafa" };
        var javi = new Tecnico { Nombre = "Javi" };
        var oscar = new Tecnico { Nombre = "Óscar" };
        var manolo = new Tecnico { Nombre = "Manolo" };
        datos.Tecnicos.AddRange(rafa, javi, oscar, manolo);
        await datos.SaveChangesAsync();

        Incidencia Abierta(string titulo, string descripcion, PrioridadIncidencia prioridad, Zona zona, int diasAbierta) => new()
        {
            Titulo = titulo,
            Descripcion = descripcion,
            Prioridad = prioridad,
            Estado = EstadoIncidencia.Abierta,
            Zona = zona,
            FechaApertura = hoy.AddDays(-diasAbierta),
        };

        Incidencia Resuelta(string titulo, string descripcion, PrioridadIncidencia prioridad, Zona zona, int diasAbierta, int diasResuelta)
        {
            var incidencia = Abierta(titulo, descripcion, prioridad, zona, diasAbierta);
            incidencia.Estado = EstadoIncidencia.Resuelta;
            incidencia.FechaResolucion = hoy.AddDays(-diasResuelta);
            return incidencia;
        }

        Incidencia Cerrada(string titulo, string descripcion, PrioridadIncidencia prioridad, Zona zona, int diasAbierta, int diasCerrada)
        {
            var incidencia = Resuelta(titulo, descripcion, prioridad, zona, diasAbierta, diasCerrada + 1);
            incidencia.Estado = EstadoIncidencia.Cerrada;
            incidencia.FechaCierre = hoy.AddDays(-diasCerrada);
            return incidencia;
        }

        // Una a una, para que la incidencia 1 sea siempre la primera.
        Incidencia[] incidencias =
        [
            Abierta("Fuga en el garaje, plaza 14", "Mirador del Parque. Agua en el suelo junto a la plaza 14.", PrioridadIncidencia.Urgente, norte, 11),
            Abierta("Portero automático no abre", "Las Acacias 12. El portero suena y la puerta no abre.", PrioridadIncidencia.Normal, centro, 4),
            Abierta("Gotera en el ático", "Olmos 3. Mancha nueva en el techo del rellano.", PrioridadIncidencia.Alta, sur, 8),
            Abierta("Barandilla suelta en la escalera", "Ronda Norte 40. Pendiente de soldador.", PrioridadIncidencia.Alta, norte, 6),
            Abierta("Antena sin señal", "Pinar 8. Sin señal en todo el bloque.", PrioridadIncidencia.Normal, centro, 2),
            Resuelta("Grupo de presión sin agua", "Ronda Norte 40. Falta revisar el parte.", PrioridadIncidencia.Urgente, norte, 5, 3),
            Resuelta("Humedad en la pared del portal", "Olmos 3. Falta revisar el parte.", PrioridadIncidencia.Normal, sur, 9, 2),
            Cerrada("Fuga en el garaje, plaza 7", "Pinar 8. Junta cambiada.", PrioridadIncidencia.Urgente, centro, 20, 12),
            Cerrada("Caldera con error E04", "Pinar 8. Sonda sustituida.", PrioridadIncidencia.Urgente, centro, 35, 33),
            Cerrada("Puerta de garaje no cierra", "Mirador del Parque. Fotocélula ajustada.", PrioridadIncidencia.Urgente, norte, 70, 68),
            Cerrada("Luz de escalera fundida", "Las Acacias 12. Lámpara cambiada.", PrioridadIncidencia.Normal, centro, 120, 118),
        ];

        var hoyFecha = DateOnly.FromDateTime(hoy);
        (incidencias[0].Tecnico, incidencias[0].Plazo) = (rafa, hoyFecha.AddDays(-1));
        (incidencias[1].Tecnico, incidencias[1].Plazo) = (javi, hoyFecha.AddDays(3));
        (incidencias[2].Tecnico, incidencias[2].Plazo) = (oscar, hoyFecha);

        foreach (var incidencia in incidencias)
        {
            datos.Incidencias.Add(incidencia);
            await datos.SaveChangesAsync();
        }
    }
}
