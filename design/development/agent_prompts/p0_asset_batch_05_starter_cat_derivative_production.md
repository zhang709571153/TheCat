# P0 Asset Batch 05 Starter Cat Derivative Production Agent Prompt

## Task Scope

Produce or refine starter-cat derivative asset candidates only when they stay
strictly aligned with the locked colored turnarounds. This batch is for
controlled candidate production, not direct Unity import replacement.

The initial allowed derivatives are:

- `combat_sprite_refinement_512`
- `hud_avatar_256`
- `skill_icon_motif_128`
- `front_animation_keyframe_512`

All candidates must first be written under:

`design/development/asset_candidates/starter_cats/<cat_id>/`

Do not replace files in `Assets/TheCat/Art/Characters/Sprites` unless the main
session explicitly accepts a candidate and updates source locks, manifest rows,
review packet evidence, and Unity validation records.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/03_characters/character_design.md`
- `design/梦境支配者核心玩法/docs/03_characters/character_voice_lines_appendix.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- `design/梦境支配者核心玩法/markdown/04 - 游戏角色设定.md`
- `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`
- `design/development/asset_review/p0_starter_cat_turnaround_review_2026-06-14.png`
- `design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.md`
- `design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.csv`
- `design/development/asset_review/P0_RUNTIME_VISUAL_REVIEW_PACKET.md`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`

## Code Files To Read

- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatTurnaroundSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatSourceLockPacketEvidence.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetProductionReadiness.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetReviewPacket.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0VisualAcceptanceReport.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Tests/EditMode/P0StarterCatTurnaroundSourceLocksTests.cs`
- `Assets/TheCat/Tests/EditMode/P0AssetProductionReadinessTests.cs`

## Required Character Locks

Saiban must keep:

- silver-blue armored non-human cat proportions
- front-view tabby face markings
- oath shield silhouette
- sword silhouette
- cape and helm read from the colored turnaround

Nephthys must keep:

- hooded non-human cat body
- moon-sand Egyptian motif read
- floating pyramid / obelisk prop silhouette
- gold and blue palette from the colored turnaround
- dream-script controller identity

Suzune must keep:

- calico markings from the colored turnaround
- shrine outfit on non-human cat body
- bell ornaments
- wand / branch healer silhouette
- vermilion, warm white, and moon-blue healer palette

## Expected Output

- Candidate PNGs under the candidate directory only.
- A Markdown candidate review note beside each candidate containing:
  - candidate file path
  - exact source turnaround path
  - current Unity sprite path
  - active-cat screenshot name from the Play Mode capture plan
  - trait-by-trait comparison notes
  - accepted / rejected recommendation
  - if rejected, the exact drift reason
- Updated `design/development/DEVELOPMENT_LOG.md` if a candidate is produced.
- No Unity import replacement unless separately approved.

## Current Baseline Candidate Pack

A deterministic source-locked baseline pack already exists:

- Builder:
  `design/development/tools/build_starter_cat_derivative_candidates.py`
- Batch summary:
  `design/development/asset_candidates/starter_cats/batch_05_source_locked_derivatives_2026-06-14/README.md`
- Batch manifest:
  `design/development/asset_candidates/starter_cats/batch_05_source_locked_derivatives_2026-06-14/starter_cat_batch05_candidate_manifest.csv`
- Per-cat review sheets and notes:
  - `design/development/asset_candidates/starter_cats/saiban/batch_05_source_locked_derivatives_2026-06-14/`
  - `design/development/asset_candidates/starter_cats/nephthys/batch_05_source_locked_derivatives_2026-06-14/`
  - `design/development/asset_candidates/starter_cats/suzune/batch_05_source_locked_derivatives_2026-06-14/`

If this agent creates a higher-polish candidate, compare it against the current
baseline pack and reject it unless it preserves every required colored-turnaround
trait. Do not use free-form image generation for starter cats unless the prompt
uses the locked colored turnaround as the only character source of truth and the
output remains in the candidate directory for review.

## Prohibited Changes

- Do not modify the three locked colored turnaround source files.
- Do not overwrite:
  - `Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png`
  - `Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png`
  - `Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png`
- Do not use `Assets/TheCat/Art/_GeneratedReferences/thecat_style_startercats_lineup_2048_v001.png` as the primary cat source.
- Do not generate humanoid poses, human-costume proportions, new cat breeds,
  alternate palettes, or civilization motifs not present in the colored
  turnarounds.
- Do not edit gameplay code, tuning, scene files, packages, or project settings.

## Acceptance Criteria

- `P0StarterCatAssetProductionSpec.EvaluateP0Spec()` passes.
- `P0StarterCatSourceLockPacketEvidence.EvaluateCurrentPacket()` passes.
- `P0StarterCatVisualConsistencyChecklist.EvaluateP0Checklist()` passes.
- `P0StarterCatTurnaroundSourceLocks.EvaluateP0Locks()` passes before and after
  candidate generation.
- Candidate review notes include all required evidence items:
  - locked colored turnaround source image
  - starter cat source-lock packet row and SHA-256 values
  - current Unity combat sprite
  - registered active-cat Play Mode screenshot
  - candidate PNG path under candidate review directory
  - side-by-side comparison against starter cat contact sheet
  - colored-turnaround trait coverage notes
  - acceptance or rejection decision with reason
- Any candidate with colored-turnaround drift, human proportions, palette drift,
  or missing required prop/costume/marking traits is rejected.
- `P0VisualAcceptance.EvaluateCurrent()` must continue to report architecture
  ready for systematic asset production, while starter-cat formal import remains
  blocked unless the main session has approved all active-cat screenshots.

## Validation

Local commands:

```powershell
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:OutDir=Temp\Bin\RuntimeStarterCatAssets\ /v:minimal
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:OutDir=Temp\Bin\EditModeStarterCatAssets\ /v:minimal
git diff --check
```

Unity validation after candidate acceptance:

1. Refresh AssetDatabase.
2. Run `TheCat/P0/Run Acceptance Gates (Log Only)`.
3. Run Play Mode screenshot smoke.
4. Compare:
   - `05-active-cat-saiban.png`
   - `06-active-cat-nephthys.png`
   - `07-active-cat-suzune.png`
   against `design/development/asset_review/p0_starter_cat_turnaround_review_2026-06-14.png`.
5. Confirm Console has no errors.
