# P0 Asset Batch 58 - Starter Cat HUD Avatar Install

Date: 2026-06-15

## Task Scope

Install source-locked HUD avatar icons for Saiban, Nephthys, and Suzune. This
batch must produce deterministic 256x256 transparent avatar icons derived from
the locked starter-cat combat sprites, which are themselves hash-locked to the
colored three-view turnarounds.

This is not an AI body-art generation task. Do not import Batch 48, Batch 49,
Batch 50, or Batch 51 AI candidate body art.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/03_characters/character_design.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- Saiban source: `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- Nephthys source: `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- Suzune source: `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`

## Project Files To Read

- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatTurnaroundSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetMetaImportSettingsReadiness.cs`
- `Assets/TheCat/Tests/EditMode/P0VisualAssetCatalogTests.cs`
- `Assets/TheCat/Tests/EditMode/P0AssetMetaImportSettingsReadinessTests.cs`
- `design/development/asset_candidates/starter_cats/batch_45_starter_cat_source_lock_audit_pack_2026-06-15/starter_cat_batch45_source_lock_audit_manifest.csv`
- `design/development/asset_candidates/starter_cats/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/starter_cat_batch47_strict_generation_spec_manifest.csv`

## Expected Outputs

- Installed Unity PNGs:
  - `Assets/TheCat/Art/UI/Icons/thecat_cat_saiban_hud_avatar_256_v001.png`
  - `Assets/TheCat/Art/UI/Icons/thecat_cat_nephthys_hud_avatar_256_v001.png`
  - `Assets/TheCat/Art/UI/Icons/thecat_cat_suzune_hud_avatar_256_v001.png`
- Matching `.png.meta` files with Sprite Single import settings, disabled
  mipmaps, alpha transparency, and `TheCatP0ImportSettings:v1`.
- Batch 58 manifest and review packet under:
  `design/development/asset_candidates/starter_cats/batch_58_starter_cat_hud_avatar_install_2026-06-15`
- Runtime catalog bindings:
  - `cat.avatar.saiban`
  - `cat.avatar.nephthys`
  - `cat.avatar.suzune`

## Forbidden Changes

- Do not overwrite starter-cat combat sprites.
- Do not edit colored turnaround source PNGs.
- Do not copy AI candidate body art into `Assets`.
- Do not approve formal starter-cat body-art import.
- Do not remove source-lock ids from starter-cat manifest rows.

## Acceptance

- All three installed avatar PNGs are exactly `256x256`.
- All three installed avatar PNGs have alpha transparency.
- All three avatar manifest rows carry source locks:
  - `saiban_turnaround_colored`
  - `nephthys_turnaround_colored`
  - `suzune_turnaround_colored`
- `P0VisualAssetCatalog.GetStarterCatHudAvatar` resolves each avatar.
- Runtime visual bindings include the three avatar slots.
- `P0AssetMetaImportSettingsReadiness.EvaluateP0Manifest()` remains ready.
- Batch 58 review note explicitly says no AI cat body art was imported.

## Validation

```powershell
& 'C:\Users\PC\.cache\codex-runtimes\codex-primary-runtime\dependencies\python\python.exe' design/development/tools/build_starter_cat_hud_avatar_install_assets.py
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_starter_cat_hud_avatar_install.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```

Unity MCP validation, when available:

- Refresh AssetDatabase.
- Inspect all three avatar import settings.
- Capture HUD / character-selection screenshots at runtime scale.
- Compare avatar screenshots against the Batch 58 review sheet and colored
  three-view turnarounds.
