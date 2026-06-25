Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$sourceDir = Join-Path $projectRoot "Assets/TheCat/Art/Enemies/Frames"
$slicedDir = Join-Path $sourceDir "Sliced"
$reviewDir = Join-Path $projectRoot "design/development/asset_review"
$manifestPath = Join-Path $reviewDir "ENEMY_FRAMESHEET_SLICED_SPRITES_2026-06-25.csv"
$policyPath = Join-Path $reviewDir "ENEMY_FRAMESHEET_IMPORT_POLICY_2026-06-25.md"
$contactSheetPath = Join-Path $reviewDir "ENEMY_FRAMESHEET_SLICED_SPRITES_2026-06-25.png"

$expectedSources = @(
    @{ EnemyId = "black_mud_nightmare"; AnimationId = "black_mud_move"; File = "thecat_enemy_blackmud_move_framesheet_4x256_v001.png"; SourceLock = "black_mud_animation" },
    @{ EnemyId = "cold_light_shadow"; AnimationId = "cold_light_cast"; File = "thecat_enemy_coldlight_cast_framesheet_4x256_v001.png"; SourceLock = "cold_light_animation" },
    @{ EnemyId = "call_tyrant"; AnimationId = "call_tyrant_boss_pattern"; File = "thecat_enemy_calltyrant_bosspattern_framesheet_4x256_v001.png"; SourceLock = "call_tyrant_animation" }
)

$failures = New-Object System.Collections.Generic.List[string]

function Add-Failure {
    param([string]$Message)
    $failures.Add($Message)
}

function ConvertTo-ProjectRelativePath {
    param([string]$Path)

    $fullPath = (Resolve-Path -LiteralPath $Path).Path
    $rootPath = $projectRoot.TrimEnd([System.IO.Path]::DirectorySeparatorChar, [System.IO.Path]::AltDirectorySeparatorChar) + [System.IO.Path]::DirectorySeparatorChar
    $rootUri = [System.Uri]::new($rootPath)
    $fileUri = [System.Uri]::new($fullPath)
    return [System.Uri]::UnescapeDataString($rootUri.MakeRelativeUri($fileUri).ToString()).Replace("\", "/")
}

function Get-ImageSize {
    param([string]$Path)

    $image = [System.Drawing.Image]::FromFile($Path)
    try {
        return "$($image.Width)x$($image.Height)"
    } finally {
        $image.Dispose()
    }
}

function Test-MetaToken {
    param(
        [string]$MetaPath,
        [string]$Token,
        [string]$Label
    )

    if (-not (Test-Path -LiteralPath $MetaPath)) {
        Add-Failure "$Label missing meta at $MetaPath"
        return
    }

    $content = Get-Content -LiteralPath $MetaPath -Raw
    if ($content -notmatch [regex]::Escape($Token)) {
        Add-Failure "$Label meta missing token: $Token"
    }
}

function Test-TransparentCorners {
    param(
        [string]$ImagePath,
        [string]$Label
    )

    $bitmap = [System.Drawing.Bitmap]::FromFile($ImagePath)
    try {
        if (-not [System.Drawing.Image]::IsAlphaPixelFormat($bitmap.PixelFormat)) {
            Add-Failure "$Label must use an alpha-capable pixel format but was $($bitmap.PixelFormat)"
        }

        $corners = @(
            $bitmap.GetPixel(0, 0),
            $bitmap.GetPixel($bitmap.Width - 1, 0),
            $bitmap.GetPixel(0, $bitmap.Height - 1),
            $bitmap.GetPixel($bitmap.Width - 1, $bitmap.Height - 1)
        )

        foreach ($corner in $corners) {
            if ($corner.A -gt 8) {
                Add-Failure "$Label corner alpha should be transparent but found $($corner.A)"
            }
        }
    } finally {
        $bitmap.Dispose()
    }
}

foreach ($source in $expectedSources) {
    $sourcePath = Join-Path $sourceDir $source.File
    if (-not (Test-Path -LiteralPath $sourcePath)) {
        Add-Failure "Missing source framesheet: $sourcePath"
        continue
    }

    $size = Get-ImageSize $sourcePath
    if ($size -ne "1024x256") {
        Add-Failure "Source framesheet $($source.File) must be 1024x256 but was $size"
    }

    $metaPath = $sourcePath + ".meta"
    Test-MetaToken $metaPath "textureType: 8" "$($source.File) source"
    Test-MetaToken $metaPath "spriteMode: 1" "$($source.File) source"
    Test-MetaToken $metaPath "alphaIsTransparency: 1" "$($source.File) source"
    Test-MetaToken $metaPath "SourceFramesheetSingleSprite" "$($source.File) source"
}

if (-not (Test-Path -LiteralPath $manifestPath)) {
    Add-Failure "Missing split sprite manifest: $manifestPath"
} else {
    $rows = @(Import-Csv -LiteralPath $manifestPath)
    if ($rows.Count -ne 12) {
        Add-Failure "Expected 12 split sprite manifest rows but found $($rows.Count)"
    }

    foreach ($source in $expectedSources) {
        $sourceRows = @($rows | Where-Object { $_.enemy_id -eq $source.EnemyId -and $_.animation_id -eq $source.AnimationId })
        if ($sourceRows.Count -ne 4) {
            Add-Failure "Expected 4 split rows for $($source.AnimationId) but found $($sourceRows.Count)"
        }

        for ($frame = 1; $frame -le 4; $frame++) {
            $row = $sourceRows | Where-Object { [int]$_.frame_index -eq $frame } | Select-Object -First 1
            if ($null -eq $row) {
                Add-Failure "Missing frame $frame row for $($source.AnimationId)"
                continue
            }

            if ($row.source_lock -ne $source.SourceLock) {
                Add-Failure "$($source.AnimationId) frame $frame has source_lock $($row.source_lock), expected $($source.SourceLock)"
            }

            if ($row.output_sprite_path -like "*Art/Characters*" -or $row.output_sprite_path -like "*starter*") {
                Add-Failure "$($source.AnimationId) frame $frame points into a character/starter lane: $($row.output_sprite_path)"
            }

            $spritePath = Join-Path $projectRoot ($row.output_sprite_path -replace "/", [System.IO.Path]::DirectorySeparatorChar)
            if (-not (Test-Path -LiteralPath $spritePath)) {
                Add-Failure "Missing split sprite file: $spritePath"
                continue
            }

            $size = Get-ImageSize $spritePath
            if ($size -ne "256x256") {
                Add-Failure "Split sprite $($row.output_sprite_path) must be 256x256 but was $size"
            }
            Test-TransparentCorners $spritePath $row.output_sprite_path

            $actualHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $spritePath).Hash.ToLowerInvariant()
            if ($actualHash -ne $row.sha256.ToLowerInvariant()) {
                Add-Failure "Hash mismatch for $($row.output_sprite_path): manifest $($row.sha256), actual $actualHash"
            }

            $metaPath = $spritePath + ".meta"
            Test-MetaToken $metaPath "textureType: 8" "$($row.output_sprite_path) split"
            Test-MetaToken $metaPath "spriteMode: 1" "$($row.output_sprite_path) split"
            Test-MetaToken $metaPath "alphaIsTransparency: 1" "$($row.output_sprite_path) split"
            Test-MetaToken $metaPath "maxTextureSize: 256" "$($row.output_sprite_path) split"
            Test-MetaToken $metaPath "EnemyFramesheetSlice" "$($row.output_sprite_path) split"
        }
    }
}

