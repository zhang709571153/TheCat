# Batch 92 Cat Room Interaction States Process Note

Date: 2026-06-25

Mode: built-in Codex `imagegen` plus local chroma-key removal.

## Goal

Produce a narrow candidate-only sprite pack for cat-room interaction state UI pieces, without generating starter-cat body art or baked text. This batch is a focused follow-up to Batch 90 cat-room UI and Batch 91 cat-room map-elements review.

## Files

- `source/thecat_ui_cat_room_interaction_states_batch92_chromakey_source_v001.png`
- `source/thecat_ui_cat_room_interaction_states_batch92_alpha_sheet_v001.png`
- `semantic_sprites/*.png`
- `thecat_ui_cat_room_interaction_states_batch92_semantic_manifest.csv`
- `thecat_ui_cat_room_interaction_states_batch92_final_review.csv`
- `thecat_ui_cat_room_interaction_states_batch92_semantic_contact_sheet_v003.png`

## Cutting Notes

The first equal-grid cut produced neighbor-cell fragments because the generated layout did not strictly follow the nominal 3 by 4 grid. The final sprites were cut using alpha-projection row/column bands, then padded with transparent margins.

All 12 original semantic sprites have alpha extrema `(0, 255)`. The final manifest also records cooldown v002 and v003 rework candidates.

## Review Outcome

Independent reviews found the base package clean enough for candidate use but identified original cooldown v001 as a P1 blocker because it behaved like a heavy grey overlay. Two replacement cooldown sprites were generated:

- v002: lighter but superseded because it retained inner dark scuffs.
- v003: accepted as the current cooldown candidate after independent follow-up review. It reduces visible coverage to 17.39% and center visible coverage to 0.47%.

Final package verdict: `PASS_WITH_P2`, candidate-only pending Unity review.
