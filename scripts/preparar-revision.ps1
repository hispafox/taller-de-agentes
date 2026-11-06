param(
    [Parameter(Mandatory)][string]$Base,
    [Parameter(Mandatory)][string]$Spec
)

if (git status --porcelain) {
    Write-Output 'Hay cambios sin confirmar. Se revisa un commit, no una carpeta.'
    exit 2
}

$carpeta = 'salida/revision'
New-Item -ItemType Directory -Force $carpeta | Out-Null

git diff --output="$carpeta/cambio.diff" "$Base...HEAD"
dotnet test --logger 'console;verbosity=normal' *> "$carpeta/pruebas.txt"
$resultado = $LASTEXITCODE

@(
    "Commit revisado: $(git rev-parse --short HEAD)"
    "Base: $(git rev-parse --short $Base)"
    "Último commit de la especificación: $(git log -1 --format=%h -- $Spec)"
    "Salida de las pruebas: $resultado"
) | Set-Content "$carpeta/resumen.txt"

exit $resultado
