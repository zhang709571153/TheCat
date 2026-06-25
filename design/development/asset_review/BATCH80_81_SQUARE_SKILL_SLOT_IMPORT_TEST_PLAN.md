# Batch 80/81 Square Skill Slot Import Test Plan

Project: `D:\Unity Workspace\TheCat`
Created: 2026-06-25
Status: `v002_light_preferred_for_import_test_planning`

## Purpose

Batch 80 recommended skill icons should not be formally imported into the
current horizontal `skill_ready_frame`. Batch 81 review selected square skill
slots as the next HUD test direction. This plan defines the narrow import-test
scope and the evidence required before any formal promotion.

## Import-Test Scope

Only these candidate families may be used for the import test:

| Family | Source | Scope |
| --- | --- | --- |
| Batch 80 recommended skill icons | `design/development/asset_candidates/ui/starter_skill_icon_motifs/batch_80_starter_skill_icon_motifs_2026-06-25/recommended/` | 18 recommended icons, preferably 128/64 px for HUD testing. |
| Batch 81 square skill slots v001 | `design/development/asset_candidates/ui/skill_slot_frames/batch_81_skill_slot_frame_candidates_2026-06-25/frames/` | `square_ready`, `square_cooldown`, `square_disabled`, `square_selected` only. |
| Batch 81 square skill slots v002_light | `design/development/asset_candidates/ui/skill_slot_frames/batch_81_skill_slot_frame_candidates_2026-06-25/frames/frames_square_v002_light_*` | Preferred square import-test candidate after independent review; still candidate-only. |

Do not import:

- Batch 80 rejected v1 cells.
- Batch 80 non-selected lightframe variants.
- Batch 81 round slot frames, except as backup/reference if square fails.
- Any starter-cat body, portrait, combat sprite replacement, recolor, crop, or
  framesheet.

## Required Test States

| State | Requirement |
| --- | --- |
| Ready | Icon centered inside square slot; no center ornament collision; icon reads at 64 px and 128 px. |
| Cooldown | Digits `1`, `12`, and `99` fit in the top-right cooldown badge or selected digit zone; icon remains identifiable. |
| Disabled | Disabled square slot plus icon remains distinguishable from cooldown state. |
| Selected | Selected square slot glow does not overpower bright skill icons. |

## Evidence Required Before Promotion

| Gate | Evidence |
| --- | --- |
| Unity import settings | Sprite/UI type, alpha transparency, filter/compression, pixels-per-unit checked. |
| Battle HUD screenshot | Ready/cooldown/disabled/selected states captured in actual Battle HUD. |
| Cooldown digits | `1`, `12`, `99` shown on at least Saiban sword, Nephthys sandstorm, Suzune team heal. |
| Console | Console clean after AssetDatabase refresh and Play Mode screenshot capture. |
| Binding | Prefab/catalog binding proof shows only selected variants are referenced. |
| Review | Visual review confirms square slot direction before formal import. |

## Current Decision

Batch 80/81 may proceed toward Unity `import_test_only` planning, but not
formal runtime promotion. Candidate-only cooldown digit mockups for `1`, `12`,
and `99` passed focused review as readable enough for import-test planning.
Batch 81 `v002_light` is the preferred lighter square-only option for the
Unity import-test plan after independent review integration.

Watch items for Unity screenshots:

- `99` is readable but tight in the top-right badge.
- Right-side badge ornament plus square-slot corner decoration can feel busy.
- Check Saiban `battle_flag_rally`, Nephthys `sandstorm_swirl`,
  Nephthys `sand_tornado_column`, Suzune `torii_gate_ward`, and Suzune
  `team_heal_ice_enchant` first.
- Use square frames only for the import test. Do not import round frames or
  non-selected Batch 80 variants.
- Compare v001 square vs `v002_light` square in real HUD only if `v002_light`
  fails badge legibility; otherwise start with v002_light.
- Check ready vs selected clarity on cyan-heavy icons.
