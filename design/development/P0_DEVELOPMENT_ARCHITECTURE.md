# P0 Development Architecture

Status: current stable entry point
Last updated: 2026-06-25

This document is the current architecture entry point for the TheCat P0 Unity
demo. Dated files are evidence snapshots; this file is the working contract for
future implementation and review.

## Source Authority

| Source | Status | Use rule |
|-|-|-|
| `Qr1XdXd6KosnjMxjgW7cS89kn9c` | Live fetch/export succeeded on 2026-06-25 | Current product and P0 boundary authority. |
| `MDrSdEoaToB5cnxZgrOcAE34nof` | Live fetch/export blocked by Feishu API `3380004` | Use the synced local 2026-06-13 gameplay copy until access is granted. |
| `IZpFdIwtboEzzrx4ZFlcZLD2npe` | Live fetch/export blocked by Feishu API `3380004`; no local manifest copy | Hard blocker. Do not infer design content from this file. |

The Feishu blocker is ACL, not local OAuth. The current token is valid, but the
second and third documents deny document operations.

## P0 Demo Target

The current P0 target is a complete first playable loop, not only an isolated
battle prototype:

1. Entry/title screen.
2. Main menu.
3. Cat room hub with bed, feeder, litter box, dream entrance, and return
   feedback.
4. Starter selection or default three-cat start.
5. Dream route selection.
6. Bedroom dream battle path.
7. Egypt dream theme represented as a formal P0 map target.
8. Reward, rest, shop, event, blessing, and cat-upgrade surfaces.
9. Layer-10 boss and settlement.
10. Return to cat room with visible growth/result feedback.

The existing bedroom 10-layer route is P0.0 implementation evidence. The live
source boundary extends it into a hub-and-dream P0 demo rather than replacing
it.

## Non-Negotiable Gameplay

- 10-layer route with layer-10 boss.
- Center-bed defense battle.
- Three starter cats as the core combat team.
- Four core values: owner sleep, cat HP, team poop, and team hunger.
- Cat switching, skills, auto-targeting, interactables, rewards, blessings,
  event items, and joined-cat upgrades.
- P0 status tags: sleep stable, slow, knockback, mark, and shield.
- Cat-room feedback must support the loop without becoming a separate
  real-time punishment system.

## System Layers

| Layer | Responsibility | Current direction |
|-|-|-|
| Source control room | Source status, blockers, retired guidance, agent prompts | Keep this file, task breakdown, blockers log, and development log as current entry points. |
| Scene flow | Entry/menu/cat room/route/battle/settlement transitions | Centralize scene ids and add cat-room handoff without disrupting current route/battle flow. |
| Run state | Route, roster, wallet, blessings, event items, cat upgrades, core values | Extend existing run state with cat-room-facing summary and return feedback. |
| Battle simulation | Bed defense, enemies, skills, statuses, boss, values | Preserve current rules; add dream-map context so bedroom and Egypt can share mechanics. |
| Content catalogs | Cats, skills, enemies, blessings, route, maps, visual bindings | Add map/theme data before broad content expansion. |
| UI presenters | Testable screen/presenter contracts | Add cat room, entry, character-select, pause/settings, upgrade, and settlement coverage as presenter-first slices. |
| Asset gates | Candidate review, source locks, import readiness | Keep candidates outside runtime until Unity evidence approves specific install rows. |
| Validation | Compile, EditMode, Play Mode, screenshots, Console, reviews | Extend gates for cat room, Egypt, and return-to-room loop before claiming demo completion. |

## Current Evidence

The existing implementation already proves a real P0.0 slice:

- `design/development/unity_batchmode/P0_PLAYMODE_ACCEPTANCE_SMOKE_REPORT.md`
  reports the current normal Editor Play Mode smoke passed, screenshot
  evidence present, route completion through 10 layers, boss observation, and
  defeat flow. The 2026-06-25 11:54 refresh has 11 validated capture files.
- Build settings already include `P0MainMenu`, `P0CatRoom`, `P0RouteMap`, and
  `P0GrayboxBattle`.
- Runtime and review asset gates currently report 118 review assets and 111
  runtime bindings.
- `design/development/unity_batchmode/P0_OFFLINE_ACCEPTANCE_REPORT.md` is not
  currently green because starter-cat source-lock/turnaround/strict-candidate
  evidence remains stale or incomplete. Treat this as an asset evidence blocker,
  not as a cat-room or dream-map behavior failure.

