# Agent Prompt - P0 Asset Batch 67 Bedroom Interaction Affordance Candidates

## Task Scope

Produce a candidate-only non-cat UI/VFX asset pack for P0 bedroom interaction
affordances. The assets should make bed, litter box, feeder, blocked
interaction, and valid interaction range states easier to read in the graybox
battle scene.

This is a Codex-side candidate batch. Do not install anything into Unity.

## Required Reading

Design docs:

- `D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\docs\00_overview\p0_minimum_design.md`
- `D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\docs\04_art_production\p0_digital_asset_inventory.md`

Local production docs:

- `D:\Unity Workspace\TheCat\design\development\P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `D:\Unity Workspace\TheCat\design\development\asset_review\P0_ASSET_UNITY_VALIDATION_CHECKLIST.md`
- `D:\Unity Workspace\TheCat\design\development\asset_candidates\p0_asset_dashboard\batch_66_systematic_asset_master_plan_2026-06-15\p0_asset_batch66_master_blueprint.md`
- `D:\Unity Workspace\TheCat\design\development\asset_candidates\p0_asset_dashboard\batch_66_systematic_asset_master_plan_2026-06-15\p0_asset_batch66_master_gap_matrix.csv`

Relevant code:

- `D:\Unity Workspace\TheCat\Assets\TheCat\Scripts\Runtime\Data\Catalogs\P0AssetProductionQueueCatalog.cs`
- `D:\Unity Workspace\TheCat\Assets\TheCat\Scripts\Runtime\Gameplay\GrayboxBattleController.cs`
- `D:\Unity Workspace\TheCat\Assets\TheCat\Scripts\Runtime\Gameplay\P0InteractablePresenter.cs`
- `D:\Unity Workspace\TheCat\Assets\TheCat\Scripts\Runtime\Data\Catalogs\P0VisualAssetCatalog.cs`

## Expected Outputs

Create a new candidate directory:

`design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15`

Generate transparent PNG candidates:

- `thecat_ui_interaction_bed_ready_ring_256_candidate_v001.png`
- `thecat_ui_interaction_bed_restore_pulse_256_candidate_v001.png`
- `thecat_ui_interaction_litter_urgent_marker_256_candidate_v001.png`
- `thecat_ui_interaction_feeder_ready_marker_256_candidate_v001.png`
- `thecat_ui_interaction_blocked_marker_256_candidate_v001.png`
- `thecat_ui_interaction_range_ripple_512_candidate_v001.png`

Also generate:

- `bedroom_interaction_affordance_batch67_manifest.csv`
- `thecat_ui_bedroom_interaction_affordance_batch67_review_sheet.png`
- `bedroom_interaction_affordance_batch67_candidate_review.md`
- `bedroom_interaction_affordance_batch67_process_note.md`
- a validator script under `design/development/tools`

## Visual Requirements

- Non-cat UI/VFX only.
- Transparent PNGs.
- Hand-painted dream-defense look.
- No text baked into images.
- Clear on warm bedroom background and dark dream overlay.
- Use the existing P0 UI/VFX language:
  - moon-blue sleep pulse for bed readiness
  - soft repair glow for bed restore
  - clean sand / amber urgency for litter box
  - gentle kibble / feeder glow for food readiness
  - muted red-violet cross or lock for blocked interaction
  - cyan-gold soft ripple for valid interaction range

## Forbidden Modification Scope

Do not modify:

- `Assets/`
- `Packages/`
- `ProjectSettings/`
- starter-cat source turnaround images
- starter-cat candidate images
- existing runtime sprite bindings
- Unity `.meta` files
- formal install decision rows

Do not generate or crop:

- Saiban body art
- Nephthys body art
- Suzune body art
- Shadowmaru, Mianhua, or Yuheng body art
- cat faces, fur markings, costumes, paws, tails, or weapon silhouettes

## Acceptance Criteria

- Candidate directory exists outside `Assets`.
- Six transparent PNG candidates exist with the expected dimensions.
- Manifest has exactly six candidate rows.
- Review sheet shows every candidate on warm and dark preview fields.
- Review note explicitly states the batch is candidate-only and non-cat.
- Process note records no Unity import and no `.meta` files.
- Validator passes.
- No `.meta` files exist in the candidate directory.
- `git diff --check` passes.

## Unity MCP / Editor Validation Later

When Unity MCP/editor tools are available, the follow-up install review must:

- refresh AssetDatabase only after an approved install decision
- inspect Sprite import settings
- capture battle interaction screenshots for bed, litter box, feeder, blocked,
  and range states
- verify Console has no errors
- verify scene/prefab references before any runtime binding

This prompt does not authorize Unity installation.
