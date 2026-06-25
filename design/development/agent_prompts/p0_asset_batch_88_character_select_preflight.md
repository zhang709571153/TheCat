# P0 Asset Batch 88 - Character Select Preflight

## Scope

Review the Batch 88 character-select preflight candidate packet before any Unity import or binding work.

Candidate packet:

- `design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25`
- `thecat_ui_character_select_batch88_manifest.csv`
- `thecat_ui_character_select_batch88_review_sheet_v001.png`
- `thecat_ui_character_select_batch88_candidate_review.md`

## Source Truth

- UI/style source truth: Feishu `Qr1XdXd6KosnjMxjgW7cS89kn9c`, especially the P0 UI surface list and dreamglass/cyan/gold interface language.
- Character identity source truth: local source-lock packets derived from Feishu `IAdkdcpciobUTXxa7dBcRx7Bngf`; live Feishu fetch remains ACL-blocked in this shell.
- HUD avatar reuse is allowed for this lane. Do not generate or accept any new starter-cat body art, pose, portrait, costume, recolor, framesheet, or runtime sprite replacement.

## Required Review

- Validate selected versus idle card readability at `1920x1080`, `1365x768`, `1280x720`, and `1024x768`.
- Confirm source-locked HUD avatars still read as Saiban, Nephthys, and Suzune.
- Confirm the layout leaves room for Unity-rendered Chinese names, roles, descriptions, and action labels.
- Confirm actual sprites and mockups do not bake Chinese text.
- Confirm all candidate PNGs remain under `design/development/asset_candidates` and no Unity `.meta` files were generated.
- Run `design/development/tools/validate_ui_character_select_preflight_candidates.ps1`.

## Output

Return `PASS`, `PASS_WITH_P2`, or `FAIL` with concrete paths and required follow-up. A pass only approves candidate review readiness; it is not Unity import approval.
