# Batch 81 Skill Slot Frame Candidates Review

Status: `v002_light_preferred_import_test_candidate`

## Initial Local Read

| Item | State | Notes |
| --- | --- | --- |
| Candidate boundary | [x] | Batch is under `design/development/asset_candidates/...`; no Unity `.meta` should be present. |
| Frame split | [x] | Square and round ready/cooldown/disabled/selected frames exist at 256, 128, and 64 px. |
| Slot-fit board | [x] | `icon_fit_test/thecat_ui_skill_slot_frames_batch81_icon_fit_board_v001.png` compares square and round ready/cooldown states using Batch 80 recommended icons. |
| Visual direction | [x] | Square and round slots give the skill icon more center space than the existing horizontal frame. |
| Cooldown risk | [ ] | v001 cooldown frames still use a small top-right badge and radial wedge; verify with cooldown digits before import. |
| Chroma key risk | [ ] | First key pass produced noisy green background; final alpha pass is improved, but edge cleanup should be reviewed. |

## Independent Review Integration

| Review | Verdict | Action |
| --- | --- | --- |
| Visual review | Batch 81 can enter Unity import testing, but only square skill slot should be the main HUD direction. Round slot creates double-ring clutter with Batch 80 circular icons. | Mark square ready/cooldown/disabled/selected as `import_test_only`; keep round as backup/reference, not formal HUD direction. |
| Production QA | Candidate package organization, manifest, dimensions, icon-fit outputs, validator, and `.meta` boundary pass. Formal Unity import approval fails until real HUD screenshots, cooldown digits, import settings, Console, and binding proof pass. | Keep candidate-only; plan square-only import test with selected variants only. |

## Square Slot Import-Test Recommendation

| Slot | State | Notes |
| --- | --- | --- |
| `square_ready` | [x] import-test candidate | Gives Batch 80 recommended circular icons a clear button boundary. |
| `square_cooldown` | [x] import-test candidate | Radial cooldown direction is clear, but must be tested with digits `1`, `12`, and `99`. |
| `square_disabled` | [x] import-test candidate | Gray-blue desaturation reads as unavailable. |
| `square_selected` | [x] import-test candidate | Usable, but yellow outer glow may need lowering in real HUD. |
| `round_*` | [ ] backup only | Do not use as main HUD direction with Batch 80 recommended icons unless square fails. |

## Cooldown Digit Mockup

| Item | State | Notes |
| --- | --- | --- |
| Digit board | [x] | `cooldown_digit_test/thecat_ui_skill_slot_frames_batch81_square_cooldown_digit_board_v001.png` |
| Digits covered | [x] | `1`, `12`, and `99` generated across all 18 Batch 80 recommended icons. |
| Initial local read | [x] | Digits fit, but `99` is tight and badge ornament competes with attention. |
| Focused digit visual review | [x] | `1`, `12`, and `99` are generally readable; no systemic `99` failure. Watch Saiban battle flag, Nephthys sandstorm/tornado, Suzune torii, and Suzune team heal in real HUD. |
| Production QA | [x] | Candidate boundary, report paths, dimensions, validator coverage, and `.meta` absence pass. Not Unity import approval. |

## V002 Light Square Variant

