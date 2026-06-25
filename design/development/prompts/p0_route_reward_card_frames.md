# P0 Route Reward Card Frames

Date: 2026-06-14

Batch: `p0_asset_batch_13_route_reward_card_frames`

## Purpose

Produce deterministic, non-cat route reward card frames for the P0 route-map
reward-choice surface. These frames sit behind the Batch 12 route choice icons
and make partner, shop, authority blessing, dream-event, and rest-nest choices
read as distinct reward categories before final UGUI work.

## Inputs

- `thecat_ui_panel_dreamglass_512x256_v001`
- `thecat_ui_choice_partner_recruit_icon_128_v001`
- `thecat_ui_choice_purchase_supply_icon_128_v001`
- `thecat_ui_choice_authority_blessing_icon_128_v001`
- `thecat_ui_choice_dreamevent_modifier_icon_128_v001`
- `thecat_ui_choice_rest_supply_icon_128_v001`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`

## Output Assets

| asset id | subject | path | size |
| --- | --- | --- | --- |
| `thecat_ui_routecard_partner_frame_512x256_v001` | partner route card | `Assets/TheCat/Art/UI/Frames/thecat_ui_routecard_partner_frame_512x256_v001.png` | 512x256 |
| `thecat_ui_routecard_shop_frame_512x256_v001` | shop route card | `Assets/TheCat/Art/UI/Frames/thecat_ui_routecard_shop_frame_512x256_v001.png` | 512x256 |
| `thecat_ui_routecard_blessing_frame_512x256_v001` | blessing route card | `Assets/TheCat/Art/UI/Frames/thecat_ui_routecard_blessing_frame_512x256_v001.png` | 512x256 |
| `thecat_ui_routecard_dreamevent_frame_512x256_v001` | dream event route card | `Assets/TheCat/Art/UI/Frames/thecat_ui_routecard_dreamevent_frame_512x256_v001.png` | 512x256 |
| `thecat_ui_routecard_restnest_frame_512x256_v001` | rest nest route card | `Assets/TheCat/Art/UI/Frames/thecat_ui_routecard_restnest_frame_512x256_v001.png` | 512x256 |

## Visual Rules

- Use transparent PNGs with a dark dreamglass body and readable category accent.
- Leave the right side open for route-choice text.
- Keep category motifs symbolic:
  - partner: linked circles and plus sign
  - shop: fish treat supply bag
  - blessing: altar star
  - dream event: cloud and risk/benefit arrows
  - rest nest: crescent nest
- Use the same restrained P0 UI palette as Batch 08 and Batch 12.

## Prohibited

- Do not include cat bodies, cat faces, fur markings, starter-cat costume
  fragments, or colored-turnaround crops.
- Do not use photorealistic card art, glossy gacha card frames, or single-hue
  gradient-only cards.
- Do not overwrite existing frame assets without incrementing the version.

## Acceptance

- Each PNG is `512x256` and has a matching `.png.meta` with
  `TheCatP0ImportSettings:v1`.
- Each manifest row is `generated`, `frame`, and references
  `thecat_ui_panel_dreamglass_512x256_v001`.
- Runtime visual bindings include all five `route_reward_card.*` entries.
- `P0RouteMapPresenter` exposes the correct card frame for partner, shop,
  blessing, dream-event, and rest-nest reward-choice surfaces.
