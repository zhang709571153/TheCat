# P0 Asset Batch 26 Starter Cat Candidate Gate Agent Prompt

## Task Scope

Review starter-cat candidates before any Unity import. This is a gate batch. It can approve continued candidate iteration or reject candidates, but it cannot approve formal Unity import.

Every review must compare the candidate against the colored three-view turnaround for Saiban, Nephthys, and Suzune. The generated lineup is not an authority source.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/03_characters/character_design.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`

## Project Files To Read

- `design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.md`
- `design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`
- `design/development/asset_review/p0_starter_cat_candidate_gate_2026-06-14.md`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatTurnaroundSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetReviewPacket.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetProductionReadiness.cs`

## Review Requirements

- Confirm candidate files are under `design/development/asset_candidates/starter_cats`.
- Confirm candidate files remain outside `Assets`.
- For each cat, score front, side, back, palette, markings, costume, prop, and civilization-symbol consistency.
- Call out every mismatch as blocking when it would change player-facing identity.
- formal import remains blocked until active-cat Play Mode screenshot review passes.

## Prohibited Changes

- Do not generate new images in this gate.
- Do not move candidate files into Unity.
- Do not modify runtime sprites, prefabs, source turnarounds, or source-lock hashes.
- Do not weaken formal import blocked state.

## Validation

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_starter_cat_candidate_pack.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```
