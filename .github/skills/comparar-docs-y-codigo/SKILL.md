---
name: comparar-docs-y-codigo
description: Compara un documento de docs/ con el código actual y lista lo que ya no coincide. Úsalo cuando haya que comprobar si la documentación sigue al día, antes de una versión o cuando alguien dude de lo que dice un documento, aunque no se diga la palabra comparar.
---

# Comparar documentación y código

1. Si no te han dado el documento, pregunta cuál.
2. Ejecuta `./scripts/comprobar-referencias.ps1 -Documento <ruta>` y apunta lo que falle.
3. Busca el último commit que tocó el documento y mira qué ha cambiado en el código
   desde entonces. Lee lo que afecte al documento.
4. Para cada afirmación afectada, lee su código y clasifícala: coincide, no coincide
   o no se puede comprobar.
5. Devuelve una tabla con la afirmación, lo que hace el código, la referencia
   y si el documento describe lo que hay o recoge una decisión.
   No cambies nada hasta que te lo diga.

## Lo que se suele torcer

- Una diferencia no dice quién tiene razón. Si el documento recoge una decisión,
  el fallo puede estar en el código.
- Lo que coincide no se toca, aunque se pudiera redactar mejor.
