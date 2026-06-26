# P0 Blockers And Retirement Log - 2026-06-24

This file records blockers, stale materials, and review gaps for the current
goal. It exists so future work can keep moving toward current design documents
instead of repeating old assumptions.

## Active Blockers

| Id | Blocker | Evidence | Required action |
|-|-|-|-|
| B-001 | Third Feishu document is not readable | `docs +fetch` on `IZpFdIwtboEzzrx4ZFlcZLD2npe` returned API code `3380004`, no view/edit permission | User or owner grants current Feishu user access; then fetch outline/full content and re-run design review |
| B-004 | Candidate asset formal installs remain blocked | Formal install packet says no candidate is approved for install | Keep candidates outside runtime until Unity review approves one install row at a time |
| B-005 | Lark CLI update is available | CLI notice: current `1.0.53`, latest `1.0.57` | Not blocking P0 work; update only in a controlled tool-maintenance pass |
| B-006 | Second Feishu document live fetch is blocked | 2026-06-25 `docs +fetch` and `drive +export` on `MDrSdEoaToB5cnxZgrOcAE34nof` returned API `3380004`, no view/edit permission | Continue using the synced 2026-06-13 local Markdown copy until access is granted; refresh after permission changes |
| B-007 | Starter-cat formal body-art replacement remains blocked by missing explicit approval notes | Current offline acceptance and Play Mode smoke are green, and active-cat screenshots are registered `3/3`; the remaining starter-cat disagreement is formal colored-turnaround comparison approval, not screenshot existence, entry, cat-room, route, or dream-map behavior | Keep starter-cat body replacement locked; replace block notes only after explicit per-cat colored-turnaround comparison approval |

## Active Visual Debts

| Id | Debt | Evidence | Required action |
|-|-|-|-|
| None | No current P0/P1 battle-world label debt after I1 | I1 refreshed `04-battle-hud-layer1.png`, `08-battle-world-visuals.png`, `09-call-tyrant-warning-vfx.png`, and `10-battle-result-layer1.png` at `2026-06-25 21:22 +08:00`; independent screenshot review found no P0/P1 blocker | Continue batch-specific visual parity and import-review evidence; do not treat I1 as candidate import approval |

## Retired Or Superseded Guidance

| File or topic | Status | Replacement |
|-|-|-|
| `P0_DEVELOPMENT_BLUEPRINT.md` current Unity facts | Superseded: it says current gameplay scripts/assets were absent, which is no longer true | `P0_CURRENT_ARCHITECTURE_2026-06-24.md` |
| `P0_CURRENT_ARCHITECTURE_2026-06-24.md` as final P0 scope | Superseded for live-source boundary: it describes the implemented single-bedroom route target, while live `Qr1` now requires cat room and Egypt dream P0 coverage | `P0_DEVELOPMENT_ARCHITECTURE_2026-06-25.md` |
| Dated 2026-06-25 architecture/task graph as the only current entry point | Superseded as sole entry: dated files remain evidence snapshots, but future work needs stable file names | `P0_DEVELOPMENT_ARCHITECTURE.md` and `P0_IMPLEMENTATION_TASK_BREAKDOWN.md` |
| Blank-project assumptions from 2026-06-13 | Retired | Current Runtime, Gameplay, Roguelite, Tools, asset catalog, and tests are authoritative |
| Single-bedroom-only P0 as final demo claim | Retired as final scope, preserved as P0.0 core implementation | Live-source P0 requires cat room hub, return-to-room feedback, and two dream themes |
| Blind new asset generation | Retired for current phase | Review existing candidate packs and gather Unity evidence first |
| Starter-cat body replacement candidates | Blocked, not retired | Keep as review-only candidates until registered active-cat screenshots receive explicit colored-turnaround comparison approval |
| Attribute blessing P0 implementation | Deferred | P0 authority blessings only; attribute blessings are second-wave content |
| Large roguelike item pool | Deferred | P0 uses small authority blessing pool and event items |

## Retired This Pass

