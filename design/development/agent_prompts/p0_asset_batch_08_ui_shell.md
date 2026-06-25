# Agent Prompt: P0 Asset Batch 08 UI Shell

## Task Scope

Produce and integrate the non-cat P0 UI shell asset batch:

- main menu dream-entry background
- title logo frame
- dreamglass panel frame
- primary button frame
- fish treat reward icon
- dream shard reward icon

This batch is limited to UI shell assets. It must not generate, replace, or
reinterpret starter cat body art.

## Source Of Truth

- `D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\docs`
- `design/development/prompts/p0_ui_shell_assets.md`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.md`

## Code And Data To Read

- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetGenerationBatchCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetManifestCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetImportReadiness.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetMetaImportSettingsReadiness.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RuntimeVisualBindingCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetReviewPacket.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetProductionReadiness.cs`
- `design/development/tools/build_ui_shell_assets.ps1`
- `design/development/tools/build_runtime_visual_contact_sheet.ps1`

## Expected Output

- Six PNG assets under `Assets/TheCat/Art/UI`.
- Matching `.png.meta` files using `TheCatP0ImportSettings:v1`.
- Manifest, runtime binding, batch catalog, review packet, production readiness,
  and tests updated to the new asset and runtime binding counts.
- Runtime visual contact sheet regenerated and listing all 34 runtime bindings.
- Development log and visual pipeline notes updated with the batch result.

## Forbidden Scope

- Do not edit `Assets/TheCat/Art/Characters/Sprites` starter cat runtime files.
- Do not move starter cat derivative candidates into `Assets`.
- Do not remove or weaken source locks for Saiban, Nephthys, or Suzune.
- Do not change combat, route, or core value behavior unless a compile issue
  directly requires a local naming fix.

## Acceptance Criteria

- `P0AssetManifestCoverage.EvaluateP0Manifest()` passes with 38 manifest rows.
- `P0AssetImportReadiness.EvaluateP0Manifest()` passes with 38 required files.
- `P0AssetMetaImportSettingsReadiness.EvaluateP0Manifest()` passes with 38
  matching meta files.
- `P0RuntimeVisualBindingCoverage.EvaluateP0Bindings()` passes with 34 runtime
  bindings and 34 resolved textures.
- `P0AssetReviewPacket.EvaluateP0Packet()` passes with 38 review assets and 34
  runtime-bound entries.
- `P0AssetProductionReadiness.EvaluateP0OfflineReadiness()` passes while starter
  cat formal import remains blocked until active-cat screenshots are approved.

## Validation Commands

```powershell
& .\design\development\tools\build_ui_shell_assets.ps1
& .\design\development\tools\build_runtime_visual_contact_sheet.ps1
dotnet build .\TheCat.sln
```

If Unity MCP is available, also refresh the AssetDatabase, run the P0 offline
asset production readiness menu item, inspect Console errors, and capture the
runtime contact sheet plus Play Mode screenshots for UI overlap review.
