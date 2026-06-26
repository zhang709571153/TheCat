# Asset Candidate Review - bedroom_event_pickups

Review only. Do not edit files.

Inputs:
- Candidate folder: `design/development/asset_candidates/map/bedroom_event_pickups/batch_117_bedroom_event_pickups_2026-06-26`
- Asset table: `design/development/asset_candidates/map/bedroom_event_pickups/batch_117_bedroom_event_pickups_2026-06-26/tc_map_pickup_batch_117_bedroom_event_pickups_2026-06-26_asset_table.csv`
- Process note: `design/development/asset_candidates/map/bedroom_event_pickups/batch_117_bedroom_event_pickups_2026-06-26/tc_map_pickup_batch_117_bedroom_event_pickups_2026-06-26_process_note.md`
- Final review CSV: `design/development/asset_candidates/map/bedroom_event_pickups/batch_117_bedroom_event_pickups_2026-06-26/tc_map_pickup_batch_117_bedroom_event_pickups_2026-06-26_final_review.csv`
- Source truth: `Qr1 UI/style truth revision 816; Qr1 P0 bedroom dream map boundary; Batch101/108/109 bedroom-map context; Batch104 reward-token context; no IAd character-body claim; no HDo/FoW9 map-archive claim`

Role:
- Visual/style reviewer: check source fit, shape language, palette, silhouette, target-size readability, semantic drift, baked text, forbidden content, and consistency across the family.
- Production QA reviewer: check alpha edges, padding, dimensions, hashes, manifest completeness, runtime-folder separation, no engine metadata, pivot/ppu/sorting/collider notes, and import blockers.

Return:
- Verdict: `PASS`, `PASS_WITH_P2`, `PASS_WITH_P1`, or `FAIL`.
- Accepted candidates.
- Conditional candidates and exact follow-up checks.
- Rejected or superseded candidates.
- Engine/runtime gates still required.
