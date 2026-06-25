# Agent Prompt - P0 Asset Batch 77 Owner Sleep Status Icon Candidates

## Task Scope

Review or regenerate the candidate-only owner sleep-state status icon packet
for P0. This batch covers the four HUD/settlement status icons requested by
the P0 UI inventory:

Batch 77 owner sleep-state status icons are a symbolic UI companion to the
Batch 76 owner-state animation packet.

- deep sleep / stable hypnosis
- half awake
- near awake
- wake failure

This is a non-cat, candidate-only UI batch. Do not install anything into Unity.

## Required Reading

- `D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\docs\04_art_production\p0_digital_asset_inventory.md`
- `D:\Unity Workspace\TheCat\design\development\P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `D:\Unity Workspace\TheCat\design\development\asset_review\P0_ASSET_MEMORY_TODO_LEDGER.md`
- `D:\Unity Workspace\TheCat\design\development\asset_candidates\owner_sleep_states\batch_76_owner_sleep_state_framesheet_candidate_2026-06-24\owner_sleep_states_batch76_candidate_review.md`

## Expected Outputs

Use the existing batch directory:

`design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24`

Expected image outputs:

- `thecat_ui_owner_sleep_status_icons_batch77_chromakey_source_v001.png`
- `thecat_ui_owner_sleep_status_icons_batch77_alpha_sheet_v001.png`
- `icons_256/thecat_ui_owner_sleep_status_deep_sleep_256_candidate_v001.png`
- `icons_256/thecat_ui_owner_sleep_status_half_awake_256_candidate_v001.png`
- `icons_256/thecat_ui_owner_sleep_status_near_awake_256_candidate_v001.png`
- `icons_256/thecat_ui_owner_sleep_status_wake_failure_256_candidate_v001.png`
- matching `icons_64` and `icons_32` derivatives for each state
- `thecat_ui_owner_sleep_status_icons_batch77_contact_sheet_v001.png`
- `thecat_ui_owner_sleep_status_icons_batch77_review_sheet_v001.png`

Expected text/control outputs:

- `owner_sleep_status_icons_batch77_manifest.csv`
- `owner_sleep_status_icons_batch77_candidate_review.md`
- `owner_sleep_status_icons_batch77_process_note.md`
- `design/development/tools/build_owner_sleep_status_icon_candidates.py`
- `design/development/tools/validate_owner_sleep_status_icon_candidates.ps1`

## Visual Requirements

- Four symbolic status icons, not character portraits.
- The icons must match the P0 dream-defense UI language: moon/sleep wave,
  cracked dream bubble, amber wake pulse, and returning consciousness orb.
- The progression should escalate from safe blue sleep to amber warning and
  purple wake failure.
- Icons must remain readable at 64px and 32px.
- No cats.
- No cat body, face, paw, tail, fur, starter-cat costume, starter-cat prop,
  weapon silhouette, or colored-turnaround crop.
- No text, letters, or numbers baked into any image.
- Transparent alpha icon outputs.

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
- Alpha sheet exists.
- Exactly 4 state icons exist for each size: 256px, 64px, and 32px.
- Manifest has exactly 12 rows.
- Review sheet and contact sheet exist.
- Review note explicitly states candidate-only, non-cat, and do-not-import.
- Process note records built-in imagegen generation and chroma-key removal.
- Validator passes.
- No `.meta` files exist in the candidate directory.
- `git diff --check` passes.

## Unity MCP / Editor Validation Later

When Unity MCP/editor tools are available, the follow-up install review must:

- refresh AssetDatabase only after an approved install decision
- inspect Sprite import settings
- verify 64px and 32px readability over dark and light HUD panels
- verify cooldown-mask and numeric-overlay compatibility
- capture battle HUD and settlement screenshots for all four owner states
- verify Console has no errors
- verify scene/prefab references before any runtime binding

This prompt does not authorize Unity installation.
