# P0 Asset Batch 04 Runtime Visual Acceptance Agent Prompt

## Task Scope

Validate the current P0 runtime visual integration before starting any new
systematic asset-production batch. The goal is to prove that the 21
manifest-backed runtime visual slots are visible, source-locked, and reviewable
inside Unity, without approving final visual polish yet.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/00_overview/p0_minimum_design.md`
- `design/梦境支配者核心玩法/docs/01_core_gameplay/core_gameplay_rules_and_player_path.md`
- `design/梦境支配者核心玩法/docs/03_characters/character_design.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/asset_review/P0_RUNTIME_VISUAL_REVIEW_PACKET.md`
- `design/development/asset_review/p0_runtime_visual_contact_sheet_2026-06-14.png`
- `design/development/asset_review/p0_starter_cat_turnaround_review_2026-06-14.png`

## Code Files To Read

- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0WorldVisualAssetView.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/GrayboxBattleController.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/GrayboxEnemyView.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0EnemyWarningIndicatorView.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RuntimeVisualBindingCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetReviewPacket.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatTurnaroundSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetProductionReadiness.cs`
- `Assets/TheCat/Scripts/Editor/P0BatchmodeAcceptanceRunner.cs`

## Expected Output

- A short Markdown validation report under
  `design/development/asset_review/` with:
  - acceptance gate results
  - Unity Console status
  - screenshot paths
  - visual observations for all 34 runtime slots
  - comparison notes against the offline runtime visual contact sheet
  - starter cat comparison notes against the colored turnaround contact sheet
  - blocker list and non-blocking polish notes
- Updated `design/development/DEVELOPMENT_LOG.md`.
- If Unity screenshots expose a clear wiring bug, a narrow code fix plus
  compile/test evidence.

## Runtime Visual Slots To Verify

- `background.bedroom_dream`
- `cat.combat.saiban`
- `cat.combat.nephthys`
- `cat.combat.suzune`
- `enemy.combat.black_mud`
- `enemy.combat.cold_light`
- `enemy.combat.call_tyrant`
- `prop.bed`
- `prop.litter_box`
- `prop.feeder`
- `core.owner_sleep`
- `core.cat_hp`
- `core.team_poop`
- `core.team_hunger`
- `status.sleep_stable`
- `status.slow`
- `status.knockback`
- `status.mark`
- `status.shield`
- `route.boss_node`
- `warning.call_tyrant`

## Prohibited Changes

- Do not regenerate or replace starter cat assets unless the user explicitly
  asks. The current cat sprites are locked to the colored turnaround source
  images.
- Do not bulk-generate new P1 content.
- Do not change combat tuning, route logic, or core values while doing visual
  acceptance unless a visual blocker makes it unavoidable.
- Do not mark an asset final from filesystem presence alone.

## Acceptance Criteria

- `P0 Offline Asset Production Readiness` passes.
- `Runtime Visual Binding Coverage` reports 21 bindings and 21 resolved
  textures.
- `Asset Review Packet` reports 38 review assets and 34 runtime-bound entries.
- `P0 Offline Asset Production Readiness` reports the runtime visual contact
  sheet is present.
- `P0StarterCatVisualConsistencyChecklist` reports 3 checklist cats, at least
  15 required colored-turnaround traits, 3 source-lock matches, and 3
  active-cat screenshot matches.
- `P0PlayModeScreenshotSmoke.HasP0ScreenshotCapturePlan()` and
  `HasRuntimeVisualScreenshotCapturePlan()` both pass.
- `P0PlayModeEvidenceChecklist` includes 7 checks, including
  `runtime_visual_contact_sheet` and `screenshot_file_evidence`.
- Unity Console has no errors after opening the P0 scenes and entering Play
  Mode.
- Screenshots show the 34 runtime visual slots in route map, battle HUD,
  status HUD, or battle world.
- Saiban, Nephthys, and Suzune are compared against the colored turnaround
  contact sheet; any drift in markings, costume, props, or non-human cat body
  proportions blocks further cat-related asset production.

## Unity Verification

Preferred with Unity MCP:

1. Query Unity editor state and active scene.
2. Run `TheCat/P0/Run Acceptance Gates (Log Only)`.
3. Inspect Console for errors.
4. Open `P0MainMenu`, `P0RouteMap`, and `P0GrayboxBattle`.
5. Enter Play Mode and capture screenshots for:
   - `01-main-menu.png`: main menu start surface
   - `02-route-map-layer1.png`: route map with Boss node icon
   - `03-battle-hud-layer1.png`: battle HUD with four-core icons and cat HUD
   - `05-active-cat-saiban.png`: active Saiban visual
   - `06-active-cat-nephthys.png`: active Nephthys visual
   - `07-active-cat-suzune.png`: active Suzune visual
   - `07-battle-world-visuals.png`: Bedroom Dream battle background, Black
     Mud, Cold Light, bed, litter box, feeder, and active cat world visuals
   - `08-call-tyrant-warning-vfx.png`: Call Tyrant summon or throw warning VFX
   - `09-battle-result-layer1.png`: first battle result surface
   - `10-settlement.png`: run settlement surface

Fallback without Unity MCP:

1. If no Unity editor instance has the project open, run
   `design/development/unity_batchmode/P0_BATCHMODE_ACCEPTANCE_RUNBOOK.md`.
2. If the project is already open, do not close it automatically. Record the
   blocker and run only MSBuild / text-level checks.
3. Use the existing Unity menu manually in the open editor when a human can
   operate it.
