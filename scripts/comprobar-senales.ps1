param([string]$Base = 'main')

$senales = [ordered]@{
    'Datos y migraciones'     = '^src/Datos/|^src/Migrations/'
    'Contratos públicos'      = 'Rutas\.cs$|\.openapi\.yaml$'
    'Lo que leen los agentes' = '^\.github/|^\.mcp\.json$'
}

$ficheros = git diff --name-only "$Base...HEAD"
$encontradas = 0

foreach ($senal in $senales.Keys) {
    $tocados = $ficheros | Where-Object { $_ -match $senales[$senal] }
    if ($tocados) {
        $encontradas++
        Write-Output "${senal}:"
        $tocados | ForEach-Object { Write-Output "  $_" }
    }
}

if ($encontradas -eq 0) { Write-Output 'Ninguna señal en los ficheros de este cambio.' }
exit $encontradas
