# P0 Asset Batch 52 - Starter Cat Active Validation

## Task Scope

Validate the current starter-cat strict candidates in Unity context without
installing them. Compare Saiban Batch 49, Nephthys Batch 50, and Suzune Batch
51 against the locked colored three-view turnarounds.

## Required Design Inputs

- `design/梦境支配者核心玩法/docs`
- `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`
- `design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`

## Code And Records To Read

- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatStrictCandidateEvidence.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatFormalImportReadiness.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `design/development/asset_candidates/starter_cats`

## Expected Output

- Active-cat screenshots:
  - `05-active-cat-saiban.png`
  - `06-active-cat-nephthys.png`
  - `07-active-cat-suzune.png`
- A review note that keeps each cat blocked or explicitly recommends formal
  install.
- A short process note with Console, screenshot, scale, HUD readability, and
  source-lock comparison results.

## Forbidden Changes

- Do not copy candidate PNGs into `Assets`.
- Do not modify prefabs, scenes, runtime visual bindings, source locks, or
  formal import state.
- Do not approve any cat whose active screenshot drifts from its colored
  three-view turnaround.

## Acceptance

- All three active-cat screenshots exist.
- Unity Console has no new errors.
- Review notes mention the relevant candidate batch, baseline batch, source
  turnaround, active screenshot, and final blocked-or-approved decision.

## Validation

- Unity MCP: capture screenshots, inspect Console, and confirm scene/prefab
  bindings.
- Offline fallback: run MSBuild and the strict candidate evidence tests, then
  leave Unity validation pending.
