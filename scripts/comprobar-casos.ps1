param([string]$Carpeta)

$casos = Select-String -Path "$Carpeta/spec.md" -Pattern 'C-\d+' -AllMatches |
    ForEach-Object { $_.Matches.Value } | Sort-Object -Unique
$plan = Get-Content "$Carpeta/plan.md" -Raw

$sinTarea = $casos | Where-Object { $plan -notmatch $_ }
if ($sinTarea) { "Sin tarea: $($sinTarea -join ', ')"; exit 1 }
"Todos los casos tienen tarea: $($casos.Count)"
