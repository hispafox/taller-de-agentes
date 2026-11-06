---
name: exportar-pdf
description: Saca un documento de docs/ a PDF con el bloque de control. Úsalo cuando alguien pida un PDF o un documento que no se vaya a corregir encima.
---

# Exportar a PDF

Un PDF es lo que se manda cuando nadie lo va a corregir encima. Si ya hay Word, sale de ahí, para mantener una sola maqueta.

1. Comprueba que `docs/` no tiene cambios sin confirmar y que el repositorio tiene una etiqueta de versión.
   Si falta alguna, dímelo y para.
2. Si hay Word en `salida/` de esa versión y `soffice --version` responde:
   `soffice --headless --convert-to pdf --outdir salida <fichero>.docx`.
3. Si no, ejecuta `node <carpeta de este skill>/scripts/md-a-pdf.mjs <ruta>`.
   Maqueta el Markdown con un navegador.
4. Devuelve la ruta del fichero, su versión y su commit.

## Lo que se suele torcer

- Sacar un PDF de un Word de otra versión. Mira que el nombre del Word lleve la versión de ahora.
- Dar por buena una tabla ancha sin mirarla: en el PDF no hay barra para desplazarse.

La prueba de este skill: `pwsh <carpeta de este skill>/pruebas/probar.ps1`.
