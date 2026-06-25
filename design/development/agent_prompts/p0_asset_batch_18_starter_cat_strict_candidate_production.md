# P0 Asset Batch 18 Starter Cat Strict Candidate Production Agent Prompt

## Task Scope

Produce starter-cat candidate art only after the conformance gate is green. Work one cat at a time. The colored three-view turnaround PNGs are the visual source of truth for silhouette, markings, costume, prop language, and palette.

This is candidate production only. Do not import candidates into Unity and do not replace runtime cat sprites.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/03_characters/character_design.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`

## Project Files To Read

- `design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.md`
- `design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`
- `design/development/asset_review/p0_starter_cat_turnaround_review_2026-06-14.png`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatTurnaroundSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetProductionReadiness.cs`

## Output Policy

- Write candidate PNGs only under `design/development/asset_candidates/starter_cats/<cat_id>/<batch_slug>/`.
- Keep all candidate art outside `Assets`.
- Write review notes under `design/development/asset_review`.
- Include explicit front, side, and back reference checks in review notes.
- formal import remains blocked until active-cat Play Mode screenshot review passes.

## Cat-Specific Locks

- Saiban: preserve shield, sword, tabby face, cape, helm, silver-gray fur, red cape, silver armor, gold trim, and blue gem accents.
- Nephthys: preserve hood, pyramid/obelisk prop, gold-brown tabby fur, deep navy cloak, sand-gold trim, blue gems, ankh/script marks, and compact non-human cat posture.
- Suzune: preserve calico markings, shrine outfit, bells, wand/branch, vermilion and white fabric, blue talismans, and non-human cat posture.

## Prohibited Changes

- Do not modify source turnaround PNGs or hashes.
- Do not modify `Assets/TheCat/Art/Characters/Sprites`.
- Do not produce a lineup as the primary deliverable.
- Do not approve formal import; formal import remains blocked pending active-cat screenshot review.
- Reject any human-proportion, palette-drift, missing-prop, or front-view-only candidate.

## Validation

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_starter_cat_candidate_pack.ps1
git diff --check
```
