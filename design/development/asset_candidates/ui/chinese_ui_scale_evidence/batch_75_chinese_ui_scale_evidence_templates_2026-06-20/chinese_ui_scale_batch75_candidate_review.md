# P0 Batch 75 - Chinese UI Scale Evidence Template Review

## Decision

- Validation template only pending Unity review.
- Do not import into `Assets`; these files are evidence templates, not runtime UI art.
- Non-cat UI validation only; no starter-cat body, fur, costume, prop, or colored-turnaround crop is included.
- Unity MCP is currently required for final screenshots, but manual Unity Editor capture may fill the matrix while MCP approval is revoked.

## Review Sheet

- `design/development/asset_candidates/ui/chinese_ui_scale_evidence/batch_75_chinese_ui_scale_evidence_templates_2026-06-20/thecat_ui_chinese_scale_batch75_review_sheet.png`

## Capture Matrix

- `design/development/asset_candidates/ui/chinese_ui_scale_evidence/batch_75_chinese_ui_scale_evidence_templates_2026-06-20/chinese_ui_scale_batch75_capture_matrix.csv`
- Required resolutions: `1024x768`, `1280x720`, `1600x900`, `1920x1080`.
- Required surfaces: main menu / character select, route map, battle HUD, skill/enemy HUD, result / pause settings.

## Template Rows

- `thecat_ui_chinese_scale_capture_matrix_batch75_1920x1080_v001` -> `design/development/asset_candidates/ui/chinese_ui_scale_evidence/batch_75_chinese_ui_scale_evidence_templates_2026-06-20/thecat_ui_chinese_scale_capture_matrix_batch75_1920x1080_v001.png` use `validation.capture_matrix`
- `thecat_ui_chinese_scale_safe_area_overlay_batch75_1920x1080_v001` -> `design/development/asset_candidates/ui/chinese_ui_scale_evidence/batch_75_chinese_ui_scale_evidence_templates_2026-06-20/thecat_ui_chinese_scale_safe_area_overlay_batch75_1920x1080_v001.png` use `validation.safe_area_overlay`
- `thecat_ui_chinese_scale_surface_note_card_batch75_1280x720_v001` -> `design/development/asset_candidates/ui/chinese_ui_scale_evidence/batch_75_chinese_ui_scale_evidence_templates_2026-06-20/thecat_ui_chinese_scale_surface_note_card_batch75_1280x720_v001.png` use `validation.surface_note_card`
- `thecat_ui_chinese_scale_resolution_strip_batch75_1600x320_v001` -> `design/development/asset_candidates/ui/chinese_ui_scale_evidence/batch_75_chinese_ui_scale_evidence_templates_2026-06-20/thecat_ui_chinese_scale_resolution_strip_batch75_1600x320_v001.png` use `validation.resolution_strip`

## Pending Unity Checks

- Capture all 20 surface/resolution screenshots.
- Verify Chinese-facing text, except necessary HP/shortcut/technical tokens.
- Verify no overlap, no clipping, scrollable long panels, narrow stacking, and readable HUD labels.
- Record Console status for every reviewed surface.
- Keep this batch outside runtime catalogs unless it is explicitly repurposed as tooling art.

## Local Preflight Addendum

- `local_scale_mockups_v001/` now contains 20 deterministic local mockups covering the same 5 surfaces x 4 resolutions.
- The local mockup validator passes and checks manifest hashes, exact sizes, `resolution_id`, file-set equality, path containment, no `.meta`, and no `Assets` leakage.
- Independent visual and engineering QA found no local P0 blocker.
- This preflight does not replace Unity screenshots, Canvas Scaler proof, font fallback checks, prefab binding, or Console notes.
