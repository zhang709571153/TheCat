# P0 Asset Batch 34 - Suzune AI Refinement Candidate

Date: 2026-06-14

## Task Scope

Use Codex built-in image generation to produce exactly one Suzune front-view combat sprite refinement candidate. This is a candidate batch only. Do not import the result into Unity and do not replace runtime sprites.

This is a one-cat-at-a-time Suzune task. Do not produce Saiban, Nephthys, or additional starter-cat bitmap candidates in this batch.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/03_characters/character_design.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`

## Project Files To Read

- `design/development/asset_review/p0_codex_to_unity_asset_pipeline_2026-06-14.md`
- `design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`
- `design/development/asset_candidates/starter_cats/suzune/batch_33_suzune_strict_turnaround_derivatives_2026-06-14/thecat_cat_suzune_batch33_strict_turnaround_review_sheet.png`
- `design/development/tools/build_suzune_ai_refinement_candidate.py`
- `design/development/tools/validate_suzune_ai_refinement_candidate.ps1`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatTurnaroundSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetProductionReadiness.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetReviewPacket.cs`

## Expected Output

- One raw Codex-generated Suzune candidate copy under `design/development/asset_candidates/starter_cats/suzune/batch_34_suzune_ai_refinement_candidate_2026-06-14`.
- One standardized 1024x1024 candidate PNG.
- One 512x512 preview PNG.
- One prompt record, manifest, review note, and review sheet.
- All outputs stay outside `Assets`.
- formal import remains blocked until active-cat Play Mode screenshot review passes.

## Output Policy

- Candidate files must stay under `design/development/asset_candidates/starter_cats/suzune`.
- Keep all candidate PNGs outside `Assets`.
- Do not create Unity `.meta` files for candidate outputs.
- Do not modify `Assets/TheCat/Art/Characters/Sprites`.
- Do not modify colored turnaround PNGs or source-lock hashes.
- Do not modify prefabs, scenes, runtime visual bindings, or formal import state.

## Suzune Lock Rules

Suzune must preserve orange, black, and white calico face patches, blue eyes, white shrine robe, vermilion skirt/sash/bow/ribbons, central gold bell, red-white flower hair ornament with hanging bells, clustered kagura bell wand/branch, blue paper talismans, blue snowflake sleeve motifs, red stitch trim, moon-blue sleep effects, compact non-human cat posture, and calico tail.

Reject generic shrine-cat drift, human shrine maiden proportions, long human legs, human hands, human costume posture, missing bell wand, missing bells, missing calico patches, missing blue talismans, missing snowflake sleeve marks, or palette drift away from the colored three-view turnaround.

## Built-In Image Generation Prompt

Use case: stylized-concept
Asset type: game character combat sprite candidate for TheCat P0 starter cat Suzune
Primary request: Create one polished full-body front-facing combat sprite candidate for Suzune, the non-human calico cat sleep shrine healer shown in the Batch 33 reference sheet. The candidate must preserve the reference identity strictly.
Input images: the visible Suzune colored three-view turnaround is the hard visual authority; the visible Batch 33 review sheet is the production reference.
Subject: compact non-human cat body, large cat head, upright ears, blue eyes, orange/black/white calico face and body patches, small cat muzzle, no human face, no human proportions.
Costume and props: white shrine robe, vermilion red skirt and sash, central gold bell, red-white flower ornament, hanging bells, clustered kagura bell wand, red ribbons, pale moon-blue paper talismans, snowflake sleeve marks, visible calico tail.
Constraints: preserve calico markings, blue eyes, white shrine robe, vermilion skirt, sash, bow, ribbons, central gold bell, flower ornament, hanging bells, bell wand, blue talismans, snowflake sleeve marks, compact non-human cat posture, and healer palette.
Avoid: human shrine maiden body, long legs, human torso, human hands, generic shrine cat, generic healer costume, missing bell wand, missing bells, missing calico patches, missing flower ornament, missing snowflake sleeve marks, missing blue talismans, missing calico tail, palette drift, extra weapons, text, logo, watermark, UI frame.

## Validation

```powershell
& 'C:\Users\PC\.cache\codex-runtimes\codex-primary-runtime\dependencies\python\python.exe' design/development/tools/build_suzune_ai_refinement_candidate.py <generated_image_path>
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_suzune_ai_refinement_candidate.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```
