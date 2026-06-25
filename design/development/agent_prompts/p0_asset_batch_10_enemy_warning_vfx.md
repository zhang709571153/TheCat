# Agent Prompt - P0 Asset Batch 10 Enemy Warning VFX

## Task Scope

Produce and integrate the non-cat Batch 10 enemy/Boss warning VFX assets for
the P0 battle warning layer.

Assets:

- `thecat_vfx_blackmud_bed_claw_256_v001`
- `thecat_vfx_coldlight_beam_warning_256_v001`
- `thecat_vfx_calltyrant_app_throw_256_v001`
- `thecat_vfx_calltyrant_summon_portal_256_v001`

## Required Reading

- `design/梦境支配者核心玩法/docs`
- `design/development/prompts/p0_enemy_warning_vfx_assets.md`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0EnemyWarningIndicatorPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetManifestCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RuntimeVisualBindingCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0HardReferenceSourceLocks.cs`

## Expected Output

- Four transparent PNGs and `.png.meta` files under
  `Assets/TheCat/Art/Enemies/VFX`.
- Updated asset manifest CSV and runtime catalog rows.
- Runtime warning presenter routes:
  - Black Mud / near-bed warning to black mud bed claw VFX.
  - Cold Light ranged pressure to cold light beam warning VFX.
  - Call Tyrant boss throw to app throw VFX.
  - Call Tyrant boss summon to summon portal VFX.
- Updated runtime visual contact sheet and review notes.
- Updated tests and local development records.

## Do Not Modify

- Do not replace or regenerate starter cat PNGs.
- Do not alter colored three-view turnaround source images.
- Do not flip the starter-cat formal import gate from blocked to approved.
- Do not create uncontrolled broad content outside this batch.

## Acceptance Criteria

- All four assets are registered in `P0_ASSET_MANIFEST.csv`.
- `P0AssetManifestCatalog.P0ManifestAssetCount` reflects the new manifest total.
- `P0VisualAssetCatalog.P0RuntimeVisualBindingCount` reflects the new runtime
  visual binding total.
- Source locks resolve to `black_mud_animation`, `cold_light_animation`, and
  `call_tyrant_animation`.
- Runtime warning presenter exposes the new VFX in warning summaries.
- Runtime visual contact sheet includes the four new `battle_warning` bindings.
- No starter-cat source-lock or formal-import rows are loosened.

## Validation

- `git diff --check`
- Runtime MSBuild
- EditModeTests MSBuild
- Editor MSBuild
- Unity MCP or editor-side follow-up when available:
  - refresh AssetDatabase
  - check Console
  - capture Play Mode warning screenshots
