# P0 Asset Batch 57 - Skill HUD Feedback Candidates

## Task Scope

Produce Codex-side, non-cat UI/VFX candidate assets for P0 skill-HUD feedback.
This batch improves player operation readability for skill readiness,
cooldown, targeting, hunger cost, auto target, and interactable range cues.

This is a candidate-production batch only. It must not install Unity assets.

## Required Design Inputs

- `design/梦境支配者核心玩法/docs`
- Current UI shell assets under `Assets/TheCat/Art/UI`
- Current P0 status icons under `Assets/TheCat/Art/UI/Icons`
- Current P0 battle feedback VFX under `Assets/TheCat/Art/VFX`
- Current skill HUD presenter and affordance code.

## Code And Records To Read

- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0PrototypeCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0SkillPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0SkillHudPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0SkillIndicatorPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0BattleActionAffordancePresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetProductionQueueCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0ArchitectureCompletionAudit.cs`

## Expected Output

- Candidate PNGs under
  `design/development/asset_candidates/ui/skill_hud/batch_57_skill_hud_feedback_candidates_2026-06-15`
- Candidate subjects:
  - skill ready frame
  - skill cooldown overlay
  - no target marker
  - hunger cost chip
  - auto target reticle
  - interaction range ripple
- Manifest CSV, review sheet, review note, process note, and this prompt path.

## Forbidden Changes

- Do not write into `Assets/TheCat/Art/UI`.
- Do not write into `Assets/TheCat/Art/VFX`.
- Do not create Unity `.meta` files.
- Do not modify prefabs, scenes, runtime visual bindings, or manifest catalog
  counts.
- Do not draw cat bodies, cat portraits, starter-cat costume fragments, fur
  patterns, or colored-turnaround crops.
- Do not treat this candidate pack as a Unity install batch.

## Acceptance

- Candidate rows use recommendation `candidate_review_only_do_not_import`.
- Every candidate remains transparent PNG review material outside `Assets`.
- Visual language stays non-cat and readable at HUD scale.
- The review note explicitly calls out that Unity install requires runtime
  scale, HUD readability, scene/prefab binding, Console, and screenshot checks.
- The prompt and output preserve the rule: non-cat UI/VFX only.

## Validation

- Run `design/development/tools/validate_skill_hud_feedback_candidates.ps1`.
- Run MSBuild and `git diff --check`.
- Unity MCP validation is deferred until a formal Unity install candidate is
  chosen.
