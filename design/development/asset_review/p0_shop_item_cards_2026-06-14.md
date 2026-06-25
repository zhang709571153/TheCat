# P0 Shop Item Cards

- Batch: `p0_asset_batch_20_shop_item_cards`
- Output directory: `Assets/TheCat/Art/UI/Cards`
- Prompt: `design/development/prompts/p0_shop_item_cards.md`
- Scope: deterministic non-cat UI cards for existing shop reward choice ids.
- Cat constraint: no cat silhouette, no fur markings, no starter-cat turnaround derivative, no civilization costume motif.
- Runtime bindings: `shop_item.bed_patch`, `shop_item.litter_sachet`, `shop_item.late_kibble`, `shop_item.free_sample`.

## Generated Assets

- `thecat_ui_shop_item_bed_patch_card_384x160_v001`
  - subject: `shop_bed_patch`
  - motif: patched pillow and owner sleep repair
  - size: 384x160
  - path: `Assets/TheCat/Art/UI/Cards/thecat_ui_shop_item_bed_patch_card_384x160_v001.png`
  - md5: `c2aad886cbe728a5b5c803efd79f0a39`
- `thecat_ui_shop_item_litter_sachet_card_384x160_v001`
  - subject: `shop_litter_sachet`
  - motif: litter sachet and clean scoop relief
  - size: 384x160
  - path: `Assets/TheCat/Art/UI/Cards/thecat_ui_shop_item_litter_sachet_card_384x160_v001.png`
  - md5: `7e63ff97364f57c2a011105651b90356`
- `thecat_ui_shop_item_late_kibble_card_384x160_v001`
  - subject: `shop_late_kibble`
  - motif: kibble pouch and hunger safe line
  - size: 384x160
  - path: `Assets/TheCat/Art/UI/Cards/thecat_ui_shop_item_late_kibble_card_384x160_v001.png`
  - md5: `0dc25391feb1e29ed653988010c25baa`
- `thecat_ui_shop_item_free_sample_card_384x160_v001`
  - subject: `shop_free_sample`
  - motif: fish treat free sample tag
  - size: 384x160
  - path: `Assets/TheCat/Art/UI/Cards/thecat_ui_shop_item_free_sample_card_384x160_v001.png`
  - md5: `cc6a9d30b1d17f0ccfa472aaed0b8e97`

## Consistency Check

- Uses the accepted dreamglass/shop-card UI palette and Batch 19 shop summary banner visual language.
- Keeps all forms symbolic: patch, sachet, kibble, fish treat, arrows, stars, and safe-line marks.
- Does not create or modify any starter cat asset; colored turnaround conformance remains untouched.
- `.meta` files carry `TheCatP0ImportSettings:v1`, `batch:p0_asset_batch_20_shop_item_cards`, and `nonCatSymbolicOnly:true`.
