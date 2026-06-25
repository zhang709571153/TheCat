# P0 Implementation Task Breakdown

Status: current stable task entry point
Last updated: 2026-06-25

This document breaks the current P0 architecture into implementation slices.
It should be updated as slices land, blockers change, or review agents retire
old assumptions.

## Current Cut Line

Build forward from the existing P0.0 bedroom route and battle implementation.
Do not replace the current route, battle, cat upgrade, reward, or validation
systems while adding the missing live-source scope.

The next code should continue structural seams first:

1. Batch 87 battle-HUD candidate screenshot parity and import-review evidence
   against the cleaned I1 baseline.
2. Asset review gates for UI candidates that already have local preflight packs.
3. Starter-cat asset evidence cleanup without unlocking body-art replacement.

## Milestones

| Milestone | Goal | Exit criteria |
|-|-|-|
| M0 Source and planning lock | Keep the team pointed at current source truth | Stable architecture, task breakdown, README, blockers log, and development log agree. |
| M1 Cat-room contract | Add a testable hub model before building scene art | Bed, feeder, litter box, dream entrance, and return feedback are represented in state/presenter tests. |
| M2 Cat-room flow | Connect hub to existing route/battle loop | Menu or start flow can reach cat room, dream entry can continue into route, result can return to room. |
| M3 Dream map split | Make bedroom and Egypt formal map contexts | Runtime can distinguish map id/theme, target, entrances, props, and display copy without duplicating combat rules. |
| M4 P0 UI coverage | Make every required P0 screen reviewable | Entry, main, character select, cat room, route, battle HUD, skill/upgrade, pause/settings, and settlement have presenter or scene coverage. |
| M5 Asset approval | Install only approved candidates | Candidate import rows have Unity evidence before runtime binding; starter-cat body art remains locked. |
| M6 Demo release gate | Claim high-completion initial demo | Compile, tests, Play Mode smoke, screenshots, Console, and independent reviews are clean or documented. |

## Active Task Graph

| Id | Slice | Status | Depends on | Write scope | Review gate |
|-|-|-|-|-|-|
| A1 | Merge current source/agent findings into stable docs | Complete | none | `design/development/*.md` | Documentation review |
| B1 | Add cat-room state and presenter | Complete | A1 | `Assets/TheCat/Scripts/Runtime/Gameplay/*CatRoom*.cs`, focused tests | Code + design review |
| B2 | Add cat-room flow hook | Complete | B1 | `P0SceneFlow`, scene controller glue, tests | Unity integration review |
| B3 | Add return-to-cat-room result handoff | Complete | B1/B2 | run/session result flow, presenter tests | Gameplay review |
| B4 | Add dedicated cat-room Play Mode smoke and screenshots | Complete | B2/B3 | Play Mode smoke helpers, reports, screenshots | QA + design review |
| C1 | Add dream map/theme data model | Complete | A1/B2 | runtime data/catalog files and tests | Design consistency review |
| C2 | Route bedroom/Egypt context into battle start | Complete | C1 | route catalog, battle start context, battle controller | Gameplay review |
| C3 | Add Egypt readiness gate | Complete | C1/C2 | tools/checklists/docs | Validation review |
| D1 | Add entry and character-select UI contracts | Complete | A1/C3 | presenters/tools/tests | UI review |
| D2 | Add pause/settings and skill-selection acceptance coverage | Complete | D1 | presenters/tools/tests | UI + validation review |
| E1 | Polish bedroom battle readability while preserving rules | Complete | B/C as needed | existing battle HUD/presenter files | Play Mode screenshot review |
| F1 | Review Batch 83-88 UI candidate packs in install order | Complete as docs-only gate | Tesla/asset audit/E1 | docs/checklists only | Asset gate review |
| H1 | Add loading/start and full settings screenshot hooks | Complete at code/readiness level | F1 runtime-surface mapping | presenters/tools/tests only | UI code review |
| G1 | Refresh 11-capture Play Mode evidence and screenshot parity baseline | Complete for 2026-06-25 refresh | E1/F1/H1 | reports/screenshots/logs only | QA + design review |
| I1 | Fix battle-world label safe-area and result overlay hierarchy | Complete | G1 | battle view gates, smoke runners, tests, reports/screenshots | Screenshot + code boundary review |

## I1 Battle World Label And Overlay Fix

