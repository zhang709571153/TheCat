# P0 Demo Execution Plan - 2026-06-24

This plan breaks the current objective into executable slices. It is scoped to
the real project state on 2026-06-24 and assumes the third Feishu document
`IZpFdIwtboEzzrx4ZFlcZLD2npe` is blocked until the user grants access.

## Stage 0 - Source Sync And Retirement

Goal: keep future work aligned with current design documents and prevent stale
planning facts from driving implementation.

| Task | Acceptance | Primary files |
|-|-|-|
| Record current Feishu access state | Two available docs mapped to local Markdown; third doc logged as permission-blocked | This file, `P0_BLOCKERS_AND_RETIREMENT_LOG_2026-06-24.md` |
| Mark stale current-fact documents | Older blueprint points to current architecture doc before any stale facts | `P0_DEVELOPMENT_BLUEPRINT.md` |
| Define current source authority | Current design, code, asset, validation sources listed in one place | `P0_CURRENT_ARCHITECTURE_2026-06-24.md` |

## Stage 1 - Architecture And Evidence Baseline

Goal: prove what already exists and identify what needs code rather than more
planning.

| Task | Acceptance | Primary files |
|-|-|-|
| Compile runtime and tests | Runtime, Editor, EditMode projects compile with known warnings only | `TheCat.Runtime.csproj`, `TheCat.Editor.csproj`, `TheCat.EditModeTests.csproj` |
| Run focused EditMode tests | Core simulation, route, input, UI presenters, visual catalog, and acceptance tests pass | `Assets/TheCat/Tests/EditMode` |
| Run golden path acceptance offline | 10-layer route clears and report has no failures | `P0GoldenPathSimulator`, `P0GoldenPathAcceptance` |
| Update evidence log | Test commands, Play Mode evidence, and remaining review gaps recorded without overstating final polish | `DEVELOPMENT_LOG.md` |

## Stage 2 - Player-Facing Loop

Goal: make the run legible and complete from player start to settlement.

| Task | Dependencies | Acceptance | Primary files |
|-|-|-|-|
| Main menu sanity | Stage 1 compile | Default three-cat start and selected-cat start both initialize run state | `MainMenuController`, `P0MainMenuPresenter`, `P0RunSession` |
| Route map progression | Main menu sanity | Player can select/confirm route options with keyboard and buttons | `RouteMapController`, `P0RouteMapPresenter`, `RunRouteState` |
| Battle start context | Route map progression | Current route node selects correct wave, modifiers, and reward context | `P0BattleStartContext`, `P0PrototypeCatalog`, `P0RouteCatalog` |
| Post-battle return | Battle start context | Victory/defeat returns to route map and records node completion exactly once | `GrayboxBattleController`, `P0SceneFlow`, `RouteNodeResolver` |
| Settlement | Post-battle return | Layer 10 Boss win or sleep failure produces clear result with metrics | `P0BattleResultPresenter`, `P0RunSettlementSummary` |

## Stage 3 - Battle Minimum Loop

Goal: make the bedroom defense loop playable, understandable, and design-faithful.

| Task | Dependencies | Acceptance | Primary files |
|-|-|-|-|
| Movement and targeting | Stage 2 battle start | Active cat moves, auto-target attempts are recorded, and nearest-enemy targeting is readable | `P0BattleNavigationState`, `P0AutoAttackTargetResolver`, `GrayboxBattleController` |
| Three-cat switching | Movement | Saiban, Nephthys, Suzune can switch; weak cats cannot be selected | `CatBattleState`, `P0CatHudPresenter`, `P0KeyboardInputMap` |
| Starter skills | Switching | Each starter cat has usable skills matching defender/controller/healer intent | `SkillDefinition`, `BattleSimulation.CastSkill`, `P0SkillHudPresenter` |
| Enemy bed pressure | Skills | Enemies spawn, move to bed, apply sleep pressure, and can be cleared | `BattleSimulation`, `BattleEnemyState`, `GrayboxEnemyView` |
| Boss pressure | Enemy pressure | Call Tyrant summon and app throw both appear in telemetry and feedback | `BattleSimulation`, `P0EnemyWarningIndicatorPresenter` |

