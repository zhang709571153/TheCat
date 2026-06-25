# P0 Asset Batch 39 - Black Mud AI Refinement Candidate

Date: 2026-06-15

## Task Scope

Create one Codex built-in image-generation refinement candidate for the P0
enemy Black Mud Nightmare, then standardize it into a project-local candidate
pack for review.

This is a candidate review batch only. Do not import the candidate into Unity,
do not replace runtime enemy sprites, and do not modify runtime visual bindings,
prefabs, scenes, source-lock hashes, or manifest counts.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/00_overview/p0_minimum_design.md`
- `design/梦境支配者核心玩法/docs/02_combat_and_systems/core_numeric_system_v1.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- `design/梦境支配者核心玩法/assets/enemies/en01_black_mud_nightmare/concept/black_mud_nightmare_concept.png`
- `design/梦境支配者核心玩法/assets/enemies/en01_black_mud_nightmare/animation/black_mud_nightmare_animation.png`
- `design/development/asset_candidates/enemies/p0_core/batch_38_p0_enemy_source_reference_pack_2026-06-15/thecat_enemy_p0_core_batch38_source_reference_review_sheet.png`

## Project Files To Read

- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `Assets/TheCat/Scripts/Runtime/Tools/P0HardReferenceSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `design/development/tools/build_black_mud_ai_refinement_candidate.py`
- `design/development/tools/validate_black_mud_ai_refinement_candidate.ps1`

## Built-In Image Generation Prompt

```text
Use case: stylized-concept
Asset type: game enemy combat sprite candidate for TheCat P0 enemy Black Mud Nightmare
Primary request: Create one polished full-body combat sprite candidate for Black Mud Nightmare, strictly preserving the provided reference image identity.
Input images: the visible Black Mud Nightmare concept image is the hard visual authority.
Subject: low crawling black-violet sludge mass, glossy soft mud body, squat dome silhouette, two bright hostile red eyes, dark sleepy face imprint inside the mud, small dripping curl of sludge on top, puddled edges spreading along the floor, readable near-bed pressure threat.
Style/medium: hand-painted dream-fantasy 2D game sprite, clean readable silhouette, soft ink linework, painterly highlights, consistent with the provided reference.
Composition/framing: centered single enemy, square 1024x1024, three-quarter front view suitable for top/angled combat camera, generous padding, neutral warm parchment background, no UI frame.
Color palette: lock to black and deep violet mud, glossy indigo highlights, dim gray sleeping-face imprint, saturated red glowing eyes, subtle warm parchment ground.
Constraints: preserve black sludge body, red eyes, soft-mud monster silhouette, crawling pressure, bed-contact threat, glossy pooled mud, and low squat shape. Keep it clearly the same enemy as the reference, not a redesign.
Avoid: cute pet styling, generic ghost shape, humanoid body, limbs with anatomy, gore, realistic horror anatomy, extra characters, phone/lamp/alarm motifs, cat features, text, logo, watermark, UI frame, heavy scene background.
```

## Expected Output

- One Batch 39 candidate folder under
  `design/development/asset_candidates/enemies/black_mud_nightmare/batch_39_black_mud_ai_refinement_candidate_2026-06-15`.
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

Black Mud Nightmare must preserve black sludge body, red eyes, soft-mud monster
silhouette, crawling pressure, bed-contact threat, glossy pooled mud, sleepy
face imprint, top drip, and low squat shape.

Reject cute pet styling, generic ghost shape, humanoid body, gore, realistic
horror anatomy, extra dream interruption objects, cat features, or palette
drift. Reject any Unity import before active-enemy screenshot review.

## Validation

```powershell
& 'C:\Users\PC\.cache\codex-runtimes\codex-primary-runtime\dependencies\python\python.exe' design/development/tools/build_black_mud_ai_refinement_candidate.py <generated_image_path>
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_black_mud_ai_refinement_candidate.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```

Unity validation remains pending until MCP/editor execution is available:

1. Refresh AssetDatabase only after a separate import batch exists.
2. Capture `07-active-enemy-black-mud.png`.
3. Compare runtime enemy scale, silhouette, warning readability, and bed-contact
   threat read against the Batch 39 review sheet and Batch 38 source reference
   sheet.
4. Check Unity Console, Sprite import settings, prefab references, and scene
   bindings before approving any runtime installation.
