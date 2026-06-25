# P0 Asset Batch 06 Route Node Icons Agent Prompt

## Task Scope

Produce and maintain the P0 route-map node icon set for the 10-layer roguelite
route. This batch is for UI route readability and does not produce starter-cat
character art.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/01_core_gameplay/core_gameplay_rules_and_player_path.md`
- `design/梦境支配者核心玩法/docs/00_overview/p0_minimum_design.md`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/P0_ASSET_MANIFEST.csv`
- `design/development/prompts/p0_route_node_icons.md`

## Code Files To Read

- `Assets/TheCat/Scripts/Runtime/Roguelite/RouteNodeType.cs`
- `Assets/TheCat/Scripts/Runtime/Roguelite/P0RouteNodePresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0RouteMapPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetGenerationBatchCatalog.cs`
- `Assets/TheCat/Tests/EditMode/P0VisualAssetCatalogTests.cs`
- `Assets/TheCat/Tests/EditMode/P0RouteMapSurfaceCoverageTests.cs`

## Expected Output

- 7 transparent 128x128 PNG icons under:
  `Assets/TheCat/Art/UI/Icons/`
- Matching `.png.meta` files using P0 Sprite import settings.
- Manifest rows and runtime visual bindings for:
  - `route.defense_node`
  - `route.elite_node`
  - `route.partner_node`
  - `route.shop_node`
  - `route.dream_event_node`
  - `route.blessing_node`
  - `route.rest_nest_node`
- Route node presenter wiring so all 8 node types, including Boss, expose a
  `P0VisualAssetReference`.
- Updated development log and validation backlog.

## Prohibited Changes

- Do not modify starter cat assets or colored turnaround source files.
- Do not replace `thecat_ui_route_bossnode_icon_128_v001.png`; Boss remains
  Call Tyrant source-locked.
- Do not change route topology, rewards, battle tuning, or scene flow.
- Do not introduce generated assets outside the manifest/runtime binding flow.

## Acceptance Criteria

- `P0AssetManifestCoverage.EvaluateP0Manifest()` passes.
- `P0AssetImportReadiness.EvaluateP0Manifest()` passes.
- `P0AssetMetaImportSettingsReadiness.EvaluateP0Manifest()` passes.
- `P0RuntimeVisualBindingCoverage.EvaluateP0Bindings()` passes.
- `P0RouteMapSurfaceCoverage.EvaluatePrototypeRouteMap()` passes.
- Route node cards and branch option cards expose icon assets for the node
  types they present.
- Starter-cat formal import remains blocked unless separately approved by the
  active-cat screenshot review gate.

## Validation

Local commands:

```powershell
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' TheCat.Runtime.csproj /nologo /v:minimal
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' TheCat.EditModeTests.csproj /nologo /v:minimal
git diff --check
```

Unity validation after editor tooling is available:

1. Refresh AssetDatabase.
2. Run `TheCat/P0/Write P0 Asset Review Packet`.
3. Run `TheCat/P0/Write P0 Visual Acceptance Report`.
4. Capture route-map screenshots and verify all node icons are visible and
   readable without changing route behavior.
