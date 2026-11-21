---
name: abrir-pull-request
description: Prepara el texto de un pull request con la referencia que cierra su issue, lo comprobado y lo pendiente, y lo abre cuando se lo digan. Úsalo con una rama ya subida, terminada o sin terminar.
---

# Abrir un pull request

1. Apunta la rama, la base y el repositorio que te he dado. Si falta uno, pídemelo y para.
2. Lee lo que haya de este cambio: plan.md y su registro, issues.md, verificacion.md,
   auditoria.md, plan-pruebas.md y ruta.md.
3. Escribe salida/pull-request.md con .github/pull_request_template.md.
   Cada línea de «Comprobado» sale de un fichero. Si no la encuentras, va a «Sin comprobar».
4. Elige la forma:
   - Listo: Aprobado, plan de pruebas pasado y todas las casillas cubiertas.
   - Borrador: falta alguna de las tres. Añade «Pendiente», con quién lo recoge.
5. Pon «Closes» solo con los issues que tengan todas sus casillas cubiertas.
   Si cubre una parte, escribe «Parte de #N».
6. Dame la ruta y la forma, y para.
7. Cuando te diga «abre», ábrelo con una sola llamada. Apunta el número en issues.md.

## Lo que se suele torcer

- «Closes» en un pull request que no va a la rama por defecto: no cierra nada.
- Escribir «probado» sin la orden y el resultado.
