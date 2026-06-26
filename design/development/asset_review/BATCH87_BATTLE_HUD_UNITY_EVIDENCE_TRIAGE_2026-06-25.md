# Batch 87 Battle HUD Unity Evidence Triage

Date: 2026-06-25
Project: `D:\Unity Workspace\TheCat`
Queue id: `p0_asset_queue_battle_hud_preflight_candidates`
Candidate packet: `design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25`
Status: `candidate_complete_pending_unity_review`

## Decision

Batch 87 remains candidate-only. Do not import, bind, or mark it accepted from
this triage alone.

This pass confirms that the local Batch 87 packet is still intact and that M2
has candidate-backed four-resolution Unity screenshot evidence with explicit
candidate texture draw auditing. It does not prove clean Console status,
explicit human approval, or final formal runtime catalog binding.

## Current Evidence

| Evidence | Result | Notes |
| --- | --- | --- |
| Candidate validator | Pass | `design/development/tools/validate_ui_battle_hud_preflight_candidates.ps1` passed with 10 manifest rows. |
| Candidate no-meta policy | Pass | No `.meta` files were found under the Batch 87 candidate packet. |
| Manifest scope | Pass | 6 transparent sprite rows and 4 local mockup rows are present in `thecat_ui_battle_hud_batch87_manifest.csv`. |
| 1024x768 local four-gauge mockup | Pass for local mockup only | `thecat_ui_battle_hud_battle_hud_dense_1024x768_local_mockup_v001.png` is `1024x768` and visually shows all four core gauges after the earlier top-rail fix. |
| Current Play Mode baseline | Pass for existing runtime only | `P0_PLAYMODE_ACCEPTANCE_SMOKE_REPORT.md` reports `Result: passed`, `Smoke state: Passed`, and `8 passed check(s)` for the current I1 baseline. |
| Current battle HUD screenshot | Partial | `04-battle-hud-layer1.png` is `1920x1080`, shows the existing IMGUI/runtime HUD with Unity-rendered Chinese text and dynamic values, but it is not Batch 87 candidate-backed. |
| Current warning/telegraph screenshot | Partial | `09-call-tyrant-warning-vfx.png` is `1920x1080`; warning shapes remain readable beside the current HUD, but this still does not validate Batch 87 candidate binding. |

## Files Inspected

| File | Size / dimensions | Role |
| --- | --- | --- |
| `thecat_ui_battle_hud_battle_hud_dense_1024x768_local_mockup_v001.png` | `1024x768`, 1169554 bytes | Local dense mockup and four-gauge proof before Unity binding. |
| `thecat_ui_battle_hud_batch87_contact_sheet_v001.png` | `560x840`, 269722 bytes | Batch 87 sprite/contact overview. |
| `thecat_ui_battle_hud_batch87_review_sheet_v001.png` | `1440x1680`, 633316 bytes | Batch 87 review sheet. |
| `04-battle-hud-layer1.png` | `1920x1080`, 288763 bytes | Current I1 Play Mode battle HUD baseline. |
| `09-call-tyrant-warning-vfx.png` | `1920x1080`, 292709 bytes | Current I1 warning VFX/telegraph baseline. |

## Gate Matrix

