# Batch 89 Skill Selection Preflight Candidate Review

Result: local candidate packet generated; not Unity accepted.

## Scope

- Covers skill-selection screen composition, selected/ready/disabled/locked skill cards, detail panel, cost/cooldown strip, and confirm button.
- Reuses existing Qr1-style UI shell plus Batch 80 symbolic skill icons, Batch 81 skill slots, Batch 82 common UI state rows, and Batch 79 lock/warning icons.
- Does not generate, crop, recolor, or import starter-cat body art or new character poses.
- Does not bake Chinese text into sprites; Unity-rendered skill names, descriptions, values, cost, cooldown, and confirm labels remain required.

## Candidate Rows

| Variant | Type | Size | Path |
| --- | --- | --- | --- |
| `skill_selection_panel_frame` | `sprite` | `1180x640` | `design/development/asset_candidates/ui/skill_selection/batch_89_skill_selection_preflight_2026-06-25/sprites/thecat_ui_skill_selection_skill_selection_panel_frame_1180x640_candidate_v001.png` |
| `skill_choice_card_selected` | `sprite` | `420x240` | `design/development/asset_candidates/ui/skill_selection/batch_89_skill_selection_preflight_2026-06-25/sprites/thecat_ui_skill_selection_skill_choice_card_selected_420x240_candidate_v001.png` |
| `skill_choice_card_ready` | `sprite` | `420x240` | `design/development/asset_candidates/ui/skill_selection/batch_89_skill_selection_preflight_2026-06-25/sprites/thecat_ui_skill_selection_skill_choice_card_ready_420x240_candidate_v001.png` |
| `skill_choice_card_disabled` | `sprite` | `420x240` | `design/development/asset_candidates/ui/skill_selection/batch_89_skill_selection_preflight_2026-06-25/sprites/thecat_ui_skill_selection_skill_choice_card_disabled_420x240_candidate_v001.png` |
| `skill_choice_card_locked` | `sprite` | `420x240` | `design/development/asset_candidates/ui/skill_selection/batch_89_skill_selection_preflight_2026-06-25/sprites/thecat_ui_skill_selection_skill_choice_card_locked_420x240_candidate_v001.png` |
| `skill_detail_panel_frame` | `sprite` | `760x320` | `design/development/asset_candidates/ui/skill_selection/batch_89_skill_selection_preflight_2026-06-25/sprites/thecat_ui_skill_selection_skill_detail_panel_frame_760x320_candidate_v001.png` |
| `skill_cost_cooldown_strip` | `sprite` | `420x96` | `design/development/asset_candidates/ui/skill_selection/batch_89_skill_selection_preflight_2026-06-25/sprites/thecat_ui_skill_selection_skill_cost_cooldown_strip_420x96_candidate_v001.png` |
| `skill_confirm_button_frame` | `sprite` | `420x112` | `design/development/asset_candidates/ui/skill_selection/batch_89_skill_selection_preflight_2026-06-25/sprites/thecat_ui_skill_selection_skill_confirm_button_frame_420x112_candidate_v001.png` |
| `skill_selection_1920x1080` | `local_mockup` | `1920x1080` | `design/development/asset_candidates/ui/skill_selection/batch_89_skill_selection_preflight_2026-06-25/mockups/thecat_ui_skill_selection_skill_selection_1920x1080_local_mockup_v001.png` |
| `skill_selection_1365x768` | `local_mockup` | `1365x768` | `design/development/asset_candidates/ui/skill_selection/batch_89_skill_selection_preflight_2026-06-25/mockups/thecat_ui_skill_selection_skill_selection_1365x768_local_mockup_v001.png` |
| `skill_selection_1280x720` | `local_mockup` | `1280x720` | `design/development/asset_candidates/ui/skill_selection/batch_89_skill_selection_preflight_2026-06-25/mockups/thecat_ui_skill_selection_skill_selection_1280x720_local_mockup_v001.png` |
| `skill_selection_1024x768` | `local_mockup` | `1024x768` | `design/development/asset_candidates/ui/skill_selection/batch_89_skill_selection_preflight_2026-06-25/mockups/thecat_ui_skill_selection_skill_selection_1024x768_local_mockup_v001.png` |

## Required Unity Gates

- Skill-selection screenshots at 1920x1080, 1365x768, 1280x720, and 1024x768.
- Unity-rendered Chinese skill names, descriptions, numerical values, cooldown digits, cost chips, and confirm labels.
- Selected/ready/disabled/locked states must remain distinct at low height.
- Cooldown, low-resource, and no-target semantics must not conflict with battle HUD skill states.
- Click-target proof for cards, detail panel controls, and confirm action.
- Sprite import settings, screen binding proof, and clean Console.