Goal: resolve the G1 P1 visual debt where yellow world-space diagnostic labels
overlapped the bed, cats, warning VFX, and result overlay in normal battle
screenshots.

Status: completed on 2026-06-25 as a battle readability and validation fix. No
candidate assets were imported or approved.

Implemented evidence:

- `GrayboxBattleController` now gates active-cat, bed, enemy, and enemy-status
  world diagnostic labels behind diagnostics HUD plus in-progress battle state.
- `GrayboxEnemyView` and `P0EnemyWarningIndicatorView` preserve warning
  rings/lines during active battle while hiding only warning text labels in the
  default collapsed HUD.
- Warning visuals are hidden after battle outcome before the result overlay is
  captured.
- `P0StatusIndicatorView`, `P0EnemyWarningIndicatorView`, graybox markers, and
  enemy fallback renderers no longer use `renderer.material` in EditMode paths.
- `P0PlayModeScreenshotSmoke` and `P0PlayModeRouteFlowSmoke` batch fixed smoke
  simulation ticks per frame, preserving the same drive order, delta, and max
  tick budgets while avoiding evidence-runner timeouts.
- Added `P0GrayboxBattleWorldDiagnosticsTests`.
- Focused Unity EditMode passed `19/19`:
  `Logs/p0_i1_world_label_overlay_final4_editmode_20260625.xml`.
- Final normal Editor Play Mode acceptance passed:
  `Logs/P0PlayModeAcceptanceVisual_I1_final_20260625.log`.
- Current Play Mode report is `passed`, `Smoke state: Passed`, `8/8` evidence
  checks, and `11/11` screenshots refreshed at `2026-06-25 21:22 +08:00`.

Independent review:

- Nietzsche: no P0/P1 screenshot finding; I1 visual debt is resolved.
- Sartre: no P0 code-boundary finding; follow-up test/material lifecycle risks
  were addressed before final validation.

Boundary:

- This does not approve Batch 87 candidate import or final art acceptance.
- Batch 87 may now proceed to batch-specific screenshot parity, import-settings,
  binding proof, and Console evidence.

## First Code Slice: B1

Goal: add the first cat-room hub contract without replacing the current menu to
route to battle implementation.

Status: completed on 2026-06-25 as a code-level contract. Scene assets,
BuildSettings, and runtime art remain untouched.

Expected outputs:

- `P0CatRoomState` or equivalent immutable/current-state model.
- `P0CatRoomPresenter` or equivalent display contract.
- Hub affordances for bed, feeder, litter box, dream entrance, and result
  feedback.
- Focused EditMode tests.
- No art import and no ProjectSettings edit in this slice.

Acceptance criteria:

- Presenter exposes player-facing labels and availability.
- Result feedback can represent at least victory, defeat, and route-complete
  returns.
- Feeder/litter/bed feedback is informational in the hub and does not mutate
  battle punishment rules.
- Existing route/battle tests and compile gates remain green.

Implemented evidence:

- Added `P0CatRoomPresenter`, `P0CatRoomState`, cat-room hotspots, actions, and
  return reasons.
- Added `P0SceneFlow.CatRoomSceneName`, `P0RunStartMode.CatRoom`, and
  `GameState.CatRoom` transition support without replacing current route/battle
  flow.
- Added focused tests in `P0CatRoomPresenterTests`.
- `TheCat.Runtime.csproj` and `TheCat.EditModeTests.csproj` MSBuild passed.
- Focused Unity EditMode run passed: `12/12`, result file
  `Logs/p0_cat_room_contract_editmode_20260625.xml`.

## Second Code Slice: B2/B3

Goal: connect the cat-room contract to flow once B1 is reviewed.

Status: completed at code, scene-validation, and Play Mode screenshot level on
2026-06-25.

Expected outputs:

- Central scene-flow id for cat room.
- Placeholder scene or controller hook only after B1 is stable.
- Dream-entry path that preserves existing route/battle behavior.
- Return-to-room handoff from settlement or battle result.

Forbidden until this slice starts:

- Editing `ProjectSettings/EditorBuildSettings.asset`.
- Replacing route map or battle controllers.
- Installing candidate art as runtime assets.

Implemented evidence:

- Added `CatRoomController` and `P0CatRoomSession`.
- Generated `Assets/TheCat/Scenes/P0CatRoom.unity` with `P0CatRoomRoot` and
  `CatRoomController`.
