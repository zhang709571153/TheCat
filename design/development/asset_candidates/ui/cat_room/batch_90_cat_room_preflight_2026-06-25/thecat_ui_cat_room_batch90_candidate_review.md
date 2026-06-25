# Batch 90 Cat Room Preflight Candidate Review

Result: local candidate packet generated; not Unity accepted.

## Scope

- Covers cat room screen composition, status rail, prop hotspots, interaction card, hover/disabled chip, and dream entrance button.
- Reuses existing BedroomDream background and props, Batch 67 interaction affordances, Qr1-style UI shell, and core status icons.
- Does not generate, crop, recolor, or import starter-cat body art or new cat room character poses.
- Does not bake Chinese text into sprites; Unity-rendered interaction labels, status values, and dream entrance labels remain required.

## Candidate Rows

| Variant | Type | Size | Path |
| --- | --- | --- | --- |
| `cat_room_stage_panel_frame` | `sprite` | `1180x640` | `design/development/asset_candidates/ui/cat_room/batch_90_cat_room_preflight_2026-06-25/sprites/thecat_ui_cat_room_cat_room_stage_panel_frame_1180x640_candidate_v001.png` |
| `cat_room_status_rail_frame` | `sprite` | `960x120` | `design/development/asset_candidates/ui/cat_room/batch_90_cat_room_preflight_2026-06-25/sprites/thecat_ui_cat_room_cat_room_status_rail_frame_960x120_candidate_v001.png` |
| `cat_room_interaction_card_slot` | `sprite` | `440x180` | `design/development/asset_candidates/ui/cat_room/batch_90_cat_room_preflight_2026-06-25/sprites/thecat_ui_cat_room_cat_room_interaction_card_slot_440x180_candidate_v001.png` |
| `cat_room_prop_hotspot_frame` | `sprite` | `256x256` | `design/development/asset_candidates/ui/cat_room/batch_90_cat_room_preflight_2026-06-25/sprites/thecat_ui_cat_room_cat_room_prop_hotspot_frame_256x256_candidate_v001.png` |
| `cat_room_dream_entrance_button_frame` | `sprite` | `420x112` | `design/development/asset_candidates/ui/cat_room/batch_90_cat_room_preflight_2026-06-25/sprites/thecat_ui_cat_room_cat_room_dream_entrance_button_frame_420x112_candidate_v001.png` |
| `cat_room_hover_disabled_prompt_chip` | `sprite` | `360x96` | `design/development/asset_candidates/ui/cat_room/batch_90_cat_room_preflight_2026-06-25/sprites/thecat_ui_cat_room_cat_room_hover_disabled_prompt_chip_360x96_candidate_v001.png` |
| `cat_room_1920x1080` | `local_mockup` | `1920x1080` | `design/development/asset_candidates/ui/cat_room/batch_90_cat_room_preflight_2026-06-25/mockups/thecat_ui_cat_room_cat_room_1920x1080_local_mockup_v001.png` |
| `cat_room_bed_hover_1365x768` | `local_mockup` | `1365x768` | `design/development/asset_candidates/ui/cat_room/batch_90_cat_room_preflight_2026-06-25/mockups/thecat_ui_cat_room_cat_room_bed_hover_1365x768_local_mockup_v001.png` |
| `cat_room_interaction_disabled_1280x720` | `local_mockup` | `1280x720` | `design/development/asset_candidates/ui/cat_room/batch_90_cat_room_preflight_2026-06-25/mockups/thecat_ui_cat_room_cat_room_interaction_disabled_1280x720_local_mockup_v001.png` |
| `cat_room_compact_1024x768` | `local_mockup` | `1024x768` | `design/development/asset_candidates/ui/cat_room/batch_90_cat_room_preflight_2026-06-25/mockups/thecat_ui_cat_room_cat_room_compact_1024x768_local_mockup_v001.png` |

## Required Unity Gates

- Cat room screenshots at 1920x1080, 1365x768, 1280x720, and 1024x768.
- Unity-rendered Chinese interaction text, status values, prompts, and dream entrance labels.
- Bed, feeder, litter, dream entrance, hover, disabled, blocked, and range states must be distinct.
- Prop scale must remain consistent with BedroomDream and Batch 54/67 bedroom source references.
- Click-target proof for bed, feeder, litter, dream entrance, and close/back affordances.
- Sprite import settings, screen binding proof, and clean Console.
