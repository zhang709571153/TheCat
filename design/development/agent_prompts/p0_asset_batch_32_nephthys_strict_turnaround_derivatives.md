# P0 Asset Batch 32 - Nephthys Strict Turnaround Derivatives

Date: 2026-06-14

## Task Scope

Produce one source-derived Nephthys candidate pack in Codex, outside Unity. This batch creates reviewable bitmap assets by cropping and sampling the locked colored three-view turnaround. It does not use the existing Unity runtime sprite as the visual authority and does not import anything into `Assets`.

This is a one-cat-at-a-time Nephthys task. Do not produce Saiban, Suzune, or AI-generated bitmap candidates in this batch.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/03_characters/character_design.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`

## Project Files To Read

- `design/development/asset_review/p0_starter_cat_strict_reference_pack_2026-06-14.md`
- `design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`
- `design/development/tools/build_nephthys_strict_turnaround_derivatives.py`
- `design/development/tools/validate_nephthys_strict_turnaround_derivatives.ps1`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatTurnaroundSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetProductionReadiness.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetReviewPacket.cs`

## Expected Output

- Candidate PNGs under `design/development/asset_candidates/starter_cats/nephthys/batch_32_nephthys_strict_turnaround_derivatives_2026-06-14`.
- Batch manifest under `design/development/asset_candidates/starter_cats/batch_32_nephthys_strict_turnaround_derivatives_2026-06-14`.
- Review note and review sheet beside the Nephthys candidate PNGs.
- All candidates remain outside `Assets`.
- formal import remains blocked until active-cat Play Mode screenshot review passes.

## Output Policy

- Candidate art must stay under `design/development/asset_candidates/starter_cats/nephthys`.
- Keep all candidate PNGs outside `Assets`.
- Do not create Unity `.meta` files for candidate outputs.
- Do not modify `Assets/TheCat/Art/Characters/Sprites`.
- Do not modify the colored turnaround source PNGs or source-lock hashes.
- Do not modify prefabs, scenes, runtime visual bindings, or formal import state.

## Nephthys Lock Rules

Nephthys must preserve gold-brown tabby face, large golden eyes, dark blue hood, gold script trim, crescent hood ornament, blue tear gem, floating pyramid over inverted obelisk prop, deep navy cloak, sand-gold trim, blue gems, cyan magic particles, ankh symbols, winged blue jewel, compact non-human cat posture, and striped tail.

Reject Cleopatra costume cliche, human body language, human robe posture, generic Egyptian fantasy drift, missing side/back anchors, or palette drift away from the colored three-view turnaround.

## Validation

```powershell
& 'C:\Users\PC\.cache\codex-runtimes\codex-primary-runtime\dependencies\python\python.exe' design/development/tools/build_nephthys_strict_turnaround_derivatives.py
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_nephthys_strict_turnaround_derivatives.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```
