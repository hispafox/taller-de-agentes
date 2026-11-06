"""Pasa a Excel las tablas Markdown de uno o varios documentos, con una pestaña Control.

Uso: py tablas-a-excel.py <ruta> [<ruta> ...]
Salidas: 0 bien, 1 el repositorio o el documento no están listos, 2 falta una herramienta.
"""
import pathlib
import re
import subprocess
import sys

try:
    from openpyxl import Workbook
    from openpyxl.styles import Font
except ImportError:
    print("Falta el paquete openpyxl. Instálalo con: py -m pip install openpyxl")
    sys.exit(2)


def git(*args):
    resultado = subprocess.run(["git", *args], capture_output=True, encoding="utf-8")
    return resultado.stdout.strip() if resultado.returncode == 0 else ""


def celdas(linea):
    return [c.strip().replace("`", "") for c in re.split(r"(?<!\\)\|", linea.strip().strip("|"))]


def tablas(texto):
    """Devuelve (título más cercano, cabecera, filas) de cada tabla del texto."""
    encontradas, titulo, lineas, i = [], "Tabla", texto.splitlines(), 0
    while i < len(lineas):
        linea = lineas[i]
        if linea.startswith("#"):
            titulo = linea.lstrip("# ").strip()
        separador = i + 1 < len(lineas) and re.fullmatch(r"\s*\|[\s:|-]+\|\s*", lineas[i + 1])
        if linea.lstrip().startswith("|") and separador:
            cabecera, filas, i = celdas(linea), [], i + 2
            while i < len(lineas) and lineas[i].lstrip().startswith("|"):
                filas.append(celdas(lineas[i]))
                i += 1
            encontradas.append((titulo, cabecera, filas))
            continue
        i += 1
    return encontradas


def nombre_de_pestana(titulo, usados):
    base = re.sub(r"[\[\]:*?/\\]", " ", titulo)[:28].strip() or "Tabla"
    nombre, n = base, 2
    while nombre in usados or nombre == "Control":
        nombre, n = f"{base} {n}", n + 1
    usados.add(nombre)
    return nombre


def main():
    fuentes = sys.argv[1:]
    if not fuentes:
        print("Uso: py tablas-a-excel.py <ruta> [<ruta> ...]")
        return 2
    if git("status", "--porcelain", "--", "docs"):
        print("Hay cambios sin confirmar en docs/. Haz el commit antes de exportar.")
        return 1
    version = git("describe", "--tags", "--abbrev=0")
    if not version:
        print("El repositorio no tiene ninguna etiqueta de versión.")
        return 1

    libro = Workbook()
    control = libro.active
    control.title = "Control"
    principal = fuentes[0]
    for campo, valor in (
        ("Proyecto", "GI"),
        ("Versión", version),
        ("Fecha", git("log", "-1", "--format=%cs", "--", principal)),
        ("Origen", git("log", "-1", "--format=%h", "--", principal)),
    ):
        control.append([campo, valor])

    usados, total = set(), 0
    for fuente in fuentes:
        for titulo, cabecera, filas in tablas(pathlib.Path(fuente).read_text(encoding="utf-8")):
            hoja = libro.create_sheet(nombre_de_pestana(titulo, usados))
            hoja.append(cabecera)
            for celda in hoja[1]:
                celda.font = Font(bold=True)
            for fila in filas:
                hoja.append(fila)
            total += 1

    if total == 0:
        print("No he encontrado ninguna tabla.")
        return 1

    pathlib.Path("salida").mkdir(exist_ok=True)
    destino = f"salida/{pathlib.Path(principal).stem}-{version}.xlsx"
    libro.save(destino)
    print(f"Exportado: {destino} ({total} tablas, {version})")
    return 0


if __name__ == "__main__":
    sys.exit(main())
