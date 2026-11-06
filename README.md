# taller-de-agentes

Un gestor de incidencias de mantenimiento, pequeño a propósito, y el repositorio de prácticas del curso de orquestación de agentes y skills.

Las incidencias se abren, se editan, se resuelven, se reabren y se cierran. Se filtran y se buscan, y cada una tiene su prioridad y su zona.

## Qué hay dentro

| Carpeta | Qué lleva |
|---|---|
| `src/` | La API, en ASP.NET Core con .NET 10, y Entity Framework Core sobre SQLite |
| `src/interfaz/` | La interfaz, en React |
| `pruebas/` | Las pruebas de la API |
| `docs/` | La memoria del proyecto |
| `.github/` | Las instrucciones, los skills y los agentes |

## Cómo se arranca

Hacen falta el SDK de .NET 10 y Node.js. La primera vez, confía en el certificado de desarrollo con `dotnet dev-certs https --trust` e instala la interfaz con `npm install --prefix src/interfaz`.

Después, en dos terminales:

```bash
dotnet run --project src
```

```bash
npm run dev --prefix src/interfaz
```

La API escucha en `https://localhost:5001` y la interfaz, en `http://localhost:5173`. Al arrancar, la API crea la base de datos `incidencias.db` y la llena con datos de ejemplo. Para volver a empezar, para la API y borra ese fichero.

## Cómo se comprueba

```bash
dotnet test
```

```bash
npm test --prefix src/interfaz
```
