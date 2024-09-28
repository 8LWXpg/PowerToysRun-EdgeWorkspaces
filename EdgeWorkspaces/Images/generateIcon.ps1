Push-Location
Set-Location $PSScriptRoot
$ErrorActionPreference = 'Stop'

$light = @(
    '2669e9', '00656c', '00713b', '56801e', '8333fd', 'b147c0', 'e2078a', 'c33403', 'b54710',
    '8e4d04', '5c5957', '383838', '044e8b', '000000'
)
$dark = @(
    '6aa1f8', '57d2da', '58dfa0', 'a3cb6d', 'ab85fd', 'cf87d8', 'ee5fb5', 'e8825e', 'de8d64', 
    'feb967', '9d9a98', 'dedede', 'c6dbec', 'ffffff'
)
# the index is not in order with the UI
# $index[$uiIndex] = $colorIndex
$index = @(
    0, 1, 11, 2, 10, 3, 4, 8, 6,
    5, 7, 9, 13, 12
)
$colors = @{
    light = New-Object string[] $index.Count
    dark  = New-Object string[] $index.Count
}
# reorder the colors
for ($i = 0; $i -lt $index.Count; $i++) {
    $colors.light[$index[$i]] = $light[$i]
    $colors.dark[$index[$i]] = $dark[$i]
}

$svg = Get-Content -Raw ./Template.svg
foreach ($k in $colors.Keys) {
    0..($colors[$k].Count - 1) | ForEach-Object -Parallel {
        "Generating $using:k/$_.png"
        $using:svg -f ($using:colors)[$using:k][$_] | & 'C:\Program Files\Inkscape\bin\inkscape.com' `
            --pipe --export-type=png --export-filename=./$using:k/$_.png `
            --export-width=48 --export-height=48 --export-png-color-mode=RGBA_8
    }
    oxipng -o max ./$k/*.png
}

Pop-Location