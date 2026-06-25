# P0 Development Architecture - 2026-06-25

Checked: 2026-06-25 09:36 +08:00

This is the current development architecture for the TheCat / 喵梦空间 P0 demo.
It supersedes the planning boundary in `P0_CURRENT_ARCHITECTURE_2026-06-24.md`
where the live Feishu source has moved the P0 target beyond the single bedroom
route demo. The 2026-06-24 document remains useful as an implementation status
map for the current codebase.

## 1. Source Authority

| Source | Current access | Local use rule |
|-|-|-|
| `Qr1XdXd6KosnjMxjgW7cS89kn9c` | Live fetch/export succeeded; title `《喵梦空间》游戏总策划案`; revision `816` | Current product/P0 boundary authority. Snapshot exported to `tmp/feishu_source_2026-06-25/01_qr1_game_master_plan.md` for this pass. |
| `MDrSdEoaToB5cnxZgrOcAE34nof` | Live fetch/export returned Feishu API `3380004` under the valid user token | Use the synced 2026-06-13 local copy at `design/梦境支配者核心玩法/docs/01_core_gameplay/core_gameplay_rules_and_player_path.md` until live access is granted. |
| `IZpFdIwtboEzzrx4ZFlcZLD2npe` | Live fetch/export returned Feishu API `3380004` and no local manifest copy exists | Hard blocker. Do not infer content from this document. |

The current Feishu blocker is ACL, not OAuth. `lark-cli auth status --verify`
reported the user token valid, but the second and third documents denied
document operations with `3380004`.

## 2. Design Boundary Update

The earlier implementation target centered on:

- main menu;
- 10-layer route map;
- bedroom dream defense battle;
- three starter cats;
- four core values;
- route rewards, boss, and settlement.

The live `Qr1` source now defines P0 as a fuller demo loop:

- cat room preparation;
- entering a dream from the bed or dream entrance;
- at least one full bedroom dream battle/run path;
- growth feedback;
- return to the cat room;
- P0 art/UI coverage for entry screen, main menu, cat room, battle HUD, pause,
  settings, skill selection, and settlement;
- two dream themes in the P0 boundary: male-owner bedroom dream and Egypt
  dream, with each map having a defense target, monster entrances, obstacles,
  and theme props.

Architecture decision: keep the existing bedroom 10-layer route as P0.0 core
implementation evidence, then extend it into the live-source P0.1/P0.2 product
demo instead of rewriting it.

## 3. Target Player Loop

The target demo loop is:

1. Entry/title screen.
2. Main menu.
3. Cat room hub with bed, feeder, litter box, and dream entrance.
4. Starter selection or default three-cat start.
5. Dream route selection.
6. Dream battle map: bedroom first, Egypt second.
7. Battle rewards: fish treats, shared cat experience, event item, blessing, or
   rest/shop result.
8. Cat upgrade or build choice when shared experience is full.
9. Boss / route settlement.
10. Return to cat room with visible growth feedback and restart/continue
    affordance.

P0 must not depend on complete story cinematics, complete four-chapter content,
full boss pool, full cat gacha, large item pool, or formal starter-cat body
replacement art.

## 4. System Architecture

| Layer | Responsibility | Current status | Next architectural work |
|-|-|-|-|
| Source control room | Source authority, blockers, retirement, agent prompts | Exists in `design/development` | Promote this file and task graph as current entry points. |
| Scene flow | Entry/menu/cat room/route/battle/settlement transitions | Menu, route, battle exist | Add cat room state and scene contract; keep scene flow enum/string ownership centralized. |
| Run state | Route, roster, wallet, blessings, event items, cat upgrades, core values | Substantial implementation exists | Add cat-room-facing persistent state and return-to-room handoff. |
| Battle simulation | Bed defense, enemies, skills, core values, status tags, boss | Substantial implementation exists | Support map theme/context so bedroom and Egypt can share rules with different content. |
| Content catalogs | Cats, skills, enemies, route, blessings, visual bindings | Substantial implementation exists | Add dream-map/theme catalog entries and avoid hardcoding bedroom-only labels. |
| UI presentation | Testable presenter state plus IMGUI demo surfaces | Substantial IMGUI/presenter coverage exists | Use presenters as the contract; add player-facing screens for cat room, character select, pause/settings, and skill selection before large visual replacement. |
| Asset gates | Manifest, source locks, import readiness, candidate review | Strong offline gates exist | Keep candidate UI/asset packs outside runtime until Unity review approves specific rows. |
| Validation | Compile, EditMode, offline gates, Play Mode smoke, screenshots | Current offline and Play Mode smoke are green | Add cat room, Egypt map, and live-source boundary checks to acceptance gates. |

## 5. Required P0 Systems

