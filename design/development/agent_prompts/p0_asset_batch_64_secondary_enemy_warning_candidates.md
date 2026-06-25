# Agent Prompt - P0 Asset Batch 64 Secondary Enemy Warning Candidates

## Task Scope

Produce a candidate-only non-cat warning VFX packet for the secondary enemy
set that follows the current P0 core enemy baseline:

- Dream Rail Train straight charge track warning.
- Red Eye Alarm shock-ring warning.
- Unread Red Dot swarm attachment warning.
- Falling Dream Teddy jump-slam landing warning.

This batch is asset-candidate work only. It must not install anything into
Unity and must not change manifest/runtime binding counts.

## Required Reading

- `design/梦境支配者核心玩法/docs/00_overview/p0_minimum_design.md`
- `design/梦境支配者核心玩法/assets/enemies/en02_dream_rail_train`
- `design/梦境支配者核心玩法/assets/enemies/en03_red_eye_alarm`
- `design/梦境支配者核心玩法/assets/enemies/en05_falling_dream_teddy`
- `design/development/prompts/p0_enemy_warning_vfx_assets.md`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetProductionQueueCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0EnemyWarningIndicatorPresenter.cs`

## Expected Output

- Four transparent 256x256 candidate PNGs outside `Assets`.
- One manifest CSV.
- One review sheet PNG.
- One candidate review note.
- One process note.
- One scoped validator.
- Queue coverage updated so Batch 64 is visible as candidate-complete pending
  Unity review.

## Do Not Modify

- Do not write to `Assets/TheCat/Art/Enemies/VFX`.
- Do not create Unity `.meta` files in the candidate folder.
- Do not replace existing Batch 10 enemy warning VFX.
- Do not modify starter-cat source locks, colored turnarounds, candidate body
  images, runtime sprites, prefabs, or scenes.

## Acceptance Criteria

- Candidate PNGs are transparent 256x256 files.
- The manifest records all four subject ids and expected future binding hints.
- The review note states candidate-only status and Unity readability checks.
- The validator proves dimensions, hashes, transparent corners, no `.meta`
  files, packet files, and queue catalog references.
- `P0AssetProductionQueueCoverage`, `P0AssetReviewPacket`, and
  `P0AssetUnityValidationChecklist` tests reflect the new queue baseline.

## Validation

- `powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_secondary_enemy_warning_candidates.ps1`
- `MSBuild TheCat.Runtime.csproj`
- `MSBuild TheCat.EditModeTests.csproj`
- `git diff --check`
- Unity MCP/editor follow-up when available:
  - inspect warning readability at battle scale
  - confirm Console has no import or missing texture errors
  - approve or reject formal install into `Assets/TheCat/Art/Enemies/VFX`
