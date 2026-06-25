# P0 Route Reward Detail Badges

Date: 2026-06-14

Batch: `p0_asset_batch_15_route_reward_detail_badges`

## Purpose

Produce deterministic, non-cat route reward detail badges for the P0 route-map
reward-choice cards. These badges clarify the effect class of each choice after
the player sees the route node category and choice icon.

## Inputs

- `thecat_ui_panel_dreamglass_512x256_v001`
- `thecat_ui_reward_fishtreat_icon_128_v001`
- `thecat_ui_reward_dreamshard_icon_128_v001`
- `thecat_ui_choice_purchase_supply_icon_128_v001`
- `thecat_ui_choice_rest_supply_icon_128_v001`
- `thecat_ui_choice_dreamevent_modifier_icon_128_v001`
- `thecat_ui_choice_authority_upgrade_icon_128_v001`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`

## Output Assets

| asset id | subject | path | size |
| --- | --- | --- | --- |
| `thecat_ui_reward_detail_gain_badge_192x64_v001` | route reward gain detail | `Assets/TheCat/Art/UI/Badges/thecat_ui_reward_detail_gain_badge_192x64_v001.png` | 192x64 |
| `thecat_ui_reward_detail_cost_badge_192x64_v001` | route reward cost detail | `Assets/TheCat/Art/UI/Badges/thecat_ui_reward_detail_cost_badge_192x64_v001.png` | 192x64 |
| `thecat_ui_reward_detail_recovery_badge_192x64_v001` | route reward recovery detail | `Assets/TheCat/Art/UI/Badges/thecat_ui_reward_detail_recovery_badge_192x64_v001.png` | 192x64 |
| `thecat_ui_reward_detail_risk_badge_192x64_v001` | route reward risk detail | `Assets/TheCat/Art/UI/Badges/thecat_ui_reward_detail_risk_badge_192x64_v001.png` | 192x64 |
| `thecat_ui_reward_detail_upgrade_badge_192x64_v001` | route reward upgrade detail | `Assets/TheCat/Art/UI/Badges/thecat_ui_reward_detail_upgrade_badge_192x64_v001.png` | 192x64 |

## Visual Rules

- Use transparent PNGs with a dark dreamglass badge body and category accent.
- Keep each badge readable when drawn on the right side of a route reward card.
- Include symbolic motifs, not only text:
  - gain: plus sign and star
  - cost: coin/fish spend cue
  - recovery: crescent, pulse, or healing arc
  - risk: warning triangle or dream-risk cue
  - upgrade: upward arrow and star
- Use restrained P0 UI colors from Batch 08, Batch 12, and Batch 13.

## Prohibited

- Do not include cat bodies, cat faces, fur markings, starter-cat costume
  fragments, or colored-turnaround crops.
- Do not read or modify starter cat sprite files.
- Do not use photorealistic UI, glossy gacha badges, or single-hue gradients.
- Do not overwrite existing badge assets without incrementing the version.

## Acceptance

- Each PNG is exactly `192x64`.
- Each `.png.meta` contains `TheCatP0ImportSettings:v1` and
  `batch:p0_asset_batch_15_route_reward_detail_badges`.
- Manifest rows are `generated`, `badge`, and reference
  `thecat_ui_panel_dreamglass_512x256_v001`.
- Runtime visual bindings include all five `route_reward_detail.*` entries.
- `P0RouteMapPresenter` exposes effect detail badges for reward choices.
