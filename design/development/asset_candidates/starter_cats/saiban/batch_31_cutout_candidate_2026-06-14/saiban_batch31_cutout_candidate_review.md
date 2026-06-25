# Saiban Batch 31 Cutout Candidate Review

Decision: candidate review only; do not import into Unity yet.

formal import remains blocked until active-cat Play Mode screenshot review passes.

## Source Authority

- Colored three-view turnaround: `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- Batch 29 review sheet: `design/development/asset_candidates/starter_cats/saiban/batch_29_strict_turnaround_derivatives_2026-06-14/thecat_cat_saiban_batch29_strict_turnaround_review_sheet.png`
- Batch 30 candidate review: `design/development/asset_candidates/starter_cats/saiban/batch_30_ai_refinement_candidate_2026-06-14/saiban_batch30_ai_refinement_candidate_review.md`
- Source lock id: `saiban_turnaround_colored`
- Active screenshot required before import: `05-active-cat-saiban.png`

## Cutout Outputs

- Alpha 1024 candidate: `design/development/asset_candidates/starter_cats/saiban/batch_31_cutout_candidate_2026-06-14/thecat_cat_saiban_cutout_alpha_1024_candidate_v001.png`
- Alpha 512 preview: `design/development/asset_candidates/starter_cats/saiban/batch_31_cutout_candidate_2026-06-14/thecat_cat_saiban_cutout_alpha_512_preview_v001.png`
- Checkerboard review composite: `design/development/asset_candidates/starter_cats/saiban/batch_31_cutout_candidate_2026-06-14/thecat_cat_saiban_cutout_checkerboard_512_review_v001.png`
- Alpha mask review: `design/development/asset_candidates/starter_cats/saiban/batch_31_cutout_candidate_2026-06-14/thecat_cat_saiban_cutout_alpha_mask_512_review_v001.png`
- Review sheet: `design/development/asset_candidates/starter_cats/saiban/batch_31_cutout_candidate_2026-06-14/thecat_cat_saiban_batch31_cutout_review_sheet.png`
- Process note: `design/development/asset_candidates/starter_cats/saiban/batch_31_cutout_candidate_2026-06-14/saiban_batch31_cutout_process_note.md`

## Cutout Metrics

- Background RGB sampled from border: `250,244,234`
- Flood threshold: `52`
- Transparent pixels: `695333`
- Opaque pixels: `353243`
- Opaque coverage: `33.69%`

## Visual Review

- Pass: output has an alpha channel and transparent corners for Unity sprite review.
- Pass: core Saiban identity remains inherited from Batch 30: non-human cat, shield, sword, tabby face, red cape, silver armor, gold trim, and blue gems.
- Watch: local flood-fill transparency can leave edge residue or over-soften pale fur; verify in Unity against dark and warm HUD fields.
- Watch: this is still a front-view candidate only; side and back anchors remain governed by the colored turnaround and Batch 29 source-derived references.

## Rejection Rules

- Reject if future cutout iterations clip ears, shield, sword, cape, tail, paws, whiskers, or tabby face markings.
- Reject if future iterations introduce human body proportions, long legs, human hands, or generic knight-cat drift.
- Reject if alpha edges show obvious parchment halos in Unity active-cat screenshot review.
- Reject if the candidate is imported into Unity before active-cat screenshot review.
