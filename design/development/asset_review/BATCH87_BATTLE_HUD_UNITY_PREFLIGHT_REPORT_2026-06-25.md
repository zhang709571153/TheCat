# Batch 87 Battle HUD Unity Preflight

Batch 87 battle HUD candidates are Unity-import preflight ready; formal install remains blocked by runtime evidence and approval.

## Decision

- Ready for Unity preflight: yes
- Formal install allowed: no
- Unity editor import validation ready: yes
- Runtime evidence: 6/8
- Unity evidence complete: no
- Formal runtime binding decision approved: no
- Candidate policy: `candidate-backed Unity preflight only`
- Shared Console classifier: active strict-clean contract
- Console classifier policy: StrictClean is required for formal clean-Console evidence; known environment noise is classified but still blocks formal clean-Console approval.

## Candidate Imports

| component | variant | source candidate | Unity preflight import | size |
| --- | --- | --- | --- | --- |
| battle_hud_top | battle_top_resource_rail_frame | `design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25/sprites/thecat_ui_battle_hud_battle_top_resource_rail_frame_1240x144_candidate_v001.png` | `Assets/TheCat/Art/UI/BattleHUD/thecat_ui_battle_hud_battle_top_resource_rail_frame_1240x144_candidate_v001.png` | 1240x144 |
| battle_hud_party | battle_cat_party_panel | `design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25/sprites/thecat_ui_battle_hud_battle_cat_party_panel_520x188_candidate_v001.png` | `Assets/TheCat/Art/UI/BattleHUD/thecat_ui_battle_hud_battle_cat_party_panel_520x188_candidate_v001.png` | 520x188 |
| battle_hud_enemy | battle_enemy_status_panel | `design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25/sprites/thecat_ui_battle_hud_battle_enemy_status_panel_520x156_candidate_v001.png` | `Assets/TheCat/Art/UI/BattleHUD/thecat_ui_battle_hud_battle_enemy_status_panel_520x156_candidate_v001.png` | 520x156 |
| battle_hud_skill | battle_skill_tray_frame | `design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25/sprites/thecat_ui_battle_hud_battle_skill_tray_frame_900x180_candidate_v001.png` | `Assets/TheCat/Art/UI/BattleHUD/thecat_ui_battle_hud_battle_skill_tray_frame_900x180_candidate_v001.png` | 900x180 |
| battle_hud_status | battle_status_chip_strip | `design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25/sprites/thecat_ui_battle_hud_battle_status_chip_strip_480x96_candidate_v001.png` | `Assets/TheCat/Art/UI/BattleHUD/thecat_ui_battle_hud_battle_status_chip_strip_480x96_candidate_v001.png` | 480x96 |
| battle_hud_runtime | battle_runtime_control_cluster | `design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25/sprites/thecat_ui_battle_hud_battle_runtime_control_cluster_360x96_candidate_v001.png` | `Assets/TheCat/Art/UI/BattleHUD/thecat_ui_battle_hud_battle_runtime_control_cluster_360x96_candidate_v001.png` | 360x96 |

## Blocking Runtime Evidence

- Missing Unity evidence: `design/development/asset_review/batch_87_battle_hud_unity_preflight/console_clean_report.md`
- Missing Unity evidence: `design/development/asset_review/batch_87_battle_hud_unity_preflight/human_review_approval.md`
- Formal runtime binding decision has not passed.

## Protected Runtime State

- Human review request packet: `design/development/asset_review/BATCH87_BATTLE_HUD_HUMAN_REVIEW_REQUEST_2026-06-26.md`.
- Formal runtime binding decision request packet: `design/development/asset_review/BATCH87_BATTLE_HUD_FORMAL_RUNTIME_BINDING_DECISION_REQUEST_2026-06-26.md`.
- Batch 87 imports are not included in `P0VisualAssetCatalog.P0RuntimeVisualBindingCount`.
- Current IMGUI battle HUD remains the playable runtime path until screenshots, Console, click-target, and review gates pass.
- Do not mark Batch 87 as formally installed before explicit review approval.
