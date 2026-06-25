# P0 Asset Batch 54 - Bedroom Interactable Candidates

## Task Scope

Produce Codex-side review candidates for the P0 bed, litter box, and feeder
interactables. This is candidate production only and must not install Unity
assets.

## Required Design Inputs

- `design/梦境支配者核心玩法/docs`
- Bedroom source references under
  `design/梦境支配者核心玩法/assets/levels/lv01_bedroom_dream`
- Current runtime placeholder prop assets under
  `Assets/TheCat/Art/Scenes/BedroomDream`
- P0 bedroom dream art direction in
  `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`

## Code And Records To Read

- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetProductionQueueCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetProductionQueueCoverage.cs`
- `design/development/asset_review/p0_runtime_visual_contact_sheet_2026-06-14.md`

## Expected Output

- Candidate PNGs under
  `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15`
- Transparent alpha candidates, preview sheet, manifest CSV, review note,
  process note, and this prompt path recorded in the manifest.
- Bed, litter box, and feeder must remain readable at P0 game scale.

## Forbidden Changes

- Do not write into `Assets/TheCat/Art/Props`.
- Do not overwrite current runtime placeholders under
  `Assets/TheCat/Art/Scenes/BedroomDream`.
- Do not create Unity `.meta` files.
- Do not modify prefabs, scenes, runtime visual bindings, or manifest catalog
  counts.

## Acceptance

- Candidate files stay outside `Assets`.
- Manifest rows use recommendation `candidate_review_only_do_not_import`.
- Review note records dimensions, alpha status, style anchors, and Unity install
  blockers.

## Validation

- Run the batch-specific validator after creation.
- Run MSBuild and `git diff --check`.
- Unity MCP validation is deferred until a formal install candidate is chosen.
