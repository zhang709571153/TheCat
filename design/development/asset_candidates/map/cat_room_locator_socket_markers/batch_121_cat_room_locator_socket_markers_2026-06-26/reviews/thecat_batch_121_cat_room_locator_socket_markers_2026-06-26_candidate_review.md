# Batch121 Cat-Room Locator Socket Markers Candidate Review

Decision: `candidate_complete_pending_unity_review`; do not import into runtime folders.

## Output Summary

- Candidate folder: `design/development/asset_candidates/map/cat_room_locator_socket_markers/batch_121_cat_room_locator_socket_markers_2026-06-26`
- Asset table: `thecat_batch_121_cat_room_locator_socket_markers_2026-06-26_asset_table.csv`
- Manifest: `thecat_map_catroom_socket_batch121_semantic_manifest.csv`
- Contact sheet: `thecat_map_catroom_socket_batch121_semantic_contact_sheet_v001.png`
- Readability board: `thecat_map_catroom_socket_batch121_96px_64px_cat_room_readability_board_v001.png`
- Process note: `thecat_batch_121_cat_room_locator_socket_markers_2026-06-26_process_note.md`
- Final review CSV: `thecat_batch_121_cat_room_locator_socket_markers_2026-06-26_final_review.csv`

## Integrated Review

Visual/source-boundary review: `PASS_WITH_P2`.

Readability review: `PASS_WITH_P2`.

Production QA: local dedicated validator, generic candidate-pack validator, and independent production QA review are `PASS_WITH_P2`; no P1 was found. Manifest/variant hashes and dimensions align, no `.meta` files or runtime `Assets` leak was found, and the max active path length is 241 characters as a P2 watch for future nested prompt files.

No P1 target-size readability failure is currently known. No candidate is rejected on current local evidence.

## Candidate Decisions

`candidate_keep`:

- `dream_portal_locator_socket`
- `return_to_room_floor_socket`
- `locked_interaction_floor_socket`

`candidate_conditional`:

- `bed_care_locator_socket`
- `feeder_meal_locator_socket`
- `litter_clean_locator_socket`
- `rest_ready_floor_socket`
- `low_hunger_alert_floor_socket`
- `cleanup_needed_alert_floor_socket`

## P2 Watch Items

- Marker-vs-prop drift: bed, feeder, litter, rest, and cleanup are emblem-heavy and must prove they read as locator/socket markers beside Batch91 props and Batch92 interaction states.
- Pairwise semantic proof is required for bed/rest, feeder/low-hunger, litter/cleanup, portal/return, and lock/blocked-unavailable.
- Red alert markers must not collapse into generic danger/error when placed on the cat-room surface.

## Source Boundary

This batch is static symbolic map/UI marker art only. It does not claim IAd live character approval, HDo/FoW9 map archive coverage, character body/face/portrait/paw/costume/framesheet/animation approval, or runtime replacement approval.

## Runtime Gate

- [ ] Sprite import settings.
- [ ] Declared target runtime path.
- [ ] PPU/pivot/sorting layer confirmation.
- [ ] Cat-room screenshots at 96px and 64px on intended live backgrounds.
- [ ] Marker-vs-prop proof beside Batch91/92/93 context.
- [ ] Binding proof and no recursive candidate import.
- [ ] Clean Console.
- [ ] Explicit human approval.
