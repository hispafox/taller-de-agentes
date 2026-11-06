---
name: abrir-documento-de-cambio
description: Crea el documento de un cambio que no se puede retomar con la petición y el diff, con sus especificaciones, tareas y registro. Úsalo antes de escribir código en un cambio grande sin plan.
---

# Abrir el documento de un cambio

1. Crea docs/<funcionalidad>/cambio.md con [la plantilla](./assets/plantilla-cambio.md).
2. Copia la petición, literal, en R-01.
3. Escribe cada especificación con su número y cómo se acepta.
   Los textos que alguien haya pedido van literales.
4. Escribe una línea por tarea, con lo que cubre y su ruta.
5. Apunta la orden con la que se pasan las pruebas en este repositorio.
   Si no la encuentras, pregúntala.

## Lo que se suele torcer

- Resumir la petición. R-01 va tal como llegó.
- Una tarea que no cubre ninguna especificación. O sobra la tarea, o falta algo.
