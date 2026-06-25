# P0 Asset Batch 42 - Cold Light Beam-Preserving Transparent Cutout Candidate

Date: 2026-06-15

## Task Scope

Create a deterministic beam-preserving transparent cutout candidate from the
Batch 41 Cold Light Shadow AI refinement image. This is a local
post-processing candidate batch only. Do not import the result into Unity and
do not replace runtime enemy sprites or warning VFX.

This is a one-enemy-at-a-time Cold Light task. Do not produce Black Mud, Call
Tyrant, starter-cat, UI, or additional enemy bitmap candidates in this batch.

## Required Design Sources

- `design/姊﹀鏀厤鑰呮牳蹇冪帺娉?docs/00_overview/p0_minimum_design.md`
- `design/姊﹀鏀厤鑰呮牳蹇冪帺娉?docs/02_combat_and_systems/core_numeric_system_v1.md`
- `design/姊﹀鏀厤鑰呮牳蹇冪帺娉?docs/04_art_production/p0_digital_asset_inventory.md`
- `design/姊﹀鏀厤鑰呮牳蹇冪帺娉?assets/enemies/en04_cold_light_shadow/concept/cold_light_shadow_concept.png`
- `design/姊﹀鏀厤鑰呮牳蹇冪帺娉?assets/enemies/en04_cold_light_shadow/animation/cold_light_shadow_animation.png`
- `design/development/asset_candidates/enemies/p0_core/batch_38_p0_enemy_source_reference_pack_2026-06-15/thecat_enemy_p0_core_batch38_source_reference_review_sheet.png`
- `design/development/asset_candidates/enemies/cold_light_shadow/batch_41_cold_light_ai_refinement_candidate_2026-06-15/thecat_enemy_cold_light_batch41_ai_refinement_review_sheet.png`

## Project Files To Read

- `design/development/asset_review/p0_codex_to_unity_asset_pipeline_2026-06-14.md`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/tools/build_cold_light_ai_refinement_candidate.py`
- `design/development/tools/validate_cold_light_ai_refinement_candidate.ps1`
- `design/development/tools/build_black_mud_cutout_candidate.py`
- `design/development/tools/validate_black_mud_cutout_candidate.ps1`
- `Assets/TheCat/Scripts/Runtime/Tools/P0HardReferenceSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`

## Expected Output

- One beam-preserving transparent `1024x1024` Cold Light cutout candidate under
  `design/development/asset_candidates/enemies/cold_light_shadow/batch_42_cold_light_cutout_candidate_2026-06-15`.
- One `512x512` transparent preview PNG.
- One `512x512` checkerboard review composite.
- One `512x512` dark-field review composite.
- One `512x512` warm-HUD review composite.
- One `512x512` alpha-mask review PNG.
- One manifest, review note, process note, and review sheet.
- All outputs stay outside `Assets`.
- formal import remains blocked until active-enemy Play Mode screenshot review
  passes.

## Output Policy

- Candidate files must stay under
  `design/development/asset_candidates/enemies/cold_light_shadow`.
- Keep all candidate PNGs outside `Assets`.
- Do not create Unity `.meta` files for candidate outputs.
- Do not modify `Assets/TheCat/Art/Enemies/Sprites`.
- Do not modify source concept, source animation, or source-lock hashes.
- Do not modify prefabs, scenes, runtime visual bindings, runtime manifests, or
  formal import state.

## Cold Light Lock Rules

Cold Light Shadow must preserve cold lamp silhouette, cyan beam/light language,
mechanical spring arm, black mud base, single red eye, long shadow-limb feel,
and ranged-pressure read.

Reject ordinary clean desk lamp, warm candle or fire palette, cute robot
styling, humanoid body, black mud removal, missing red eye, missing cyan light,
missing spring arm, missing mud base, or removal of all beam readability.

## Cutout Method

Use deterministic local post-processing only. Start from:

`design/development/asset_candidates/enemies/cold_light_shadow/batch_41_cold_light_ai_refinement_candidate_2026-06-15/thecat_enemy_cold_light_shadow_ai_refinement_combat_1024_candidate_v001.png`

The builder should sample the image border, keep dark metal/mud and red-eye
pixels opaque, preserve pale cyan lamp and beam pixels as semi-transparent
cyan alpha, convert non-foreground parchment regions to alpha, and generate
checkerboard, dark-field, warm-HUD, alpha-mask, and preview evidence.

The semi-transparent cyan beam cue is part of this candidate's review evidence;
do not erase every beam pixel during local alpha preparation.

## Validation

```powershell
& 'C:\Users\PC\.cache\codex-runtimes\codex-primary-runtime\dependencies\python\python.exe' design/development/tools/build_cold_light_cutout_candidate.py
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_cold_light_cutout_candidate.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```

Unity validation remains pending until MCP/editor execution is available:

1. Refresh AssetDatabase only after a separate import batch exists.
2. Capture `08-active-enemy-cold-light.png`.
3. Compare runtime enemy scale, silhouette, beam-warning readability,
   ranged-pressure read, dark-field contrast, warm-HUD contrast, and hitbox
   readability against Batch 38, Batch 41, and this Batch 42 cutout sheet.
4. Check Unity Console, Sprite import settings, prefab references, scene
   bindings, and warning VFX separation before approving any runtime
   installation.
