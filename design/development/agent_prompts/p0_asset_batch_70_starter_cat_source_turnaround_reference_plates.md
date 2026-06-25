# Agent Prompt - P0 Asset Batch 70 Starter Cat Source Turnaround Reference Plates

## Task Scope

Create deterministic front, side, and back reference plates for Saiban,
Nephthys, and Suzune by cropping the authoritative colored three-view
turnarounds. These plates are hard visual inputs for future Codex image
generation, local cutout work, and active-cat Play Mode screenshot review.

This is a Codex-side source-derived reference batch. Do not generate new cat
art and do not install anything into Unity.

## Required Reading

Design docs:

- `D:\Unity Workspace\TheCat\design\*\docs\00_overview\p0_minimum_design.md`
- `D:\Unity Workspace\TheCat\design\*\docs\03_characters\character_design.md`
- `D:\Unity Workspace\TheCat\design\*\docs\04_art_production\p0_digital_asset_inventory.md`

Locate the real design root by finding:

- `design/*/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- `design/*/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- `design/*/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`

Local production docs:

- `D:\Unity Workspace\TheCat\design\development\P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `D:\Unity Workspace\TheCat\design\development\P0_ARCHITECTURE_ASSET_PRODUCTION_READINESS_AUDIT.md`
- `D:\Unity Workspace\TheCat\design\development\asset_candidates\starter_cats\batch_69_turnaround_runtime_comparison_audit_2026-06-15\starter_cat_turnaround_runtime_comparison_batch69_review.md`
- `D:\Unity Workspace\TheCat\design\development\asset_review\P0_ASSET_UNITY_VALIDATION_CHECKLIST.md`

Relevant code:

- `D:\Unity Workspace\TheCat\Assets\TheCat\Scripts\Runtime\Tools\P0StarterCatSourceLockPacketEvidence.cs`
- `D:\Unity Workspace\TheCat\Assets\TheCat\Scripts\Runtime\Tools\P0AssetReviewPacket.cs`
- `D:\Unity Workspace\TheCat\Assets\TheCat\Tests\EditMode\P0StarterCatSourceLockPacketEvidenceTests.cs`
- `D:\Unity Workspace\TheCat\Assets\TheCat\Tests\EditMode\P0AssetReviewPacketTests.cs`

## Expected Outputs

Create or refresh:

`design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15`

The batch must contain:

- nine `768x768` reference plate PNGs:
  - Saiban front / side / back
  - Nephthys front / side / back
  - Suzune front / side / back
- `starter_cat_turnaround_reference_plates_batch70_manifest.csv`
- `thecat_cat_starter_turnaround_reference_plates_batch70_review_sheet.png`
- `starter_cat_turnaround_reference_plates_batch70_review.md`
- `starter_cat_turnaround_reference_plates_batch70_process_note.md`

Also create or update:

- `design/development/tools/build_starter_cat_turnaround_reference_plates.ps1`
- `design/development/tools/validate_starter_cat_turnaround_reference_plates.ps1`
- Runtime/EditMode evidence that makes this reference pack part of the P0
  asset review packet gate.

## Visual Requirements

- Source-derived only.
- No AI image generation.
- No repainting, retouching, inpainting, recoloring, cleanup, pose changes, or
  costume simplification.
- Each plate must preserve the relevant front, side, or back view from the
  colored three-view turnaround.
- Each plate must be clearly marked as a reference input, not a final
  transparent runtime sprite.

## Forbidden Modification Scope

Do not modify:

- starter-cat source turnaround images
- current Unity combat sprite PNGs
- Unity `.meta` files
- sprite import settings
- runtime visual bindings
- source-lock hashes
- formal import decision notes
- Batch 49/50/51 AI starter-cat candidates

Do not crop from Batch 01 style anchors, generated lineup sheets, or existing
runtime sprites. The colored three-view turnaround is the only visual source.

## Acceptance Criteria

- Batch directory exists outside `Assets`.
- Nine `768x768` reference plate PNGs exist.
- Manifest has exactly nine rows, one per starter-cat view.
- Each row records cat id, view, source lock id, exact colored-turnaround
  source path, source hash, crop rect, plate path, plate hash, and
  `reference_only_do_not_import_pending_active_cat_playmode_screenshot`.
- Review sheet is nonblank and shows all nine plates.
- Review note states `reference-only`, `Do not import into Unity yet`,
  `no image generation`, `deterministic crop`, and the strict trait locks.
- Process note states that no image generation was performed.
- Candidate directory has no Unity `.meta` files.
- Runtime evidence reports the reference plate batch as ready.
- `P0AssetReviewPacket` includes the reference plate section and keeps the P0
  packet ready only when the batch passes.

## Test Commands

Run:

```powershell
& .\design\development\tools\build_starter_cat_turnaround_reference_plates.ps1
& .\design\development\tools\validate_starter_cat_turnaround_reference_plates.ps1
git diff --check
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\amd64\MSBuild.exe' TheCat.Runtime.csproj
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\amd64\MSBuild.exe' TheCat.EditModeTests.csproj
```

## Unity MCP / Editor Validation Later

When Unity MCP/editor tools are available, validate:

- capture active-cat Play Mode screenshots for Saiban, Nephthys, and Suzune
- compare active screenshots against Batch 69 and Batch 70
- inspect SpriteRenderer/HUD bindings
- verify Console has no errors
- keep starter-cat sprite replacement blocked unless all visual evidence agrees
  with the colored three-view turnarounds
