# P0 Starter Cat Strict Reference Pack

Date: 2026-06-14

## Purpose

This packet tightens systematic starter-cat asset production after the current
consistency issue: Saiban, Nephthys, and Suzune assets must strictly follow the
colored three-view turnarounds from the design source. This is a reference and
prompt-readiness gate, not a new cat-art generation batch and not a Unity import
approval.

## Hard Source Paths

- Saiban:
  `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- Nephthys:
  `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- Suzune:
  `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`

## Gate Changes

- Added `P0StarterCatProductionPromptReadiness`.
- `P0AssetProductionReadiness` now fails if starter-cat production prompts:
  - use mojibake design paths
  - omit the real colored-turnaround source PNG paths
  - omit the candidate-only path policy
  - omit the formal-import blocked policy
  - omit one-cat-at-a-time production guidance
- `P0AssetReviewPacket.BuildMarkdown()` now includes a "Starter Cat Production
  Prompt Readiness" section.
- Rebuilt Batch 17, Batch 18, and Batch 26 prompts with real UTF-8 design paths.
- Added Batch 28 prompt:
  `design/development/agent_prompts/p0_asset_batch_28_starter_cat_strict_reference_pack.md`

## Current Decision

- No new starter-cat PNGs were generated.
- No runtime cat sprites were modified.
- Candidate art remains review-only under
  `design/development/asset_candidates/starter_cats`.
- Formal starter-cat import remains blocked until active-cat Play Mode
  screenshots are captured and reviewed against the colored three-view
  turnarounds.

## Validation

- A mojibake-path scan over Batch 17/18/26/28 prompts found no remaining stale
  encoded design path text.
- Visual Studio 2022 MSBuild compile passed for Runtime:
  `TheCat.Runtime.csproj` with 0 warnings and 0 errors.
- Visual Studio 2022 MSBuild compile passed for EditMode tests:
  `TheCat.EditModeTests.csproj` with 0 warnings and 0 errors.
- Visual Studio 2022 MSBuild compile passed for Editor:
  `TheCat.Editor.csproj` with 0 errors and the existing `MSB3277`
  `System.Numerics.Vectors` Unity/VS reference warning.
- `design/development/tools/validate_starter_cat_candidate_pack.ps1` passed:
  12 rows, 3 review notes, 3 review sheets, formal Unity import remains
  blocked.
- `git diff --check` passed.

## Pending Unity-Side Evidence

- Refresh AssetDatabase.
- Rebuild the asset review packet from Unity menu.
- Capture `05-active-cat-saiban.png`, `06-active-cat-nephthys.png`, and
  `07-active-cat-suzune.png`.
- Compare active screenshots and future candidates against the colored
  three-view turnarounds before any runtime cat art replacement.