| Gate | Current state | Evidence |
| --- | --- | --- |
| Local candidate packet complete | Passed | Validator passes with 10 rows; agent review already records production QA and follow-up visual review pass. |
| Candidate remains outside `Assets` | Passed | Candidate path is under `design/development/asset_candidates`; no candidate `.meta` files found. |
| Current battle HUD runtime screenshot exists | Passed for baseline | I1 Play Mode smoke captured `04-battle-hud-layer1.png`. |
| Current warning/telegraph screenshot exists | Passed for baseline | I1 Play Mode smoke captured `09-call-tyrant-warning-vfx.png`. |
| Unity-rendered Batch 87 candidate screenshots | Passed for preflight evidence | M2 captured all four target dimensions through the candidate-backed overlay. |
| 1024x768 Unity four-gauge proof | Passed for preflight evidence | `04-battle-hud-batch87-1024x768.png` shows all four top-rail gauges. |
| Chinese labels/tooltips/text/number replacement with Batch 87 | Partial | M1 proves dynamic gauge numbers and English placeholder review labels inside the candidate-backed overlay; final localized Chinese copy still needs human/product review before formal install. |
| Skill ready/selected/cooldown/disabled/low-resource readability with Batch 87 | Passed for preflight evidence | Automatic `text_and_skill_state_review.md` is generated only after all four screenshots pass. |
| Enemy spawn and telegraph occlusion with Batch 87 | Passed for preflight evidence | Automatic `telegraph_occlusion_click_target_review.md` is generated only after all four screenshots pass and target layouts validate non-overlap. |
| Pause/speed/restart click-target proof with Batch 87 | Partial | Runtime control cluster appears at all target sizes, but formal click-target acceptance still waits for clean Console and human approval. |
| Sprite import settings | Passed for preflight evidence | K1 validates 6/6 P0 Sprite import settings under `Assets/TheCat/Art/UI/BattleHUD`. |
| Candidate texture draw proof | Passed for preflight evidence | Runtime report records `Candidate frame draws: 6/6` and `No candidate texture fallback: yes`; every target capture records `fallback=0`. |
| Scene/prefab binding proof | Partial | Candidate-backed overlay validates the preflight binding path; formal runtime catalog binding remains blocked. |
| Clean Console after candidate validation | Missing | `console_clean_report.md` has not been created. |

## L1 Runtime Evidence Attempt

L1 added a candidate-backed runtime evidence runner, then attempted the four
required screenshot captures through Unity Editor execution. The attempt is
blocked because the local Unity Editor batchmode path captured each target as
`640x480` even when launched with `-screen-width/-screen-height`.

| Target | Log | Result |
| --- | --- | --- |
| `1920x1080` | `Logs/batch87_battle_hud_runtime_evidence_20260625_l1_1920x1080.log` | Blocked: captured `640x480`. |
| `1365x768` | `Logs/batch87_battle_hud_runtime_evidence_20260625_l1_1365x768.log` | Blocked: captured `640x480`. |
| `1280x720` | `Logs/batch87_battle_hud_runtime_evidence_20260625_l1_1280x720.log` | Blocked: captured `640x480`. |
| `1024x768` | `Logs/batch87_battle_hud_runtime_evidence_20260625_l1_1024x768.log` | Blocked: captured `640x480`. |

The preflight gate now rejects those named PNG files as invalid screenshot
evidence unless their actual PNG dimensions match the target resolution. Do not
create `text_and_skill_state_review.md`,
`telegraph_occlusion_click_target_review.md`, `console_clean_report.md`, or
`human_review_approval.md` from this L1 attempt.

## M1 Runtime Evidence Pass

M1 resolves the L1 screenshot-size blocker with a GameView custom-size override
and adds two additional guards: screenshot visual-content validation rejects
blank captures, and target-layout validation rejects inside-screen panel
overlap before automatic review evidence is written.

| Evidence | Result | Notes |
| --- | --- | --- |
| `01-battle-hud-batch87-1920x1080.png` | Pass | Captured at `1920x1080` through `Logs/batch87_battle_hud_runtime_evidence_20260625_m1_final_noquit.log`. |
| `02-battle-hud-batch87-1365x768.png` | Pass | Captured at `1365x768`. |
| `03-battle-hud-batch87-1280x720.png` | Pass | Captured at `1280x720`. |
| `04-battle-hud-batch87-1024x768.png` | Pass | Captured at `1024x768`; legacy battle HUD is suppressed and candidate panels do not overlap. |
| `text_and_skill_state_review.md` | Pass for automatic preflight review | Generated only after all four screenshots passed. |
| `telegraph_occlusion_click_target_review.md` | Pass for automatic preflight review | Generated only after all four screenshots passed. |
| `console_clean_report.md` | Missing | Do not create until Unity log blockers are reviewed and a clean Batch 87 validation log exists. |
| `human_review_approval.md` | Missing | Do not create without explicit human approval. |

