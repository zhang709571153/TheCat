# Agent Prompt - P0 Asset Batch 76 Owner Sleep State Framesheet Candidate

## Task Scope

Review or regenerate the candidate-only owner sleep state sprite-sheet packet
for P0. This batch covers the owner-in-bed sleep state animation requested by
the P0 digital asset inventory:

- deep sleep idle
- half dream / half awake
- nearly awake warning
- wake / failure transition

This is a non-cat, candidate-only batch. Do not install anything into Unity.
The active source revision is v002: owner/pillow/blanket overlay framing with
padded normalized frame outputs. The v001 full-bed source is retained as
historical evidence only.

## Required Reading

- `D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\docs\00_overview\p0_minimum_design.md`
- `D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\docs\04_art_production\p0_digital_asset_inventory.md`
- `D:\Unity Workspace\TheCat\design\development\P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `D:\Unity Workspace\TheCat\design\development\asset_candidates\p0_asset_dashboard\batch_66_systematic_asset_master_plan_2026-06-15\p0_asset_batch66_master_blueprint.md`

## Expected Outputs

Use the existing batch directory:

`design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24`

Expected image outputs:

- `thecat_owner_sleep_states_batch76_chromakey_source_1536x1024_v002.png`
- `thecat_owner_sleep_states_batch76_alpha_sheet_1536x1024_candidate_v002.png`
- `frames/thecat_owner_sleep_state_deep_sleep_f01_256_candidate_v001.png` through `f06`
- `frames/thecat_owner_sleep_state_half_awake_f01_256_candidate_v001.png` through `f06`
- `frames/thecat_owner_sleep_state_near_awake_f01_256_candidate_v001.png` through `f06`
- `frames/thecat_owner_sleep_state_wake_failure_f01_256_candidate_v001.png` through `f06`
- `thecat_owner_sleep_states_batch76_contact_sheet_1920x1320_v001.png`
- `thecat_owner_sleep_states_batch76_review_sheet_1920x1320_v001.png`

Expected text/control outputs:

- `owner_sleep_states_batch76_manifest.csv`
- `owner_sleep_states_batch76_candidate_review.md`
- `owner_sleep_states_batch76_process_note.md`
- `design/development/tools/build_owner_sleep_state_framesheet_candidate.py`
- `design/development/tools/validate_owner_sleep_state_framesheet_candidate.ps1`

## Visual Requirements

- 4-row by 6-column source sprite sheet.
- Six frames per owner state, with small sequential animation changes.
- Owner remains generic and mostly under the blanket.
- Use owner/pillow/blanket overlay framing rather than a full bed-frame prop,
  so runtime tests can layer it over the existing bedroom/bed scene safely.
- Output frames must be padded normalized 256x256 alpha PNGs with visible
  content away from slice edges.
- Blanket and effects should match the P0 bedroom-dream palette: navy star
  blanket, warm lamp, blue-lavender sleep effects, amber warning effects.
- Near-awake and wake-failure rows must include alarm/light vibration, dream
  cracks, and a consciousness-orb return cue.
- No cats.
- No cat body, face, paw, tail, fur, starter-cat costume, starter-cat prop,
  weapon silhouette, or colored-turnaround crop.
- No text baked into any image.
- Transparent alpha frame outputs.

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

## Acceptance Criteria

- Candidate directory exists outside `Assets`.
- Alpha sheet exists at 1536x1024.
- Exactly 24 split frame PNGs exist, each 256x256.
- Manifest has exactly 24 rows.
- Review sheet and contact sheet exist at 1920x1320.
- Review note explicitly states candidate-only, non-cat, and do-not-import.
- Process note records built-in imagegen generation and chroma-key removal.
- Validator passes.
- No `.meta` files exist in the candidate directory.
- `git diff --check` passes.

## Unity MCP / Editor Validation Later

When Unity MCP/editor tools are available, the follow-up install review must:

- refresh AssetDatabase only after an approved install decision
- inspect Sprite import settings
- verify frame slicing and pivot
- capture battle-world sleep-state screenshots
- verify Console has no errors
- verify scene/prefab references before any runtime binding

This prompt does not authorize Unity installation.
