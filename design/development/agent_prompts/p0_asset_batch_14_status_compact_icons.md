# Agent Prompt: P0 Asset Batch 14 Status Compact Icons

## Task Scope

Create and wire the 32x32 compact HUD icon batch for the five P0 status tags.
This batch is a deterministic derivative pass from accepted 64px status icons;
it is not a character-art or cat-production batch.

## Design / Spec Sources

Read:

- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/prompts/p0_status_compact_icons.md`
- `design/development/P0_ASSET_MANIFEST.csv`

Relevant requirement: P0 UI rules require status icons to have 32x32 and 64x64
exports.

## Code / Data To Read

- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetGenerationBatchCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0StatusHudPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/GrayboxBattleController.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StatusHudCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RuntimeVisualBindingCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetManifestCoverage.cs`
- `design/development/tools/build_runtime_visual_contact_sheet.ps1`

## Expected Output

- Five generated 32x32 PNG status icons under `Assets/TheCat/Art/UI/Icons`.
- Matching Unity `.meta` files with the P0 import marker and Batch14 userData.
- Manifest/catalog/runtime binding updates.
- Status HUD uses compact icons for inline IMGUI display while retaining 64px full icon references.
- Tests and coverage gates updated for the new binding and manifest counts.
- Review note under `design/development/asset_review`.
- Development log entry documenting this batch.

## Do Not Modify

- Any starter cat turnaround, locked starter cat sprite, or starter cat candidate.
- Enemy source concept or animation references.
- Battle status logic or numeric tuning.
- Existing 64px status icons.

## Acceptance Checks

Run or verify:

- Generate assets with `design/development/tools/build_status_compact_icons.ps1`.
- Regenerate runtime contact sheet with `design/development/tools/build_runtime_visual_contact_sheet.ps1`.
- Confirm all five compact PNG files are exactly `32x32`.
- Confirm `.meta` files contain `TheCatP0ImportSettings:v1` and `batch:p0_asset_batch_14_status_compact_icons`.
- Compile runtime/editor/test assemblies or run Unity EditMode tests when available.
- Keep Unity MCP / editor screenshot validation as follow-up if MCP is unavailable.
