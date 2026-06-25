# Black Mud Batch 40 Cutout Candidate Review

Decision: candidate review only; do not import into Unity yet.

Formal Unity import remains blocked until active-enemy Play Mode screenshot review passes.

## Source Authority

- Enemy: `Black Mud Nightmare`
- Source concept: `design/梦境支配者核心玩法/assets/enemies/en01_black_mud_nightmare/concept/black_mud_nightmare_concept.png`
- Source animation: `design/梦境支配者核心玩法/assets/enemies/en01_black_mud_nightmare/animation/black_mud_nightmare_animation.png`
- Batch 38 review sheet: `design/development/asset_candidates/enemies/p0_core/batch_38_p0_enemy_source_reference_pack_2026-06-15/thecat_enemy_p0_core_batch38_source_reference_review_sheet.png`
- Batch 39 candidate review: `design/development/asset_candidates/enemies/black_mud_nightmare/batch_39_black_mud_ai_refinement_candidate_2026-06-15/black_mud_batch39_ai_refinement_candidate_review.md`
- Source lock ids: `black_mud_concept;black_mud_animation`
- Active screenshot required before import: `07-active-enemy-black-mud.png`

## Cutout Outputs

- Alpha 1024 candidate: `design/development/asset_candidates/enemies/black_mud_nightmare/batch_40_black_mud_cutout_candidate_2026-06-15/thecat_enemy_black_mud_nightmare_cutout_alpha_1024_candidate_v001.png`
- Alpha 512 preview: `design/development/asset_candidates/enemies/black_mud_nightmare/batch_40_black_mud_cutout_candidate_2026-06-15/thecat_enemy_black_mud_nightmare_cutout_alpha_512_preview_v001.png`
- Checkerboard review composite: `design/development/asset_candidates/enemies/black_mud_nightmare/batch_40_black_mud_cutout_candidate_2026-06-15/thecat_enemy_black_mud_nightmare_cutout_checkerboard_512_review_v001.png`
- Dark-field review composite: `design/development/asset_candidates/enemies/black_mud_nightmare/batch_40_black_mud_cutout_candidate_2026-06-15/thecat_enemy_black_mud_nightmare_cutout_darkfield_512_review_v001.png`
- Alpha mask review: `design/development/asset_candidates/enemies/black_mud_nightmare/batch_40_black_mud_cutout_candidate_2026-06-15/thecat_enemy_black_mud_nightmare_cutout_alpha_mask_512_review_v001.png`
- Review sheet: `design/development/asset_candidates/enemies/black_mud_nightmare/batch_40_black_mud_cutout_candidate_2026-06-15/thecat_enemy_black_mud_batch40_cutout_review_sheet.png`
- Process note: `design/development/asset_candidates/enemies/black_mud_nightmare/batch_40_black_mud_cutout_candidate_2026-06-15/black_mud_batch40_cutout_process_note.md`

## Cutout Metrics

- Background RGB sampled from border: `245,232,209`
- Luma threshold: `205`
- Background distance minimum: `58`
- Initial foreground pixels: `327008`
- Transparent pixels: `721780`
- Opaque pixels: `326796`
- Opaque coverage: `31.17%`

## Visual Review

- Pass: output has an alpha channel and transparent corners for Unity sprite review.
- Pass: black sludge body, red eyes, soft-mud monster silhouette, glossy pooled mud, sleepy face imprint, top drip, and low crawling shape remain intact.
- Watch: local foreground masking can over-trim pale ground shadows or small mud droplets; verify in Unity against dark and warm HUD fields.
- Watch: final runtime scale, hitbox readability, bed-contact threat read, and animation identity still need active-enemy screenshot review.

## Rejection Rules

- Reject if future cutout iterations clip the black sludge body, red eyes, soft-mud monster silhouette, crawling pressure, bed-contact threat, glossy pooled mud, sleepy face imprint, top drip, low squat shape, or puddled crawl edges.
- Reject cute pet styling, generic ghost shape, humanoid body, gore, realistic horror anatomy, extra dream interruption objects, cat features, or palette drift.
- Reject if alpha edges show obvious parchment halos in Unity active-enemy screenshot review.
- Reject if the candidate is imported into Unity before active-enemy screenshot review.
