# P0 Asset Batch 46 - P0 Asset Production Dashboard

## Mission

Create and maintain the P0 asset production dashboard that decides what can move
from Codex-side candidate production into Unity-side installation review.

This is an orchestration and evidence batch. Do not import into Unity. Do not
overwrite runtime art. Do not approve replacement art. The purpose is to make
source lock, candidate preview, current runtime asset, install target, and
active screenshot gates visible in one place.

## Scope

Cover exactly these P0 subjects:

- Saiban
- Nephthys
- Suzune
- Black Mud Nightmare
- Cold Light Shadow
- Call Tyrant

Codex-side asset production is allowed in later batches: image generation,
cleanup, cutout preparation, contact sheets, manifest rows, process notes, and
review packets can be created outside Unity. Unity remains responsible for
import settings, prefab/scene connection, Play Mode screenshots, Console checks,
and runtime feel validation.

## Read First

- `design/梦境支配者核心玩法/docs`
- `design/梦境支配者核心玩法/assets`
- `design/development/asset_candidates/starter_cats/batch_45_starter_cat_source_lock_audit_pack_2026-06-15/starter_cat_batch45_source_lock_audit_manifest.csv`
- `design/development/asset_candidates/enemies/batch_40_black_mud_cutout_candidate_2026-06-15/black_mud_batch40_cutout_manifest.csv`
- `design/development/asset_candidates/enemies/batch_42_cold_light_cutout_candidate_2026-06-15/cold_light_batch42_cutout_manifest.csv`
- `design/development/asset_candidates/enemies/batch_44_call_tyrant_cutout_candidate_2026-06-15/call_tyrant_batch44_cutout_manifest.csv`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0PlayModeScreenshotSmoke.cs`

## Expected Outputs

- `design/development/tools/build_p0_asset_production_dashboard.py`
- `design/development/tools/validate_p0_asset_production_dashboard.ps1`
- `design/development/asset_candidates/p0_asset_dashboard/batch_46_p0_asset_production_dashboard_2026-06-15/p0_asset_batch46_production_dashboard_manifest.csv`
- `design/development/asset_candidates/p0_asset_dashboard/batch_46_p0_asset_production_dashboard_2026-06-15/thecat_p0_asset_batch46_production_dashboard_review_sheet.png`
- `design/development/asset_candidates/p0_asset_dashboard/batch_46_p0_asset_production_dashboard_2026-06-15/p0_asset_batch46_production_dashboard_review.md`
- `design/development/asset_candidates/p0_asset_dashboard/batch_46_p0_asset_production_dashboard_2026-06-15/p0_asset_batch46_production_dashboard_process_note.md`

## Required Manifest Fields

Each row must include:

- subject id
- display name
- category
- combat role
- source lock id or ids
- source reference paths and hashes
- current Unity runtime asset path and hash
- latest candidate preview path and hash
- latest candidate manifest
- active screenshot
- Unity install target path
- Unity validation gate
- next action
- blockers
- recommendation

The recommendation must remain
`dashboard_only_unity_validation_pending`.

## Forbidden Changes

- Do not import into Unity.
- Do not copy candidate files into `Assets`.
- Do not generate Unity `.meta` files.
- Do not modify `P0AssetManifestCatalog.P0ManifestAssetCount`.
- Do not modify `P0VisualAssetCatalog.P0RuntimeVisualBindingCount`.
- Do not replace starter-cat runtime sprites until colored three-view source
  locks and active screenshot review pass.
- Do not use old mojibake source paths in new manifest rows.

## Acceptance Standards

- The dashboard has exactly 6 rows, one for each scoped P0 subject.
- Saiban, Nephthys, and Suzune remain locked to their colored three-view
  turnaround identity.
- Black Mud Nightmare, Cold Light Shadow, and Call Tyrant remain locked to their
  concept plus animation source references.
- Every row has an active screenshot gate.
- Call Tyrant explicitly records that the current runtime asset is a concept
  proxy and that a formal boss combat sprite binding decision is still needed.
- The review sheet is readable without opening Unity.
- All candidate outputs stay outside `Assets`.
- Unity validation pending is explicit in the review and process notes.

## Validation

Run:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_p0_asset_production_dashboard.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_starter_cat_source_lock_audit_pack.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_black_mud_cutout_candidate.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_cold_light_cutout_candidate.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_call_tyrant_cutout_candidate.ps1
```

When Unity MCP/editor access is available, capture and compare:

- `05-active-cat-saiban.png`
- `06-active-cat-nephthys.png`
- `07-active-cat-suzune.png`
- `07-active-enemy-black-mud.png`
- `08-active-enemy-cold-light.png`
- `09-active-enemy-call-tyrant.png`

Also verify Unity Console, AssetDatabase refresh, Sprite import settings, and
prefab/scene references before any dashboard row is promoted into an install
batch.
