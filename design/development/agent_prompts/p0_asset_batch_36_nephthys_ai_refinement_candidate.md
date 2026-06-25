# P0 Asset Batch 36 - Nephthys AI Refinement Candidate

Date: 2026-06-15

## Task Scope

Use Codex built-in image generation to produce exactly one Nephthys front-view combat sprite refinement candidate. This is a candidate batch only. Do not import the result into Unity and do not replace runtime sprites.

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
- `design/development/tools/build_nephthys_ai_refinement_candidate.py`
- `design/development/tools/validate_nephthys_ai_refinement_candidate.ps1`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatTurnaroundSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetProductionReadiness.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetReviewPacket.cs`

## Expected Output

- One raw Codex-generated Nephthys candidate copy under `design/development/asset_candidates/starter_cats/nephthys/batch_36_nephthys_ai_refinement_candidate_2026-06-15`.
- One standardized 1024x1024 candidate PNG.
- One 512x512 preview PNG.
- One prompt record, manifest, review note, and review sheet.
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

Reject generic Egyptian fantasy drift, Cleopatra costume cliche, human Egyptian priest proportions, long human legs, human hands, human robe posture, missing hood, missing crescent, missing blue gem, missing floating pyramid prop, missing ankh, missing striped tail, or palette drift away from the colored three-view turnaround.

## Built-In Image Generation Prompt

Use case: stylized-concept
Asset type: game character combat sprite candidate for TheCat P0 starter cat Nephthys
Primary request: Create one polished full-body front-facing combat sprite candidate for Nephthys, the non-human gold-brown tabby moon-sand agent cat shown in the Batch 32 reference sheet. The candidate must preserve the reference identity strictly.
Input images: the visible Nephthys colored three-view turnaround is the hard visual authority; the visible Batch 32 review sheet is the production reference.
Subject: compact non-human cat body, large cat head, upright ears, golden eyes, gold-brown tabby face stripes, small cat muzzle, short paws and compact legs, no human face, no human proportions.
Costume and props: deep navy hood and cloak, crescent moon hood ornament with blue tear gem, sand-gold script border, blue gemstone chest ornament, winged gold collar, ankh emblem, layered navy cloak panels, wrapped cream trousers, blue-gold bracelets, striped gold-brown tail, floating pyramid/obelisk prop with eye markings and cyan magic particles.
Constraints: preserve hood, gold script trim, crescent ornament, blue tear gem, golden eyes, tabby face, blue gemstone chest ornament, winged gold collar, ankh emblem, floating pyramid prop, striped tail, compact non-human cat posture, and moon-sand controller palette.
Avoid: human Egyptian priest body, Cleopatra costume cliche, long legs, human torso, human hands, generic Egyptian fantasy cat, missing hood, missing crescent, missing blue gem, missing floating pyramid prop, missing ankh, missing striped tail, palette drift, extra weapons, text, logo, watermark, UI frame.

## Validation

```powershell
& 'C:\Users\PC\.cache\codex-runtimes\codex-primary-runtime\dependencies\python\python.exe' design/development/tools/build_nephthys_ai_refinement_candidate.py <generated_image_path>
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_nephthys_ai_refinement_candidate.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```