## Stage 4 - Four Core Values And Interactions

Goal: ensure the defining pressure loop exists in real play.

| Task | Dependencies | Acceptance | Primary files |
|-|-|-|-|
| Owner sleep | Stage 3 enemy pressure | Sleep falls from bed/Boss pressure and reaching 0 fails the battle | `OwnerSleepState`, `BattleSimulation` |
| Cat HP and weak state | Enemy/player pressure | Cat damage, shield, heal, weak state, and blocked switching are visible | `CatVitalState`, `CatBattleState`, `P0CatPressureApplier` |
| Poop gauge | Runtime tick | Gauge rises, countdown/incidents occur, litter box relieves pressure | `TeamPoopGauge`, `BattleSimulation.RecordLitterBoxUse` |
| Hunger gauge | Runtime tick | Hunger drains, skills spend hunger, feeder restores and starts digestion | `TeamHungerGauge`, `BattleSimulation.RecordFeederUse` |
| Bed care | Battle loop | Bed-care interaction restores sleep at a visible hunger cost | `BattleSimulation.RecordBedCareUse`, `P0BattleActionAffordancePresenter` |

## Stage 5 - Roguelite Content

Goal: turn the deterministic route into a meaningful P0 route instead of a
sequence of labels.

| Task | Dependencies | Acceptance | Primary files |
|-|-|-|-|
| Partner node | Stage 2 route map | Reward visibly recruits or previews a partner, queues immediate next-battle support, and does not break the three-cat combat core | `RunPartnerRoster`, `RunPendingBattleModifiers`, `P0RouteRewardResolver` |
| Shop node | Wallet and blessings | At least one purchase/upgrade changes wallet or blessing state | `RunWallet`, `RunBlessingInventory`, `RouteNodeResolver` |
| Dream event | Pending modifiers | Event option can grant reward or create next-battle modifier; current soft-rain and unread-red-dot ContentIds expose different default choices and risk values | `RunPendingBattleModifiers`, `RouteRewardChoice`, `P0RouteRewardResolver` |
| Event items | Dream event choices and cat-upgrade surface | Faded fish bag, folded coupon, and blank wish tag have concrete run effects; old dream map hides future route labels by default and reveals only the next layer while held; paw stamp consumes on pending cat-upgrade offers to refresh candidates | `RunEventItemInventory`, `RunProgressionState`, `P0RouteRewardResolver`, `P0RouteMapPresenter` |
| Cat upgrades | Battle rewards and joined roster | Battle rewards grant shared cat experience; when full, route progression is gated until one joined-cat upgrade candidate is selected; paw stamp rerolls that offer once; selected upgrades now bind into starter-cat 2/4/2 passive, small-skill, and ultimate combat loadouts; route upgrade cards and upgraded battle-skill presentation now use localized player-facing copy with intent labels. Follow-up: balance plus VFX/readability review against the current Play Mode screenshots | `RunCatUpgradeState`, `P0CatUpgradeRuntimeCatalog`, `RouteBattleReward`, `RouteMapController`, `P0RouteMapPresenter`, `P0SkillPresenter`, `GrayboxBattleController` |
| Blessing offering | Blessing inventory | Authority blessing gained and reflected in battle modifiers | `P0BlessingCatalog`, `BattleModifierSet` |
| Rest nest | Core values | Rest node restores sleep/hunger/poop according to P0 design | `RunCoreValues`, `RouteNodeResolver` |
| Content-id differentiation | Dream event branch plus stateful non-battle rewards | Current DreamEvent route nodes with different `ContentId` values expose different default choices and risk/reward values; other current non-battle node types stay stateful until new distinct ContentIds exist | `P0RouteCatalog`, `P0RouteRewardResolver`, `P0RouteChoiceCoverage` |

## Stage 6 - UI And Feedback Polish

Goal: make the demo readable enough for user review before deeper art work.

