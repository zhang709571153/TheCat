# Batch 91 Cat Room Map Elements Imagegen Process Note

Date: 2026-06-25

Scope: first visible built-in `imagegen` production pass for cat-room map elements.

## Source

- Built-in Codex `imagegen` mode, not CLI/API fallback.
- No `OPENAI_API_KEY` required for this pass.
- Output copied from the Codex default generated-image folder into this batch.
- Prompt requested a chroma-key sprite source sheet with no characters, no text, and separated cat-room map elements.

## Produced Files

- `source/thecat_map_cat_room_elements_batch91_chromakey_source_v001.png`
- `source/thecat_map_cat_room_elements_batch91_alpha_sheet_v001.png`
- `source/thecat_map_cat_room_dream_entrance_portal_batch91_chromakey_source_v002.png`
- `source/thecat_map_cat_room_dream_entrance_portal_batch91_alpha_v002.png`
- `source/thecat_map_cat_room_dream_entrance_portal_batch91_chromakey_source_v003.png`
- `source/thecat_map_cat_room_dream_entrance_portal_batch91_alpha_v003.png`
- `source/thecat_map_cat_room_dream_entrance_portal_batch91_magenta_source_v004.png`
- `source/thecat_map_cat_room_dream_entrance_portal_batch91_alpha_v004.png`
- `sprites/thecat_map_cat_room_elements_batch91_prop_01_candidate_v001.png` through `sprites/thecat_map_cat_room_elements_batch91_prop_12_candidate_v001.png`
- `sprites/thecat_map_cat_room_dream_entrance_portal_1037x1151_candidate_v002.png`
- `sprites/thecat_map_cat_room_dream_entrance_portal_1037x1151_clean_candidate_v002.png` - rejected cleanup attempt; do not use.
- `sprites/thecat_map_cat_room_dream_entrance_portal_1153x1235_candidate_v003.png` - rejected green chroma-key attempt; do not use.
- `sprites/thecat_map_cat_room_dream_entrance_portal_1178x1269_candidate_v004.png` - magenta chroma-key replacement candidate pending independent review.
- `sprites/thecat_map_cat_room_dream_entrance_portal_1194x1314_import_test_candidate_v004.png` - rejected soft-halo attempt; do not use.
- `sprites/thecat_map_cat_room_dream_entrance_portal_1194x1313_alpha_safe_candidate_v004.png` - rejected soft-halo attempt; do not use.
- `sprites/thecat_map_cat_room_dream_entrance_portal_1192x1313_hard_alpha_candidate_v004.png` - accepted semantic/import-test replacement for `prop_04`, pending Unity import review.
- `semantic_sprites/*.png` - 12 semantic candidate copies for Unity review.
- `thecat_map_cat_room_elements_batch91_semantic_manifest.csv`
- `thecat_map_cat_room_elements_batch91_semantic_contact_sheet_v002.png`
- `thecat_map_cat_room_elements_batch91_cutout_contact_sheet_v001.png`
- `thecat_map_cat_room_elements_batch91_manifest.csv`
- `thecat_map_cat_room_elements_batch91_semantic_review.csv`

## Cutout Method

- Removed the flat green background with the installed imagegen chroma-key helper.
- Detected alpha connected components from the alpha sheet and exported 12 padded sprite cutouts.
- Current sprite names are generic `prop_01` through `prop_12`; semantic names remain pending visual review.

## Current Gate

- Status: `PASS_WITH_P1` after independent visual and production QA review.
- 11/12 original cutouts may proceed to semantic candidate review.
- Original `prop_04` is blocked before final semantic naming because the portal opening contains visible dark scratch/dash artifacts; it is replaced in the semantic pack by the v004 hard-alpha portal candidate.
- v002 portal replacement is generated and cut out but still pending independent acceptance.
- v003 portal replacement is rejected because green chroma-key conflicted with the teal mist and produced alpha holes.
- v004 portal replacement uses magenta chroma-key to avoid teal-mist loss.
- v004 original cutout passed visual review with `PASS_WITH_P2` and production QA with `PASS_WITH_P1`.
- v004 hard-alpha import-test candidate fixes the QA-noted bottom padding and semi-transparent fringe risk enough for semantic candidate review; Unity import settings remain required.
- Do not promote into `Assets/` yet.
- Do not treat this as starter-cat body art or character sequence-frame production.
