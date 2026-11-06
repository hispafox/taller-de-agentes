---
name: exportar-word
description: Saca un documento de docs/ a Word con el bloque de control, y pasa a texto un Word que vuelve corregido. Úsalo cuando alguien pida un .docx, un Word o el manual para mandarlo por correo.
---

# Exportar a Word

El texto se mantiene en `docs/`. El Word es un resultado que se vuelve a sacar y nunca se retoca a mano.

1. Comprueba que `docs/` no tiene cambios sin confirmar y que el repositorio tiene una etiqueta de versión.
   Si falta alguna, dímelo y para: la etiqueta la pone una persona.
2. Si `pandoc --version` responde, ejecuta `pwsh scripts/exportar-documento.ps1 -Fuente <ruta>`.
   Da el aspecto de `docs/plantillas/plantilla.docx`.
3. Si no hay Pandoc, ejecuta `node <carpeta de este skill>/scripts/md-a-word.mjs <ruta>`.
   Sale un Word correcto, sin plantilla corporativa. Dilo al entregarlo.
4. Devuelve la ruta del fichero, su versión y su commit.

Para leer un Word que ha vuelto corregido: `pandoc <fichero>.docx -t gfm -o salida/corregido.md`.
Compáralo con el documento en el commit que dice su bloque de control. No cambies nada en `docs/` hasta que te lo diga.

## Lo que se suele torcer

- Escribir el bloque de control a mano. Sale de Git, por el script.
- Abrir el Word y arreglarlo. Lo que haya que cambiar se cambia en `docs/` y se vuelve a exportar.
- Dar el Word por bueno sin abrirlo. Una tabla ancha o una imagen pueden salir mal: mira el resultado.

La prueba de este skill: `pwsh <carpeta de este skill>/pruebas/probar.ps1`.
