# P0 Asset Batch 35 - Suzune Transparent Cutout Candidate

Date: 2026-06-15

## Task Scope

Produce one deterministic transparent cutout candidate pack for Suzune from the Batch 34 AI refinement candidate. This is a candidate batch only. Do not invoke new image generation, do not import the result into Unity, and do not replace runtime sprites.

This is a one-cat-at-a-time Suzune task. Do not produce Saiban, Nephthys, or additional starter-cat bitmap candidates in this batch.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/03_characters/character_design.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`

## Project Files To Read

- `design/development/asset_review/p0_codex_to_unity_asset_pipeline_2026-06-14.md`
- `design/development/asset_candidates/starter_cats/suzune/batch_33_suzune_strict_turnaround_derivatives_2026-06-14/thecat_cat_suzune_batch33_strict_turnaround_review_sheet.png`
- `design/development/asset_candidates/starter_cats/suzune/batch_34_suzune_ai_refinement_candidate_2026-06-14/thecat_cat_suzune_batch34_ai_refinement_review_sheet.png`
- `design/development/tools/build_suzune_cutout_candidate.py`
- `design/development/tools/validate_suzune_cutout_candidate.ps1`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatTurnaroundSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetProductionReadiness.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetReviewPacket.cs`

## Expected Output

- One transparent 1024x1024 Suzune candidate PNG under `design/development/asset_candidates/starter_cats/suzune/batch_35_suzune_cutout_candidate_2026-06-15`.
- One transparent 512x512 preview PNG.
- One checkerboard review PNG.
- One alpha mask review PNG.
- One process note, manifest, review note, and review sheet.
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

Reject cutouts that clip ears, paws, tail, bell wand, hanging bells, talismans, flower ornament, robe sleeves, red ribbons, or calico markings. Reject obvious parchment halos, broken alpha, or any Unity import before active-cat screenshot review.

## Validation

```powershell
& 'C:\Users\PC\.cache\codex-runtimes\codex-primary-runtime\dependencies\python\python.exe' design/development/tools/build_suzune_cutout_candidate.py
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_suzune_cutout_candidate.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```
