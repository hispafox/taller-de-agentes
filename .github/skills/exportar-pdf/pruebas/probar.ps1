# Prueba del skill: exporta un documento de muestra en un repositorio de usar y tirar.
# Salidas: 0 bien · 1 ha fallado · 2 falta una herramienta.
$script = Resolve-Path "$PSScriptRoot/../scripts/md-a-pdf.mjs"
$carpeta = Join-Path ([IO.Path]::GetTempPath()) "prueba-pdf-$([guid]::NewGuid().ToString('N'))"
New-Item -ItemType Directory -Force "$carpeta/docs" | Out-Null
Push-Location $carpeta
try {
    git init -q
    git config user.email prueba@ejemplo.invalid
    git config user.name prueba
    "# Título de prueba`n`nUn párrafo.`n`n| Campo | Valor |`n|---|---|`n| Plazo | Mañana |" | Set-Content docs/muestra.md -Encoding utf8
    git add -A
    git commit -q -m muestra

    node $script docs/muestra.md | Out-Null
    if ($LASTEXITCODE -ne 1) { Write-Output "FALLO: sin etiqueta tenía que salir con 1 y salió con $LASTEXITCODE"; exit 1 }

    git tag v1.0
    git commit -q --allow-empty -m despues
    node $script docs/muestra.md | Out-Null
    if ($LASTEXITCODE -ne 1) { Write-Output "FALLO: con un commit después de la etiqueta tenía que salir con 1 y salió con $LASTEXITCODE"; exit 1 }
    git tag v1.1
    node $script docs/muestra.md
    if ($LASTEXITCODE -eq 2) { exit 2 }
    if ($LASTEXITCODE -ne 0) { Write-Output 'FALLO: no ha exportado'; exit 1 }

    $bytes = [IO.File]::ReadAllBytes((Resolve-Path 'salida/muestra-v1.1.pdf'))
    $cabecera = [Text.Encoding]::ASCII.GetString($bytes, 0, 4)
    if ($cabecera -ne '%PDF' -or $bytes.Length -lt 1000) { Write-Output 'FALLO: no parece un PDF'; exit 1 }
    Write-Output 'Prueba de exportar-pdf: bien.'
}
finally {
    Pop-Location
    Remove-Item $carpeta -Recurse -Force -ErrorAction SilentlyContinue
}
