# P0 Current Architecture - 2026-06-24

This is the current architecture control document for the TheCat / 梦境支配者
P0 demo goal. It supersedes older "current facts" in
`P0_DEVELOPMENT_BLUEPRINT.md`; that older file remains useful only as an
initial 2026-06-13 planning artifact.

## 1. Source Authority

Current design sources that are safe to use:

| Source | Status | Local authority |
|-|-|-|
| `Qr1XdXd6KosnjMxjgW7cS89kn9c` | Available from the 2026-06-13 clone | `design/梦境支配者核心玩法/docs/00_overview/p0_minimum_design.md` |
| `MDrSdEoaToB5cnxZgrOcAE34nof` | Available from the 2026-06-13 clone | `design/梦境支配者核心玩法/docs/01_core_gameplay/core_gameplay_rules_and_player_path.md` |
| `IZpFdIwtboEzzrx4ZFlcZLD2npe` | Blocked: current Feishu user lacks view permission | Do not infer content until access is granted and fetched |

The third document returned Feishu API error `3380004` on 2026-06-24:
current user lacks view or edit access. Until it is granted, it is a hard
review gap, not a design source.

## 2. Non-Negotiable P0 Intent

The first high-quality demo must preserve these design pillars:

- One complete 10-layer run: main menu, route map, combat nodes, non-combat
  reward nodes, Boss, victory or failure settlement.
- Center-bed defense in the bedroom dream: enemies pressure the bed, and owner
  sleep reaching 0 is the clean failure line.
- Three starter cats as the hard playable core: Saiban / defender, Nephthys /
  controller, Suzune / healer.
- Cat switching, auto targeting, skill release, and readable skill/state HUD.
- Four core values: owner sleep, cat HP, team poop, and team hunger.
- Litter box, feeder, and bed-care interactions as the moment-to-moment
  pressure relief loop.
- P0 status tags only: sleep stable, slow, knockback, mark, and shield.
- Authority blessings only for P0. Attribute blessings and heavy elemental
  reactions stay out of the initial demo.
- Shop upgrades strengthen existing build values; they must not rewrite skill
  mechanics.
- New partner or starter entry must have immediate skill value. Do not make a
  recruited cat wait as a normal-attack-only unit.

## 3. Current Runtime Architecture

The project is no longer at the blank skeleton stage. Current architecture is
already organized around a simulated P0 loop with IMGUI presentation surfaces.

| Layer | Current modules | Role |
|-|-|-|
| Scene flow | `P0SceneFlow`, `MainMenuController`, `RouteMapController`, `GrayboxBattleController` | Main menu, route map, graybox battle, post-battle return |
| Combat simulation | `BattleSimulation`, `BattleSimulationConfig`, `CatBattleState`, `BattleEnemyState` | Bed pressure, waves, skills, statuses, Boss summon/throw, outcome |
| Run state | `P0RunSession`, `RunProgressionState`, `RunRouteState`, `RunCoreValues`, `RunCatVitals`, `RunWallet`, `RunPendingBattleModifiers`, `RunEventItemInventory`, `RunCatUpgradeState` | Persistent single-run state between route and battle, including one-shot next-battle route effects, lightweight P0 event items, and shared cat-upgrade progress |
| Content catalogs | `P0PrototypeCatalog`, `P0RouteCatalog`, `P0BlessingCatalog`, `P0VisualAssetCatalog` | Starter cats, skills, waves, enemies, route, blessings, visuals |
| UI presenters | `P0MainMenuPresenter`, `P0RouteMapPresenter`, `P0CatHudPresenter`, `P0SkillHudPresenter`, status/enemy/result presenters | Testable UI state independent of IMGUI drawing |
| Input | `P0KeyboardInputMap`, `P0InputCommand`, `P0InputBinding` | Keyboard route, battle, runtime, skill, and interaction commands |
| Validation tools | `P0GoldenPathSimulator`, `P0GoldenPathAcceptance`, `P0PlayableReadiness`, `P0ArchitectureCompletionAudit`, Play Mode evidence tools | Offline and Unity-side acceptance gates |
| Asset gates | `P0AssetManifestCatalog`, `P0AssetProductionQueueCatalog`, `P0AssetSystematicProductionPlan` | Asset baseline, candidate queue, install prohibition rules |

Current implementation style: runtime logic is mainly code-first and
scene-light. The player-facing demo currently relies on IMGUI screens and
runtime visual catalog lookups. This is acceptable for the current demo path,
and the 2026-06-25 normal Editor Play Mode pass now provides validated
runtime screenshots for menu, route, battle HUD, active cats, Boss warning, and
settlement. A later 2026-06-25 HUD readability pass collapses diagnostics
behind `F10`, then a compact-card pass keeps cat switching, skills,
interactions, and restart reachable in the battle screenshot first viewport.
Final polish still needs stronger player-facing visual hierarchy and
scene/prefab evidence beyond the current IMGUI demo surfaces.

## 4. Demo System Frame

The target runtime frame is:

1. `P0MainMenu`: show title, selected starter cats, route preview, start
   default run or quick battle.
2. `P0RouteMap`: choose current layer option, resolve non-battle nodes, show
   wallet, roster, blessings, route progress, and settlement state.
