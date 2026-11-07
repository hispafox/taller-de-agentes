# Issues · Técnico responsable y plazo

- Destino: mi-organizacion/mi-proyecto
- Especificación: spec.md

## H-01 · Asignar un técnico responsable y un plazo a una incidencia

- Issue: pendiente
- Parecidos: ninguno.

Como coordinadora de avisos,
quiero asignar un técnico responsable a cada incidencia,
para que nadie dé por hecho que la lleva otro.

### Criterios de aceptación

- [ ] C-01 · Una incidencia nueva se guarda sin técnico ni plazo, y en la lista sale «Sin asignar».
- [ ] C-02 · Al volver a leerla, tiene el técnico y el plazo asignados.
- [ ] C-03 · El técnico y el plazo se pueden cambiar y quitar.
- [ ] C-04 · Un técnico que no está en la lista da error, y la incidencia no cambia.
- [ ] C-05 · Al cerrarla, conserva su técnico.
- [ ] C-06 · Tras la migración siguen todas las incidencias, sin técnico y sin plazo.
- [ ] C-07 · Con plazo de ayer, la fecha sale en rojo con el texto «Fuera de plazo».
- [ ] C-08 · Con plazo de hoy, no sale fuera de plazo.

### De dónde sale

- Especificación: docs/tecnico-y-plazo/spec.md
- Contrato: docs/tecnico-y-plazo/contrato.openapi.yaml
- Plan: tareas T-03 y T-05

## H-02 · Filtrar la lista por técnico

- Issue: pendiente
- Parecidos: ninguno.

Como técnico,
quiero filtrar la lista por técnico,
para ver solo lo mío.

### Criterios de aceptación

- [ ] C-09 · Al filtrar por un técnico salen solo sus incidencias.

### De dónde sale

- Especificación: docs/tecnico-y-plazo/spec.md
- Plan: tareas T-04 y T-05
