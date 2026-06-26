# Batch 87 Battle HUD Formal Runtime Binding Decision Request

This is a formal-runtime-binding decision request only. It is not an approval file, does not change runtime bindings, and must not be counted as formal install evidence.

## Decision Boundary

- Decision request ready: yes
- Formal runtime binding decision approved: no
- Formal install allowed: no
- Unity evidence complete: no
- Candidate policy: `candidate-backed Unity preflight only`
- Shared Console classifier: active strict-clean contract
- Console classifier policy: StrictClean is required for formal clean-Console evidence; known environment noise is classified but still blocks formal clean-Console approval.

## Proposed Binding Scope

- Surface: battle HUD runtime visual frames and supporting HUD panels.
- Candidate import root: `Assets/TheCat/Art/UI/BattleHUD`.
- Runtime catalog under review: `P0VisualAssetCatalog.CreateP0RuntimeBindings()`.
- Current playable runtime path: IMGUI battle HUD, protected until all gates pass.
- Candidate component count: 6

| component | variant | Unity preflight import | size |
| --- | --- | --- | --- |
| battle_hud_top | battle_top_resource_rail_frame | `Assets/TheCat/Art/UI/BattleHUD/thecat_ui_battle_hud_battle_top_resource_rail_frame_1240x144_candidate_v001.png` | 1240x144 |
| battle_hud_party | battle_cat_party_panel | `Assets/TheCat/Art/UI/BattleHUD/thecat_ui_battle_hud_battle_cat_party_panel_520x188_candidate_v001.png` | 520x188 |
| battle_hud_enemy | battle_enemy_status_panel | `Assets/TheCat/Art/UI/BattleHUD/thecat_ui_battle_hud_battle_enemy_status_panel_520x156_candidate_v001.png` | 520x156 |
| battle_hud_skill | battle_skill_tray_frame | `Assets/TheCat/Art/UI/BattleHUD/thecat_ui_battle_hud_battle_skill_tray_frame_900x180_candidate_v001.png` | 900x180 |
| battle_hud_status | battle_status_chip_strip | `Assets/TheCat/Art/UI/BattleHUD/thecat_ui_battle_hud_battle_status_chip_strip_480x96_candidate_v001.png` | 480x96 |
| battle_hud_runtime | battle_runtime_control_cluster | `Assets/TheCat/Art/UI/BattleHUD/thecat_ui_battle_hud_battle_runtime_control_cluster_360x96_candidate_v001.png` | 360x96 |

## Evidence Required Before Decision

- Batch 87 preflight report remains ready and linked: `design/development/asset_review/BATCH87_BATTLE_HUD_UNITY_PREFLIGHT_REPORT_2026-06-25.md`.
- Human review approval exists separately; this request packet is not that approval.
- Clean Console evidence exists separately and passes the shared `StrictClean` classifier.
- Runtime binding review confirms which scene, presenter, and catalog path will consume the candidate frames.
- Post-binding validation plan covers four-resolution screenshots, click targets, warning/telegraph visibility, and rollback.

## Decision Questions

- Should Batch 87 replace any current IMGUI battle HUD framing, or remain an overlay/reference until a later UI-system pass?
- Which exact `P0VisualAssetCatalog` rows would be added or changed if the decision later passes?
- Which presenter/controller owns layout responsibilities after binding, and which test proves no click-target or text regression?
- What rollback condition returns the battle HUD to the current playable IMGUI path?
- Does this decision intentionally exclude starter-cat body art, other UI candidate batches, and final visual acceptance?

## Current Blocking Items

- Missing Unity evidence: `design/development/asset_review/batch_87_battle_hud_unity_preflight/console_clean_report.md`
- Missing Unity evidence: `design/development/asset_review/batch_87_battle_hud_unity_preflight/human_review_approval.md`
- Formal runtime binding decision has not passed.

## Protected Runtime State

- Do not add Batch 87 candidate ids to `P0VisualAssetCatalog` from this request packet.
- Do not alter battle HUD scene, presenter, or controller runtime binding from this request packet.
- Do not create or count clean Console or human approval evidence from this request packet.
- Keep the current IMGUI battle HUD as the playable runtime path.