- Updated Build Settings order to `P0MainMenu`, `P0CatRoom`, `P0RouteMap`,
  `P0GrayboxBattle`.
- Updated `P0SceneSetupValidator` and added `P0CatRoomSceneBuilder`.
- Added a main-menu cat-room entry action and controller button.
- Added battle-result `ReturnCatRoom` action while preserving the existing
  `ContinueRoute` route-map path.
- Scene validation passed with `0` warnings:
  `Logs/p0_cat_room_scene_validation_20260625.log`.
- Focused Unity EditMode run passed: `27/27`, result file
  `Logs/p0_cat_room_flow_editmode_20260625_rerun.xml`.
- Play Mode acceptance smoke passed after the cat-room insertion:
  `design/development/unity_batchmode/P0_PLAYMODE_ACCEPTANCE_SMOKE_REPORT.md`.
  The current report validates an 11-capture plan and includes
  `design/development/screenshots/p0-playmode-smoke/02-cat-room.png`.

## Dream Map Slice: C1/C2

Goal: make bedroom and Egypt formal P0 map contexts.

Status: completed at code and focused-test level on 2026-06-25. Egypt remains
a formal placeholder target, not a finished playable art/content map.

Expected outputs:

- Map id/theme data model with display name, defense target, entrance labels,
  obstacle/prop tags, and default wave/content hooks.
- Bedroom as the current default map.
- Egypt as a formal pending/placeholder P0 target.
- Tests proving map data does not fork combat rules.

Implemented evidence:

- Added `DreamMapDefinition` and `P0DreamMapCatalog`.
- `RouteDefinition` now carries a `DreamMap` while existing route constructors
  default to the playable bedroom map.
- `P0RouteCatalog.CreateEgyptPlaceholderRoute()` creates an Egypt-context route
  with the same 10-layer structure and combat content ids.
- `P0RunSession` can start a run against a chosen dream map.
- `P0BattleStartContext` carries `DreamMap` and still resolves waves through
  the current route node content id.
- `P0RouteMapPresenter` exposes the current dream-map summary row.
- `P0PlayableReadiness` now includes a `Dream Maps` gate.
- Focused Unity EditMode passed: `78/78`, result file
  `Logs/p0_dream_map_context_focused_editmode_20260625.xml`.

Known boundary:

- The broader `P0CodeSmokeSuiteTests` run is still blocked by the existing
  starter-cat asset evidence gate, not by dream-map compile or behavior tests.

## Egypt Readiness Slice: C3

Goal: make Egypt visible as a P0 placeholder/readiness target without implying
that it is a finished playable map or a separate combat branch.

Status: completed at code, copy, tooling, and focused-test level on 2026-06-25.

Implemented evidence:

- Added `P0PlayableReadiness.EgyptReadinessCheckId`.
- `P0PlayableReadiness` now verifies that Egypt is registered as a placeholder,
  carries distinct display/theme/target labels, uses the same 10-layer route
  shape, and keeps combat content ids shared with the bedroom route.
- `P0RouteMapSurfaceCoverage` now includes an Egypt placeholder surface check
  and raises expected route-map checks from `20` to `21`.
- Cat-room dream-entry copy now says the playable path is the bedroom dream and
  that Egypt remains placeholder validation, so the player main path does not
  imply Egypt is fully playable.
- Focused Unity EditMode passed: `85/85`, result file
  `Logs/p0_egypt_readiness_editmode_20260625_final.xml`.

Known boundary:

- Egypt is not a unique combat/content map yet. It remains a registered
  placeholder target that reuses the current route and combat catalog until a
  later design-approved map-content slice.

## Entry And Character-Select UI Contract: D1

Goal: make the opening player path explicit and testable before adding more UI
surfaces or importing candidate art.

Status: completed at presenter, controller, readiness, and focused-test level
on 2026-06-25.

Implemented evidence:

- Added `P0MainMenuActionCategory` so the main menu distinguishes the only
  player-primary path from graybox route helpers, quick-battle debug shortcuts,
  and utility actions.
- `EnterCatRoom` is now the sole player-primary CTA and points to `P0CatRoom`
  with copy for `cat room -> bedroom dream -> center-bed defense`.
