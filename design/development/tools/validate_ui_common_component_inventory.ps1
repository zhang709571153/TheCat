Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$csvPath = Join-Path $projectRoot "design/development/asset_review/P0_UI_COMMON_COMPONENT_INVENTORY_2026-06-25.csv"
$markdownPath = Join-Path $projectRoot "design/development/asset_review/P0_UI_COMMON_COMPONENT_INVENTORY_2026-06-25.md"

$failures = New-Object System.Collections.Generic.List[string]
$rows = @()

function Add-Failure {
    param([string]$Message)
    $failures.Add($Message)
}

function Resolve-ProjectPath {
    param([string]$RelativePath)

    if ([string]::IsNullOrWhiteSpace($RelativePath) -or $RelativePath -eq "none") {
        return $null
    }

    $trimmed = $RelativePath.Trim()
    if ([System.IO.Path]::IsPathRooted($trimmed)) {
        throw "Evidence paths must be project-relative: $trimmed"
    }

    $normalized = $trimmed -replace "/", [System.IO.Path]::DirectorySeparatorChar
    $fullPath = [System.IO.Path]::GetFullPath((Join-Path $projectRoot $normalized))
    $rootFull = [System.IO.Path]::GetFullPath($projectRoot).TrimEnd(
        [System.IO.Path]::DirectorySeparatorChar,
        [System.IO.Path]::AltDirectorySeparatorChar
    )
    $rootPrefix = $rootFull + [System.IO.Path]::DirectorySeparatorChar

    if (-not ($fullPath.StartsWith($rootPrefix, [System.StringComparison]::OrdinalIgnoreCase) -or $fullPath -eq $rootFull)) {
        throw "Evidence path escapes project root: $trimmed"
    }

    return $fullPath
}

function Test-AllPaths {
    param([string]$SemicolonPaths)

    if ([string]::IsNullOrWhiteSpace($SemicolonPaths) -or $SemicolonPaths -eq "none") {
        return $false
    }

    $paths = @($SemicolonPaths -split ";" | Where-Object { -not [string]::IsNullOrWhiteSpace($_) })
    if ($paths.Count -eq 0) {
        return $false
    }

    foreach ($relativePath in ($SemicolonPaths -split ";" | Where-Object { -not [string]::IsNullOrWhiteSpace($_) })) {
        $path = Resolve-ProjectPath $relativePath
        if ($null -eq $path -or -not (Test-Path -LiteralPath $path)) {
            return $false
        }
    }

    return $true
}

