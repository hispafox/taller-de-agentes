---
name: modelo-y-migraciones
description: Convenciones de este repositorio para las entidades y las migraciones de Entity Framework Core. Úsalo al añadir o cambiar una entidad o un campo.
---

# Modelo y migraciones

1. La entidad va en la carpeta de lo que gestiona, como `src/Incidencias/Incidencia.cs`.
   Si es nueva, añade su `DbSet` a `src/Datos/Datos.cs`.
2. Un campo nuevo sobre una tabla con datos es opcional o lleva valor por defecto.
3. Compila, y crea la migración desde `src/` con `dotnet ef migrations add <Nombre>`.
4. Las fechas sin hora van en `DateOnly`, y la hora se pide a `TimeProvider`.

## Lo que se suele torcer

- Compilar y dar el cambio por bueno sin migración: la aplicación falla al arrancar.
- Editar una migración que ya está en la rama principal. Se añade otra.