- Direct selected/default route starts remain available, but are labeled and
  classified as graybox route helpers.
- Quick battle remains available for debugging, but is classified as a
  graybox battle helper and no longer serves as player-main-path proof.
- Starter cards now expose selected/idle state labels, ready badges, skill
  previews, and source-locked HUD avatar ids while keeping internal visual
  tokens out of player-facing starter text.
- `P0PlayableReadiness` now includes `Entry Character Select`, proving the
  entry -> selected roster -> cat room -> bedroom dream route contract.
- Runtime and EditMode MSBuild passed.
- Focused Unity EditMode passed: `25/25`, result file
  `Logs/p0_d1_entry_character_select_editmode_20260625.xml`.
- Consumer smoke for `P0CodeSmokeSuiteTests` passed inside
  `Logs/p0_d1_entry_character_select_consumers_editmode_20260625.xml`.

Known boundary:

- The wider consumer batch still has two visual/architecture acceptance
  failures tied to the current starter-cat active screenshot/formal-import
  evidence state. Do not treat those as D1 entry/character-select failures, and
  do not unlock starter-cat body replacement from this slice.

## Pause Settings And Skill-Selection Acceptance: D2

Goal: make pause/settings and skill-selection reviewable as code-side
acceptance contracts before importing Batch 85 or Batch 89 candidate UI art.

Status: completed at presenter, controller-confirmation, readiness, and
focused-test level on 2026-06-25.

Implemented evidence:

- Added `P0PauseSettingsSurface` on top of the existing runtime settings
  presentation.
- Pause/settings now exposes pause/continue, speed controls, key hints, and a
  two-step restart confirmation contract.
- `GrayboxBattleController` now routes the restart input through
  request/confirm semantics instead of treating the pause/settings restart
  affordance as a direct one-click action.
- Runtime settings coverage now checks pause overlay, settings scope,
  restart confirmation, keyboard/button parity, and the Batch 85 candidate-only
  boundary.
- Added `P0SkillSelectionPresenter` and
  `P0SkillSelectionAcceptanceCoverage` to represent pending upgrade choices as
  ready, selected, disabled, and locked states with a confirm action.
- Skill-selection acceptance maps small-skill and ultimate choices through
  `P0SkillPresenter` and `P0CatUpgradeRuntimeCatalog` while preserving
  `RunCatUpgradeState.TrySelect()` as the only state mutation path.
- `P0PlayableReadiness` now includes `Pause Settings Acceptance` and
  `Skill Selection Acceptance` checks.
- Runtime and EditMode MSBuild passed.
- Focused Unity EditMode passed: `30/30`, result file
  `Logs/p0_d2_pause_skill_selection_editmode_20260625.xml`.

Known boundary:

- Batch 85 settings/pause and Batch 89 skill-selection assets remain
  candidate-only. This slice does not import PNGs, create `.meta` files, bind
  runtime art, change skill numbers, or replace the existing route-map upgrade
  flow.

## Bedroom Battle Readability: E1

Goal: make the default bedroom battle HUD answer "what matters now" before
deeper diagnostic text, while preserving combat rules, cooldowns, targeting,
route flow, and asset-import boundaries.

Status: completed at presenter, default-HUD hook, result-action surfacing,
readiness, and focused-test level on 2026-06-25.

Implemented evidence:

- Added `P0BattlePlayerBriefPresenter`, `P0BattlePlayerBrief`, and
  `P0BattleReadabilityCoverage`.
- The default battle HUD now shows a top brief with priority, current action,
  and compact threat/route context before denser HUD detail.
- Battle results now surface `继续路线`, `回到猫窝`, and `重新开始` above
  normal cat/skill controls while preserving existing scene-flow behavior.
- `P0PlayableReadiness` now includes `Battle Readability Acceptance`, covering
  priority/action/threat line caps, result actions, and candidate-only asset
  boundaries.
- `P0UnityRuntimeValidationPlan.BuildMarkdown()` now carries the exact
  `colored-turnaround` lock phrase required by the active-cat body-art review
  gate.
- Runtime, EditModeTests, and Editor MSBuild passed.
- Focused Unity EditMode passed: `47/47`, result file
  `Logs/p0_e1_battle_readability_editmode_20260625.xml`.
- Play Mode plan/evidence-structure Unity EditMode passed: `17/17`, result
  file `Logs/p0_e1_playmode_plan_editmode_20260625.xml`.