| Item | Evidence | Replacement state |
|-|-|-|
| Earlier offline code-smoke failures before the current asset-evidence gate | Historical `P0_OFFLINE_ACCEPTANCE_REPORT.md` runs reached a green Code Smoke Suite before later cat-room/dream-map work and starter-cat evidence tightening; current broad CodeSmoke may still fail on B-007 asset-review packet evidence | Do not use this row as current acceptance status; current authority is B-007 plus `UNITY_VALIDATION_BACKLOG.md` |
| 115-asset baseline count | Current manifest/review packet reports `118` review assets and `111` runtime-bound assets | Use 118 review assets / 111 runtime bindings in current docs and gates |
| Generic Call Tyrant warning VFX coverage | Runtime warning presenter uses Boss throw/summon specific VFX | Coverage now validates the app-throw warning VFX for Boss throw |
| Partner node as roster-only placeholder | Shadowmaru preview recruit now queues next-battle support and route-choice coverage validates the support snapshot | Partner remains route preview/support, not a fourth formal combat starter |
| DreamEvent rewards collapsed by node type only | `event_soft_rain_window` and `event_unread_red_dot_rain` now resolve to different default choices and risk/reward values | Current DreamEvent content ids are differentiated; shop/partner/rest/blessing pools stay stateful until new distinct content ids exist |
| Event items absent from run state | `RunEventItemInventory` tracks the five P0 event-item ids; faded fish bag, folded coupon, blank wish tag, and old dream map now have visible run effects or surfaces | Paw stamp remains explicit follow-up work, not hidden behavior |
| Red-dot rain overdrive branch | Unread red-dot rain now uses the design-doc `dream_event_red_dot_ignore` branch: blank wish tag plus next-battle pressure | Older `dream_event_red_dot_overdrive` is retired from active resolver code |
| Old dream map as display-only item | Route map now hides future route labels by default and reveals only the next future layer while `old_dream_map` is held | Old dream map is a passive route-preview item; paw stamp is handled separately by the cat-upgrade reroll surface |
| Paw stamp as display-only item | Pending cat-upgrade route-map surface now exposes joined-cat upgrade choices, gates route progression, and consumes `paw_stamp` to refresh the current offer | Paw stamp now targets the cat-upgrade reroll surface; Play Mode evidence was restored on 2026-06-25, so remaining debt is balance plus VFX/readability |
| Cat-upgrade combat depth incomplete | `RunCatUpgradeState` now exposes starter-cat 2/4/2 candidate pools, and `P0CatUpgradeRuntimeCatalog` binds selected passives, small skills, and ultimates into `GrayboxBattleController` battle loadouts | Remaining work is balance and VFX/readability polish rather than missing core combat binding or missing Play Mode evidence |
| Cat-upgrade localized copy placeholder | `P0SkillPresenter` now presents all upgraded cat skills with Chinese player-facing names, roles, effects, and voice lines; the pending route-map cat-upgrade surface now uses intent labels, player punctuation, and explicit paw-stamp consumption copy | Remaining cat-upgrade work is balance and visual/VFX readability, not English or internal-token presentation on the main upgrade path |
| Live Unity visual/runtime evidence incomplete | Normal Editor Play Mode acceptance passed on 2026-06-26 with screenshot smoke, route-flow smoke, defeat-flow smoke, and `8/8` Play Mode evidence checks green; route flow now verifies RestNest, DreamEvent, Shop bed-patch, and cat-room return evidence | Current evidence report is `design/development/unity_batchmode/P0_PLAYMODE_ACCEPTANCE_SMOKE_REPORT.md`; this is baseline smoke evidence, while candidate import/binding/Console proof and final visual acceptance remain separate gates |
| Missing baseline Play Mode screenshots | 2026-06-26 Play Mode acceptance now generates `11/11` validated screenshots in `design/development/screenshots/p0-playmode-smoke`, including `02-cat-room.png` | Baseline screenshot files are present and usable; G1 per-surface screenshot parity review, candidate import/binding/Console evidence, and final visual polish remain pending |
| Batchmode blank screenshot evidence accepted | Batchmode generated 640x480 single-color PNGs before validation was hardened | `P0PlayModeScreenshotFileEvidence` now decodes and samples every expected PNG; normal Editor mode generated validated 1920x1080 captures |
| G1 screenshot report generation could OOM while decoding captures | First G1 Play Mode run captured screenshots but hit fatal Texture allocation OOM inside `P0PlayModeScreenshotFileEvidence.DefaultFileIsUsable()` while building report evidence | `DefaultFileIsUsable()` now destroys decoded transient `Texture2D` objects immediately; retry Play Mode acceptance passed and supersedes the failed run |
| Expanded debug battle HUD as default demo view | 2026-06-25 refreshed `03-battle-hud-layer1.png` shows diagnostics collapsed by default while smoke builders still verify full sections | Player-facing battle HUD is default; press `F10` for diagnostics and smoke/debug tools |
| Scroll-heavy default cat/skill/interaction HUD | 2026-06-25 compact-card pass shows three cat cards, three skills, three interactions, and restart in the first battle screenshot viewport | Remaining UI work is hierarchy/prefab polish, not basic first-screen reachability |
| Slow status `0.35` display assertion | Current `BattleEnemyState.MovementRateMultiplier` displays final movement rate, so Slow appears as `移动 0.65 倍` for the current P0 enemy response | Tests now verify the final displayed movement-rate multiplier instead of the raw Slow magnitude |

