# Batch 87 Battle HUD Unity Preflight Console Blockers

Date: 2026-06-25

## Decision

- Batch 87 candidate-only Unity import preflight passed.
- Batch 87 focused EditMode tests passed through M2 gate hardening.
- Batch 87 runtime screenshot evidence now proves 6/6 candidate texture draws
  with fallback=0 at all four target resolutions.
- This is not a clean Console acceptance.
- Do not create or count `design/development/asset_review/batch_87_battle_hud_unity_preflight/console_clean_report.md` until the Unity log is free of new errors.

## Evidence

- Import/apply/validation log:
  `Logs/batch87_battle_hud_unity_preflight_20260625.log`
- Gate-semantics fix validation log:
  `Logs/batch87_battle_hud_unity_preflight_20260625_k1_fix.log`
- Focused EditMode XML:
  `Logs/p0_batch87_battle_hud_preflight_editmode_20260625_m2_gate_hardening_noquit.xml`
- Focused EditMode log:
  `Logs/p0_batch87_battle_hud_preflight_editmode_20260625_m2_gate_hardening_noquit.log`
- Runtime evidence log:
  `Logs/batch87_battle_hud_runtime_evidence_20260625_m2_visual_retry.log`
- M2 preflight log:
  `Logs/p0_batch87_battle_hud_unity_preflight_20260625_m2_gate_hardening.log`
- Preflight report:
  `design/development/asset_review/BATCH87_BATTLE_HUD_UNITY_PREFLIGHT_REPORT_2026-06-25.md`

## Passing Checks

- Unity generated `.meta` files for all 6 candidate-only preflight imports under
  `Assets/TheCat/Art/UI/BattleHUD`.
- `P0AssetImportSettingsApplier` applied P0 import settings to 6 importer(s)
  on the first pass; the K1 fix rerun found 0 changed importer(s), as expected.
- `P0AssetImportSettingsValidator` validated 6 generated/imported asset(s) with
  0 warnings.
- Focused Unity EditMode test run passed `17/17` for
  `P0BattleHudBatch87UnityPreflightTests`.
- The M2 gate hardening verifies that forged Console markdown, missing runtime
  logs, dirty runtime logs, and runtime reports without candidate-draw tokens do
  not allow formal install.
- Runtime evidence regenerated all four target screenshots and records
  `Candidate frame draws: 6/6` plus `No candidate texture fallback: yes`.

## Console-Clean Blockers

The focused Unity EditMode log still contains editor/environment errors:

- `[Licensing::Client] Error: HandshakeResponse reported an error`
- `[Licensing::Module] Error: Failed to handshake to channel: "LicenseClient-PC"`
- `[Licensing::Module] Error: Access token is unavailable; failed to update`
- `[Licensing::Client] Error: Code 404 while processing request`
- `d3d12: failed to query info queue interface (0x80004002)`
- `##utp:{"type":"MemoryLeaks"...}`
- `~StackAllocator(ALLOC_TEMP_MAIN) m_LastAlloc not NULL`

These did not fail the focused EditMode run, but they block a clean Console
claim for Batch 87.

## Remaining Formal Install Evidence

- Clean Console report.
- Explicit human review approval.
- Formal runtime scene/presenter binding decision.
