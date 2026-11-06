---
name: revisar-seguridad
description: Revisión de seguridad de un cambio, con informe de formato fijo. Secretos, paquetes, patrones peligrosos y lo que llevan instalado los agentes. Úsalo en un cambio que toca datos, permisos o dependencias.
---

# Revisar la seguridad

Lee en salida/revision/ lo que han dejado los scripts, y cambio.diff.
Si falta alguno, esa zona queda como «Sin comprobar».

## Secretos
Resume secretos.json. Nunca copies un valor: solo el fichero y la línea.

## Paquetes
Resume las salidas de paquetes, de más a menos graves.
Cada paquete nuevo del diff: ¿existe, quién lo mantiene y qué licencia tiene?

## Patrones
Busca en el diff y en el código que toca:
- SQL montado con texto: FromSqlRaw con cadenas interpoladas o concatenadas.
- Una ruta que cambia datos sin comprobar quién llama.
- Lo que el usuario escribe, pintado como HTML.
- Un error que devuelve detalles internos.

## Lo que llevan los agentes
Si el diff toca .github/ o .mcp.json, di qué cambia y quién lo tiene que aprobar.

## Informe
Una tabla: Zona, Resultado (Limpio, Hallazgos o Sin comprobar), Qué has mirado.
Después, los hallazgos con el formato y la gravedad de auditar-calidad.
