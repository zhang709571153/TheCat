# Owner Sleep States Batch 76 Candidate Review

Decision: candidate review only; do not import into Unity.

This batch fills a P0 art-inventory gap for the owner-in-bed sleep state animation. It creates a non-cat v002 overlay sprite-sheet candidate and 24 padded alpha frames for later Unity slicing or overlay review.

## Outputs

- Chroma source: `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/thecat_owner_sleep_states_batch76_chromakey_source_1536x1024_v002.png`
- Alpha sheet: `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/thecat_owner_sleep_states_batch76_alpha_sheet_1536x1024_candidate_v002.png`
- Frame directory: `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames`
- Contact sheet: `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/thecat_owner_sleep_states_batch76_contact_sheet_1920x1320_v001.png`
- Review sheet: `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/thecat_owner_sleep_states_batch76_review_sheet_1920x1320_v001.png`
- Manifest: `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/owner_sleep_states_batch76_manifest.csv`
- Process note: `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/owner_sleep_states_batch76_process_note.md`
- Agent prompt: `design/development/agent_prompts/p0_asset_batch_76_owner_sleep_state_framesheet_candidate.md`

## Visual Decision

- Pass: 4 states x 6 frames are present: deep sleep, half awake, near awake, wake failure.
- Pass: no cat body, fur markings, paws, tails, starter-cat costumes, or colored-turnaround crops are present.
- Pass: v002 uses owner/pillow/blanket overlay art instead of a full bed-frame prop, reducing collision risk with the existing bedroom scene layer.
- Pass: active frame outputs are normalized into padded 256x256 canvases for slice-safety review.
- Pass: the sheet uses the bedroom-dream palette from the P0 art direction: navy star blanket, warm lamp, blue-lavender sleep, and amber wake warning.
- Pass: near-awake and wake-failure rows include alarm/light vibration, dream cracks, and a consciousness orb returning toward the owner body.
- Watch: v001 retained historical full-bed source evidence, but v002 is the active source for the manifest and generated frames.
- Watch: the owner's face is generic and should stay generic; this is not a named character portrait.

## Unity Gate

- Import is blocked until Unity validates Sprite import settings, slicing, pivot, runtime scale, scene/prefab binding, sleep-state timing, battle-world screenshot readability, and Console status.
- Agent review correction: v001 was source-safe but not slice-safe as raw 256x256 cells; v002 plus padded normalization is the current review packet.
- Candidate files stay outside `Assets` and must not receive Unity `.meta` files.

## Manifest Rows

- `deep_sleep` frame `1` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_deep_sleep_f01_256_candidate_v001.png`
- `deep_sleep` frame `2` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_deep_sleep_f02_256_candidate_v001.png`
- `deep_sleep` frame `3` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_deep_sleep_f03_256_candidate_v001.png`
- `deep_sleep` frame `4` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_deep_sleep_f04_256_candidate_v001.png`
- `deep_sleep` frame `5` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_deep_sleep_f05_256_candidate_v001.png`
- `deep_sleep` frame `6` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_deep_sleep_f06_256_candidate_v001.png`
- `half_awake` frame `1` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_half_awake_f01_256_candidate_v001.png`
- `half_awake` frame `2` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_half_awake_f02_256_candidate_v001.png`
- `half_awake` frame `3` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_half_awake_f03_256_candidate_v001.png`
- `half_awake` frame `4` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_half_awake_f04_256_candidate_v001.png`
- `half_awake` frame `5` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_half_awake_f05_256_candidate_v001.png`
- `half_awake` frame `6` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_half_awake_f06_256_candidate_v001.png`
- `near_awake` frame `1` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_near_awake_f01_256_candidate_v001.png`
- `near_awake` frame `2` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_near_awake_f02_256_candidate_v001.png`
- `near_awake` frame `3` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_near_awake_f03_256_candidate_v001.png`
- `near_awake` frame `4` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_near_awake_f04_256_candidate_v001.png`
- `near_awake` frame `5` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_near_awake_f05_256_candidate_v001.png`
- `near_awake` frame `6` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_near_awake_f06_256_candidate_v001.png`
- `wake_failure` frame `1` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_wake_failure_f01_256_candidate_v001.png`
- `wake_failure` frame `2` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_wake_failure_f02_256_candidate_v001.png`
- `wake_failure` frame `3` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_wake_failure_f03_256_candidate_v001.png`
- `wake_failure` frame `4` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_wake_failure_f04_256_candidate_v001.png`
- `wake_failure` frame `5` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_wake_failure_f05_256_candidate_v001.png`
- `wake_failure` frame `6` -> `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/frames/thecat_owner_sleep_state_wake_failure_f06_256_candidate_v001.png`
