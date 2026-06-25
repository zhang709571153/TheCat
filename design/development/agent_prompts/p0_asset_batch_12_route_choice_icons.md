# Agent Prompt: P0 Asset Batch 12 Route Choice Icons

## Task Scope

Produce and integrate deterministic non-cat route choice icons for P0 roguelite
reward-choice surfaces:

- partner recruit choice
- shop purchase supply choice
- gain authority blessing choice
- upgrade authority blessing choice
- rest-nest recovery choice
- dream-event next-battle modifier choice

This batch is limited to route-map UI choice icons and route-choice surface
binding. It must not generate, replace, or reinterpret starter cat body art.

## Source Of Truth

- `D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\docs`
- `design/development/prompts/p0_route_choice_icons.md`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.md`

## Code And Data To Read

- `Assets/TheCat/Scripts/Runtime/Roguelite/P0RouteRewardResolver.cs`
- `Assets/TheCat/Scripts/Runtime/Roguelite/RouteRewardChoiceType.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0RouteMapPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/RouteMapController.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetGenerationBatchCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetManifestCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RuntimeVisualBindingCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RouteMapSurfaceCoverage.cs`
- `design/development/tools/build_route_choice_icons.ps1`
- `design/development/tools/build_runtime_visual_contact_sheet.ps1`

## Expected Output

- Six PNG assets under `Assets/TheCat/Art/UI/Icons`.
- Matching `.png.meta` files using `TheCatP0ImportSettings:v1`.
- Manifest, batch catalog, runtime binding catalog, route-map reward surface,
  coverage tools, review packet, tests, and contact sheet updated to the new
  asset and runtime binding counts.
- Development log, art pipeline notes, architecture status, and Unity validation
  backlog updated with Batch 12 results.

## Forbidden Scope

- Do not edit `Assets/TheCat/Art/Characters/Sprites`.
- Do not move starter cat derivative candidates into `Assets`.
- Do not remove or weaken source locks for Saiban, Nephthys, or Suzune.
- Do not change route reward economy or blessing behavior except to attach
  visual references to the existing choice-type presentation.

## Acceptance Criteria

- `P0AssetManifestCoverage.EvaluateP0Manifest()` passes with 57 manifest rows.
- `P0AssetGenerationBatchCoverage.EvaluateP0Batches()` passes with the Batch 12
  route choice icon batch after Batch 11.
- `P0RuntimeVisualBindingCoverage.EvaluateP0Bindings()` passes with 53 runtime
  bindings and 53 resolved textures.
- `P0RouteMapSurfaceCoverage.EvaluateP0RouteMapSurface()` confirms reward
  choices expose route-choice icons.
- `P0AssetReviewPacket.EvaluateP0Packet()` passes with 57 review assets and 53
  runtime-bound entries.
- Starter cat formal import remains blocked until active-cat screenshots are
  approved against the colored three-view turnarounds.

## Validation Commands

```powershell
& .\design\development\tools\build_route_choice_icons.ps1
& .\design\development\tools\build_runtime_visual_contact_sheet.ps1
git diff --check
& "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" .\TheCat.Runtime.csproj /nologo /verbosity:minimal /p:OutDir="Temp\bin\RuntimeBatch12RouteChoiceIcons\"
& "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" .\TheCat.EditModeTests.csproj /nologo /verbosity:minimal /p:OutDir="Temp\bin\EditModeBatch12RouteChoiceIcons\"
& "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" .\TheCat.Editor.csproj /nologo /verbosity:minimal /p:OutDir="Temp\bin\EditorBatch12RouteChoiceIcons\"
```

If Unity MCP is available, also refresh the AssetDatabase, run Console checks,
open `P0RouteMap`, select a Dream Event / Shop / Blessing / Rest Nest node, and
capture screenshots showing the reward choice icons.
