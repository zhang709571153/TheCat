param(
    [string]$BatchDir = "design/development/asset_candidates/ui/skill_slot_frames/batch_81_skill_slot_frame_candidates_2026-06-25"
)

$ErrorActionPreference = "Stop"

$MockupDir = Join-Path $BatchDir "actual_scale_hud_test_v002_light"
$BoardsDir = Join-Path $MockupDir "boards"
$SlotsDir = Join-Path $MockupDir "slots_64"
$SlotCsv = Join-Path $MockupDir "thecat_batch81_v002_light_actual_scale_hud_mockup_slots.csv"
$BoardCsv = Join-Path $MockupDir "thecat_batch81_v002_light_actual_scale_hud_mockup_boards.csv"
$NotePath = Join-Path $MockupDir "thecat_batch81_v002_light_actual_scale_hud_mockup_note.md"
$Batch80RecommendedManifest = "design/development/asset_candidates/ui/starter_skill_icon_motifs/batch_80_starter_skill_icon_motifs_2026-06-25/recommended/thecat_ui_starter_skill_icon_motifs_batch80_recommended_manifest.csv"

if (-not (Test-Path -LiteralPath $MockupDir)) {
    throw "Mockup directory not found: $MockupDir"
}
foreach ($path in @($BoardsDir, $SlotsDir, $SlotCsv, $BoardCsv, $NotePath)) {
    if (-not (Test-Path -LiteralPath $path)) {
        throw "Required Batch 81 v002_light HUD mockup evidence missing: $path"
    }
}
if (-not (Test-Path -LiteralPath $Batch80RecommendedManifest)) {
    throw "Batch 80 recommended manifest not found: $Batch80RecommendedManifest"
}

Add-Type -AssemblyName System.Drawing

$WorkspaceRoot = (Get-Location).Path
$MockupRootResolved = (Resolve-Path -LiteralPath $MockupDir).Path
$MockupRootPrefix = $MockupRootResolved.TrimEnd([System.IO.Path]::DirectorySeparatorChar, [System.IO.Path]::AltDirectorySeparatorChar) + [System.IO.Path]::DirectorySeparatorChar

function Test-CandidatePath {
    param([string]$Path)

    if ($Path -like "Assets/*" -or $Path -like "Assets\*") {
        throw "Mockup path must not point into Assets: $Path"
    }
    $candidate = if ([System.IO.Path]::IsPathRooted($Path)) {
        $Path
    }
    else {
        Join-Path $WorkspaceRoot $Path
    }
    if (-not (Test-Path -LiteralPath $candidate)) {
        throw "Mockup path not found: $Path"
    }
    $resolved = (Resolve-Path -LiteralPath $candidate).Path
    if (-not ($resolved.Equals($MockupRootResolved, [System.StringComparison]::OrdinalIgnoreCase) -or $resolved.StartsWith($MockupRootPrefix, [System.StringComparison]::OrdinalIgnoreCase))) {
        throw "Mockup path must stay under exact actual_scale_hud_test_v002_light root: $Path"
    }
    return $resolved
}

function Test-PngDimensions {
    param(
        [string]$Path,
        [int]$Width,
        [int]$Height
    )

    $image = [System.Drawing.Image]::FromFile($Path)
    try {
        if ($image.Width -ne $Width -or $image.Height -ne $Height) {
            throw "Wrong PNG dimensions for $Path`: $($image.Width)x$($image.Height), expected ${Width}x${Height}"
        }
    }
    finally {
        $image.Dispose()
    }
}

$slotRows = Import-Csv -LiteralPath $SlotCsv
if ($slotRows.Count -ne 72) {
    throw "Expected 72 actual-scale slot rows, found $($slotRows.Count)"
}

$recommendedRows64 = @(Import-Csv -LiteralPath $Batch80RecommendedManifest | Where-Object { $_.size -eq "64x64" })
if ($recommendedRows64.Count -ne 18) {
    throw "Expected 18 Batch 80 recommended 64px icons, found $($recommendedRows64.Count)"
}
$expectedIcons = @{}
foreach ($row in $recommendedRows64) {
    $key = "$($row.cat)/$($row.skill)"
    if ($expectedIcons.ContainsKey($key)) {
        throw "Duplicate Batch 80 recommended 64px icon identity: $key"
    }
    $expectedIcons[$key] = $true
}

