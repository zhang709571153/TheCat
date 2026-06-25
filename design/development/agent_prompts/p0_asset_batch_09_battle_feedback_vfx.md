# Agent Prompt: P0 Asset Batch 09 Battle Feedback VFX

## Mission

Produce and integrate the P0 battle feedback VFX asset batch so the graybox battle HUD can show distinct skill, status, and interactable feedback sprites without touching starter cat assets.

## Read First

- `design/梦境支配者核心玩法/docs/00_overview/p0_minimum_design.md`
- `design/梦境支配者核心玩法/docs/02_combat_and_systems/status_tag_system.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- `design/development/prompts/p0_battle_feedback_vfx_assets.md`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0BattleFeedbackVisualPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/GrayboxBattleController.cs`

## Output Scope

- Generate these six transparent PNG sprites and `.png.meta` files:
  - `Assets/TheCat/Art/VFX/thecat_vfx_hit_spark_256_v001.png`
  - `Assets/TheCat/Art/VFX/thecat_vfx_bed_shield_pulse_256_v001.png`
  - `Assets/TheCat/Art/VFX/thecat_vfx_sleep_stable_wave_256_v001.png`
  - `Assets/TheCat/Art/VFX/thecat_vfx_litter_cleanse_256_v001.png`
  - `Assets/TheCat/Art/VFX/thecat_vfx_feeder_kibble_256_v001.png`
  - `Assets/TheCat/Art/VFX/thecat_vfx_enemy_mark_ring_256_v001.png`
- Keep `P0_ASSET_MANIFEST.csv`, `P0AssetManifestCatalog`, `P0AssetGenerationBatchCatalog`, and `P0VisualAssetCatalog` in sync.
- Regenerate `design/development/asset_review/p0_runtime_visual_contact_sheet_2026-06-14.png`.
- Update architecture/development notes with the new 44-asset, 40-runtime-binding baseline.

## Forbidden Scope

- Do not modify any starter cat PNG, source turnaround, candidate sheet, cat manifest row, or cat import gate.
- Do not replace the current Saiban, Nephthys, or Suzune runtime sprites.
- Do not weaken colored-turnaround source-lock checks.
- Do not add broad UI redesign or scene rewiring outside the feedback VFX presentation path.

## Validation

- Run `design/development/tools/build_battle_feedback_vfx_assets.ps1`.
- Run `design/development/tools/build_runtime_visual_contact_sheet.ps1`.
- Run `git diff --check`.
- Build `TheCat.Runtime.csproj`, `TheCat.EditModeTests.csproj`, and `TheCat.Editor.csproj`.
- Confirm these gates pass:
  - `P0AssetManifestCoverage`
  - `P0AssetGenerationBatchCoverage`
  - `P0AssetImportReadiness`
  - `P0AssetMetaImportSettingsReadiness`
  - `P0RuntimeVisualBindingCoverage`
  - `P0AssetReviewPacket`
  - `P0AssetProductionReadiness`
  - `P0BattleFeedbackVisualCoverage`

## Review Focus

- The six VFX are readable in the feedback card at small size.
- The assets stay non-cat and do not conflict with starter cat turnaround identity.
- Runtime binding order is stable and screenshot/contact-sheet evidence is refreshed.
