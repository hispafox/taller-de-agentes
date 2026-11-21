param(
    [ValidateSet('Ruta', 'Agente', 'Modelo')][string]$Por = 'Ruta',
    [string]$Hoja = 'docs/hoja-de-consumo.md',
    [string]$Tarea,
    [string]$Ruta
)

$tabla = Get-Content $Hoja -Encoding utf8 | Where-Object { $_ -match '^\|' }
$columnas = ($tabla[0].Trim().Trim('|') -split '\|').Trim()

$filas = $tabla | Select-Object -Skip 2 | ForEach-Object {
    $celdas = ($_.Trim().Trim('|') -split '\|').Trim()
    $fila = @{}
    for ($i = 0; $i -lt $columnas.Count; $i++) { $fila[$columnas[$i]] = $celdas[$i] }
    if ($Tarea -and $fila['Tarea'] -notlike "*$Tarea*") { return }
    if ($Ruta -and $fila['Ruta'] -ne $Ruta) { return }

    if ($fila['Consumo'] -match '^(\d+) de (\d+)') {
        [pscustomobject]@{
            Grupo    = $fila[$Por]
            Gastado  = [int]$Matches[1]
            Previsto = [int]$Matches[2]
            Sirvio   = $celdas[4] -match '^sí'
        }
    }
    elseif ($fila['Tarea']) {
        Write-Warning "Sin consumo: $($fila['Tarea'])"
    }
}

$filas | Group-Object Grupo | ForEach-Object {
    $gastado = [int]($_.Group | Measure-Object Gastado -Sum).Sum
    $sirvieron = @($_.Group | Where-Object Sirvio).Count
    [pscustomobject]@{
        $Por          = $_.Name
        Filas         = $_.Count
        Gastado       = $gastado
        Previsto      = [int]($_.Group | Measure-Object Previsto -Sum).Sum
        'Sí'          = $sirvieron
        'Por cada sí' = if ($sirvieron) { [int]($gastado / $sirvieron) } else { '-' }
    }
} | Sort-Object Gastado -Descending | Format-Table -AutoSize
