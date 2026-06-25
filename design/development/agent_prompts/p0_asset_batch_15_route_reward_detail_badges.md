# Agent Prompt: P0 Asset Batch 15 Route Reward Detail Badges

## Task Scope

Create and wire the 192x64 route reward detail badge batch for P0 route reward
choices. This is a deterministic non-cat UI batch for gain, cost, recovery,
risk, and upgrade effects.

## Design / Spec Sources

Read:

- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/prompts/p0_route_reward_detail_badges.md`
- `design/development/P0_ASSET_MANIFEST.csv`
- `design/development/P0_ARCHITECTURE_CODE_STATUS_2026-06-14.md`

Relevant requirement: route reward choices should communicate the effect class
at a glance while keeping cat assets locked to colored three-view turnarounds.

## Code / Data To Read

- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetGenerationBatchCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0RouteMapPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/RouteMapController.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0ImGuiVisualAssetDrawer.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RouteMapSurfaceCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RuntimeVisualBindingCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetManifestCoverage.cs`
- `design/development/tools/build_runtime_visual_contact_sheet.ps1`

## Expected Output

- Five generated 192x64 PNG badges under `Assets/TheCat/Art/UI/Badges`.
- Matching Unity `.meta` files with the P0 import marker and Batch15 userData.
- Manifest/catalog/runtime binding updates.
- Route reward card drawing shows the detail badge on the right side.
- Coverage tests and screenshot smoke plan include the new runtime bindings.
- Review note under `design/development/asset_review`.
- Development log entry documenting this batch.

## Do Not Modify

- Any starter cat turnaround, locked starter cat sprite, starter cat candidate,
  or cat source reference image.
- Enemy source concept or animation references.
- Reward economy logic, route node topology, or combat numeric tuning.
- Existing route-card frame assets.

## Acceptance Checks

Run or verify:

- Generate assets with `design/development/tools/build_route_reward_detail_badges.ps1`.
- Regenerate runtime contact sheet with `design/development/tools/build_runtime_visual_contact_sheet.ps1`.
- Confirm all five badge PNG files are exactly `192x64`.
- Confirm `.meta` files contain `TheCatP0ImportSettings:v1` and
  `batch:p0_asset_batch_15_route_reward_detail_badges`.
- Confirm `P0AssetManifestCatalog.P0ManifestAssetCount == 72`.
- Confirm `P0VisualAssetCatalog.P0RuntimeVisualBindingCount == 68`.
- Compile runtime/editor/test assemblies or run Unity EditMode tests when
  available.
- Keep Unity MCP / Play Mode screenshot validation as follow-up if MCP is
  unavailable.
