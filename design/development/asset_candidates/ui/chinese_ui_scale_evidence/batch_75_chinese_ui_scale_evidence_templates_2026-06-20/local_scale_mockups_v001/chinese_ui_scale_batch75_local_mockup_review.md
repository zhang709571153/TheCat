# P0 Batch 75 - Local Chinese UI Scale Mockup Review

Status: `local_preflight_not_unity_screenshot`

These 20 mockups render the required Batch 75 surface/resolution matrix with Chinese UI text, cooldown numbers, reward numbers, and narrow-screen layouts. They are local visual preflight only and do not replace Unity screenshots or Console notes.

## Outputs

- `mockups/`: 5 P0 UI surfaces x 4 required resolutions = 20 PNGs.
- `thecat_ui_chinese_scale_batch75_local_mockup_contact_sheet.png`: reviewer contact sheet.
- `chinese_ui_scale_batch75_local_mockup_manifest.csv`: hash and path manifest.

## Watch Items

- Compact 1024x768 layouts should be checked for wrapped Chinese text and control stacking.
- Battle and skill HUD numbers should stay readable beside cooldown `99`, HP, reward, and damage values.
- Local mockups use deterministic composition and cannot prove Unity Canvas scaling, font fallback, Console state, prefab binding, or runtime overlap.

## Independent Review Integration

| Review | Verdict | Action |
| --- | --- | --- |
| Visual review | 20 PNGs cover 5 surfaces x 4 resolutions. No local P0 visual blocker; 1024x768 Chinese titles, buttons, long notes, reward/HP/damage numbers, and cooldown `99` remain readable. | Carry P1 watch items into Unity screenshot matrix. |
| Visual watch items | Check all 1024x768 surfaces, battle HUD bottom skill cooldown `99`, two-row top gauges plus pause button, route lock button near nodes, character cards/long-note wrapping, skill/enemy HP alignment, and result/settings reward numbers/sliders. | Do not close Batch 75 until Unity screenshots and Console notes are captured. |
| Engineering QA | Manifest, 20 mockup PNGs, 5x4 coverage, sizes, hashes, status, exact path root, no `.meta`, and no `Assets` leakage pass. | Validator now also checks `resolution_id`, manifest/file set equality, and `mockups/` containment. |
| Boundary | Local preflight only. | Unity Canvas Scaler, font fallback, prefab binding, runtime overlap, and Console status remain unproven. |

## Matrix

| Surface | 1024x768 | 1280x720 | 1600x900 | 1920x1080 |
| --- | --- | --- | --- | --- |
| 角色选择 | `thecat_batch75_main_menu_character_select_1024x768_local_scale_mockup_v001.png` | `thecat_batch75_main_menu_character_select_1280x720_local_scale_mockup_v001.png` | `thecat_batch75_main_menu_character_select_1600x900_local_scale_mockup_v001.png` | `thecat_batch75_main_menu_character_select_1920x1080_local_scale_mockup_v001.png` |
| 梦层路线 | `thecat_batch75_route_map_1024x768_local_scale_mockup_v001.png` | `thecat_batch75_route_map_1280x720_local_scale_mockup_v001.png` | `thecat_batch75_route_map_1600x900_local_scale_mockup_v001.png` | `thecat_batch75_route_map_1920x1080_local_scale_mockup_v001.png` |
| 卧室守卫战 | `thecat_batch75_battle_hud_1024x768_local_scale_mockup_v001.png` | `thecat_batch75_battle_hud_1280x720_local_scale_mockup_v001.png` | `thecat_batch75_battle_hud_1600x900_local_scale_mockup_v001.png` | `thecat_batch75_battle_hud_1920x1080_local_scale_mockup_v001.png` |
| 技能与敌人 | `thecat_batch75_skill_enemy_hud_1024x768_local_scale_mockup_v001.png` | `thecat_batch75_skill_enemy_hud_1280x720_local_scale_mockup_v001.png` | `thecat_batch75_skill_enemy_hud_1600x900_local_scale_mockup_v001.png` | `thecat_batch75_skill_enemy_hud_1920x1080_local_scale_mockup_v001.png` |
| 结算 / 暂停 / 设置 | `thecat_batch75_result_pause_settings_1024x768_local_scale_mockup_v001.png` | `thecat_batch75_result_pause_settings_1280x720_local_scale_mockup_v001.png` | `thecat_batch75_result_pause_settings_1600x900_local_scale_mockup_v001.png` | `thecat_batch75_result_pause_settings_1920x1080_local_scale_mockup_v001.png` |