### Opening And Hub

- Entry/title state.
- Main menu with default start and selected starter start.
- Cat room hub with bed, feeder, litter box, dream entrance, and short feedback
  lines.
- Return-to-cat-room result handoff after victory, failure, or chapter clear.

### Core Gameplay

- 10-layer route remains the core single-run structure.
- Layer 1 defense, layer 10 boss, layers 2-9 mixed nodes.
- Partner/shop/dream event/blessing/rest nodes continue to be stateful.
- New cat/partner entries must grant immediate skill value.
- Skill progression remains 2 passives / 4 small skills / 2 ultimates per cat,
  with joined-cat-only upgrades.

### Battle

- Center-bed defense, auto-targeting, movement, cat switching, skills, status
  tags, enemies, boss, and interactables remain non-negotiable.
- Bedroom and Egypt map themes should share battle mechanics but allow distinct
  wave pools, props, warnings, and visual bindings.
- P0 status tags remain sleep stable, slow, knockback, mark, and shield.

### Values And Interactions

- Owner sleep, cat HP, team poop, and team hunger stay as four core values.
- Bed care, litter box, and feeder are required in battle/rest contexts.
- Cat room versions of feeder/litter/bed should be lighter feedback, not a
  real-time punishment loop.

### UI

- Entry screen.
- Main menu.
- Character select/default starter surface.
- Cat room.
- Route map.
- Battle HUD.
- Skill/upgrade selection.
- Shop/event/blessing/rest reward surfaces.
- Pause/settings.
- Victory/defeat/route settlement.

## 6. Current Evidence

Current evidence proves the existing P0.0 implementation is real:

- `design/development/unity_batchmode/P0_OFFLINE_ACCEPTANCE_REPORT.md` reports
  offline gates passed with failure count `0`.
- `design/development/unity_batchmode/P0_PLAYMODE_ACCEPTANCE_SMOKE_REPORT.md`
  reports Play Mode smoke passed, 11 screenshots captured including
  `02-cat-room.png`, route flow cleared 10/10 nodes, boss observed, and defeat
  flow passed.
- Build settings currently cover `P0MainMenu`, `P0CatRoom`, `P0RouteMap`, and
  `P0GrayboxBattle`.
- Current runtime visual gates report 118 review assets and 111 runtime
  bindings.

This evidence does not complete the new live-source P0 boundary because Batch90
cat-room candidate import/binding, Egypt dream, and the full opening/hub loop
still need focused implementation and Unity review.

## 7. Retirement Rules

| Retire from active planning | Replacement |
|-|-|
| Blank-project assumptions from 2026-06-13 | Current runtime/scenes/tests/assets are real and should be extended. |
| Single-bedroom-only P0 as the final demo | Single-bedroom route is P0.0 core; live-source P0 adds cat room and Egypt dream. |
| Treating `IZp` as a known design source | `IZp` is blocked until access is granted and fetched. |
| Large blind asset generation | Candidate review, Unity evidence, and source-lock gates first. |
| Starter-cat body-art replacement | Blocked until active-cat screenshot review passes against colored turnarounds. |

## 8. Phase Gates

| Gate | Goal | Exit criteria |
|-|-|-|
| G0 Source lock | Make source boundary explicit | `Qr1` live data, `MDr` local copy, and `IZp` blocker recorded. |
| G1 Current architecture | Prevent old scope drift | This architecture and task graph become current entry points. |
| G2 Cat room loop | Add the missing hub | Player can start in menu, enter cat room, enter dream, resolve a run/battle, and return to cat room. |
| G3 Dream theme split | Add map-theme architecture | Bedroom and Egypt are represented as distinct dream contexts without duplicating combat rules. |
| G4 UI completion | Cover all P0 screens | Entry, main, character select, cat room, route, battle HUD, upgrade, pause/settings, and settlement are reviewable. |
| G5 Asset install review | Bind candidate non-cat UI/art only after approval | Candidate packs pass Unity screenshot/import/binding checks before runtime install. |
| G6 Full demo QA | Claim high-completion initial demo | Offline gates, Play Mode smoke, screenshot matrix, Console, and independent review are clean or explicitly deferred. |

## 9. Next Development Slice

The next code slice should be small and structural:

1. Add a cat-room scene/state contract without replacing the current route and
   battle implementation.
2. Add a `P0DreamMap` / map-theme layer in data/catalogs so current bedroom
   content can remain the default while Egypt content becomes a formal target.
3. Extend acceptance/readiness checks to flag cat room and Egypt as pending
   live-source P0 requirements.
4. Keep asset candidates review-only; use existing placeholders until a Unity
   import review approves specific UI or map assets.

Do not begin broad visual replacement before the hub and map-theme code seams
exist and pass focused tests.
