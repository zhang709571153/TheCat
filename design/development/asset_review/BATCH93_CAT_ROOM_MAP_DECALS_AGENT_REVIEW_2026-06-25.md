# Batch 93 Cat Room Map Decals Agent Review

Date: 2026-06-25

Scope: `design/development/asset_candidates/map/cat_room_decals/batch_93_cat_room_map_decals_imagegen_2026-06-25`

## Verdict

`PASS_WITH_P2`

Batch 93 is a narrow built-in Codex `imagegen` map/decal batch for cat-room floor, wall trim, prop shadow, window light, dream crack, threshold, dust, and boundary utility pieces. It contains no character body art, no baked text, no UI button art, and no runtime promotion.

The candidate package is complete pending Unity import, sorting-layer, scale, and scene screenshot review.

## Production Inputs

- Source sheet: `source/thecat_map_cat_room_decals_batch93_chromakey_source_v001.png`
- Alpha sheet: `source/thecat_map_cat_room_decals_batch93_alpha_sheet_v001.png`
- Semantic sprites: `semantic_sprites/`
- Manifest: `thecat_map_cat_room_decals_batch93_semantic_manifest.csv`
- Contact sheet: `thecat_map_cat_room_decals_batch93_semantic_contact_sheet_v001.png`
- Review CSV: `thecat_map_cat_room_decals_batch93_review.csv`

## Review Inputs

- Visual/style reviewer Sagan: `PASS_WITH_P2`.
  - Batch93 matches the Batch91/92 cat-room palette and cozy dream-room language.
  - No body art, readable text, watermark, obvious magenta fringe, or neighbor-cell fragments found.
  - P2: `teal_cushion_placement_shadow_pad` reads more like a floor mat/cushion pad; `moonlit_window_light_floor_patch` reads more like a small focal object than a pure light decal; `threshold_mat_doorstep_trim` should stay doorway-only.
- Production QA reviewer Hypatia: `PASS_WITH_P2`.
  - All 12 PNGs passed alpha/edge checks with transparent padding and no nontransparent edge pixels.
  - All PNG dimensions are under 512 px and manifest SHA256 values match current files.
  - P2: semi-transparent decals need scene screenshots on light floor, dark floor, and target cat-room background.

## Accepted Candidate Set

- `warm_wood_floor_tile_patch`
- `soft_oval_prop_shadow_decal`
- `rose_rug_edge_strip`
- `cream_wall_baseboard_straight`
- `cream_wall_baseboard_corner`
- `tiny_paw_print_floor_scuff`
- `small_dream_crack_ground_decal`
- `soft_dust_sparkle_ground_decal`
- `rounded_room_boundary_corner_trim`

## Conditional Or Reclassified Candidate Set

- `teal_cushion_placement_shadow_pad`: reclassify as floor mat / cushion pad, not a generic placement shadow.
- `moonlit_window_light_floor_patch`: use as focal floor light/decal only; if a large-area light overlay is needed, produce a lighter follow-up variant.
- `threshold_mat_doorstep_trim`: use only as doorway/threshold trim, not a generic trim or UI frame.

## Unity Import Notes

- Texture Type: Sprite (2D and UI).
- Sprite Mode: Single.
- Alpha Source: Input; Alpha Is Transparency: On.
- Mip Maps: Off.
- Compression: None.
- Max Size: 512 or higher.
- Candidate PPU: 100, matching current P0 sprite convention.
- Mesh Type: Full Rect for candidate import, especially `soft_oval_prop_shadow_decal`, `moonlit_window_light_floor_patch`, `small_dream_crack_ground_decal`, and `soft_dust_sparkle_ground_decal`.
- Pivot: center for ground decals; bottom-center or custom baseline for rug/threshold/baseboards; custom inner-corner pivot for corner trim after placement testing.
- No colliders and no click hitboxes for shadow/light/dust decals.

## Current Gate

- Candidate package status: complete pending Unity import review.
- No hard P1/FAIL rework item.
- Do not promote Batch93 into `Assets/`.
- Required Unity evidence: import settings, layer/sorting plan, shallow/deep floor screenshots, target cat-room screenshot, prop overlap check, and Console check.