$expectedStates = @("ready", "selected", "cooldown_99", "disabled")
$slotFiles = Get-ChildItem -LiteralPath $SlotsDir -Filter "*.png"
if ($slotFiles.Count -ne 72) {
    throw "Expected 72 actual-scale slot PNGs, found $($slotFiles.Count)"
}

$slotIds = @{}
foreach ($row in $slotRows) {
    if ($expectedStates -notcontains $row.state) {
        throw "Unexpected slot state in actual-scale mockup CSV: $($row.state)"
    }
    if ($row.slot_size -ne "64x64") {
        throw "Unexpected actual-scale mockup slot size: $($row.slot_size)"
    }
    if ($row.status -ne "local_actual_scale_mockup_not_unity_screenshot") {
        throw "Unexpected slot status: $($row.status)"
    }
    $iconKey = "$($row.cat)/$($row.skill)"
    if (-not $expectedIcons.ContainsKey($iconKey)) {
        throw "Slot mockup row does not match Batch 80 recommended 64px icon identity: $iconKey"
    }
    $absolute = Test-CandidatePath -Path $row.path
    Test-PngDimensions -Path $absolute -Width 64 -Height 64
    $id = "$($row.cat)/$($row.skill)/$($row.state)"
    if ($slotIds.ContainsKey($id)) {
        throw "Duplicate slot mockup row: $id"
    }
    $slotIds[$id] = $true
}

foreach ($iconKey in $expectedIcons.Keys) {
    foreach ($state in $expectedStates) {
        $id = "$iconKey/$state"
        if (-not $slotIds.ContainsKey($id)) {
            throw "Missing expected actual-scale slot mockup row: $id"
        }
    }
}

$expectedBoards = @{
    "thecat_batch81_v002_light_hud_1280x720_64px_focus_matrix_v001.png" = "1280x720"
    "thecat_batch81_v002_light_hud_1280x720_64px_fullbar_v001.png" = "1280x720"
    "thecat_batch81_v002_light_hud_1920x1080_128px_focus_matrix_v001.png" = "1920x1080"
    "thecat_batch81_v002_light_hud_1920x1080_64px_focus_matrix_v001.png" = "1920x1080"
    "thecat_batch81_v002_light_hud_1920x1080_64px_fullbar_v001.png" = "1920x1080"
    "thecat_batch81_v002_light_hud_720x1280_64px_focus_matrix_v001.png" = "720x1280"
}

$boardRows = Import-Csv -LiteralPath $BoardCsv
if ($boardRows.Count -ne $expectedBoards.Count) {
    throw "Expected $($expectedBoards.Count) actual-scale board rows, found $($boardRows.Count)"
}

$boardFiles = Get-ChildItem -LiteralPath $BoardsDir -Filter "*.png"
if ($boardFiles.Count -ne $expectedBoards.Count) {
    throw "Expected $($expectedBoards.Count) actual-scale board PNGs, found $($boardFiles.Count)"
}

foreach ($row in $boardRows) {
    if (-not $expectedBoards.ContainsKey($row.board)) {
        throw "Unexpected actual-scale mockup board: $($row.board)"
    }
    if ($row.size -ne $expectedBoards[$row.board]) {
        throw "Unexpected board CSV size for $($row.board): $($row.size)"
    }
    if ($row.status -ne "local_hud_mockup_not_unity_screenshot") {
        throw "Unexpected board status: $($row.status)"
    }
    $absolute = Test-CandidatePath -Path $row.path
    $parts = $row.size.Split("x")
    Test-PngDimensions -Path $absolute -Width ([int]$parts[0]) -Height ([int]$parts[1])
}

$note = Get-Content -LiteralPath $NotePath -Raw
foreach ($required in @(
    "local_mockup_not_unity_screenshot",
    "do not replace Unity Battle HUD screenshots",
    "No round slots, rejected icons, or starter-cat body art are used"
)) {
    if ($note -notlike "*$required*") {
        throw "Mockup note is missing required boundary text: $required"
    }
}

$metaFiles = Get-ChildItem -LiteralPath $MockupDir -Recurse -Filter "*.meta"
if ($metaFiles.Count -ne 0) {
    throw "Actual-scale mockup evidence must not include Unity .meta files. Found $($metaFiles.Count)"
}

Write-Output "Batch 81 v002_light actual-scale HUD mockup validation passed"
