# bedroom_obstacle_props Process Note

Batch: `batch_108_bedroom_obstacle_props_2026-06-26`
Family: `map`
Source truth: `Qr1 UI/style truth revision 816; Qr1 P0 bedroom dream map boundary; Batch101/102/103 local bedroom-map context; no IAd character-body claim; no HDo/FoW9 map-archive claim`

Process: built-in Codex imagegen source sheet, workspace source copy, local magenta chroma-key alpha removal, projection-based 3x3 sprite split, review variant generation, and 64px target-ground readability board.

Prompt or derivation goal: produce a small static bedroom dream map obstacle/prop family without character bodies, portraits, animation frames, enemy bodies, Egypt archive coverage, or runtime replacement claims.

Generated source: `design/development/asset_candidates/map/bedroom_obstacle_props/batch_108_bedroom_obstacle_props_2026-06-26/source/thecat_map_obst_batch108_chromakey_source_v001.png`

Alpha sheet: `design/development/asset_candidates/map/bedroom_obstacle_props/batch_108_bedroom_obstacle_props_2026-06-26/alpha/thecat_map_obst_batch108_alpha_sheet_v001.png`

Manifest: `design/development/asset_candidates/map/bedroom_obstacle_props/batch_108_bedroom_obstacle_props_2026-06-26/thecat_map_obst_batch108_semantic_manifest.csv`

Contact sheet: `design/development/asset_candidates/map/bedroom_obstacle_props/batch_108_bedroom_obstacle_props_2026-06-26/thecat_map_obst_batch108_semantic_contact_sheet_v001.png`

64px readability board: `design/development/asset_candidates/map/bedroom_obstacle_props/batch_108_bedroom_obstacle_props_2026-06-26/thecat_map_obst_batch108_64px_bedroom_obstacle_readability_board_v001.png`

Review variants: `design/development/asset_candidates/map/bedroom_obstacle_props/batch_108_bedroom_obstacle_props_2026-06-26/reviews/variants/`

Chroma-key and cut results:

- Source key sampled as `#f903fa`.
- `remove_chroma_key.py` wrote the alpha sheet with 1026255 transparent pixels and 160212 partially transparent pixels.
- `sprite_batch_tools.py split-sheet` wrote 9 semantic sprites using projection bands: rows `(83,394)`, `(491,788)`, `(861,1197)` and columns `(46,408)`, `(472,803)`, `(847,1210)`.
- Current self-triage: 7 `candidate_keep` and 2 `candidate_conditional` before independent review. `nightstand_corner_prop` needs furniture-vs-obstacle scale proof; `moon_dust_patch_prop` needs decal contrast and collider exclusion proof.

Checklist:

- [x] Asset table written before generation.
- [x] Raw source art copied into `source/`.
- [x] Background removed locally and alpha candidates stored outside runtime folders.
- [x] Manifest/contact sheet and review variants produced.
- [x] 64px target-ground readability board produced.
- [ ] Independent visual/style review completed.
- [ ] Independent source-lock boundary review completed.
- [ ] Independent production QA review completed.
- [ ] Runtime import remains blocked until import settings, binding proof, screenshots, and clean console evidence pass.

Asset table: `design/development/asset_candidates/map/bedroom_obstacle_props/batch_108_bedroom_obstacle_props_2026-06-26/thecat_map_obst_batch_108_bedroom_obstacle_props_2026-06-26_asset_table.csv`
Final review CSV: `design/development/asset_candidates/map/bedroom_obstacle_props/batch_108_bedroom_obstacle_props_2026-06-26/thecat_map_obst_batch_108_bedroom_obstacle_props_2026-06-26_final_review.csv`

No runtime import was performed.

No Unity .meta files were intentionally created for this candidate batch.
