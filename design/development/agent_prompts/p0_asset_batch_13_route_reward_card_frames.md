# Agent Prompt: P0 Asset Batch 13 Route Reward Card Frames

## Task Scope

Generate and wire deterministic non-cat route reward card frame assets for
partner, shop, blessing, dream-event, and rest-nest route reward choices.

## Required Reading

- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/prompts/p0_route_reward_card_frames.md`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetGenerationBatchCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0RouteMapPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/RouteMapController.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RouteMapSurfaceCoverage.cs`

## Expected Outputs

- Five `512x256` transparent PNG route card frames under
  `Assets/TheCat/Art/UI/Frames`.
- Matching `.png.meta` files with `TheCatP0ImportSettings:v1`.
- Updated manifest CSV and runtime manifest catalog.
- Updated batch catalog and batch order coverage.
- Runtime bindings for all five `route_reward_card.*` entries.
- Route-map presenter/controller support for drawing reward choices with these
  frames.
- Updated tests, review packet, contact sheet, development log, and Unity
  validation backlog.

## Forbidden Areas

- Do not modify starter cat sprites, colored three-view turnarounds, candidate
  cat sheets, source-lock packet hashes, or formal starter-cat import state.
- Do not replace route choice icons from Batch 12 unless the task explicitly
  includes a Batch 12 revision.
- Do not add final UGUI/prefab bindings in this batch.

## Acceptance Criteria

- `P0AssetManifestCoverage.EvaluateP0Manifest()` passes with the updated
  manifest total.
- `P0AssetGenerationBatchCoverage.EvaluateP0Batches()` passes with Batch 13
  after Batch 12.
- `P0RuntimeVisualBindingCoverage.EvaluateP0Bindings()` passes with the updated
  runtime binding total.
- `P0RouteMapSurfaceCoverage.EvaluatePrototypeRouteMap()` proves all five
  non-battle route node types resolve the correct card frame.
- Runtime, EditModeTests, and Editor MSBuild compile.
- Unity follow-up backlog records the pending AssetDatabase, Console, and
  route-map screenshot checks.
