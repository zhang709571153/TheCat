# Batch 87 Battle HUD Human Review Request

This is a review-request packet only. It is not `human_review_approval.md` and must not be counted as formal approval evidence.

## Decision Boundary

- Review request ready: yes
- Formal install allowed: no
- Unity evidence complete: no
- Formal runtime binding decision approved: no
- Candidate policy: `candidate-backed Unity preflight only`
- Shared Console classifier: active strict-clean contract
- Console classifier policy: StrictClean is required for formal clean-Console evidence; known environment noise is classified but still blocks formal clean-Console approval.

## Evidence Packet

- Preflight report: `design/development/asset_review/BATCH87_BATTLE_HUD_UNITY_PREFLIGHT_REPORT_2026-06-25.md`
- Formal runtime binding decision request: `design/development/asset_review/BATCH87_BATTLE_HUD_FORMAL_RUNTIME_BINDING_DECISION_REQUEST_2026-06-26.md`
- Runtime evidence report: `design/development/asset_review/BATCH87_BATTLE_HUD_RUNTIME_EVIDENCE_REPORT_2026-06-25.md`
- Evidence triage: `design/development/asset_review/BATCH87_BATTLE_HUD_UNITY_EVIDENCE_TRIAGE_2026-06-25.md`
- Console blockers: `design/development/asset_review/BATCH87_BATTLE_HUD_UNITY_PREFLIGHT_CONSOLE_BLOCKERS_2026-06-25.md`
- Screenshot: `design/development/screenshots/batch_87_battle_hud_unity_preflight/01-battle-hud-batch87-1920x1080.png`
- Screenshot: `design/development/screenshots/batch_87_battle_hud_unity_preflight/02-battle-hud-batch87-1365x768.png`
- Screenshot: `design/development/screenshots/batch_87_battle_hud_unity_preflight/03-battle-hud-batch87-1280x720.png`
- Screenshot: `design/development/screenshots/batch_87_battle_hud_unity_preflight/04-battle-hud-batch87-1024x768.png`
- Automatic review: `design/development/asset_review/batch_87_battle_hud_unity_preflight/text_and_skill_state_review.md`
- Automatic review: `design/development/asset_review/batch_87_battle_hud_unity_preflight/telegraph_occlusion_click_target_review.md`

## Reviewer Checklist

- Confirm the four candidate-backed screenshots remain readable at 1920x1080, 1365x768, 1280x720, and 1024x768.
- Confirm battle HUD Chinese copy, gauge values, skill state language, enemy pressure, and runtime controls match the live P0 design intent.
- Confirm skill trays, warning/telegraph areas, pause/speed/restart controls, and click targets remain readable and non-overlapping.
- Confirm Batch 87 remains outside `P0VisualAssetCatalog` until strict-clean Console evidence and a formal runtime binding decision exist.
- Confirm no starter-cat source-lock, body-art replacement, or unrelated UI candidate lane is implicitly approved by this review.

## Required Before Approval File

- `console_clean_report.md` must reference an existing Batch 87 runtime evidence log that passes the shared `StrictClean` Console classifier.
- A human reviewer must explicitly approve formal install in a separate `human_review_approval.md` file with the required reviewer and approval-date tokens.
- The formal runtime binding decision must explicitly approve adding Batch 87 to the runtime scene/presenter/catalog path.

## Current Blocking Items

- Missing Unity evidence: `design/development/asset_review/batch_87_battle_hud_unity_preflight/console_clean_report.md`
- Missing Unity evidence: `design/development/asset_review/batch_87_battle_hud_unity_preflight/human_review_approval.md`
- Formal runtime binding decision has not passed.

## Protected Runtime State

- Do not create or count `human_review_approval.md` from this request packet.
- Do not create or count `console_clean_report.md` until the referenced Unity log is strict clean.
- Do not add Batch 87 candidate ids to formal runtime bindings from this request packet.
- Keep the current IMGUI battle HUD as the playable runtime path.
