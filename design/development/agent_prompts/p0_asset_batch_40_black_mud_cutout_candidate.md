# P0 Asset Batch 40 - Black Mud Transparent Cutout Candidate

Date: 2026-06-15

## Task Scope

Create a deterministic transparent cutout candidate from the Batch 39 Black Mud
Nightmare AI refinement image. This is a local post-processing candidate batch
only. Do not import the result into Unity and do not replace runtime enemy
sprites.

This is a one-enemy-at-a-time Black Mud task. Do not produce Cold Light, Call
Tyrant, starter-cat, or additional enemy bitmap candidates in this batch.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/00_overview/p0_minimum_design.md`
- `design/梦境支配者核心玩法/docs/02_combat_and_systems/core_numeric_system_v1.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- `design/梦境支配者核心玩法/assets/enemies/en01_black_mud_nightmare/concept/black_mud_nightmare_concept.png`
- `design/梦境支配者核心玩法/assets/enemies/en01_black_mud_nightmare/animation/black_mud_nightmare_animation.png`
- `design/development/asset_candidates/enemies/p0_core/batch_38_p0_enemy_source_reference_pack_2026-06-15/thecat_enemy_p0_core_batch38_source_reference_review_sheet.png`
- `design/development/asset_candidates/enemies/black_mud_nightmare/batch_39_black_mud_ai_refinement_candidate_2026-06-15/thecat_enemy_black_mud_batch39_ai_refinement_review_sheet.png`

## Project Files To Read

- `design/development/asset_review/p0_codex_to_unity_asset_pipeline_2026-06-14.md`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/tools/build_black_mud_cutout_candidate.py`
- `design/development/tools/validate_black_mud_cutout_candidate.ps1`
- `Assets/TheCat/Scripts/Runtime/Tools/P0HardReferenceSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`

## Expected Output

- One transparent `1024x1024` Black Mud cutout candidate under
  `design/development/asset_candidates/enemies/black_mud_nightmare/batch_40_black_mud_cutout_candidate_2026-06-15`.
- One `512x512` transparent preview PNG.
- One `512x512` checkerboard review composite.
- One `512x512` dark-field review composite.
- One `512x512` alpha-mask review PNG.
- One manifest, review note, process note, and review sheet.
- All outputs stay outside `Assets`.
- formal import remains blocked until active-enemy Play Mode screenshot review
  passes.

## Output Policy

- Candidate files must stay under
  `design/development/asset_candidates/enemies/black_mud_nightmare`.
- Keep all candidate PNGs outside `Assets`.
- Do not create Unity `.meta` files for candidate outputs.
- Do not modify `Assets/TheCat/Art/Enemies/Sprites`.
- Do not modify source concept, source animation, or source-lock hashes.
- Do not modify prefabs, scenes, runtime visual bindings, runtime manifests, or
  formal import state.

## Black Mud Lock Rules

Black Mud Nightmare must preserve black sludge body, red eyes, soft-mud monster
silhouette, crawling pressure, bed-contact threat, glossy pooled mud, sleepy
face imprint, top drip, low squat shape, and puddled crawl edges.

Reject clipping of the sludge body, red eyes, eye glow, top drip, puddled crawl
edges, or low silhouette. Reject cute pet styling, generic ghost shape,
humanoid body, gore, realistic horror anatomy, extra dream interruption
objects, cat features, or palette drift away from the source concept.

## Cutout Method

Use deterministic local post-processing only. Start from:

`design/development/asset_candidates/enemies/black_mud_nightmare/batch_39_black_mud_ai_refinement_candidate_2026-06-15/thecat_enemy_black_mud_nightmare_ai_refinement_combat_1024_candidate_v001.png`

The builder should sample the image border, identify dark-mud and red-eye
foreground pixels that differ from the parchment background, remove tiny specks,
convert non-foreground regions to alpha, and generate checkerboard, dark-field,
alpha-mask, and preview evidence.

## Validation

```powershell
& 'C:\Users\PC\.cache\codex-runtimes\codex-primary-runtime\dependencies\python\python.exe' design/development/tools/build_black_mud_cutout_candidate.py
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_black_mud_cutout_candidate.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```

Unity validation remains pending until MCP/editor execution is available:

1. Refresh AssetDatabase only after a separate import batch exists.
2. Capture `07-active-enemy-black-mud.png`.
3. Compare runtime enemy scale, silhouette, warning readability, dark-field
   contrast, hitbox readability, and bed-contact threat read against Batch 38,
   Batch 39, and this Batch 40 cutout sheet.
4. Check Unity Console, Sprite import settings, prefab references, and scene
   bindings before approving any runtime installation.