| Result/HUD developer-copy leaks | 2026-06-25 UI copy readability pass removes covered player-facing `HP`, ` Lv`, `| Target`, `| hunger`, raw feedback enum titles, stale `未解锁` continue wording, and action telemetry terms such as `阻止`/`索敌`/`缺失定义` | Covered settlement, battle result, HUD, cat/enemy/skill/status, route reward, blessing, and feedback title surfaces now use localized player-facing copy; remaining UI work is visual hierarchy/prefab polish |

| Route-complete page as route-log-first surface | 2026-06-25 settlement focus layout pass moves result banner, compact settlement outcome, and restart/menu actions before route history, telemetry, and resource/team details | Complete-route screen now answers result and next action first; detailed logs remain below for evidence and review |

| Default battle HUD diagnostic count wall | 2026-06-25 battle HUD denoise pass removes default `敌人 HUD：...` and `状态 HUD：...` compact count summaries from `03-battle-hud-layer1.png`, keeps them in `F10` diagnostics, and passes Play Mode acceptance with `8/8` evidence checks and `0` warnings | Default battle HUD now starts from target, warnings, battle pace, concise threat/status briefs, cat/skill/interactions, and softened skill range feedback; diagnostics remain available for smoke/debug work |

| Default battle HUD large warning icon blocks | 2026-06-25 battle warning micro-cue pass replaces the default `DrawWarningSummary()` path with `DrawPlayerWarningSummary()` and verifies `03-battle-hud-layer1.png` after Play Mode acceptance | Default HUD warning VFX icons are compact inline micro-cues; full-size warning visuals remain in `F10` diagnostics and world-space warning feedback |

| Default HUD exposed debug footer messages | 2026-06-25 HUD message-filter pass adds `P0HudMessagePresenter`, then the typed-channel follow-up replaces prefix filtering with `P0HudMessageChannel.Player`/`P0HudMessageChannel.Diagnostics`; `03-battle-hud-layer1.png` was refreshed after Play Mode acceptance | Player HUD keeps ordinary battle feedback only; diagnostic smoke/debug summaries remain visible in `F10` diagnostics even when their copy does not start with `调试` or `诊断` |

| Prefix-based HUD debug-footer filter | 2026-06-25 typed HUD message channel slice routes `GrayboxBattleController` footer writes through `SetPlayerMessage(...)` or `SetDiagnosticsMessage(...)`, with follow-up focused EditMode `7/7` and Play Mode acceptance `8/8`, `0` warnings | Retired; channel ownership is now the authoritative default-HUD gate, not localized message text |

| Scattered default battle action context | 2026-06-25 battle command-deck compact slice adds `P0BattleCommandDeckPresenter`, draws a compact `当前行动` line from existing cat/skill/interaction presenters, and verifies `03-battle-hud-layer1.png` after Play Mode acceptance | Current action context is now visible before command rows; detailed cat, skill, interaction, and restart controls remain in the first viewport |

