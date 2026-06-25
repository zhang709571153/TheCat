# Batch88 Character Select Agent Review

Date: 2026-06-25

Batch: `batch_88_character_select_preflight_2026-06-25`

Candidate directory: `design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25`

## Verdict

`PASS_WITH_P2`

The candidate packet is acceptable as review-only character-select UI art. It is not Unity import approval.

## Independent Review Results

| Reviewer | Result | Notes |
| --- | --- | --- |
| Visual/style review agent | `PASS_WITH_P2` | Qr1 cyan/gold dreamglass language, selected/idle states, HUD avatar reuse, and no-new-cat-body policy pass. `1024x768` and especially `1280x720` are dense and require Unity screenshot proof with real Chinese labels and click targets. |
| Production QA agent | `PASS_WITH_P2` | Manifest, hashes, source assets, candidate-only directory policy, no `.meta`, no `Assets` writes, and non-image2 source model pass. Initial provenance wording was too weak. |

## P2 Fix Applied

- Updated `design/development/tools/build_ui_character_select_preflight_candidates.py` so the generated process note names Feishu `Qr1`, Feishu `IAd`, the current ACL block, missing `OPENAI_API_KEY`, and the unavailable explicit built-in model selector.
- Updated `design/development/tools/validate_ui_character_select_preflight_candidates.ps1` so those provenance tokens are required.
- Regenerated the Batch88 manifest and evidence hashes.
- Re-ran `validate_ui_character_select_preflight_candidates.ps1`: pass, 10 rows.

## Remaining Unity Gates

- Capture Unity-rendered character-select screenshots at `1920x1080`, `1365x768`, `1280x720`, and `1024x768`.
- Confirm real Chinese names, roles, descriptions, selected labels, and start labels do not crowd the `1280x720` and `1024x768` layouts.
- Confirm three starter-card click targets and the start action are large enough and do not overlap.
- Confirm HUD avatars still match source-locked Saiban, Nephthys, and Suzune identity.
- Confirm import settings, binding proof, and clean Console before any formal install.
