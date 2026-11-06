---
name: contratos-de-api
description: Convenciones de este repositorio para lo que entra y sale de la API. Úsalo al añadir o cambiar un DTO o una validación.
---

# Contratos de API

1. Lo que entra y lo que sale son `record`, junto a la entidad, como en
   `src/Incidencias/IncidenciaEnLista.cs`. Una entidad no sale tal cual por la API.
2. Si hay un `contrato.openapi.yaml`, manda él: nombres, campos obligatorios y formatos.
3. Lo que un dato del cuerpo no cumple se valida antes de guardar nada.

## Lo que se suele torcer

- Añadir un campo que nadie ha acordado. Si hace falta, dilo y cambia antes el contrato.
