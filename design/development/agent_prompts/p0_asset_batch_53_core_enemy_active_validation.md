# P0 Asset Batch 53 - Core Enemy Active Validation

## Task Scope

Validate the current P0 enemy and Boss candidates in Unity context without
installing them. Focus on Black Mud Nightmare Batch 40, Cold Light Shadow Batch
42, and Call Tyrant Boss Batch 44.

## Required Design Inputs

- `design/梦境支配者核心玩法/docs`
- `design/development/asset_candidates/enemies`
- Existing runtime enemy behavior and warning design records.

## Code And Records To Read

- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetReviewPacket.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetProductionQueueCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Combat/BattleSimulation.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0EnemyWarningIndicatorPresenter.cs`

## Expected Output

- Active enemy screenshot set for Black Mud, Cold Light, and Call Tyrant.
- A review note for each enemy candidate with scale, silhouette, warning
  readability, bed-pressure readability, and source-reference comparison.
- A process note that records Console status and whether candidates remain
  blocked or are ready for a formal install packet.

## Forbidden Changes

- Do not copy candidate PNGs into `Assets`.
- Do not modify enemy prefabs, scenes, runtime bindings, or formal import state.
- Do not approve a candidate if its warning, attack posture, or Boss identity
  is unclear at game scale.

## Acceptance

- Unity Console has no new errors.
- Each candidate has an active screenshot and explicit blocked-or-approved
  review decision.
- Call Tyrant review covers summon portal, app throw, and Boss silhouette.

## Validation

- Unity MCP: capture screenshots, inspect Console, and confirm scene/prefab
  binding.
- Offline fallback: run MSBuild and leave Unity validation pending.