| Screen | Required state |
|-|-|
| Main menu | Title, selected cats, default start, route start, quick battle, clear progress |
| Route map | Current layer, route options, current node, wallet, roster, blessings, reward choices, pending cat-upgrade choices, paw-stamp reroll |
| Battle HUD | Sleep, active cat life/shield, three cat cards, poop, hunger, skills, interactions, runtime controls, and no player-facing `HP`/`Lv`/`Target`/`hunger` debug tokens |
| Battle feedback | Skill cast, blocked target, interaction success/fail, enemy warning, result prompt, and localized feedback titles instead of raw enum names |
| Settlement | Win/fail reason, completed layers, resources, metrics, continue/restart, and localized action telemetry/result reasons |

Implementation should keep using existing presenters first. Unity evidence now
exists for the IMGUI loop, and the 2026-06-25 HUD readability pass collapses
diagnostics behind `F10` while preserving compact battle, enemy, status, cat,
skill, and interaction cues by default. The later compact-card pass removes
duplicate summary rows, folds skill tracking into the skill header, and keeps
cat, skill, interaction, and restart controls visible in the battle screenshot
first viewport. A later cat-upgrade readability pass removes developer-facing
tokens from the pending upgrade route surface, clarifies paw-stamp consumption,
and localizes upgraded battle-skill names, roles, effects, and voice lines. The
2026-06-25 UI copy readability pass removes the remaining result/HUD `HP`,
`Lv`, `Target`, `hunger`, raw enum-title, and stale disabled-action wording from
covered player-facing surfaces, with fresh EditMode and Play Mode acceptance
evidence. The later settlement-focus pass moves complete-route pages to a
settlement-first layout: result banner, compact outcome rows, and restart/menu
actions appear before route history and telemetry details. The later battle HUD
denoise pass moves diagnostics-style enemy/status count summaries behind `F10`,
puts target/battle-state content before runtime controls, weakens the default
skill tracking control, and softens range/target indicators while preserving
smoke/debug verification. The next polish pass should move the demo
presentation toward denser player-facing cards/prefabs before replacing the
presentation stack. The later warning micro-cue pass moves default enemy
warning VFX icons into a compact inline HUD row while keeping the full-size
warning visual stack in `F10` diagnostics and world-space warning feedback. The
later HUD message-filter pass hides `调试`/`诊断` footer messages from the
default player HUD while preserving them in `F10` diagnostics. The follow-up
typed-message pass replaces prefix filtering with
`P0HudMessageChannel.Player`/`P0HudMessageChannel.Diagnostics`, so future
diagnostic footer messages stay off the default HUD even when their copy does
not match old debug prefixes. The later battle command-deck pass adds a compact
`当前行动` line built from existing cat, skill, and interaction presenter data;
its initial multi-line version was retired during screenshot QA because it
pushed interaction controls below the first viewport.

## Stage 7 - Asset Review And Install Gates

Goal: use current assets safely and avoid visual drift.

| Task | Acceptance |
|-|-|
| Use installed baseline | Runtime uses the 118 review-asset / 111 binding baseline only |
| Review candidate queue | Candidate packs stay under `design/development/asset_candidates` |
| Keep cat body lock | No starter-cat body generation/import/binding without active-cat screenshot approval |
| Capture Unity evidence | Current Play Mode screenshots prove the baseline loop; future asset installs still require Console, Sprite import, scene/prefab, and screenshot review for the specific candidate |

## Stage 8 - Verification Matrix

Minimum command/evidence set before calling a slice complete:

| Gate | Evidence |
|-|-|
| Static hygiene | `git diff --check` |
| Runtime compile | Visual Studio MSBuild for `TheCat.Runtime.csproj` |
| Editor compile | Visual Studio MSBuild for `TheCat.Editor.csproj` |
| Test compile/run | Visual Studio MSBuild and Unity/EditMode tests where available |
| Golden path | `P0GoldenPathAcceptance` report accepted |
| Unity runtime | Console clean, scenes present, Play Mode screenshots captured and validated as non-blank |
| Documentation | `DEVELOPMENT_LOG.md` records what changed, what passed, and what is blocked |

## Stage 9 - Agent Review Cadence

Use agents at these decision points:

- After source sync: design-faithfulness reviewer.
- After architecture/WBS changes: systems decomposition reviewer.
- Before asset install: asset gate reviewer.
- Before claiming playable demo: QA/validation reviewer.
- After major code slice: independent code review focused on regressions and
  missing tests.
