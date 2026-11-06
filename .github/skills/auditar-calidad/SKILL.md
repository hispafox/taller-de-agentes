---
name: auditar-calidad
description: Procedimiento para auditar código terminado en este repositorio. Qué se mira, cómo se gradúa cada hallazgo y qué hacer con un problema repetido. Úsalo al auditar un cambio.
---

# Auditar un cambio

## Qué se mira aquí

1. Capas: lo nuevo está donde lo demás de su clase, como dice docs/arquitectura.md.
2. Consultas: una consulta dentro de un bucle, o una tabla entera traída para filtrar en memoria.
3. Código asíncrono: .Result o .Wait() sobre una tarea, o un async sin await.
4. Validaciones: lo que entra por la API y no se comprueba en la entrada.
5. Errores: un catch que se calla, o un fallo que deja datos a medias.
6. Seguridad de bulto: lo que salte a la vista. La revisión a fondo es otra.

## Cómo se escribe un hallazgo

H-01 · Gravedad · Título en una línea
Dónde: fichero y línea.
Qué pasa: con esta entrada o esta situación, ocurre esto.
Qué propones: lo mínimo que lo arregla.
Si no se hace: qué nota quien usa la aplicación.

## La gravedad

Crítico: se pierden datos, o alguien hace lo que no debe, en el uso normal.
Alto: falla en una situación que va a pasar con la carga prevista.
Medio: funciona hoy, y va a costar: al crecer, al cambiarlo o al mantenerlo.
Bajo: se podría hacer mejor, y nadie lo va a notar.
Crítico y Alto bloquean. Lo demás no.

## Si algo se repite

Si la misma forma de problema sale en dos sitios o más, añade una sección «Se repite»:
dónde sale, qué skill, instrucción o código anterior puede estar detrás, y qué línea propones.

## Lo que se suele torcer

- Un hallazgo sin situación concreta. Es una opinión.
- Repetir lo que ya ha dicho el Verificador.
- Un gusto de estilo con gravedad de fallo.
