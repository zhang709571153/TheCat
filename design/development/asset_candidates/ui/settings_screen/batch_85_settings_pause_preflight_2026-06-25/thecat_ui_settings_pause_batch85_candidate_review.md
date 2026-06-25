# Batch 85 Settings / Pause Preflight Candidate Review

Result: local candidate packet generated; not Unity accepted.

## Scope

- Covers settings main, audio settings, pause overlay, and compact settings local screen compositions.
- Provides textless panel, tab bar, option row, modal, key hint, and divider sprites for later Unity import testing.
- Reuses existing P0 UI shell, Batch 78 settings controls, and Batch 79 system icons without altering them.
- Does not generate, crop, recolor, or import starter-cat body art.

## Candidate Rows

| Variant | Type | Size | Path |
| --- | --- | --- | --- |
| `screen_panel_frame` | `sprite` | `960x640` | `design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25/sprites/thecat_ui_settings_pause_screen_panel_frame_960x640_candidate_v001.png` |
| `tab_bar_frame` | `sprite` | `760x80` | `design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25/sprites/thecat_ui_settings_pause_tab_bar_frame_760x80_candidate_v001.png` |
| `option_row_frame` | `sprite` | `840x96` | `design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25/sprites/thecat_ui_settings_pause_option_row_frame_840x96_candidate_v001.png` |
| `confirm_modal_frame` | `sprite` | `720x420` | `design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25/sprites/thecat_ui_settings_pause_confirm_modal_frame_720x420_candidate_v001.png` |
| `key_hint_chip_frame` | `sprite` | `256x72` | `design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25/sprites/thecat_ui_settings_pause_key_hint_chip_frame_256x72_candidate_v001.png` |
| `settings_section_divider` | `sprite` | `640x24` | `design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25/sprites/thecat_ui_settings_pause_settings_section_divider_640x24_candidate_v001.png` |
| `settings_main_1920x1080` | `local_mockup` | `1920x1080` | `design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25/mockups/thecat_ui_settings_pause_settings_main_1920x1080_local_mockup_v001.png` |
| `settings_audio_1365x768` | `local_mockup` | `1365x768` | `design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25/mockups/thecat_ui_settings_pause_settings_audio_1365x768_local_mockup_v001.png` |
| `pause_overlay_1280x720` | `local_mockup` | `1280x720` | `design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25/mockups/thecat_ui_settings_pause_pause_overlay_1280x720_local_mockup_v001.png` |
| `settings_compact_1024x768` | `local_mockup` | `1024x768` | `design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25/mockups/thecat_ui_settings_pause_settings_compact_1024x768_local_mockup_v001.png` |

## Required Unity Gates

- Settings and pause screen screenshots at target resolutions.
- Unity-rendered Chinese title, option, button, and value text replacement proof; no baked Chinese text in sprites.
- Slider, switch, checkbox, tab, button, and close/back affordances must remain readable on 1024x768.
- Sprite import settings, scene/prefab binding proof, pointer/click target proof, and clean Console.
