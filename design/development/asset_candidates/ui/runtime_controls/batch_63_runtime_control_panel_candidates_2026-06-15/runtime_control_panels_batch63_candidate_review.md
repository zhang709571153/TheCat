# P0 Batch 63 - Runtime Control Panel Candidate Review

## Decision

- Candidate pack complete pending Unity review.
- Do not import into `Assets` until pause overlay scale, speed selector readability, restart confirmation affordance, keyboard hint readability, Console, and screenshot checks pass.
- Non-cat UI only; no starter-cat body, fur, costume, or colored-turnaround crop is included.

## Review Sheet

- `design/development/asset_candidates/ui/runtime_controls/batch_63_runtime_control_panel_candidates_2026-06-15/thecat_ui_runtime_control_panels_batch63_review_sheet.png`

## Candidate Rows

- `thecat_ui_runtime_pause_overlay_panel_768x432_candidate_v001` -> `design/development/asset_candidates/ui/runtime_controls/batch_63_runtime_control_panel_candidates_2026-06-15/thecat_ui_runtime_pause_overlay_panel_768x432_candidate_v001.png` binding hint `runtime_control.pause_overlay_panel`
- `thecat_ui_runtime_speed_segmented_control_512x128_candidate_v001` -> `design/development/asset_candidates/ui/runtime_controls/batch_63_runtime_control_panel_candidates_2026-06-15/thecat_ui_runtime_speed_segmented_control_512x128_candidate_v001.png` binding hint `runtime_control.speed_segmented_control`
- `thecat_ui_runtime_restart_confirm_plate_512x256_candidate_v001` -> `design/development/asset_candidates/ui/runtime_controls/batch_63_runtime_control_panel_candidates_2026-06-15/thecat_ui_runtime_restart_confirm_plate_512x256_candidate_v001.png` binding hint `runtime_control.restart_confirm_plate`
- `thecat_ui_runtime_keyboard_hint_strip_768x128_candidate_v001` -> `design/development/asset_candidates/ui/runtime_controls/batch_63_runtime_control_panel_candidates_2026-06-15/thecat_ui_runtime_keyboard_hint_strip_768x128_candidate_v001.png` binding hint `runtime_control.keyboard_hint_strip`

## Pending Unity Checks

- Pause overlay does not hide critical four-core values unintentionally.
- Speed segmented control is readable near 0.5x / 1x / 1.5x labels.
- Restart confirmation plate is visually distinct from normal pause controls.
- Keyboard hint strip remains readable at battle HUD scale.
- Console has no missing texture or IMGUI layout errors after any future install.