| Multi-line battle command deck prototype | Screenshot QA showed the initial title-plus-three-lines command deck pushed interaction controls below the first viewport | Retired in the same slice; retained implementation uses one compact `BuildCompactPlayerLine()` row |

| Cat-room scene missing from runtime flow | 2026-06-25 B2/B3 adds `Assets/TheCat/Scenes/P0CatRoom.unity`, `CatRoomController`, Build Settings registration, and scene setup validation with `0` warnings | Cat-room is now a validated placeholder scene and has Play Mode screenshot evidence in `02-cat-room.png` |
| No player-facing cat-room entry | Main menu now exposes the `EnterCatRoom` action and starts `P0RunStartMode.CatRoom` while preserving direct route/battle starts | Current menu can enter cat room before dream entry; next smoke must prove the path visually |
| Battle result lacks return-to-cat-room handoff | `P0BattleResultPresenter` now exposes `ReturnCatRoom`, and `GrayboxBattleController.ReturnToCatRoom()` records `P0CatRoomSession` return state before loading `P0CatRoom` | Battle result now supports both `继续路线` and `返回猫房`; action count `2` is retired in favor of `3` |

| Bedroom/Egypt dream-map context absent from runtime model | 2026-06-25 C1/C2 adds `DreamMapDefinition`, `P0DreamMapCatalog`, route/run/battle context propagation, a `Dream Maps` readiness check, and focused Unity EditMode `78/78` pass in `Logs/p0_dream_map_context_focused_editmode_20260625.xml` | Bedroom is the playable default dream; Egypt is a known P0 placeholder context that reuses the route/combat content until C3 readiness and UI hooks are implemented |
| Egypt readiness/placeholder gate absent | 2026-06-25 C3 adds `Egypt Readiness`, route-map Egypt placeholder surface coverage, narrowed cat-room dream-entry copy, and focused Unity EditMode `85/85` pass in `Logs/p0_egypt_readiness_editmode_20260625_final.xml` | Egypt is visible as a P0 placeholder/readiness target, not a playable second map or separate combat fork |
| Main menu route/quick-battle shortcuts as player-main-path proof | 2026-06-25 D1 adds `P0MainMenuActionCategory`, makes `EnterCatRoom` the only `PlayerPrimary` CTA, demotes route/quick-battle shortcuts to graybox helpers, and validates `25/25` focused EditMode in `Logs/p0_d1_entry_character_select_editmode_20260625.xml` | Player path is entry/character selection -> cat room -> bedroom dream route; route and quick battle remain explicit development shortcuts |
| Default battle HUD lacks top player-priority brief and result actions are low in the scroll | 2026-06-25 E1 adds `P0BattlePlayerBriefPresenter`, `Battle Readability Acceptance`, top priority/action/threat brief, and result actions for continue route / return cat room / restart; focused EditMode passed `47/47` and Play Mode plan checks passed `17/17`; G1 later refreshed the screenshot baseline | Battle readability is code/readiness complete; battle world label hierarchy remains the next visual acceptance debt |
| F1 as Batch 83-90 single import gate | 2026-06-25 F1 review split the scope: Batch 83-88 now have a docs-only import-order gate, while Batch 89 skill-selection and Batch 90 cat-room remain in the broader Unity-evidence queue for later UI passes | Current F1 gate is `design/development/asset_review/F1_UI_CANDIDATE_IMPORT_ORDER_GATE_2026-06-25.md`; no candidate art is imported or accepted by this gate |
| Batch83 lacks loading/start runtime hook | 2026-06-25 H1 adds `P0LoadingStartPresenter`, loading/start coverage, readiness integration, and screenshot-smoke hook validation; review-fix EditMode passed `33/33` in `Logs/p0_h1_loading_settings_reviewfix_editmode_20260625.xml` | Batch 83 is hook-ready only. Unity-rendered loading/start screenshots, import settings, binding proof, and clean Console remain required before import review |
| Batch85 lacks full settings-screen hook | 2026-06-25 H1 adds `P0FullSettingsSurface`, semantic full-settings option rows, deep Batch 85 candidate-token scanning, and readiness coverage; review-fix EditMode passed `33/33` | Batch 85 is hook-ready only. Unity-rendered full settings screenshots, import settings, binding proof, and clean Console remain required before import review |
| Battle world label safe-area and overlay hierarchy | 2026-06-25 I1 gates world diagnostic labels behind diagnostics HUD and in-progress battle state, preserves warning shapes, hides warning visuals after battle result, fixes EditMode material-instancing warnings, adds `P0GrayboxBattleWorldDiagnosticsTests`, passes focused EditMode `19/19`, and passes Play Mode acceptance with `8/8` checks and 11 screenshots | V-001 is retired for the current baseline. Batch 87 still needs batch-specific screenshot parity, import settings, binding proof, and Console evidence before any candidate install decision |

