param([string]$Fichero)

if (-not $Fichero -or -not (Test-Path $Fichero)) {
    Write-Output 'Uso: comprobar-issue.ps1 -Fichero <ruta del borrador>'
    exit 2
}

$secciones = 'Pasos para reproducir', 'Resultado esperado', 'Resultado actual', 'Parte afectada'
$texto = Get-Content -Path $Fichero -Raw
$faltan = $secciones | Where-Object { $texto -notmatch "(?m)^## $_\s*$" }

if ($faltan) {
    Write-Output "Faltan estas secciones: $($faltan -join ', ')"
    exit 1
}

Write-Output 'El borrador tiene todas las secciones.'
