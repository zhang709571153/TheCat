# P0 Asset Batch 79 Review Prompt - System Icon Candidates

You are reviewing `D:\Unity Workspace\TheCat` Batch 79 candidate UI assets.
This is a review-only packet for system icons. Do not install anything into
Unity, do not move files into `Assets`, and do not create `.meta` files.
Do not install anything into Unity during this review.

## Required Reading

- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/asset_review/P0_ASSET_MEMORY_TODO_LEDGER.md`

## Candidate Packet

- Directory:
  `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24`
- Manifest:
  `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/system_icons_batch79_manifest.csv`
- Review sheet:
  `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/thecat_ui_system_icons_batch79_review_sheet_v001.png`
- Contact sheet:
  `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/thecat_ui_system_icons_batch79_contact_sheet_v001.png`
- Validator:
  `design/development/tools/validate_system_icon_candidates.ps1`

## Scope

Batch 79 should contain exactly ten transparent candidate system icons, each
exported at 128px, 64px, and 32px:

- `settings`
- `sound`
- `mute`
- `back`
- `close`
- `pause`
- `continue`
- `retry`
- `lock`
- `warning`

These system icons support global UI, settings, pause menu, navigation,
warnings, and locked-content states from the P0 UI inventory. They should stay
stylistically consistent with the Batch 78 dreamglass / moon-blue / restrained
fish-gold UI language.

## Source-Lock Boundary

- This is non-cat UI work only.
- Do not generate, crop, recolor, import, or runtime-bind starter-cat body art.
- No cats, paws, tails, fur markings, starter-cat costume motifs, character
  faces, text, letters, numbers, or watermarks should appear in the icons.

## Review Questions

1. Are all ten icons present and easy to map to their intended meaning?
2. Do the 64px and 32px variants remain readable?
3. Does `mute` read as muted even with the slash and remaining sound arcs?
4. Does `warning` read as a warning without exclamation text?
5. Do the icons read on both dark and light UI backgrounds?
6. Does the manifest remain candidate-only, outside `Assets`, and hash-stable?
7. What should be carried into Unity review before formal import?

## Expected Output

Return a concise review with:

- `P0 blockers`: any issue that should stop this packet from being considered
  candidate-complete.
- `P1 watch items`: issues that can remain pending for Unity screenshot/import
  review.
- `Tracking recommendation`: the status this batch should carry in the asset
  ledger and master gap matrix.
