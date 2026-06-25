# Agent Prompt - P0 Asset Batch 83 Loading Start Preflight

## Scope

Review the Batch 83 loading/start candidate packet before any Unity import or
runtime binding. This is a non-cat UI surface review, not a formal install.

## Inputs

- Candidate directory:
  `design/development/asset_candidates/ui/loading_start/batch_83_loading_start_preflight_2026-06-25`
- Manifest:
  `thecat_ui_loading_start_batch83_manifest.csv`
- Review sheet:
  `thecat_ui_loading_start_batch83_review_sheet_v001.png`
- Candidate review:
  `thecat_ui_loading_start_batch83_candidate_review.md`
- Process note:
  `thecat_ui_loading_start_batch83_process_note.md`
- Validator:
  `design/development/tools/validate_ui_loading_start_preflight_candidates.ps1`
- Source truth:
  `Qr1XdXd6KosnjMxjgW7cS89kn9c` UI/style document. Use the local synced
  evidence if live Feishu access is not available.

## Required Review

1. Confirm the 4 transparent sprites and 4 resolution mockups are present and
   match the manifest.
2. Confirm the candidates stay outside `Assets` and no Unity `.meta` files are
   created in the candidate packet.
3. Check visual consistency with the existing dreamglass UI shell: logo,
   background, panel, primary button, sleep gauge frame/fill, and sleep icon.
4. Flag any interpretation risk where the spinner or dots could be read as
   character/body art instead of abstract loading feedback.
5. Check the 1024x768 mockup for crowding and overlap risk.
6. Keep placeholder text/state replacement as a Unity gate. Do not approve
   baked text or placeholder UI as final.

## Acceptance Output

Return one of:

- `PASS`: no blocking issue for Unity review.
- `PASS_WITH_P2`: only low-priority Unity watch items remain.
- `FAIL`: a P0/P1 issue requires candidate packet changes before Unity review.

Mention exact files and rows where possible. Do not recommend installing or
binding the candidates until Unity screenshots, import settings, binding proof,
and Console evidence pass.
