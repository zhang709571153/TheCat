# P0 Asset Batch 63 - Runtime Control Panel Candidates

## Task Scope

Produce a candidate-only non-cat UI panel pack for P0 runtime controls:

- pause overlay panel
- speed segmented control
- restart confirmation plate
- keyboard hint strip

This batch extends the Batch 62 runtime control icon lane into larger HUD
surfaces. It does not install assets into Unity and does not update runtime
visual binding counts.

## Relevant Design And Code

- `D:\Unity Workspace\TheCat\design\姊﹀鏀厤鑰呮牳蹇冪帺娉昞docs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0RuntimeSettingsPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/GrayboxBattleController.cs`
- `Assets/TheCat/Scripts/Runtime/Input/P0KeyboardInputMap.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetProductionQueueCatalog.cs`
- `design/development/asset_candidates/ui/runtime_controls/batch_62_runtime_control_icon_candidates_2026-06-15`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`

## Expected Output

- Candidate directory:
  `design/development/asset_candidates/ui/runtime_controls/batch_63_runtime_control_panel_candidates_2026-06-15`
- Four transparent PNG candidates:
  - `768x432` pause overlay panel
  - `512x128` speed segmented control
  - `512x256` restart confirmation plate
  - `768x128` keyboard hint strip
- Manifest:
  `runtime_control_panels_batch63_manifest.csv`
- Review sheet:
  `thecat_ui_runtime_control_panels_batch63_review_sheet.png`
- Candidate review:
  `runtime_control_panels_batch63_candidate_review.md`
- Process note:
  `runtime_control_panels_batch63_process_note.md`
- Validator:
  `design/development/tools/validate_runtime_control_panel_candidates.ps1`

## Forbidden Scope

- Do not write any Batch 63 candidate into `Assets`.
- Do not create Unity `.meta` files for this candidate pack.
- Do not modify starter-cat body art, combat sprites, HUD avatars, colored
  turnarounds, prefabs, scenes, or runtime visual bindings.
- Do not update `P0AssetManifestCatalog.P0ManifestAssetCount`.
- Do not update `P0VisualAssetCatalog.P0RuntimeVisualBindingCount`.
- Do not use cat fur patterns, cat costume fragments, or cat body silhouettes.

## Acceptance Criteria

- The candidate pack has four rows with recommendation
  `candidate_review_only_do_not_import`.
- All candidate PNGs have the exact expected dimensions and transparent
  corners.
- All candidates remain outside `Assets`.
- The review note clearly states candidate-only status and Unity pending checks.
- `P0AssetProductionQueueCatalog` records the batch as complete pending Unity
  review.
- Runtime and EditMode MSBuild pass after queue/test updates.

## Validation

Run:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_runtime_control_panel_candidates.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' 'TheCat.Runtime.csproj' /t:Rebuild /p:Configuration=Debug /v:minimal /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' 'TheCat.EditModeTests.csproj' /t:Rebuild /p:Configuration=Debug /v:minimal /nologo
git diff --check
```

Unity follow-up when MCP editor tools are exposed:

- inspect pause overlay and speed selector readability in battle HUD
- verify restart confirmation does not conflict with normal pause controls
- verify keyboard hint strip remains readable at gameplay scale
- verify no Console errors
- decide whether to promote the panel set into `Assets/TheCat/Art/UI`
