# P0 Asset Batch 44 - Call Tyrant Boss Transparent Cutout Candidate

Date: 2026-06-15

## Task Scope

Create a deterministic Boss transparent cutout candidate from the Batch 43 Call
Tyrant AI refinement image. This is a local post-processing candidate batch
only. Do not import the result into Unity and do not replace runtime Boss
sprites, warning VFX, framesheets, prefabs, scenes, or manifest counts.

This is a one-enemy-at-a-time Call Tyrant task. Do not produce Black Mud, Cold
Light, starter-cat, UI, or additional enemy bitmap candidates in this batch.

## Required Design Sources

- `design/姊﹀鏀厤鑰呮牳蹇冪帺娉?docs/00_overview/p0_minimum_design.md`
- `design/姊﹀鏀厤鑰呮牳蹇冪帺娉?docs/02_combat_and_systems/core_numeric_system_v1.md`
- `design/姊﹀鏀厤鑰呮牳蹇冪帺娉?docs/04_art_production/p0_digital_asset_inventory.md`
- `design/姊﹀鏀厤鑰呮牳蹇冪帺娉?assets/enemies/en06_call_tyrant/concept/call_tyrant_concept.png`
- `design/姊﹀鏀厤鑰呮牳蹇冪帺娉?assets/enemies/en06_call_tyrant/animation/call_tyrant_animation.png`
- `design/development/asset_candidates/enemies/p0_core/batch_38_p0_enemy_source_reference_pack_2026-06-15/thecat_enemy_p0_core_batch38_source_reference_review_sheet.png`
- `design/development/asset_candidates/enemies/call_tyrant/batch_43_call_tyrant_ai_refinement_candidate_2026-06-15/thecat_enemy_call_tyrant_batch43_ai_refinement_review_sheet.png`

## Project Files To Read

- `design/development/asset_review/p0_codex_to_unity_asset_pipeline_2026-06-14.md`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/prompts/p0_boss_assets.md`
- `design/development/tools/build_call_tyrant_ai_refinement_candidate.py`
- `design/development/tools/validate_call_tyrant_ai_refinement_candidate.ps1`
- `design/development/tools/build_cold_light_cutout_candidate.py`
- `design/development/tools/validate_cold_light_cutout_candidate.ps1`
- `Assets/TheCat/Scripts/Runtime/Tools/P0HardReferenceSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`

## Expected Output

- One transparent `1024x1024` Call Tyrant Boss cutout candidate under
  `design/development/asset_candidates/enemies/call_tyrant/batch_44_call_tyrant_cutout_candidate_2026-06-15`.
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
  `design/development/asset_candidates/enemies/call_tyrant`.
- Keep all candidate PNGs outside `Assets`.
- Do not create Unity `.meta` files for candidate outputs.
- Do not modify `Assets/TheCat/Art/Enemies/Sprites`.
- Do not modify source concept, source animation, or source-lock hashes.
- Do not modify prefabs, scenes, runtime visual bindings, runtime manifests, or
  formal import state.

## Call Tyrant Lock Rules

Call Tyrant must preserve giant phone shell, red call-eye signal, purple tie,
black mud body and base, app projectile language, summon portal/minion
vibration feel, Boss-scale silhouette, cracked glass screen, and phone-call
nightmare identity.

Reject human office boss body, generic smartphone icon mascot, cute robot
styling, clean ordinary phone, brand logos, readable text, keyboard, laptop,
alarm/lamp/toy motifs, black mud removal, missing purple tie, missing red call
eyes, missing cracked phone shell, or missing app projectile language.

## Cutout Method

Use deterministic local post-processing only. Start from:

`design/development/asset_candidates/enemies/call_tyrant/batch_43_call_tyrant_ai_refinement_candidate_2026-06-15/thecat_enemy_call_tyrant_ai_refinement_combat_1024_candidate_v001.png`

The builder should sample the image border, keep dark phone/mud, red call-eye,
purple tie, and saturated app projectile pixels opaque, preserve throw streak
pixels as semi-transparent alpha, convert non-foreground parchment regions to
alpha, and generate checkerboard, dark-field, warm-HUD, alpha-mask, and preview
evidence.

## Validation

```powershell
& 'C:\Users\PC\.cache\codex-runtimes\codex-primary-runtime\dependencies\python\python.exe' design/development/tools/build_call_tyrant_cutout_candidate.py
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_call_tyrant_cutout_candidate.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```

Unity validation remains pending until MCP/editor execution is available:

1. Refresh AssetDatabase only after a separate import batch exists.
2. Capture `09-active-enemy-call-tyrant.png`.
3. Compare runtime Boss scale, silhouette, summon readability, app-throw
   readability, dark-field contrast, warm-HUD contrast, and hitbox readability
   against Batch 38, Batch 43, and this Batch 44 cutout sheet.
4. Check Unity Console, Sprite import settings, prefab references, scene
   bindings, and warning VFX separation before approving any runtime
   installation.
