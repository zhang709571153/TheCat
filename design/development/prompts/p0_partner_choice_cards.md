# P0 Partner Choice Cards

Generated: 2026-06-14

## Scope

Create deterministic, non-cat route choice card assets for the P0 partner node:

- `thecat_ui_partner_shadowmaru_preview_card_384x160_v001`
- `thecat_ui_partner_duplicate_supply_card_384x160_v001`

These cards support the current `partner_shadowmaru_preview` and
`partner_preview_duplicate_supply` choices without defining or replacing any
cat body artwork.

## Style Anchors

- `thecat_ui_panel_dreamglass_512x256_v001`
- `thecat_ui_routecard_partner_frame_512x256_v001`
- `thecat_ui_route_partner_icon_128_v001`
- `thecat_ui_choice_partner_recruit_icon_128_v001`
- `thecat_ui_reward_fishtreat_icon_128_v001`
- `thecat_ui_reward_detail_gain_badge_192x64_v001`

## Hard Constraints

- Do not draw cat bodies, cat faces, ears, paws, tails, fur markings, collars,
  armor, hats, robes, bells, weapons, or any starter-cat civilization symbols.
- Do not infer Shadowmaru's appearance. This batch is UI symbolism only.
- Do not derive from Saiban, Nephthys, Suzune, or any colored turnaround except
  as a negative constraint.
- Do not place text in the bitmap; runtime UI supplies labels and descriptions.
- Keep the cards at `384x160`, transparent background, and 12px sprite border.

## Card Direction

`partner_shadowmaru_preview`:

- Partner invitation, shadow contract, linked dream path, and invite arrow.
- Partner cyan and violet accents with small gold star highlights.
- Should read as "invite a partner" rather than "portrait of a partner".

`partner_preview_duplicate_supply`:

- Duplicate partner fallback, linked circles, conversion arrow, and night-fish
  supply symbols.
- Violet-to-gold accents, with fish reward meaning visible at card scale.
- Should read as "duplicate converts into supplies" without any cat imagery.

## Acceptance

- Both PNGs exist under `Assets/TheCat/Art/UI/Cards`.
- Both `.png.meta` files include `batch:p0_asset_batch_23_partner_choice_cards`,
  `spriteBorder:12`, and `nonCatSymbolicOnly:true`.
- Manifest count becomes 88 and runtime binding count becomes 84.
- Route map partner surfaces resolve the specific choice cards for both first
  recruit and duplicate-fallback states.
- Starter cat source locks and formal import blocks are unchanged.
