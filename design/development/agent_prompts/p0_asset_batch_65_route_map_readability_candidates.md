# Agent Prompt - P0 Asset Batch 65 Route Map Readability Candidates

## Task Scope

Produce a candidate-only non-cat UI packet for route-map readability:

- Current route-node halo.
- Selected branch node ring.
- Available route path connector.
- Locked/future route path connector.
- Boss path pressure connector.

This batch is asset-candidate work only. It must not install anything into
Unity and must not change manifest/runtime binding counts.

## Required Reading

- `design/梦境支配者核心玩法/docs/00_overview/p0_minimum_design.md`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0RouteMapPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/RouteMapController.cs`
- `Assets/TheCat/Scripts/Runtime/Roguelite/P0RouteCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RouteMapSurfaceCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetProductionQueueCatalog.cs`

## Expected Output

- Five transparent candidate PNGs outside `Assets`.
- One manifest CSV.
- One review sheet PNG.
- One candidate review note.
- One process note.
- One scoped validator.
- Queue coverage updated so Batch 65 is visible as candidate-complete pending
  Unity review.

## Do Not Modify

- Do not write to `Assets/TheCat/Art/UI`.
- Do not create Unity `.meta` files in the candidate folder.
- Do not modify route node icons, reward cards, main menu art, prefabs, or
  scenes.
- Do not read, crop, recolor, regenerate, or route starter-cat body art,
  colored turnarounds, fur markings, costumes, or props.

## Acceptance Criteria

- Candidate PNGs are transparent `256x256` or `512x128` files as specified in
  the manifest.
- The manifest records all five subject ids and expected future binding hints.
- The review note states candidate-only status and Unity readability checks.
- The validator proves dimensions, hashes, transparent corners, no `.meta`
  files, packet files, and queue catalog references.
- `P0AssetProductionQueueCoverage`, `P0AssetReviewPacket`, and
  `P0AssetUnityValidationChecklist` tests reflect the new queue baseline.

## Validation

- `powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_route_map_readability_candidates.ps1`
- `MSBuild TheCat.Runtime.csproj`
- `MSBuild TheCat.EditModeTests.csproj`
- `git diff --check`
- Unity MCP/editor follow-up when available:
  - inspect current/selected route markers at route-map scale
  - inspect available/locked/Boss connector readability
  - confirm Console has no import, missing texture, or IMGUI layout errors
  - approve or reject formal install into `Assets/TheCat/Art/UI`
