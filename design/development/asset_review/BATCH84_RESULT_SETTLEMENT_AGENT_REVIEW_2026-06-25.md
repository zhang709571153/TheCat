# Batch 84 Result / Settlement Agent Review

Status: `candidate_complete_pending_unity_validation`

Batch 84 is a candidate-only local preflight packet for result and settlement
screens. It is not Unity-accepted and is not a formal `Assets/` promotion.

## Packet

- Candidate directory:
  `design/development/asset_candidates/ui/result_settlement/batch_84_result_settlement_preflight_2026-06-25`
- Manifest:
  `thecat_ui_result_settlement_batch84_manifest.csv`
- Contact sheet:
  `thecat_ui_result_settlement_batch84_contact_sheet_v001.png`
- Review sheet:
  `thecat_ui_result_settlement_batch84_review_sheet_v001.png`
- Candidate review:
  `thecat_ui_result_settlement_batch84_candidate_review.md`
- Process note:
  `thecat_ui_result_settlement_batch84_process_note.md`
- Validator:
  `design/development/tools/validate_ui_result_settlement_preflight_candidates.ps1`

## Production Method

Batch 84 uses deterministic local derivatives from existing P0 UI assets. It
does not claim image2 provenance because `OPENAI_API_KEY` was not available to
run the strict `gpt-image-2` CLI path.

The packet contains 11 rows:

- 7 transparent result/settlement sprites.
- 4 local mockups for battle victory, battle defeat, run cleared, and run
  failed states.

## Review Results

### Visual Review

Initial result: `PASS_WITH_P2`

Findings:

- P2: failure-state mockups originally tinted a checkmark red, making failure
  semantics ambiguous.
- P2: the 1024x768 run-failed mockup may crowd once Unity-rendered Chinese
  labels, numbers, and buttons are added.

Fix:

- Rebuilt the stamp sprites as separate `success_stamp_ring` and
  `failure_stamp_ring` rows.
- Updated `battle_defeat_1920x1080` and `run_failed_1024x768` to use a clear
  red/purple X failure stamp instead of a tinted success mark.

Delta result: `PASS_WITH_P2`

The failure-stamp semantic issue is resolved. The 1024x768 crowding watch
remains as a Unity validation gate.

### Production QA

Initial result: `PASS_WITH_P2`

Finding:

- P2: the contact sheet and review sheet were present but not hash-locked in
  the manifest.

Fix:

- Added `contact_sheet_sha256` and `review_sheet_sha256` fields to every
  manifest row.
- Updated the validator to hash-check both sheet files.

Delta result: `PASS`

The manifest sheet hashes now match current files:

- `contact_sheet_sha256`: `a7e73956750d91600c446c76ed5dd42746080ad5ab8dd9e16b42867d14275b69`
- `review_sheet_sha256`: `a60d772ac42427ef2b34b6a62e61e0c2e0475da248d33a9639e21c44e4d72238`

## Current Gates

- Dedicated validator passes with 11 rows.
- Non-cat candidate validation matrix includes `ui_result_settlement_preflight`
  and passes.
- Candidate PNGs remain outside `Assets`.
- Candidate folder contains no Unity `.meta` files.
- Unity-rendered victory, defeat, run-cleared, and run-failed screenshots remain
  required before promotion.
- Unity-rendered Chinese text replacement, 1024x768 crowding, Sprite import
  settings, binding proof, and Console checks remain required before promotion.

## Decision

Batch 84 may be queued for Unity validation as
`p0_asset_queue_result_settlement_preflight_candidates`.

Do not import, bind, or mark accepted until the Unity screenshot and Console
evidence above is attached.
