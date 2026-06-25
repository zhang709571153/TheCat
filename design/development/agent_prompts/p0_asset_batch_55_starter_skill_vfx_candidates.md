# P0 Asset Batch 55 - Starter Skill VFX Candidates

## Task Scope

Produce Codex-side symbolic VFX candidates for Saiban, Nephthys, and Suzune P0
skills. This batch must not redraw cat bodies and must not install Unity
assets.

## Required Design Inputs

- `design/梦境支配者核心玩法/docs`
- Locked starter-cat colored turnarounds:
  - `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
  - `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
  - `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`
- `design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`
- Current P0 VFX assets under `Assets/TheCat/Art/VFX`
- Current authority blessing icons under `Assets/TheCat/Art/UI/Icons`

## Code And Records To Read

- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0PrototypeCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0SkillPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0BlessingCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetProductionQueueCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetProductionQueueCoverage.cs`

## Expected Output

- Candidate PNGs under
  `design/development/asset_candidates/vfx/starter_skills/batch_55_starter_skill_vfx_candidates_2026-06-15`
- Transparent symbolic VFX candidates for:
  - Saiban defense/bedline authority
  - Nephthys control/moon-sand authority
  - Suzune healing/lullaby authority
- Manifest CSV, review sheet, review note, process note, and prompt path.

## Forbidden Changes

- Do not draw new cat bodies.
- Do not write into `Assets/TheCat/Art/VFX`.
- Do not create Unity `.meta` files.
- Do not modify prefabs, scenes, runtime visual bindings, or manifest catalog
  counts.
- Do not treat this candidate pack as a Unity install batch.

## Acceptance

- VFX candidates are symbolic and role-readable.
- The prompt and output must preserve the rule: no cat bodies.
- Palette and motif choices remain consistent with each cat's colored
  turnaround and authority blessing.
- Candidate rows use recommendation `candidate_review_only_do_not_import`.
- Review notes explicitly call out Unity install blockers and any visual risks.

## Validation

- Run `design/development/tools/validate_starter_skill_vfx_candidates.ps1`.
- Run MSBuild and `git diff --check`.
- Unity MCP validation is deferred until a formal Unity install candidate is
  chosen.
