# P0 Asset Batch 28 - Starter Cat Strict Reference Pack

Date: 2026-06-14

This is a reference-pack and prompt-readiness batch for systematic starter-cat asset production. It does not generate new runtime sprites and does not import any candidate into Unity.

## Scope

Prepare a one-cat-at-a-time production lane for Saiban, Nephthys, and Suzune. The colored three-view turnaround PNGs are the hard authority for color, silhouette, markings, costume, prop, and civilization-symbol decisions.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/03_characters/character_design.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- Saiban source: `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- Nephthys source: `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- Suzune source: `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`

## Required Project Sources

- `design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.md`
- `design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`
- `design/development/asset_review/p0_starter_cat_turnaround_review_2026-06-14.png`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatTurnaroundSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetProductionReadiness.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetReviewPacket.cs`

## Output Policy

- Candidate art must be written only under `design/development/asset_candidates/starter_cats/<cat_id>/<batch_slug>/`.
- Candidate review notes and sheets may be written under `design/development/asset_review`.
- Keep all candidate PNGs outside `Assets`.
- formal import remains blocked until active-cat Play Mode screenshot review passes.
- Never edit `Assets/TheCat/Art/Characters/Sprites` in this batch.
- Never edit the colored turnaround source PNGs or source-lock hashes.

## Cat-Specific Lock Rules

Saiban must preserve shield, sword, tabby face, cape, helm, silver-gray fur, red cape, silver armor, gold trim, and blue gem accents.

Nephthys must preserve hood, pyramid/obelisk prop, gold-brown tabby fur, deep navy cloak, sand-gold trim, blue gems, ankh/script marks, and compact non-human cat posture.

Suzune must preserve calico markings, shrine outfit, bells, wand/branch, vermilion and white fabric, blue talismans, and non-human cat posture.

## Review Rules

- Reject any candidate that matches only the front view while losing side/back identity anchors.
- Reject any candidate with human body proportions.
- Reject palette drift away from the locked colored turnaround.
- Reject missing props, costume pieces, or civilization symbols.
- Hold every candidate outside Unity until active-cat screenshots are captured and compared against the colored three-view source.

## Validation

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_starter_cat_candidate_pack.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```
