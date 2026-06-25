# P0 Asset Batch 74 - Starter Cat Runtime Combat Sprite Source Audit

Date: 2026-06-15

## Task Scope

Audit the currently runtime-bound Saiban, Nephthys, and Suzune combat sprites
against the strict colored-turnaround source chain. This task does not generate
new cat body art and does not replace any Unity sprite. Its purpose is to make
the current runtime baseline explicit before the next formal animation,
paintover, or image-generation pass.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/03_characters/character_design.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- Batch 70 front/side/back reference plates:
  `design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15`

## Project Files To Read

- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RuntimeVisualBindingCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetReviewPacket.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatFormalImportReadiness.cs`
- `Assets/TheCat/Tests/EditMode/P0VisualAssetCatalogTests.cs`
- `Assets/TheCat/Tests/EditMode/P0RuntimeVisualBindingCoverageTests.cs`
- `Assets/TheCat/Tests/EditMode/P0AssetReviewPacketTests.cs`

## Runtime Rows

- `cat.combat.saiban` -> `Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png`; source lock `saiban_turnaround_colored`; front plate `design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/thecat_cat_saiban_turnaround_front_reference_plate_768_batch70_v001.png`
- `cat.combat.nephthys` -> `Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png`; source lock `nephthys_turnaround_colored`; front plate `design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/thecat_cat_nephthys_turnaround_front_reference_plate_768_batch70_v001.png`
- `cat.combat.suzune` -> `Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png`; source lock `suzune_turnaround_colored`; front plate `design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/thecat_cat_suzune_turnaround_front_reference_plate_768_batch70_v001.png`


## Forbidden Changes

- Do not overwrite the three runtime combat sprites.
- Do not import Batch 49, 50, or 51 AI candidates into `Assets`.
- Do not change `P0StarterCatFormalImportReadiness` from blocked to approved.
- Do not remove the active-cat Play Mode screenshot requirement.
- Do not edit the original colored turnaround sources or Batch 70 reference plates.

## Expected Outputs

- Batch 74 manifest, review sheet, review note, process note, and agent prompt.
- Runtime evidence class and EditMode test coverage proving that all three
  runtime-bound combat sprites have source-lock, front-plate, hash, meta, and
  runtime-binding evidence.
- `P0AssetReviewPacket` section showing Batch 74 is ready while formal
  cat-body import remains blocked pending active-cat Play Mode screenshots.

## Validation

```powershell
& 'C:\Users\PC\.cache\codex-runtimes\codex-primary-runtime\dependencies\python\python.exe' design/development/tools/build_starter_cat_runtime_combat_sprite_source_audit.py
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_starter_cat_runtime_combat_sprite_source_audit.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```

Unity MCP validation, when available:

- Refresh AssetDatabase.
- Inspect all three runtime sprite import settings.
- Capture active-cat Play Mode screenshots for Saiban, Nephthys, and Suzune.
- Compare runtime screenshots against the Batch 74 review sheet and Batch 70
  front reference plates.
