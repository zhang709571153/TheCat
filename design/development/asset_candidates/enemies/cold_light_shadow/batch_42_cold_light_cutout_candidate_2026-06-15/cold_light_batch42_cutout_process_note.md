# Cold Light Batch 42 Cutout Process Note

This batch is a deterministic local post-processing pass over the Batch 41 Cold Light Shadow candidate. No new image generation was invoked.

- Source concept authority: `design/梦境支配者核心玩法/assets/enemies/en04_cold_light_shadow/concept/cold_light_shadow_concept.png`
- Source animation authority: `design/梦境支配者核心玩法/assets/enemies/en04_cold_light_shadow/animation/cold_light_shadow_animation.png`
- Input candidate: `design/development/asset_candidates/enemies/cold_light_shadow/batch_41_cold_light_ai_refinement_candidate_2026-06-15/thecat_enemy_cold_light_shadow_ai_refinement_combat_1024_candidate_v001.png`
- Output directory: `design/development/asset_candidates/enemies/cold_light_shadow/batch_42_cold_light_cutout_candidate_2026-06-15`
- Method: sample the 16-pixel image border, keep dark metal/mud and red-eye pixels opaque, preserve pale cyan lamp and beam pixels as semi-transparent alpha, then apply a 0.55px soft matte.
- Background RGB: `236,219,189`
- Opaque coverage: `21.44%`
- Semi-transparent coverage: `1.63%`
- Transparent coverage: `76.94%`
- Safety: files remain outside `Assets`; formal import remains blocked until active-enemy Play Mode screenshot review passes.