3. `P0GrayboxBattle`: run bedroom defense battle, read current route node,
   simulate wave/Boss content, allow movement/switching/skills/interactions,
   and record node completion.
4. Return to `P0RouteMap`: resolve reward or next layer.
5. If shared cat experience is full, resolve one joined-cat upgrade candidate
   on the route map before proceeding. Paw stamp can refresh that pending
   upgrade offer once by consuming the item.
6. Layer 10 Boss: Call Tyrant battle with summon and throw behavior.
7. Settlement: route cleared or failed, with metrics and reason.

## 5. Content Boundaries

P0 playable content should be treated as:

- Starter cats: Saiban, Nephthys, Suzune.
- Optional preview partner is acceptable as a route/reward support entry, but it
  must not displace the three-cat hard core. Current Shadowmaru preview recruit
  queues next-battle support instead of becoming a fourth formal combat starter.
- Core enemies for the demo: Black Mud Nightmare, Cold Light Shadow, Call
  Tyrant Boss. Secondary enemies may exist in the catalog and route, but their
  polish can follow after the core loop is proven.
- Route: deterministic 10-layer route first; seeded/random route can follow
  after acceptance evidence is stable.
- Node rewards: placeholders are allowed only while they preserve the design
  contract and are visible in the WBS as replaceable development debt.
- Current DreamEvent route nodes now branch by `ContentId`: soft rain window is
  the lower-pressure event, while unread red-dot rain has its own stronger
  reward/risk pool.
- Current event-item support is intentionally lightweight and event-first:
  faded fish bag, folded coupon, and blank wish tag have active run effects;
  old dream map now passively reveals only the next future route layer while
  held; paw stamp now consumes on the pending cat-upgrade route-map surface to
  refresh the joined-cat upgrade candidates.
- Current cat-upgrade support now spans route and combat: battle rewards grant
  shared cat experience, a full meter blocks route progression until a joined
  cat upgrade candidate is selected, paw stamp refreshes the pending offer, and
  selected upgrades alter the next battle loadout through starter-cat
  `2 passive / 4 small / 2 ultimate` candidate pools. The pending route-map
  upgrade surface and upgraded battle-skill presentation now use localized
  player-facing copy with intent labels. Remaining debt is balance and final
  VFX/readability polish against the current Play Mode screenshots.
- Current shop, partner, rest-nest, and blessing nodes remain stateful P0 pools
  rather than separate content pools.

## 6. Current Asset Baseline

Use the currently installed, manifest-backed baseline for the demo:

- Current runtime manifest/catalog: 118 generated/import-ready review assets.
- `P0VisualAssetCatalog.P0RuntimeVisualBindingCount`: 111 runtime visual
  bindings.
- Starter cats: use the current locked combat sprites and Batch 58 HUD avatars.
- Skill feedback: use Batch 60 skill HUD feedback and Batch 61 symbolic starter
  skill VFX.

Do not install or runtime-bind candidate-only assets until Unity evidence
approves them.

Starter-cat body art hard rule: do not generate, crop, recolor, import, or
runtime-bind starter-cat body replacements until active-cat Play Mode
screenshots are approved against the locked colored three-view turnarounds.

## 7. Validation Frame

The demo is not complete until evidence proves:

- Runtime, Editor, and EditMode assemblies compile.
- EditMode tests pass for simulation, route, UI presenters, assets, gates, and
  input.
- Golden path clears 10 layers, includes required battle count, Boss behavior,
  route content, resources, interactions, status use, and telemetry.
- Unity opens without Console errors.
- Build Settings include the intended P0 scenes.
- Play Mode screenshots show menu, route map, battle HUD, active cats, enemies,
  interactions, skill VFX, Boss, and settlement.
- Chinese UI scale remains readable across required desktop resolutions.
- Candidate assets remain outside runtime until formal Unity review approves
  installation.

Current 2026-06-25 evidence satisfies the first Play Mode screenshot gate with
10 validated `1920x1080` captures and `8/8` Play Mode evidence checks. It does
not by itself prove final player-facing polish: the battle screenshots still
show a heavy debug IMGUI panel that should be reduced for the next demo pass.

## 8. Current Highest-Value Next Development

The next implementation should not rebuild architecture. It should close the
gap between existing simulation scaffolding and a reliable player-facing demo:

1. Reconcile current scenes/Build Settings with `P0SceneFlow`.
2. Keep the all-green offline acceptance gate and the new normal Editor Play
   Mode evidence gate stable while improving player-facing readability.
3. Extend route event items only through visible player surfaces: old dream map
   now has a route-preview surface and paw stamp now refreshes pending
   cat-upgrade candidates on the route map.
4. Keep partner semantics explicit: preview/support partner now grants
   immediate next-battle value, but formal combat-cat expansion remains outside
   the current P0 starter core until design and asset gates approve it.
5. Tune the now-bound starter-cat 2/4/2 upgrade pools in Play Mode before
   claiming final cat-build feel.
6. Improve Boss and secondary enemy readability only after the core route loop
   is green.
7. Keep candidate asset installs blocked until formal Unity review approves
   them; the current Play Mode screenshot set is baseline evidence, not blanket
   approval for candidate art replacement.