if (-not (Test-Path -LiteralPath $csvPath)) {
    Add-Failure "Missing CSV inventory: $csvPath"
} else {
    $rows = @(Import-Csv -LiteralPath $csvPath)
    if ($rows.Count -ne 17) {
        Add-Failure "Expected 17 inventory rows but found $($rows.Count)"
    }

    $requiredIds = @(
        "primary_button_default",
        "button_state_atlas",
        "dreamglass_panel",
        "modal_dialog_frame",
        "tabs_segmented_controls",
        "route_reward_card_frames",
        "choice_content_cards",
        "list_row_frame",
        "system_icon_set",
        "settings_control_set",
        "runtime_control_panels",
        "lock_warning_controls",
        "skill_slot_state_frames",
        "skill_hud_feedback_overlays",
        "core_gauge_bars",
        "result_settlement_banners",
        "reward_detail_badges"
    )

    foreach ($id in $requiredIds) {
        if (-not ($rows | Where-Object { $_.component_id -eq $id })) {
            Add-Failure "Missing component row: $id"
        }
    }

    foreach ($row in $rows) {
        $allowedStates = @(
            "installed_pending_unity",
            "installed_pending_unity_validation",
            "candidate_complete_pending_unity_review",
            "missing_design_needed"
        )
        if ($allowedStates -notcontains $row.current_state) {
            Add-Failure "$($row.component_id) has unsupported current_state=$($row.current_state)"
        }

        if ($row.current_state -like "installed*" -and $row.installed_present -ne "yes") {
            Add-Failure "$($row.component_id) is marked installed but installed_present=$($row.installed_present)"
        }

        if ($row.current_state -like "*candidate*" -and $row.candidate_present -ne "yes") {
            Add-Failure "$($row.component_id) is marked candidate/import-test but candidate_present=$($row.candidate_present)"
        }

        if ($row.current_state -eq "missing_design_needed" -and ($row.installed_present -ne "no" -or $row.candidate_present -ne "no")) {
            Add-Failure "$($row.component_id) is marked missing but has installed/candidate evidence."
        }

        if ($row.candidate_evidence -like "*Assets/TheCat*") {
            Add-Failure "$($row.component_id) candidate_evidence must not point into Assets."
        }

        if ($row.installed_paths -like "*starter_cat*" -or $row.installed_paths -like "*Art/Characters*") {
            Add-Failure "$($row.component_id) installed path leaks into starter/character body lane."
        }

        if ($row.candidate_evidence -ne "none" -and -not (Test-AllPaths $row.candidate_evidence)) {
            Add-Failure "$($row.component_id) candidate evidence paths do not exist."
        }

        if ($row.installed_paths -ne "none" -and -not (Test-AllPaths $row.installed_paths)) {
            Add-Failure "$($row.component_id) installed paths do not exist."
        }
    }

    $missingRows = @($rows | Where-Object { $_.current_state -eq "missing_design_needed" })
    if ($missingRows.Count -ne 0) {
        Add-Failure "Expected 0 missing design-needed rows after Batch 82, but found $($missingRows.Count)"
    }

    foreach ($batch82Component in @("button_state_atlas", "modal_dialog_frame", "tabs_segmented_controls", "list_row_frame")) {
        $row = $rows | Where-Object { $_.component_id -eq $batch82Component } | Select-Object -First 1
        if ($null -eq $row) {
            Add-Failure "Missing Batch 82-covered component row: $batch82Component"
        } elseif ($row.current_state -ne "candidate_complete_pending_unity_review" -or $row.candidate_present -ne "yes") {
            Add-Failure "$batch82Component should be Batch 82 candidate-covered, but state=$($row.current_state), candidate_present=$($row.candidate_present)"
        }
    }
}

if (-not (Test-Path -LiteralPath $markdownPath)) {
    Add-Failure "Missing Markdown inventory: $markdownPath"
} else {
    $markdown = Get-Content -LiteralPath $markdownPath -Raw
    if ($rows.Count -gt 0) {
        $installedCount = @($rows | Where-Object { $_.current_state -like "installed*" }).Count
        $candidateCount = @($rows | Where-Object { $_.current_state -like "*candidate*" }).Count
        $missingCount = @($rows | Where-Object { $_.current_state -eq "missing_design_needed" }).Count
        $summaryTokens = @(
            "Rows: $($rows.Count)",
            "Installed pending Unity evidence: $installedCount",
            "Candidate-only or import-test rows: $candidateCount",
            "Missing design-needed rows: $missingCount"
        )

        foreach ($token in $summaryTokens) {
            if ($markdown -notmatch [regex]::Escape($token)) {
                Add-Failure "Markdown inventory summary count mismatch: $token"
            }
        }
    }

    $requiredTokens = @(
        "Qr1XdXd6KosnjMxjgW7cS89kn9c",
        "section 9",
        "Batch 82 derivative candidates now cover the prior missing button state, modal/dialog, tab/segmented-control, and list-row rows",
        "No missing design-needed rows remain in the common-component inventory",
        'Candidate-only system/settings/runtime/skill-slot rows must stay outside `Assets/`',
        "Avoid baked Chinese text",
        "No starter-cat body",
        "validate_ui_common_component_inventory.ps1"
    )

    foreach ($token in $requiredTokens) {
        if ($markdown -notmatch [regex]::Escape($token)) {
            Add-Failure "Markdown inventory missing token: $token"
        }
    }
}

if ($failures.Count -gt 0) {
    Write-Error ("UI common component inventory validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "UI common component inventory validation passed. Rows: 17 Missing design-needed rows: 0"
Write-Output "design/development/asset_review/P0_UI_COMMON_COMPONENT_INVENTORY_2026-06-25.csv"
Write-Output "design/development/asset_review/P0_UI_COMMON_COMPONENT_INVENTORY_2026-06-25.md"
