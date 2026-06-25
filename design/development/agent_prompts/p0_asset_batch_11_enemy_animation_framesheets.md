# P0 Asset Batch 11 - Enemy Animation Framesheets

## Task Scope

Create and validate source-locked P0 enemy/Boss animation framesheets for:

- Black Mud Nightmare move/crawl loop.
- Cold Light Shadow cast/pressure loop.
- Call Tyrant Boss pattern loop covering summon/APP throw language.

## Required Reading

- `design/梦境支配者核心玩法/docs/00_overview/p0_minimum_design.md`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/prompts/p0_enemy_animation_framesheets.md`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetGenerationBatchCatalog.cs`

## Source Files

- `design/梦境支配者核心玩法/assets/enemies/en01_black_mud_nightmare/animation/black_mud_nightmare_animation.png`
- `design/梦境支配者核心玩法/assets/enemies/en04_cold_light_shadow/animation/cold_light_shadow_animation.png`
- `design/梦境支配者核心玩法/assets/enemies/en06_call_tyrant/animation/call_tyrant_animation.png`

## Expected Outputs

- `Assets/TheCat/Art/Enemies/Frames/thecat_enemy_blackmud_move_framesheet_4x256_v001.png`
- `Assets/TheCat/Art/Enemies/Frames/thecat_enemy_coldlight_cast_framesheet_4x256_v001.png`
- `Assets/TheCat/Art/Enemies/Frames/thecat_enemy_calltyrant_bosspattern_framesheet_4x256_v001.png`
- Matching `.png.meta` files with `TheCatP0ImportSettings:v1`.
- Manifest rows, runtime visual bindings, source-lock checks, tests, contact
  sheet, review packet, and development log updates.

## Do Not Modify

- Starter cat sprites under `Assets/TheCat/Art/Characters`.
- Starter cat candidate derivatives under `design/development/asset_candidates/starter_cats`.
- Colored turnaround source images.
- Starter cat formal import gate files.

## Acceptance Criteria

- Each framesheet is `1024x256`, transparent, and visually source-derived.
- Black Mud frames preserve crawling black mud body, red eyes, and sludge tail.
- Cold Light frames preserve lamp silhouette, mechanical arm, black mud base,
  red eye, and cast pressure cue.
- Call Tyrant frames preserve phone Boss body, red call eyes, black mud base,
  purple tie, and APP throw language.
- Source locks are exactly `black_mud_animation`, `cold_light_animation`, and
  `call_tyrant_animation`.
- Runtime binding count, manifest count, source-locked count, and review packet
  count all update through shared constants.

## Validation

- Run `design/development/tools/build_enemy_animation_framesheets.ps1`.
- Verify dimensions and `.png.meta` markers.
- Run `git diff --check`.
- Compile `TheCat.Runtime.csproj`, `TheCat.EditModeTests.csproj`, and
  `TheCat.Editor.csproj` with Visual Studio MSBuild.
- When Unity MCP/editor-side execution is available, refresh AssetDatabase and
  run Console plus screenshot validation for enemy animation previews.
