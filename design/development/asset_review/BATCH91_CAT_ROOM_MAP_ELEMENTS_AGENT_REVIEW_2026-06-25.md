# Batch 91 Cat Room Map Elements Agent Review

Date: 2026-06-25

Scope: `design/development/asset_candidates/map/cat_room_elements/batch_91_cat_room_map_elements_imagegen_2026-06-25`

## Verdict

`PASS_WITH_P1`

Batch 91 is the first visible built-in Codex `imagegen` cat-room map-elements pass. It produced a chroma-key source sheet, alpha sheet, 12 automatic cutouts, contact sheet, manifest, and process note.

Keep 11/12 original cutouts moving into semantic candidate review. Block `prop_04` before final semantic naming or Unity import because the dream-entrance portal contains visible dark scratch/dash artifacts inside the lower portal opening.

## Review Inputs

- Visual/style agent Gibbs: `PASS_WITH_P2`
- Production QA agent Euler: `PASS_WITH_P1`
- Manual visual check: `prop_04` artifact confirmed.
- Built-in imagegen v002 replacement portal generated and cut out as an alternate candidate, but not accepted.
- Built-in imagegen v003 replacement used green chroma-key and is rejected because teal mist conflicted with the key.
- Built-in imagegen v004 replacement used magenta chroma-key, passed visual review with `PASS_WITH_P2`, passed production QA with `PASS_WITH_P1`, and was converted into a hard-alpha import-test candidate.
- Final semantic pack review Carson: `PASS_WITH_P2`.

## Accepted For Semantic Candidate Review

- `prop_01`: cat bed / sleep platform.
- `prop_02`: litter station.
- `prop_03`: feeder and water station.
- `prop_05`: cat-face rug.
- `prop_06` through `prop_09`: interaction ring variants; group as affordance/VFX markers, not ordinary room props.
- `prop_10`: heart cushion.
- `prop_11`: teal star cushion.
- `prop_12`: yarn cushion.

## Blocked Or Pending

- `prop_04`: blocked P1. Visible dark marks inside the portal lower opening.
- `thecat_map_cat_room_dream_entrance_portal_1192x1313_hard_alpha_candidate_v004.png`: accepted as the replacement semantic/import-test portal candidate, pending Unity scale and import review.
- `thecat_map_cat_room_dream_entrance_portal_1037x1151_candidate_v002.png`: replacement candidate generated with built-in imagegen and chroma-key cutout; superseded by v004.
- `thecat_map_cat_room_dream_entrance_portal_1037x1151_clean_candidate_v002.png`: local cleanup attempt over-corrected the lower glow area; do not use.
- `thecat_map_cat_room_dream_entrance_portal_1153x1235_candidate_v003.png`: green chroma-key conflicted with teal portal mist and produced dark alpha holes; do not use.
- `thecat_map_cat_room_dream_entrance_portal_1194x1314_import_test_candidate_v004.png`: fixed padding but retained visible soft halo; do not use.
- `thecat_map_cat_room_dream_entrance_portal_1194x1313_alpha_safe_candidate_v004.png`: reduced dark fringe but retained visible grey/pink halo; do not use.

## P2 Watch Items

- Interaction rings are close variants and skew neon yellow. Treat them as a grouped affordance/VFX family and check color dominance in Unity.
- The accepted v004 dream portal is visually dominant. Scale it down or reserve it as the dream entrance focal prop; do not let it overpower the quieter cat-room UI shell.
- Normalize the portal scale and pivot during Unity import.
- Decide later whether all four near-identical interaction ring variants are needed or whether the set should be curated down.
- Manifest is candidate-grade only. Before import it needs semantic role, pivot/anchor, PPU/scale, sorting intent, variant group, QA status, import settings, binding proof, and Console evidence.

## Current Gate

- Safe to proceed with semantic naming for 12 cutouts using `thecat_map_cat_room_dream_entrance_portal_1192x1313_hard_alpha_candidate_v004.png` as the portal replacement.
- Use `thecat_map_cat_room_elements_batch91_semantic_manifest.csv` and `thecat_map_cat_room_elements_batch91_semantic_contact_sheet_v002.png` as the current Batch91 candidate package entry points.
- Do not promote Batch91 into `Assets/`.
- Candidate package status: complete pending Unity import review.
- Unity import remains required before using these as runtime props.
