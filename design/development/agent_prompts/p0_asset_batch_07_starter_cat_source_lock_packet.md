# P0 Asset Batch 07 Starter Cat Source-Lock Packet Agent Prompt

## Task Scope

Maintain the source-lock evidence packet for Saiban, Nephthys, and Suzune before
any further starter-cat asset production. This is a preflight/review batch, not
an image-generation batch and not a Unity import batch.

The goal is to make every future cat derivative prove that it matches the
colored turnarounds from the design source before a candidate is accepted.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/03_characters/character_design.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`

## Code Files To Read

- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatTurnaroundSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatSourceLockPacketEvidence.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatFormalImportReadiness.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetProductionReadiness.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetReviewPacket.cs`
- `Assets/TheCat/Tests/EditMode/P0StarterCatSourceLockPacketEvidenceTests.cs`
- `Assets/TheCat/Tests/EditMode/P0StarterCatTurnaroundSourceLocksTests.cs`

## Required Outputs

- Regenerate:
  - `design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.md`
  - `design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.csv`
- Keep the deterministic builder current:
  - `design/development/tools/build_starter_cat_source_lock_packet.ps1`
- Update development notes if the source-lock packet, hashes, candidate review
  sheet links, or formal import gate state changes.

## Prohibited Changes

- Do not modify colored turnaround source PNGs.
- Do not overwrite the three locked Unity starter-cat sprites.
- Do not copy Batch 05 candidates into `Assets`.
- Do not approve formal import unless all three active-cat Play Mode screenshots
  exist and pass side-by-side review against the colored turnarounds.
- Do not use the generated starter-cat lineup as the primary source of truth.

## Acceptance Criteria

- `P0StarterCatSourceLockPacketEvidence.EvaluateCurrentPacket()` passes.
- `P0StarterCatTurnaroundSourceLocks.EvaluateP0Locks()` still passes.
- The packet records all three:
  - colored turnaround paths and SHA-256 hashes
  - locked Unity sprite paths and SHA-256 hashes
  - runtime binding ids
  - active-cat Play Mode screenshot filenames
  - Batch 05 review notes and review sheets
  - explicit `do not import into Unity yet` decision while screenshots are
    missing
- `P0AssetReviewPacket` includes the source-lock packet section.
- `P0AssetProductionReadiness` fails if the packet is missing or stale.

## Validation

```powershell
& .\design\development\tools\build_starter_cat_source_lock_packet.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:OutDir=Temp\Bin\RuntimeStarterCatSourceLockPacket\ /v:minimal
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:OutDir=Temp\Bin\EditModeStarterCatSourceLockPacket\ /v:minimal
git diff --check
```

Unity validation remains pending until Unity MCP/editor execution is available:

1. Refresh AssetDatabase.
2. Run `TheCat/P0/Write P0 Asset Review Packet`.
3. Run Play Mode screenshot smoke.
4. Compare `05-active-cat-saiban.png`, `06-active-cat-nephthys.png`, and
   `07-active-cat-suzune.png` against the colored-turnaround contact sheet and
   this source-lock packet.
