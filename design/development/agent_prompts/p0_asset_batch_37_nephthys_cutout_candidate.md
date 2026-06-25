# P0 Asset Batch 37 - Nephthys Transparent Cutout Candidate

Date: 2026-06-15

## Task Scope

Create a deterministic transparent cutout candidate from the Batch 36 Nephthys AI refinement image. This is a local post-processing candidate batch only. Do not import the result into Unity and do not replace runtime sprites.

This is a one-cat-at-a-time Nephthys task. Do not produce Saiban, Suzune, or additional starter-cat bitmap candidates in this batch.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/03_characters/character_design.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`

## Project Files To Read

- `design/development/asset_review/p0_codex_to_unity_asset_pipeline_2026-06-14.md`
- `design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`
- `design/development/asset_candidates/starter_cats/nephthys/batch_32_nephthys_strict_turnaround_derivatives_2026-06-14/thecat_cat_nephthys_batch32_strict_turnaround_review_sheet.png`
- `design/development/asset_candidates/starter_cats/nephthys/batch_36_nephthys_ai_refinement_candidate_2026-06-15/thecat_cat_nephthys_batch36_ai_refinement_review_sheet.png`
- `design/development/tools/build_nephthys_cutout_candidate.py`
- `design/development/tools/validate_nephthys_cutout_candidate.ps1`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatTurnaroundSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetProductionReadiness.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetReviewPacket.cs`

## Expected Output

- One transparent 1024x1024 Nephthys cutout candidate under `design/development/asset_candidates/starter_cats/nephthys/batch_37_nephthys_cutout_candidate_2026-06-15`.
- One 512x512 transparent preview PNG.
- One 512x512 checkerboard review composite.
- One 512x512 alpha-mask review PNG.
- One manifest, review note, process note, and review sheet.
- All outputs stay outside `Assets`.
- formal import remains blocked until active-cat Play Mode screenshot review passes.

## Output Policy

- Candidate files must stay under `design/development/asset_candidates/starter_cats/nephthys`.
- Keep all candidate PNGs outside `Assets`.
- Do not create Unity `.meta` files for candidate outputs.
- Do not modify `Assets/TheCat/Art/Characters/Sprites`.
- Do not modify colored turnaround PNGs or source-lock hashes.
- Do not modify prefabs, scenes, runtime visual bindings, or formal import state.

## Nephthys Lock Rules

Nephthys must preserve gold-brown tabby face and body fur, dark tabby stripes, golden eyes, deep navy hood and cloak, crescent moon hood ornament, blue tear gem, sand-gold script border, winged gold collar, blue gemstone chest ornament, ankh emblem, layered cloak panels, wrapped cream trousers, blue-gold bracelets, striped gold-brown tail, floating pyramid/obelisk prop with eye markings, cyan magic particles, compact non-human cat posture, and moon-sand controller palette.

Reject clipping of ears, hood, crescent ornament, blue tear gem, face markings, floating pyramid prop, cyan particles, ankh, cloak panels, paws, or striped tail. Reject generic Egyptian fantasy drift, Cleopatra costume cliche, human Egyptian priest proportions, long human legs, human hands, or palette drift away from the colored three-view turnaround.

## Cutout Method

Use deterministic local post-processing only. Start from:

`design/development/asset_candidates/starter_cats/nephthys/batch_36_nephthys_ai_refinement_candidate_2026-06-15/thecat_cat_nephthys_ai_refinement_combat_1024_candidate_v001.png`

The builder should sample the image border, flood-fill connected parchment background pixels, convert the background to alpha, and generate checkerboard, dark-field, alpha-mask, and preview evidence.

## Validation

```powershell
& 'C:\Users\PC\.cache\codex-runtimes\codex-primary-runtime\dependencies\python\python.exe' design/development/tools/build_nephthys_cutout_candidate.py
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_nephthys_cutout_candidate.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```
