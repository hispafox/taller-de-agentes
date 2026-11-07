# Prueba del skill: exporta un documento de muestra en un repositorio de usar y tirar.
# Salidas: 0 bien · 1 ha fallado · 2 falta una herramienta.
$script = Resolve-Path "$PSScriptRoot/../scripts/md-a-word.mjs"
$carpeta = Join-Path ([IO.Path]::GetTempPath()) "prueba-word-$([guid]::NewGuid().ToString('N'))"
New-Item -ItemType Directory -Force "$carpeta/docs" | Out-Null
Push-Location $carpeta
try {
    git init -q
    git config user.email prueba@ejemplo.invalid
    git config user.name prueba
    "# Título de prueba`n`nUn párrafo con **negrita**.`n`n- Uno`n- Dos`n`n| Campo | Valor |`n|---|---|`n| Plazo | Mañana |" |
        Set-Content docs/muestra.md -Encoding utf8
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

    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $zip = [IO.Compression.ZipFile]::OpenRead((Resolve-Path 'salida/muestra-v1.1.docx'))
    $lector = [IO.StreamReader]::new($zip.GetEntry('word/document.xml').Open())
    $xml = $lector.ReadToEnd()
    $lector.Dispose()
    $zip.Dispose()

    $faltan = 'Título de prueba', 'negrita', 'Mañana', 'Proyecto', 'v1.1' | Where-Object { $xml -notmatch $_ }
    if ($faltan) { Write-Output "FALLO: no sale en el Word: $($faltan -join ', ')"; exit 1 }
    Write-Output 'Prueba de exportar-word: bien.'
}
finally {
    Pop-Location
    Remove-Item $carpeta -Recurse -Force -ErrorAction SilentlyContinue
}
