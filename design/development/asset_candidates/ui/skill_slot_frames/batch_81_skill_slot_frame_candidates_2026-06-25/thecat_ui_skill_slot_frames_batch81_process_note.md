# Batch 81 Skill Slot Frame Candidates Process Note

Date: 2026-06-25
Lane: `skill_slot_frame_candidates`
Status: `v002_light_preferred_import_test_candidate`

## Scope

Batch 81 creates alternative square and round skill slot frames because Batch 80
HUD overlay testing showed that the existing horizontal
`thecat_ui_skill_ready_frame_512_v001.png` competes with round skill icons.

Output:

- 1 chroma-key source sheet.
- 1 alpha source sheet.
- 8 skill slot frame components.
- 3 sizes per frame: 256, 128, 64.
- 24 transparent frame PNG candidates.
- 18 recommended Batch 80 icons fitted into square/round ready/cooldown slots.
- 72 slot-fit composite QA PNGs.
- 54 square-slot cooldown digit mockups using digits `1`, `12`, and `99`.
- 1 v002_light square-only source sheet with lighter corner/badge ornamentation.
- 12 v002_light square-only frames across 256, 128, and 64 px.
- 72 v002_light square slot-fit QA PNGs across ready/cooldown/disabled/selected.
- 54 v002_light square cooldown digit mockups using digits `1`, `12`, and `99`.
- 72 v002_light actual-scale 64 px HUD slot composites using Batch 80 recommended icons.
- 6 local HUD mockup boards at 1920x1080, 1280x720, and 720x1280.
- Contact sheets, manifests, and a fit-test report.

## Source Truth

| Area | Authority |
| --- | --- |
| UI style | `Qr1XdXd6KosnjMxjgW7cS89kn9c`; local P0 UI style/asset inventory. |
| Icon content | Batch 80 recommended mixed candidate set. |
| Runtime status | Candidate-only. No Unity `.meta`, prefab binding, or import approval. |

## Generation Path

1. Used built-in `image_gen`; strict `gpt-image-2` CLI path remains blocked
   because `OPENAI_API_KEY` is not set in this shell.
2. Prompt requested square and round ready/cooldown/disabled/selected frames on
   green chroma background, with no embedded skill icons and no text.
3. Copied source image from Codex generated-images into this candidate folder.
4. Ran `remove_chroma_key.py` with border auto-key after the first strict key
   attempt left green-background noise.
5. Split frames with `design/development/tools/split_batch81_skill_slot_frames.py`.
6. Built icon-fit QA composites with
   `design/development/tools/build_batch81_skill_slot_icon_fit_board.py`.
7. Built candidate-only cooldown digit mockups with
   `design/development/tools/build_batch81_square_cooldown_digit_mockup.py`.
8. Generated a square-only `v002_light` source sheet after review found v001
   corner and top-right badge ornamentation visually busy in small HUD tests.
9. Removed chroma key with border auto-key, soft matte, edge contract, and
   despill; saved the alpha sheet under `source/`.
10. Split the v002_light source with
    `design/development/tools/split_batch81_square_light_v002_skill_slot_frames.py`.
11. Built v002_light icon-fit and cooldown digit QA boards with
    `design/development/tools/build_batch81_square_light_v002_qa_boards.py`.
12. Built local actual-scale HUD preflight boards with
    `design/development/tools/build_batch81_v002_actual_scale_hud_mockups.py`.
13. Validated the local mockup evidence with
    `design/development/tools/validate_batch81_v002_actual_scale_hud_mockups.ps1`.
14. Integrated independent visual and engineering QA review for the local
    actual-scale HUD mockup pack.

## Candidate Boundary

Batch 81 must remain under `design/development/asset_candidates/...` until a
focused visual review, real Battle HUD screenshot test, Unity import settings,
Console check, and prefab/catalog binding proof pass.

## Cooldown Digit Mockup

The candidate-only digit mockup is stored under:

`cooldown_digit_test/`

It covers every Batch 80 recommended skill icon with cooldown digits `1`, `12`,
and `99` in the Batch 81 square cooldown badge. This is not a Unity screenshot.

## V002 Light Square Variant

The lighter square-only variant is stored under:

- `source/thecat_ui_skill_slot_frames_batch81_chromakey_source_v002_square_light.png`
- `source/thecat_ui_skill_slot_frames_batch81_alpha_sheet_v002_square_light.png`
- `frames/frames_square_v002_light_256/`
- `frames/frames_square_v002_light_128/`
- `frames/frames_square_v002_light_64/`
- `icon_fit_test_v002_light/`
- `cooldown_digit_test_v002_light/`

It reduces top-right badge and corner ornament clutter enough to become the
preferred square Unity import-test candidate. It is still candidate-only and
requires real Battle HUD screenshots before any promotion. Watch ready vs
selected clarity and cooldown `99` at 64 px or actual HUD scale.

## Local Actual-Scale HUD Mockups

The local preflight mockup pack is stored under:

`actual_scale_hud_test_v002_light/`

It includes 18 Batch 80 recommended icons x 4 states as 64 px composites, plus
desktop and portrait HUD boards. This pack is useful for reviewer alignment but
does not replace Unity-rendered Battle HUD screenshots, Sprite import settings,
Console checks, or prefab/catalog binding proof.

Independent review found no P0 local-preflight blocker. Watch selected-vs-ready
clarity on cyan-heavy icons and cooldown `99` tightness on dense red/gold and
cyan/purple icons during the real Unity HUD capture.
