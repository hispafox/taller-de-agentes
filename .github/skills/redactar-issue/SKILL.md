---
name: redactar-issue
description: Redacta un issue con el formato de este equipo. Úsalo siempre que haya que abrir, escribir o preparar un issue, un aviso de fallo o una petición de mejora, aunque no se diga la palabra issue.
---

# Redactar un issue

1. Reúne qué pasa, cómo se reproduce y qué debería pasar. Si falta algo, pregúntalo.
2. Busca entre los issues abiertos si ya existe. Si hay uno parecido, dímelo y para.
3. Rellena [la plantilla](./assets/plantilla-issue.md) en un fichero temporal.
4. Comprueba el borrador con `./scripts/comprobar-issue.ps1 -Fichero <ruta>`.
   Si falla, corrige y vuelve a comprobarlo.
5. Enséñame el borrador, con el repositorio de destino.
   No publiques nada hasta que te lo diga.

## Lo que se suele torcer

- En «Parte afectada» va la pantalla o la ruta de la API, nunca el nombre de una clase:
  quien lee el issue puede no conocer el código.
