# Agent Prompt: P0 Asset Batch 22 RestNest Recovery Card

## Task Scope

Create and wire the P0 RestNest recovery card asset batch. The batch produces one deterministic, non-cat, 384x160 transparent route choice card for the existing `rest_nest_recovery` reward choice and connects it to the route-map surface through the generic route choice card slot.

## Read First

- `design/development/prompts/p0_rest_nest_recovery_card.md`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `Assets/TheCat/Scripts/Runtime/Roguelite/P0RouteRewardResolver.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetGenerationBatchCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0RouteMapPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RouteMapSurfaceCoverage.cs`

## Expected Outputs

- Generate:
  - `Assets/TheCat/Art/UI/Cards/thecat_ui_restnest_recovery_card_384x160_v001.png`
- Generate a matching `.meta` file with `batch:p0_asset_batch_22_rest_nest_recovery_card`.
- Add manifest entry, visual catalog constant, lookup helper, runtime binding, and batch definition.
- Make route-map RestNest reward choices resolve the specific recovery card asset.
- Update coverage tools and EditMode tests.
- Update the runtime visual contact sheet and asset review packet.
- Add/update local development records.

## Forbidden Areas

- Do not modify starter cat sprites, colored turnaround references, cat character descriptions, or cat asset source-lock language.
- Do not generate cat-shaped motifs in this batch.
- Do not touch unrelated battle tuning, route topology, or gameplay reward math.

## Acceptance Standards

- `P0AssetManifestCatalog.P0ManifestAssetCount == 86`.
- `P0VisualAssetCatalog.P0RuntimeVisualBindingCount == 82`.
- `P0AssetGenerationBatchCatalog.RestNestRecoveryCardBatchId` exists and is included after Batch 21.
- `P0VisualAssetCatalog.GetRestNestChoiceCard(choice)` resolves `rest_nest_recovery` and returns empty for unrelated choices.
- `P0VisualAssetCatalog.GetRouteChoiceCard(choice)` resolves shop item cards, DreamEvent choice cards, and the RestNest recovery card.
- `P0RouteMapSurfaceCoverage` verifies RestNest recovery card surface coverage.
- Contact sheet shows all 82 runtime bindings.

## Test / Validation

- Run `design/development/tools/build_rest_nest_recovery_card.ps1`.
- Run `design/development/tools/build_runtime_visual_contact_sheet.ps1`.
- Verify the PNG is 384x160 and its `.meta` file carries Sprite import settings plus `nonCatSymbolicOnly:true`.
- Compile:
  - `TheCat.Runtime.csproj`
  - `TheCat.EditModeTests.csproj`
  - `TheCat.Editor.csproj`
- Run `git diff --check`.
- If Unity MCP is available, run AssetDatabase refresh, Console check, and route-map screenshot smoke. If unavailable, record that editor-side validation remains pending.