| Item | State | Notes |
| --- | --- | --- |
| Generation | [x] | `source/thecat_ui_skill_slot_frames_batch81_chromakey_source_v002_square_light.png` |
| Alpha cutout | [x] | `source/thecat_ui_skill_slot_frames_batch81_alpha_sheet_v002_square_light.png`; selected glow remains wider than other states but does not fill the icon window. |
| Split frames | [x] | 12 square-only PNGs across 256/128/64 in `frames/frames_square_v002_light_*`. |
| Icon-fit board | [x] | `icon_fit_test_v002_light/thecat_ui_skill_slot_frames_batch81_icon_fit_board_v002_light.png` covers ready/cooldown/disabled/selected across all 18 Batch 80 recommended icons. |
| Cooldown digit board | [x] | `cooldown_digit_test_v002_light/thecat_ui_skill_slot_frames_batch81_square_cooldown_digit_board_v002_light.png` covers digits `1`, `12`, and `99`. |
| Actual-scale HUD mockup | [x] | `actual_scale_hud_test_v002_light/` covers 72 local 64 px slot composites and 6 desktop/mobile HUD boards using Batch 80 recommended icons. |
| Validator coverage | [x] | `validate_skill_slot_frame_candidates.ps1` now checks v001 plus v002_light counts, dimensions, reports, boards, and `.meta` absence. |
| Actual-scale mockup validator | [x] | `validate_batch81_v002_actual_scale_hud_mockups.ps1` checks counts, dimensions, CSV paths, board sizes, status boundaries, and `.meta` absence. |
| Independent review | [x] | Visual and production QA agents integrated. |

## V002 Light Independent Review Integration

| Review | Verdict | Action |
| --- | --- | --- |
| Visual review | Conditional pass for HUD import-test planning. Prefer `v002_light` over v001 for square-slot testing because ornament density is lower and Batch 80 icon fit remains coherent. | Use v002_light as the preferred square import-test candidate. Keep v001 as reference fallback for badge legibility. |
| Visual watch item | Ready and selected are too close, especially on cyan-heavy icons such as shield, moon/frost swirls, and ice talisman. `99` is readable at board scale but tight near the upper-right edge/corner node. | Unity test matrix must include ready vs selected plus cooldown `99` at 64 px or actual HUD scale. |
| Production QA | No blocking integrity failures. Counts, dimensions, manifest paths, reports, no `.meta`, and no Assets import-boundary violation pass. | Validator now locks v002 counts, dimensions, path boundary, unique IDs, source dimensions, and transparent borders. |
| Production watch item | v002 source width is `2007`, not divisible by 4, and 64 px padding is tight at 1-2 px. | Keep explicit source-dimension and transparent-border checks; avoid broad consumer globbing over the whole Batch 81 root. |

## Local Actual-Scale HUD Mockup Read

| Item | Verdict | Action |
| --- | --- | --- |
| Desktop focus board | Layout is readable after side-label/header revision. `ready`, `selected`, `cooldown_99`, and `disabled` are comparable at 64 px without annotation overlap. | Use as local preflight evidence only. |
| Mobile portrait board | Status gauges wrap cleanly and no longer collide with `PAUSE`; long skill labels are truncated to avoid covering slots. | Keep as a reviewer aid for 720x1280 readability. |
| Visual watch item | `selected` still reads close to `ready` on cyan-heavy icons; cooldown `99` is legible but tight near the upper-right node. | Unity evidence queue must still capture real HUD selected-vs-ready and cooldown `99`. |
| Boundary | The mockup is generated by local composition, not Unity. | Do not promote assets from this evidence alone. |
| Independent visual review | Candidate-only local preflight passes with no P0 visual blocker. `99` is readable but tight; highest-risk icons include Saiban battle flag, Nephthys sandstorm/tornado, and Suzune team heal. | Keep the current pack for reviewer alignment, then verify in real Battle HUD. |
| Independent QA review | 72 slot composites, 6 boards, CSV path boundaries, exact mockup root, Batch 80 recommended 64 px icon identities, board dimensions, status strings, and `.meta` absence validate. | Keep this as local integrity evidence only; Unity screenshots/import settings/Console/binding proof remain required. |

## Recommendation

Proceed to a square-only `import_test_only` plan. Do not formally promote Batch
81 until focused digit review, disabled/selected states, Unity Sprite/UI import
settings, Console, and prefab/catalog binding proof pass.

Current gate result: v002_light is the preferred square Unity
`import_test_only` candidate, not formal runtime promotion. v001 remains a
reference fallback for cooldown-badge legibility only.
