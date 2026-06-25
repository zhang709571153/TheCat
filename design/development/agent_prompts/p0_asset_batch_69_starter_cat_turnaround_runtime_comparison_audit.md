# Agent Prompt - P0 Asset Batch 69 Starter Cat Turnaround Runtime Comparison Audit

## Task Scope

Create an audit-only comparison package for the three P0 starter cats. The
package must compare each authoritative colored three-view turnaround against
the current Unity combat sprite and record whether the sprite can move forward
to active-cat Play Mode screenshot validation.

This is a Codex-side review package. It does not authorize Unity installation,
sprite replacement, source-lock hash updates, or formal import approval.

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
- `D:\Unity Workspace\TheCat\design\development\asset_candidates\starter_cats\p0_starter_cat_turnaround_source_lock_packet_2026-06-14.md`
- `D:\Unity Workspace\TheCat\design\development\asset_review\P0_ASSET_UNITY_VALIDATION_CHECKLIST.md`

Relevant code:

- `D:\Unity Workspace\TheCat\Assets\TheCat\Scripts\Runtime\Tools\P0StarterCatSourceLockPacketEvidence.cs`
- `D:\Unity Workspace\TheCat\Assets\TheCat\Scripts\Runtime\Tools\P0AssetReviewPacket.cs`
- `D:\Unity Workspace\TheCat\Assets\TheCat\Scripts\Runtime\Data\Catalogs\P0VisualAssetCatalog.cs`
- `D:\Unity Workspace\TheCat\Assets\TheCat\Tests\EditMode\P0StarterCatSourceLockPacketEvidenceTests.cs`
- `D:\Unity Workspace\TheCat\Assets\TheCat\Tests\EditMode\P0AssetReviewPacketTests.cs`

## Expected Outputs

Create or refresh:

`design/development/asset_candidates/starter_cats/batch_69_turnaround_runtime_comparison_audit_2026-06-15`

The batch must contain:

- `thecat_cat_starter_turnaround_runtime_comparison_batch69_review_sheet.png`
- `starter_cat_turnaround_runtime_comparison_batch69_manifest.csv`
- `starter_cat_turnaround_runtime_comparison_batch69_review.md`
- `starter_cat_turnaround_runtime_comparison_batch69_process_note.md`

Also create or update:

- `design/development/tools/build_starter_cat_turnaround_runtime_comparison_audit.ps1`
- `design/development/tools/validate_starter_cat_turnaround_runtime_comparison_audit.ps1`
- Runtime/EditMode evidence that makes this audit part of the P0 asset review
  packet gate.

## Visual Review Requirements

The review sheet must show each row as:

- source colored three-view turnaround on the left
- current Unity combat sprite on the right
- exact source lock id
- exact source turnaround path
- current sprite path
- active-cat Play Mode screenshot target
- an audit-only recommendation

The review text must explicitly check:

- body proportion
- face markings
- palette
- costume
- props
- civilization motifs
- active-cat screenshot requirement

## Forbidden Modification Scope

Do not modify:

- starter-cat source turnaround images
- current Unity combat sprite PNGs
- Unity `.meta` files
- sprite import settings
- runtime visual bindings
- source-lock hashes
- formal import decision notes
- generated cat candidate packs from earlier batches

Do not generate new cat art in this batch. This batch is a comparison audit,
not an image-production batch.

## Acceptance Criteria

- Batch directory exists outside `Assets`.
- Four expected audit artifacts exist.
- Review sheet is nonblank and `1800x1460`.
- Manifest has exactly three rows: Saiban, Nephthys, Suzune.
- Each row records source lock id, exact colored-turnaround source path, current
  sprite path, active screenshot target, SHA-256 values, and
  `audit_only_no_import_pending_active_cat_playmode_screenshot`.
- Review note states `audit-only`, `Do not import into Unity yet`, and
  `active-cat Play Mode screenshot`.
- Process note states that no image generation was performed.
- Candidate directory has no Unity `.meta` files.
- Runtime evidence reports the audit as ready.
- `P0AssetReviewPacket` includes the audit section and keeps the P0 packet
  ready only when the audit passes.

## Test Commands

Run:

```powershell
& .\design\development\tools\build_starter_cat_turnaround_runtime_comparison_audit.ps1
& .\design\development\tools\validate_starter_cat_turnaround_runtime_comparison_audit.ps1
git diff --check
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\amd64\MSBuild.exe' TheCat.Runtime.csproj
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\amd64\MSBuild.exe' TheCat.EditModeTests.csproj
```

## Unity MCP / Editor Validation Later

When Unity MCP/editor tools are available, validate:

- open the bedroom battle validation scene
- capture active-cat Play Mode screenshots for Saiban, Nephthys, and Suzune
- compare screenshots against the Batch 69 review sheet and colored
  turnarounds
- check SpriteRenderer/HUD bindings still reference the intended combat sprites
- refresh Console and confirm no errors

Only after that validation may a separate formal import decision consider
installing any revised cat sprite.
