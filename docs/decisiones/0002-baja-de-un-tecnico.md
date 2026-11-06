# 0002 · Un técnico que se va se marca como inactivo

- Estado: Propuesta
- Fecha:
- Decide:
- Confianza: media
- Requisitos: RF-01, RF-03

## Contexto

El cliente pide no perder quién llevaba cada aviso, y que no queden avisos abiertos a nombre de alguien que ya no está sin que nadie se entere (respuesta del cliente, 28-10-2026). Le ha pasado con Tomás.

## Alternativas

- Impedir la baja mientras tenga avisos abiertos. Sin confirmar si la oficina puede esperar a repasarlos.
- Quitarle los avisos al darlo de baja. Descartada: se pierde quién los llevaba, que es lo que el cliente pide no perder.
- Marcarlo inactivo y avisar a quien coordina.

## Decisión

Marcarlo inactivo y avisar a quien coordina. Sus avisos siguen a su nombre hasta que los reasigne.

## Consecuencias

- La lista de técnicos necesita un campo `Activo`.
- Un inactivo no se puede elegir para un aviso nuevo. Sin confirmar cómo se avisa.
