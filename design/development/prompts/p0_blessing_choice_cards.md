# P0 Blessing Choice Cards

Generated: 2026-06-14

## Scope

Create deterministic, non-cat route choice card assets for the P0 authority
blessing node:

- `thecat_ui_blessing_oath_bedline_card_384x160_v001`
- `thecat_ui_blessing_dominion_sandglass_card_384x160_v001`
- `thecat_ui_blessing_rhythm_lullaby_card_384x160_v001`

These cards support both first-time authority blessing picks and later upgrade
choices. They must express the blessing power symbolically; they must not define
or replace Saiban, Nephthys, Suzune, or any other cat body artwork.

## Style Anchors

- `thecat_ui_panel_dreamglass_512x256_v001`
- `thecat_ui_routecard_blessing_frame_512x256_v001`
- `thecat_ui_route_blessing_icon_128_v001`
- `thecat_ui_choice_authority_blessing_icon_128_v001`
- `thecat_ui_choice_authority_upgrade_icon_128_v001`
- `thecat_ui_blessing_oath_bedline_seal_128_v001`
- `thecat_ui_blessing_dominion_sandglass_seal_128_v001`
- `thecat_ui_blessing_rhythm_lullaby_seal_128_v001`
- `thecat_ui_reward_detail_gain_badge_192x64_v001`
- `thecat_ui_reward_detail_upgrade_badge_192x64_v001`

## Hard Constraints

- Do not draw cat bodies, cat faces, ears, paws, tails, fur markings, collars,
  clothing, armor, hats, bells, weapons, or character portraits.
- Do not derive from Saiban, Nephthys, Suzune, or their colored three-view
  turnarounds except as negative constraints.
- Do not use any starter-cat civilization prop as the main silhouette.
- Do not place text in the bitmap; runtime UI supplies labels and descriptions.
- Keep the cards at `384x160`, transparent background, and 12px sprite border.

## Card Direction

`blessing_authority_oath_bedline`:

- Bedline oath, shield crest, knockback wave, and protected-bed silhouette.
- Cool cyan and silver defense accents with small gold oath sparks.
- Should read as "defensive authority" rather than "Saiban portrait".

`blessing_authority_dominion_sandglass`:

- Moon sandglass, slow ring, mark-eye, and drifting sand motes.
- Violet, moon-gold, and cyan accents.
- Should read as "time and mark control" rather than "Nephthys costume".

`blessing_authority_rhythm_lullaby`:

- Crescent lullaby waves, bell rhythm, recovery pulse, and sleep-stable arc.
- Pink-violet, warm gold, and soft cyan accents.
- Should read as "healing rhythm authority" rather than "Suzune portrait".

## Acceptance

- All three PNGs exist under `Assets/TheCat/Art/UI/Cards`.
- All three `.png.meta` files include
  `batch:p0_asset_batch_24_blessing_choice_cards`, `spriteBorder:12`, and
  `nonCatSymbolicOnly:true`.
- Manifest count becomes 91 and runtime binding count becomes 87.
- Route map blessing surfaces resolve the specific choice cards for first-time
  blessing gains and upgrade choices.
- Starter cat source locks and formal import blocks are unchanged.
