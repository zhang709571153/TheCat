# P0 Starter Cat Candidate Gate

Date: 2026-06-14

Purpose: convert the user's corrected requirement into an operating gate for
systematic cat asset production. Saiban, Nephthys, and Suzune assets must match
their colored three-view turnarounds exactly enough to preserve body
proportion, palette, markings, costume, props, and civilization motifs.

## Current Architecture Read

- P0 architecture is ready for systematic asset production, but not final P0
  visual acceptance.
- Manifest baseline: 95 generated/import-ready assets.
- Runtime visual baseline: 91 manifest-backed runtime bindings.
- Starter-cat source locks exist for the three colored turnarounds and the
  current Unity combat sprites.
- Starter-cat visual checklist, turnaround conformance spec, asset-production
  spec, and formal import readiness are already wired into offline readiness
  reports.
- Formal starter-cat Unity import is still blocked because active-cat Play Mode
  screenshots are not available for side-by-side review.

## Candidate Pack State

Batch 05 currently contains 12 review-only candidate PNGs:

- 3 cats: Saiban, Nephthys, Suzune.
- 4 derivative types per cat:
  `combat_sprite_refinement_512`, `front_animation_keyframe_512`,
  `hud_avatar_256`, and `skill_icon_motif_128`.
- 3 per-cat review sheets at `1600x900`.
- 3 per-cat review notes.
- 1 candidate manifest CSV.

These candidates remain outside `Assets` and must not receive Unity `.meta`
files.

## Hard Acceptance Rule

Any cat candidate is rejected if it fails any of these checks:

- It is not tied to the matching `*_turnaround_colored_2026-06-03.png` source
  lock.
- It changes the cat's colored-turnaround palette, markings, props, costume, or
  silhouette.
- It introduces human body proportions, human costume posture, or generic
  cute-cat drift.
- It uses the generated starter-cat lineup as the primary reference.
- It lacks a per-cat review note with front, side, back, palette, prop/costume,
  and prohibited-drift comparisons.
- It is placed under `Assets` before active-cat screenshot approval.
- It has a `.png.meta` file while still in the candidate review directory.

## New Offline Gate

Added:

- `design/development/tools/validate_starter_cat_candidate_pack.ps1`
- `design/development/agent_prompts/p0_asset_batch_26_starter_cat_candidate_gate.md`

The validation script checks:

- 12 manifest rows and exactly 3 starter cats.
- Exactly 4 allowed derivative asset types per cat.
- Candidate PNG paths are under `design/development/asset_candidates/starter_cats`.
- Candidate PNG dimensions match their derivative type.
- Candidate SHA-256 values match the manifest.
- Source turnaround and locked Unity sprite SHA-256 values still match.
- Per-cat review sheets are present and `1600x900`.
- Candidate PNGs and review sheets do not have Unity `.meta` files.
- Review notes include colored-turnaround, front / side / back, palette,
  prohibited-drift, rejection-rule, and cat-specific trait tokens.
- Recommendation remains `candidate_review_only_pending_playmode_screenshot`.

## Validation Result

Command:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_starter_cat_candidate_pack.ps1
```

Result:

- Passed.
- Rows: 12.
- Review notes: 3.
- Review sheets: 3.
- Formal Unity import remains blocked pending active-cat Play Mode screenshot
  review.

## Next Asset Production Decision

Proceed with systematic assets in this order:

1. Continue non-cat UI/VFX/background batches directly into `Assets` when they
   are symbolic or source-locked and pass manifest/runtime gates.
2. Produce new Saiban, Nephthys, and Suzune assets only as candidate packs under
   `design/development/asset_candidates/starter_cats`.
3. Do not import or replace starter-cat runtime sprites until Unity editor
   screenshots `05-active-cat-saiban.png`, `06-active-cat-nephthys.png`, and
   `07-active-cat-suzune.png` are captured and approved against the colored
   turnaround contact sheet.
