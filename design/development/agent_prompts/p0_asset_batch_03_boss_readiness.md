# P0 Asset Batch 03 - Boss Readiness Agent Prompt

## Task Scope

Generate the P0 Call Tyrant boss visual support set:

- `thecat_enemy_calltyrant_concept_2048_v001`
- `thecat_vfx_calltyrant_warning_512_v001`
- `thecat_ui_route_bossnode_icon_128_v001`

This batch should start only after the Boss behavior code path is stable enough
to confirm the visual needs for summon warnings, throw warnings, and route map
presentation. The Call Tyrant source concept and animation are mandatory hard
references; the agent may improve gameplay readability, but may not redesign the
Boss away from those references.

## Required Reading

- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/P0_ASSET_MANIFEST.csv`
- `design/development/prompts/p0_boss_assets.md`
- `design/梦境支配者核心玩法/assets/enemies/en06_call_tyrant/concept/call_tyrant_concept.png`
- `design/梦境支配者核心玩法/assets/enemies/en06_call_tyrant/animation/call_tyrant_animation.png`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetGenerationBatchCatalog.cs`

## Expected Output

- Accepted PNG files under:
  - `Assets/TheCat/Art/Enemies/Concepts/`
  - `Assets/TheCat/Art/Enemies/VFX/`
  - `Assets/TheCat/Art/UI/Icons/`
- Manifest status changes only for accepted files present in the workspace.
- Development log entry with prompt summaries, paths, and boss readability notes.
- Rejected outputs moved outside `Assets` to `design/development/rejected_assets/`
  with a short mismatch reason.

## Do Not Modify

- Boss AI, route map, or battle systems unless a separate code task is opened.
- Batch 01 or Batch 02 assets.
- Final UI layout files before the icon is validated as a raw asset.
- Design-source reference files under `design/梦境支配者核心玩法/assets/`.

## Acceptance Criteria

- Call Tyrant reads as the source phone-call/light-intrusion nightmare, not a
  generic monster or generic phone mascot.
- Boss concept preserves the source device-shell silhouette, red notification
  cue, cyan waveform language, and vibration/throw/summon read.
- Warning VFX communicates summon and throw danger at gameplay scale while using
  source Call Tyrant waveform/notification language.
- Boss route node icon is readable at 128x128 and consistent with the route UI.
- No baked text, watermark, gore, or photoreal phone branding.
- No asset is marked `generated` unless the file exists at its manifest path and
  passes the source-reference consistency check.
- Import readiness passes after any manifest status update.

## Validation

Run offline compile, P0 code smoke, and import readiness. When Unity MCP/editor
is available, verify Console state, texture imports, and a Boss node/preview
screenshot.
