# Plan · Técnico responsable y plazo

- Estado: Propuesto
- Especificación: spec.md, Aprobada
- Contrato: contrato.openapi.yaml

## Lo que he leído

spec.md, contrato.openapi.yaml, arquitectura.md, las decisiones 0001 y 0002,
src/Incidencias/ y las migraciones existentes.

## Tareas

### T-01 · Los datos y la migración
- Toca: src/Tecnicos/ (nueva), src/Incidencias/ y una migración.
- Casos: C-06.
- Skills: ninguno.
- Se comprueba: se aplica a una copia de la base con incidencias, y siguen todas.

### T-02 · La lista de técnicos en la API
- Toca: src/Tecnicos/.
- Casos: ninguno. El alta, en espera de PE-04.
- Skills: ninguno.
- Se comprueba: las rutas responden como dice el contrato.

### T-03 · Asignar, cambiar y quitar
- Toca: src/Incidencias/.
- Casos: C-02, C-03, C-04, C-05, C-10 y C-11.
- Skills: generar-pruebas.
- Se comprueba: las pruebas de cada caso y las de contrato de esa ruta.

### T-04 · La lista de incidencias en la API
- Toca: src/Incidencias/.
- Casos: C-07, C-08, C-09 y C-12.
- Skills: generar-pruebas.
- Se comprueba: las pruebas de cada caso, con fechas fijas.

### T-05 · La pantalla
- Toca: la lista de incidencias de la interfaz.
- Casos: C-01, C-07 y C-09.
- Skills: ninguno.
- Se comprueba: extremo a extremo y plan de pruebas manuales.

## Preguntas

- PE-03 · Para: Elena. Una incidencia resuelta y sin cerrar, con el plazo pasado,
  ¿sale fuera de plazo? La especificación dice «plazo de ayer» sin decir en qué estado.
  Espera: T-04.
- PE-04 · Para: quien lleva la especificación. «Una lista de técnicos, que mantiene
  quien coordina» no tiene ningún caso. Espera: el alta de T-02.

## Registro

- R-01 · T-01 cerrada: dotnet test en verde. Migración aplicada a una copia con datos, y siguen todas.
- R-02 · T-02 cerrada: dotnet test en verde. PE-04 sigue sin caso: la lista se comprueba con sus pruebas.
- R-03 · T-04 en espera de PE-03.

- R-04 · T-03 cerrada: dotnet test en verde. Cubre C-02, C-03, C-04, C-05, C-10 y C-11.

- R-05 · PE-03 contestada: pasa a C-12.
- R-06 · T-04 y T-05 cerradas, seguidas: dotnet test y npm test en verde. Sin comprobar: las pruebas de Playwright, que piden la aplicación en marcha.

- R-07 · Verificador: C-03 sin evidencia y C-04 no cumple. Corregidos: prueba primero y código después. Aprobado.
