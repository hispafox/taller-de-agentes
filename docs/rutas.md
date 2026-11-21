# Rutas

## Ruta ligera

Lo entendido, con el riesgo acotado y sin señales duras.
Lo hace el Desarrollador, lo cierra con evidencia y lo revisa una persona en el pull request.
Si no se puede retomar con la petición y el diff, lleva cambio.md.

## Ruta completa

Lo que toca una señal dura, cambia una especificación o tiene decisiones sin tomar.
Entra por el primer paso cuya pregunta siga sin contestar.

`<f>` es la carpeta de la funcionalidad, `docs/<funcionalidad>/`:

| Paso | Quién | Deja | Para seguir | Clase | Si no se cumple |
|---|---|---|---|---|---|
| Análisis | Analista | `docs/encargo/`: requisitos, historias, encaje y preguntas | Las preguntas que bloquean tienen respuesta escrita | Humana | Al cliente, y la ruta espera |
| Diseño | Arquitecto | `<f>/arquitectura.md` y los registros en Propuesta | Una persona acepta las decisiones en un pull request | Humana | Al Arquitecto, con lo que falta |
| Especificación | Especificador | `<f>/spec.md` y su contrato | Aprobada en un pull request | Humana | Al Especificador, o al cliente si es una pregunta |
| Plan | Planificador | `<f>/plan.md` | Cada caso tiene tarea, y quien va a implementar lo entiende | Script y humana | A la especificación, si le falta algo |
| Issues | Publicador | `<f>/issues.md`, `<f>/plan-pruebas.md` y los issues | «Publica», con el borrador delante | Humana | Se corrige el borrador |
| Implementación | Desarrollador | Los commits y el registro de `plan.md` | Compila y pasan las pruebas | Automática | Al Desarrollador, con su límite de rondas |
| Verificación | Verificador | `<f>/verificacion.md` | `preparar-revision.ps1` sale con 0 y el veredicto es Aprobado | Automática y de agente | Al Desarrollador, con las filas |
| Auditoría | Auditor | Su informe, y `<f>/auditoria.md` con lo descartado | Sin bloqueos, o una persona decide | De agente y humana | Al Desarrollador, y se verifica otra vez |
| Manual | Documentador | `docs/manual/` y sus capturas | Las catas de 6.1 y la ronda de 6.2 | Humana y automática | Al Documentador |
| Pull request | Publicador y una persona | El pull request | Lo trabaja 11.3 | Humana | Borrador, con lo que falta |
