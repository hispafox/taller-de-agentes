"""Prueba del skill: exporta un documento de muestra en un repositorio de usar y tirar.

Salidas: 0 bien, 1 ha fallado, 2 falta una herramienta.
"""
import os
import pathlib
import subprocess
import sys
import tempfile

script = pathlib.Path(__file__).resolve().parent.parent / "scripts" / "tablas-a-excel.py"

try:
    from openpyxl import load_workbook
except ImportError:
    print("Falta el paquete openpyxl. Instálalo con: py -m pip install openpyxl")
    sys.exit(2)


def correr(carpeta, *args):
    return subprocess.run([sys.executable, str(script), *args], cwd=carpeta, capture_output=True, encoding="utf-8", errors="replace", env=os.environ)


def git(carpeta, *args):
    subprocess.run(["git", *args], cwd=carpeta, check=True, capture_output=True)


with tempfile.TemporaryDirectory(ignore_cleanup_errors=True) as carpeta:
    raiz = pathlib.Path(carpeta)
    (raiz / "docs").mkdir()
    (raiz / "docs/muestra.md").write_text(
        "# Tareas\n\n| Tarea | Consumo |\n|---|---|\n| Documentar | 120 de 150 |\n\n## Sin tablas\n\nTexto.\n", encoding="utf-8")
    (raiz / "docs/vacio.md").write_text("Solo texto.\n", encoding="utf-8")
    git(carpeta, "init", "-q")
    git(carpeta, "config", "user.email", "prueba@ejemplo.invalid")
    git(carpeta, "config", "user.name", "prueba")
    git(carpeta, "add", "-A")
    git(carpeta, "commit", "-q", "-m", "muestra")

    if correr(carpeta, "docs/muestra.md").returncode != 1:
        print("FALLO: sin etiqueta tenía que salir con 1")
        sys.exit(1)
    git(carpeta, "tag", "v1.0")
    if correr(carpeta, "docs/vacio.md").returncode != 1:
        print("FALLO: sin tablas tenía que salir con 1")
        sys.exit(1)
    resultado = correr(carpeta, "docs/muestra.md")
    if resultado.returncode != 0:
        print("FALLO:", resultado.stdout, resultado.stderr)
        sys.exit(1)

    libro = load_workbook(raiz / "salida/muestra-v1.0.xlsx")
    if libro.sheetnames != ["Control", "Tareas"]:
        print("FALLO: pestañas", libro.sheetnames)
        sys.exit(1)
    if libro["Control"]["B2"].value != "v1.0" or libro["Tareas"]["B2"].value != "120 de 150":
        print("FALLO: contenido")
        sys.exit(1)
    libro.close()

print("Prueba de exportar-excel: bien.")
