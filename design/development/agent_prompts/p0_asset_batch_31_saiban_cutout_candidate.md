# P0 Asset Batch 31 - Saiban Transparent Cutout Candidate

Date: 2026-06-14

## Task Scope

Produce a deterministic transparent cutout candidate pack from the approved-for-review Batch 30 Saiban front-view image candidate. This is a candidate batch only. Do not import the result into Unity and do not replace runtime sprites.

This is a one-cat-at-a-time Saiban task. Do not produce Nephthys or Suzune bitmap candidates in this batch.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/03_characters/character_design.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`

## Project Files To Read

- `design/development/asset_review/p0_codex_to_unity_asset_pipeline_2026-06-14.md`
- `design/development/asset_candidates/starter_cats/saiban/batch_29_strict_turnaround_derivatives_2026-06-14/thecat_cat_saiban_batch29_strict_turnaround_review_sheet.png`
- `design/development/asset_candidates/starter_cats/saiban/batch_30_ai_refinement_candidate_2026-06-14/thecat_cat_saiban_ai_refinement_combat_1024_candidate_v001.png`
- `design/development/asset_candidates/starter_cats/saiban/batch_30_ai_refinement_candidate_2026-06-14/saiban_batch30_ai_refinement_candidate_review.md`
- `design/development/tools/build_saiban_cutout_candidate.py`
- `design/development/tools/validate_saiban_cutout_candidate.ps1`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatTurnaroundSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetReviewPacket.cs`

## Expected Output

- One transparent cutout PNG under `design/development/asset_candidates/starter_cats/saiban/batch_31_cutout_candidate_2026-06-14`.
- One 512x512 transparent preview PNG.
- One checkerboard review composite.
- One alpha mask review PNG.
- One manifest, review note, process note, and review sheet.
- All outputs stay outside `Assets`.
- formal import remains blocked until active-cat Play Mode screenshot review passes.

## Output Policy

- Candidate files must stay under `design/development/asset_candidates/starter_cats/saiban`.
- Keep all candidate PNGs outside `Assets`.
- Do not create Unity `.meta` files for candidate outputs.
- Do not modify `Assets/TheCat/Art/Characters/Sprites`.
- Do not modify colored turnaround PNGs, source-lock hashes, prefabs, scenes, runtime visual bindings, or formal import state.

## Saiban Lock Rules

Saiban must preserve shield, sword, tabby face, cape, helm/crown detail, silver-gray fur, red cape, silver armor, gold trim, blue gems, compact non-human cat posture, pale green eyes, and no human body proportions.

Transparent cutout work must not repaint, redesign, or restyle the Batch 30 candidate. It may only remove the parchment background and prepare alpha evidence for Unity sprite review.

## Acceptance Criteria

- The alpha PNG has transparent corners and an opaque visible Saiban center.
- The review sheet compares the colored turnaround authority, Batch 30 input candidate, checkerboard cutout, dark-field cutout, alpha mask, and preview.
- The review note states candidate review only, do not import into Unity yet, and formal import remains blocked.
- The manifest points to the Saiban colored turnaround source lock and Batch 30 input candidate.
- The batch remains compatible with `P0StarterCatProductionPromptReadiness`.

## Validation

```powershell
& 'C:\Users\PC\.cache\codex-runtimes\codex-primary-runtime\dependencies\python\python.exe' design/development/tools/build_saiban_cutout_candidate.py
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_saiban_cutout_candidate.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```
