# P0 Shop Item Cards

## Scope

Create four deterministic, non-cat UI item cards for the existing P0 shop choices:

- `shop_bed_patch`
- `shop_litter_sachet`
- `shop_late_kibble`
- `shop_free_sample`

Each asset is a transparent PNG at `384x160` and is imported as a Unity Sprite.

## Source Authority

- Design docs: `design/梦境支配者核心玩法/docs`
- UI references:
  - `thecat_ui_panel_dreamglass_512x256_v001`
  - `thecat_ui_routecard_shop_frame_512x256_v001`
  - `thecat_ui_node_shop_summary_banner_512x160_v001`
  - `thecat_ui_choice_purchase_supply_icon_128_v001`
  - `thecat_ui_reward_fishtreat_icon_128_v001`
  - `thecat_ui_core_sleep_icon_64_v001`
  - `thecat_ui_core_poop_icon_64_v001`
  - `thecat_ui_core_hunger_icon_64_v001`

## Visual Rules

- Strictly symbolic and non-cat.
- Do not use cat silhouettes, ears, tails, paws, fur patches, face markings, or starter-cat costume/civilization motifs.
- Do not derive from Saiban, Nephthys, or Suzune colored turnaround assets.
- Use the existing dreamglass UI language: dark translucent body, cyan rim, fish-gold accents, soft sleep-glow, clean icon silhouettes.
- The image must remain readable at compact route-map size.

## Item Motifs

- `shop_bed_patch`: patched pillow, owner sleep repair line, fish-cost shop accent.
- `shop_litter_sachet`: sachet, scoop/cleanse arc, poop-pressure relief arrow.
- `shop_late_kibble`: kibble pouch, feeder bowl, hunger safe-line arrow.
- `shop_free_sample`: fish treat, free sample tag, small shop sparkle.

## Output

- `Assets/TheCat/Art/UI/Cards/thecat_ui_shop_item_bed_patch_card_384x160_v001.png`
- `Assets/TheCat/Art/UI/Cards/thecat_ui_shop_item_litter_sachet_card_384x160_v001.png`
- `Assets/TheCat/Art/UI/Cards/thecat_ui_shop_item_late_kibble_card_384x160_v001.png`
- `Assets/TheCat/Art/UI/Cards/thecat_ui_shop_item_free_sample_card_384x160_v001.png`

## Validation

- `P0AssetManifestCatalog.P0ManifestAssetCount == 82`
- `P0VisualAssetCatalog.P0RuntimeVisualBindingCount == 78`
- The four `shop_item.*` bindings resolve in `P0RuntimeVisualBindingCoverage`.
- The shop route-map surface exposes matching `ItemCardAsset` references for all four choice ids when the run has enough fish treats.
