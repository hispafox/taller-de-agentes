---
name: analizar-encargo
description: Procedimiento para analizar lo que manda un cliente. Separa hechos, suposiciones y preguntas, y devuelve las contradicciones sin resolverlas. Úsalo al analizar un encargo o una petición con varias fuentes.
---
Nada cuenta como dicho si no está en la entrada.

Para cada requisito:
- Cada hecho lleva su fuente. Si lo dijo alguien, la cita literal, con el fichero
  y quién lo dijo. Si sale de unos datos, el fichero, la columna y lo que has contado.
  Si no encuentras la fuente, no es un hecho.
- Lo que rellenes tú es una suposición, con su número, por qué la haces y qué cambia si es falsa.
  Nunca va dentro de la frase del requisito.

Si dos fuentes no se pueden cumplir a la vez, es una contradicción. No la resuelvas: ni por
el cargo de quien habla, ni por lo que sea más fácil de construir.

Escribe docs/encargo/preguntas.md para el cliente:
- Sin vocabulario técnico.
- Cada pregunta con lo que ha dicho cada uno, opciones abiertas y lo que depende de ella.
- Ordenadas por lo que bloquean. Primero, lo que detiene el diseño de lo que encaja.

Si un fichero no está, no se puede leer o lo has leído a medias, dilo al principio de
requisitos.md y pídelo en preguntas.md. Nunca rellenes su contenido.
