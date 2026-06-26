# Batch 86 Dream Route Unity Preflight

Batch 86 dream-route candidates are Unity-import preflight ready; runtime screenshots/reviews passed, formal install remains blocked by scene/Console and human approval gates.

## Decision

- Ready for Unity preflight: yes
- Formal install allowed: no
- Unity editor import validation ready: yes
- Runtime evidence: 6/8
- Candidate policy: `candidate-backed Unity preflight only`

## Candidate Imports

| component | variant | source candidate | Unity preflight import | size |
| --- | --- | --- | --- | --- |
| dream_route_panel | route_map_panel_frame | `design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25/sprites/thecat_ui_dream_route_route_map_panel_frame_1120x640_candidate_v001.png` | `Assets/TheCat/Art/UI/DreamRoute/thecat_ui_dream_route_route_map_panel_frame_1120x640_candidate_v001.png` | 1120x640 |
| dream_route_header | route_layer_header_frame | `design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25/sprites/thecat_ui_dream_route_route_layer_header_frame_760x96_candidate_v001.png` | `Assets/TheCat/Art/UI/DreamRoute/thecat_ui_dream_route_route_layer_header_frame_760x96_candidate_v001.png` | 760x96 |
| dream_route_node | route_node_socket_frame | `design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25/sprites/thecat_ui_dream_route_route_node_socket_frame_192x192_candidate_v001.png` | `Assets/TheCat/Art/UI/DreamRoute/thecat_ui_dream_route_route_node_socket_frame_192x192_candidate_v001.png` | 192x192 |
| dream_route_choice | route_choice_card_slot | `design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25/sprites/thecat_ui_dream_route_route_choice_card_slot_440x220_candidate_v001.png` | `Assets/TheCat/Art/UI/DreamRoute/thecat_ui_dream_route_route_choice_card_slot_440x220_candidate_v001.png` | 440x220 |
| dream_route_path | route_path_ribbon_frame | `design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25/sprites/thecat_ui_dream_route_route_path_ribbon_frame_640x96_candidate_v001.png` | `Assets/TheCat/Art/UI/DreamRoute/thecat_ui_dream_route_route_path_ribbon_frame_640x96_candidate_v001.png` | 640x96 |
| dream_route_boss | route_boss_gate_frame | `design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25/sprites/thecat_ui_dream_route_route_boss_gate_frame_360x260_candidate_v001.png` | `Assets/TheCat/Art/UI/DreamRoute/thecat_ui_dream_route_route_boss_gate_frame_360x260_candidate_v001.png` | 360x260 |

## Blocking Runtime Evidence

- Missing Unity evidence: `design/development/asset_review/batch_86_dream_route_unity_preflight/scene_binding_console_clean_report.md`.
- Missing Unity evidence: `design/development/asset_review/batch_86_dream_route_unity_preflight/human_review_approval.md`.

## Protected Runtime State
- Batch 86 imports are not included in `P0VisualAssetCatalog.P0RuntimeVisualBindingCount`.
- Current dream-entry and route-map presenters remain authoritative until screenshot, text replacement, node/path semantics, route-card density, boss gate scale, click targets, Console, and human approval gates pass.
- Screenshot evidence must be nonblank and confirmed by `runtime_evidence_report.md` with candidate draw count, no fallback, route semantics, text, scale, and click-target tokens.
- Scene/Console evidence must identify an existing Batch 86 runtime evidence log with the runtime pass token and no known failure/noise tokens.
- Do not mark Batch 86 as formally installed before explicit review approval.