## Current Authoritative Entry Points

| Area | Entry point |
|-|-|
| Current live-source architecture | `design/development/P0_DEVELOPMENT_ARCHITECTURE.md` |
| Current implementation task graph | `design/development/P0_IMPLEMENTATION_TASK_BREAKDOWN.md` |
| 2026-06-25 source/agent snapshot | `design/development/P0_DEVELOPMENT_ARCHITECTURE_2026-06-25.md` and `design/development/P0_AGENT_DISPATCH_AND_TASK_GRAPH_2026-06-25.md` |
| Design overview | `design/梦境支配者核心玩法/docs/00_overview/p0_minimum_design.md` |
| Core gameplay path | `design/梦境支配者核心玩法/docs/01_core_gameplay/core_gameplay_rules_and_player_path.md` |
| Implemented route/battle architecture evidence | `design/development/P0_CURRENT_ARCHITECTURE_2026-06-24.md` |
| Historical execution-plan snapshot | `design/development/P0_DEMO_EXECUTION_PLAN_2026-06-24.md` |
| Asset gate | `Assets/TheCat/Scripts/Runtime/Tools/P0AssetSystematicProductionPlan.cs` |
| Asset validation checklist | `design/development/asset_review/P0_ASSET_UNITY_VALIDATION_CHECKLIST.md` |
| Runtime validation backlog | `design/development/UNITY_VALIDATION_BACKLOG.md` |
| Development history | `design/development/DEVELOPMENT_LOG.md` |

## Review Notes From Agents

Design-faithfulness review:

- Hard target is the 10-layer route plus center-bed defense, three starter cats,
  four core values, Boss, and settlement.
- Third Feishu document must not be inferred until permission is granted.
- P0 should not expand into multi-chapter maps, full attribute blessings, or a
  large item pool before the core loop is proven.

Asset-gate review:

- The installed baseline can support the demo, but it is not final visual
  acceptance.
- Candidate packs are review-only until Unity evidence approves them.
- Starter-cat body art remains locked behind active-cat screenshot review
  against the colored three-view turnarounds.

Route implementation review:

- Runtime command flow uses the stateful `RunProgressionState` non-battle path,
  so wallet, blessings, roster, pending modifiers, core values, and route
  progress are real run state changes.
- The lightweight `ResolveCurrentNode(RunRouteState)` path still only advances
  route state. Do not use it for player-facing non-battle reward resolution.
- Current DreamEvent `ContentId` differentiation is implemented for soft rain
  window vs unread red-dot rain. Do not split shop/partner/rest/blessing into
  new content pools until the route catalog or design source introduces
  distinct ids for those node types.
- Current cat-upgrade implementation is design-faithful at the core route and
  combat-binding level: only joined cats appear, shared battle experience opens
  the offer, paw stamp refreshes that offer by consuming the item, and selected
  passives, small skills, and ultimates now affect battle loadouts through
  starter-cat 2/4/2 pools. The route-map upgrade surface and upgraded
  battle-skill presentation now have localized player-facing copy. Do not claim
  final cat-build feel until balance and VFX/readability are reviewed against
  the current Play Mode screenshots.
- Current dream-map implementation is a context layer, not a combat fork:
  bedroom is the playable default dream, Egypt is a P0 placeholder context, and
  battle waves still resolve from route node content ids. Do not add Egypt-only
  combat pools until the source docs or C3 readiness review explicitly require
  them.