- `git diff --check` passed.

Known boundary:

- E1 did not change combat math, cooldowns, enemy timing, route progression,
  skill-selection state mutation, input mappings, candidate asset import, or
  starter-cat body art.
- G1 has refreshed the baseline Play Mode screenshots, but final battle visual
  acceptance remains blocked by the battle world label safe-area / overlay
  hierarchy debt found during screenshot review.

## Asset And UI Order

Use existing placeholders until evidence approves replacement assets.

Priority order:

1. Battle HUD UI candidates.
2. Character select/default starter surface.
3. Dream route and result/settlement surfaces.
4. Pause/settings/loading surfaces.
5. Map-specific props and Egypt placeholder visuals.

Starter-cat body art, framesheets, recolors, and runtime bindings remain locked
until active-cat screenshots pass the colored-turnaround comparison gate.

## Review Cadence

Run independent review at these points:

- After B1: code architecture and gameplay consistency.
- After B2/B3: scene flow and player-loop review.
- After C2: bedroom/Egypt map split review.
- F1 docs-only asset gate is complete; before any runtime install, run the
  matching Unity screenshot/import/binding/Console evidence gate for one batch
  at a time.
- G1 11-capture Play Mode evidence refresh is complete for the current
  baseline. Treat it as screenshot/evidence smoke only; it does not approve
  candidate import, runtime binding, install rows, or final visual polish.
- Before M6: full release-gate review covering source, code, gameplay, UI,
  assets, validation, and blockers.

Any claim based on `IZpFdIwtboEzzrx4ZFlcZLD2npe` is invalid until that document
is granted, fetched, and summarized into the current docs.

## F1 UI Candidate Import-Order Gate

Status: complete as a docs-only gate on 2026-06-25.

Evidence:

- Gate document:
  `design/development/asset_review/F1_UI_CANDIDATE_IMPORT_ORDER_GATE_2026-06-25.md`.
- Visual priority review: Mendel recommended Batch 88, 87, 86, 84, 85, then
  83, with fresh Play Mode screenshots required for every batch.
- Production/checklist review: Bernoulli confirmed the current queue/checklist
  is sufficient for candidate-only F1 documentation and insufficient for import
  approval.
- Runtime-surface review: Hypatia confirmed Batch 87, 88, 86, and 84 are ready
  for screenshot parity review. At F1 time, Batch 85 still needed a full
  settings-screen hook and Batch 83 still needed a dedicated loading/start
  surface plus screenshot hook; H1 has since added those hooks without
  approving import.
- Current queue/checklist counts are 19 queue items, 14 candidate-review items,
  and 5 Unity-blocked items.

Boundary:

- F1 covers Batch 83-88 only. Batch 89 skill-selection and Batch 90 cat-room
  remain in the broader Unity-evidence queue but are routed to later UI passes.
- No Batch 83-88 candidate art was imported, bound, or accepted.
- Starter-cat body art remains locked.

Next:

- G1 has refreshed the current 11-capture Play Mode baseline for Batch 87,
  Batch 88, Batch 86, and Batch 84 review context, but it is not an import or
  final visual acceptance gate.
- Battle world label safe-area / overlay hierarchy polish is required before
  claiming final Batch 87 battle-HUD or battle-world visual acceptance.
- H1 has added the Batch 85 full settings hook and Batch 83 loading/start hook;
  both still need dedicated Unity-rendered screenshots, import settings,
  binding proof, and Console review before any import decision.

## Loading Start And Full Settings Hooks: H1

Goal: make Batch 83 loading/start and Batch 85 full settings reviewable as
runtime hook surfaces without importing candidate art.

Status: completed at code, readiness, and focused-test level on 2026-06-25.
Screenshot/import evidence is still pending.

Implemented evidence:

- Added `P0LoadingStartPresenter`, `P0LoadingStartSurface`, and
  `P0LoadingStartCoverage`.
- Loading/start now exposes target scene/name, progress, spinner, detail rows,
  and a screenshot-hook flag while rejecting candidate asset references in the
  runtime contract.
- `MainMenuController` exposes a smoke-only loading/start surface builder, and
  `P0PlayModeScreenshotSmoke` validates the hook without changing the current
  11-capture screenshot plan.
