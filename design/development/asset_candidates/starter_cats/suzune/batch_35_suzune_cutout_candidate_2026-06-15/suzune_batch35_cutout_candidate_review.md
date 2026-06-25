# Suzune Batch 35 Cutout Candidate Review

Decision: candidate review only; do not import into Unity yet.

formal import remains blocked until active-cat Play Mode screenshot review passes.

## Source Authority

- Colored three-view turnaround: `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`
- Batch 33 review sheet: `design/development/asset_candidates/starter_cats/suzune/batch_33_suzune_strict_turnaround_derivatives_2026-06-14/thecat_cat_suzune_batch33_strict_turnaround_review_sheet.png`
- Batch 34 candidate review: `design/development/asset_candidates/starter_cats/suzune/batch_34_suzune_ai_refinement_candidate_2026-06-14/suzune_batch34_ai_refinement_candidate_review.md`
- Source lock id: `suzune_turnaround_colored`
- Active screenshot required before import: `07-active-cat-suzune.png`

## Cutout Outputs

- Alpha 1024 candidate: `design/development/asset_candidates/starter_cats/suzune/batch_35_suzune_cutout_candidate_2026-06-15/thecat_cat_suzune_cutout_alpha_1024_candidate_v001.png`
- Alpha 512 preview: `design/development/asset_candidates/starter_cats/suzune/batch_35_suzune_cutout_candidate_2026-06-15/thecat_cat_suzune_cutout_alpha_512_preview_v001.png`
- Checkerboard review composite: `design/development/asset_candidates/starter_cats/suzune/batch_35_suzune_cutout_candidate_2026-06-15/thecat_cat_suzune_cutout_checkerboard_512_review_v001.png`
- Alpha mask review: `design/development/asset_candidates/starter_cats/suzune/batch_35_suzune_cutout_candidate_2026-06-15/thecat_cat_suzune_cutout_alpha_mask_512_review_v001.png`
- Review sheet: `design/development/asset_candidates/starter_cats/suzune/batch_35_suzune_cutout_candidate_2026-06-15/thecat_cat_suzune_batch35_cutout_review_sheet.png`
- Process note: `design/development/asset_candidates/starter_cats/suzune/batch_35_suzune_cutout_candidate_2026-06-15/suzune_batch35_cutout_process_note.md`

## Cutout Metrics

- Background RGB sampled from border: `245,237,226`
- Flood threshold: `48`
- Transparent pixels: `723694`
- Opaque pixels: `324882`
- Opaque coverage: `30.98%`

## Visual Review

- Pass: output has an alpha channel and transparent corners for Unity sprite review.
- Pass: core Suzune identity remains inherited from Batch 34: non-human cat body, calico markings, blue eyes, white shrine robe, vermilion skirt and sash, central gold bell, flower ornament, hanging bells, bell wand, blue talismans, snowflake sleeve marks, and calico tail.
- Watch: local flood-fill transparency can leave edge residue or over-soften pale fur and robe edges; verify in Unity against dark and warm HUD fields.
- Watch: this is still a front-view candidate only; side and back anchors remain governed by the colored turnaround and Batch 33 source-derived references.

## Rejection Rules

- Reject if future cutout iterations clip ears, calico face markings, bell wand, flower ornament, hanging bells, talismans, sleeves, tail, paws, or robe ribbons.
- Reject if future iterations introduce human body proportions, long legs, human hands, generic shrine-cat drift, or generic healer costume.
- Reject if alpha edges show obvious parchment halos in Unity active-cat screenshot review.
- Reject if the candidate is imported into Unity before active-cat screenshot review.