These facts do not complete the live-source P0 boundary because full UI
coverage, candidate asset-install approval, final visual/Console review, and
release review remain first-class gaps.

2026-06-25 B1 update: the first cat-room code contract now exists
(`P0CatRoomPresenter`, `P0CatRoomState`, cat-room scene-flow constants, and
focused EditMode tests). This proves the hub contract, not a playable cat-room
scene. G2 remains open until B2/B3 connect the scene and return handoff.

2026-06-25 B2/B3 update: `P0CatRoom` now exists as a Unity scene, is included
in Build Settings after the main menu, has `P0CatRoomRoot` plus
`CatRoomController`, and has a main-menu entry path plus a battle-result
`ReturnCatRoom` action. G2 is now complete at code and scene-validation level;
Play Mode screenshot evidence now includes `02-cat-room.png`.

2026-06-25 C1/C2 update: `DreamMapDefinition` and `P0DreamMapCatalog` now make
bedroom and Egypt distinct dream contexts. The existing route constructors
default to the playable bedroom map, `P0RouteCatalog.CreateEgyptPlaceholderRoute()`
registers Egypt as a formal P0 placeholder target, and `P0BattleStartContext`
carries the current dream map while still resolving waves from route-node
content ids. G3 is complete at context and focused-test level.

2026-06-25 C3 update: `P0PlayableReadiness` now has an explicit Egypt readiness
gate, `P0RouteMapSurfaceCoverage` includes the Egypt placeholder surface, and
cat-room dream-entry copy names the playable path as bedroom dream while Egypt
remains placeholder validation. Focused Unity EditMode passed `85/85` in
`Logs/p0_egypt_readiness_editmode_20260625_final.xml`.

2026-06-25 D1 update: `P0MainMenuPresenter` now marks the cat-room action as
the only `PlayerPrimary` entry CTA, demotes direct route and quick-battle paths
to graybox helpers, and exposes character-select starter card state, ready
badges, skill previews, and source-locked HUD avatar ids without leaking
internal visual tokens into player-facing text. `P0PlayableReadiness` now
includes `Entry Character Select`. Focused Unity EditMode passed `25/25` in
`Logs/p0_d1_entry_character_select_editmode_20260625.xml`; `P0CodeSmokeSuiteTests`
passed in the consumer batch.

2026-06-25 D2 update: pause/settings and skill-selection now have code-side
acceptance contracts before visual import. `P0PauseSettingsSurface` covers
pause/continue, speed controls, key hints, shared key/button bindings, and
two-step restart confirmation. `P0SkillSelectionPresenter` covers ready,
selected, disabled, and locked upgrade-choice states while mapping small-skill
and ultimate choices through existing runtime skill definitions. `P0PlayableReadiness`
now includes `Pause Settings Acceptance` and `Skill Selection Acceptance`.
Focused Unity EditMode passed `30/30` in
`Logs/p0_d2_pause_skill_selection_editmode_20260625.xml`.

2026-06-25 E1 update: bedroom battle readability now has a code-side player
brief and readiness gate. `P0BattlePlayerBriefPresenter` puts priority,
current action, and compact threat context at the top of the default battle
HUD, and battle results now surface continue-route, return-to-cat-room, and
restart actions before normal cat/skill controls. `P0PlayableReadiness` now
includes `Battle Readability Acceptance`. Focused Unity EditMode passed
`47/47` in `Logs/p0_e1_battle_readability_editmode_20260625.xml`; Play Mode
plan/evidence-structure checks passed `17/17` in
`Logs/p0_e1_playmode_plan_editmode_20260625.xml`. G1 later refreshed the
baseline Play Mode screenshots; battle-world label hierarchy remains the next
visual acceptance debt before claiming final battle polish.

2026-06-25 F1 update: Batch 83-88 UI candidate packets now have a docs-only
import-order gate in
`design/development/asset_review/F1_UI_CANDIDATE_IMPORT_ORDER_GATE_2026-06-25.md`.
The visual import-review priority is Batch 88, 87, 86, 84, 85, then 83. The
immediate screenshot-parity order is Batch 87, 88, 86, then 84 because those
runtime surfaces already exist. F1 identified missing hooks for Batch 85 full
settings and Batch 83 loading/start; H1 addresses those hooks, but screenshot
and import evidence remains pending. This does not approve import, binding,
runtime use, or starter-cat body-art replacement. Batch 89 and Batch 90 remain
in the broader Unity-evidence queue for later skill-selection and cat-room UI
passes.

