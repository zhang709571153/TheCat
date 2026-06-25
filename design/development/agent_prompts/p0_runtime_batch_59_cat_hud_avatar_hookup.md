# P0 Runtime Batch 59 - Cat HUD Avatar Hookup

## Task Scope

Wire the Batch 58 source-locked starter-cat HUD avatars into the runtime P0 cat
HUD path.

## Related Design And Asset Sources

- `design/梦境支配者核心玩法/docs`
- `design/development/asset_candidates/starter_cats/batch_58_starter_cat_hud_avatar_install_2026-06-15`
- `design/development/asset_review/P0_RUNTIME_VISUAL_REVIEW_PACKET.md`
- `design/development/asset_review/p0_runtime_visual_contact_sheet_2026-06-14.md`

## Code To Read

- `Assets/TheCat/Scripts/Runtime/Gameplay/P0CatHudPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/GrayboxBattleController.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0CatHudCoverage.cs`
- `Assets/TheCat/Tests/EditMode/P0CatHudCoverageTests.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`

## Expected Output

- `P0CatHudCard` exposes a HUD avatar reference for each starter cat.
- Runtime HUD rendering uses the source-locked HUD avatar as the primary cat
  HUD icon and falls back to the combat sprite only if the avatar is missing.
- Cat HUD coverage verifies all three starter-cat avatars by asset id, asset
  type, and colored-turnaround source lock.
- EditMode tests assert the active cat card resolves the avatar and uses it as
  the primary HUD icon.

## Forbidden Scope

- Do not replace combat sprites.
- Do not import AI starter-cat body candidates.
- Do not change P0 manifest or runtime visual binding counts unless a new
  asset is intentionally installed.
- Do not alter enemy, route, settlement, or skill behavior.

## Acceptance Criteria

- Batch 58 avatars remain source-locked to the colored three-view turnarounds.
- `GrayboxBattleController.DrawCatControls` draws `PrimaryHudIcon` instead of
  directly drawing the combat sprite.
- Runtime and EditMode MSBuild both pass with 0 errors.
- Unity MCP follow-up remains required for Console, Sprite import, HUD
  screenshot, scene/prefab binding, and readability validation.
