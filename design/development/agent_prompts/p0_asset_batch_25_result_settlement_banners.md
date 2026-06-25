# P0 Asset Batch 25 Result Settlement Banners Agent Prompt

## Task Scope

Produce and wire the Batch25 non-cat outcome banners for battle result and run
settlement surfaces in `D:\Unity Workspace\TheCat`.

## Source Documents

- `design/梦境支配者核心玩法/docs`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/prompts/p0_result_settlement_banners.md`
- `design/development/P0_ASSET_MANIFEST.csv`

## Code To Read

- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0BattleResultPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0RouteMapPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/GrayboxBattleController.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/RouteMapController.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0BattleResultCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RouteMapSurfaceCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RuntimeVisualBindingCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetManifestCoverage.cs`

## Expected Output

- Four 512x160 transparent PNG banners under `Assets/TheCat/Art/UI/Banners`.
- Matching `.png.meta` files with P0 sprite settings and Batch25 userData.
- Manifest, runtime catalog, batch catalog, coverage tests, contact sheet, and
  review packet updates.
- Review note at
  `design/development/asset_review/p0_result_settlement_banners_2026-06-14.md`.

## Forbidden Scope

- Do not modify starter-cat body art, source locks, formal import gates, or
  colored three-view turnaround rules.
- Do not generate new Saiban, Nephthys, Suzune, Shadowmaru, Mianhua, or Yuheng
  character portraits.
- Do not embed text into pixels; presenter/UI code controls labels.
- Do not perform broad scene or prefab refactors.

## Acceptance

- `P0VisualAssetCatalog.P0RuntimeVisualBindingCount == 91`.
- `P0AssetManifestCatalog.P0ManifestAssetCount == 95`.
- `P0BattleResultPresenter.HasP0BattleResultSurface()` requires outcome banner
  assets for resolved battle results.
- `P0RouteMapPresenter.BuildSurface()` exposes cleared and failed settlement
  outcome banners.
- `P0RuntimeVisualBindingCoverage` includes battle victory, battle defeat,
  cleared settlement, and failed settlement banner bindings.
- Runtime visual contact sheet reports 91 bindings.

## Validation

- Run `design/development/tools/build_result_settlement_banners.ps1`.
- Run `design/development/tools/build_runtime_visual_contact_sheet.ps1`.
- Verify all four PNG dimensions are 512x160.
- Verify all four `.png.meta` files contain
  `batch:p0_asset_batch_25_result_settlement_banners`,
  `spriteBorder:16`, `textureType: 8`, `spriteMode: 1`, and
  `nonCatSymbolicOnly:true`.
- Compile Runtime, EditModeTests, and Editor assemblies with MSBuild or Unity
  editor validation.
- If Unity MCP is available, run Console and Play Mode screenshot checks for
  `09-battle-result-layer1.png` and `10-settlement.png`.
