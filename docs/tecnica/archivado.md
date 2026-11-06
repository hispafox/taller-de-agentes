# Archivado de incidencias

## Arquitectura

El archivado vive en la API. La marca es el campo `Archivada` de la incidencia (`src/Incidencias/Incidencia.cs#Archivada`), y la lógica, en `src/Incidencias/ArchivadoServicio.cs#ArchivarCerradas`. Las rutas están en `src/Incidencias/IncidenciasRutas.cs#MapearIncidencias`.

## Endpoints

| Ruta | Qué hace | Cuándo falla |
|---|---|---|
| `POST /incidencias/{id}/archivar` | Archiva una incidencia cerrada (`src/Incidencias/ArchivadoServicio.cs#Archivar`) | 404 si no existe, 409 si no está cerrada |
| `POST /incidencias/archivar-cerradas` | Archiva las cerradas que han pasado el plazo y devuelve cuántas (`src/Incidencias/ArchivadoServicio.cs#ArchivarCerradas`) | |
| `POST /incidencias/{id}/reabrir` | Reabre una incidencia, y deja de estar archivada (`src/Incidencias/Cierre.cs#Reabrir`) | 404 si no existe, 409 si ya está abierta |

## Modelo de datos

Un campo booleano en la tabla `Incidencias`. No hay tabla aparte: una incidencia archivada sigue en su sitio.

## Reglas de negocio

Una incidencia cerrada hace 90 días o más se marca como archivada (`src/Incidencias/ArchivadoServicio.cs#DiasHastaArchivar`). Deja de salir en la lista (`src/Incidencias/ListaDeIncidencias.cs#Consultar`) y sigue saliendo en la búsqueda (`src/Incidencias/BusquedaServicio.cs#Buscar`).

## Sin comprobar

- No he encontrado ningún aviso por correo al reabrir una incidencia.
- `docs/archivado.md` dice que el proceso se ejecuta una vez al día, de madrugada. La tarea que lo lanza no está en el repositorio: no la he leído.
- `docs/archivado.md` no menciona el botón de archivar a mano, y el código lo tiene.
