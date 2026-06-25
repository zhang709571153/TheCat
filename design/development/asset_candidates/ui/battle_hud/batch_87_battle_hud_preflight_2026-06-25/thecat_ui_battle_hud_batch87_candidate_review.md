# Batch 87 Battle HUD Preflight Candidate Review

Result: local candidate packet generated; not Unity accepted.

## Scope

- Covers battle HUD composition, top resource rail, cat party panel, enemy status panel, skill tray, status chips, and runtime controls.
- Reuses existing bedroom battle background, core gauges, status icons, HUD avatars, enemy sprites, skill frames, recommended symbolic skill icons, and runtime control icons.
- Does not generate, crop, recolor, or import starter-cat body art.
- Does not bake Chinese text into sprites; Unity-rendered labels, numbers, cooldowns, and tooltips remain required.

## Candidate Rows

| Variant | Type | Size | Path |
| --- | --- | --- | --- |
| `battle_top_resource_rail_frame` | `sprite` | `1240x144` | `design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25/sprites/thecat_ui_battle_hud_battle_top_resource_rail_frame_1240x144_candidate_v001.png` |
| `battle_cat_party_panel` | `sprite` | `520x188` | `design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25/sprites/thecat_ui_battle_hud_battle_cat_party_panel_520x188_candidate_v001.png` |
| `battle_enemy_status_panel` | `sprite` | `520x156` | `design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25/sprites/thecat_ui_battle_hud_battle_enemy_status_panel_520x156_candidate_v001.png` |
| `battle_skill_tray_frame` | `sprite` | `900x180` | `design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25/sprites/thecat_ui_battle_hud_battle_skill_tray_frame_900x180_candidate_v001.png` |
| `battle_status_chip_strip` | `sprite` | `480x96` | `design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25/sprites/thecat_ui_battle_hud_battle_status_chip_strip_480x96_candidate_v001.png` |
| `battle_runtime_control_cluster` | `sprite` | `360x96` | `design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25/sprites/thecat_ui_battle_hud_battle_runtime_control_cluster_360x96_candidate_v001.png` |
| `battle_hud_1920x1080` | `local_mockup` | `1920x1080` | `design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25/mockups/thecat_ui_battle_hud_battle_hud_1920x1080_local_mockup_v001.png` |
| `battle_hud_pressure_1365x768` | `local_mockup` | `1365x768` | `design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25/mockups/thecat_ui_battle_hud_battle_hud_pressure_1365x768_local_mockup_v001.png` |
| `battle_hud_compact_1280x720` | `local_mockup` | `1280x720` | `design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25/mockups/thecat_ui_battle_hud_battle_hud_compact_1280x720_local_mockup_v001.png` |
| `battle_hud_dense_1024x768` | `local_mockup` | `1024x768` | `design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25/mockups/thecat_ui_battle_hud_battle_hud_dense_1024x768_local_mockup_v001.png` |

## Required Unity Gates

- Battle HUD screenshots at 1920x1080, 1365x768, 1280x720, and 1024x768.
- Unity-rendered Chinese labels, dynamic gauge values, cooldown digits, cost chips, and status counts.
- Skill slot selected/ready/cooldown/disabled states remain distinguishable at actual HUD scale.
- Top resource rail and bottom skill tray must not cover enemies, bed/props, or attack telegraphs.
- Runtime pause/speed/restart controls need click-target proof and do not conflict with gameplay input.
- Sprite import settings, scene/prefab binding proof, and clean Console.