- Extended `P0RuntimeSettingsPresenter` with `P0FullSettingsSurface`, tabs,
  option rows, speed choices, pause/continue, restart request, confirm
  restart, and restart-confirmation modal state.
- `P0RuntimeSettingsCoverage` now validates the full settings hook in addition
  to the existing pause overlay.
- H1 independent review found no P0 issues. Review-fix tightened Batch 83/85
  candidate-only boundaries with deep public-text scanning and changed
  pause/restart full-settings rows so they no longer present as speed values.
- `P0PlayableReadiness` now includes the `Loading Start Surface` gate and
  describes D2/H1 settings acceptance as overlay plus full settings hook.
- Runtime and EditModeTests MSBuild passed.
- Initial focused Unity EditMode passed: `31/31`, result file
  `Logs/p0_h1_loading_settings_editmode_20260625.xml`.
- Review-fix focused Unity EditMode passed: `33/33`, result file
  `Logs/p0_h1_loading_settings_reviewfix_editmode_20260625.xml`.

Known boundary:

- H1 does not import Batch 83 or Batch 85 PNGs, create candidate `.meta`
  files, approve Sprite import settings, bind runtime art, or replace any
  existing pause overlay behavior.
- Unity CLI validation for this project should omit `-quit`; with `-quit`,
  Unity exits after script refresh before the Test Framework writes XML.
- Batch 83 and Batch 85 are now hook-ready, not import-approved.
- The current Play Mode screenshot-smoke plan remains 11 captures; H1 validates
  hooks only and does not add dedicated loading/settings captures yet.

## G1 11-Capture Play Mode Evidence Refresh

Goal: refresh the current Play Mode screenshot/evidence baseline after E1 and
H1 without importing candidate art or claiming final visual acceptance.

Status: complete for the 2026-06-25 refresh. The retry normal Editor Play Mode
acceptance run passed, and independent review found no P0 blocker. P1 visual
debt remains for battle world labels and overlay hierarchy.

Implemented evidence:

- Refreshed `design/development/screenshots/p0-playmode-smoke` at
  `2026-06-25 11:54 +08:00` with 11 screenshots:
  `01-main-menu.png`, `02-cat-room.png`, `03-route-map-layer1.png`,
  `04-battle-hud-layer1.png`, `05-active-cat-saiban.png`,
  `06-active-cat-nephthys.png`, `07-active-cat-suzune.png`,
  `08-battle-world-visuals.png`, `09-call-tyrant-warning-vfx.png`,
  `10-battle-result-layer1.png`, and `11-settlement.png`.
- Regenerated
  `design/development/unity_batchmode/P0_PLAYMODE_ACCEPTANCE_SMOKE_REPORT.md`
  with `Result: passed`, `Smoke state: Passed`, no failures, `0` pending
  warnings, and `8` passed Play Mode evidence checks.
- Verified route-flow smoke through `10/10` nodes, `5` battles, boss observed,
  and defeat-flow smoke.
- Verified the H1 loading/start hook in screenshot smoke without changing the
  11-capture screenshot plan.
- Fixed the first G1 run's fatal screenshot-evidence OOM by releasing decoded
  `Texture2D` evidence textures immediately.

Validation:

- First G1 run failed during report generation:
  `Logs/P0PlayModeAcceptanceVisual_G1_20260625.log`. This log is root-cause
  evidence only and is superseded by the retry.
- Focused screenshot/evidence EditMode passed `18/18`:
  `Logs/p0_g1_screenshot_evidence_editmode_20260625.xml`.
- Retry Play Mode acceptance passed:
  `Logs/P0PlayModeAcceptanceVisual_G1_retry_20260625.log`.
- `P0_FULL_ACCEPTANCE_REPORT.md` remains "not evaluated by Play Mode smoke
  runner" and must not be described as full acceptance.

Known boundary:

- No candidate PNG import, candidate `.meta`, Sprite import settings approval,
  binding proof, formal install row, or final visual acceptance is included in
  G1.
- The screenshot file evidence gate validates expected PNG presence, decode,
  minimum dimensions, and sampled color variation. It does not judge visual
  composition.
- Battle world yellow world-space labels overlap the bed/scene/result overlay
  and top warning text can clip in the current screenshots. This is a P1 visual
  debt for the next battle-world UI polish slice, not a reason to discard the
  current G1 baseline.
