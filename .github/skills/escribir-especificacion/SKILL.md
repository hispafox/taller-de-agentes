---
name: escribir-especificacion
description: Escribe o actualiza la especificación de una funcionalidad con el formato de este equipo. Úsalo cuando haya que fijar qué tiene que pasar, caso por caso, antes de planificar o programar.
---

# Escribir una especificación

1. Rellena [la plantilla](./assets/plantilla-spec.md). Enlaza requisitos, diseño y
   decisiones, sin copiarlos.
2. Para cada requisito, escribe sus casos con Dado, Cuando y Entonces. Pregúntale a cada uno
   qué pasa con lo que ya existe, qué pasa si el dato no está o no vale, y qué pasa
   en cada estado.
3. Repasa cada caso: ¿se contesta con un sí o un no sin preguntarle a nadie?
   Si no, reescríbelo con lo que se ve desde fuera.
4. En «Qué queda fuera» pon lo que el cliente ha aplazado y lo que alguien
   podría añadir de buena fe.
5. Lo que falte por decidir va a «Preguntas abiertas», con para quién es,
   tu propuesta y qué caso espera la respuesta.

## Lo que se suele torcer

- Un caso que dice cómo se programa. Las tablas, las clases y los campos internos
  van en el plan.
- Una decisión tomada al redactar. Si has elegido entre dos opciones, es una pregunta.
