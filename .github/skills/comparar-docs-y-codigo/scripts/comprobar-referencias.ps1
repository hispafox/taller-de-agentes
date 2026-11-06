param([string]$Documento)

if (-not $Documento -or -not (Test-Path $Documento)) {
    Write-Output 'Uso: comprobar-referencias.ps1 -Documento <ruta del documento>'
    exit 2
}

$texto = Get-Content -Path $Documento -Raw
$referencias = [regex]::Matches($texto, '`([\w./-]+\.\w+)#(\w+)`')

$rotas = foreach ($referencia in $referencias) {
    $fichero = $referencia.Groups[1].Value
    $simbolo = $referencia.Groups[2].Value
    if (-not (Test-Path $fichero)) {
        "No existe el fichero: $fichero"
    }
    elseif (-not (Select-String -Path $fichero -Pattern "\b$simbolo\b" -Quiet)) {
        "No aparece $simbolo en $fichero"
    }
}

if ($rotas) {
    $rotas | Sort-Object -Unique
    exit 1
}

Write-Output "Comprobadas $($referencias.Count) referencias, todas encontradas."
