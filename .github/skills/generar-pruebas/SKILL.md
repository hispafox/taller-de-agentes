---
name: generar-pruebas
description: Escribe las pruebas de una tarea a partir de sus casos, con las convenciones de este repositorio. Úsalo al implementar una tarea o al cubrir un cambio con pruebas.
---

# Generar pruebas

1. Busca una prueba parecida en el repositorio y sigue su forma.
   No añadas otra biblioteca de pruebas. Si no hay ninguna, propón una y espera.
2. Por cada caso de la tarea, escribe su prueba en el nivel más bajo que lo demuestre.
   El nombre empieza por el identificador del caso.
3. Lo esperado sale del caso y se escribe a mano. No lo calcules con el código que pruebas.
4. Comprueba lo que se ve desde fuera: la respuesta, lo guardado, lo que sale en pantalla.
5. Si la tarea toca una ruta con contrato, prueba también lo que el contrato prohíbe:
   cada campo obligatorio que falta, uno de más y un formato que no vale. Cada uno da
   error y no cambia nada.
6. Ejecútala y mírala fallar. Si falla por otra razón que la esperada, arréglalo.
7. Si lo que ya existía pasa por este código, añade una prueba de que sigue igual.

## Lo que se suele torcer

- Una prueba que solo comprueba que no ha dado error.
- Un simulador que sustituye justo lo que se quería probar.
- Cambiar lo esperado para que pase.
