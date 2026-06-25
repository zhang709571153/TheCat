# P0 Asset Batch 56 - Formal Install Decision Packet

## Task Scope

Create a formal Unity install decision packet after candidate batches have
review evidence. This prompt is not permission to install assets by itself.
It is the planning gate that decides which approved candidate files may move
from `design/development/asset_candidates` into Unity import roots.

## Required Design Inputs

- `design/梦境支配者核心玩法/docs`
- Active screenshot review notes for starter cats and core enemies.
- Runtime screenshot or scene-scale review notes for bedroom interactables and
  starter skill VFX.
- Candidate manifests, review sheets, process notes, validators, and
  source-lock records for:
  - Batch 49 Saiban low-drift refinement
  - Batch 50 Nephthys strict AI generation candidate
  - Batch 51 Suzune strict AI generation candidate
  - Batch 40 Black Mud cutout candidate
  - Batch 42 Cold Light cutout candidate
  - Batch 44 Call Tyrant cutout candidate
  - Batch 54 bedroom interactable candidates
  - Batch 55 starter skill VFX candidates

## Code And Records To Read

- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetProductionQueueCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetReviewPacket.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetProductionReadiness.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetProductionQueueCoverage.cs`
- `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/bedroom_interactables_batch54_manifest.csv`
- `design/development/asset_candidates/vfx/starter_skills/batch_55_starter_skill_vfx_candidates_2026-06-15/starter_skill_vfx_batch55_manifest.csv`

## Expected Output

- A scoped install decision packet under
  `design/development/asset_candidates/formal_install_decisions`.
- For each candidate group, list:
  - approved, rejected, or blocked decision
  - exact candidate source files
  - Unity import destinations
  - `.meta` expectations
  - manifest updates
  - runtime visual binding updates
  - scene/prefab hookup notes
  - rollback notes
- A review note explaining why each candidate is approved or blocked.
- If installation proceeds in a later execution, update manifests, runtime
  bindings, review packet, readiness tests, and development logs in the same
  batch.

## Forbidden Changes

- Do not install anything unless screenshot or runtime-scale review explicitly
  approves the candidate.
- Do not mix starter-cat, enemy, prop, and VFX installs in one unreviewed
  change.
- Do not modify unrelated code, scenes, prefabs, or settings.
- Do not treat a polished Codex candidate as approved without Unity evidence.

## Acceptance

- Every install row has an explicit human-visible approval source.
- Unity Console, screenshot, sprite import, runtime scale, HUD readability, and
  scene/prefab binding evidence are attached.
- Batch 54 and Batch 55 remain blocked if their runtime scale/readability
  evidence is missing.
- `P0AssetProductionReadiness` and `P0AssetReviewPacket` pass after any
  install decision packet or later install.

## Validation

- Unity MCP, when available: refresh AssetDatabase, inspect Console, capture
  screenshots, and verify scene/prefab binding.
- Offline: run relevant batch validators, MSBuild, and `git diff --check`.
