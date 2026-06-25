# P0 Dream Event Choice Cards

## Scope

Generate deterministic P0 route-map card art for the three concrete DreamEvent choices:

- `dream_event_clear_notifications`
- `dream_event_catnip_residue`
- `dream_event_mark_all_read`

These are route choice cards, not cat portraits or character illustrations.

## Required Outputs

All outputs must be transparent PNG sprites in `Assets/TheCat/Art/UI/Cards`:

- `thecat_ui_dreamevent_clear_notifications_card_384x160_v001.png`
- `thecat_ui_dreamevent_catnip_residue_card_384x160_v001.png`
- `thecat_ui_dreamevent_mark_all_read_card_384x160_v001.png`

Unity `.meta` files must use `TextureImporter.textureType: 8`, `spriteMode: 1`, `spriteBorder: {x: 12, y: 12, z: 12, w: 12}`, and `userData` containing:

- `TheCatP0ImportSettings:v1`
- `batch:p0_asset_batch_21_dream_event_choice_cards`
- `nonCatSymbolicOnly:true`

## Visual Language

Use the existing P0 dream UI language:

- dreamglass panel dark translucent body
- purple-cyan dream-event accent
- fish-gold reward accent
- red notification/risk accent
- soft glow, rounded frame, hand-drawn symbolic lines

Reference assets:

- `thecat_ui_panel_dreamglass_512x256_v001`
- `thecat_ui_routecard_dreamevent_frame_512x256_v001`
- `thecat_ui_node_dreamevent_summary_banner_512x160_v001`
- `thecat_ui_choice_dreamevent_modifier_icon_128_v001`
- `thecat_ui_reward_fishtreat_icon_128_v001`
- `thecat_ui_core_sleep_icon_64_v001`

## Motifs

- `dream_event_clear_notifications`: red-dot notification rain swept away into fish-treat reward.
- `dream_event_catnip_residue`: purple residue cloud, green skill-damage-up arrow, red poop-growth warning arrow.
- `dream_event_mark_all_read`: message cards, large check mark, sleep-stabilization wave.

## Hard Constraints

- Do not create cat bodies, cat silhouettes, cat ears, paws, tails, fur markings, collars, costumes, or civilization symbols.
- Do not derive from Saiban, Nephthys, Suzune, or any colored turnaround.
- Do not modify starter cat assets.
- Do not introduce text into the image pixels; runtime UI supplies labels.
- Keep silhouette readable at 216x90 in IMGUI preview.

## Acceptance

- `P0AssetManifestCatalog.P0ManifestAssetCount == 85`
- `P0VisualAssetCatalog.P0RuntimeVisualBindingCount == 81`
- Route map DreamEvent surface resolves the three choice cards.
- Runtime bindings include:
  - `dream_event_choice.clear_notifications`
  - `dream_event_choice.catnip_residue`
  - `dream_event_choice.mark_all_read`
- The runtime visual contact sheet includes all three new bindings.
- Offline checks pass for dimensions, `.meta` import settings, catalog coverage, route-map surface coverage, and C# compilation.
