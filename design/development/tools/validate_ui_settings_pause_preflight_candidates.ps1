Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_85_settings_pause_preflight_2026-06-25"
$batchDirRelative = "design/development/asset_candidates/ui/settings_screen/$batchSlug"
$batchDir = Join-Path $projectRoot ($batchDirRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)
$manifestRelative = "$batchDirRelative/thecat_ui_settings_pause_batch85_manifest.csv"
$manifestPath = Join-Path $projectRoot ($manifestRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)
$contactSheetRelative = "$batchDirRelative/thecat_ui_settings_pause_batch85_contact_sheet_v001.png"
$reviewSheetRelative = "$batchDirRelative/thecat_ui_settings_pause_batch85_review_sheet_v001.png"
$reviewNoteRelative = "$batchDirRelative/thecat_ui_settings_pause_batch85_candidate_review.md"
$processNoteRelative = "$batchDirRelative/thecat_ui_settings_pause_batch85_process_note.md"
$specRelative = "$batchDirRelative/thecat_ui_settings_pause_batch85_agent_review_prompt.md"
$settingsControlsManifestRelative = "design/development/asset_candidates/ui/settings_controls/batch_78_settings_control_candidates_2026-06-24/settings_controls_batch78_manifest.csv"
$systemIconsManifestRelative = "design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/system_icons_batch79_manifest.csv"

$expectedVariants = [ordered]@{
    screen_panel_frame = "960x640"
    tab_bar_frame = "760x80"
    option_row_frame = "840x96"
    confirm_modal_frame = "720x420"
    key_hint_chip_frame = "256x72"
    settings_section_divider = "640x24"
    settings_main_1920x1080 = "1920x1080"
    settings_audio_1365x768 = "1365x768"
    pause_overlay_1280x720 = "1280x720"
    settings_compact_1024x768 = "1024x768"
}

$expectedTypes = @{
    screen_panel_frame = "sprite"
    tab_bar_frame = "sprite"
    option_row_frame = "sprite"
    confirm_modal_frame = "sprite"
    key_hint_chip_frame = "sprite"
    settings_section_divider = "sprite"
    settings_main_1920x1080 = "local_mockup"
    settings_audio_1365x768 = "local_mockup"
    pause_overlay_1280x720 = "local_mockup"
    settings_compact_1024x768 = "local_mockup"
}

$baseSourcePaths = @(
    "Assets/TheCat/Art/UI/Backgrounds/thecat_ui_mainmenu_dreamentry_bg_1920x1080_v001.png",
    "Assets/TheCat/Art/UI/Frames/thecat_ui_panel_dreamglass_512x256_v001.png",
    "Assets/TheCat/Art/UI/Frames/thecat_ui_button_primary_384x96_v001.png",
    "Assets/TheCat/Art/UI/Frames/thecat_ui_title_logo_512x256_v001.png",
    $settingsControlsManifestRelative,
    $systemIconsManifestRelative
)

$controlIds = @("slider_track", "slider_knob", "switch_off", "switch_on", "checkbox_unchecked", "checkbox_checked")
$iconIds = @("settings", "sound", "mute", "back", "close", "pause", "continue", "retry")

$failures = New-Object System.Collections.Generic.List[string]

function Add-Failure {
    param([string]$Message)
    $failures.Add($Message)
}

