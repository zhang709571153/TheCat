# bedroom_map_overlay_controls Process Note

Batch: `batch_110_bedroom_map_overlay_controls_2026-06-26`
Family: `ui`
Source truth: `Qr1 UI/style truth revision 816; Qr1 P0 bedroom dream map boundary; Batch101 marker context; Batch108 obstacle context; Batch109 entry/path context; no IAd character-body claim; no HDo/FoW9 map-archive claim`

Process: built-in Codex imagegen source generation, workspace source copy, local magenta chroma-key alpha removal, projection-based 3x3 semantic split, review variant generation, and 64px readability-board generation.

Prompt goal: create nine textless, symbolic bedroom dream-map overlay UI sprites in Qr1-style cozy moonlit UI language. The batch intentionally avoids character bodies, cat faces, paws, portraits, animation frames, Egypt/desert/archive motifs, and any HDo/FoW9 folder coverage claim.

Source and alpha results:

- Source: `source/thecat_ui_mapovr_batch110_chromakey_source_v001.png`
- Alpha sheet: `alpha/thecat_ui_mapovr_batch110_alpha_sheet_v001.png`
- Auto-detected key color: `#fa02f9`
- Transparent pixels: `963868/1572516`
- Partially transparent pixels: `22981/1572516`
- Semantic split: `3x3`, names from `source/thecat_ui_mapovr_batch110_names.txt`, short file batch id `batch110` used to avoid Windows path-length failures.
- Projection bands: rows `(70,389)`, `(466,802)`, `(872,1183)`; columns `(74,373)`, `(471,771)`, `(811,1192)`.
- Manifest: `thecat_ui_mapovr_batch110_semantic_manifest.csv`
- Contact sheet: `thecat_ui_mapovr_batch110_semantic_contact_sheet_v001.png`
- 64px readability board: `thecat_ui_mapovr_batch110_64px_bedroom_map_overlay_readability_board_v001.png`
- Review variants: `reviews/variants/` with 5 variants per sprite. The info-foldout variants use the shortened prefix `tc_ui_mapovr_info_foldout_batch110_v001` because the full semantic filename exceeded stable Windows path length.
- Superseded cleanup: duplicate full-prefix info-foldout alpha preview from the failed long-path variant attempt was moved to `superseded/path_length_duplicate_variant/info_foldout_duplicate_alpha_preview_512.png`.

Checklist:

- [x] Asset table written before generation.
- [x] Raw source art copied into `source/`.
- [x] Background removed locally and alpha candidates stored outside runtime folders.
- [x] Manifest/contact sheet and review variants produced.
- [x] 64px readability board produced.
- [x] Visual/style review completed by Kepler: `PASS_WITH_P2`.
- [x] Source-lock boundary review completed by Ptolemy: `PASS_WITH_P2`.
- [x] Production QA review completed by Wegener: `PASS_WITH_P2`.
- [ ] Runtime import remains blocked until import settings, binding proof, screenshots, and clean console evidence pass.

Asset table: `design/development/asset_candidates/ui/bedroom_map_overlay_controls/batch_110_bedroom_map_overlay_controls_2026-06-26/thecat_ui_mapovr_batch_110_bedroom_map_overlay_controls_2026-06-26_asset_table.csv`
Final review CSV: `design/development/asset_candidates/ui/bedroom_map_overlay_controls/batch_110_bedroom_map_overlay_controls_2026-06-26/thecat_ui_mapovr_batch_110_bedroom_map_overlay_controls_2026-06-26_final_review.csv`

Local QA:

- Semantic sprites: 9 transparent PNGs.
- Candidate package PNG count: 59 after source, alpha, sprites, contact sheet, readability board, and review variants.
- Unity `.meta` files in candidate package: 0.
- Runtime `Assets/` leak for `batch110` or `mapovr`: 0.

No runtime import was performed.
