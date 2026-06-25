# P0 RestNest Recovery Card

## Scope

Generate one deterministic P0 route-map choice card for the concrete RestNest reward choice:

- `rest_nest_recovery`

This is a route choice card for a recovery node. It is not a cat portrait, cat item, or character illustration.

## Required Output

Create a transparent PNG sprite in `Assets/TheCat/Art/UI/Cards`:

- `thecat_ui_restnest_recovery_card_384x160_v001.png`

The matching Unity `.meta` file must use `TextureImporter.textureType: 8`, `spriteMode: 1`, `spriteBorder: {x: 12, y: 12, z: 12, w: 12}`, and `userData` containing:

- `TheCatP0ImportSettings:v1`
- `batch:p0_asset_batch_22_rest_nest_recovery_card`
- `nonCatSymbolicOnly:true`

## Visual Language

Use the existing P0 RestNest and dream UI language:

- dreamglass panel dark translucent body
- mint/cyan RestNest recovery accent
- fish-gold small repair highlight
- soft glow, rounded frame, hand-drawn symbolic lines

Reference assets:

- `thecat_ui_panel_dreamglass_512x256_v001`
- `thecat_ui_routecard_restnest_frame_512x256_v001`
- `thecat_ui_node_restnest_summary_banner_512x160_v001`
- `thecat_ui_choice_rest_supply_icon_128_v001`
- `thecat_ui_core_sleep_icon_64_v001`
- `thecat_ui_core_poop_icon_64_v001`
- `thecat_ui_core_hunger_icon_64_v001`
- `thecat_ui_core_hp_icon_64_v001`
- `thecat_ui_reward_detail_recovery_badge_192x64_v001`

## Motif

`rest_nest_recovery` should read as a single compact recovery package:

- crescent rest nest or folded blanket ring
- repaired bedline stitch
- owner sleep wave restored
- poop pressure arrow down
- hunger bowl safe-line
- cat HP plus or safe-line symbol

## Hard Constraints

- Do not create cat bodies, cat silhouettes, cat ears, paws, tails, fur markings, collars, costumes, or civilization symbols.
- Do not derive from Saiban, Nephthys, Suzune, or any colored turnaround.
- Do not modify starter cat assets.
- Do not introduce text into the image pixels; runtime UI supplies labels.
- Keep silhouette readable at 216x90 in IMGUI preview.

## Acceptance

- `P0AssetManifestCatalog.P0ManifestAssetCount == 86`
- `P0VisualAssetCatalog.P0RuntimeVisualBindingCount == 82`
- `P0VisualAssetCatalog.GetRestNestChoiceCard(choice)` resolves `rest_nest_recovery` and returns empty for unrelated choices.
- `P0VisualAssetCatalog.GetRouteChoiceCard(choice)` resolves shop item cards, DreamEvent choice cards, and the RestNest recovery card.
- Runtime bindings include `rest_nest_choice.recovery`.
- The runtime visual contact sheet includes the new binding.
- Offline checks pass for dimensions, `.meta` import settings, catalog coverage, route-map surface coverage, and C# compilation.
