# P0 Asset Batch 61 - Starter Skill VFX Install

## Task Scope

Promote the accepted symbolic Batch 55 starter skill VFX candidates into the
Unity runtime art set, then bind them to battle feedback for Saiban, Nephthys,
and Suzune skill casts.

This is a Codex-side asset install batch. Unity MCP validation is a later gate
for AssetDatabase refresh, Sprite import inspection, Console, Play Mode timing,
and screenshot review.

## Required Design/Source Context

- `design/梦境支配者核心玩法/docs`
- `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`
- `design/development/asset_candidates/vfx/starter_skills/batch_55_starter_skill_vfx_candidates_2026-06-15/starter_skill_vfx_batch55_candidate_review.md`
- `design/development/asset_candidates/vfx/starter_skills/batch_55_starter_skill_vfx_candidates_2026-06-15/starter_skill_vfx_batch55_manifest.csv`

## Code Files To Read

- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0BattleFeedbackVisualPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RuntimeVisualBindingCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0HardReferenceSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetProductionQueueCatalog.cs`
- `Assets/TheCat/Tests/EditMode/P0VisualAssetCatalogTests.cs`
- `Assets/TheCat/Tests/EditMode/P0BattleFeedbackVisualCoverageTests.cs`

## Expected Output

- Install three resized 512x512 transparent PNG VFX assets under
  `Assets/TheCat/Art/VFX`.
- Generate `.png.meta` files carrying `TheCatP0ImportSettings:v1`.
- Create a Batch 61 manifest, review sheet, review note, and process note under
  `design/development/asset_candidates/vfx/starter_skills/batch_61_starter_skill_vfx_install_2026-06-15`.
- Add manifest/runtime catalog entries and battle feedback routing:
  - `skill_vfx.saiban_bedline`
  - `skill_vfx.nephthys_moonsand`
  - `skill_vfx.suzune_lullaby`
- Preserve generic feedback VFX fallbacks.
- Update tests, coverage, asset review packet expectations, runtime contact
  sheet, backlog, and development records.

## Forbidden Changes

- Do not import or replace any starter-cat body, portrait, combat sprite, or
  three-view turnaround asset.
- Do not modify `Assets/TheCat/Art/Characters/Sprites` or starter-cat Prefabs.
- Do not create Unity scene or Prefab changes in this batch.
- Do not mark Unity validation complete without editor-side evidence.

## Acceptance Criteria

- Batch 61 validator passes.
- Runtime and EditMode assemblies compile.
- `P0AssetManifestCatalog.P0ManifestAssetCount == 115`.
- `P0VisualAssetCatalog.P0RuntimeVisualBindingCount == 111`.
- `P0HardReferenceSourceLocks.ExpectedManifestLinkedAssetCount == 22`.
- Starter skill feedback routes to the new VFX by skill display name while
  generic hit/shield/sleep/mark fallbacks remain available.
- Queue state records Batch 61 as installed but blocked by Unity validation.

## Verification Commands

- `powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_starter_skill_vfx_install.ps1`
- Runtime assembly MSBuild.
- EditMode assembly MSBuild.
- `git diff --check`

## Unity MCP Validation Later

- Refresh AssetDatabase.
- Inspect Sprite import settings for all three Batch 61 VFX assets.
- Cast one Saiban, Nephthys, and Suzune skill in Play Mode.
- Capture battle-feedback screenshots and verify scale, alpha edges, timing,
  and Console cleanliness.
