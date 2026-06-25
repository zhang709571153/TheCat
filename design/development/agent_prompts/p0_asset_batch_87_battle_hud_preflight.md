# P0 Asset Batch 87 Battle HUD Preflight

Use this prompt to review or continue the Batch 87 battle HUD preflight candidate pack.

## Scope

- Candidate directory: `design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25`
- Unity import root, if later approved: `Assets/TheCat/Art/UI/BattleHUD`
- Current state: `CandidatePackCompletePendingUnityReview`
- Do not write into `Assets/TheCat/Art/UI/BattleHUD`, `Assets/TheCat/Prefabs`, or `Assets/TheCat/Scenes` during candidate review.

## Source Truth

- UI style source truth: Feishu `Qr1XdXd6KosnjMxjgW7cS89kn9c`.
- Character design truth remains locked to local source-lock packets from Feishu `IAdkdcpciobUTXxa7dBcRx7Bngf`.
- Do not generate, crop, recolor, import, or bind starter-cat body art in this batch.

## Review Inputs

- `thecat_ui_battle_hud_batch87_manifest.csv`
- `thecat_ui_battle_hud_batch87_review_sheet_v001.png`
- `thecat_ui_battle_hud_batch87_contact_sheet_v001.png`
- `mockups/thecat_ui_battle_hud_battle_hud_dense_1024x768_local_mockup_v001.png`
- `mockups/thecat_ui_battle_hud_battle_hud_1920x1080_local_mockup_v001.png`
- `thecat_ui_battle_hud_batch87_candidate_review.md`
- `thecat_ui_battle_hud_batch87_process_note.md`
- `design/development/asset_review/BATCH87_BATTLE_HUD_AGENT_REVIEW_2026-06-25.md`

## Required Checks

- Confirm all four core gauges are present in the 1024x768 dense mockup.
- Confirm runtime pause/speed/restart controls do not displace gauges.
- Confirm enemy/status panel placement does not obviously hide the central playfield in local mockups.
- Confirm skill tray and slot language remain consistent with existing skill HUD and Batch 80/81 icon-frame work.
- Confirm all candidate PNGs stay outside `Assets/` and have no Unity `.meta` files.
- Keep remaining Unity gates open until there are Unity-rendered screenshots, import settings, binding proof, click-target proof, enemy telegraph proof, and clean Console evidence.
