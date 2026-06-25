# P0 Asset Batch 41 - Cold Light AI Refinement Candidate

Date: 2026-06-15

## Task Scope

Create one Codex built-in image-generation refinement candidate for the P0
enemy Cold Light Shadow, then standardize it into a project-local candidate
pack for review.

This is a candidate review batch only. Do not import the candidate into Unity,
do not replace runtime enemy sprites, and do not modify runtime visual bindings,
prefabs, scenes, source-lock hashes, or manifest counts.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/00_overview/p0_minimum_design.md`
- `design/梦境支配者核心玩法/docs/02_combat_and_systems/core_numeric_system_v1.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- `design/梦境支配者核心玩法/assets/enemies/en04_cold_light_shadow/concept/cold_light_shadow_concept.png`
- `design/梦境支配者核心玩法/assets/enemies/en04_cold_light_shadow/animation/cold_light_shadow_animation.png`
- `design/development/asset_candidates/enemies/p0_core/batch_38_p0_enemy_source_reference_pack_2026-06-15/thecat_enemy_p0_core_batch38_source_reference_review_sheet.png`

## Project Files To Read

- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `Assets/TheCat/Scripts/Runtime/Tools/P0HardReferenceSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `design/development/tools/build_cold_light_ai_refinement_candidate.py`
- `design/development/tools/validate_cold_light_ai_refinement_candidate.ps1`

## Built-In Image Generation Prompt

```text
Use case: stylized-concept
Asset type: game enemy combat sprite candidate for TheCat P0 enemy Cold Light Shadow
Primary request: Create one polished full-body combat sprite candidate for Cold Light Shadow, strictly preserving the provided reference image identity.
Input images: the visible Cold Light Shadow concept image is the hard visual authority.
Subject: corrupted mechanical desk-lamp nightmare enemy, thin floating lamp-shadow silhouette, angled metal lamp arm with springs and joints, cold cyan lamp head glow, one hostile red eye inside the pale cyan light, black mud dripping from the arm and shade, black mud base pooling around the foot, ranged beam pressure cue.
Style/medium: hand-painted dream-fantasy 2D game sprite, readable combat silhouette for a top/angled camera, soft ink linework, painterly metal and glossy mud texture, consistent with the provided reference.
Composition/framing: centered single enemy, square 1024x1024, three-quarter front view, full body including base and lamp head, generous padding, neutral warm parchment background, no UI frame.
Color palette: lock to dark gunmetal, black glossy mud, pale cyan cold light, small saturated red eye, subtle cold blue highlights, neutral parchment ground.
Constraints: preserve cold lamp silhouette, cyan beam/light language, mechanical arm, black mud base, single red eye, long shadow-limb feel, and ranged-pressure read. Keep it clearly the same enemy as the reference, not a redesign.
Avoid: ordinary clean desk lamp, warm candle or fire palette, cute robot, humanoid body, human hands, extra phone/alarm/toy motifs, black mud removed, missing red eye, missing cyan light, missing mechanical spring arm, missing mud base, gore, realistic horror anatomy, text, logo, watermark, UI frame, heavy scene background.
```

## Expected Output

- One Batch 41 candidate folder under
  `design/development/asset_candidates/enemies/cold_light_shadow/batch_41_cold_light_ai_refinement_candidate_2026-06-15`.
- One raw project copy of the Codex built-in generated PNG.
- One standardized `1024x1024` combat candidate.
- One standardized `512x512` preview.
- One prompt record, one manifest CSV, one review note, one summary, and one
  review sheet.
- All outputs stay outside `Assets`.
- Formal Unity import remains blocked until active-enemy Play Mode screenshot
  review passes.

## Output Policy

- Candidate files must stay under `design/development/asset_candidates/enemies`.
- Keep all candidate PNGs outside `Assets`.
- Do not create Unity `.meta` files for candidate outputs.
- Do not modify `Assets/TheCat/Art/Enemies`.
- Do not modify source concept or animation PNGs.
- Do not modify hard-reference source-lock hashes.
- Do not modify prefabs, scenes, runtime visual bindings, or formal import
  state.

## Visual Lock Rules

Cold Light Shadow must preserve cold lamp silhouette, cyan beam/light language,
mechanical arm, black mud base, single red eye, long shadow-limb feel, and
ranged-pressure read.

Reject ordinary clean desk lamp, warm candle or fire palette, cute robot
styling, humanoid body, black mud removal, missing red eye, missing cyan light,
missing mechanical spring arm, missing mud base, or extra dream interruption
objects. Reject any Unity import before active-enemy screenshot review.

## Validation

```powershell
& 'C:\Users\PC\.cache\codex-runtimes\codex-primary-runtime\dependencies\python\python.exe' design/development/tools/build_cold_light_ai_refinement_candidate.py <generated_image_path>
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_cold_light_ai_refinement_candidate.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```

Unity validation remains pending until MCP/editor execution is available:

1. Refresh AssetDatabase only after a separate import batch exists.
2. Capture `08-active-enemy-cold-light.png`.
3. Compare runtime enemy scale, silhouette, beam warning readability, black mud
   base, and ranged-pressure read against the Batch 41 review sheet and Batch
   38 source reference sheet.
4. Check Unity Console, Sprite import settings, prefab references, and scene
   bindings before approving any runtime installation.
