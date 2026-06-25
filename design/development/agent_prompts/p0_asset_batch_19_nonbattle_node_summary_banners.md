# Agent Prompt: P0 Asset Batch 19 Non-Battle Node Summary Banners

## Task Scope

Generate and verify the Batch 19 non-battle route node summary banners for `TheCat`.

Create exactly these assets:

- `Assets/TheCat/Art/UI/Banners/thecat_ui_node_shop_summary_banner_512x160_v001.png`
- `Assets/TheCat/Art/UI/Banners/thecat_ui_node_dreamevent_summary_banner_512x160_v001.png`
- `Assets/TheCat/Art/UI/Banners/thecat_ui_node_restnest_summary_banner_512x160_v001.png`

## Relevant Design And Code Paths

- `design/development/prompts/p0_nonbattle_node_summary_banners.md`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetGenerationBatchCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0RouteMapPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetManifestCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RouteMapSurfaceCoverage.cs`

## Allowed Changes

- Add or regenerate only the three Batch 19 PNGs and their `.meta` files.
- Update review evidence for this batch.
- Update catalog, runtime binding, and coverage tests only as needed for these three banners.

## Forbidden Changes

- Do not modify starter cat source assets, generated starter cat candidates, approved cat import directories, or cat manifest source-lock ids.
- Do not derive any part of these banners from cat bodies, cat face markings, cat costumes, or colored turnaround images.
- Do not change route gameplay behavior beyond surfacing the optional current-node banner asset.

## Expected Output

- Three transparent 512x160 PNG banners with matching Unity `.meta` files.
- Review note at `design/development/asset_review/p0_nonbattle_node_summary_banners_2026-06-14.md`.
- Catalog entries in manifest, batch catalog, runtime visual catalog, and route map surface coverage.

## Verification

- Run `design/development/tools/build_nonbattle_node_summary_banners.ps1`.
- Confirm all three PNGs exist at 512x160.
- Run Runtime, EditMode, and Editor compile checks.
- Run `git diff --check`.
- If Unity MCP is available, import assets, check Console, and capture the route map surface for shop, dream event, and rest nest nodes.
