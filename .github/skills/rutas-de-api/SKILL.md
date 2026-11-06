---
name: rutas-de-api
description: Convenciones de este repositorio para las rutas y los errores de la API. Úsalo al añadir o cambiar una ruta.
---

# Rutas de API

1. Las rutas son de API mínima y van en el fichero `<Cosa>Rutas.cs` de su carpeta,
   como `src/Incidencias/IncidenciasRutas.cs`.
2. Cada error sale de `src/Incidencias/Errores.cs`, con su título y un detalle
   que entienda quien usa la aplicación.
3. Ruta que apunta a algo que no existe, 404. Dato del cuerpo que no vale, 400.
   Choque con el estado de la incidencia, 409.

## Lo que se suele torcer

- Dos errores iguales con dos formas distintas. Busca antes cómo responde la API a una zona que no existe.
