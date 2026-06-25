# Call Tyrant Batch 44 Cutout Process Note

This batch is a deterministic local post-processing pass over the Batch 43 Call Tyrant candidate. No new image generation was invoked.

- Source concept authority: `design/梦境支配者核心玩法/assets/enemies/en06_call_tyrant/concept/call_tyrant_concept.png`
- Source animation authority: `design/梦境支配者核心玩法/assets/enemies/en06_call_tyrant/animation/call_tyrant_animation.png`
- Input candidate: `design/development/asset_candidates/enemies/call_tyrant/batch_43_call_tyrant_ai_refinement_candidate_2026-06-15/thecat_enemy_call_tyrant_ai_refinement_combat_1024_candidate_v001.png`
- Output directory: `design/development/asset_candidates/enemies/call_tyrant/batch_44_call_tyrant_cutout_candidate_2026-06-15`
- Method: sample the 16-pixel image border, keep dark phone/mud, red call-eye, purple tie, and saturated app projectile pixels opaque, preserve throw streak pixels as semi-transparent alpha, then apply a 0.65px soft matte.
- Background RGB: `229,211,178`
- Opaque coverage: `47.93%`
- Semi-transparent coverage: `2.06%`
- Transparent coverage: `50.01%`
- Safety: files remain outside `Assets`; formal import remains blocked until active-enemy Play Mode screenshot review passes.
