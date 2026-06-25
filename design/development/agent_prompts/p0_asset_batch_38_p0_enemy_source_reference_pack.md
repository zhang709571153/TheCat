# P0 Asset Batch 38 - P0 Core Enemy Source Reference Pack

Date: 2026-06-15

## Task Scope

Create a deterministic source-derived reference pack for the three P0 core
enemies: Black Mud Nightmare, Cold Light Shadow, and Call Tyrant.

This is a review and source-lock batch only. Do not generate new enemy art with
AI, do not import the outputs into Unity, and do not replace runtime sprites,
VFX, framesheets, prefabs, scenes, or manifest counts.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/00_overview/p0_minimum_design.md`
- `design/梦境支配者核心玩法/docs/02_combat_and_systems/core_numeric_system_v1.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- `design/梦境支配者核心玩法/assets/enemies/en01_black_mud_nightmare/concept/black_mud_nightmare_concept.png`
- `design/梦境支配者核心玩法/assets/enemies/en01_black_mud_nightmare/animation/black_mud_nightmare_animation.png`
- `design/梦境支配者核心玩法/assets/enemies/en04_cold_light_shadow/concept/cold_light_shadow_concept.png`
- `design/梦境支配者核心玩法/assets/enemies/en04_cold_light_shadow/animation/cold_light_shadow_animation.png`
- `design/梦境支配者核心玩法/assets/enemies/en06_call_tyrant/concept/call_tyrant_concept.png`
- `design/梦境支配者核心玩法/assets/enemies/en06_call_tyrant/animation/call_tyrant_animation.png`

## Project Files To Read

- `Assets/TheCat/Scripts/Runtime/Tools/P0HardReferenceSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Tests/EditMode/P0AssetManifestCoverageTests.cs`
- `Assets/TheCat/Tests/EditMode/P0VisualAssetCatalogTests.cs`
- `design/development/tools/build_p0_enemy_source_reference_pack.py`
- `design/development/tools/validate_p0_enemy_source_reference_pack.ps1`

## Expected Output

- One Batch 38 candidate folder under
  `design/development/asset_candidates/enemies/p0_core/batch_38_p0_enemy_source_reference_pack_2026-06-15`.
- For each P0 core enemy, generate:
  - `concept_reference_512`
  - `animation_reference_512`
  - `combat_sprite_reference_512`
  - `warning_motif_reference_256`
  - `palette_swatch_reference_256`
- One manifest CSV under
  `design/development/asset_candidates/enemies/batch_38_p0_enemy_source_reference_pack_2026-06-15`.
- One review note, one summary, and one combined review sheet.
- All outputs stay outside `Assets`.
- Formal Unity import remains blocked until active-enemy Play Mode screenshot
  review passes.

## Output Policy

- Candidate files must stay under `design/development/asset_candidates/enemies`.
- Keep all candidate PNGs outside `Assets`.
- Do not create Unity `.meta` files for candidate outputs.
- Do not modify `Assets/TheCat/Art/Enemies`.
- Do not modify design source PNGs or hard-reference source-lock hashes.
- Do not modify prefabs, scenes, runtime visual bindings, or formal import
  state.

## Enemy Lock Rules

Black Mud Nightmare must preserve black sludge body, red eyes, soft-mud monster
silhouette, crawling pressure, and bed-contact threat. Reject cute pet styling,
generic ghost shape, or loss of the red eye threat read.

Cold Light Shadow must preserve cold lamp silhouette, cyan beam language,
mechanical arm, black mud base, and single red eye. Reject ordinary desk lamp,
warm candle/fire palette, or missing ranged beam cue.

Call Tyrant must preserve giant phone shell, red call eyes, purple tie, black
mud body, app projectile language, and summon portal vibration. Reject human
office boss body, generic phone app icon, or missing purple tie/red call-eye
signal.

## Validation

```powershell
& 'C:\Users\PC\.cache\codex-runtimes\codex-primary-runtime\dependencies\python\python.exe' design/development/tools/build_p0_enemy_source_reference_pack.py
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_p0_enemy_source_reference_pack.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```

Unity validation remains pending until MCP/editor execution is available:

1. Refresh AssetDatabase only after a separate import batch exists.
2. Capture active-enemy screenshots:
   - `07-active-enemy-black-mud.png`
   - `08-active-enemy-cold-light.png`
   - `09-active-enemy-call-tyrant.png`
3. Compare runtime enemy scale, silhouette, warning readability, and behavior
   cues against this Batch 38 review sheet.
4. Check Unity Console, Sprite import settings, prefab references, and scene
   bindings before approving any runtime installation.
