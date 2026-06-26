# Asset Candidate Review - scene_selection_tokens

Review only. Do not edit files.

Inputs:
- Candidate folder: `design/development/asset_candidates/ui/scene_selection_tokens/batch_107_scene_selection_tokens_2026-06-26`
- Asset table: `design/development/asset_candidates/ui/scene_selection_tokens/batch_107_scene_selection_tokens_2026-06-26/tc_ui_scene_batch_107_scene_selection_tokens_2026-06-26_asset_table.csv`
- Process note: `design/development/asset_candidates/ui/scene_selection_tokens/batch_107_scene_selection_tokens_2026-06-26/tc_ui_scene_batch_107_scene_selection_tokens_2026-06-26_process_note.md`
- Final review CSV: `design/development/asset_candidates/ui/scene_selection_tokens/batch_107_scene_selection_tokens_2026-06-26/tc_ui_scene_batch_107_scene_selection_tokens_2026-06-26_final_review.csv`
- Source truth: `Qr1 UI/style truth revision 816; static scene-selection UI token lane; no IAd character-body claim; no HDo map-archive claim`

Role:
- Visual/style reviewer: check source fit, shape language, palette, silhouette, target-size readability, semantic drift, baked text, forbidden content, and consistency across the family.
- Production QA reviewer: check alpha edges, padding, dimensions, hashes, manifest completeness, runtime-folder separation, no engine metadata, pivot/ppu/sorting/collider notes, and import blockers.

Return:
- Verdict: `PASS`, `PASS_WITH_P2`, `PASS_WITH_P1`, or `FAIL`.
- Accepted candidates.
- Conditional candidates and exact follow-up checks.
- Rejected or superseded candidates.
- Engine/runtime gates still required.
