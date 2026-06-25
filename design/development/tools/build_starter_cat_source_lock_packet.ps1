Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$markdownRelative = "design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.md"
$csvRelative = "design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.csv"
$markdownPath = Join-Path $projectRoot $markdownRelative
$csvPath = Join-Path $projectRoot $csvRelative

$designFolder = -join ([char[]]@(26790, 22659, 25903, 37197, 32773, 26680, 24515, 29609, 27861))
$batchSlug = "batch_05_source_locked_derivatives_2026-06-14"
$candidateRoot = "design/development/asset_candidates/starter_cats"

$cats = @(
    @{
        CatId = "saiban"
        DisplayName = "Saiban / Sword Saint"
        SourceLockId = "saiban_turnaround_colored"
        SourcePath = "design/$designFolder/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png"
        SpritePath = "Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png"
        RuntimeBinding = "cat.combat.saiban"
        ActiveScreenshot = "04-active-cat-saiban.png"
        Traits = "silver-blue armored non-human cat proportions; front-view tabby face markings; oath shield silhouette; sword silhouette; cape and helm from colored turnaround"
        HardBlockers = "Reject colored turnaround drift, human knight proportions, palette drift, missing shield, missing sword, missing tabby face, missing cape or helm."
    },
    @{
        CatId = "nephthys"
        DisplayName = "Nephthys / Moon-Sand Agent"
        SourceLockId = "nephthys_turnaround_colored"
        SourcePath = "design/$designFolder/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png"
        SpritePath = "Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png"
        RuntimeBinding = "cat.combat.nephthys"
        ActiveScreenshot = "05-active-cat-nephthys.png"
        Traits = "hooded non-human cat body; moon-sand Egyptian motif read; floating pyramid / obelisk prop silhouette; gold and blue palette from colored turnaround; dream-script controller identity"
        HardBlockers = "Reject colored turnaround drift, Cleopatra costume cliche, human body language, palette drift, missing hood, missing pyramid or obelisk, missing gold-blue moon-sand motifs."
    },
    @{
        CatId = "suzune"
        DisplayName = "Suzune / Sleep Shrine Maiden"
        SourceLockId = "suzune_turnaround_colored"
        SourcePath = "design/$designFolder/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png"
        SpritePath = "Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png"
        RuntimeBinding = "cat.combat.suzune"
        ActiveScreenshot = "06-active-cat-suzune.png"
        Traits = "calico markings from colored turnaround; shrine outfit on non-human cat body; bell ornaments; wand / branch healer silhouette; vermilion, warm white, and moon-blue healer palette"
        HardBlockers = "Reject colored turnaround drift, human shrine-maiden proportions, palette drift, missing calico markings, missing shrine outfit, missing bell ornaments, missing wand or branch healer silhouette."
    }
)

function Get-FileSha256 {
    param([string]$RelativePath)

    $fullPath = [string](Join-Path -Path $projectRoot -ChildPath ([string]$RelativePath))
    try {
        $stream = [System.IO.File]::OpenRead($fullPath)
    } catch {
        throw "OpenRead failed for [$fullPath] from [$RelativePath]: $($_.Exception.Message)"
    }
    try {
        $algorithm = [System.Security.Cryptography.SHA256]::Create()
        try {
            $hash = $algorithm.ComputeHash($stream)
            return -join ($hash | ForEach-Object { $_.ToString("x2") })
        } finally {
            $algorithm.Dispose()
        }
    } finally {
        $stream.Dispose()
    }
}

function Get-ImageSize {
    param([string]$RelativePath)

    $fullPath = [string](Join-Path -Path $projectRoot -ChildPath ([string]$RelativePath))
    $image = [System.Drawing.Image]::FromFile($fullPath)
    try {
        return "$($image.Width)x$($image.Height)"
    } finally {
        $image.Dispose()
    }
}

function ConvertTo-CsvCell {
    param([string]$Value)

    $text = $Value -replace '"', '""'
    return '"' + $text + '"'
}

$records = @()
foreach ($cat in $cats) {
    $catId = $cat.CatId
    $candidateDir = "$candidateRoot/$catId/$batchSlug"
    $reviewNote = "$candidateDir/$($catId)_batch05_source_locked_candidate_review.md"
    $reviewSheet = "$candidateDir/thecat_cat_$($catId)_batch05_source_locked_review_sheet.png"
    $records += [pscustomobject]@{
        CatId = $catId
        DisplayName = $cat.DisplayName
        SourceLockId = $cat.SourceLockId
        SourceTurnaroundPath = $cat.SourcePath
        SourceTurnaroundSha256 = Get-FileSha256 $cat.SourcePath
        SourceTurnaroundSize = Get-ImageSize $cat.SourcePath
        CurrentSpritePath = $cat.SpritePath
        CurrentSpriteSha256 = Get-FileSha256 $cat.SpritePath
        CurrentSpriteSize = Get-ImageSize $cat.SpritePath
        RuntimeBinding = $cat.RuntimeBinding
        ActiveScreenshot = $cat.ActiveScreenshot
        CandidateDirectory = $candidateDir
        CandidateReviewNote = $reviewNote
        CandidateReviewSheet = $reviewSheet
        FormalImportState = "Blocked"
        ImportAllowed = "no"
        RequiredTraits = $cat.Traits
        HardBlockers = $cat.HardBlockers
    }
}