if (-not (Test-Path -LiteralPath $policyPath)) {
    Add-Failure "Missing import policy doc: $policyPath"
} else {
    $policy = Get-Content -LiteralPath $policyPath -Raw
    $requiredPolicyTokens = @(
        "SourceFramesheetSingleSprite",
        "EnemyFramesheetSlice",
        "not Unity runtime acceptance",
        "Starter cat bodies",
        "active enemy animation screenshot",
        "prefab/catalog binding proof",
        "Console check",
        "validate_enemy_framesheet_import_policy.ps1"
    )

    foreach ($token in $requiredPolicyTokens) {
        if ($policy -notmatch [regex]::Escape($token)) {
            Add-Failure "Policy doc missing token: $token"
        }
    }

    if ($policy -match "Assets/TheCat/Art/Characters") {
        Add-Failure "Policy doc must not reference character body asset paths."
    }
}

if (-not (Test-Path -LiteralPath $contactSheetPath)) {
    Add-Failure "Missing split sprite contact sheet: $contactSheetPath"
} else {
    $sheetSize = Get-ImageSize $contactSheetPath
    if ($sheetSize -ne "1120x900") {
        Add-Failure "Contact sheet must be 1120x900 but was $sheetSize"
    }
}

if ($failures.Count -gt 0) {
    Write-Error ("Enemy framesheet import policy validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "Enemy framesheet import policy validation passed. Sources: $($expectedSources.Count) Split sprites: 12"
Write-Output (ConvertTo-ProjectRelativePath $manifestPath)
Write-Output (ConvertTo-ProjectRelativePath $policyPath)
Write-Output (ConvertTo-ProjectRelativePath $contactSheetPath)
