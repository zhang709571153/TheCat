# Agent Prompt: P0 Asset Batch 16 Authority Blessing Seals

## Task Scope

Create and wire the 128x128 non-cat authority blessing seal batch for the three
P0 authority blessings: Oath Bedline, Moon-Sand Dominion, and Lullaby Rhythm.

## Design / Spec Sources

Read:

- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/prompts/p0_authority_blessing_seals.md`
- `design/development/P0_ASSET_MANIFEST.csv`
- `design/development/P0_ARCHITECTURE_CODE_STATUS_2026-06-14.md`

Relevant requirement: P0 blessings are authority blessings only; visual assets
must distinguish the three authority identities without modifying or deriving
from starter cat body art.

## Code / Data To Read

- `Assets/TheCat/Scripts/Runtime/Roguelite/P0BlessingCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Roguelite/P0RouteRewardResolver.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetGenerationBatchCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0RouteMapPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RouteMapSurfaceCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RuntimeVisualBindingCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetManifestCoverage.cs`
- `design/development/tools/build_runtime_visual_contact_sheet.ps1`

## Expected Output

- Three generated 128x128 PNG seals under `Assets/TheCat/Art/UI/Icons`.
- Matching Unity `.meta` files with the P0 import marker and Batch16 userData.
- Manifest/catalog/runtime binding updates.
- Route reward choices use a specific authority seal when the choice gains or
  upgrades a P0 authority blessing.
- Coverage tests and screenshot smoke plan include the new runtime bindings.
- Review note under `design/development/asset_review`.
- Development log entry documenting this batch and the current architecture
  completeness assessment.

## Do Not Modify

- Any starter cat turnaround, locked starter cat sprite, starter cat candidate,
  or cat source reference image.
- Enemy source concept or animation references.
- Reward economy logic, route topology, blessing math, or combat tuning.
- Existing route-card frame, generic route-choice icon, or reward-detail badge
  assets.

## Acceptance Checks

Run or verify:

- Generate assets with `design/development/tools/build_authority_blessing_seals.ps1`.
- Regenerate runtime contact sheet with `design/development/tools/build_runtime_visual_contact_sheet.ps1`.
- Confirm all three seal PNG files are exactly `128x128`.
- Confirm `.meta` files contain `TheCatP0ImportSettings:v1` and
  `batch:p0_asset_batch_16_authority_blessing_seals`.
- Confirm `P0AssetManifestCatalog.P0ManifestAssetCount == 75`.
- Confirm `P0VisualAssetCatalog.P0RuntimeVisualBindingCount == 71`.
- Confirm `P0RouteMapPresenter` resolves specific blessing seals on
  `layer_07_blessing`.
- Compile runtime/editor/test assemblies or run Unity EditMode tests when
  available.
- Keep Unity MCP / Play Mode screenshot validation as follow-up if MCP is
  unavailable.
