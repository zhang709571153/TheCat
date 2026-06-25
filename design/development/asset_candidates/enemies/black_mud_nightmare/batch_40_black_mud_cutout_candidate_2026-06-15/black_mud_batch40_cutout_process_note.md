# Black Mud Batch 40 Cutout Process Note

This batch is a deterministic local post-processing pass over the Batch 39 Black Mud Nightmare candidate. No new image generation was invoked.

- Source concept authority: `design/梦境支配者核心玩法/assets/enemies/en01_black_mud_nightmare/concept/black_mud_nightmare_concept.png`
- Source animation authority: `design/梦境支配者核心玩法/assets/enemies/en01_black_mud_nightmare/animation/black_mud_nightmare_animation.png`
- Input candidate: `design/development/asset_candidates/enemies/black_mud_nightmare/batch_39_black_mud_ai_refinement_candidate_2026-06-15/thecat_enemy_black_mud_nightmare_ai_refinement_combat_1024_candidate_v001.png`
- Output directory: `design/development/asset_candidates/enemies/black_mud_nightmare/batch_40_black_mud_cutout_candidate_2026-06-15`
- Method: sample the 16-pixel image border, identify dark-mud and red-eye foreground pixels that differ from the parchment background, remove tiny specks, and apply a 0.7px soft matte.
- Background RGB: `245,232,209`
- Luma threshold: `205`
- Background distance minimum: `58`
- Opaque coverage: `31.17%`
- Safety: files remain outside `Assets`; formal import remains blocked until active-enemy Play Mode screenshot review passes.
