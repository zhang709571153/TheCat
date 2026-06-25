# Call Tyrant Batch 44 Cutout Candidate Review

Decision: candidate review only; do not import into Unity yet.

Formal Unity import remains blocked until active-enemy Play Mode screenshot review passes.

## Source Authority

- Enemy: `Call Tyrant`
- Source concept: `design/梦境支配者核心玩法/assets/enemies/en06_call_tyrant/concept/call_tyrant_concept.png`
- Source animation: `design/梦境支配者核心玩法/assets/enemies/en06_call_tyrant/animation/call_tyrant_animation.png`
- Batch 38 review sheet: `design/development/asset_candidates/enemies/p0_core/batch_38_p0_enemy_source_reference_pack_2026-06-15/thecat_enemy_p0_core_batch38_source_reference_review_sheet.png`
- Batch 38 combat crop reference: `design/development/asset_candidates/enemies/p0_core/batch_38_p0_enemy_source_reference_pack_2026-06-15/call_tyrant/thecat_enemy_call_tyrant_batch38_combat_sprite_reference_512_512x512_candidate_v001.png`
- Batch 43 input candidate review: `design/development/asset_candidates/enemies/call_tyrant/batch_43_call_tyrant_ai_refinement_candidate_2026-06-15/call_tyrant_batch43_ai_refinement_candidate_review.md`
- Source lock ids: `call_tyrant_concept;call_tyrant_animation`
- Active screenshot required before import: `09-active-enemy-call-tyrant.png`

## Cutout Outputs

- Boss alpha 1024 candidate: `design/development/asset_candidates/enemies/call_tyrant/batch_44_call_tyrant_cutout_candidate_2026-06-15/thecat_enemy_call_tyrant_cutout_boss_alpha_1024_candidate_v001.png`
- Alpha 512 preview: `design/development/asset_candidates/enemies/call_tyrant/batch_44_call_tyrant_cutout_candidate_2026-06-15/thecat_enemy_call_tyrant_cutout_boss_alpha_512_preview_v001.png`
- Checkerboard review composite: `design/development/asset_candidates/enemies/call_tyrant/batch_44_call_tyrant_cutout_candidate_2026-06-15/thecat_enemy_call_tyrant_cutout_boss_checkerboard_512_review_v001.png`
- Dark-field review composite: `design/development/asset_candidates/enemies/call_tyrant/batch_44_call_tyrant_cutout_candidate_2026-06-15/thecat_enemy_call_tyrant_cutout_boss_darkfield_512_review_v001.png`
- Warm-HUD review composite: `design/development/asset_candidates/enemies/call_tyrant/batch_44_call_tyrant_cutout_candidate_2026-06-15/thecat_enemy_call_tyrant_cutout_boss_warmfield_512_review_v001.png`
- Alpha mask review: `design/development/asset_candidates/enemies/call_tyrant/batch_44_call_tyrant_cutout_candidate_2026-06-15/thecat_enemy_call_tyrant_cutout_boss_alpha_mask_512_review_v001.png`
- Review sheet: `design/development/asset_candidates/enemies/call_tyrant/batch_44_call_tyrant_cutout_candidate_2026-06-15/thecat_enemy_call_tyrant_batch44_cutout_review_sheet.png`
- Process note: `design/development/asset_candidates/enemies/call_tyrant/batch_44_call_tyrant_cutout_candidate_2026-06-15/call_tyrant_batch44_cutout_process_note.md`

## Cutout Metrics

- Background RGB sampled from border: `229,211,178`
- Initial foreground pixels: `471160`
- Dark phone/mud pixels before matte: `461491`
- Red call-eye pixels before matte: `7099`
- Purple tie pixels before matte: `2035`
- App projectile pixels before matte: `279`
- Throw streak pixels before matte: `256`
- Transparent pixels after matte: `524388`
- Semi-transparent pixels after matte: `21585`
- Opaque pixels after matte: `502603`
- Transparent coverage: `50.01%`
- Semi-transparent coverage: `2.06%`
- Opaque coverage: `47.93%`

## Visual Review

- Pass: output has an alpha channel and transparent corners for Unity sprite review.
- Pass: giant phone shell, red call-eye signal, purple tie, black mud body and base, app projectile language, tiny phone minions, and Boss-scale silhouette remain visible.
- Pass: app projectiles are preserved as opaque colored shapes while throw streaks are retained as semi-transparent evidence.
- Watch: app projectiles and throw arcs may be better split into separate Unity warning VFX later; this batch intentionally keeps them for candidate identity review.
- Watch: final runtime scale, hitbox readability, summon readability, app-throw readability, and prefab binding still need active-enemy screenshot review.

## Rejection Rules

- Reject if future cutout iterations clip the giant phone shell, red call-eye signal, purple tie, black mud body and base, app projectile language, summon portal/minion vibration feel, Boss-scale silhouette, cracked glass screen, or phone-call nightmare identity.
- Reject human office boss body, generic smartphone icon mascot, cute robot styling, clean ordinary phone, brand logos, readable text, keyboard, laptop, alarm/lamp/toy motifs, black mud removal, missing purple tie, missing red call eyes, missing cracked phone shell, or missing app projectile language.
- Reject if alpha edges show obvious parchment halos in Unity active-enemy screenshot review.
- Reject if the candidate is imported into Unity before active-enemy screenshot review.
