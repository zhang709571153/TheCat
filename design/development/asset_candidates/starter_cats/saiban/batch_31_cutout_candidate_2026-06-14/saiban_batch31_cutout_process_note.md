# Saiban Batch 31 Cutout Process Note

This batch is a deterministic local post-processing pass over the Batch 30 Saiban candidate. No new image generation was invoked.

- Source turnaround authority: `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- Input candidate: `design/development/asset_candidates/starter_cats/saiban/batch_30_ai_refinement_candidate_2026-06-14/thecat_cat_saiban_ai_refinement_combat_1024_candidate_v001.png`
- Output directory: `design/development/asset_candidates/starter_cats/saiban/batch_31_cutout_candidate_2026-06-14`
- Method: sample the 12-pixel image border, flood-fill connected parchment background pixels, convert that region to alpha, and apply a 0.65px soft matte.
- Background RGB: `250,244,234`
- Flood threshold: `52`
- Opaque coverage: `33.69%`
- Safety: files remain outside `Assets`; formal import remains blocked until active-cat Play Mode screenshot review passes.
