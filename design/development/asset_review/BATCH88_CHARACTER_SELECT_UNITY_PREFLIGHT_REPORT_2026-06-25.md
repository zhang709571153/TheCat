# Batch 88 Character Select Unity Preflight

Batch 88 character-select candidates are Unity-import preflight ready; formal install remains blocked until all runtime evidence, scene/presenter/Console, and approval gates pass.

## Decision

- Ready for Unity preflight: yes
- Formal install allowed: no
- Unity editor import validation ready: yes
- Runtime evidence: 6/8
- Candidate policy: `candidate-backed Unity preflight only`

## Candidate Imports

| component | variant | source candidate | Unity preflight import | size |
| --- | --- | --- | --- | --- |
| character_select_shell | character_select_stage_panel | `design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25/sprites/thecat_ui_character_select_character_select_stage_panel_1180x640_candidate_v001.png` | `Assets/TheCat/Art/UI/CharacterSelect/thecat_ui_character_select_character_select_stage_panel_1180x640_candidate_v001.png` | 1180x640 |
| character_select_card | starter_card_frame_selected | `design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25/sprites/thecat_ui_character_select_starter_card_frame_selected_360x500_candidate_v001.png` | `Assets/TheCat/Art/UI/CharacterSelect/thecat_ui_character_select_starter_card_frame_selected_360x500_candidate_v001.png` | 360x500 |
| character_select_card | starter_card_frame_idle | `design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25/sprites/thecat_ui_character_select_starter_card_frame_idle_360x500_candidate_v001.png` | `Assets/TheCat/Art/UI/CharacterSelect/thecat_ui_character_select_starter_card_frame_idle_360x500_candidate_v001.png` | 360x500 |
| character_select_chip | starter_role_chip_strip | `design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25/sprites/thecat_ui_character_select_starter_role_chip_strip_360x96_candidate_v001.png` | `Assets/TheCat/Art/UI/CharacterSelect/thecat_ui_character_select_starter_role_chip_strip_360x96_candidate_v001.png` | 360x96 |
| character_select_badge | starter_ready_badge | `design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25/sprites/thecat_ui_character_select_starter_ready_badge_220x80_candidate_v001.png` | `Assets/TheCat/Art/UI/CharacterSelect/thecat_ui_character_select_starter_ready_badge_220x80_candidate_v001.png` | 220x80 |
| character_select_button | starter_launch_button_frame | `design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25/sprites/thecat_ui_character_select_starter_launch_button_frame_420x112_candidate_v001.png` | `Assets/TheCat/Art/UI/CharacterSelect/thecat_ui_character_select_starter_launch_button_frame_420x112_candidate_v001.png` | 420x112 |

## Blocking Runtime Evidence

- Missing Unity evidence: `design/development/asset_review/batch_88_character_select_unity_preflight/scene_binding_console_clean_report.md`
- Missing Unity evidence: `design/development/asset_review/batch_88_character_select_unity_preflight/human_review_approval.md`

## Protected Runtime State
- Batch 88 imports are not included in `P0VisualAssetCatalog.P0RuntimeVisualBindingCount`.
- Current IMGUI main-menu character-select contract remains the playable runtime path until the remaining scene/presenter/Console and human approval gates pass.
- Do not mark Batch 88 as formally installed before explicit review approval.
