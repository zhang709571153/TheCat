# Agent Prompt: P0 Asset Batch 20 Shop Item Cards

## Task Scope

Produce and integrate four non-cat P0 shop item cards for the route-map shop reward choices:

- `shop_bed_patch`
- `shop_litter_sachet`
- `shop_late_kibble`
- `shop_free_sample`

This is a UI asset integration task. Do not edit starter cat assets or create cat derivatives.

## Read First

- `design/梦境支配者核心玩法/docs`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/P0_ARCHITECTURE_CODE_STATUS_2026-06-14.md`
- `design/development/prompts/p0_shop_item_cards.md`
- `Assets/TheCat/Scripts/Runtime/Roguelite/P0RouteRewardResolver.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0RouteMapPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/RouteMapController.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetManifestCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RuntimeVisualBindingCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RouteMapSurfaceCoverage.cs`

## Expected Output

- Four `384x160` transparent PNG cards in `Assets/TheCat/Art/UI/Cards`.
- Matching `.png.meta` files using Sprite import settings and `TheCatP0ImportSettings:v1`.
- Manifest rows, batch catalog entries, runtime visual bindings, route-map presenter/controller integration, and coverage tests.
- Review note in `design/development/asset_review/p0_shop_item_cards_2026-06-14.md`.
- Development log and validation backlog updates.

## Forbidden Scope

- Do not modify starter cat combat sprites, three-view turnaround references, candidate cat derivatives, or any source-lock packets.
- Do not change shop reward mechanics or economy values.
- Do not broaden the route map UI beyond the optional item-card visual slot needed for this batch.
- Do not use cat body shapes, cat markings, paws, ears, tails, or starter-cat civilization/costume motifs.

## Acceptance Criteria

- `P0AssetManifestCatalog.P0ManifestAssetCount == 82`.
- `P0VisualAssetCatalog.P0RuntimeVisualBindingCount == 78`.
- `P0AssetManifestCoverage`, `P0RuntimeVisualBindingCoverage`, `P0RouteMapSurfaceCoverage`, `P0AssetGenerationBatchCoverage`, and `P0AssetReviewPacket` cover this batch.
- The route-map shop surface resolves `ItemCardAsset` for bed patch, litter sachet, late kibble, and free sample choices.
- The generated PNGs are exactly `384x160`, transparent, readable, and non-cat.
- No Unity Console validation is claimed unless Unity MCP or editor-side tests are actually available and run.

## Verification

- Run `design/development/tools/build_shop_item_cards.ps1`.
- Run `design/development/tools/build_runtime_visual_contact_sheet.ps1`.
- Verify dimensions and `.meta` userData for all four card assets.
- Build `TheCat.Runtime.csproj`, `TheCat.EditModeTests.csproj`, and `TheCat.Editor.csproj` with Visual Studio MSBuild using `Temp/bin` and `Temp/obj` output paths.
- Run `git diff --check`.
