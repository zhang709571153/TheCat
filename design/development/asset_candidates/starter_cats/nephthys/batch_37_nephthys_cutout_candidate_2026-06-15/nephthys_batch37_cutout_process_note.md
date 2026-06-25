# Nephthys Batch 37 Cutout Process Note

This batch is a deterministic local post-processing pass over the Batch 36 Nephthys candidate. No new image generation was invoked.

- Source turnaround authority: `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- Input candidate: `design/development/asset_candidates/starter_cats/nephthys/batch_36_nephthys_ai_refinement_candidate_2026-06-15/thecat_cat_nephthys_ai_refinement_combat_1024_candidate_v001.png`
- Output directory: `design/development/asset_candidates/starter_cats/nephthys/batch_37_nephthys_cutout_candidate_2026-06-15`
- Method: sample the 12-pixel image border, flood-fill connected parchment background pixels, convert that region to alpha, and apply a 0.65px soft matte.
- Background RGB: `242,237,229`
- Flood threshold: `48`
- Opaque coverage: `32.30%`
- Safety: files remain outside `Assets`; formal import remains blocked until active-cat Play Mode screenshot review passes.
