# Batch 90 Cat Room Unity Preflight

Batch 90 cat-room candidates are Unity-import preflight ready with runtime evidence 6/8; formal install remains blocked by scene/presenter binding, clean Console evidence, and explicit human approval.

## Decision

- Ready for Unity preflight: yes
- Formal install allowed: no
- Unity editor import validation ready: yes
- Runtime evidence: 6/8
- Candidate policy: `candidate-backed Unity preflight only`

## Candidate Imports

| component | variant | source candidate | Unity preflight import | size |
| --- | --- | --- | --- | --- |
| cat_room_stage_panel | cat_room_stage_panel_frame | `design/development/asset_candidates/ui/cat_room/batch_90_cat_room_preflight_2026-06-25/sprites/thecat_ui_cat_room_cat_room_stage_panel_frame_1180x640_candidate_v001.png` | `Assets/TheCat/Art/UI/CatRoom/thecat_ui_cat_room_cat_room_stage_panel_frame_1180x640_candidate_v001.png` | 1180x640 |
| cat_room_status_rail | cat_room_status_rail_frame | `design/development/asset_candidates/ui/cat_room/batch_90_cat_room_preflight_2026-06-25/sprites/thecat_ui_cat_room_cat_room_status_rail_frame_960x120_candidate_v001.png` | `Assets/TheCat/Art/UI/CatRoom/thecat_ui_cat_room_cat_room_status_rail_frame_960x120_candidate_v001.png` | 960x120 |
| cat_room_interaction_card | cat_room_interaction_card_slot | `design/development/asset_candidates/ui/cat_room/batch_90_cat_room_preflight_2026-06-25/sprites/thecat_ui_cat_room_cat_room_interaction_card_slot_440x180_candidate_v001.png` | `Assets/TheCat/Art/UI/CatRoom/thecat_ui_cat_room_cat_room_interaction_card_slot_440x180_candidate_v001.png` | 440x180 |
| cat_room_prop_hotspot | cat_room_prop_hotspot_frame | `design/development/asset_candidates/ui/cat_room/batch_90_cat_room_preflight_2026-06-25/sprites/thecat_ui_cat_room_cat_room_prop_hotspot_frame_256x256_candidate_v001.png` | `Assets/TheCat/Art/UI/CatRoom/thecat_ui_cat_room_cat_room_prop_hotspot_frame_256x256_candidate_v001.png` | 256x256 |
| cat_room_dream_entrance | cat_room_dream_entrance_button_frame | `design/development/asset_candidates/ui/cat_room/batch_90_cat_room_preflight_2026-06-25/sprites/thecat_ui_cat_room_cat_room_dream_entrance_button_frame_420x112_candidate_v001.png` | `Assets/TheCat/Art/UI/CatRoom/thecat_ui_cat_room_cat_room_dream_entrance_button_frame_420x112_candidate_v001.png` | 420x112 |
| cat_room_prompt_chip | cat_room_hover_disabled_prompt_chip | `design/development/asset_candidates/ui/cat_room/batch_90_cat_room_preflight_2026-06-25/sprites/thecat_ui_cat_room_cat_room_hover_disabled_prompt_chip_360x96_candidate_v001.png` | `Assets/TheCat/Art/UI/CatRoom/thecat_ui_cat_room_cat_room_hover_disabled_prompt_chip_360x96_candidate_v001.png` | 360x96 |

## Blocking Runtime Evidence

- Missing Unity evidence: `design/development/asset_review/batch_90_cat_room_unity_preflight/scene_binding_console_clean_report.md`.
- Missing Unity evidence: `design/development/asset_review/batch_90_cat_room_unity_preflight/human_review_approval.md`.

## Protected Runtime State
- Batch 90 imports are not included in `P0VisualAssetCatalog.P0RuntimeVisualBindingCount`.
- Current P0 cat-room surface remains authoritative until scene/presenter binding, clean Console evidence, and human approval gates pass.
- Do not mark Batch 90 as formally installed before explicit review approval.
