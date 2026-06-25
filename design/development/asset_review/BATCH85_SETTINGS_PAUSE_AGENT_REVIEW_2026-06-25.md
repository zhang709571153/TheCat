# Batch 85 Settings / Pause Agent Review

Date: 2026-06-25

Batch directory:
`design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25`

## Result

Batch 85 is approved for candidate-only Unity validation queueing.
It is not approved for Unity import, prefab binding, or runtime acceptance.

## Independent Review Summary

| Reviewer | Verdict | Finding |
| --- | --- | --- |
| Visual/UI style | PASS_WITH_P2 | The settings/pause sprites and mockups match the dreamglass UI shell, Batch 78 settings controls, and Batch 79 system icons. The 1024x768 compact mockup has one watch item: the lower-left key hint chip sits close to the panel edge and could be mistaken for a clickable back affordance in Unity. |
| Production QA | PASS | Manifest, hashes, source assets, validator, matrix row, candidate containment, no `.meta`, and honest non-image2 provenance all passed. |

## Candidate Evidence

- Manifest: `thecat_ui_settings_pause_batch85_manifest.csv`
- Contact sheet: `thecat_ui_settings_pause_batch85_contact_sheet_v001.png`
- Review sheet: `thecat_ui_settings_pause_batch85_review_sheet_v001.png`
- Candidate review: `thecat_ui_settings_pause_batch85_candidate_review.md`
- Process note: `thecat_ui_settings_pause_batch85_process_note.md`
- Validator: `design/development/tools/validate_ui_settings_pause_preflight_candidates.ps1`
- Matrix row: `ui_settings_pause_preflight` in `P0_NONCAT_CANDIDATE_VALIDATION_MATRIX_2026-06-25.md`

## Accepted Candidate Scope

- 6 transparent textless settings/pause sprites.
- 4 local settings/pause mockups across 1920x1080, 1365x768, 1280x720, and 1024x768 compact.
- Sources are existing UI shell assets plus Batch 78 settings controls and Batch 79 system icons.
- No starter-cat body art, character body crop, outfit recolor, or runtime replacement is included.
- `source_model` is intentionally `deterministic_local_derivative_from_batch78_imagegen_not_image2`; strict image2 CLI generation remains blocked because `OPENAI_API_KEY` is not set in this shell.

## Remaining Unity Gates

- Capture settings and pause screenshots at target resolutions, including 1024x768 compact.
- Replace placeholder text with Unity-rendered Chinese UI text, shortcut text, option names, and values.
- Verify slider, switch, checkbox, tab, close, back, and key hint click-target semantics and z-order.
- Specifically check the lower-left key hint chip in compact layout; if it reads as a clickable back button, reposition it or reduce its clickable affordance before formal acceptance.
- Verify Sprite import settings, prefab/scene binding proof, and a clean Console.

## Queue Decision

Add Batch 85 to the P0 asset production queue as
`p0_asset_queue_settings_pause_preflight_candidates` with state
`CandidatePackCompletePendingUnityReview`.