The current preflight report therefore records `Runtime evidence: 6/8` and
`Formal install allowed: no`.

## M2 Gate Hardening Pass

M2 addresses independent review findings around evidence forgery and visual
readability. The runtime report now needs explicit candidate-draw tokens before
the four screenshots count, and the preflight gate rejects a self-asserted
`console_clean_report.md` unless it references an existing clean runtime log.

| Evidence | Result | Notes |
| --- | --- | --- |
| Candidate texture resolution | Pass | `BATCH87_BATTLE_HUD_RUNTIME_EVIDENCE_REPORT_2026-06-25.md` records `Validated Batch 87 candidate texture bindings: 6/6 candidate textures resolved`. |
| Per-target candidate draw audit | Pass | The same report records `6/6 candidate textures drawn; fallback=0` for `1920x1080`, `1365x768`, `1280x720`, and `1024x768`. |
| Forged/dirty Console markdown blocker | Pass | `P0BattleHudBatch87UnityPreflight` now reads the referenced runtime log and blocks missing logs, dirty logs, and logs without the Batch 87 pass token. |
| Focused tests | Pass | `Logs/p0_batch87_battle_hud_preflight_editmode_20260625_m2_gate_hardening_noquit.xml` passed `17/17`. |
| Runtime evidence run | Pass | `Logs/batch87_battle_hud_runtime_evidence_20260625_m2_visual_retry.log` regenerated the four screenshots. |
| Unity preflight run | Pass for preflight, blocked for formal install | `Logs/p0_batch87_battle_hud_unity_preflight_20260625_m2_gate_hardening.log` reports `Runtime evidence: 6/8` and `Formal install allowed: no`. |

The current runtime evidence log still contains licensing / Unity service error
lines, so it cannot be used to create `console_clean_report.md`.

## Boundary

- This packet may be used to start a formal Batch 87 candidate-backed Unity
  validation slice.
- This packet must not be used as a formal install decision.
- This packet does not alter starter-cat source locks.
- This packet does not retire the requirement for 1024x768, 1280x720,
  1365x768, and 1920x1080 candidate-backed screenshots.
- This packet does not replace the need for import settings and binding proof
  under `Assets/TheCat/Art/UI/BattleHUD`.

## Independent Review

| Agent | Scope | Finding |
| --- | --- | --- |
| `019efefd-6078-7dd1-b67d-79889d3c7e7d` / Euclid | Visual and evidence-chain review | No P0. The triage does not confuse current runtime baseline with Batch 87 candidate-backed evidence. The `1024x768` four-gauge proof is correctly limited to the local mockup. The remaining P1 evidence-chain risk is that candidate-backed Unity validation has not happened yet. |
| `019efefd-74aa-76e0-b2cd-924f52a7c1d3` / Hubble | Gate and boundary review | No P0/P1. The triage follows candidate-only and no-import-before-Unity-review boundaries. Recommended tightening wording from generic battle-HUD screenshots to Batch 87 candidate-backed screenshots, now applied in queue/checklist/matrix docs. |
| `019eff62-d244-73f1-b662-61b69946a7fc` / Beauvoir | Code and evidence-gate review | Found P1 gate risks: token-only Console markdown and screenshot evidence without proving candidate texture draws. M2 hardens both paths. |
| `019eff62-faf0-7093-9fcd-ae117fca3d9c` / Halley | Visual/documentation review | Found text/crowding and wording risks. M2 shortens low-width labels, keeps formal install blocked, and records candidate draw evidence without promoting runtime binding. |

## Next Actions

1. Resolve or explicitly document the Unity log blockers, then create
   `console_clean_report.md` only from a clean Batch 87 validation log.
2. Keep the M2 screenshot set review-only until clean Console and explicit
   human approval exist.
3. Ask for explicit human approval before creating
   `human_review_approval.md`.
4. Decide whether Batch 87 remains a preflight overlay or becomes the formal
   runtime battle-HUD binding; do not add candidate ids to
   `P0VisualAssetCatalog` before Console and human gates pass.
