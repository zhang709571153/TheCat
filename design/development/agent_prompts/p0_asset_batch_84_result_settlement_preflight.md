# Agent Prompt - P0 Asset Batch 84 Result Settlement Preflight

## Scope

Review the Batch 84 result/settlement candidate packet before any Unity import
or runtime binding. This is a non-cat UI surface review, not a formal install.

## Inputs

- Candidate directory:
  `design/development/asset_candidates/ui/result_settlement/batch_84_result_settlement_preflight_2026-06-25`
- Manifest:
  `thecat_ui_result_settlement_batch84_manifest.csv`
- Review sheet:
  `thecat_ui_result_settlement_batch84_review_sheet_v001.png`
- Candidate review:
  `thecat_ui_result_settlement_batch84_candidate_review.md`
- Process note:
  `thecat_ui_result_settlement_batch84_process_note.md`
- Validator:
  `design/development/tools/validate_ui_result_settlement_preflight_candidates.ps1`
- Source truth:
  `Qr1XdXd6KosnjMxjgW7cS89kn9c` UI/style document. Use the local synced
  evidence if live Feishu access is not available.

## Required Review

1. Confirm the 7 transparent sprites and 4 result/settlement mockups are present
   and match the manifest.
2. Confirm the candidates stay outside `Assets` and no Unity `.meta` files are
   created in the candidate packet.
3. Check visual consistency with the existing dreamglass UI shell: result panel,
   reward row, stat chip, action button, divider, success stamp, and failure
   stamp.
4. Confirm failure states use the red/purple X stamp and do not reuse or recolor
   the success check stamp.
5. Check the 1024x768 run-failed mockup for crowding and overlap risk once
   Unity-rendered Chinese labels, numbers, and buttons are added.
6. Keep text replacement as a Unity gate. Do not approve baked text or
   placeholder UI as final.

## Acceptance Output

Return one of:

- `PASS`: no blocking issue for Unity review.
- `PASS_WITH_P2`: only low-priority Unity watch items remain.
- `FAIL`: a P0/P1 issue requires candidate packet changes before Unity review.

Mention exact files and rows where possible. Do not recommend installing or
binding the candidates until Unity screenshots, import settings, binding proof,
text replacement, and Console evidence pass.
