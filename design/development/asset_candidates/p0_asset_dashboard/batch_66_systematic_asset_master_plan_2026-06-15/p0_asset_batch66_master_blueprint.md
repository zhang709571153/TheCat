# P0 Asset Batch 66 - Systematic Asset Master Blueprint

Decision: spec/control batch complete. Do not import into Unity.

This batch turns the current asset work into an explicit production map before
opening more image-generation work. It answers the current production question:
Codex can continue producing scoped candidate/spec batches outside Unity, but
Unity remains the install and runtime-acceptance boundary.

## Inputs

- Art pipeline:
  `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- Current queue:
  `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetProductionQueueCatalog.cs`
- Unity handoff checklist:
  `design/development/asset_review/P0_ASSET_UNITY_VALIDATION_CHECKLIST.md`
- Starter-cat strict source locks:
  `design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.md`
- Starter-cat conformance spec:
  `design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`
- Batch 46 dashboard:
  `design/development/asset_candidates/p0_asset_dashboard/batch_46_p0_asset_production_dashboard_2026-06-15/p0_asset_batch46_production_dashboard_review.md`

## Output

- Gap matrix:
  `design/development/asset_candidates/p0_asset_dashboard/batch_66_systematic_asset_master_plan_2026-06-15/p0_asset_batch66_master_gap_matrix.csv`
- Process note:
  `design/development/asset_candidates/p0_asset_dashboard/batch_66_systematic_asset_master_plan_2026-06-15/p0_asset_batch66_process_note.md`
- Next execution prompt:
  `design/development/agent_prompts/p0_asset_batch_67_bedroom_interaction_affordance_candidates.md`

## Current Production Lanes

### Blocked Until Unity Evidence

- Starter-cat body replacements.
- Core enemy/Boss runtime sprite replacement.
- Formal install decisions.
- Installed Batch 60 and Batch 61 acceptance completion.

These lanes need Unity screenshots, Console status, Sprite import settings, and
scene/prefab binding checks before any completion claim.

### Candidate-Complete Pending Unity Review

- Batch 54 bedroom interactable props.
- Batch 62 runtime control icons.
- Batch 63 runtime control panels.
- Batch 64 secondary enemy warning VFX.
- Batch 65 route-map readability UI.

These packs stay outside `Assets` and must not receive Unity `.meta` files
until a formal install decision exists.

### Recommended Next Codex Candidate Lane

Batch 67 should produce bedroom interaction affordance candidates:

- bed sleep-protect ready ring
- bed repair / restore pulse
- litter-box urgent-use marker
- feeder ready / low-food marker
- interaction blocked marker
- valid interaction range ripple

Reasoning:

- It directly improves P0 hand feel for bed, litter box, feeder, hunger, poop,
  and sleep interactions.
- It is non-cat UI/VFX, so it cannot drift starter-cat identity.
- It can be produced as transparent PNG candidates outside `Assets`.
- It has a clear Unity acceptance path: runtime interaction screenshot,
  Console check, input timing readability, and scene/prefab binding review.

## Cat Asset Rule

No future Saiban, Nephthys, Suzune, Shadowmaru, Mianhua, or Yuheng body asset
may be accepted because it is merely attractive, readable, or stylistically
close. Cat body acceptance requires an exact relationship to the relevant
colored three-view turnaround:

- same body proportion
- same coat color and markings
- same costume anchors
- same symbolic props
- same non-human cat identity
- explicit active screenshot comparison in Unity

Until those checks exist, cat-body work remains review-only and outside
`Assets`.

## Unity Boundary

This batch does not prove Unity acceptance. It intentionally creates no PNG
candidate, no Unity `.meta`, no manifest row, no runtime visual binding, and no
Prefab or Scene change.

Unity acceptance remains pending for:

- AssetDatabase refresh.
- Console status.
- Sprite import settings.
- Play Mode screenshots.
- Runtime scale/readability.
- Scene and prefab references.
