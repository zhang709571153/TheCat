param(
    [string]$OutCsv = "design/development/asset_review/P0_NONCAT_CANDIDATE_VALIDATION_MATRIX_2026-06-25.csv",
    [string]$OutMarkdown = "design/development/asset_review/P0_NONCAT_CANDIDATE_VALIDATION_MATRIX_2026-06-25.md"
)

$ErrorActionPreference = "Stop"
$packetDate = "2026-06-25"
$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$toolDir = Join-Path $projectRoot "design/development/tools"

function Resolve-OutputPath {
    param([string]$Path)

    if ([System.IO.Path]::IsPathRooted($Path)) {
        return $Path
    }

    return Join-Path $projectRoot ($Path -replace "/", [System.IO.Path]::DirectorySeparatorChar)
}

$validators = @(
    @{ Lane = "bedroom_interactables"; Script = "validate_bedroom_interactable_candidates.ps1"; RuntimeGate = "scene scale, Sprite import, binding" },
    @{ Lane = "bedroom_interaction_affordances"; Script = "validate_bedroom_interaction_affordance_candidates.ps1"; RuntimeGate = "interaction timing screenshots, Console" },
    @{ Lane = "owner_sleep_state_animation"; Script = "validate_owner_sleep_state_framesheet_candidate.ps1"; RuntimeGate = "Unity slicing, pivot, scale, overlay decision" },
    @{ Lane = "owner_sleep_status_icons"; Script = "validate_owner_sleep_status_icon_candidates.ps1"; RuntimeGate = "HUD/settlement screenshots, 64/32 readability" },
    @{ Lane = "settings_controls"; Script = "validate_settings_control_candidates.ps1"; RuntimeGate = "settings screen screenshots, drag/click behavior" },
    @{ Lane = "system_icons"; Script = "validate_system_icon_candidates.ps1"; RuntimeGate = "UI screenshots, 64/32 semantics" },
    @{ Lane = "runtime_control_icons"; Script = "validate_runtime_control_icon_candidates.ps1"; RuntimeGate = "HUD scale screenshots, shortcut readability" },
    @{ Lane = "runtime_control_panels"; Script = "validate_runtime_control_panel_candidates.ps1"; RuntimeGate = "HUD panel screenshots, click target checks" },
    @{ Lane = "ui_common_component_inventory"; Script = "validate_ui_common_component_inventory.ps1"; RuntimeGate = "Screen-priority review, then Unity screenshots and click-target checks" },
    @{ Lane = "ui_common_state_derivative_candidates"; Script = "validate_ui_common_state_derivative_candidates.ps1"; RuntimeGate = "Screen priority, Unity import settings, click-target/readability screenshots, binding proof, Console" },
    @{ Lane = "ui_loading_start_preflight"; Script = "validate_ui_loading_start_preflight_candidates.ps1"; RuntimeGate = "Unity-rendered loading/start screenshots, import settings, click-target/readability proof, binding proof, Console" },
    @{ Lane = "ui_result_settlement_preflight"; Script = "validate_ui_result_settlement_preflight_candidates.ps1"; RuntimeGate = "Unity-rendered victory/defeat/settlement screenshots, text replacement, import settings, binding proof, Console" },
    @{ Lane = "ui_settings_pause_preflight"; Script = "validate_ui_settings_pause_preflight_candidates.ps1"; RuntimeGate = "Unity-rendered settings/pause screenshots, text replacement, 1024x768 crowding, import settings, click-target proof, binding proof, Console" },
    @{ Lane = "ui_dream_route_preflight"; Script = "validate_ui_dream_route_preflight_candidates.ps1"; RuntimeGate = "Unity-rendered dream-entry/route-map screenshots, node/path semantics, text replacement, click-target proof, binding proof, Console" },
    @{ Lane = "ui_character_select_preflight"; Script = "validate_ui_character_select_preflight_candidates.ps1"; RuntimeGate = "Unity-rendered character-select screenshots, source-lock avatar consistency, text replacement, selected/idle state readability, click-target proof, binding proof, Console" },
    @{ Lane = "ui_battle_hud_preflight"; Script = "validate_ui_battle_hud_preflight_candidates.ps1"; RuntimeGate = "Unity-rendered battle HUD screenshots, gauge/text replacement, skill state readability, click-target proof, binding proof, Console" },
    @{ Lane = "ui_skill_selection_preflight"; Script = "validate_ui_skill_selection_preflight_candidates.ps1"; RuntimeGate = "Unity-rendered skill-selection screenshots, selected/disabled/locked states, cooldown/low-resource/no-target semantics, click-target proof, binding proof, Console" },
    @{ Lane = "ui_cat_room_preflight"; Script = "validate_ui_cat_room_preflight_candidates.ps1"; RuntimeGate = "Unity-rendered cat-room screenshots, bed/feeder/litter/dream entrance interactions, hover/disabled/range states, click-target proof, binding proof, Console" },
    @{ Lane = "route_map_readability"; Script = "validate_route_map_readability_candidates.ps1"; RuntimeGate = "route flow screenshots if P0 route flow is active" },
    @{ Lane = "skill_hud_feedback_candidates"; Script = "validate_skill_hud_feedback_candidates.ps1"; RuntimeGate = "battle HUD timing/readability screenshots" },
    @{ Lane = "skill_hud_feedback_install"; Script = "validate_skill_hud_feedback_install.ps1"; RuntimeGate = "prefab/catalog binding and Console proof" },
    @{ Lane = "starter_skill_vfx_candidates"; Script = "validate_starter_skill_vfx_candidates.ps1"; RuntimeGate = "skill-cast timing screenshots" },
    @{ Lane = "starter_skill_vfx_install"; Script = "validate_starter_skill_vfx_install.ps1"; RuntimeGate = "prefab/catalog binding and Console proof" },
    @{ Lane = "secondary_enemy_warning_vfx"; Script = "validate_secondary_enemy_warning_candidates.ps1"; RuntimeGate = "enemy warning screenshots and prefab binding" },
    @{ Lane = "chinese_ui_scale_evidence"; Script = "validate_chinese_ui_scale_evidence_templates.ps1"; RuntimeGate = "5 surfaces x 4 resolutions screenshot matrix" },
    @{ Lane = "chinese_ui_scale_local_mockups"; Script = "validate_batch75_local_chinese_ui_scale_mockups.ps1"; RuntimeGate = "Unity-rendered screenshots and Console notes for 5 surfaces x 4 resolutions" },
    @{ Lane = "p0_asset_dashboard"; Script = "validate_p0_asset_production_dashboard.ps1"; RuntimeGate = "dashboard review and Unity evidence links" },
    @{ Lane = "p0_enemy_source_reference"; Script = "validate_p0_enemy_source_reference_pack.ps1"; RuntimeGate = "active enemy screenshots and source consistency" },
    @{ Lane = "enemy_framesheet_import_policy"; Script = "validate_enemy_framesheet_import_policy.ps1"; RuntimeGate = "Unity import refresh, active enemy animation screenshots, prefab/catalog binding, Console proof" },
    @{ Lane = "black_mud_ai_refinement"; Script = "validate_black_mud_ai_refinement_candidate.ps1"; RuntimeGate = "enemy prefab binding and Console proof" },
    @{ Lane = "black_mud_cutout"; Script = "validate_black_mud_cutout_candidate.ps1"; RuntimeGate = "active enemy screenshot and import settings" },
    @{ Lane = "cold_light_ai_refinement"; Script = "validate_cold_light_ai_refinement_candidate.ps1"; RuntimeGate = "enemy prefab binding and Console proof" },
    @{ Lane = "cold_light_cutout"; Script = "validate_cold_light_cutout_candidate.ps1"; RuntimeGate = "active enemy screenshot and import settings" },
    @{ Lane = "call_tyrant_ai_refinement"; Script = "validate_call_tyrant_ai_refinement_candidate.ps1"; RuntimeGate = "boss prefab binding and Console proof" },
    @{ Lane = "call_tyrant_cutout"; Script = "validate_call_tyrant_cutout_candidate.ps1"; RuntimeGate = "boss screenshot and import settings" },
    @{ Lane = "starter_skill_icon_motifs"; Script = "validate_starter_skill_icon_motif_candidates.ps1"; RuntimeGate = "Battle HUD import-test screenshots" },
    @{ Lane = "skill_slot_frames"; Script = "validate_skill_slot_frame_candidates.ps1"; RuntimeGate = "selected-vs-ready HUD proof and cooldown 99 actual-scale proof" },
    @{ Lane = "skill_slot_frames_actual_scale_hud_mockup"; Script = "validate_batch81_v002_actual_scale_hud_mockups.ps1"; RuntimeGate = "Unity-rendered Battle HUD screenshot with selected-vs-ready and cooldown 99 proof" }
)

