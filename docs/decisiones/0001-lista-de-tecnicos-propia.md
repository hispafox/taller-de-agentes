# 0001 · La lista de técnicos se mantiene en la aplicación

- Estado: Aceptada
- Fecha: 30-10-2026
- Decide: la responsable de operaciones y el equipo de desarrollo
- Confianza: alta
- Requisitos: RF-01, RF-03

## Contexto

Cada incidencia necesita un técnico responsable, y la aplicación no guarda técnicos. En la hoja
de avisos el nombre se escribe a mano, y sale de varias formas. Administración propuso sacar
la lista de la hoja de nóminas, que está al día. Esa hoja lleva sueldos y bajas.

## Alternativas

- Texto libre en la incidencia, con una lista de nombres válidos. Es el cambio más pequeño
  sobre lo que hay hoy. Descartada: esa lista hay que mantenerla igual, y un técnico seguiría
  sin identificarse de una sola forma (RF-03).
- Sincronizar con la hoja de nóminas. Evita mantener dos listas. Descartada: «esa hoja tiene
  sueldos y bajas, y no puede salir de administración» (respuesta del cliente, 28-10-2026).
- Lista propia en la aplicación, que mantiene quien coordina los avisos.

## Decisión

Lista propia en la aplicación. La mantiene quien coordina los avisos.

## Consecuencias

- Cada técnico se identifica de una sola forma, y la lista se puede filtrar.
- Hay dos listas que mantener: un alta se hace dos veces.
- Queda por decidir qué pasa cuando alguien se va (ver 0002).
