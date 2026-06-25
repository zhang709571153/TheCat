# Agent Prompt - P0 Asset Batch 85 Settings / Pause Preflight

## Scope

Review the Batch 85 settings/pause candidate packet before any Unity import or
runtime binding. This is a non-cat UI surface review, not a formal install.

## Inputs

- Candidate directory:
  `design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25`
- Manifest:
  `thecat_ui_settings_pause_batch85_manifest.csv`
- Review sheet:
  `thecat_ui_settings_pause_batch85_review_sheet_v001.png`
- Candidate review:
  `thecat_ui_settings_pause_batch85_candidate_review.md`
- Process note:
  `thecat_ui_settings_pause_batch85_process_note.md`
- Validator:
  `design/development/tools/validate_ui_settings_pause_preflight_candidates.ps1`
- Source truth:
  `Qr1XdXd6KosnjMxjgW7cS89kn9c` UI/style document. Use the local synced
  evidence if live Feishu access is not available.
- Upstream candidate sources:
  `batch_78_settings_control_candidates_2026-06-24` and
  `batch_79_system_icon_candidates_2026-06-24`.

## Required Review

1. Confirm the 6 transparent sprites and 4 settings/pause mockups are present
   and match the manifest.
2. Confirm the candidates stay outside `Assets` and no Unity `.meta` files are
   created in the candidate packet.
3. Check visual consistency with the existing dreamglass UI shell: settings
   panel, segmented tabs, option rows, confirmation modal, key hint chip, and
   section divider.
4. Confirm the sprites are textless and do not bake Chinese UI labels, option
   names, values, or shortcuts into the art.
5. Check the 1024x768 compact mockup for crowding, z-order, and whether the
   lower-left key hint chip could be mistaken for a clickable back button.
6. Keep text replacement, input semantics, click targets, and clean Console as
   Unity gates. Do not approve placeholder UI as final.

## Acceptance Output

Return one of:

- `PASS`: no blocking issue for Unity review.
- `PASS_WITH_P2`: only low-priority Unity watch items remain.
- `FAIL`: a P0/P1 issue requires candidate packet changes before Unity review.

Mention exact files and rows where possible. Do not recommend installing or
binding the candidates until Unity screenshots, import settings, binding proof,
text replacement, click-target proof, and Console evidence pass.
