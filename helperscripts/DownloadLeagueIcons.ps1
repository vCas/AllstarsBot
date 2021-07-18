param($RootFolder)

$Version = Invoke-WebRequest -Uri "https://ddragon.leagueoflegends.com/api/versions.json" -ContentType "application/json" -UseBasicParsing | ConvertFrom-Json
$ChampUrl = "http://ddragon.leagueoflegends.com/cdn/$( $Version[0] )/data/en_US/champion.json"
$ChampionList = Invoke-WebRequest -Uri $ChampUrl -ContentType "application/json" -UseBasicParsing | ConvertFrom-Json | Select-Object -ExpandProperty data | % { $_.psobject.properties.Value.Id }

$wc = New-Object System.Net.WebClient
$i = 0;
$j = 0;
foreach ($champ in $ChampionList) {
    
    
    $url = "http://ddragon.leagueoflegends.com/cdn/$( $Version[0] )/img/champion/$( $champ ).png"
    $url
    if ($i -eq 50) {
        $j++
        $i = 0
    }
    
    if (-Not(Test-Path -Path "$RootFolder\$j")) {
        New-Item -Path "$RootFolder\" -Name $j -ItemType "directory" | Out-Null
    }
    
    $wc.DownloadFile($url, "$RootFolder\$( $j )\$( $champ ).png")
    
    $i++
    Write-Host "Created File $( $j )\$( $champ ).png"
}

