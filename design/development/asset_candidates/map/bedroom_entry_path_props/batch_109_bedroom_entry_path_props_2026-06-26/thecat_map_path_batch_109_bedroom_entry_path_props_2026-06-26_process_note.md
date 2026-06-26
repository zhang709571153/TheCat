# bedroom_entry_path_props Process Note

Batch: `batch_109_bedroom_entry_path_props_2026-06-26`
Family: `map`
Source truth: `Qr1 UI/style truth revision 816; Qr1 P0 bedroom dream map boundary; Batch101 marker context; Batch102/103 decal context; Batch108 obstacle prop context; no IAd character-body claim; no HDo/FoW9 map-archive claim`

Process: built-in Codex imagegen source generation, workspace copy, local magenta chroma-key alpha removal, projection-based 3x3 sprite splitting, review-variant generation, local 64px readability board generation, independent visual/source-lock/production QA review integration.

Prompt/derivation goal: create static bedroom dream-map entrance and path-hint props only. The batch intentionally excludes character bodies, faces, paws, portraits, animation frames, enemies, Egypt/desert/archive motifs, baked text, and runtime import.

Source and cut outputs:

- Source sheet: `design/development/asset_candidates/map/bedroom_entry_path_props/batch_109_bedroom_entry_path_props_2026-06-26/source/thecat_map_path_batch109_chromakey_source_v001.png`
- Alpha sheet: `design/development/asset_candidates/map/bedroom_entry_path_props/batch_109_bedroom_entry_path_props_2026-06-26/alpha/thecat_map_path_batch109_alpha_sheet_v001.png`
- Names file: `design/development/asset_candidates/map/bedroom_entry_path_props/batch_109_bedroom_entry_path_props_2026-06-26/source/thecat_map_path_batch109_names.txt`
- Manifest: `design/development/asset_candidates/map/bedroom_entry_path_props/batch_109_bedroom_entry_path_props_2026-06-26/thecat_map_path_batch109_semantic_manifest.csv`
- Contact sheet: `design/development/asset_candidates/map/bedroom_entry_path_props/batch_109_bedroom_entry_path_props_2026-06-26/thecat_map_path_batch109_semantic_contact_sheet_v001.png`
- 64px readability board: `design/development/asset_candidates/map/bedroom_entry_path_props/batch_109_bedroom_entry_path_props_2026-06-26/thecat_map_path_batch109_64px_bedroom_entry_path_readability_board_v001.png`
- Review variants: `design/development/asset_candidates/map/bedroom_entry_path_props/batch_109_bedroom_entry_path_props_2026-06-26/reviews/variants/`

Chroma-key and cut results:

- Key: magenta border auto-key, detected near `#f802f7`.
- Transparent pixels: `1028316/1572516`; partially transparent pixels: `237892/1572516`.
- Projection row bands: `(51, 423)`, `(485, 837)`, `(883, 1194)`.
- Projection column bands: `(56, 373)`, `(414, 817)`, `(863, 1221)`.
- Rows: `9` semantic sprites, all with alpha extrema `(0, 255)` and manifest hashes.

Review outcomes:

- Visual/style reviewer: `PASS_WITH_P2`; 5 candidate_keep, 4 candidate_conditional, 0 reject.
- Source-lock/boundary reviewer: `PASS`; no character/body/portrait/paw/face/framesheet, no animation, and no HDo/FoW9 Egypt/archive claim.
- Production QA reviewer: `PASS_WITH_P2`; no P1 blockers after review-trail and metadata-join cleanup.

Gate tokens: `source_lock_boundary_review_complete`; No Unity .meta files were created.

Final candidate decisions:

- `candidate_keep`: `north_doorway_dream_arch_prop`, `side_window_moonbeam_marker`, `floor_path_ribbon_curve`, `sock_trail_path_marker`, `toy_train_path_blocker`.
- `candidate_conditional`: `blanket_curtain_entry_prop`, `alarm_clock_hazard_prop`, `nightlight_safe_glow_marker`, `wall_crack_entry_marker`.
- `reject_rework`: none.

Checklist:

- [x] Asset table written before generation.
- [x] Raw source art copied into `source/`.
- [x] Background removed locally and alpha candidates stored outside runtime folders.
- [x] Manifest/contact sheet and review variants produced.
- [x] Visual/style review completed.
- [x] Source-lock/boundary review completed.
- [x] Production QA review completed.
- [x] Final review CSV reconciled with manifest asset IDs.
- [x] No Unity `.meta` files were created in the candidate folder.
- [x] No runtime import was performed.
- [ ] Unity import settings, binding proof, bedroom map screenshots, sorting/collider proof, and clean console evidence remain pending.

Asset table: `design/development/asset_candidates/map/bedroom_entry_path_props/batch_109_bedroom_entry_path_props_2026-06-26/thecat_map_path_batch_109_bedroom_entry_path_props_2026-06-26_asset_table.csv`
Final review CSV: `design/development/asset_candidates/map/bedroom_entry_path_props/batch_109_bedroom_entry_path_props_2026-06-26/thecat_map_path_batch_109_bedroom_entry_path_props_2026-06-26_final_review.csv`

No runtime import was performed.
