param(
    [string]$BatchDir = "design/development/asset_candidates/ui/starter_skill_icon_motifs/batch_80_starter_skill_icon_motifs_2026-06-25"
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path -LiteralPath $BatchDir)) {
    throw "Batch directory not found: $BatchDir"
}

$manifestPath = Join-Path $BatchDir "thecat_ui_starter_skill_icon_motifs_batch80_manifest.csv"
if (-not (Test-Path -LiteralPath $manifestPath)) {
    throw "Manifest not found: $manifestPath"
}

$rows = Import-Csv -LiteralPath $manifestPath
if ($rows.Count -ne 152) {
    throw "Expected 152 manifest rows, found $($rows.Count)"
}

$expectedSizes = @("256", "128", "64", "32")
foreach ($size in $expectedSizes) {
    $dir = Join-Path $BatchDir "icons/icons_$size"
    if (-not (Test-Path -LiteralPath $dir)) {
        throw "Missing icon size directory: $dir"
    }

    $files = Get-ChildItem -LiteralPath $dir -Filter "*.png"
    if ($files.Count -ne 20) {
        throw "Expected 20 PNGs in $dir, found $($files.Count)"
    }

    Add-Type -AssemblyName System.Drawing
    foreach ($file in $files) {
        $img = [System.Drawing.Image]::FromFile($file.FullName)
        try {
            if ($img.Width -ne [int]$size -or $img.Height -ne [int]$size) {
                throw "Wrong dimensions for $($file.Name): $($img.Width)x$($img.Height)"
            }
        }
        finally {
            $img.Dispose()
        }
    }
}

foreach ($size in $expectedSizes) {
    $dir = Join-Path $BatchDir "icons/icons_lightframe_$size"
    if (-not (Test-Path -LiteralPath $dir)) {
        throw "Missing lightframe icon size directory: $dir"
    }

    $files = Get-ChildItem -LiteralPath $dir -Filter "*.png"
    if ($files.Count -ne 18) {
        throw "Expected 18 PNGs in $dir, found $($files.Count)"
    }

    Add-Type -AssemblyName System.Drawing
    foreach ($file in $files) {
        $img = [System.Drawing.Image]::FromFile($file.FullName)
        try {
            if ($img.Width -ne [int]$size -or $img.Height -ne [int]$size) {
                throw "Wrong dimensions for $($file.Name): $($img.Width)x$($img.Height)"
            }
        }
        finally {
            $img.Dispose()
        }
    }
}

foreach ($size in $expectedSizes) {
    $dir = Join-Path $BatchDir "recommended/recommended_$size"
    if (-not (Test-Path -LiteralPath $dir)) {
        throw "Missing recommended icon size directory: $dir"
    }

    $files = Get-ChildItem -LiteralPath $dir -Filter "*.png"
    if ($files.Count -ne 18) {
        throw "Expected 18 PNGs in $dir, found $($files.Count)"
    }

    Add-Type -AssemblyName System.Drawing
    foreach ($file in $files) {
        $img = [System.Drawing.Image]::FromFile($file.FullName)
        try {
            if ($img.Width -ne [int]$size -or $img.Height -ne [int]$size) {
                throw "Wrong dimensions for $($file.Name): $($img.Width)x$($img.Height)"
            }
        }
        finally {
            $img.Dispose()
        }
    }
}

$hudReadyDir = Join-Path $BatchDir "hud_overlay_test/ready_128"
if (-not (Test-Path -LiteralPath $hudReadyDir)) {
    throw "Missing HUD ready test directory: $hudReadyDir"
}
$hudReadyFiles = Get-ChildItem -LiteralPath $hudReadyDir -Filter "*.png"
if ($hudReadyFiles.Count -ne 18) {
    throw "Expected 18 HUD ready test PNGs, found $($hudReadyFiles.Count)"
}

$hudCooldownDir = Join-Path $BatchDir "hud_overlay_test/cooldown_64"
if (-not (Test-Path -LiteralPath $hudCooldownDir)) {
    throw "Missing HUD cooldown test directory: $hudCooldownDir"
}
$hudCooldownFiles = Get-ChildItem -LiteralPath $hudCooldownDir -Filter "*.png"
if ($hudCooldownFiles.Count -ne 18) {
    throw "Expected 18 HUD cooldown test PNGs, found $($hudCooldownFiles.Count)"
}

Add-Type -AssemblyName System.Drawing
foreach ($file in $hudReadyFiles) {
    $img = [System.Drawing.Image]::FromFile($file.FullName)
    try {
        if ($img.Width -ne 128 -or $img.Height -ne 128) {
            throw "Wrong HUD ready dimensions for $($file.Name): $($img.Width)x$($img.Height)"
        }
    }
    finally {
        $img.Dispose()
    }
}
foreach ($file in $hudCooldownFiles) {
    $img = [System.Drawing.Image]::FromFile($file.FullName)
    try {
        if ($img.Width -ne 64 -or $img.Height -ne 64) {
            throw "Wrong HUD cooldown dimensions for $($file.Name): $($img.Width)x$($img.Height)"
        }
    }
    finally {
        $img.Dispose()
    }
}

$metaFiles = Get-ChildItem -LiteralPath $BatchDir -Recurse -Filter "*.meta"
if ($metaFiles.Count -ne 0) {
    throw "Candidate batch must not include Unity .meta files. Found $($metaFiles.Count)"
}

$sourceFiles = @(
    "source/thecat_ui_starter_skill_icon_motifs_batch80_chromakey_source_v001.png",
    "source/thecat_ui_starter_skill_icon_motifs_batch80_alpha_sheet_v001.png",
    "source/thecat_ui_starter_skill_icon_motifs_batch80_chromakey_source_v002.png",
    "source/thecat_ui_starter_skill_icon_motifs_batch80_alpha_sheet_v002.png",
    "source/thecat_ui_starter_skill_icon_motifs_batch80_chromakey_source_v003_lightframe.png",
    "source/thecat_ui_starter_skill_icon_motifs_batch80_alpha_sheet_v003_lightframe.png",
    "icons/thecat_ui_starter_skill_icon_motifs_batch80_contact_sheet_v001.png",
    "icons/thecat_ui_starter_skill_icon_motifs_batch80_v002_contact_sheet.png",
    "icons/thecat_ui_starter_skill_icon_motifs_batch80_v003_lightframe_contact_sheet.png",
    "icons/thecat_ui_starter_skill_icon_motifs_batch80_hud_readability_board_v001.png",
    "icons/thecat_ui_starter_skill_icon_motifs_batch80_lightframe_comparison_board_v001.png",
    "recommended/thecat_ui_starter_skill_icon_motifs_batch80_recommended_contact_sheet_v001.png",
    "recommended/thecat_ui_starter_skill_icon_motifs_batch80_recommended_manifest.csv",
    "hud_overlay_test/thecat_ui_starter_skill_icon_motifs_batch80_battle_hud_overlay_board_v001.png",
    "hud_overlay_test/thecat_ui_starter_skill_icon_motifs_batch80_battle_hud_overlay_report.csv",
    "thecat_ui_starter_skill_icon_motifs_batch80_hud_readability_report.csv",
    "thecat_ui_starter_skill_icon_motifs_batch80_process_note.md",
    "thecat_ui_starter_skill_icon_motifs_batch80_review.md"
)

foreach ($relative in $sourceFiles) {
    $path = Join-Path $BatchDir $relative
    if (-not (Test-Path -LiteralPath $path)) {
        throw "Missing required file: $path"
    }
}

Write-Output "Starter skill icon motif Batch 80 validation passed"
