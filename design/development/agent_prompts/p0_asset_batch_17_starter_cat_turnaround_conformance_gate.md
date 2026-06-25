# P0 Asset Batch 17 Starter Cat Turnaround Conformance Gate Agent Prompt

## Task Scope

Tighten the starter-cat asset pipeline before any new Saiban, Nephthys, or Suzune image generation. This is a conformance-gate batch, not a replacement sprite batch and not a Unity import batch.

The purpose is to prove that every future cat candidate is judged against the colored three-view turnarounds, including front, side, and back anchors. Do not produce new cat art unless the main session explicitly assigns a separate candidate-generation task after this gate is green.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/03_characters/character_design.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`

## Project Files To Read

- `design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.md`
- `design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`
- `design/development/asset_review/P0_RUNTIME_VISUAL_REVIEW_PACKET.md`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatTurnaroundSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetReviewPacket.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetProductionReadiness.cs`
- `Assets/TheCat/Tests/EditMode/P0StarterCatTurnaroundSourceLocksTests.cs`
- `Assets/TheCat/Tests/EditMode/P0AssetReviewPacketTests.cs`
- `Assets/TheCat/Tests/EditMode/P0AssetProductionReadinessTests.cs`

## Expected Output

- Keep `P0StarterCatTurnaroundConformanceSpec.EvaluateP0Spec()` green.
- Keep `P0AssetReviewPacket` and `P0AssetProductionReadiness` wired to the conformance spec.
- Update review packet docs if any anchor text changes.
- Update development log with validation and remaining Unity-side evidence.
- If a future candidate task is scheduled, keep candidate outputs under `design/development/asset_candidates/starter_cats` and outside `Assets`.

## Prohibited Changes

- Do not modify, overwrite, crop, or regenerate the colored turnaround PNGs.
- Do not modify the locked runtime cat sprites under `Assets/TheCat/Art/Characters/Sprites`.
- Do not import Batch 05 candidate outputs into `Assets`.
- Do not weaken source hashes, source-lock ids, active screenshot filenames, or formal import blocked state.
- Do not use the generated starter-cat lineup as the primary reference.
- Do not approve formal import; formal import remains blocked until active-cat Play Mode screenshots pass colored-turnaround review.
- Do not create one-cat-at-a-time candidate art from this conformance-only batch.

## Acceptance Criteria

- The conformance spec covers exactly Saiban, Nephthys, and Suzune.
- Each cat has at least 3 front-view, 3 side-view, 3 back-view, 3 palette, and 3 prop/costume anchors.
- Each cat has at least 4 prohibited drift rules covering generated-lineup drift, human-proportion drift, palette drift, and missing view anchors.
- `P0AssetReviewPacket.BuildMarkdown()` includes the "Starter Cat Turnaround Conformance Spec" section.
- `P0AssetProductionReadiness.EvaluateP0OfflineReadiness()` includes the conformance spec gate and keeps starter-cat formal import blocked.

## Validation

```powershell
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```

Unity validation remains pending until MCP/editor execution is available:

1. Refresh AssetDatabase.
2. Run `TheCat/P0/Write P0 Asset Review Packet`.
3. Run Play Mode screenshot smoke.
4. Compare `05-active-cat-saiban.png`, `06-active-cat-nephthys.png`, and `07-active-cat-suzune.png` against the colored three-view turnarounds and the conformance spec before allowing formal cat import approval.
