# Batch 84 Result / Settlement Preflight Candidate Review

Result: local candidate packet generated; not Unity accepted.

## Scope

- Covers battle victory, battle defeat, run-cleared settlement, and run-failed settlement local screen compositions.
- Provides textless panel, reward-row, stat-chip, action-button, divider, and stamp sprites for later Unity import testing.
- Reuses existing P0 UI shell/banner/reward sources from `Assets/TheCat/Art/UI` without altering them.
- Does not generate, crop, recolor, or import starter-cat body art.

## Candidate Rows

| Variant | Type | Size | Path |
| --- | --- | --- | --- |
| `panel_frame` | `sprite` | `960x540` | `design/development/asset_candidates/ui/result_settlement/batch_84_result_settlement_preflight_2026-06-25/sprites/thecat_ui_result_settlement_panel_frame_960x540_candidate_v001.png` |
| `reward_row_frame` | `sprite` | `760x96` | `design/development/asset_candidates/ui/result_settlement/batch_84_result_settlement_preflight_2026-06-25/sprites/thecat_ui_result_settlement_reward_row_frame_760x96_candidate_v001.png` |
| `stat_chip_frame` | `sprite` | `256x72` | `design/development/asset_candidates/ui/result_settlement/batch_84_result_settlement_preflight_2026-06-25/sprites/thecat_ui_result_settlement_stat_chip_frame_256x72_candidate_v001.png` |
| `action_button_frame` | `sprite` | `384x96` | `design/development/asset_candidates/ui/result_settlement/batch_84_result_settlement_preflight_2026-06-25/sprites/thecat_ui_result_settlement_action_button_frame_384x96_candidate_v001.png` |
| `outcome_divider` | `sprite` | `640x32` | `design/development/asset_candidates/ui/result_settlement/batch_84_result_settlement_preflight_2026-06-25/sprites/thecat_ui_result_settlement_outcome_divider_640x32_candidate_v001.png` |
| `success_stamp_ring` | `sprite` | `256x256` | `design/development/asset_candidates/ui/result_settlement/batch_84_result_settlement_preflight_2026-06-25/sprites/thecat_ui_result_settlement_success_stamp_ring_256x256_candidate_v001.png` |
| `failure_stamp_ring` | `sprite` | `256x256` | `design/development/asset_candidates/ui/result_settlement/batch_84_result_settlement_preflight_2026-06-25/sprites/thecat_ui_result_settlement_failure_stamp_ring_256x256_candidate_v001.png` |
| `battle_victory_1920x1080` | `local_mockup` | `1920x1080` | `design/development/asset_candidates/ui/result_settlement/batch_84_result_settlement_preflight_2026-06-25/mockups/thecat_ui_result_settlement_battle_victory_1920x1080_local_mockup_v001.png` |
| `battle_defeat_1920x1080` | `local_mockup` | `1920x1080` | `design/development/asset_candidates/ui/result_settlement/batch_84_result_settlement_preflight_2026-06-25/mockups/thecat_ui_result_settlement_battle_defeat_1920x1080_local_mockup_v001.png` |
| `run_cleared_1365x768` | `local_mockup` | `1365x768` | `design/development/asset_candidates/ui/result_settlement/batch_84_result_settlement_preflight_2026-06-25/mockups/thecat_ui_result_settlement_run_cleared_1365x768_local_mockup_v001.png` |
| `run_failed_1024x768` | `local_mockup` | `1024x768` | `design/development/asset_candidates/ui/result_settlement/batch_84_result_settlement_preflight_2026-06-25/mockups/thecat_ui_result_settlement_run_failed_1024x768_local_mockup_v001.png` |

## Required Unity Gates

- Result/settlement screen screenshots at target resolutions.
- Unity-rendered Chinese title/button/reward text replacement proof; no baked Chinese text in sprites.
- Reward rows and stat chips must remain readable on 1024x768.
- Sprite import settings, scene/prefab binding proof, and clean Console.
