# Batch 88 Character Select Console / Binding Blockers

Status: blocker record only. This is not formal Unity evidence and must not be
renamed to `scene_binding_console_clean_report.md`.

## Current Evidence

- Runtime evidence log:
  `Logs/batch88_character_select_runtime_evidence_20260625_l2_overlay_only.log`
- Preflight refresh log:
  `Logs/p0_batch88_character_select_unity_preflight_20260625_runtime_evidence_l2.log`
- Runtime report:
  `design/development/asset_review/batch_88_character_select_unity_preflight/runtime_evidence_report.md`
- Preflight report:
  `design/development/asset_review/BATCH88_CHARACTER_SELECT_UNITY_PREFLIGHT_REPORT_2026-06-25.md`

## What Passed

- Batch 88 runtime evidence runner completed.
- Four target screenshots exist at `1920x1080`, `1365x768`, `1280x720`, and
  `1024x768`.
- Candidate draw audit records `6/6` candidate textures and `fallback=0`.
- Source-locked HUD avatars, selected card state, idle card states, Chinese
  text density, and click targets have automatic review evidence.

## Why The Clean Report Is Still Blocked

The current Unity logs contain Console-noise tokens that make a clean Console
claim unsafe:

- `Logs/batch88_character_select_runtime_evidence_20260625_l2_overlay_only.log`
  contains licensing errors including `[Licensing::Client] Error:` and
  `[Licensing::Module] Error:`.
- The same runtime log contains Unity AI Assistant tracing noise.
- `Logs/p0_batch88_character_select_unity_preflight_20260625_runtime_evidence_l2.log`
  contains licensing errors and `Curl error 42`.

Because of those tokens, do not create
`design/development/asset_review/batch_88_character_select_unity_preflight/scene_binding_console_clean_report.md`.

## Remaining Gate

`scene_binding_console_clean_report.md` can be written only after a later
Batch 88 validation run proves all of the following:

- `Runtime scene/presenter binding: yes`
- `Console clean: yes`
- `Unity log reviewed: yes`
- `Batch 88 runtime evidence log: <existing log path>`
- the referenced log contains
  `[TheCat] Batch 88 character-select runtime evidence passed:`
- the referenced log has no known failure or Console-noise tokens.

`human_review_approval.md` remains a separate manual approval gate and is not
covered by this blocker record.
