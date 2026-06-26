# Batch 83 Loading Start Unity Preflight

Batch 83 loading/start candidates are Unity-import preflight ready; runtime screenshots/reviews passed, formal install remains blocked by scene/Console and human approval gates.

## Decision

- Ready for Unity preflight: yes
- Formal install allowed: no
- Unity editor import validation ready: yes
- Runtime evidence: 6/8
- Candidate policy: `candidate-backed Unity preflight only`

## Candidate Imports

| component | variant | source candidate | Unity preflight import | size |
| --- | --- | --- | --- | --- |
| loading_progress | progress_frame | `design/development/asset_candidates/ui/loading_start/batch_83_loading_start_preflight_2026-06-25/sprites/thecat_ui_loading_progress_frame_640x48_candidate_v001.png` | `Assets/TheCat/Art/UI/LoadingStart/thecat_ui_loading_progress_frame_640x48_candidate_v001.png` | 640x48 |
| loading_progress | progress_fill | `design/development/asset_candidates/ui/loading_start/batch_83_loading_start_preflight_2026-06-25/sprites/thecat_ui_loading_progress_fill_640x48_candidate_v001.png` | `Assets/TheCat/Art/UI/LoadingStart/thecat_ui_loading_progress_fill_640x48_candidate_v001.png` | 640x48 |
| loading_spinner | spinner_crescent | `design/development/asset_candidates/ui/loading_start/batch_83_loading_start_preflight_2026-06-25/sprites/thecat_ui_loading_spinner_crescent_128x128_candidate_v001.png` | `Assets/TheCat/Art/UI/LoadingStart/thecat_ui_loading_spinner_crescent_128x128_candidate_v001.png` | 128x128 |
| loading_pulse | dot_sequence | `design/development/asset_candidates/ui/loading_start/batch_83_loading_start_preflight_2026-06-25/sprites/thecat_ui_loading_dot_sequence_384x64_candidate_v001.png` | `Assets/TheCat/Art/UI/LoadingStart/thecat_ui_loading_dot_sequence_384x64_candidate_v001.png` | 384x64 |

## Blocking Runtime Evidence

- Missing Unity evidence: `design/development/asset_review/batch_83_loading_start_unity_preflight/scene_binding_console_clean_report.md`.
- Missing Unity evidence: `design/development/asset_review/batch_83_loading_start_unity_preflight/human_review_approval.md`.

## Protected Runtime State
- Batch 83 imports are not included in `P0VisualAssetCatalog.P0RuntimeVisualBindingCount`.
- Screenshot evidence must be visually nonblank and confirmed by the Batch 83 runtime evidence report.
- Scene/Console evidence must identify the matching Batch 83 runtime evidence log and that log must be clean.
- Current loading/start presenter and smoke hook remain authoritative until screenshot, spinner/state, Console, and human approval gates pass.
- Do not mark Batch 83 as formally installed before explicit review approval.
