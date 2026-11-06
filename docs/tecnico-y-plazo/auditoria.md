# Auditoría · Técnico responsable y plazo

Resultado: Sin bloqueos.
Carga supuesta: nueve técnicos y unas decenas de avisos, de la petición del encargo.

H-01 · Medio · Una consulta por cada fila de la lista
Dónde: src/Incidencias/ListaDeIncidencias.cs
Qué pasa: con 40 incidencias se lanzan 41 consultas, porque el técnico se busca dentro del bucle.
Qué propones: traerlo en la misma consulta, con un Select. Devuelve las mismas filas.
Si no se hace: hoy nadie lo nota. La pantalla irá más lenta a medida que crezca la lista.
Corregido.

## Se repite

Las pruebas de C-03, C-04 y C-11 comprueban la respuesta y no vuelven a leer la incidencia.
Salen las tres de T-03, que el plan hace con el skill generar-pruebas. Añadida la línea a su lista.
