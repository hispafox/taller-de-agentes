$carpeta = 'salida/revision'
New-Item -ItemType Directory -Force $carpeta | Out-Null
Remove-Item "$carpeta/paquetes-*.txt", "$carpeta/secretos.json" -ErrorAction SilentlyContinue

function Instalado($orden) {
    if (Get-Command $orden -ErrorAction SilentlyContinue) { return $true }
    Write-Warning "Falta ${orden}: esa zona queda sin comprobar."
    return $false
}

if (Instalado 'dotnet') {
    dotnet list package --vulnerable --include-transitive *> "$carpeta/paquetes-dotnet.txt"
}
if (Instalado 'npm') {
    npm audit --prefix src/interfaz *> "$carpeta/paquetes-npm.txt"
}
if (Instalado 'gitleaks') {
    gitleaks git --report-path "$carpeta/secretos.json"
}
