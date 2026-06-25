param(
    [string]$BatchDir = "design/development/asset_candidates/ui/skill_slot_frames/batch_81_skill_slot_frame_candidates_2026-06-25"
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path -LiteralPath $BatchDir)) {
    throw "Batch directory not found: $BatchDir"
}

$manifestPath = Join-Path $BatchDir "thecat_ui_skill_slot_frames_batch81_manifest.csv"
if (-not (Test-Path -LiteralPath $manifestPath)) {
    throw "Manifest not found: $manifestPath"
}

$rows = Import-Csv -LiteralPath $manifestPath
if ($rows.Count -ne 24) {
    throw "Expected 24 frame manifest rows, found $($rows.Count)"
}

Add-Type -AssemblyName System.Drawing
foreach ($size in @("256", "128", "64")) {
    $dir = Join-Path $BatchDir "frames/frames_$size"
    if (-not (Test-Path -LiteralPath $dir)) {
        throw "Missing frame size directory: $dir"
    }
    $files = Get-ChildItem -LiteralPath $dir -Filter "*.png"
    if ($files.Count -ne 8) {
        throw "Expected 8 frame PNGs in $dir, found $($files.Count)"
    }
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

$v002ManifestPath = Join-Path $BatchDir "thecat_ui_skill_slot_frames_batch81_manifest_v002_light.csv"
if (-not (Test-Path -LiteralPath $v002ManifestPath)) {
    throw "V002 manifest not found: $v002ManifestPath"
}
$v002Rows = Import-Csv -LiteralPath $v002ManifestPath
if ($v002Rows.Count -ne 12) {
    throw "Expected 12 v002_light frame manifest rows, found $($v002Rows.Count)"
}
$v002AssetIds = @{}
foreach ($row in $v002Rows) {
    if ($v002AssetIds.ContainsKey($row.asset_id)) {
        throw "Duplicate v002_light asset_id: $($row.asset_id)"
    }
    $v002AssetIds[$row.asset_id] = $true
    if ($row.path -like "Assets/*" -or $row.path -like "Assets\*") {
        throw "V002_light manifest path must not point into Assets: $($row.path)"
    }
    if ($row.path -notlike "design/development/asset_candidates/ui/skill_slot_frames/*") {
        throw "V002_light manifest path must stay under the candidate batch: $($row.path)"
    }
    $path = Join-Path (Get-Location) $row.path
    if (-not (Test-Path -LiteralPath $path)) {
        throw "V002_light manifest path not found: $($row.path)"
    }
}

function Test-TransparentBorder {
    param(
        [string]$Path,
        [int]$RequiredTransparentBorder
    )

    $bitmap = [System.Drawing.Bitmap]::FromFile($Path)
    try {
        for ($x = 0; $x -lt $bitmap.Width; $x++) {
            for ($offset = 0; $offset -lt $RequiredTransparentBorder; $offset++) {
                if ($bitmap.GetPixel($x, $offset).A -ne 0) {
                    throw "Non-transparent top border pixel in $Path"
                }
                if ($bitmap.GetPixel($x, $bitmap.Height - 1 - $offset).A -ne 0) {
                    throw "Non-transparent bottom border pixel in $Path"
                }
            }
        }
        for ($y = 0; $y -lt $bitmap.Height; $y++) {
            for ($offset = 0; $offset -lt $RequiredTransparentBorder; $offset++) {
                if ($bitmap.GetPixel($offset, $y).A -ne 0) {
                    throw "Non-transparent left border pixel in $Path"
                }
                if ($bitmap.GetPixel($bitmap.Width - 1 - $offset, $y).A -ne 0) {
                    throw "Non-transparent right border pixel in $Path"
                }
            }
        }
    }
    finally {
        $bitmap.Dispose()
    }
}

$v002AlphaPath = Join-Path $BatchDir "source/thecat_ui_skill_slot_frames_batch81_alpha_sheet_v002_square_light.png"
if (-not (Test-Path -LiteralPath $v002AlphaPath)) {
    throw "V002 alpha source not found: $v002AlphaPath"
}
$v002AlphaImage = [System.Drawing.Image]::FromFile($v002AlphaPath)
try {
    if ($v002AlphaImage.Width -ne 2007 -or $v002AlphaImage.Height -ne 784) {
        throw "Unexpected v002 alpha source dimensions: $($v002AlphaImage.Width)x$($v002AlphaImage.Height)"
    }
}
finally {
    $v002AlphaImage.Dispose()
}
Test-TransparentBorder -Path $v002AlphaPath -RequiredTransparentBorder 1

foreach ($size in @("256", "128", "64")) {
    $dir = Join-Path $BatchDir "frames/frames_square_v002_light_$size"
    if (-not (Test-Path -LiteralPath $dir)) {
        throw "Missing v002_light frame size directory: $dir"
    }
    $files = Get-ChildItem -LiteralPath $dir -Filter "*.png"
    if ($files.Count -ne 4) {
        throw "Expected 4 v002_light frame PNGs in $dir, found $($files.Count)"
    }
    foreach ($file in $files) {
        $img = [System.Drawing.Image]::FromFile($file.FullName)
        try {
            if ($img.Width -ne [int]$size -or $img.Height -ne [int]$size) {
                throw "Wrong v002_light dimensions for $($file.Name): $($img.Width)x$($img.Height)"
            }
        }
        finally {
            $img.Dispose()
        }
        Test-TransparentBorder -Path $file.FullName -RequiredTransparentBorder 1
    }
}

$fitReportPath = Join-Path $BatchDir "icon_fit_test/thecat_ui_skill_slot_frames_batch81_icon_fit_report.csv"
if (-not (Test-Path -LiteralPath $fitReportPath)) {
    throw "Fit report not found: $fitReportPath"
}
$fitRows = Import-Csv -LiteralPath $fitReportPath
if ($fitRows.Count -ne 72) {
    throw "Expected 72 icon-fit rows, found $($fitRows.Count)"
}

foreach ($subdir in @("square_ready_128", "round_ready_128", "square_cooldown_128", "round_cooldown_128")) {
    $dir = Join-Path $BatchDir "icon_fit_test/$subdir"
    if (-not (Test-Path -LiteralPath $dir)) {
        throw "Missing icon-fit directory: $dir"
    }
    $files = Get-ChildItem -LiteralPath $dir -Filter "*.png"
    if ($files.Count -ne 18) {
        throw "Expected 18 icon-fit PNGs in $dir, found $($files.Count)"
    }
    foreach ($file in $files) {
        $img = [System.Drawing.Image]::FromFile($file.FullName)
        try {
            if ($img.Width -ne 128 -or $img.Height -ne 128) {
                throw "Wrong icon-fit dimensions for $($file.Name): $($img.Width)x$($img.Height)"
            }
        }
        finally {
            $img.Dispose()
        }
    }
}

$v002FitReportPath = Join-Path $BatchDir "icon_fit_test_v002_light/thecat_ui_skill_slot_frames_batch81_icon_fit_report_v002_light.csv"
if (-not (Test-Path -LiteralPath $v002FitReportPath)) {
    throw "V002 fit report not found: $v002FitReportPath"
}
$v002FitRows = Import-Csv -LiteralPath $v002FitReportPath
if ($v002FitRows.Count -ne 72) {
    throw "Expected 72 v002_light icon-fit rows, found $($v002FitRows.Count)"
}
foreach ($row in $v002FitRows) {
    if ($row.path -like "Assets/*" -or $row.path -like "Assets\*") {
        throw "V002_light icon-fit path must not point into Assets: $($row.path)"
    }
    if ($row.path -notlike "design/development/asset_candidates/ui/skill_slot_frames/*") {
        throw "V002_light icon-fit path must stay under the candidate batch: $($row.path)"
    }
    $path = Join-Path (Get-Location) $row.path
    if (-not (Test-Path -LiteralPath $path)) {
        throw "V002_light icon-fit path not found: $($row.path)"
    }
}

foreach ($subdir in @("ready", "cooldown", "disabled", "selected")) {
    $dir = Join-Path $BatchDir "icon_fit_test_v002_light/$subdir"
    if (-not (Test-Path -LiteralPath $dir)) {
        throw "Missing v002_light icon-fit directory: $dir"
    }
    $files = Get-ChildItem -LiteralPath $dir -Filter "*.png"
    if ($files.Count -ne 18) {
        throw "Expected 18 v002_light icon-fit PNGs in $dir, found $($files.Count)"
    }
    foreach ($file in $files) {
        $img = [System.Drawing.Image]::FromFile($file.FullName)
        try {
            if ($img.Width -ne 128 -or $img.Height -ne 128) {
                throw "Wrong v002_light icon-fit dimensions for $($file.Name): $($img.Width)x$($img.Height)"
            }
        }
        finally {
            $img.Dispose()
        }
    }
}

$digitReportPath = Join-Path $BatchDir "cooldown_digit_test/thecat_ui_skill_slot_frames_batch81_square_cooldown_digit_report.csv"
if (-not (Test-Path -LiteralPath $digitReportPath)) {
    throw "Cooldown digit report not found: $digitReportPath"
}
$digitRows = Import-Csv -LiteralPath $digitReportPath
if ($digitRows.Count -ne 54) {
    throw "Expected 54 cooldown digit rows, found $($digitRows.Count)"
}

foreach ($digit in @("1", "12", "99")) {
    $dir = Join-Path $BatchDir "cooldown_digit_test/digit_$digit"
    if (-not (Test-Path -LiteralPath $dir)) {
        throw "Missing cooldown digit directory: $dir"
    }
    $files = Get-ChildItem -LiteralPath $dir -Filter "*.png"
    if ($files.Count -ne 18) {
        throw "Expected 18 cooldown digit PNGs in $dir, found $($files.Count)"
    }
    foreach ($file in $files) {
        $img = [System.Drawing.Image]::FromFile($file.FullName)
        try {
            if ($img.Width -ne 128 -or $img.Height -ne 128) {
                throw "Wrong cooldown digit dimensions for $($file.Name): $($img.Width)x$($img.Height)"
            }
        }
        finally {
            $img.Dispose()
        }
    }
}

$v002DigitReportPath = Join-Path $BatchDir "cooldown_digit_test_v002_light/thecat_ui_skill_slot_frames_batch81_square_cooldown_digit_report_v002_light.csv"
if (-not (Test-Path -LiteralPath $v002DigitReportPath)) {
    throw "V002 cooldown digit report not found: $v002DigitReportPath"
}
$v002DigitRows = Import-Csv -LiteralPath $v002DigitReportPath
if ($v002DigitRows.Count -ne 54) {
    throw "Expected 54 v002_light cooldown digit rows, found $($v002DigitRows.Count)"
}
foreach ($row in $v002DigitRows) {
    if ($row.path -like "Assets/*" -or $row.path -like "Assets\*") {
        throw "V002_light cooldown digit path must not point into Assets: $($row.path)"
    }
    if ($row.path -notlike "design/development/asset_candidates/ui/skill_slot_frames/*") {
        throw "V002_light cooldown digit path must stay under the candidate batch: $($row.path)"
    }
    $path = Join-Path (Get-Location) $row.path
    if (-not (Test-Path -LiteralPath $path)) {
        throw "V002_light cooldown digit path not found: $($row.path)"
    }
}

foreach ($digit in @("1", "12", "99")) {
    $dir = Join-Path $BatchDir "cooldown_digit_test_v002_light/digit_$digit"
    if (-not (Test-Path -LiteralPath $dir)) {
        throw "Missing v002_light cooldown digit directory: $dir"
    }
    $files = Get-ChildItem -LiteralPath $dir -Filter "*.png"
    if ($files.Count -ne 18) {
        throw "Expected 18 v002_light cooldown digit PNGs in $dir, found $($files.Count)"
    }
    foreach ($file in $files) {
        $img = [System.Drawing.Image]::FromFile($file.FullName)
        try {
            if ($img.Width -ne 128 -or $img.Height -ne 128) {
                throw "Wrong v002_light cooldown digit dimensions for $($file.Name): $($img.Width)x$($img.Height)"
            }
        }
        finally {
            $img.Dispose()
        }
    }
}

$required = @(
    "source/thecat_ui_skill_slot_frames_batch81_chromakey_source_v001.png",
    "source/thecat_ui_skill_slot_frames_batch81_alpha_sheet_v001.png",
    "source/thecat_ui_skill_slot_frames_batch81_chromakey_source_v002_square_light.png",
    "source/thecat_ui_skill_slot_frames_batch81_alpha_sheet_v002_square_light.png",
    "frames/thecat_ui_skill_slot_frames_batch81_contact_sheet_v001.png",
    "frames/thecat_ui_skill_slot_frames_batch81_contact_sheet_v002_light.png",
    "icon_fit_test/thecat_ui_skill_slot_frames_batch81_icon_fit_board_v001.png",
    "icon_fit_test_v002_light/thecat_ui_skill_slot_frames_batch81_icon_fit_board_v002_light.png",
    "cooldown_digit_test/thecat_ui_skill_slot_frames_batch81_square_cooldown_digit_board_v001.png",
    "cooldown_digit_test/thecat_ui_skill_slot_frames_batch81_square_cooldown_digit_report.csv",
    "cooldown_digit_test_v002_light/thecat_ui_skill_slot_frames_batch81_square_cooldown_digit_board_v002_light.png",
    "cooldown_digit_test_v002_light/thecat_ui_skill_slot_frames_batch81_square_cooldown_digit_report_v002_light.csv",
    "thecat_ui_skill_slot_frames_batch81_process_note.md",
    "thecat_ui_skill_slot_frames_batch81_review.md"
)

foreach ($relative in $required) {
    $path = Join-Path $BatchDir $relative
    if (-not (Test-Path -LiteralPath $path)) {
        throw "Missing required file: $path"
    }
}

$metaFiles = Get-ChildItem -LiteralPath $BatchDir -Recurse -Filter "*.meta"
if ($metaFiles.Count -ne 0) {
    throw "Candidate batch must not include Unity .meta files. Found $($metaFiles.Count)"
}

Write-Output "Skill slot frame Batch 81 validation passed"
