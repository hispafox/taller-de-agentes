# Técnico responsable y plazo

- Estado: Borrador
- Requisitos: RF-01 a RF-05, en docs/encargo/requisitos.md
- Diseño: arquitectura.md
- Decisiones: 0001, 0002 y decisiones.md

## Para qué

Nadie sabe quién lleva cada incidencia. Una fuga en un garaje llevaba once días abierta porque
cada técnico pensaba que la llevaba otro. Cada incidencia tiene que decir quién responde
de ella y para cuándo, y quien coordina tiene que ver lo que está sin asignar y lo que
se ha pasado de plazo.

## Qué se construye

- Una lista de técnicos, que mantiene quien coordina.
- Un técnico responsable y un plazo en cada incidencia. Los dos son opcionales,
  y solo los pone, los cambia o los quita la oficina.
- En la lista de incidencias: quién la lleva, su plazo, qué está fuera de plazo
  y un filtro por técnico.

## Qué queda fuera

- Calcular el plazo según la prioridad. Lo pone quien coordina, y es una fecha.
- Dos responsables en una incidencia. El segundo va en las observaciones.
- Que un técnico cambie él mismo el técnico o el plazo, ni al irse de vacaciones.
- Notificar al técnico cuando le asignan una incidencia.
- Importar la hoja de avisos.
- Los contratos de mantenimiento, el inventario, la facturación, las fotos y lo que ven los administradores.

## Cómo se comporta

| Caso | Dado | Cuando | Entonces | Requisito |
|---|---|---|---|---|
| C-01 | Una incidencia nueva | Se abre sin técnico y sin plazo | Se guarda, y en la lista sale «Sin asignar» | RF-01 |
| C-02 | Una incidencia abierta y un técnico de la lista | Se le asigna, con un plazo | Al volver a leerla, tiene ese técnico y ese plazo | RF-01, RF-02 |
| C-03 | Una incidencia con técnico y plazo | Se cambian o se quitan | Queda el valor nuevo, o vacío | RF-01, RF-02 |
| C-04 | Un técnico que no está en la lista | Se intenta asignar | Da el mismo error que una zona que no existe, y la incidencia no cambia | RF-03 |
| C-05 | Una incidencia con técnico | Se cierra | Conserva su técnico | RF-01 |
| C-06 | Incidencias anteriores al cambio | Se aplica la migración | Siguen todas, sin técnico y sin plazo | RF-01, RF-02 |
| C-07 | Una incidencia con plazo de ayer | Se mira la lista | La fecha sale en rojo con el texto «Fuera de plazo» | RF-04 |
| C-08 | Una incidencia con plazo de hoy | Se mira la lista | No sale fuera de plazo | RF-04 |
| C-09 | Incidencias de varios técnicos | Se filtra por uno | Salen solo las suyas | RF-05 |

## Preguntas abiertas

- PE-01 · ¿Se puede asignar un técnico a una incidencia cerrada?
  Para: Elena.
  Propuesta: no. Cerrada quiere decir que la oficina ya ha revisado el parte.
  Primero se reabre.
  Espera la respuesta: un caso nuevo, C-10.
- PE-02 · ¿Se puede poner un plazo que ya ha pasado?
  Para: Elena.
  Propuesta: sí. Nuria dijo «Es una fecha y ya está». La incidencia saldría
  fuera de plazo desde el primer momento.
  Espera la respuesta: un caso nuevo, C-11.
