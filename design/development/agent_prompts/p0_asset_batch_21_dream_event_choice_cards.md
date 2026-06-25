# Agent Prompt: P0 Asset Batch 21 Dream Event Choice Cards

## Task Scope

Create and wire the P0 DreamEvent choice card asset batch. The batch produces three deterministic, non-cat, 384x160 transparent route choice cards for existing DreamEvent reward choice ids and connects them to the route-map surface.

## Read First

- `design/development/prompts/p0_dream_event_choice_cards.md`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `Assets/TheCat/Scripts/Runtime/Roguelite/P0RouteRewardResolver.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetGenerationBatchCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0RouteMapPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RouteMapSurfaceCoverage.cs`

## Expected Outputs

- Generate:
  - `Assets/TheCat/Art/UI/Cards/thecat_ui_dreamevent_clear_notifications_card_384x160_v001.png`
  - `Assets/TheCat/Art/UI/Cards/thecat_ui_dreamevent_catnip_residue_card_384x160_v001.png`
  - `Assets/TheCat/Art/UI/Cards/thecat_ui_dreamevent_mark_all_read_card_384x160_v001.png`
- Generate matching `.meta` files with `batch:p0_asset_batch_21_dream_event_choice_cards`.
- Add manifest entries, visual catalog constants, lookup helpers, runtime bindings, and batch definition.
- Make route-map DreamEvent reward choices resolve their specific choice card assets.
- Update coverage tools and EditMode tests.
- Update the runtime visual contact sheet and asset review packet.
- Add/update local development records.

## Forbidden Areas

- Do not modify starter cat sprites, colored turnaround references, cat character descriptions, or cat asset source-lock language.
- Do not generate cat-shaped motifs in this batch.
- Do not touch unrelated battle tuning, route topology, or gameplay reward math.

## Acceptance Standards

- `P0AssetManifestCatalog.P0ManifestAssetCount == 85`.
- `P0VisualAssetCatalog.P0RuntimeVisualBindingCount == 81`.
- `P0AssetGenerationBatchCatalog.DreamEventChoiceCardBatchId` exists and is included after Batch 20.
- `P0VisualAssetCatalog.GetDreamEventChoiceCard(choice)` resolves all three DreamEvent ids and returns empty for unrelated choices.
- `P0VisualAssetCatalog.GetRouteChoiceCard(choice)` resolves both shop item cards and DreamEvent choice cards.
- `P0RouteMapSurfaceCoverage` verifies DreamEvent choice card surface coverage.
- Contact sheet shows all 81 runtime bindings.

## Test / Validation

- Run `design/development/tools/build_dream_event_choice_cards.ps1`.
- Run `design/development/tools/build_runtime_visual_contact_sheet.ps1`.
- Verify all three PNGs are 384x160 and `.meta` files carry Sprite import settings plus `nonCatSymbolicOnly:true`.
- Compile:
  - `TheCat.Runtime.csproj`
  - `TheCat.EditModeTests.csproj`
  - `TheCat.Editor.csproj`
- Run `git diff --check`.
- If Unity MCP is available, run AssetDatabase refresh, Console check, and route-map screenshot smoke. If unavailable, record that editor-side validation remains pending.
