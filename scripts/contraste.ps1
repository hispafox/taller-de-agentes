param([string]$Texto, [string]$Fondo)

function Luminancia([string]$hex) {
    $c = 1, 3, 5 | ForEach-Object {
        $v = [Convert]::ToInt32($hex.Substring($_, 2), 16) / 255
        if ($v -le 0.04045) { $v / 12.92 } else { [Math]::Pow(($v + 0.055) / 1.055, 2.4) }
    }
    0.2126 * $c[0] + 0.7152 * $c[1] + 0.0722 * $c[2]
}

$a = Luminancia $Texto
$b = Luminancia $Fondo
$ratio = ([Math]::Max($a, $b) + 0.05) / ([Math]::Min($a, $b) + 0.05)
[Math]::Floor($ratio * 100) / 100
