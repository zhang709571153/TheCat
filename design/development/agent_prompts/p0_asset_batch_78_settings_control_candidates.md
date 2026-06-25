# P0 Asset Batch 78 Review Prompt - Settings Control Candidates

You are reviewing `D:\Unity Workspace\TheCat` Batch 78 candidate UI assets.
This is a review-only packet for settings controls. Do not install anything
into Unity, do not move files into `Assets`, and do not create `.meta` files.
Do not install anything into Unity during this review.

## Required Reading

- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/asset_review/P0_ASSET_MEMORY_TODO_LEDGER.md`

## Candidate Packet

- Directory:
  `design/development/asset_candidates/ui/settings_controls/batch_78_settings_control_candidates_2026-06-24`
- Manifest:
  `design/development/asset_candidates/ui/settings_controls/batch_78_settings_control_candidates_2026-06-24/settings_controls_batch78_manifest.csv`
- Review sheet:
  `design/development/asset_candidates/ui/settings_controls/batch_78_settings_control_candidates_2026-06-24/thecat_ui_settings_controls_batch78_review_sheet_v001.png`
- Contact sheet:
  `design/development/asset_candidates/ui/settings_controls/batch_78_settings_control_candidates_2026-06-24/thecat_ui_settings_controls_batch78_contact_sheet_v001.png`
- Validator:
  `design/development/tools/validate_settings_control_candidates.ps1`

## Scope

Batch 78 should contain exactly six transparent candidate controls for P0
settings screens:

- `slider_track` at `384x64`
- `slider_knob` at `96x96`
- `switch_off` at `192x96`
- `switch_on` at `192x96`
- `checkbox_unchecked` at `96x96`
- `checkbox_checked` at `96x96`

These settings controls should support music volume, sound-effect volume,
display toggles, and basic option confirmation surfaces from the P0 UI
inventory. They should remain stylistically consistent with the dreamglass /
moon-blue / restrained fish-gold UI language.

## Source-Lock Boundary

- This is non-cat UI work only.
- Do not generate, crop, recolor, import, or runtime-bind starter-cat body art.
- No cats, paws, tails, fur markings, starter-cat costume motifs, character
  faces, text, letters, numbers, or watermarks should appear in the controls.

## Review Questions

1. Are all six controls present and easy to map to their intended state?
2. Are the active and inactive states visually distinct enough for the settings
   screen?
3. Do the controls read on both dark and light settings panels?
4. Are the exact target dimensions and transparent backgrounds correct?
5. Does the manifest remain candidate-only, outside `Assets`, and hash-stable?
6. What should be carried into Unity review before formal import?

## Expected Output

Return a concise review with:

- `P0 blockers`: any issue that should stop this packet from being considered
  candidate-complete.
- `P1 watch items`: issues that can remain pending for Unity screenshot/import
  review.
- `Tracking recommendation`: the status this batch should carry in the asset
  ledger and master gap matrix.