$rows = foreach ($validator in $validators) {
    $scriptPath = Join-Path $toolDir $validator.Script
    if (-not (Test-Path -LiteralPath $scriptPath)) {
        [pscustomobject]@{
            Lane = $validator.Lane
            Validator = $validator.Script
            Status = "missing_validator"
            ExitCode = -1
            OutputSummary = "Validator file not found"
            RuntimeGate = $validator.RuntimeGate
        }
        continue
    }

    $process = New-Object System.Diagnostics.Process
    $process.StartInfo = New-Object System.Diagnostics.ProcessStartInfo
    $process.StartInfo.FileName = "powershell"
    $escapedScriptPath = $scriptPath.Replace('"', '\"')
    $process.StartInfo.Arguments = "-NoProfile -ExecutionPolicy Bypass -File `"$escapedScriptPath`""
    $process.StartInfo.WorkingDirectory = $projectRoot
    $process.StartInfo.UseShellExecute = $false
    $process.StartInfo.RedirectStandardOutput = $true
    $process.StartInfo.RedirectStandardError = $true
    [void]$process.Start()
    $stdout = $process.StandardOutput.ReadToEnd()
    $stderr = $process.StandardError.ReadToEnd()
    $process.WaitForExit()
    $exitCode = $process.ExitCode

    $summary = (($stdout, $stderr | Where-Object { -not [string]::IsNullOrWhiteSpace($_) }) -join " ").Trim()
    if ($summary.Length -gt 360) {
        $summary = $summary.Substring(0, 357) + "..."
    }

    [pscustomobject]@{
        Lane = $validator.Lane
        Validator = $validator.Script
        Status = if ($exitCode -eq 0) { "pass_local_validator" } else { "fail_local_validator" }
        ExitCode = $exitCode
        OutputSummary = $summary
        RuntimeGate = $validator.RuntimeGate
    }
}

$outCsvPath = Resolve-OutputPath $OutCsv
$outMarkdownPath = Resolve-OutputPath $OutMarkdown
New-Item -ItemType Directory -Force -Path (Split-Path -Parent $outCsvPath) | Out-Null
$rows | Export-Csv -LiteralPath $outCsvPath -NoTypeInformation -Encoding UTF8

$passed = @($rows | Where-Object { $_.Status -eq "pass_local_validator" }).Count
$failed = @($rows | Where-Object { $_.Status -ne "pass_local_validator" }).Count

$markdown = New-Object System.Collections.Generic.List[string]
$markdown.Add("# P0 Non-Cat Candidate Validation Matrix")
$markdown.Add("")
$markdown.Add("Packet date: $packetDate")
$markdown.Add("")
$markdown.Add("This report validates existing non-cat or symbolic candidate packs before opening broad new image-generation batches. Passing here proves only local package integrity, not Unity runtime acceptance.")
$markdown.Add("")
$markdown.Add("Scope note: this is a curated non-cat/symbolic candidate matrix, not an exhaustive whole-project validator. Source-lock, formal install decision, systematic master-plan, and starter-cat body/framesheet conformance validators remain separate gates.")
$markdown.Add("")
$markdown.Add("- Validators run: $($rows.Count)")
$markdown.Add("- Local passes: $passed")
$markdown.Add("- Local failures: $failed")
$markdown.Add("")
$markdown.Add("| Lane | Validator | Local status | Runtime gate still required | Output summary |")
$markdown.Add("| --- | --- | --- | --- | --- |")
foreach ($row in $rows) {
    $safeSummary = $row.OutputSummary.Replace("`r", " ").Replace("`n", " ").Replace("|", "\|")
    $markdown.Add("| $($row.Lane) | $($row.Validator) | $($row.Status) | $($row.RuntimeGate) | $safeSummary |")
}
$markdown.Add("")
$markdown.Add("## Control Rule")
$markdown.Add("")
$markdown.Add("Do not treat this matrix as import approval. Candidate packs remain outside formal Unity acceptance until screenshot evidence, import settings, Console checks, and prefab/catalog binding proof pass.")

$markdown | Set-Content -LiteralPath $outMarkdownPath -Encoding UTF8

Write-Output "P0 non-cat candidate validation matrix complete: $passed passed, $failed failed"
Write-Output $OutMarkdown
