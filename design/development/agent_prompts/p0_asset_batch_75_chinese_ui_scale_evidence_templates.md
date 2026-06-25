# Agent Prompt - P0 Asset Batch 75 Chinese UI Scale Evidence Templates

## Task Scope

Produce a validation-only non-cat UI evidence packet for the Chinese UI
responsive-scale pass. This batch exists to help Unity Play Mode screenshot
review land in a consistent evidence folder.

The packet must include:

- one capture-matrix checklist sheet
- one transparent safe-area / overlap ruler
- one per-surface evidence note card
- one resolution checklist strip
- one 20-row surface/resolution capture matrix CSV
- one manifest CSV
- one review sheet PNG
- one candidate review note
- one process note
- one scoped validator

This batch is evidence-template work only. It must not install anything into
Unity and must not change runtime visual catalogs, scenes, prefabs, manifests,
or queue counts.

## Required Reading

- `Assets/TheCat/Scripts/Runtime/Tools/P0ChineseUiScaleValidationPlan.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0ChineseUiCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0ArchitectureCompletionAudit.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0ImGuiLayout.cs`
- `design/development/UNITY_VALIDATION_BACKLOG.md`
- `design/development/P0_ARCHITECTURE_COMPLETION_AUDIT_2026-06-15.md`

## Expected Output

- Output directory:
  `design/development/asset_candidates/ui/chinese_ui_scale_evidence/batch_75_chinese_ui_scale_evidence_templates_2026-06-20`
- Four generated PNG templates.
- `chinese_ui_scale_batch75_manifest.csv`
- `chinese_ui_scale_batch75_capture_matrix.csv`
- `thecat_ui_chinese_scale_batch75_review_sheet.png`
- `chinese_ui_scale_batch75_candidate_review.md`
- `chinese_ui_scale_batch75_process_note.md`
- `design/development/tools/build_chinese_ui_scale_evidence_templates.py`
- `design/development/tools/validate_chinese_ui_scale_evidence_templates.ps1`

## Do Not Modify

- Do not write to `Assets/TheCat/Art` or any other Unity runtime asset folder.
- Do not create Unity `.meta` files.
- Do not modify `P0AssetManifestCatalog`, `P0VisualAssetCatalog`,
  `P0AssetProductionQueueCatalog`, scenes, prefabs, or runtime bindings.
- Do not read, crop, recolor, regenerate, or route starter-cat body art,
  colored turnarounds, fur markings, costumes, or props.

## Acceptance Criteria

- The manifest has exactly four template rows.
- The capture matrix has exactly 20 rows: five P0 UI surfaces multiplied by
  four required resolutions.
- Required resolutions are `1024x768`, `1280x720`, `1600x900`, and
  `1920x1080`.
- Required surfaces are main menu / character select, 10-layer route map,
  battle HUD, skill/enemy HUD, and result / pause settings.
- All template paths stay outside `Assets`.
- All hashes and dimensions are validated.
- The batch folder contains no Unity `.meta` files.
- The review note states validation-only status, non-cat scope, Unity MCP /
  manual Editor screenshot follow-up, and no runtime import.

## Validation

- `python design/development/tools/build_chinese_ui_scale_evidence_templates.py`
- `powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_chinese_ui_scale_evidence_templates.ps1`
- `git diff --check`
- Unity MCP/editor follow-up when available:
  - capture all 20 surface/resolution screenshots
  - confirm Console has no errors
  - compare screenshots against the safe-area overlay and note card
  - save filled evidence under a Unity validation evidence folder