function Resolve-ProjectPath {
    param([string]$RelativePath)

    if ([string]::IsNullOrWhiteSpace($RelativePath)) {
        return $null
    }

    if ([System.IO.Path]::IsPathRooted($RelativePath)) {
        Add-Failure "Path must be project-relative: $RelativePath"
        return $null
    }

    $full = [System.IO.Path]::GetFullPath((Join-Path $projectRoot ($RelativePath -replace "/", [System.IO.Path]::DirectorySeparatorChar)))
    $rootFull = [System.IO.Path]::GetFullPath($projectRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar, [System.IO.Path]::AltDirectorySeparatorChar)
    $rootPrefix = $rootFull + [System.IO.Path]::DirectorySeparatorChar
    if (-not ($full.StartsWith($rootPrefix, [System.StringComparison]::OrdinalIgnoreCase) -or $full -eq $rootFull)) {
        Add-Failure "Path escapes project root: $RelativePath"
        return $null
    }

    return $full
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

function Test-Hash {
    param(
        [string]$Path,
        [string]$ExpectedHash,
        [string]$Label
    )

    if (-not (Test-Path -LiteralPath $Path)) {
        Add-Failure "$Label missing: $Path"
        return
    }

    $actual = (Get-FileHash -Algorithm SHA256 -LiteralPath $Path).Hash.ToLowerInvariant()
    if ($actual -ne $ExpectedHash.ToLowerInvariant()) {
        Add-Failure "$Label hash mismatch. Expected $ExpectedHash but found $actual"
    }
}

function Test-TransparentCorners {
    param([string]$Path)

    $bitmap = [System.Drawing.Bitmap]::new($Path)
    try {
        $points = @(
            @(0, 0),
            @(($bitmap.Width - 1), 0),
            @(0, ($bitmap.Height - 1)),
            @(($bitmap.Width - 1), ($bitmap.Height - 1))
        )

        foreach ($point in $points) {
            $pixel = $bitmap.GetPixel($point[0], $point[1])
            if ($pixel.A -gt 24) {
                Add-Failure "$Path should have transparent sprite corners but found alpha $($pixel.A) at $($point[0]),$($point[1])"
            }
        }
    } finally {
        $bitmap.Dispose()
    }
}

function Test-VisibleAlpha {
    param([string]$Path)

    $bitmap = [System.Drawing.Bitmap]::new($Path)
    try {
        $visible = 0
        $semiTransparent = 0
        for ($y = 0; $y -lt $bitmap.Height; $y += [Math]::Max(1, [Math]::Floor($bitmap.Height / 32))) {
            for ($x = 0; $x -lt $bitmap.Width; $x += [Math]::Max(1, [Math]::Floor($bitmap.Width / 64))) {
                $alpha = $bitmap.GetPixel($x, $y).A
                if ($alpha -gt 24) {
                    $visible++
                }
                if ($alpha -gt 24 -and $alpha -lt 250) {
                    $semiTransparent++
                }
            }
        }

        if ($visible -lt 3) {
            Add-Failure "$Path has too little visible alpha coverage"
        }

        if ($semiTransparent -lt 1) {
            Add-Failure "$Path should preserve antialiased or translucent alpha"
        }
    } finally {
        $bitmap.Dispose()
    }
}

function Get-RequiredSourcePaths {
    $paths = New-Object System.Collections.Generic.List[string]
    foreach ($path in $baseSourcePaths) {
        $paths.Add($path)
    }

    $settingsControlsManifestPath = Resolve-ProjectPath $settingsControlsManifestRelative
    if ($null -eq $settingsControlsManifestPath -or -not (Test-Path -LiteralPath $settingsControlsManifestPath)) {
        Add-Failure "Missing Batch 78 settings controls manifest: $settingsControlsManifestRelative"
    } else {
        $settingsRows = @(Import-Csv -LiteralPath $settingsControlsManifestPath)
        foreach ($controlId in $controlIds) {
            $matches = @($settingsRows | Where-Object { $_.control_id -eq $controlId })
            if ($matches.Count -ne 1) {
                Add-Failure "Expected exactly one Batch 78 control row for $controlId but found $($matches.Count)"
            } else {
                $paths.Add($matches[0].candidate_path)
            }
        }
    }

    $systemIconsManifestPath = Resolve-ProjectPath $systemIconsManifestRelative
    if ($null -eq $systemIconsManifestPath -or -not (Test-Path -LiteralPath $systemIconsManifestPath)) {
        Add-Failure "Missing Batch 79 system icons manifest: $systemIconsManifestRelative"
    } else {
        $iconRows = @(Import-Csv -LiteralPath $systemIconsManifestPath)
        foreach ($iconId in $iconIds) {
            $matches = @($iconRows | Where-Object { $_.icon_id -eq $iconId -and $_.size_variant -eq "64" })
            if ($matches.Count -ne 1) {
                Add-Failure "Expected exactly one Batch 79 64px icon row for $iconId but found $($matches.Count)"
            } else {
                $paths.Add($matches[0].candidate_path)
            }
        }
    }

    return @($paths)
}

if (-not (Test-Path -LiteralPath $manifestPath)) {
    throw "Batch 85 manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath)
if ($rows.Count -ne 10) {
    Add-Failure "Expected 10 Batch 85 rows but found $($rows.Count)"
}

foreach ($variant in $expectedVariants.Keys) {
    $match = @($rows | Where-Object { $_.variant_id -eq $variant })
    if ($match.Count -ne 1) {
        Add-Failure "Expected exactly one row for variant $variant but found $($match.Count)"
        continue
    }

    $row = $match[0]
    if ($row.target_size -ne $expectedVariants[$variant]) {
        Add-Failure "$variant target_size should be $($expectedVariants[$variant]) but found $($row.target_size)"
    }

    if ($row.asset_type -ne $expectedTypes[$variant]) {
        Add-Failure "$variant asset_type should be $($expectedTypes[$variant]) but found $($row.asset_type)"
    }

    if ($row.batch_slug -ne $batchSlug) {
        Add-Failure "$variant batch_slug mismatch: $($row.batch_slug)"
    }

    if ($row.contact_sheet -ne $contactSheetRelative) {
        Add-Failure "$variant contact_sheet should be $contactSheetRelative but found $($row.contact_sheet)"
    }

    $contactSheetPath = Resolve-ProjectPath $contactSheetRelative
    if ($null -ne $contactSheetPath -and (Test-Path -LiteralPath $contactSheetPath)) {
        Test-Hash $contactSheetPath $row.contact_sheet_sha256 "$variant contact sheet"
    }

    if ($row.review_sheet -ne $reviewSheetRelative) {
        Add-Failure "$variant review_sheet should be $reviewSheetRelative but found $($row.review_sheet)"
    }

    $reviewSheetPath = Resolve-ProjectPath $reviewSheetRelative
    if ($null -ne $reviewSheetPath -and (Test-Path -LiteralPath $reviewSheetPath)) {
        Test-Hash $reviewSheetPath $row.review_sheet_sha256 "$variant review sheet"
    }

    if ($row.review_note -ne $reviewNoteRelative) {
        Add-Failure "$variant review_note should be $reviewNoteRelative but found $($row.review_note)"
    }

    if ($row.process_note -ne $processNoteRelative) {
        Add-Failure "$variant process_note should be $processNoteRelative but found $($row.process_note)"
    }

    if ($row.source_model -ne "deterministic_local_derivative_from_batch78_imagegen_not_image2") {
        Add-Failure "$variant must not claim image2 provenance. source_model=$($row.source_model)"
    }

    if ($row.recommendation -ne "candidate_only_pending_unity_settings_pause_screenshots") {
        Add-Failure "$variant recommendation mismatch: $($row.recommendation)"
    }

    if ($row.candidate_path -notlike "$batchDirRelative/*") {
        Add-Failure "$variant candidate_path must stay in Batch 85 directory: $($row.candidate_path)"
    }

    if ($row.candidate_path -like "Assets/*") {
        Add-Failure "$variant candidate_path must not be under Assets: $($row.candidate_path)"
    }

    $candidatePath = Resolve-ProjectPath $row.candidate_path
    if ($null -eq $candidatePath) {
        continue
    }

    if (-not (Test-Path -LiteralPath $candidatePath)) {
        Add-Failure "$variant candidate file missing: $($row.candidate_path)"
        continue
    }

    if ($candidatePath.EndsWith(".meta", [System.StringComparison]::OrdinalIgnoreCase)) {
        Add-Failure "$variant points to a .meta file"
    }

    Test-Hash $candidatePath $row.candidate_sha256 "$variant candidate"

    $actualSize = Get-ImageSize $candidatePath
    if ($actualSize -ne $row.candidate_size) {
        Add-Failure "$variant candidate_size mismatch. Manifest $($row.candidate_size), actual $actualSize"
    }

    if ($actualSize -ne $expectedVariants[$variant]) {
        Add-Failure "$variant actual image size should be $($expectedVariants[$variant]) but found $actualSize"
    }

    if ($row.asset_type -eq "sprite") {
        Test-TransparentCorners $candidatePath
        Test-VisibleAlpha $candidatePath
    }
}

$manifestPaths = @($rows | ForEach-Object { $_.candidate_path })
$actualPngs = @(Get-ChildItem -LiteralPath $batchDir -Recurse -File -Filter *.png | ForEach-Object {
        $_.FullName.Substring($projectRoot.Length + 1).Replace([System.IO.Path]::DirectorySeparatorChar, "/")
    })
foreach ($png in $actualPngs) {
    if ($png -in @($contactSheetRelative, $reviewSheetRelative)) {
        continue
    }

    if ($png -notin $manifestPaths) {
        Add-Failure "Found orphan Batch 85 PNG not listed in manifest: $png"
    }
}

foreach ($sharedPath in @($contactSheetRelative, $reviewSheetRelative, $reviewNoteRelative, $processNoteRelative, $specRelative)) {
    $full = Resolve-ProjectPath $sharedPath
    if ($null -ne $full -and -not (Test-Path -LiteralPath $full)) {
        Add-Failure "Missing shared Batch 85 evidence: $sharedPath"
    }
}

$sourcePaths = Get-RequiredSourcePaths
$expectedSourceAssetString = ($sourcePaths -join ";")
$expectedSourceHashString = (($sourcePaths | ForEach-Object {
            $full = Resolve-ProjectPath $_
            if ($null -eq $full -or -not (Test-Path -LiteralPath $full)) {
                Add-Failure "Missing source asset: $_"
                ""
            } else {
                (Get-FileHash -Algorithm SHA256 -LiteralPath $full).Hash.ToLowerInvariant()
            }
        }) -join ";")

foreach ($row in $rows) {
    if ($row.source_assets -ne $expectedSourceAssetString) {
        Add-Failure "$($row.variant_id) source_assets mismatch"
    }
    if ($row.source_sha256 -ne $expectedSourceHashString) {
        Add-Failure "$($row.variant_id) source_sha256 mismatch"
    }
}

$processPath = Resolve-ProjectPath $processNoteRelative
if ($null -ne $processPath -and (Test-Path -LiteralPath $processPath)) {
    $processText = Get-Content -LiteralPath $processPath -Raw
    foreach ($token in @("settings_pause", "Qr1", "candidate-only", "no baked Chinese text", "not image2 provenance", "Unity-rendered settings/pause screenshots")) {
        if ($processText -notlike "*$token*") {
            Add-Failure "Process note missing control token: $token"
        }
    }
}

$reviewPath = Resolve-ProjectPath $reviewNoteRelative
if ($null -ne $reviewPath -and (Test-Path -LiteralPath $reviewPath)) {
    $reviewText = Get-Content -LiteralPath $reviewPath -Raw
    foreach ($token in @("settings main", "audio settings", "pause overlay", "compact settings", "1024x768")) {
        if ($reviewText -notlike "*$token*") {
            Add-Failure "Review note missing expected Batch 85 token: $token"
        }
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -File -Filter *.meta)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 85 candidate directory must not contain Unity .meta files"
}

if ($failures.Count -gt 0) {
    Write-Error ("Batch 85 settings/pause preflight validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "Batch 85 settings/pause preflight validation passed. Rows: $($rows.Count)"
Write-Output $manifestRelative
