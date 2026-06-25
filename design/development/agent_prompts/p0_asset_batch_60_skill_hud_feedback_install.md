# P0 Asset Batch 60 - Skill HUD Feedback Install

Date: 2026-06-15

## Task Scope

Promote the accepted non-cat Batch 57 Skill HUD feedback candidates into Unity
as a formal installed asset batch. Wire the installed assets into runtime
catalogs and the IMGUI battle HUD so P0 skill readiness, cooldown, targeting,
hunger pressure, auto-target, and interaction range cues have concrete visual
bindings.

This is a non-cat UI/VFX installation batch. It must not create, replace, or
approve starter-cat body art.

## Required Design Sources

- `design/梦境支配者核心玩法/docs`
- `design/development/agent_prompts/p0_asset_batch_57_skill_hud_feedback_candidates.md`
- `design/development/asset_candidates/ui/skill_hud/batch_57_skill_hud_feedback_candidates_2026-06-15`
- Current UI shell and status icon assets under `Assets/TheCat/Art/UI`

## Code And Records To Read

- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0SkillHudPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/GrayboxBattleController.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0SkillHudCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RuntimeVisualBindingCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetMetaImportSettingsReadiness.cs`
- `Assets/TheCat/Tests/EditMode/P0SkillHudCoverageTests.cs`
- `Assets/TheCat/Tests/EditMode/P0VisualAssetCatalogTests.cs`

## Expected Outputs

- Installed Unity PNGs:
  - `Assets/TheCat/Art/UI/Icons/thecat_ui_skill_ready_frame_512_v001.png`
  - `Assets/TheCat/Art/UI/Icons/thecat_ui_skill_cooldown_overlay_512_v001.png`
  - `Assets/TheCat/Art/UI/Icons/thecat_ui_skill_no_target_marker_512_v001.png`
  - `Assets/TheCat/Art/UI/Icons/thecat_ui_skill_hunger_cost_chip_512_v001.png`
  - `Assets/TheCat/Art/UI/Icons/thecat_ui_auto_target_reticle_512_v001.png`
  - `Assets/TheCat/Art/UI/Icons/thecat_ui_interaction_range_ripple_512_v001.png`
- Matching `.png.meta` files with Sprite Single import settings, disabled
  mipmaps, alpha transparency, and `TheCatP0ImportSettings:v1`.
- Batch 60 manifest and review packet under:
  `design/development/asset_candidates/ui/skill_hud/batch_60_skill_hud_feedback_install_2026-06-15`
- Runtime catalog bindings:
  - `skill_hud.ready_frame`
  - `skill_hud.cooldown_overlay`
  - `skill_hud.no_target_marker`
  - `skill_hud.hunger_cost_chip`
  - `skill_hud.auto_target_reticle`
  - `battle_hud.interaction_range_ripple`
- Skill HUD runtime cards expose installed visual assets through presenter
  fields and the IMGUI battle HUD draws them when the PNGs resolve.

## Forbidden Changes

- Do not generate or import cat body art.
- Do not overwrite starter-cat combat sprites or HUD avatars.
- Do not edit colored turnaround source PNGs.
- Do not modify scene or prefab files in this batch.
- Do not mark Unity editor-side validation complete without an actual editor
  Console/screenshot/import pass.

## Acceptance

- All installed PNGs are exactly `512x512`.
- All installed PNGs have transparent corners.
- All installed meta files carry `TheCatP0ImportSettings:v1`.
- `P0AssetManifestCatalog.P0ManifestAssetCount` is updated to include the six
  installed assets.
- `P0VisualAssetCatalog.P0RuntimeVisualBindingCount` is updated and all six
  runtime bindings resolve through catalog getters.
- `P0SkillHudPresenter` exposes skill-state visual references for ready,
  cooldown, no-target, low-hunger, hunger cost, and auto-target cues.
- `P0RuntimeVisualBindingCoverage.EvaluateP0Bindings()` remains complete.
- Development records state that Unity MCP visual smoke remains pending if the
  editor tools are unavailable.

## Validation

```powershell
& 'C:\Users\PC\.cache\codex-runtimes\codex-primary-runtime\dependencies\python\python.exe' design/development/tools/build_skill_hud_feedback_install_assets.py
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_skill_hud_feedback_install.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```

Unity MCP validation, when available:

- Refresh AssetDatabase.
- Inspect the six Sprite import settings.
- Run the battle scene and capture skill HUD states for ready, cooldown,
  no-target, low-hunger, and targeted skill cards.
- Confirm the Console stays clean.
