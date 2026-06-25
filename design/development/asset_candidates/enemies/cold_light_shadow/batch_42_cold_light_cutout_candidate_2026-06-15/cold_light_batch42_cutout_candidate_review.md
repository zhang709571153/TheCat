# Cold Light Batch 42 Cutout Candidate Review

Decision: candidate review only; do not import into Unity yet.

Formal Unity import remains blocked until active-enemy Play Mode screenshot review passes.

## Source Authority

- Enemy: `Cold Light Shadow`
- Source concept: `design/梦境支配者核心玩法/assets/enemies/en04_cold_light_shadow/concept/cold_light_shadow_concept.png`
- Source animation: `design/梦境支配者核心玩法/assets/enemies/en04_cold_light_shadow/animation/cold_light_shadow_animation.png`
- Batch 38 review sheet: `design/development/asset_candidates/enemies/p0_core/batch_38_p0_enemy_source_reference_pack_2026-06-15/thecat_enemy_p0_core_batch38_source_reference_review_sheet.png`
- Batch 38 combat crop reference: `design/development/asset_candidates/enemies/p0_core/batch_38_p0_enemy_source_reference_pack_2026-06-15/cold_light_shadow/thecat_enemy_cold_light_shadow_batch38_combat_sprite_reference_512_512x512_candidate_v001.png`
- Batch 41 input candidate review: `design/development/asset_candidates/enemies/cold_light_shadow/batch_41_cold_light_ai_refinement_candidate_2026-06-15/cold_light_batch41_ai_refinement_candidate_review.md`
- Source lock ids: `cold_light_concept;cold_light_animation`
- Active screenshot required before import: `08-active-enemy-cold-light.png`

## Cutout Outputs

- Beam-preserving alpha 1024 candidate: `design/development/asset_candidates/enemies/cold_light_shadow/batch_42_cold_light_cutout_candidate_2026-06-15/thecat_enemy_cold_light_shadow_cutout_beam_alpha_1024_candidate_v001.png`
- Alpha 512 preview: `design/development/asset_candidates/enemies/cold_light_shadow/batch_42_cold_light_cutout_candidate_2026-06-15/thecat_enemy_cold_light_shadow_cutout_beam_alpha_512_preview_v001.png`
- Checkerboard review composite: `design/development/asset_candidates/enemies/cold_light_shadow/batch_42_cold_light_cutout_candidate_2026-06-15/thecat_enemy_cold_light_shadow_cutout_beam_checkerboard_512_review_v001.png`
- Dark-field review composite: `design/development/asset_candidates/enemies/cold_light_shadow/batch_42_cold_light_cutout_candidate_2026-06-15/thecat_enemy_cold_light_shadow_cutout_beam_darkfield_512_review_v001.png`
- Warm-HUD review composite: `design/development/asset_candidates/enemies/cold_light_shadow/batch_42_cold_light_cutout_candidate_2026-06-15/thecat_enemy_cold_light_shadow_cutout_beam_warmfield_512_review_v001.png`
- Alpha mask review: `design/development/asset_candidates/enemies/cold_light_shadow/batch_42_cold_light_cutout_candidate_2026-06-15/thecat_enemy_cold_light_shadow_cutout_beam_alpha_mask_512_review_v001.png`
- Review sheet: `design/development/asset_candidates/enemies/cold_light_shadow/batch_42_cold_light_cutout_candidate_2026-06-15/thecat_enemy_cold_light_batch42_cutout_review_sheet.png`
- Process note: `design/development/asset_candidates/enemies/cold_light_shadow/batch_42_cold_light_cutout_candidate_2026-06-15/cold_light_batch42_cutout_process_note.md`

## Cutout Metrics

- Background RGB sampled from border: `236,219,189`
- Initial foreground pixels: `215624`
- Opaque pixels before matte: `211031`
- Semi-transparent pixels before matte: `4593`
- Cyan semi-transparent pixels before matte: `4593`
- Transparent pixels after matte: `806731`
- Semi-transparent pixels after matte: `17047`
- Opaque pixels after matte: `224798`
- Transparent coverage: `76.94%`
- Semi-transparent coverage: `1.63%`
- Opaque coverage: `21.44%`

## Visual Review

- Pass: output has an alpha channel and transparent corners for Unity sprite review.
- Pass: cold lamp silhouette, cyan beam/light language, mechanical spring arm, black mud base, single red eye, long shadow-limb feel, and ranged-pressure read remain visible.
- Pass: pale cyan beam information is preserved as semi-transparent alpha instead of being fully clipped.
- Watch: beam may be better split into a separate Unity warning VFX later; this batch intentionally keeps it for candidate identity review.
- Watch: final runtime scale, hitbox readability, beam-warning readability, and prefab binding still need active-enemy screenshot review.

## Rejection Rules

- Reject if future cutout iterations clip the cold lamp silhouette, cyan beam/light language, mechanical spring arm, black mud base, single red eye, long shadow-limb feel, or ranged-pressure read.
- Reject ordinary clean desk lamp, warm candle or fire palette, cute robot styling, humanoid body, black mud removal, missing red eye, missing cyan light, missing spring arm, missing mud base, or removal of all beam readability.
- Reject if alpha edges show obvious parchment halos in Unity active-enemy screenshot review.
- Reject if the candidate is imported into Unity before active-enemy screenshot review.
