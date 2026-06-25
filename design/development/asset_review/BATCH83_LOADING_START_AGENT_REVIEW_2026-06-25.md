# Batch 83 Loading/Start Agent Review

Packet date: 2026-06-25  
Updated: 2026-06-25 05:02 +08:00

Scope:
- `design/development/asset_candidates/ui/loading_start/batch_83_loading_start_preflight_2026-06-25/thecat_ui_loading_start_batch83_manifest.csv`
- `design/development/asset_candidates/ui/loading_start/batch_83_loading_start_preflight_2026-06-25/thecat_ui_loading_start_batch83_review_sheet_v001.png`
- `design/development/tools/build_ui_loading_start_preflight_candidates.py`
- `design/development/tools/validate_ui_loading_start_preflight_candidates.ps1`

## Result

No P0/P1 blockers remain. Batch 83 is candidate-only local preflight evidence for `ui_loading_start` and must not be treated as Unity acceptance. It is now visible in the P0 asset production queue/checklist as a Unity-review item, not as a formal `Assets/` promotion.

## Production Summary

Batch 83 is deterministic derivative art from existing local Qr1-style UI shell assets:
- main-menu dream-entry background
- existing title logo
- dreamglass panel
- primary button
- sleep gauge frame/fill
- sleep icon

Generated rows:
- 4 transparent candidate sprites: `progress_frame`, `progress_fill`, `spinner_crescent`, `dot_sequence`
- 4 local screen mockups: `1920x1080`, `1365x768`, `1280x720`, `1024x768`

This is not `image2` provenance. Strict `gpt-image-2` CLI generation remains blocked because `OPENAI_API_KEY` is not set in this shell.

## Visual / Style Review

Agent result: PASS_WITH_P2.

No P0/P1 blocker was found.

P2 watch items:
- `spinner_crescent` embeds the existing sleep icon, which contains a small sleeping cat symbol. It is not new starter-cat body/frame art and does not resemble Saiban, Nephthys, or Suzune, but Unity screenshots should confirm it is not misread as a character replacement.
- Mockups contain textless layout placeholders: the mid-panel line and bottom button frame need Unity-rendered text/state replacement before final acceptance.
- The `1024x768` mockup is tighter vertically. Unity screenshot review should focus on 4:3 or low-height windows.

## Production QA Review

Initial agent result: FAIL due to one P1 and one P2.

P1 finding fixed:
- `SPEC_PATH` originally pointed outside the Batch 83 candidate packet, so the builder could delete or rewrite `design/development/agent_prompts/p0_asset_batch_83_loading_start_preflight.md`.

Fix:
- `SPEC_PATH` now points to `design/development/asset_candidates/ui/loading_start/batch_83_loading_start_preflight_2026-06-25/thecat_ui_loading_start_batch83_agent_review_prompt.md`.
- The external generated prompt was removed.
- `clean_previous_outputs()` rejects shared-output cleanup paths outside the Batch 83 candidate directory.

P2 finding fixed:
- The validator now checks each manifest row's `contact_sheet`, `review_sheet`, `review_note`, and `process_note` fields against the fixed package paths.

Follow-up production QA result: PASS.

## Queue / Checklist Integration

Batch 83 was added to the P0 asset production queue as `p0_asset_queue_loading_start_preflight_candidates`.

This queue entry uses the manually maintained scoped prompt:
`design/development/agent_prompts/p0_asset_batch_83_loading_start_preflight.md`.

That file is intentionally separate from the builder-generated packet prompt. The builder remains restricted to the Batch 83 candidate directory.

Checklist evidence:
- At the time of Batch 83 integration, `design/development/asset_review/P0_ASSET_UNITY_VALIDATION_CHECKLIST.md` reported 12 queue items and 7 candidate-review items.
- Current F1 gate supersedes those historical counts: the checklist now reports 19 queue items and 14 candidate-review items while keeping Batch 83 candidate-only.
- Batch 83 required evidence explicitly includes Unity loading/start screenshots, spinner interpretation, placeholder-state replacement, 1024x768 crowding, import settings, binding, and Console validation.

## Queue Integration Review Agents

Asset-boundary reviewer: PASS with no P0/P1/P2 findings.
- Confirmed Batch 83 is only in the production queue as `CodexCandidateProduction` plus `CandidatePackCompletePendingUnityReview`.
- Confirmed `P0AssetManifestCatalog`, runtime visual bindings, `Assets/TheCat/Art/UI/LoadingStart`, and Batch 83 candidate `.meta` files contain no formal promotion evidence.

Queue/checklist consistency reviewer: initial FAIL due to one P1, now fixed.
- P1: `P0_RUNTIME_VISUAL_REVIEW_PACKET.md` top summary still had the pre-Batch-83 queue/candidate counts while its queue table already had Batch 83 and the updated 12/7 counts.
- Fix at the time: updated the top summary to 12 queue items and 7 completed candidate packs pending Unity review.
- Current superseding count: the Batch 83-90 queue/checklist pass now reports 19 queue items and 14 completed candidate packs pending Unity review.
- P2: older architecture and offline acceptance snapshots still contain historical 11/6 references; treat those as archival unless regenerated in a future architecture-report pass.

## Verified Commands

From project root:

```powershell
& "C:\Users\PC\.cache\codex-runtimes\codex-primary-runtime\dependencies\python\python.exe" "design/development/tools/build_ui_loading_start_preflight_candidates.py"
& "design/development/tools/validate_ui_loading_start_preflight_candidates.ps1"
& "design/development/tools/run_p0_noncat_candidate_validation_matrix.ps1"
```

Result:

```text
Wrote 8 Batch 83 loading/start preflight row(s).
Batch 83 loading/start preflight validation passed. Rows: 8
P0 non-cat candidate validation matrix complete: 31 passed, 0 failed
```

Current superseding matrix status after later Batch 89/90 validators: `38`
passed, `0` failed. The command result above is retained as historical Batch 83
evidence only.

Additional hygiene:

```text
python -m py_compile design/development/tools/build_ui_loading_start_preflight_candidates.py
git diff --check
```

Both passed. Temporary `__pycache__` output was removed.

## Remaining Gates

- Keep Batch 83 outside formal `Assets/` promotion until Unity-rendered loading/start screenshots pass.
- Confirm spinner readability and non-character interpretation in Unity.
- Confirm Unity text/state replacement for the textless panel/button placeholders.
- Confirm `1024x768` and other low-height layouts do not crowd the logo, progress, dots, and action area.
- Capture import settings, binding proof, and Console check before any acceptance claim.
