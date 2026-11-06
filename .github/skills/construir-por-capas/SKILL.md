---
name: construir-por-capas
description: Dice qué skills de capa usar en un cambio del backend y en qué orden. Úsalo al implementar una tarea que toque los datos o la API.
---

# Construir por capas

Mira qué toca la tarea y usa solo lo que le haga falta:

- Entidades y migraciones: skill modelo-y-migraciones.
- DTO y validaciones: skill contratos-de-api.
- Rutas y errores: skill rutas-de-api.

Sigue el orden de las dependencias: los datos, después la lógica y al final la ruta.
Si la tarea no toca una capa, no la toques.
