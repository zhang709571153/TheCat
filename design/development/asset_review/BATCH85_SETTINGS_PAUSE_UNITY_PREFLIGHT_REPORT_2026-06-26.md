# Batch 85 Settings Pause Unity Preflight

Batch 85 settings/pause candidates are Unity-import preflight ready; runtime screenshots/reviews passed, formal install remains blocked by scene/Console and human approval gates.

## Decision

- Ready for Unity preflight: yes
- Formal install allowed: no
- Unity editor import validation ready: yes
- Runtime evidence: 6/8
- Candidate policy: `candidate-backed Unity preflight only`

## Candidate Imports

| component | variant | source candidate | Unity preflight import | size |
| --- | --- | --- | --- | --- |
| settings_pause_panel | screen_panel_frame | `design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25/sprites/thecat_ui_settings_pause_screen_panel_frame_960x640_candidate_v001.png` | `Assets/TheCat/Art/UI/SettingsPause/thecat_ui_settings_pause_screen_panel_frame_960x640_candidate_v001.png` | 960x640 |
| settings_pause_tabs | tab_bar_frame | `design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25/sprites/thecat_ui_settings_pause_tab_bar_frame_760x80_candidate_v001.png` | `Assets/TheCat/Art/UI/SettingsPause/thecat_ui_settings_pause_tab_bar_frame_760x80_candidate_v001.png` | 760x80 |
| settings_pause_option_row | option_row_frame | `design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25/sprites/thecat_ui_settings_pause_option_row_frame_840x96_candidate_v001.png` | `Assets/TheCat/Art/UI/SettingsPause/thecat_ui_settings_pause_option_row_frame_840x96_candidate_v001.png` | 840x96 |
| settings_pause_modal | confirm_modal_frame | `design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25/sprites/thecat_ui_settings_pause_confirm_modal_frame_720x420_candidate_v001.png` | `Assets/TheCat/Art/UI/SettingsPause/thecat_ui_settings_pause_confirm_modal_frame_720x420_candidate_v001.png` | 720x420 |
| settings_pause_hint | key_hint_chip_frame | `design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25/sprites/thecat_ui_settings_pause_key_hint_chip_frame_256x72_candidate_v001.png` | `Assets/TheCat/Art/UI/SettingsPause/thecat_ui_settings_pause_key_hint_chip_frame_256x72_candidate_v001.png` | 256x72 |
| settings_pause_divider | settings_section_divider | `design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25/sprites/thecat_ui_settings_pause_settings_section_divider_640x24_candidate_v001.png` | `Assets/TheCat/Art/UI/SettingsPause/thecat_ui_settings_pause_settings_section_divider_640x24_candidate_v001.png` | 640x24 |

## Blocking Runtime Evidence

- Missing Unity evidence: `design/development/asset_review/batch_85_settings_pause_unity_preflight/scene_binding_console_clean_report.md`.
- Missing Unity evidence: `design/development/asset_review/batch_85_settings_pause_unity_preflight/human_review_approval.md`.

## Protected Runtime State
- Batch 85 imports are not included in `P0VisualAssetCatalog.P0RuntimeVisualBindingCount`.
- Current pause/settings presenters remain authoritative until nonblank runtime screenshots, text replacement, key-hint semantics, click targets, Console, and human approval gates pass.
- Screenshot evidence must be confirmed by `design/development/asset_review/batch_85_settings_pause_unity_preflight/runtime_evidence_report.md`; scene/Console evidence must identify the matching clean runtime evidence log.
- Do not mark Batch 85 as formally installed before explicit review approval.
