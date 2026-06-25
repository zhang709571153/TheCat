# Batch 88 Character Select Preflight Candidate Review

Result: local candidate packet generated; not Unity accepted.

## Scope

- Covers character-select screen composition, three starter cards, selected/idle card states, role chips, ready badge, and start button shell.
- Reuses existing main-menu background, dreamglass panel, title logo, source-locked HUD avatars, and symbolic starter skill icon motifs.
- Does not generate, crop, recolor, or import starter-cat body art or new character poses.
- Does not bake Chinese text into sprites; Unity-rendered names, roles, descriptions, and button labels remain required.

## Candidate Rows

| Variant | Type | Size | Path |
| --- | --- | --- | --- |
| `character_select_stage_panel` | `sprite` | `1180x640` | `design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25/sprites/thecat_ui_character_select_character_select_stage_panel_1180x640_candidate_v001.png` |
| `starter_card_frame_selected` | `sprite` | `360x500` | `design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25/sprites/thecat_ui_character_select_starter_card_frame_selected_360x500_candidate_v001.png` |
| `starter_card_frame_idle` | `sprite` | `360x500` | `design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25/sprites/thecat_ui_character_select_starter_card_frame_idle_360x500_candidate_v001.png` |
| `starter_role_chip_strip` | `sprite` | `360x96` | `design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25/sprites/thecat_ui_character_select_starter_role_chip_strip_360x96_candidate_v001.png` |
| `starter_ready_badge` | `sprite` | `220x80` | `design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25/sprites/thecat_ui_character_select_starter_ready_badge_220x80_candidate_v001.png` |
| `starter_launch_button_frame` | `sprite` | `420x112` | `design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25/sprites/thecat_ui_character_select_starter_launch_button_frame_420x112_candidate_v001.png` |
| `character_select_1920x1080` | `local_mockup` | `1920x1080` | `design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25/mockups/thecat_ui_character_select_character_select_1920x1080_local_mockup_v001.png` |
| `character_select_1365x768` | `local_mockup` | `1365x768` | `design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25/mockups/thecat_ui_character_select_character_select_1365x768_local_mockup_v001.png` |
| `character_select_1280x720` | `local_mockup` | `1280x720` | `design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25/mockups/thecat_ui_character_select_character_select_1280x720_local_mockup_v001.png` |
| `character_select_1024x768` | `local_mockup` | `1024x768` | `design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25/mockups/thecat_ui_character_select_character_select_1024x768_local_mockup_v001.png` |

## Required Unity Gates

- Character-select screenshots at 1920x1080, 1365x768, 1280x720, and 1024x768.
- Unity-rendered Chinese names, roles, descriptions, start labels, and selection state labels.
- Source-locked HUD avatars remain consistent with the starter-cat colored turnarounds.
- Selected/idle cards, ready badge, skill motif icons, and start button remain readable at 1024x768.
- Click-target proof for three starter cards and start action.
- Sprite import settings, scene/menu binding proof, and clean Console.
