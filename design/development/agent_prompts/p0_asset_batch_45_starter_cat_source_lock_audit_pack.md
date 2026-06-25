# P0 Asset Batch 45 - Starter Cat Source-Lock Audit Pack

Date: 2026-06-15

This is an audit-pack batch for systematic starter-cat asset production. It does not generate new model art, does not replace runtime sprites, and does not import anything into Unity.

## Task Scope

Create a source-lock audit pack for Saiban, Nephthys, and Suzune so every future cat asset strictly matches the colored three-view turnaround.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/03_characters/character_design.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- Saiban source: `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- Nephthys source: `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- Suzune source: `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`

## Required Project Sources

- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatTurnaroundSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatFormalImportReadiness.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetProductionReadiness.cs`
- `design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.md`
- `design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`
- `design/development/asset_review/p0_codex_to_unity_asset_pipeline_2026-06-14.md`
- Latest cutout manifests for Batch 31, Batch 35, and Batch 37.

## Expected Outputs

- Per-cat source-lock lineage cards under:
  - `design/development/asset_candidates/starter_cats/saiban/batch_45_starter_cat_source_lock_audit_pack_2026-06-15/`
  - `design/development/asset_candidates/starter_cats/nephthys/batch_45_starter_cat_source_lock_audit_pack_2026-06-15/`
  - `design/development/asset_candidates/starter_cats/suzune/batch_45_starter_cat_source_lock_audit_pack_2026-06-15/`
- Combined review sheet under:
  `design/development/asset_candidates/starter_cats/batch_45_starter_cat_source_lock_audit_pack_2026-06-15/`
- Manifest, review note, and process note under the same batch root.

## Output Policy

- candidate files stay outside `Assets`.
- do not import into Unity.
- do not create Unity `.meta` files.
- do not edit source turnaround PNGs.
- do not edit `Assets/TheCat/Art/Characters/Sprites`.
- do not change source-lock hashes, runtime visual bindings, or manifest counts.
- formal import remains blocked until active-cat Play Mode screenshot comparison passes.

## Cat-Specific Lock Rules

Saiban must preserve shield, sword, tabby face, cape, helm, silver-gray fur, red cape, silver armor, gold trim, blue gem accents, and front/side/back anchors from the colored three-view turnaround.

Nephthys must preserve hood, pyramid/obelisk prop, gold-brown tabby fur, deep navy cloak, sand-gold trim, blue gems, ankh/script marks, compact non-human cat posture, and front/side/back anchors from the colored three-view turnaround.

Suzune must preserve calico markings, shrine outfit, bells, wand/branch, vermilion and white fabric, blue talismans, compact non-human cat posture, and front/side/back anchors from the colored three-view turnaround.

## Review Rules

- Reject any asset that only resembles the project style but does not strictly match the colored three-view turnaround.
- Reject human body proportions, human costume pose, or generic cute mascot replacement.
- Reject missing side/back anchors even if the front view seems close.
- Reject palette drift from the locked colored turnaround.
- Reject any candidate without direct active-cat Play Mode screenshot comparison.

## Validation

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_starter_cat_source_lock_audit_pack.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_saiban_cutout_candidate.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_nephthys_cutout_candidate.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_suzune_cutout_candidate.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```
