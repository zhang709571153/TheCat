# P0 Asset Batch 62 - Runtime Control Icon Candidates

## Task Scope

Produce a candidate-only non-cat UI icon pack for P0 runtime controls:

- pause
- resume
- speed 0.5x
- speed 1x
- speed 1.5x
- restart run

This batch supports the existing runtime settings and keyboard-control surface.
It does not install assets into Unity and does not update runtime visual binding
counts.

## Relevant Design And Code

- `D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\docs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0RuntimeSettingsPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/GrayboxBattleController.cs`
- `Assets/TheCat/Scripts/Runtime/Input/P0KeyboardInputMap.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetProductionQueueCatalog.cs`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`

## Expected Output

- Candidate directory:
  `design/development/asset_candidates/ui/runtime_controls/batch_62_runtime_control_icon_candidates_2026-06-15`
- Six transparent `128x128` PNG candidates.
- Manifest:
  `runtime_control_icons_batch62_manifest.csv`
- Review sheet:
  `thecat_ui_runtime_controls_batch62_review_sheet.png`
- Candidate review:
  `runtime_control_icons_batch62_candidate_review.md`
- Process note:
  `runtime_control_icons_batch62_process_note.md`
- Validator:
  `design/development/tools/validate_runtime_control_icon_candidates.ps1`

## Forbidden Scope

- Do not write any Batch 62 candidate into `Assets`.
- Do not create Unity `.meta` files for this candidate pack.
- Do not modify starter-cat body art, combat sprites, HUD avatars, colored
  turnarounds, prefabs, scenes, or runtime visual bindings.
- Do not update `P0AssetManifestCatalog.P0ManifestAssetCount`.
- Do not update `P0VisualAssetCatalog.P0RuntimeVisualBindingCount`.
- Do not use cat fur patterns, cat costume fragments, or cat body silhouettes.

## Acceptance Criteria

- The candidate pack has six rows with recommendation
  `candidate_review_only_do_not_import`.
- All candidate PNGs are `128x128` with transparent corners.
- All candidates remain outside `Assets`.
- The review note clearly states candidate-only status and Unity pending checks.
- `P0AssetProductionQueueCatalog` records the batch as complete pending Unity
  review.
- Runtime and EditMode MSBuild pass after queue/test updates.

## Validation

Run:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_runtime_control_icon_candidates.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' 'TheCat.Runtime.csproj' /t:Rebuild /p:Configuration=Debug /v:minimal /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' 'TheCat.EditModeTests.csproj' /t:Rebuild /p:Configuration=Debug /v:minimal /nologo
git diff --check
```

Unity follow-up when MCP editor tools are exposed:

- inspect runtime control readability in battle HUD
- verify no Console errors
- decide whether to promote the icon set into `Assets/TheCat/Art/UI/Icons`
