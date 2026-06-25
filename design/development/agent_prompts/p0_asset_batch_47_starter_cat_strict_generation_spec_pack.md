# P0 Asset Batch 47 - Starter Cat Strict Generation Spec Pack

## Mission

Build the strict Codex-side generation specification pack for the three starter
cats: Saiban, Nephthys, and Suzune.

This batch is a preparation gate for future image generation. It does not
generate replacement art, does not import into Unity, and does not approve any
sprite replacement. Its job is to make the colored three-view turnaround
authority executable: visual spec cards, machine-readable JSON spec, prompt
files, palette guards, rejection rules, and active-cat screenshot gates.

## Required Reading

- `design/梦境支配者核心玩法/docs`
- `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`
- `design/development/asset_candidates/starter_cats/batch_45_starter_cat_source_lock_audit_pack_2026-06-15/starter_cat_batch45_source_lock_audit_manifest.csv`
- `design/development/asset_candidates/p0_asset_dashboard/batch_46_p0_asset_production_dashboard_2026-06-15/p0_asset_batch46_production_dashboard_manifest.csv`
- `Assets/TheCat/Scripts/Runtime/Tools/P0PlayModeScreenshotSmoke.cs`

## Outputs

- `design/development/tools/build_starter_cat_strict_generation_spec_pack.py`
- `design/development/tools/validate_starter_cat_strict_generation_spec_pack.ps1`
- `design/development/asset_candidates/starter_cats/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/starter_cat_batch47_strict_generation_spec_manifest.csv`
- `design/development/asset_candidates/starter_cats/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/thecat_starter_cat_batch47_strict_generation_spec_review_sheet.png`
- `design/development/asset_candidates/starter_cats/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/starter_cat_batch47_strict_generation_spec_review.md`
- `design/development/asset_candidates/starter_cats/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/starter_cat_batch47_strict_generation_spec_process_note.md`
- Per-cat visual spec card, JSON spec, and generation prompt under:
  - `design/development/asset_candidates/starter_cats/saiban/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15`
  - `design/development/asset_candidates/starter_cats/nephthys/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15`
  - `design/development/asset_candidates/starter_cats/suzune/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15`

## Hard Rules

- Codex-side generation is allowed in later batches, but this batch only writes
  specs and prompts.
- Do not import into Unity.
- Do not copy files into `Assets`.
- Do not create Unity `.meta` files.
- Do not modify `P0AssetManifestCatalog.P0ManifestAssetCount`.
- Do not modify `P0VisualAssetCatalog.P0RuntimeVisualBindingCount`.
- Do not generate art that merely matches the project style while drifting from
  the colored three-view turnaround.
- Do not allow human body proportions, humanoid costume pose, generic mascot
  redesign, missing required props, or palette drift.

## Required Spec Content

Each starter cat must have:

- source lock id
- colored turnaround path and hash
- current Unity sprite path and hash
- latest cutout preview path and hash
- sampled palette guard
- visible source bounding box
- non-human cat body rule
- composition rule
- must-keep identity anchors
- immediate reject rules
- positive generation prompt
- negative generation prompt
- active-cat screenshot gate
- Unity validation requirements
- recommendation `strict_generation_spec_only_do_not_import`

## Acceptance Standards

- Exactly 3 rows: Saiban, Nephthys, Suzune.
- Every prompt says non-human cat body and contains positive and negative
  prompt sections.
- Every JSON spec contains source lock, palette guard, must-keep list, reject
  list, positive prompt, negative prompt, active-cat screenshot, and Unity
  validation requirements.
- The review sheet makes source, current sprite, latest candidate, palette, and
  rejection rules visible without opening Unity.
- All paths use real UTF-8 design paths, not old mojibake paths containing
  `?assets`.
- Formal Unity import remains blocked.

## Validation

Run:

```powershell
C:\Users\PC\.cache\codex-runtimes\codex-primary-runtime\dependencies\python\python.exe design/development/tools/build_starter_cat_strict_generation_spec_pack.py
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_starter_cat_strict_generation_spec_pack.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_starter_cat_source_lock_audit_pack.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_p0_asset_production_dashboard.ps1
```

When Unity MCP/editor access is available, generated candidates that use these
specs must still pass:

- `05-active-cat-saiban.png`
- `06-active-cat-nephthys.png`
- `07-active-cat-suzune.png`
- Console clean
- AssetDatabase refresh
- Sprite import settings
- prefab/scene binding
- HUD readability and runtime scale
