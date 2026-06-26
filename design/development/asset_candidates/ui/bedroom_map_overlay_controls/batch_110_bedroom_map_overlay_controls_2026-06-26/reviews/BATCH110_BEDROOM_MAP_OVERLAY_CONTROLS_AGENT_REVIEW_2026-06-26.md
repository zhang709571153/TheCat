# Batch110 Bedroom Map Overlay Controls Agent Review

Decision: `candidate_complete_pending_unity_review`.

Verdict: `PASS_WITH_P2`.

Scope: static, textless, symbolic UI/map overlay sprites for the bedroom dream-map surface. This batch is not character art, not animation, and not a runtime import.

## Inputs

- Candidate folder: `design/development/asset_candidates/ui/bedroom_map_overlay_controls/batch_110_bedroom_map_overlay_controls_2026-06-26`
- Source: `source/thecat_ui_mapovr_batch110_chromakey_source_v001.png`
- Alpha sheet: `alpha/thecat_ui_mapovr_batch110_alpha_sheet_v001.png`
- Manifest: `thecat_ui_mapovr_batch110_semantic_manifest.csv`
- Contact sheet: `thecat_ui_mapovr_batch110_semantic_contact_sheet_v001.png`
- 64px board: `thecat_ui_mapovr_batch110_64px_bedroom_map_overlay_readability_board_v001.png`
- Final review CSV: `thecat_ui_mapovr_batch_110_bedroom_map_overlay_controls_2026-06-26_final_review.csv`

## Agent Results

- Kepler visual/style review: `PASS_WITH_P2`. The set fits Qr1 revision 816 symbolic UI style and is readable at 64px. P2: `map_legend_entry_token` and `map_overlay_pin_available` both rely on star/diamond language, so they need context or motif refinement before runtime promotion.
- Ptolemy source-boundary review: `PASS_WITH_P2`. No character bodies, cat faces, paws, portraits, costumes, animation frames, Egypt/desert/archive coverage, or HDo/FoW9 import claims. P2 process-note scaffold wording has been fixed in this integrated pass.
- Wegener production QA: `PASS_WITH_P2`. All 9 semantic PNGs exist, are RGBA, have matching dimensions and SHA-256 hashes, clean `(0, 255)` alpha, transparent corners/outer edges, and no detected magenta fringe. No `.meta` files and no `Assets/` leak. P2 manifest path convention has been normalized to `design/development/...` and the duplicate full-prefix info-foldout alpha preview was moved to `superseded/` in this integrated pass.

## Candidate Decisions

`candidate_keep`:

- `map_legend_path_token`
- `map_legend_obstacle_token`
- `map_legend_safe_zone_token`
- `map_legend_hazard_token`
- `map_overlay_pin_selected`
- `map_overlay_pin_locked`
- `map_overlay_info_foldout_frame`

`candidate_conditional`:

- `map_legend_entry_token`: clean and readable, but star/diamond semantics can collide with available pin. Keep only with clear legend labeling or rework toward entry/door/threshold motif before runtime promotion.
- `map_overlay_pin_available`: readable as a pin, but shares inner star family with entry and selected states. Keep if the overlay state language makes plain blue pin equal available.

Rejected candidates: none.

## Production Notes

- Source art was generated with built-in Codex imagegen and copied into the workspace candidate folder.
- Chroma-key removal was local; auto key was `#fa02f9`.
- Semantic split used 3x3 projection bands and the names file in `source/`.
- Review variants exist for all 9 sprites; the info-foldout variants use the shortened prefix `tc_ui_mapovr_info_foldout_batch110_v001` to avoid Windows path-length failures.
- No runtime import was performed.
- No Unity `.meta` files were produced in the candidate batch.
- No Batch110/map-overlay files were copied into `Assets/`.

## Runtime Gate

Runtime import remains blocked until:

- Unity import settings are explicit.
- UI binding proof exists.
- Bedroom map overlay screenshots show legend, pin states, and foldout frame in context.
- Entry-vs-available and selected-vs-available ambiguity is resolved at target scale.
- Clean Console evidence is captured.
