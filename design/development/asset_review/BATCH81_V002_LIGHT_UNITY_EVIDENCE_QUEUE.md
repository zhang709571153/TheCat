# Batch 81 V002 Light Unity Evidence Queue

Project: `D:\Unity Workspace\TheCat`
Created: 2026-06-25 02:45 +08:00
Status: `pending_unity_mcp_or_editor_access`

This queue defines the next runtime evidence needed before Batch 81
`v002_light` square skill slot frames and Batch 80 recommended skill icons can
move beyond candidate-only status.

## Decision

| Item | Decision |
| --- | --- |
| Preferred import-test slot frame | Batch 81 `v002_light` square-only frames. |
| Fallback | Batch 81 v001 square frames only if `v002_light` fails badge legibility. |
| Icon set | Batch 80 recommended mixed set only. |
| Not allowed | Round Batch 81 frames, rejected Batch 80 cells, starter-cat body art, cat body sequence frames, body recolors, body crops, or runtime body replacement. |

## Required Assets

| Asset family | Candidate path | Required variants |
| --- | --- | --- |
| Skill icons | `design/development/asset_candidates/ui/starter_skill_icon_motifs/batch_80_starter_skill_icon_motifs_2026-06-25/recommended/` | 18 recommended icons, preferably 128 and 64 px for HUD testing. |
| Skill slot frames | `design/development/asset_candidates/ui/skill_slot_frames/batch_81_skill_slot_frame_candidates_2026-06-25/frames/frames_square_v002_light_*` | `ready`, `cooldown`, `disabled`, `selected` at 256, 128, and 64 px. |

## Screenshot Matrix

| Gate | Required proof | Watch items |
| --- | --- | --- |
| Ready vs selected | Same icon shown in ready and selected states in real Battle HUD at actual scale. | Selected currently reads too close to ready on cyan-heavy icons. |
| Cooldown digits | Cooldown `1`, `12`, and `99` in real Battle HUD or equivalent Unity-rendered HUD test. | `99` is tight near the upper-right badge and corner node. |
| Disabled | Disabled state shown with at least one Saiban, one Nephthys, and one Suzune icon. | Must remain distinct from cooldown. |
| Icon fit | All 18 Batch 80 recommended icons shown at actual HUD scale. | Confirm no crop, center collision, or frame-over-subject issue. |
| Console | Console clean after import, AssetDatabase refresh, Play Mode/HUD capture. | Missing sprite/import warnings block promotion. |
| Binding | Prefab/catalog proof references selected variants only. | No round frames, rejected icons, or non-selected lightframe variants. |

## Minimum Focus Set

If full 18-icon capture is too slow, capture this minimum first:

| Cat | Skill | Reason |
| --- | --- | --- |
| Saiban | `shield_barrier` | Cyan-heavy shield tests selected-vs-ready clarity. |
| Saiban | `battle_flag_rally` | Previously needed lightframe replacement; checks red/gold density. |
| Nephthys | `sandstorm_swirl` | Gold swirl tests icon/frame contrast. |
| Nephthys | `sand_tornado_column` | Dense gold motion checks cooldown digit overlap. |
| Suzune | `ice_talisman_guard` | Cyan icon tests selected-vs-ready clarity. |
| Suzune | `team_heal_ice_enchant` | Lightframe replacement and purple/cyan contrast watch item. |

## Local Actual-Scale Mockup Preflight

| Item | State | Evidence |
| --- | --- | --- |
| Local HUD mockup pack | [x] | `design/development/asset_candidates/ui/skill_slot_frames/batch_81_skill_slot_frame_candidates_2026-06-25/actual_scale_hud_test_v002_light/` |
| Slot composites | [x] | 18 Batch 80 recommended icons x 4 states = 72 transparent 64 px local composites. |
| Boards | [x] | Full-bar and focus-matrix boards at 1920x1080, 1280x720, and 720x1280; includes a 1920x1080 128 px focus board. |
| Validator | [x] | `design/development/tools/validate_batch81_v002_actual_scale_hud_mockups.ps1` passes. |
| Independent visual review | [x] | Local preflight passes without P0 blocker; `ready`, `selected`, `cooldown_99`, and `disabled` are generally distinguishable at 64 px. |
| Independent QA review | [x] | 72 slot composites, 6 boards, CSV paths, exact mockup-root boundary, Batch 80 recommended identity checks, and no `.meta`/`Assets` leakage pass. |
| Boundary | [x] | This is local visual preflight only. It does not replace Unity-rendered Battle HUD screenshots, import settings, Console checks, or prefab/catalog binding proof. |
| Watch items from preflight | [ ] | Selected vs ready remains close on cyan-heavy icons; cooldown `99` is visible but tight at 64 px, especially on dense red/gold and cyan/purple icons. |

## Promotion Rule

Do not promote Batch 80/81 into formal Unity runtime assets until every required
proof above exists. Local validators, manifests, and contact sheets prove
candidate integrity only.

## Fallback Lane

If Unity access returns but the skill HUD import-test cannot be run, execute
Batch 75 Chinese UI scale evidence instead:

- 5 UI surfaces x 4 resolutions.
- Chinese font and number readability.
- Cooldown/damage/reward/gauge label checks.
- Console notes with no font/material/import errors.
