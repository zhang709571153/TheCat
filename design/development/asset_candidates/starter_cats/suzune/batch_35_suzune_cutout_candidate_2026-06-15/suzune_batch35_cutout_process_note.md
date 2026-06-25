# Suzune Batch 35 Cutout Process Note

This batch is a deterministic local post-processing pass over the Batch 34 Suzune candidate. No new image generation was invoked.

- Source turnaround authority: `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`
- Input candidate: `design/development/asset_candidates/starter_cats/suzune/batch_34_suzune_ai_refinement_candidate_2026-06-14/thecat_cat_suzune_ai_refinement_combat_1024_candidate_v001.png`
- Output directory: `design/development/asset_candidates/starter_cats/suzune/batch_35_suzune_cutout_candidate_2026-06-15`
- Method: sample the 12-pixel image border, flood-fill connected parchment background pixels, convert that region to alpha, and apply a 0.65px soft matte.
- Background RGB: `245,237,226`
- Flood threshold: `48`
- Opaque coverage: `30.98%`
- Safety: files remain outside `Assets`; formal import remains blocked until active-cat Play Mode screenshot review passes.
