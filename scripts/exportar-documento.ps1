param(
    [string]$Fuente = 'docs/manual/manual-usuario.md',
    [string]$Plantilla = 'docs/plantillas/plantilla.docx',
    [string]$Proyecto = 'GI'
)

if (-not (Get-Command pandoc -ErrorAction SilentlyContinue)) {
    Write-Output 'No encuentro pandoc en esta máquina.'
    exit 2
}

if (git status --porcelain -- docs) {
    Write-Output 'Hay cambios sin confirmar en docs/. Haz el commit antes de exportar.'
    exit 1
}

$version = git describe --tags --abbrev=0 2>$null
if (-not $version) {
    Write-Output 'El repositorio no tiene ninguna etiqueta de versión.'
    exit 1
}

$commit = git log -1 --format=%h -- $Fuente
$fecha = git log -1 --format=%cs -- $Fuente
$nombre = [IO.Path]::GetFileNameWithoutExtension($Fuente)
$carpeta = Split-Path $Fuente

$bloque = New-TemporaryFile
@(
    '| Proyecto | Versión | Fecha | Origen |'
    '|---|---|---|---|'
    "| $Proyecto | $version | $fecha | $commit |"
    ''
) | Set-Content -Path $bloque -Encoding utf8

New-Item -ItemType Directory -Force -Path 'salida' | Out-Null
$destino = "salida/$nombre-$version.docx"
pandoc $bloque $Fuente --from markdown --reference-doc=$Plantilla "--resource-path=$carpeta" --output $destino
Remove-Item $bloque

Write-Output "Exportado: $destino ($version, $commit)"
