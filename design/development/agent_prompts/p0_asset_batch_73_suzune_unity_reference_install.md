# P0 Asset Batch 73 - Suzune Unity Reference Install

Date: 2026-06-15

## Task Scope

Install a Unity-side Suzune source-reference atlas built only from
the Batch 70 front, side, and back reference plates. This batch is an
installation and audit bridge: it gives Unity reviewers a hard in-project visual
reference while the current Suzune combat sprite remains unchanged.

This is not a new cat-body generation task. Do not use AI-generated
Suzune body art, do not replace the existing combat sprite, and do
not approve formal starter-cat body-art import.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/03_characters/character_design.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- Suzune colored turnaround:
  `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`
- Batch 70 Suzune reference plates:
  - `design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/thecat_cat_suzune_turnaround_front_reference_plate_768_batch70_v001.png`
  - `design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/thecat_cat_suzune_turnaround_side_reference_plate_768_batch70_v001.png`
  - `design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/thecat_cat_suzune_turnaround_back_reference_plate_768_batch70_v001.png`

## Project Files To Read

- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetMetaImportSettingsReadiness.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetReviewPacket.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatUnityReferenceInstallEvidence.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatSourceLockPacketEvidence.cs`
- `Assets/TheCat/Tests/EditMode/P0AssetReviewPacketTests.cs`
- `Assets/TheCat/Tests/EditMode/P0StarterCatSourceLockPacketEvidenceTests.cs`
- `design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/starter_cat_turnaround_reference_plates_batch70_manifest.csv`

## Expected Outputs

- Installed Unity atlas:
  `Assets/TheCat/Art/Characters/References/thecat_cat_suzune_turnaround_reference_atlas_2304x768_v001.png`
- Matching `.png.meta` with Sprite Single import settings, disabled mipmaps,
  alpha transparency, max texture size at least 4096, and
  `TheCatP0ImportSettings:v1`.
- Batch 73 manifest and review packet under:
  `design/development/asset_candidates/starter_cats/suzune/batch_73_suzune_unity_reference_install_2026-06-15`
- Runtime review evidence that marks this as:
  `installed_debug_reference_not_runtime_binding_pending_unity_visual_smoke`.

## Forbidden Changes

- Do not overwrite `Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png`.
- Do not overwrite the Suzune HUD avatar `thecat_cat_suzune_hud_avatar_256_v001`.
- Do not edit the colored turnaround source PNG.
- Do not edit or regenerate Batch 70 reference plates.
- Do not copy any AI body-art candidate into `Assets`.
- Do not add a runtime visual binding for this debug reference atlas.

## Acceptance

- The installed atlas is exactly `2304x768`.
- The atlas is a direct left-to-right concatenation of the Batch 70 front,
  side, and back Suzune reference plates.
- The manifest records the Suzune colored-turnaround source hash,
  current combat sprite hash, all three Batch 70 plate hashes, Unity import
  path, and meta path.
- `P0AssetManifestCatalog` includes the atlas as a `reference_atlas` generated
  asset with `suzune_turnaround_colored` source lock.
- `P0AssetReviewPacket` reports starter-cat Unity reference installs as ready
  while keeping the assets non-runtime-bound and pending Unity visual smoke.
- Review notes explicitly state that no AI cat body art was imported and that
  formal starter-cat body-art import remains blocked.

## Validation

```powershell
& 'C:\Users\PC\.cache\codex-runtimes\codex-primary-runtime\dependencies\python\python.exe' design/development/tools/build_starter_cat_unity_reference_install_assets.py --cats suzune
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_starter_cat_unity_reference_installs.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_starter_cat_turnaround_reference_plates.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```

Unity MCP validation, when available:

- Refresh AssetDatabase.
- Inspect the reference atlas import settings.
- Open the atlas in Unity and compare it against the Batch 70 review sheet.
- Capture active-cat Suzune Play Mode screenshot and compare
  against the atlas before any future formal cat-body import decision.