$csvHeader = "cat_id,display_name,source_lock_id,source_turnaround_path,source_turnaround_sha256,source_turnaround_size,current_sprite_path,current_sprite_sha256,current_sprite_size,runtime_binding,active_screenshot,candidate_directory,candidate_review_note,candidate_review_sheet,formal_import_state,import_allowed,required_traits,hard_blockers"
$csvLines = New-Object System.Collections.Generic.List[string]
$csvLines.Add($csvHeader)
foreach ($record in $records) {
    $csvLines.Add(@(
        ConvertTo-CsvCell $record.CatId
        ConvertTo-CsvCell $record.DisplayName
        ConvertTo-CsvCell $record.SourceLockId
        ConvertTo-CsvCell $record.SourceTurnaroundPath
        ConvertTo-CsvCell $record.SourceTurnaroundSha256
        ConvertTo-CsvCell $record.SourceTurnaroundSize
        ConvertTo-CsvCell $record.CurrentSpritePath
        ConvertTo-CsvCell $record.CurrentSpriteSha256
        ConvertTo-CsvCell $record.CurrentSpriteSize
        ConvertTo-CsvCell $record.RuntimeBinding
        ConvertTo-CsvCell $record.ActiveScreenshot
        ConvertTo-CsvCell $record.CandidateDirectory
        ConvertTo-CsvCell $record.CandidateReviewNote
        ConvertTo-CsvCell $record.CandidateReviewSheet
        ConvertTo-CsvCell $record.FormalImportState
        ConvertTo-CsvCell $record.ImportAllowed
        ConvertTo-CsvCell $record.RequiredTraits
        ConvertTo-CsvCell $record.HardBlockers
    ) -join ",")
}

$markdown = New-Object System.Collections.Generic.List[string]
$markdown.Add("# P0 Starter Cat Source-Lock Packet")
$markdown.Add("")
$markdown.Add("Generated: 2026-06-14")
$markdown.Add("")
$markdown.Add("Purpose: keep all Saiban, Nephthys, and Suzune asset work pinned to the colored turnaround files before any candidate is imported into Unity.")
$markdown.Add("")
$markdown.Add("Formal import status: blocked. Do not import into Unity yet. Active-cat Play Mode screenshots are still required before candidate approval.")
$markdown.Add("")
$markdown.Add("Global blockers:")
$markdown.Add("")
$markdown.Add("- The colored turnaround is the source of truth for body, costume, props, palette, markings, and civilization motifs.")
$markdown.Add("- Reject human-proportion drift, alternate palette drift, generic cute cat prompts, missing required cat-specific traits, and any output sourced primarily from the generated lineup.")
$markdown.Add("- Candidates must stay under `design/development/asset_candidates/starter_cats/<cat_id>/` until explicitly approved.")
$markdown.Add("")
$markdown.Add("## Locked Source Table")
$markdown.Add("")
$markdown.Add("| cat | source lock | source turnaround | source sha256 | source size | current Unity sprite | sprite sha256 | sprite size | runtime binding | active screenshot | candidate review sheet | import |")
$markdown.Add("| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |")
foreach ($record in $records) {
    $markdown.Add("| $($record.CatId) | $($record.SourceLockId) | ``$($record.SourceTurnaroundPath)`` | ``$($record.SourceTurnaroundSha256)`` | $($record.SourceTurnaroundSize) | ``$($record.CurrentSpritePath)`` | ``$($record.CurrentSpriteSha256)`` | $($record.CurrentSpriteSize) | $($record.RuntimeBinding) | $($record.ActiveScreenshot) | ``$($record.CandidateReviewSheet)`` | do not import into Unity yet |")
}

$markdown.Add("")
$markdown.Add("## Required Trait Checks")
$markdown.Add("")
foreach ($record in $records) {
    $markdown.Add("### $($record.DisplayName)")
    $markdown.Add("")
    $markdown.Add("- Source lock: ``$($record.SourceLockId)``")
    $markdown.Add("- Source turnaround: ``$($record.SourceTurnaroundPath)``")
    $markdown.Add("- Current locked sprite: ``$($record.CurrentSpritePath)``")
    $markdown.Add("- Active-cat screenshot target: ``$($record.ActiveScreenshot)``")
    $markdown.Add("- Candidate directory: ``$($record.CandidateDirectory)``")
    $markdown.Add("- Candidate review note: ``$($record.CandidateReviewNote)``")
    $markdown.Add("- Candidate review sheet: ``$($record.CandidateReviewSheet)``")
    $markdown.Add("- Required colored-turnaround traits: $($record.RequiredTraits)")
    $markdown.Add("- Hard blockers: $($record.HardBlockers)")
    $markdown.Add("")
}

$markdown.Add("## Required Agent Preflight")
$markdown.Add("")
$markdown.Add("1. Read this packet, `design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`, the colored turnaround source image, the current locked Unity sprite, and the Batch 05 review sheet for the target cat.")
$markdown.Add("2. State which derivative type is being produced and confirm it is one of: combat_sprite_refinement_512, hud_avatar_256, skill_icon_motif_128, front_animation_keyframe_512.")
$markdown.Add("3. Keep the output outside Assets and write a review note with trait-by-trait comparison.")
$markdown.Add("4. Reject the candidate if any colored turnaround trait, human-proportion blocker, palette blocker, or required prop/costume/marking check fails.")
$markdown.Add("5. Do not edit the locked Unity sprite, source turnaround, source hashes, manifest source_lock_ids, or runtime binding ids without main-session approval.")

Set-Content -LiteralPath $csvPath -Value $csvLines -Encoding UTF8
Set-Content -LiteralPath $markdownPath -Value $markdown -Encoding UTF8

Write-Host "Wrote $markdownRelative"
Write-Host "Wrote $csvRelative"
