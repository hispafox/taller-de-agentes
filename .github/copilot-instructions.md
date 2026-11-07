# Gestor de incidencias

API en ASP.NET Core con .NET 10, Entity Framework Core sobre SQLite e interfaz en React.

Los identificadores del código van en castellano, sin eñes ni tildes.

Las pruebas se pasan con `dotnet test` y `npm test --prefix src/interfaz`.

## Memoria del proyecto

La arquitectura, las decisiones y el vocabulario están en docs/.
Antes de proponer un cambio, lee lo que afecte a ese cambio.
Si lo que te piden contradice una decisión de docs/, dímelo antes de seguir.

## GitHub

Antes de crear o cambiar algo en GitHub, ejecuta `git remote -v`
y di en qué repositorio vas a escribir.

## Documentación

Si al trabajar ves que el código y `docs/` dicen cosas distintas, para y dímelo antes de seguir.
Cuando un cambio que te pido deja atrás un documento de `docs/`, actualízalo en el mismo commit.

## Decisiones

De una decisión aceptada de docs/decisiones/ solo cambias el estado y las erratas.
Si hay que cambiarla, propón un registro nuevo que la sustituya, y pasa la vieja
a Sustituida en el mismo pull request.

## Documentos que salen del repositorio

Todo documento que salga del repositorio lleva su bloque de control: proyecto, versión,
fecha y commit de origen. Lo escriben los skills exportar-word, exportar-pdf y exportar-excel,
nunca a mano. Si te piden un documento sin pasar por ellos, dímelo antes de seguir.
