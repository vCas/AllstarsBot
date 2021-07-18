Param(
    [string[]]$Players,
    $RandomChamps,
    $BannedChamps
)

$Version = Invoke-WebRequest -Uri "https://ddragon.leagueoflegends.com/api/versions.json" -ContentType "application/json" -UseBasicParsing | ConvertFrom-Json
$ChampUrl = "http://ddragon.leagueoflegends.com/cdn/$( $Version[0] )/data/en_US/champion.json"
$ChampionList = Invoke-WebRequest -Uri $ChampUrl -ContentType "application/json" -UseBasicParsing | ConvertFrom-Json | Select-Object -ExpandProperty data | % { $_.psobject.properties.Value.Name }

for ($i = 0; $i -lt 5; $i++) {
    $ChampionList = $ChampionList | Sort-Object { Get-Random }
}

if (!$RandomChamps) {
    if (!$Players) {
        $RandomChamps = 12
    }
    else {
        $RandomChamps = ($Players.Count) + 2
    }
}

function Get-ChampionNumber {
    param ($i)
    $c = $ChampionList[$i]
    if ($BannedChamps -and $BannedChamps -contains $c) {
        $i = $i + 1
        return Get-ChampionNumber $i
    }
    return $i
}

$Team1
$Team2

if ($Players) {
    $Players = $Players | Sort-Object { Get-Random }
    $Team1 = $Players[0..([int]($Players.length / 2) - 1)]
    $Team2 = $Players[([int]($Players.length / 2))..($Players.Length - 1)]
}
$Team1Str = "  Team 1: $( $Team1 -join ', ' )"
$Team2Str = "  Team 2: $( $Team2 -join ', ' )"
$Padding = if ($Team1Str.Length -gt $Team2Str.Length) { $Team1Str.Length + 2 }
else { $Team2Str.Length + 2 }

Write-Host ("-" * $Padding)
Write-Host $Team1Str
Write-Host ("-" * $Padding)

$RollTeam2 = $false
$i = 0
for ($j = 0; $j -lt ($RandomChamps * 2); $j++) {
    if ($j -ge $RandomChamps -and -not$RollTeam2) {
        Write-Host ("-" * $Padding)
        Write-Host $Team2Str
        Write-Host ("-" * $Padding)
        $RollTeam2 = $true
        $i = 0
    }
    $j = Get-ChampionNumber $j
    $i++
    Write-Host "$( $i )`t $( $ChampionList[$j] )"
    
}
