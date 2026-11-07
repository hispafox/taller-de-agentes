---
name: exportar-excel
description: Saca a Excel las tablas de un documento de docs/, con una pestaña Control. Úsalo cuando alguien pida un Excel, la hoja de consumo, los requisitos o el plan de pruebas en una hoja de cálculo.
---

# Exportar a Excel

1. Comprueba que `docs/` no tiene cambios sin confirmar y que el commit tiene su etiqueta de versión
   (`git describe --tags --exact-match`). Si falta alguna, dímelo y para.
2. Ejecuta `py <carpeta de este skill>/scripts/tablas-a-excel.py <ruta> [<ruta> ...]`.
   En Mac o Linux, `python3`. Cada tabla va a su pestaña, y la pestaña Control lleva proyecto, versión, fecha y commit.
3. Devuelve la ruta del fichero y cuántas tablas ha sacado.

Para leer un Excel que llega de fuera, vuelca cada pestaña a CSV con `openpyxl` y trabaja con el texto.

## Lo que se suele torcer

- Fórmulas: `openpyxl` las escribe sin calcularlas, y una fórmula que calcula no tiene por qué estar bien. Si las añades, abre el fichero y míralo.
- Un documento sin tablas. El script lo dice y para: no sale un Excel vacío.

La prueba de este skill: `py <carpeta de este skill>/pruebas/probar.py`.
