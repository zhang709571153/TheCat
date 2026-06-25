# Batch 93 Cat Room Map Decals Process Note

Date: 2026-06-25

Mode: built-in Codex `imagegen` plus local chroma-key removal.

## Goal

Produce a narrow, candidate-only map/decal kit for cat-room floor and wall composition. This batch intentionally avoids starter-cat body art, character framesheets, UI buttons, baked text, and full background replacement.

## Files

- `source/thecat_map_cat_room_decals_batch93_chromakey_source_v001.png`
- `source/thecat_map_cat_room_decals_batch93_alpha_sheet_v001.png`
- `semantic_sprites/*.png`
- `thecat_map_cat_room_decals_batch93_semantic_manifest.csv`
- `thecat_map_cat_room_decals_batch93_semantic_contact_sheet_v001.png`
- `thecat_map_cat_room_decals_batch93_review.csv`

## Cutting Notes

The generated image was a 1254 by 1254 sheet. Chroma-key removal sampled the magenta border color and produced an alpha sheet with alpha extrema `(0, 255)`.

Projection bands were used for the 3 by 4 layout:

- Rows: `(72,314)`, `(430,600)`, `(672,882)`, `(962,1180)`.
- Columns: `(52,423)`, `(451,800)`, `(849,1207)`.

Each semantic sprite was cropped to its alpha bbox and padded with 22 px transparent margins.

## Local Validation

- Manifest rows: 12.
- Missing sprite paths: 0.
- Nontransparent edge pixels: 0.
- `soft_oval_prop_shadow_decal` alpha maximum is 35, confirming it is a low-alpha shadow decal.

## Review Outcome

Independent visual and production QA reviews both returned `PASS_WITH_P2`.

No hard rework is required before Unity candidate import. The main P2 watches are scene readability and usage constraints for semi-transparent decals, especially window light, dream crack, dust sparkle, and low-alpha shadow.
