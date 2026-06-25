# P0 Asset Batch 27 - Core Gauge Bars

## Task Scope

Generate and wire the P0 four-core HUD gauge bars:

- owner sleep frame/fill
- cat HP frame/fill
- team poop frame/fill
- team hunger frame/fill

This is a non-cat UI asset batch. Do not modify starter cat sprites,
turnaround sources, candidate packs, formal import gates, or cat silhouette
assets.

## Required Reading

- `design/development/prompts/p0_core_gauge_bars.md`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/asset_review/P0_RUNTIME_VISUAL_REVIEW_PACKET.md`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/GrayboxBattleController.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0ImGuiVisualAssetDrawer.cs`

## Expected Output

- Eight PNGs under `Assets/TheCat/Art/UI/Frames`.
- Eight `.png.meta` files with P0 Sprite import settings.
- Manifest, runtime binding catalog, batch catalog, contact sheet, coverage
  tests, and development records updated.
- Core HUD runtime drawing uses the gauge frame/fill assets.

## Forbidden Changes

- Do not change files under `design/梦境支配者核心玩法/assets/characters`.
- Do not overwrite any starter cat PNG in `Assets/TheCat/Art/Characters`.
- Do not approve formal starter cat import.
- Do not add cat body, face, fur, costume, or turnaround-derived marks to any
  gauge asset.

## Acceptance

- `P0AssetManifestCatalog.P0ManifestAssetCount == 103`.
- `P0VisualAssetCatalog.P0RuntimeVisualBindingCount == 99`.
- `P0AssetGenerationBatchCatalog` includes Batch 27.
- Runtime visual contact sheet reports 99 bindings.
- All eight new PNGs are exactly `384x48`.
- All eight new meta files include
  `batch:p0_asset_batch_27_core_gauge_bars`,
  `spriteBorder:10`, and `nonCatSymbolicOnly:true`.
- Offline C# compilation and `git diff --check` pass.