2026-06-25 H1 update: Batch 83 and Batch 85 now have code-side screenshot
hooks, but not visual acceptance. `P0LoadingStartPresenter` exposes a
loading/start surface with target, progress, spinner, detail rows, screenshot
hook, and deep candidate-token rejection. `P0RuntimeSettingsPresenter` exposes
a full settings surface with tabs, semantic option rows, restart confirmation,
and deep Batch 85 candidate-token rejection while preserving the existing pause
overlay. The Play Mode screenshot plan remains 11 captures, so H1 is hook-ready
only; Unity-rendered screenshots, import settings, binding proof, and clean
Console are still required before any Batch 83 or Batch 85 import decision.
Focused Unity EditMode passed `33/33` in
`Logs/p0_h1_loading_settings_reviewfix_editmode_20260625.xml`.

2026-06-25 G1 update: the current 11-capture Play Mode evidence baseline was
refreshed after E1/H1. The first run captured screenshots but hit a fatal
Texture allocation OOM while building screenshot file evidence; that run is
superseded by the retry after `P0PlayModeScreenshotFileEvidence` began
destroying decoded transient textures immediately. Focused screenshot/evidence
EditMode passed `18/18` in
`Logs/p0_g1_screenshot_evidence_editmode_20260625.xml`; the retry normal
Editor Play Mode acceptance passed in
`Logs/P0PlayModeAcceptanceVisual_G1_retry_20260625.log`. The current report has
no failures, `0` pending warnings, `8` passed checks, and 11 validated captures
in `design/development/screenshots/p0-playmode-smoke`. G1 is evidence-smoke
only: it does not approve candidate import, runtime binding, install rows, or
final visual acceptance. Independent review keeps a P1 visual debt open for
battle world label safe-area / overlay hierarchy.

## Development Gates

| Gate | Exit criteria |
|-|-|
| G0 Source lock | `Qr1` live, `MDr` local-copy fallback, and `IZp` blocker recorded. |
| G1 Evidence refresh / screenshot parity | Current 11-capture Play Mode baseline is regenerated and reviewed as reports/screenshots only. No candidate import, install row, or final visual acceptance is implied. |
| G2 Cat-room loop | Player can enter cat room, start a dream, resolve a run/battle path, and return to cat room. |
| G3 Dream map split | Bedroom and Egypt are represented as distinct dream contexts without duplicating combat rules. |
| G4 P0 UI coverage | Entry, main, character select, cat room, route, battle HUD, upgrade, pause/settings, and settlement are reviewable. D1 covers entry/character-select; D2 covers pause/settings and skill-selection acceptance; E1 covers battle readability at code/readiness level; H1 covers loading/start and full settings hooks; G1 refreshed the baseline screenshots but battle-world label hierarchy and settlement polish remain next. |
| G5 Asset install review | F1 docs-only order exists for Batch 83-88. H1 makes Batch 83 and Batch 85 hook-ready, but non-cat UI/art candidate rows remain unapproved until Unity import, binding, screenshot, and Console checks pass. |
| G6 Demo release gate | Compile, focused tests, Play Mode smoke, screenshot matrix, Console review, and independent review are clean or explicitly deferred. |

## Retired From Active Planning

| Retired assumption | Replacement |
|-|-|
| Blank-project assumptions from 2026-06-13 | Current Runtime, Gameplay, Roguelite, Tools, scenes, tests, and asset catalog are real. |
| Single-bedroom-only P0 as final claim | Bedroom route is P0.0; live-source P0 adds cat room and Egypt dream. |
| Treating `IZp` as known design input | `IZp` is unknown until access is granted and content is fetched. |
| Blind large asset generation | Review existing candidate packs and approve one install row at a time. |
| Starter-cat body-art replacement | Locked until active-cat screenshot review passes against colored turnarounds. |
| Large item/attribute-blessing expansion | Defer until the core P0 loop is proven. |

## Update Rule

When source access changes, a gate passes, or a stale assumption is retired,
update this file, `P0_IMPLEMENTATION_TASK_BREAKDOWN.md`,
`P0_BLOCKERS_AND_RETIREMENT_LOG_2026-06-24.md`, and `DEVELOPMENT_LOG.md`
together. Dated snapshot files may be kept for evidence, but should not be the
only current entry point.
