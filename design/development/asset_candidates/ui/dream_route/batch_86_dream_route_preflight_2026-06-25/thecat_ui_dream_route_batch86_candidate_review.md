# Batch 86 Dream Route Preflight Candidate Review

Result: local candidate packet generated; not Unity accepted.

## Scope

- Covers dream entry / route-map screen compositions and route-choice cards.
- Provides textless panel, header, node socket, choice card, path ribbon, and boss gate sprites for later Unity import testing.
- Reuses existing P0 UI shell, route node icons, route card frames, and Batch 65 route readability accents without altering them.
- Does not generate, crop, recolor, or import starter-cat body art.

## Candidate Rows

| Variant | Type | Size | Path |
| --- | --- | --- | --- |
| `route_map_panel_frame` | `sprite` | `1120x640` | `design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25/sprites/thecat_ui_dream_route_route_map_panel_frame_1120x640_candidate_v001.png` |
| `route_layer_header_frame` | `sprite` | `760x96` | `design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25/sprites/thecat_ui_dream_route_route_layer_header_frame_760x96_candidate_v001.png` |
| `route_node_socket_frame` | `sprite` | `192x192` | `design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25/sprites/thecat_ui_dream_route_route_node_socket_frame_192x192_candidate_v001.png` |
| `route_choice_card_slot` | `sprite` | `440x220` | `design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25/sprites/thecat_ui_dream_route_route_choice_card_slot_440x220_candidate_v001.png` |
| `route_path_ribbon_frame` | `sprite` | `640x96` | `design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25/sprites/thecat_ui_dream_route_route_path_ribbon_frame_640x96_candidate_v001.png` |
| `route_boss_gate_frame` | `sprite` | `360x260` | `design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25/sprites/thecat_ui_dream_route_route_boss_gate_frame_360x260_candidate_v001.png` |
| `dream_route_1920x1080` | `local_mockup` | `1920x1080` | `design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25/mockups/thecat_ui_dream_route_dream_route_1920x1080_local_mockup_v001.png` |
| `route_branch_1365x768` | `local_mockup` | `1365x768` | `design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25/mockups/thecat_ui_dream_route_route_branch_1365x768_local_mockup_v001.png` |
| `route_boss_pressure_1280x720` | `local_mockup` | `1280x720` | `design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25/mockups/thecat_ui_dream_route_route_boss_pressure_1280x720_local_mockup_v001.png` |
| `route_compact_1024x768` | `local_mockup` | `1024x768` | `design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25/mockups/thecat_ui_dream_route_route_compact_1024x768_local_mockup_v001.png` |

## Required Unity Gates

- Dream-entry and route-map screenshots at target resolutions.
- Unity-rendered Chinese title, route labels, node labels, card labels, and route rewards; no baked Chinese text in sprites.
- Current, selected, available, locked, and Boss-pressure route states must remain distinct at 1024x768.
- Route-choice cards must not occlude path connectors or node intent.
- Sprite import settings, scene/prefab binding proof, pointer/click target proof, and clean Console.
