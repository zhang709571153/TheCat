# P0 Implementation Task Breakdown

Status: current stable task entry point
Last updated: 2026-06-26

This document breaks the current P0 architecture into implementation slices.
It should be updated as slices land, blockers change, or review agents retire
old assumptions.

## Current Cut Line

Build forward from the existing P0.0 bedroom route and battle implementation.
Do not replace the current route, battle, cat upgrade, reward, or validation
systems while adding the missing live-source scope.

The next code should continue structural seams first:

1. Batch 87 clean Console and human approval remain the last formal-install
   blockers for the current battle-HUD candidate lane.
2. Batch 83 loading/start now has candidate-backed four-resolution runtime
   screenshots, spinner/placeholder semantics, progress/click-target review,
   and runtime report evidence. Keep formal install blocked until the
   scene/presenter/Console report, explicit human approval, and formal runtime
   binding decision land.
3. Batch 84 result/settlement now has candidate-backed Unity runtime evidence
   at `Runtime evidence: 6/8`: four Unity screenshots, text/reward readability,
   outcome/action review, and hardened runtime-report/log gates are present.
   Keep formal install blocked until scene/presenter binding, clean Console,
   explicit human approval, and formal runtime binding decision land.
4. Batch 85 settings/pause now has candidate-backed Unity runtime evidence at
   `Runtime evidence: 6/8`: four Unity screenshots, text/key-hint readability,
   settings-control/click-target review, and hardened runtime-report/log gates
   are present. Keep formal install blocked until scene/presenter binding,
   clean Console, explicit human approval, and formal runtime binding decision
   land.
5. Batch 86 dream-route now has candidate-backed four-resolution runtime
   screenshots, text/reward replacement, node/path semantics, route-card
   density, boss-gate scale, click-target review, and runtime report evidence.
   Keep formal install blocked until the scene/presenter/Console report and
   human approval land.
6. Batch 88 now has candidate-backed four-resolution runtime screenshots,
   source-lock avatar consistency, Chinese text/density/click-target review, and
   selected/idle readability evidence. Keep formal install blocked until the
   scene/presenter/Console report and human approval land.
7. Batch 89 skill-selection now has candidate-backed four-resolution runtime
   screenshots, selected/ready/disabled/locked state proof,
   cooldown/low-resource/no-target semantics, Chinese text/value review, and
   click-target evidence. Keep formal install blocked until the
   scene/presenter/Console report and human approval land.
8. Batch 90 cat-room now has candidate-backed four-resolution runtime
   screenshots, interaction-state/text-density review, click-target/prop-scale
   review, and runtime report evidence. Keep formal install blocked until the
   scene/presenter/Console report and human approval land.
9. Route-map settlement now has a code-level `return_cat_room` action that
   records `P0CatRoomSession.RecordRouteReturn` before loading `P0CatRoom`.
   The route-flow smoke now has a settlement screenshot pause/release path, and
   both cleared and failed route smokes require final cat-room return markers.
10. Route choice effects now have a code-level readiness gate proving RestNest
    run-core/cat-HP recovery and DreamEvent next-battle modifiers.
11. Route-flow smoke now requires RestNest recovery to carry into the next
    battle HUD before route-flow evidence is accepted.
12. Route-flow smoke now forces the soft-rain DreamEvent catnip choice in the
    smoke path only, verifies the next battle config applies skill damage `x1.2`
    and poop growth `x1.5`, and uses structure-backed evidence flags instead of
    summary marker text.
13. Route-flow smoke now buys `shop_bed_patch` at the layer 5 Shop in the
    smoke path, verifies fish spending and owner-sleep restore on the route
    state, then verifies the restored sleep value carries into the next battle.
14. Normal Editor Play Mode acceptance was refreshed after the B9 Shop gate:
    `P0_PLAYMODE_ACCEPTANCE_SMOKE_REPORT.md` is passed, `8/8` evidence checks,
    `11/11` screenshots, and route-flow evidence includes RestNest, DreamEvent,
    Shop bed-patch, and cat-room return markers.
15. Starter-cat legacy missing-screenshot blocker wording is retired: active-cat
    screenshots are registered `3/3`, but formal body-art replacement remains
    locked until explicit colored-turnaround comparison approval notes replace
    the current block notes.
16. Cat-room dream theme choice is now explicit: Bedroom is the only enabled
    playable demo route, Egypt is visible as a disabled placeholder with no
    scene target, and cat-room entry preserves an active Bedroom run plus the
    selected starter roster.
17. Normal Editor Play Mode acceptance was refreshed after B12:
    `P0_PLAYMODE_ACCEPTANCE_SMOKE_REPORT.md` is passed, `8/8` evidence checks,
    `11/11` screenshots, and `02-cat-room.png` now captures the two dream
    theme rows. The first `-batchmode` visual attempt produced gray unusable
    screenshots and is superseded by the normal Editor run.
18. Cat-room dream theme choice readability has been tightened: the dream
    choice block now appears above supporting state/resource rows, actions sit
    directly beneath the choices, Bedroom is visually marked as the enabled
    demo route, and Egypt is visually marked as a disabled no-jump placeholder.
    Normal Editor Play Mode acceptance passed after this B14 presentation pass
    with `8/8` evidence checks and `11/11` validated screenshots.
19. Main-menu player-path readability has been tightened: starter cats are now
    visible as compact cards with HUD avatars and role/skill state, the
    cat-room primary CTA sits directly below selection, and route/quick-battle
    graybox helpers are collapsed by default. `StartP0Run()` default route
    behavior and all D1 action contracts remain unchanged. Normal Editor Play
    Mode acceptance passed after this B15 pass with `8/8` evidence checks and
    `11/11` validated screenshots.
20. Route-map readability has been tightened: the first viewport now prioritizes
    current node, risk/reward, primary route CTA, and secondary route controls,
    while route history and resource/team details are folded by default.
    Route-map command/action contracts remain unchanged. Normal Editor Play
    Mode acceptance passed after this B16 pass with `8/8` evidence checks and
    `11/11` validated screenshots.
21. Battle-result readability has been tightened: the non-diagnostic result
    screen now prioritizes outcome, reward, next node, route progress, and the
    primary `继续路线` CTA before folded details. Ordinary battle controls and
    debug feedback visuals no longer appear on the player-facing result screen.
    Battle-result action ids and route completion semantics remain unchanged.
    Normal Editor Play Mode acceptance passed after this B17 pass with `8/8`
    evidence checks and `11/11` validated screenshots.
22. Route-settlement readability has been tightened: the completed route screen
    now prioritizes outcome, route progress, battle record, route-state
    resources, final core, final cat life, and the primary `返回猫房` CTA before
    folded settlement details. Route settlement action ids, return semantics,
    rewards, and route completion remain unchanged. Normal Editor Play Mode
    acceptance passed after this B18 pass with `8/8` evidence checks and `11/11`
    validated screenshots.
23. Demo release/readiness is now an explicit middle state:
    `IsP0DemoReleaseReady` / `IsP0DemoVisualEvidenceReady` can pass for the
    current B18 baseline using architecture readiness, screenshot file evidence,
    strict Play Mode report-file evidence, and a valid starter-cat formal gate.
    Final Unity runtime, clean Console, human approval, formal runtime binding,
    candidate install, and starter-cat body-art replacement remain blocked.
24. Formal install blockers are now centralized in
    `P0FormalInstallGate` and
    `P0_FORMAL_INSTALL_BLOCKER_EVIDENCE_MATRIX_2026-06-26.md`: Batch 83-90
    are visible as `8` runtime-evidence `6/8` lanes, but every lane remains
    blocked by its own scene/Console and human-approval gate. Offline acceptance
    now has `7` gates and explicitly reports `Formal install allowed: no`.
25. Unity Console/log review now has a shared classifier foundation:
    `P0UnityConsoleLogClassifier` separates strict-clean logs, project-owned
    clean logs with known environment noise, unknown blocking tokens, and
    TheCat project failure tokens. This does not approve clean Console yet; it
    prevents Licensing/Unity AI/D3D/MemoryLeaks/StackAllocator noise from being
    confused with either final cleanliness or project failure.
26. Batch 83-90 Unity preflight clean-log checks now call the shared classifier
    and the duplicated per-batch `FindConsoleFailureToken` tables have been
    retired. Formal clean-Console evidence still requires `StrictClean`; known
    environment noise remains a blocker for clean-Console approval until a
    separate reviewer accepts it as non-project noise.
27. The formal-install blocker matrix is now bound to the shared strict-clean
    Unity Console classifier contract. Generated matrix/report output names the
    active classifier policy, keeps formal install blocked, and still refuses to
    treat known environment noise as clean-Console approval.
28. Batch 87 battle-HUD preflight now separates Unity evidence completeness from
    formal install approval. The lane can become `8/8` evidence-complete in a
    future pass, but `FormalInstallAllowed` remains false until clean Console,
    explicit human approval, and a formal runtime binding decision all pass.
29. Batch 87 now has a generated human-review request packet at
    `BATCH87_BATTLE_HUD_HUMAN_REVIEW_REQUEST_2026-06-26.md`. This packet is an
    evidence checklist and reviewer handoff only; it must not be counted as
    `human_review_approval.md`, clean Console evidence, or a formal runtime
    binding approval.
30. Batch 87 now has a generated formal-runtime-binding decision request packet
    at
    `BATCH87_BATTLE_HUD_FORMAL_RUNTIME_BINDING_DECISION_REQUEST_2026-06-26.md`.
    This packet scopes the candidate binding decision without adding catalog
    rows, changing the battle HUD runtime path, or approving formal install.
31. Egypt is now a minimum enterable shared-route dream from cat room:
    `enter_egypt_dream` starts an Egypt dream-map run, `EnsureEgyptDreamRun`
    preserves selected roster, and route map/battle start keep using the
    existing ten-layer route and combat content. This supersedes the old
    disabled/no-jump Egypt placeholder claim at code/readiness level. Dedicated
    normal Editor Play Mode Egypt-entry evidence now passes through
    `P0_EGYPT_ENTRY_SMOKE_REPORT.md` with 5/5 screenshots in
    `design/development/screenshots/p0-egypt-entry-smoke`, including
    `03-egypt-battle-hud-layer1.png`, `04-egypt-battle-result-layer1.png`, and
    `05-egypt-route-map-after-layer1.png`; batchmode visual capture remains a
    documented unsupported path for this smoke. This closes first-battle
    result and route-map-return evidence only. Full Egypt route traversal,
    route settlement/cat-room return under Egypt, clean Console, human
    approval, formal install, and Egypt-specific content remain future gates.

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
| B5 | Add route-settlement return-to-cat-room handoff | Complete | B2/B3 | route-map presenter/controller, coverage tools, focused tests | Gameplay + validation review |
| B6 | Add route-choice effect readiness gate | Complete | B5 plus existing route rewards | playable readiness, focused tests, docs | Gameplay + validation review |
| B7 | Add RestNest next-battle route-flow evidence | Complete | B6 | route-flow smoke, evidence checklist, focused tests, docs | Gameplay + validation review |
| B8 | Add DreamEvent catnip next-battle route-flow evidence | Complete | B6/B7 | route-flow smoke, battle modifier evidence, evidence checklist, focused tests, docs | Gameplay + validation review |
| B9 | Add Shop bed-patch next-battle route-flow evidence | Complete | B6/B7/B8 | route-flow smoke, route-map command result, evidence checklist, focused tests, docs | Gameplay + validation review |
| B10 | Refresh Play Mode acceptance after B9 | Complete | B9 | Play Mode report, screenshots, logs, docs | QA evidence review |
| B11 | Retire stale starter-cat missing-screenshot blocker wording | Complete | B10 | formal import gate, review packet, active docs, tests | Asset gate + docs review |
| B12 | Add cat-room dream theme choice and Egypt placeholder lock | Complete | B11/C3 | cat-room presenter/controller, run session, readiness, focused tests, docs | Gameplay + UI boundary review |
| B13 | Refresh Play Mode acceptance after B12 | Complete | B12 | Play Mode report, screenshots, logs, docs | QA evidence review |
| B14 | Polish cat-room dream choice readability | Complete | B13 | cat-room controller copy/layout, readiness/tests, Play Mode report/screenshots, docs | Visual readability review |
| B15 | Polish main-menu player-path readability | Complete | B14/D1 | main-menu controller layout, focused test assertions, Play Mode report/screenshots, docs | UI boundary + validation review |
| B16 | Polish route-map current-node readability | Complete | B15/B9 | route-map controller layout, surface coverage assertions, Play Mode report/screenshots, docs | Code boundary + validation review |
| B17 | Polish battle-result player readability | Complete | B16/B3 | battle-result controller layout, result focus rows, battle-result coverage assertions, Play Mode report/screenshots, docs | Code boundary + validation review |
| B18 | Polish route-settlement player readability | Complete | B17/B3 | settlement focus rows, completed-route layout, route-map coverage assertions, Play Mode report/screenshots, docs | Code boundary + validation review |
| B19 | Sync demo release/readiness gate | Complete | B18/G1 | architecture audit, visual acceptance report evidence parser, focused assertions, offline report, docs | Architecture boundary + validation review |
| B20 | Add formal-install blocker matrix | Complete | B19/F1 | asset-production readiness gate, offline acceptance runner, matrix report, docs | Architecture + asset + validation review |
| B21 | Add shared Unity Console/log classifier | Complete | B20 validation review | runtime validation plan classifier, focused assertions, offline report, docs | Validation boundary review |
| B22 | Migrate preflight Console gates to shared classifier | Complete | B21 | Batch 83-90 preflight log checks, focused assertions, offline report, docs | Validation boundary review |
| B23 | Bind formal-install reports to shared classifier contract | Complete | B22 | formal-install gate/report policy fields, matrix report, focused assertions, offline report, docs | Validation boundary review before next formal-install lane |
| B24 | Harden Batch 87 formal gate precision | Complete | B23 + Poincare/Descartes/Linnaeus review | Batch87 preflight report semantics, focused assertions, generated preflight report, offline report, docs | Validation + asset boundary review |
| B25 | Generate Batch 87 human-review request packet | Complete | B24 | Batch87 preflight report generator, request packet, focused assertions, docs | Human-review boundary review |
| B26 | Generate Batch 87 formal-runtime-binding decision request packet | Complete | B25 | Batch87 preflight report generator, binding-decision request packet, focused assertions, docs | Runtime-binding boundary review |
| B27 | Enable Egypt minimum shared-route entry | Complete through first-result / route-map-return Play Mode evidence | B12/C3 | dream map catalog, run session, cat-room action, readiness, route-map coverage, dedicated Egypt-entry Play Mode smoke, focused tests, docs | Darwin/Carson plus Meitner/Leibniz review complete; full Egypt route/cat-room return and formal gates remain future slices |
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
| J1 | Triage Batch 87 battle-HUD Unity evidence entry | Complete as candidate-only triage | I1/F1 | docs/evidence packet only | Asset gate review |
| K5 | Import Batch 83 loading/start candidates for Unity preflight only | Complete as candidate-only preflight; runtime evidence now 6/8 via M3 | H1/F1/K1 pattern | `Assets/TheCat/Art/UI/LoadingStart`, Batch83 preflight catalog/tools/tests, report | Code boundary + evidence-chain review |
| M3 | Capture Batch 83 loading/start candidate-backed runtime evidence | Complete at 6/8 evidence; formal install still blocked | K5 | Batch83 runtime evidence runner, screenshots, reviews, report, tests, ledgers | Screenshot + evidence-chain review |
| K6 | Import Batch 84 result/settlement candidates for Unity preflight only | Complete as candidate-only preflight; runtime evidence now 6/8 via M4 | B3/G1/F1/K5 pattern | `Assets/TheCat/Art/UI/ResultSettlement`, Batch84 preflight catalog/tools/tests, report | Code boundary + evidence-chain review |
| M4 | Capture Batch 84 result/settlement candidate-backed runtime evidence | Complete at 6/8 evidence; formal install still blocked | K6 | Batch84 runtime evidence runner, screenshots, reviews, report, tests, ledgers | Screenshot + evidence-chain review |
| K7 | Import Batch 85 settings/pause candidates for Unity preflight only | Complete as candidate-only preflight; runtime evidence now 6/8 via M5 | D2/H1/F1/K5 pattern | `Assets/TheCat/Art/UI/SettingsPause`, Batch85 preflight catalog/tools/tests, report | Code boundary + evidence-chain review |
| M5 | Capture Batch 85 settings/pause candidate-backed runtime evidence | Complete at 6/8 evidence; formal install still blocked | K7 | Batch85 runtime evidence runner, screenshots, reviews, report, tests, ledgers | Screenshot + evidence-chain review |
| K8 | Import Batch 86 dream-route candidates for Unity preflight only | Complete as candidate-only preflight; runtime evidence now 6/8 via L5 | F1/G1/K5 pattern | `Assets/TheCat/Art/UI/DreamRoute`, Batch86 preflight catalog/tools/tests, report | Code boundary + evidence-chain review |
| L5 | Capture Batch 86 dream-route candidate-backed runtime evidence | Complete at 6/8 evidence; formal install still blocked | K8 | Batch86 runtime evidence runner, screenshots, reviews, report, tests, ledgers | Screenshot + evidence-chain review |
| K1 | Import Batch 87 HUD candidates for Unity preflight only | Complete as candidate-only preflight; runtime evidence now 6/8 via L1 | J1 | `Assets/TheCat/Art/UI/BattleHUD`, Batch87 preflight catalog/tools/tests, reports | Code boundary + evidence-chain review |
| L1 | Capture Batch 87 battle-HUD candidate-backed runtime evidence | Complete at 6/8 evidence; formal install still blocked | K1 | Batch87 runtime evidence runner, screenshots, reports, tests, ledgers | Screenshot + evidence-chain review |
| K2 | Import Batch 88 character-select candidates for Unity preflight only | Complete as candidate-only preflight | D1/F1/K1 pattern | `Assets/TheCat/Art/UI/CharacterSelect`, Batch88 preflight catalog/tools/tests, reports | Code boundary + evidence-chain review |
| L2 | Capture Batch 88 character-select candidate-backed runtime evidence | Complete at 6/8 evidence; formal install still blocked | K2 | Batch88 runtime evidence runner, screenshots, reports, tests, ledgers | Screenshot + evidence-chain review |
| K3 | Import Batch 89 skill-selection candidates for Unity preflight only | Complete as candidate-only preflight; runtime evidence now 6/8 via L4 | D2/K1/K2 pattern | `Assets/TheCat/Art/UI/SkillSelection`, Batch89 preflight catalog/tools/tests, report | Code boundary + evidence-chain review |
| L4 | Capture Batch 89 skill-selection candidate-backed runtime evidence | Complete at 6/8 evidence; formal install still blocked | K3 | Batch89 runtime evidence runner, screenshots, reviews, report, tests, ledgers | Screenshot + evidence-chain review |
| K4 | Import Batch 90 cat-room candidates for Unity preflight only | Complete as candidate-only preflight; runtime evidence now 6/8 via L3 | B4/F1/K1/K3 pattern | `Assets/TheCat/Art/UI/CatRoom`, Batch90 preflight catalog/tools/tests, report | Code boundary + evidence-chain review |
| L3 | Capture Batch 90 cat-room candidate-backed runtime evidence | Complete at 6/8 evidence; formal install still blocked | K4 | Batch90 runtime evidence runner, screenshots, reviews, report, tests, ledgers | Screenshot + evidence-chain review |

## K5 Batch 83 Loading Start Unity Preflight Import

Goal: prove the four Batch 83 loading/start component candidates can be
imported with P0 Sprite settings under the declared Unity preflight root without
promoting them into formal runtime visual bindings.

Status: completed on 2026-06-26 as candidate-only Unity preflight. No formal
loading/start install, runtime scene/presenter binding, clean Console, or final
visual acceptance was made.

Implemented evidence:

- Added `P0LoadingStartBatch83CandidateCatalog`,
  `P0LoadingStartBatch83UnityPreflight`,
  `P0LoadingStartBatch83UnityPreflightRunner`, and
  `P0LoadingStartBatch83UnityPreflightTests`.
- Copied four Batch 83 transparent component PNGs into
  `Assets/TheCat/Art/UI/LoadingStart` for candidate-only Unity preflight.
- Unity generated four `.png.meta` files.
- `P0AssetImportSettingsValidator` validated 4 generated/imported asset(s)
  with 0 warning(s).
- Batch 83 preflight report records `Ready for Unity preflight: yes`,
  `Formal install allowed: no`, and `Runtime evidence: 6/8` after the M3
  runtime evidence pass:
  `design/development/asset_review/BATCH83_LOADING_START_UNITY_PREFLIGHT_REPORT_2026-06-26.md`.
- Focused Unity EditMode passed `15/15`:
  `Logs/batch83_loading_start_preflight_editmode_20260626_03.xml`.

Boundary:

- Batch 83 candidate ids do not enter `P0VisualAssetCatalog` formal runtime
  bindings.
- Unity-rendered Batch 83 loading/start screenshots, spinner/placeholder
  interpretation, 1024x768 density, and progress/click-target proof are now
  covered by M3 runtime evidence.
- Scene/presenter binding, clean Console, explicit human approval, and the
  formal runtime binding decision remain open.

## M3 Batch 83 Loading Start Runtime Evidence

Goal: capture candidate-backed loading/start runtime screenshots and automatic
reviews without promoting Batch 83 into formal runtime bindings.

Status: completed on 2026-06-26 at `Runtime evidence: 6/8`. Formal install
remains blocked.

Implemented evidence:

- Added `P0LoadingStartBatch83RuntimeEvidence` and
  `P0LoadingStartBatch83RuntimeEvidenceBatchmodeRunner`.
- Captured four candidate-backed Unity screenshots under
  `design/development/screenshots/batch_83_loading_start_unity_preflight/`.
- Wrote the runtime evidence report:
  `design/development/asset_review/batch_83_loading_start_unity_preflight/runtime_evidence_report.md`.
- Wrote automatic spinner/placeholder and progress/click-target reviews:
  `design/development/asset_review/batch_83_loading_start_unity_preflight/spinner_placeholder_text_density_review.md`
  and
  `design/development/asset_review/batch_83_loading_start_unity_preflight/progress_crowding_click_target_review.md`.
- Refreshed the Batch 83 Unity preflight report to `Runtime evidence: 6/8`.
- Focused Unity EditMode passed `15/15`:
  `Logs/batch83_loading_start_preflight_editmode_20260626_03.xml`.

Boundary:

- M3 does not auto-generate `scene_binding_console_clean_report.md` or
  `human_review_approval.md`.
- The runtime evidence log contains Unity AI/licensing noise, so the clean
  Console gate remains blocked until a separate clean scene/presenter/Console
  report is captured.

## K6 Batch 84 Result Settlement Unity Preflight Import

Goal: prove the seven Batch 84 result/settlement component candidates can be
imported with P0 Sprite settings under the declared Unity preflight root without
promoting them into formal runtime visual bindings.

Status: completed on 2026-06-26 as candidate-only Unity preflight. No formal
result/settlement install, runtime scene/presenter binding, clean Console, or
final visual acceptance was made.

Implemented evidence:

- Added `P0ResultSettlementBatch84CandidateCatalog`,
  `P0ResultSettlementBatch84UnityPreflight`,
  `P0ResultSettlementBatch84UnityPreflightRunner`, and
  `P0ResultSettlementBatch84UnityPreflightTests`.
- Copied seven Batch 84 transparent component PNGs into
  `Assets/TheCat/Art/UI/ResultSettlement` for candidate-only Unity preflight.
- Unity generated seven `.png.meta` files.
- `P0AssetImportSettingsValidator` validated 7 generated/imported asset(s)
  with 0 warning(s).
- Batch 84 preflight report records `Ready for Unity preflight: yes`,
  `Formal install allowed: no`, and `Runtime evidence: 6/8` after the M4
  runtime evidence pass:
  `design/development/asset_review/BATCH84_RESULT_SETTLEMENT_UNITY_PREFLIGHT_REPORT_2026-06-26.md`.
- Focused Unity EditMode passed `15/15` after runtime evidence gate coverage:
  `Logs/batch84_result_settlement_preflight_editmode_20260626_02.xml`.

Boundary:

- Batch 84 candidate ids do not enter `P0VisualAssetCatalog` formal runtime
  bindings.
- Unity-rendered battle-victory, battle-defeat, run-cleared, and run-failed
  screenshots, text/reward readability, and outcome/action review are now
  covered by M4 runtime evidence.
- Scene/presenter binding, clean Console, explicit human approval, and formal
  runtime binding decision remain open.

## M4 Batch 84 Result Settlement Runtime Evidence

Goal: capture candidate-backed result/settlement runtime screenshots and
automatic reviews without promoting Batch 84 into formal runtime bindings.

Status: completed on 2026-06-26 at `Runtime evidence: 6/8`. Formal install
remains blocked.

Implemented evidence:

- Added `P0ResultSettlementBatch84RuntimeEvidence` and
  `P0ResultSettlementBatch84RuntimeEvidenceBatchmodeRunner`.
- Captured four candidate-backed Unity screenshots under
  `design/development/screenshots/batch_84_result_settlement_unity_preflight/`.
- Wrote the runtime evidence report:
  `design/development/asset_review/batch_84_result_settlement_unity_preflight/runtime_evidence_report.md`.
- Wrote automatic text/reward readability and outcome/action click-target
  reviews under
  `design/development/asset_review/batch_84_result_settlement_unity_preflight/`.
- Refreshed the Batch 84 Unity preflight report to `Runtime evidence: 6/8`.
- Focused Unity EditMode passed `15/15`:
  `Logs/batch84_result_settlement_preflight_editmode_20260626_02.xml`.

Boundary:

- M4 does not auto-generate `scene_binding_console_clean_report.md` or
  `human_review_approval.md`.
- The runtime/preflight/test logs include Unity licensing, Unity AI, and Curl
  noise, so the clean Console gate remains blocked until a separate clean
  scene/presenter/Console report is captured.

## K7 Batch 85 Settings Pause Unity Preflight Import

Goal: prove the six Batch 85 settings/pause component candidates can be
imported with P0 Sprite settings under the declared Unity preflight root without
promoting them into formal runtime visual bindings.

Status: completed on 2026-06-26 as candidate-only Unity preflight. No formal
settings/pause install, runtime scene/presenter binding, clean Console, or final
visual acceptance was made.

Implemented evidence:

- Added `P0SettingsPauseBatch85CandidateCatalog`,
  `P0SettingsPauseBatch85UnityPreflight`,
  `P0SettingsPauseBatch85UnityPreflightRunner`, and
  `P0SettingsPauseBatch85UnityPreflightTests`.
- Copied six Batch 85 transparent component PNGs into
  `Assets/TheCat/Art/UI/SettingsPause` for candidate-only Unity preflight.
- Unity generated six `.png.meta` files.
- `P0AssetImportSettingsValidator` validated 6 generated/imported asset(s)
  with 0 warning(s).
- Batch 85 preflight report originally recorded `Ready for Unity preflight:
  yes`, `Formal install allowed: no`, and `Runtime evidence: 0/8`; M5 later
  advanced the same report to `Runtime evidence: 6/8`:
  `design/development/asset_review/BATCH85_SETTINGS_PAUSE_UNITY_PREFLIGHT_REPORT_2026-06-26.md`.
- Focused Unity EditMode for K7 passed `8/8`:
  `Logs/batch85_settings_pause_preflight_editmode_20260626_m1.xml`.

Boundary:

- Batch 85 candidate ids do not enter `P0VisualAssetCatalog` formal runtime
  bindings.
- M5 now covers Unity-rendered settings-main, settings-audio, pause-overlay,
  and compact settings screenshots, text/key-hint readability, and
  settings-control/click-target review. Scene/presenter binding, clean Console,
  explicit human approval, and formal runtime binding decision remain open.

## M5 Batch 85 Settings Pause Runtime Evidence

Goal: prove the six Batch 85 settings/pause component candidates render in
Unity across settings main, settings audio, pause overlay, and compact settings
states without promoting them into formal runtime bindings.

Status: completed on 2026-06-26 at `Runtime evidence: 6/8`. Formal install
remains blocked by scene/presenter binding, clean Console, explicit human
approval, and formal runtime binding decision.

Implemented evidence:

- Added `P0SettingsPauseBatch85RuntimeEvidence` and
  `P0SettingsPauseBatch85RuntimeEvidenceBatchmodeRunner`.
- Hardened `P0SettingsPauseBatch85UnityPreflight` so screenshot evidence must
  be nonblank and confirmed by `batch_85_settings_pause_unity_preflight/runtime_evidence_report.md`.
- Captured four candidate-backed Unity screenshots:
  settings main `1920x1080`, settings audio `1365x768`, pause overlay
  `1280x720`, and compact settings `1024x768`.
- Runtime report records 6/6 candidate frame draws, fallback=0,
  settings main/audio state proof, pause overlay proof, key-hint readability,
  and settings/pause click-target proof.
- Focused Unity EditMode passed `15/15`:
  `Logs/batch85_settings_pause_preflight_editmode_20260626_04.xml`.

Boundary:

- Batch 85 candidate ids still do not enter `P0VisualAssetCatalog` formal
  runtime bindings.
- Unity logs still contain licensing, Unity AI/MCP, and Curl noise, so no clean
  Console report was generated.

## K8 Batch 86 Dream Route Unity Preflight Import

Goal: prove the six Batch 86 dream-route component candidates can be imported
with P0 Sprite settings under the declared Unity preflight root without
promoting them into formal runtime visual bindings.

Status: completed on 2026-06-26 as candidate-only Unity preflight. No formal
dream-route install, runtime scene/presenter binding, clean Console, or final
visual acceptance was made.

Implemented evidence:

- Added `P0DreamRouteBatch86CandidateCatalog`,
  `P0DreamRouteBatch86UnityPreflight`,
  `P0DreamRouteBatch86UnityPreflightRunner`, and
  `P0DreamRouteBatch86UnityPreflightTests`.
- Copied six Batch 86 transparent component PNGs into
  `Assets/TheCat/Art/UI/DreamRoute` for candidate-only Unity preflight.
- Unity generated six `.png.meta` files.
- `P0AssetImportSettingsValidator` validated 6 generated/imported asset(s)
  with 0 warning(s).
- Batch 86 preflight report records `Ready for Unity preflight: yes`,
  `Formal install allowed: no`, and now `Runtime evidence: 6/8` after L5:
  `design/development/asset_review/BATCH86_DREAM_ROUTE_UNITY_PREFLIGHT_REPORT_2026-06-26.md`.
- Focused Unity EditMode passed `15/15` after runtime evidence gate coverage:
  `Logs/batch86_dream_route_preflight_editmode_20260626_l7.xml`.

Boundary:

- Batch 86 candidate ids do not enter `P0VisualAssetCatalog` formal runtime
  bindings.
- Unity-rendered dream-entry and route-map screenshots, Chinese text/route
  label/reward replacement, node/path semantics, 1024x768 route-card density,
  boss-gate scale, and route-card click-target proof are now evidenced by L5.
- Scene/presenter binding, clean Console, and human approval remain open.

## L5 Batch 86 Dream Route Runtime Evidence

Goal: prove the imported Batch 86 dream-route component candidates can render
in Unity across the four target resolutions with route-state semantics,
text/reward replacement, node/path semantics, boss-gate scale, and click-target
evidence, without creating clean Console or human approval evidence
automatically.

Status: completed on 2026-06-26 at `Runtime evidence: 6/8`. Formal install
remains blocked by `scene_binding_console_clean_report.md` and
`human_review_approval.md`.

Implemented evidence:

- Added `P0DreamRouteBatch86RuntimeEvidence` and
  `P0DreamRouteBatch86RuntimeEvidenceBatchmodeRunner`.
- Captured four candidate-backed screenshots under
  `design/development/screenshots/batch_86_dream_route_unity_preflight/`.
- Wrote automatic review docs for text/reward replacement, node/path
  semantics, route-card density, boss-gate scale, and route-choice click
  targets under
  `design/development/asset_review/batch_86_dream_route_unity_preflight/`.
- Wrote `runtime_evidence_report.md` confirming `6/6` candidate frame draws,
  fallback-free candidate rendering, route-state semantics, boss-gate scale,
  Chinese route labels/rewards, and click-target evidence.
- Hardened preflight tests so screenshot evidence requires the runtime report
  and scene/Console evidence requires a passing runtime-evidence log.
- Focused Unity EditMode passed `15/15`:
  `Logs/batch86_dream_route_preflight_editmode_20260626_l7.xml`.

Boundary:

- Batch 86 still does not enter `P0VisualAssetCatalog` formal runtime bindings.
- The runner does not generate `scene_binding_console_clean_report.md` or
  `human_review_approval.md`.
- Current Unity logs include licensing/Unity-service noise, so the clean
  Console gate remains intentionally blocked.

## L4 Batch 89 Skill Selection Runtime Evidence

Goal: prove the imported Batch 89 skill-selection component candidates can
render in Unity across the four target resolutions with selected/ready/
disabled/locked states, cooldown/low-resource/no-target semantics, Chinese
labels/values, and click-target evidence, without creating clean Console or
human approval evidence automatically.

Status: completed on 2026-06-26 at `Runtime evidence: 6/8`. Formal install
remains blocked by `scene_binding_console_clean_report.md` and
`human_review_approval.md`.

Implemented evidence:

- Added `P0SkillSelectionBatch89RuntimeEvidence` and
  `P0SkillSelectionBatch89RuntimeEvidenceBatchmodeRunner`.
- Captured four candidate-backed screenshots under
  `design/development/screenshots/batch_89_skill_selection_unity_preflight/`.
- Wrote automatic review docs for state semantics/text density and
  cooldown/low-resource/click-target evidence under
  `design/development/asset_review/batch_89_skill_selection_unity_preflight/`.
- Wrote `runtime_evidence_report.md` confirming `8/8` candidate frame draws,
  fallback-free candidate rendering, selected/ready/disabled/locked states,
  cooldown/low-resource/no-target semantics, Chinese labels/values, and
  click-target evidence.
- Hardened preflight tests so screenshot evidence requires the runtime report
  and scene/Console evidence requires a passing runtime-evidence log.
- Focused Unity EditMode passed `19/19`:
  `Logs/batch89_skill_selection_preflight_editmode_20260626_l8.xml`.

Boundary:

- Batch 89 still does not enter `P0VisualAssetCatalog` formal runtime bindings.
- The runner does not generate `scene_binding_console_clean_report.md` or
  `human_review_approval.md`.
- Current Unity logs still include environment/licensing noise, so the clean
  Console gate remains intentionally blocked.

## L3 Batch 90 Cat Room Runtime Evidence

Goal: prove the imported Batch 90 cat-room component candidates can render in
Unity across the four target resolutions with interaction-state semantics and
click-target/prop-scale evidence, without creating clean Console or human
approval evidence automatically.

Status: completed on 2026-06-26 at `Runtime evidence: 6/8`. Formal install
remains blocked by `scene_binding_console_clean_report.md` and
`human_review_approval.md`.

Implemented evidence:

- Added `P0CatRoomBatch90RuntimeEvidence` and
  `P0CatRoomBatch90RuntimeEvidenceBatchmodeRunner`.
- Captured four candidate-backed screenshots under
  `design/development/screenshots/batch_90_cat_room_unity_preflight/`.
- Wrote automatic review docs for interaction/text density and click-target
  prop scale under
  `design/development/asset_review/batch_90_cat_room_unity_preflight/`.
- Wrote `runtime_evidence_report.md` confirming `6/6` candidate frame draws,
  `fallback=0`, bed/feeder/litter/dream entrance states, and
  hover/disabled/blocked/range states.
- Hardened preflight tests so screenshot evidence requires the runtime report
  and scene/Console evidence requires a passing runtime-evidence log.
- Focused Unity EditMode passed `15/15`:
  `Logs/p0_batch90_cat_room_preflight_editmode_20260626_l4.xml`.

Boundary:

- Batch 90 still does not enter `P0VisualAssetCatalog` formal runtime bindings.
- The runner does not generate `scene_binding_console_clean_report.md` or
  `human_review_approval.md`.
- Current Unity logs contain licensing and Unity AI entitlement noise, so the
  clean Console gate remains intentionally blocked.

## K4 Batch 90 Cat Room Unity Preflight Import

Goal: prove the six Batch 90 cat-room component candidates can be imported
with P0 Sprite settings under the declared Unity preflight root without
promoting them into formal runtime visual bindings.

Status: completed on 2026-06-26 as candidate-only Unity preflight. No formal
cat-room install, runtime scene/presenter binding, clean Console, or final
visual acceptance was made.

Implemented evidence:

- Added `P0CatRoomBatch90CandidateCatalog`,
  `P0CatRoomBatch90UnityPreflight`,
  `P0CatRoomBatch90UnityPreflightRunner`, and
  `P0CatRoomBatch90UnityPreflightTests`.
- Copied six Batch 90 transparent component PNGs into
  `Assets/TheCat/Art/UI/CatRoom` for candidate-only Unity preflight.
- Unity generated six `.png.meta` files.
- `P0AssetImportSettingsValidator` validated 6 generated/imported asset(s)
  with 0 warning(s).
- Batch 90 preflight report originally recorded `Ready for Unity preflight:
  yes`, `Formal install allowed: no`, and `Runtime evidence: 0/8`; L3 later
  advanced the same report to `Runtime evidence: 6/8`:
  `design/development/asset_review/BATCH90_CAT_ROOM_UNITY_PREFLIGHT_REPORT_2026-06-26.md`.
- Focused Unity EditMode passed `11/11`:
  `Logs/p0_batch90_cat_room_preflight_editmode_20260626_k4.xml`.

Boundary:

- Batch 90 candidate ids do not enter `P0VisualAssetCatalog` formal runtime
  bindings.
- Existing `02-cat-room.png` remains only runtime-surface reachability proof;
  it does not satisfy Batch 90 candidate-backed screenshot, binding, or
  interaction-state acceptance.
- L3 now covers Unity-rendered Batch 90 cat-room screenshots,
  bed/feeder/litter/dream entrance state proof, hover/disabled/range semantics,
  Chinese text/value replacement, click-target proof, and prop-scale proof.
  Scene/presenter binding, clean Console, and human approval remain open.
- Current Unity logs contain licensing and Unity AI entitlement noise, so no
  clean Console report was created.

## K3 Batch 89 Skill Selection Unity Preflight Import

Goal: prove the eight Batch 89 skill-selection component candidates can be
imported with P0 Sprite settings under the declared Unity preflight root without
promoting them into formal runtime visual bindings.

Status: completed on 2026-06-26 as candidate-only Unity preflight. No formal
skill-selection install, runtime scene/presenter binding, clean Console, or
final visual acceptance was made.

Implemented evidence:

- Added `P0SkillSelectionBatch89CandidateCatalog`,
  `P0SkillSelectionBatch89UnityPreflight`,
  `P0SkillSelectionBatch89UnityPreflightRunner`, and
  `P0SkillSelectionBatch89UnityPreflightTests`.
- Copied eight Batch 89 transparent component PNGs into
  `Assets/TheCat/Art/UI/SkillSelection` for candidate-only Unity preflight.
- Unity generated eight `.png.meta` files.
- `P0AssetImportSettingsValidator` validated 8 generated/imported asset(s)
  with 0 warning(s).
- Batch 89 preflight report originally recorded `Ready for Unity preflight:
  yes`, `Formal install allowed: no`, and `Runtime evidence: 0/8`; L4 later
  advanced the same report to `Runtime evidence: 6/8`:
  `design/development/asset_review/BATCH89_SKILL_SELECTION_UNITY_PREFLIGHT_REPORT_2026-06-26.md`.
- Focused Unity EditMode passed `11/11`:
  `Logs/p0_batch89_skill_selection_preflight_editmode_20260626_k3_gatefix.xml`.

Boundary:

- Batch 89 candidate ids do not enter `P0VisualAssetCatalog` formal runtime
  bindings.
- L4 now covers Unity-rendered skill-selection screenshots,
  selected/ready/disabled/locked state proof, cooldown/low-resource/no-target
  semantics, Chinese text/number replacement, and click-target proof.
  Scene/presenter binding, clean Console, and human approval remain open.
- Current Unity logs contain licensing and CDN/Curl noise, including
  `Curl error 42`, so no clean Console report was created.

## K2 Batch 88 Character Select Unity Preflight Import

Goal: prove the six Batch 88 character-select component candidates can be
imported with P0 Sprite settings under the declared Unity preflight root without
promoting them into formal runtime visual bindings.

Status: completed on 2026-06-25 as candidate-only Unity preflight. No formal
character-select install, runtime scene/presenter binding, clean Console, or
final visual acceptance was made.

Implemented evidence:

- Added `P0CharacterSelectBatch88CandidateCatalog`,
  `P0CharacterSelectBatch88UnityPreflight`,
  `P0CharacterSelectBatch88UnityPreflightRunner`, and
  `P0CharacterSelectBatch88UnityPreflightTests`.
- Copied six Batch 88 transparent component PNGs into
  `Assets/TheCat/Art/UI/CharacterSelect` for candidate-only Unity preflight.
- Unity generated six `.png.meta` files and `P0AssetImportSettingsApplier`
  applied P0 import settings to 6 importer(s).
- `P0AssetImportSettingsValidator` validated 6 generated/imported asset(s)
  with 0 warning(s).
- Focused Unity EditMode passed `10/10` after independent review hardening:
  `Logs/p0_batch88_character_select_preflight_editmode_20260625_hardened2.xml`.
- Preflight report:
  `design/development/asset_review/BATCH88_CHARACTER_SELECT_UNITY_PREFLIGHT_REPORT_2026-06-25.md`.
- Cicero/Tesla review findings were fixed: blank/placeholder screenshots,
  generic `PASS` review markdown, and CSV omission of the formal runtime
  scene/presenter binding gate no longer allow or imply install readiness.

Boundary:

- Batch 88 candidate ids do not enter `P0VisualAssetCatalog` formal runtime
  bindings.
- Current playable character-select remains the IMGUI main-menu path until the
  four-resolution screenshot, source-lock avatar consistency, Chinese
  name/role/description/start-label, low-height density, click-target, clean
  Console, runtime scene/presenter binding, and human review gates pass.
- K2 retires the Batch 88 Sprite import-settings and candidate preflight binding
  blockers only; it did not retire runtime evidence or install blockers at K2
  close.
- Later L2 evidence retires the four-resolution screenshot, candidate-drawn
  no-fallback report, source-lock avatar consistency, Chinese text/density, and
  card/start click-target evidence blockers. Scene/presenter/Console proof and
  human approval remain open.

## L2 Batch 88 Character Select Runtime Evidence Slice

Goal: prove the imported Batch 88 character-select candidates can render in the
main-menu character-select surface across target resolutions without fallback
textures, while keeping the formal runtime catalog untouched.

Status: completed on 2026-06-26 as candidate-backed runtime evidence. The
preflight report now records `Runtime evidence: 6/8`; no formal character-select
install or runtime scene/presenter binding was approved.

Implemented evidence:

- Added `P0CharacterSelectBatch88RuntimeEvidence` and
  `P0CharacterSelectBatch88RuntimeEvidenceBatchmodeRunner`.
- Captured candidate-backed screenshots at `1920x1080`, `1365x768`,
  `1280x720`, and `1024x768` under
  `design/development/screenshots/batch_88_character_select_unity_preflight/`.
- Wrote runtime evidence:
  `design/development/asset_review/batch_88_character_select_unity_preflight/runtime_evidence_report.md`.
- Wrote automatic source-lock and density/click-target reviews under
  `design/development/asset_review/batch_88_character_select_unity_preflight/`.
- The runner suppresses the legacy IMGUI main-menu overlay during capture after
  the first visual check showed it covering the candidate surface.
- Hardened the scene/presenter/Console clean-report gate so a markdown file
  must reference an existing Batch 88 runtime evidence log, that log must
  include the Batch 88 runtime pass token, and known Console/failure tokens
  reject clean acceptance.
- Wrote
  `design/development/asset_review/BATCH88_CHARACTER_SELECT_UNITY_PREFLIGHT_CONSOLE_BLOCKERS_2026-06-26.md`
  instead of creating a false clean report while the current Unity logs still
  contain licensing, Unity AI Assistant tracing, and network callback noise.
- Focused Unity EditMode passed `13/13`:
  `Logs/p0_batch88_character_select_preflight_editmode_20260625_runtime_evidence_l3.xml`.
- Focused Unity EditMode passed `15/15` after clean-report hardening:
  `Logs/p0_batch88_character_select_preflight_editmode_20260626_console_gate_hardening_noquit.xml`.

Boundary:

- Batch 88 candidate ids remain outside `P0VisualAssetCatalog` formal runtime
  bindings.
- `scene_binding_console_clean_report.md` and `human_review_approval.md` remain
  absent and are the remaining formal-install blockers.

## K1 Batch 87 Battle HUD Unity Preflight Import

Goal: prove the six Batch 87 battle HUD component candidates can be imported
with P0 Sprite settings under the declared Unity preflight root without
promoting them into formal runtime visual bindings.

Status: completed on 2026-06-25 as candidate-only Unity preflight. No formal
Battle HUD install, runtime binding, clean Console, or final visual acceptance
was made.

Implemented evidence:

- Added `P0BattleHudBatch87CandidateCatalog`,
  `P0BattleHudBatch87UnityPreflight`,
  `P0BattleHudBatch87UnityPreflightRunner`, and
  `P0BattleHudBatch87UnityPreflightTests`.
- Copied six Batch 87 transparent component PNGs into
  `Assets/TheCat/Art/UI/BattleHUD` for candidate-only Unity preflight.
- Unity generated six `.png.meta` files and `P0AssetImportSettingsApplier`
  applied P0 import settings to 6 importer(s).
- `P0AssetImportSettingsValidator` validated 6 generated/imported asset(s)
  with 0 warning(s).
- Focused Unity EditMode passed `7/7`:
  `Logs/p0_batch87_battle_hud_preflight_editmode_20260625_k1_fix.xml`.
- Preflight report:
  `design/development/asset_review/BATCH87_BATTLE_HUD_UNITY_PREFLIGHT_REPORT_2026-06-25.md`.
- Console blocker note:
  `design/development/asset_review/BATCH87_BATTLE_HUD_UNITY_PREFLIGHT_CONSOLE_BLOCKERS_2026-06-25.md`.
- Independent review findings fixed before final K1 validation:
  editor import validation now gates preflight readiness, formal install is
  recalculated after all checks, and markdown review/approval evidence cannot
  pass with empty or stale placeholder files.

Boundary:

- Batch 87 candidate ids do not enter `P0VisualAssetCatalog` formal runtime
  bindings.
- Current playable Battle HUD remains the IMGUI/runtime path until the
  screenshot, text/value, skill-state, telegraph, click-target, clean Console,
  and human review gates pass.
- K1 retires the "Sprite import settings missing" part of the Batch 87 preflight
  lane only; it did not retire runtime evidence or install blockers at K1
  close. Later L1 evidence retires the screenshot, candidate-draw, text/value,
  skill-state, telegraph, 1024x768 four-gauge, and click-target evidence
  blockers. Clean Console and human approval remain open.

## L1 Batch 87 Battle HUD Runtime Evidence

Goal: prove the imported Batch 87 battle-HUD candidates render in the battle
HUD surface across target resolutions without fallback textures, while keeping
formal runtime catalog bindings untouched.

Status: completed on 2026-06-25 at `Runtime evidence: 6/8`. Formal install
remains blocked by clean Console evidence and explicit human approval.

Implemented evidence:

- Added `P0BattleHudBatch87RuntimeEvidence` and
  `P0BattleHudBatch87RuntimeEvidenceBatchmodeRunner`.
- Captured candidate-backed screenshots at `1920x1080`, `1365x768`,
  `1280x720`, and `1024x768` under
  `design/development/screenshots/batch_87_battle_hud_unity_preflight/`.
- Wrote runtime evidence:
  `design/development/asset_review/BATCH87_BATTLE_HUD_RUNTIME_EVIDENCE_REPORT_2026-06-25.md`.
- Wrote automatic text/skill-state and telegraph/occlusion/click-target reviews
  under `design/development/asset_review/batch_87_battle_hud_unity_preflight/`.
- Batch 87 runtime evidence confirms 6/6 candidate frame draws, fallback=0,
  gauge/text replacement, skill-state readability, 1024x768 four-gauge proof,
  enemy/telegraph occlusion, and click targets.
- Focused EditMode evidence includes the K1 import test pass, the L1/M1
  runtime-evidence guards, and M2 gate hardening:
  `Logs/p0_batch87_battle_hud_preflight_editmode_20260625_k1_fix.xml`,
  `Logs/p0_batch87_battle_hud_preflight_editmode_20260625_l1_reviewfix2_rerun.xml`,
  `Logs/p0_batch87_battle_hud_preflight_editmode_20260625_m1_layout_guard.xml`,
  and
  `Logs/p0_batch87_battle_hud_preflight_editmode_20260625_m2_gate_hardening_noquit.xml`.

Boundary:

- Batch 87 candidate ids remain outside `P0VisualAssetCatalog` formal runtime
  bindings.
- `console_clean_report.md` and `human_review_approval.md` remain absent and
  are the remaining formal-install blockers.

## J1 Batch 87 Battle HUD Evidence Triage

Goal: open the Batch 87 battle-HUD Unity validation lane without confusing the
current I1 runtime baseline with Batch 87 candidate-backed proof.

Status: completed on 2026-06-25 as candidate-only evidence triage. No import,
binding, or formal install decision was made.

Implemented evidence:

- Added
  `design/development/asset_review/BATCH87_BATTLE_HUD_UNITY_EVIDENCE_TRIAGE_2026-06-25.md`.
- Re-ran `validate_ui_battle_hud_preflight_candidates.ps1`; it passed with
  `10` manifest rows.
- Confirmed no `.meta` files exist under the Batch 87 candidate packet.
- Confirmed the local `1024x768` dense mockup is local four-gauge proof only.
- Confirmed current `04-battle-hud-layer1.png` and
  `09-call-tyrant-warning-vfx.png` are clean I1 baseline screenshots, not
  candidate-backed Batch 87 screenshots.

Boundary:

- This did not approve Batch 87 candidate import or final art acceptance at J1
  close.
- Later K1/L1 evidence retired the Sprite import-settings,
  candidate-backed screenshot, click-target, and enemy/telegraph occlusion
  blockers. Clean Console evidence and human approval remain open.

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

- This did not approve Batch 87 candidate import or final art acceptance at I1
  close.
- Later K1/L1 evidence retired the Batch 87 screenshot parity, import-settings,
  and click-target/telegraph evidence blockers. Clean Console evidence and
  explicit human approval remain open.

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

## Route Settlement Return Hardening: B5

Goal: close the route-map final settlement return path so a completed or failed
route can be intentionally returned to the cat room, using the existing
`P0CatRoomSession` contract instead of adding a parallel settlement system.

Status: completed at code and MSBuild level on 2026-06-26. Unity EditMode runner
attempts compiled/refreshed the project but did not emit a test-results XML, so
that runner output is recorded as a validation gap rather than a pass.

Implemented evidence:

- Added `P0RouteMapActionIds.ReturnCatRoom` and settlement-only route-map
  action metadata targeting `P0SceneFlow.CatRoomSceneName`.
- Updated `RouteMapController` settlement UI so the route-complete primary
  action calls `P0CatRoomSession.RecordRouteReturn` and loads `P0CatRoom`.
- Updated route-map command routing so `ContinueRoute` on a completed route
  requests the same cat-room return handoff as the settlement button.
- Hardened `P0RouteMapSurfaceCoverage` cleared and failed settlement checks to
  require the enabled cat-room return action.
- Added `P0PlayableReadiness.RouteSettlementReturnCheckId` so the overall P0
  readiness gate covers the completed-route settlement -> cat-room handoff.
- Extended `P0PlayModeRouteFlowSmoke` so a passing route-flow smoke must verify
  final settlement -> cat-room return and include `cat-room return verified` in
  its summary.
- Added a route-flow settlement screenshot pause/release mode so screenshot
  smoke can still capture `11-settlement.png` before the final cat-room return.
- Extended forced-defeat smoke so failed settlement must execute cat-room return
  and include `failed cat-room return verified`.
- Hardened `P0PlayModeEvidenceChecklist` so old route-flow passed summaries
  without final cat-room return evidence and old defeat-flow passed summaries
  without failed cat-room return evidence fail the Play Mode evidence gate.
- Added focused tests in `P0RouteMapSurfaceCoverageTests` and
  `P0RouteMapCommandRouterTests`, `P0CatRoomPresenterTests`, and
  `P0PlayableReadinessTests`, plus evidence-checklist/visual-acceptance
  snapshot guards.
- Guarded `RouteMapController.ReturnToCatRoom()` against incomplete routes.
- MSBuild passed for `TheCat.Runtime.csproj`, `TheCat.EditModeTests.csproj`,
  and `TheCat.Editor.csproj` with the existing `System.Numerics.Vectors`
  MSB3277 warning.
- `git diff --check` passed for the changed files.
- Focused Unity EditMode attempts for the route-map/cat-room tests did not
  produce a test-results XML after project refresh/domain reload or licensing
  handshake errors, so no Unity test pass is claimed for this slice.

Boundary:

- This did not import or approve any candidate UI art.
- This did not replace route, battle, reward, cat upgrade, or settlement rules.
- Follow-up Play Mode evidence should capture the full final route settlement
  -> cat-room state handoff before claiming this as screenshot-backed demo
  evidence. The route-flow and defeat-flow smokes now require those states, but
  the current batchmode runner did not produce XML in this environment.

## Route Choice Effect Readiness: B6

Goal: make route choices prove they affect later play, not only route-map copy.
This is a narrow readiness slice for RestNest recovery and DreamEvent
next-battle modifiers.

Status: completed at code and MSBuild level on 2026-06-26. No Unity Play Mode
or screenshot pass is claimed for this slice.

Implemented evidence:

- Added `P0PlayableReadiness.RouteChoiceEffectsCheckId`.
- The readiness gate now builds a RestNest sample run with damaged core values
  and a weak low-HP cat, applies the route choice, and requires owner sleep,
  poop, hunger, cat HP, weak-state clearing, and `RestNestUses` to update.
- The readiness gate now applies `dream_event_catnip_residue`, consumes its
  pending next-battle modifier, verifies skill-damage and poop-growth
  multipliers, and starts a `BattleSimulation` with the modified config.
- Updated `P0PlayableReadinessTests` so prototype readiness and detailed
  summaries include `Route Choice Effects`.
- MSBuild passed for `TheCat.Runtime.csproj` and
  `TheCat.EditModeTests.csproj`.

Boundary:

- This does not change route reward tuning or default route choice selection.
- This does not replace the existing `P0RouteChoiceCoverage` gate; it adds a
  higher-level playable-readiness proof that the effects bridge into later
  battle configuration.
- Follow-up Play Mode evidence should still verify the same effects through
  real scene flow once Unity runner output is available.

## RestNest Next-Battle Evidence Gate: B7

Goal: prove RestNest recovery survives into the next battle HUD, not just into
route-state data.

Status: completed at code and MSBuild level on 2026-06-26. No fresh Unity Play
Mode output is claimed until the runner emits updated reports/screenshots.

Implemented evidence:

- `P0PlayModeRouteFlowSmoke` now seeds one low-HP weak cat immediately before
  the RestNest reward node inside the smoke runner.
- After RestNest resolves, route-flow smoke verifies the run-level cat vital
  snapshot reaches the RestNest safe HP line and clears weak state.
- On the next battle node, route-flow smoke reads `BuildCatHudCardsForSmoke()`
  and verifies the same cat starts recovered in the battle HUD.
- Route-flow pass summaries now include
  `RestNest next-battle recovery verified`.
- `P0PlayModeEvidenceChecklist` now rejects route-flow passed summaries unless
  they include both `RestNest next-battle recovery verified` and
  `cat-room return verified`.
- Updated evidence-checklist and visual-acceptance tests for the new route-flow
  marker.

Boundary:

- This does not change RestNest reward tuning or normal gameplay rules.
- The low-HP/weak setup is test-runner-only state seeding for deterministic
  smoke evidence.
- Follow-up Unity Play Mode evidence should refresh route-flow and screenshot
  reports before using B7 as screenshot-backed demo proof.

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

## Cat-Room Dream Theme Choice: B12

Goal: make the cat-room dream theme choice visible and testable without turning
Egypt into a playable route or resetting the selected starter roster.

Status: completed at code, readiness, MSBuild, and review level on
2026-06-26.

Implemented evidence:

- `P0CatRoomPresenter` now exposes `DreamChoices` built from
  `P0DreamMapCatalog.CreateP0DreamMaps()`.
- Bedroom dream is the only enabled playable choice and targets the existing
  route-map scene through `enter_dream`.
- Egypt dream is visible as `占位，不可进入`, disabled, has action
  `preview_egypt_dream`, and has no scene target.
- `P0RunSession.EnsureBedroomDreamRun()` preserves an active Bedroom run and
  selected roster; placeholder or completed contexts are converted back to a
  Bedroom run with the existing roster.
- `P0PlayableReadiness.EntryCharacterSelectCheckId` now requires the Bedroom
  choice to be enabled and the Egypt choice to be disabled/no-target.
- Hume and Locke independently reviewed the slice and their P0/P1 findings
  were addressed before validation.

Validation:

- Runtime, EditModeTests, and Editor MSBuild passed. Editor still reports the
  existing `System.Numerics.Vectors` MSB3277 warning.
- Unity batchmode focused attempts logged to:
  - `Logs/p0_b12_cat_room_dream_choices_editmode_20260626.log`
  - `Logs/p0_b12_cat_room_dream_choices_editmode_retry_20260626.log`
- Both Unity attempts reached AssetDatabase refresh/domain reload and exited
  with return code `0`, but produced no XML; they are not counted as passing
  Unity TestRunner results.

Boundary:

- Egypt remains placeholder/readiness only: no Egypt scene start, no unique
  combat branch, no unique map content, and no candidate-asset promotion.
- Batch 90 cat-room candidates remain candidate-backed evidence only; B12 does
  not approve formal runtime bindings.

## Play Mode Acceptance Refresh After Dream Choice: B13

Goal: refresh normal Editor Play Mode evidence after the cat-room dream theme
choice surface landed.

Status: completed on 2026-06-26.

Implemented evidence:

- Normal Editor Play Mode runner passed via
  `TheCat.EditorTools.P0PlayModeEvidenceBatchmodeRunner.RunPlayModeAcceptanceSmokeForBatchmode`.
- Refreshed report:
  `design/development/unity_batchmode/P0_PLAYMODE_ACCEPTANCE_SMOKE_REPORT.md`.
- Refreshed screenshots:
  `design/development/screenshots/p0-playmode-smoke`.
- `02-cat-room.png` now captures the cat-room panel with the two dream theme
  rows and disabled Egypt placeholder action.

Validation:

- Normal Editor runner log:
  `Logs/P0PlayModeAcceptanceVisual_B12_normal_20260626.log`.
- Report result: `passed`; smoke state: `Passed`.
- Evidence: `8/8` passed checks, `0` warnings, `0` failures.
- Screenshot evidence: `11/11` validated captures.
- A first `-batchmode` visual attempt produced gray single-color screenshots
  and failed the screenshot file evidence gate; it is superseded by the normal
  Editor result above.

Boundary:

- B13 is baseline runtime evidence only. It does not approve Batch 90 formal
  install, Egypt unique content, or starter-cat body-art replacement.

## Cat-Room Dream Choice Readability: B14

Goal: make the B12 dream choice contract readable at a glance in the current
cat-room IMGUI surface without changing routing, gameplay, or asset bindings.

Status: completed on 2026-06-26.

Implemented:

- Moved `DrawDreamChoices(surface)` directly below cat-room return feedback.
- Moved `DrawActions(surface)` directly below the dream choices so the player
  reads dream selection and route/placeholder actions together.
- Added dream-choice header formatting in `CatRoomController`:
  - enabled Bedroom route: `可进入当前Demo路线`;
  - disabled Egypt placeholder: `占位，不可进入，无跳转`.
- Shortened Egypt presenter copy to `占位，不可进入` plus the explicit no-route,
  no-map, no-enemy, no-visual-evidence boundary.
- Updated readiness and presenter tests so the new Egypt placeholder wording is
  contract-locked and does not rely on the older long-form placeholder copy.

Agent review:

- McClintock completed a read-only B14 review and flagged that Bedroom/Egypt
  were too visually similar, dream choices were too far from actions, and the
  disabled Egypt button could still read as an actionable route.
- B14 addressed those findings with local cat-room presentation/copy changes
  only. No catalog, route, scene, combat, or asset-promotion change was made.

Validation:

- Runtime MSBuild passed: `TheCat.Runtime.csproj`.
- EditModeTests MSBuild passed: `TheCat.EditModeTests.csproj`.
- Editor MSBuild passed with the existing `System.Numerics.Vectors` MSB3277
  warning: `TheCat.Editor.csproj`.
- Normal Editor Play Mode runner log:
  `Logs/P0PlayModeAcceptanceVisual_B14_catroom_readability_final_20260626.log`.
- Report result: `passed`; smoke state: `Passed`.
- Evidence: `8/8` passed checks, `0` warnings, `0` failures.
- Screenshot evidence: `11/11` validated captures.
- Visual check: `02-cat-room.png` now shows the dream choice block near the top
  of the panel with distinct Bedroom enabled and Egypt placeholder states.

Boundary:

- B14 does not approve Batch 90 formal install.
- B14 does not add playable Egypt content or import candidate art.
- Egypt remains a visible disabled/no-target readiness placeholder.

## Main-Menu Player-Path Readability: B15

Goal: make the opening menu's real player path readable at a glance without
changing the D1 action contract, route-flow smoke start APIs, or candidate art
bindings.

Status: completed on 2026-06-26.

Implemented:

- Reordered `MainMenuController` so the first viewport prioritizes starter-cat
  selection, then the cat-room primary path.
- Collapsed graybox route/quick-battle tools behind a single helper toggle.
- Widened the main-menu panel to match the current cat-room readability pass.
- Changed starter selection into compact IMGUI cards with HUD avatar, display
  name, role, ready state, role hint, authority/attribute, and skill preview.
- Preserved `MainMenuController.StartP0Run()` default route behavior for
  Play Mode route and defeat smokes.
- Added `P0MainMenuCoverageTests` assertions for the visible starter-card
  fields consumed by the new IMGUI layout.

Agent review:

- Singer reviewed main-menu code boundaries and confirmed the safe write scope
  is local presentation plus focused tests, with no scene-flow, session, or
  candidate-art changes.
- Huygens reviewed validation boundaries and confirmed Play Mode smoke verifies
  the surface contract and direct start APIs rather than depending on button
  coordinates.

Validation:

- Runtime MSBuild passed: `TheCat.Runtime.csproj`.
- EditModeTests MSBuild passed: `TheCat.EditModeTests.csproj`.
- Editor MSBuild passed with the existing `System.Numerics.Vectors` MSB3277
  warning: `TheCat.Editor.csproj`.
- Normal Editor Play Mode runner log:
  `Logs/P0PlayModeAcceptanceVisual_B15_main_menu_readability_20260626.log`.
- Report result: `passed`; smoke state: `Passed`.
- Evidence: `8/8` passed checks, `0` warnings, `0` failures.
- Screenshot evidence: `11/11` validated captures.
- Visual check: `01-main-menu.png` now shows starter cards, the cat-room
  primary CTA, and a collapsed graybox helper entry in the first viewport.
- Focused Unity EditMode attempt logged to
  `Logs/b15_main_menu_readability_editmode_20260626.log`; Unity exited with
  return code `0` after AssetDatabase refresh/domain reload but did not write
  `Logs/b15_main_menu_readability_editmode_20260626.xml`, so it is not counted
  as a passing Unity TestRunner result.

Boundary:

- B15 does not approve Batch 88 formal install.
- B15 does not import or bind candidate character-select art.
- B15 does not add coordinate-click validation; the current smoke evidence
  proves contract/API continuity and screenshot file validity.
- Focused Unity TestRunner XML generation remains a validation tooling
  follow-up.

## Route-Map Current-Node Readability: B16

Goal: make the route-map first viewport answer "where am I and what do I do
next?" without changing route rules, command routing, scene flow, or candidate
route-map art approval state.

Status: completed on 2026-06-26.

Implemented:

- Reordered `RouteMapController` so non-settlement route-map screens prioritize:
  - progress/status;
  - current-node card with icon, layer, type, title, risk, reward, and detail;
  - current route options, reward choices, or pending cat-upgrade choices;
  - primary route CTA;
  - secondary new-route/main-menu actions;
  - collapsed route-history and resource/team details.
- Widened the non-settlement route-map panel for readability.
- Preserved route-map public contracts and command semantics:
  `RouteMapSceneName`, `ExecuteInputCommand`, `BuildRouteMapSurfaceForSmoke`,
  `EnterCurrentNode`, `ReturnToCatRoom`, `P0RouteMapActionIds`, and
  `P0RouteMapCommandRouter`.
- Hardened `P0RouteMapSurfaceCoverage` and
  `P0RouteMapSurfaceCoverageTests` so layer-one evidence now requires
  player-facing current-node fields, one selected/current timeline row, enabled
  `enter_current_node` CTA command/target/detail, and no current-node developer
  tokens.

Agent review:

- Gauss reviewed route-map code boundaries and confirmed the slice should stay
  in `RouteMapController` presentation plus surface assertions while preserving
  route state, action ids, and command/router contracts.
- Einstein reviewed validation boundaries and confirmed Play Mode route and
  screenshot smokes are command/surface driven, not coordinate-click driven.

Validation:

- Runtime MSBuild passed: `TheCat.Runtime.csproj`.
- EditModeTests MSBuild passed: `TheCat.EditModeTests.csproj`.
- Editor MSBuild passed with the existing `System.Numerics.Vectors` MSB3277
  warning: `TheCat.Editor.csproj`.
- Normal Editor Play Mode runner log:
  `Logs/P0PlayModeAcceptanceVisual_B16_route_map_readability_20260626.log`.
- Report result: `passed`; smoke state: `Passed`.
- Evidence: `8/8` passed checks, `0` warnings, `0` failures.
- Screenshot evidence: `11/11` validated captures.
- Offline acceptance runner log:
  `Logs/P0OfflineAcceptance_B16_route_map_readability_20260626.log`.
- Offline report result: `passed`, failure count `0`, route-map surface coverage
  `21` check(s).
- Visual check: `03-route-map-layer1.png` now shows the current-node focus,
  `进入当前节点` primary CTA, secondary route controls, and folded route/resource
  detail buttons in the first viewport.
- Route-flow evidence remains `nodes 10/10`, `boss observed`, RestNest,
  DreamEvent, Shop bed-patch, and cat-room return verified.
- Defeat-flow evidence remains failed settlement plus failed cat-room return
  verified.

Boundary:

- B16 does not approve Batch 65 route-map readability candidate formal install.
- B16 does not add coordinate-click validation.
- B16 does not change route choices, rewards, enemy/wave rules, battle numbers,
  or cat-upgrade behavior.
- Raw Unity log still contains external Unity AI Assistant/licensing/MCP noise,
  but the project-owned smoke report is passed with no evidence warnings or
  failures.

## Battle-Result Player Readability: B17

Goal: make the post-battle result first viewport answer "what happened, what
did I earn, and what do I do next?" without changing combat rules, route
completion, result action ids, scene targets, rewards, or candidate
result/settlement art approval state.

Status: completed on 2026-06-26.

Implemented:

- Reworked the non-diagnostic `GrayboxBattleController` result state so the
  player sees a single focused result card before any detailed metrics:
  - outcome banner and prompt;
  - route result;
  - reward;
  - next node;
  - route progress;
  - owner-sleep summary;
  - primary `继续路线` action;
  - secondary `返回猫房` and `重开路线` actions;
  - folded detailed settlement metrics.
- Stopped drawing cat, skill, and interaction controls after battle resolution
  in non-diagnostic HUD mode.
- Hid post-result feedback visuals in non-diagnostic result state so runtime
  visual/debug strings no longer appear beneath the player-facing result card.
- Added `P0BattleResultPresenter.BuildPlayerFocusRows()` as a local summary
  helper while preserving `P0BattleResultSurface` fields, `TryGetAction()`, and
  `P0BattleResultActionIds`.
- Corrected result shortcut labels to match `P0KeyboardInputMap`: continue uses
  `Enter`, restart uses `N`, and return-to-cat-room shows no fake `C`
  shortcut.
- Hardened `P0BattleResultCoverage` and `P0BattleResultCoverageTests` so result
  evidence now covers focus rows and keyboard-label drift.

Agent review:

- Turing reviewed battle-result code boundaries and confirmed the slice should
  stay in controller presentation plus surface assertions while preserving
  action ids, route completion, and post-battle route-map semantics.
- Socrates reviewed validation boundaries and confirmed Play Mode route,
  defeat, and screenshot smokes should stay command/surface driven, with visual
  inspection focused on `10-battle-result-layer1.png` and `11-settlement.png`.

Validation:

- Runtime MSBuild passed: `TheCat.Runtime.csproj`.
- EditModeTests MSBuild passed: `TheCat.EditModeTests.csproj`.
- Editor MSBuild passed with the existing `System.Numerics.Vectors` MSB3277
  warning: `TheCat.Editor.csproj`.
- Offline acceptance final log:
  `Logs/P0OfflineAcceptance_B17_battle_result_readability_final_20260626.log`.
- Offline report result: `passed`, failure count `0`, battle-result coverage
  `7` result check(s), route-map surface coverage `21` surface check(s).
- Normal Editor Play Mode final log:
  `Logs/P0PlayModeAcceptanceVisual_B17_battle_result_readability_final_20260626.log`.
- Play Mode report result: `passed`; screenshot evidence remains `11/11`
  validated captures.
- Visual check: `10-battle-result-layer1.png` now shows the result focus card,
  primary `继续路线` CTA, secondary actions, folded details, and no debug
  feedback card. `11-settlement.png` remains unchanged and is a separate
  readability debt.
- Route-flow evidence remains `nodes 10/10`, `boss observed`, RestNest,
  DreamEvent, Shop bed-patch, and cat-room return verified.
- Defeat-flow evidence remains failed settlement plus failed cat-room return
  verified.

Boundary:

- B17 does not approve Batch 84 result/settlement candidate formal install.
- B17 does not change route rewards, route completion, battle outcome logic, or
  post-battle scene targets.
- B17 does not add coordinate-click validation.
- B17 leaves route settlement readability to a later slice.

## Route-Settlement Player Readability: B18

Goal: make the route-settlement screen read like a player-facing run closeout
instead of a long telemetry page, while preserving route completion, return to
cat room, and candidate-art boundaries.

Status: completed at presenter, controller, coverage, screenshot, and docs
level on 2026-06-26.

Implemented:

- Added `P0SettlementPresenter.BuildPlayerFocusRows()` for a compact
  player-facing settlement summary:
  - result;
  - route progress;
  - battle record and duration;
  - route-state resources, roster, and blessings;
  - final core values;
  - final cat-life summary.
- Added `P0RouteMapSurface.SettlementFocusRows` without replacing existing
  `SettlementRows`, `SettlementOutcomeBannerAsset`, or route-map actions.
- Updated `RouteMapController` completed-route drawing so the first viewport
  shows the outcome banner, focused settlement card, primary `返回猫房` CTA,
  secondary `新路线` / `返回主菜单` controls, and a folded `显示结算明细` control.
- Left full settlement telemetry, route history, and resource/team rows available
  inside the folded details section.
- Hardened `P0RouteMapSurfaceCoverage` and `P0RouteMapSurfaceCoverageTests` so
  cleared and failed settlements require focus rows plus `return_cat_room`
  label/detail/command/scene target.
- Added `P0SettlementPresenterTests` coverage for the readable focus rows and
  continued no-`HP`, no-`Lv`, no-internal-debug-token guarantees.

Agent review:

- Chandrasekhar reviewed route-settlement code boundaries and confirmed the
  slice should stay in completed-route drawing plus surface assertions while
  preserving action ids, route completion, and cat-room return semantics.
- Ohm reviewed validation boundaries and confirmed Play Mode route, defeat, and
  screenshot smokes should stay command/surface driven, with visual inspection
  focused on `11-settlement.png` and B17 regression review on
  `10-battle-result-layer1.png`.

Validation:

- Runtime MSBuild passed: `TheCat.Runtime.csproj`.
- EditModeTests MSBuild passed: `TheCat.EditModeTests.csproj`.
- Editor MSBuild passed with the existing `System.Numerics.Vectors` MSB3277
  warning: `TheCat.Editor.csproj`.
- Offline acceptance final log:
  `Logs/P0OfflineAcceptance_B18_route_settlement_readability_final_20260626.log`.
- Offline report result: `passed`, failure count `0`, route-map surface coverage
  `21` surface check(s), battle-result coverage `7` result check(s).
- Validation note: an intermediate offline rerun caught route-map surface
  coverage at `20/21` after the cleared settlement surface gate was coupled to
  full battle telemetry from a synthetic route state. The final surface gate now
  stays on player focus rows plus `return_cat_room`
  label/detail/command/scene target; full settlement telemetry remains covered
  by `P0SettlementPresenterTests` and Play Mode route-flow smoke.
- First normal Editor visual attempt opened a Unity `Error!` window before the
  runner entered project code and produced no new report/screenshots; it was
  terminated and superseded by the retry.
- Normal Editor Play Mode final log:
  `Logs/P0PlayModeAcceptanceVisual_B18_route_settlement_readability_final_20260626.log`.
- Play Mode report result: `passed`; screenshot evidence remains `11/11`
  validated captures refreshed at 2026-06-26 08:34 +08:00.
- Visual check: `11-settlement.png` now shows the focused settlement card,
  primary `返回猫房`, secondary actions, and folded details.
  `10-battle-result-layer1.png` still shows the B17 focused result card.
- Route-flow evidence remains `nodes 10/10`, `boss observed`, RestNest,
  DreamEvent, Shop bed-patch, and cat-room return verified.
- Defeat-flow evidence remains failed settlement plus failed cat-room return
  verified.

Boundary:

- B18 does not approve Batch 84 result/settlement candidate formal install.
- B18 does not change route rewards, route completion, battle outcome logic, or
  post-battle scene targets.
- B18 does not add coordinate-click validation.

## Demo Release/Readiness Gate Sync: B19

Goal: make the current demo-readiness claim explicit without collapsing it into
final P0 visual acceptance or formal runtime art-install approval.

Status: completed at report-parser, architecture-audit, focused-test, offline
acceptance, and docs level on 2026-06-26.

Implemented:

- Added `P0PlayModeReportFileEvidence` and
  `P0PlayModeReportFileEvidenceReport` to parse the project-owned Play Mode
  report as durable evidence.
- Required the parser to find structural anchors for result, smoke state,
  evidence summary, screenshot smoke, Screenshot File Evidence, `11/11`
  captures, the expected screenshot directory, active-cat filenames, route-flow
  RestNest/DreamEvent/Shop/cat-room-return markers, and defeat-flow failed
  return markers.
- Added `P0VisualAcceptanceReport.IsP0DemoVisualEvidenceReady` for the current
  B18 baseline evidence.
- Added `P0ArchitectureCompletionAudit.IsP0DemoReleaseReady` so architecture
  docs and gates can report current demo readiness while final runtime/visual
  acceptance remains blocked.
- Updated `P0VisualAcceptanceReportTests` and
  `P0ArchitectureCompletionAuditTests` so they assert the middle state and keep
  final acceptance false.

Agent review:

- Kierkegaard reviewed the architecture boundary and confirmed the gate should
  name the current demo-ready state without setting final acceptance true.
- Mendel reviewed the validation boundary and confirmed the report parser must
  require concrete file/screenshot anchors while keeping formal install and
  starter-cat gates blocked.

Validation:

- Runtime MSBuild passed: `TheCat.Runtime.csproj`.
- EditModeTests MSBuild passed: `TheCat.EditModeTests.csproj`.
- Editor MSBuild passed with the existing `System.Numerics.Vectors` MSB3277
  warning: `TheCat.Editor.csproj`.
- Focused Unity EditMode TestRunner exited `0` but produced no XML, so it is
  documented as non-authoritative.
- Final offline acceptance log:
  `Logs/P0OfflineAcceptance_B19_demo_readiness_gate_final_20260626.log`.
- Offline report result: `passed`, failure count `0`, code smoke suite `28`
  passed checks, route-map surface coverage `21` checks, and battle-result
  coverage `7` checks.

Boundary:

- B19 does not change gameplay, scenes, UI layout, screenshots, candidate art,
  or runtime visual bindings.
- B19 does not approve Batch 83-90 formal runtime install.
- B19 does not approve starter-cat formal body-art replacement.
- B19 keeps final Unity runtime, clean Console, human approval, formal runtime
  binding, candidate install, and final visual acceptance blocked.

## Formal Install Blocker Matrix: B20

Goal: centralize the current formal-install blocker state so runtime-evidence
`6/8` lanes cannot be mistaken for formal runtime install approval.

Status: completed at asset gate, offline acceptance, generated matrix report,
focused test, and docs level on 2026-06-26.

Implemented:

- Added `P0FormalInstallGate`, `P0FormalInstallGateReport`, and row-level
  `P0FormalInstallGateQueueRow` records.
- The gate evaluates the current `P0AssetProductionQueueCatalog` and starter-cat
  formal import gate without mutating candidate assets or runtime bindings.
- The matrix requires the current queue count, candidate-pending/Unity-blocked
  split, Batch 83-90 runtime-evidence `6/8` lanes, per-lane scene/Console
  blockers, per-lane human-approval blockers, zero ready-for-formal-install
  rows, and starter-cat body import still blocked.
- Added `P0AssetProductionReadinessTests` coverage for the current blocked
  matrix plus a forged Batch 87 lane that removes human approval wording.
- Added `P0 Formal Install Gate Matrix` to the offline acceptance runner.
- Offline acceptance writes
  `design/development/asset_review/P0_FORMAL_INSTALL_BLOCKER_EVIDENCE_MATRIX_2026-06-26.md`.

Agent review:

- Newton confirmed B19's demo-readiness boundary and recommended Batch 87 as a
  future narrow single-lane hardening target.
- Bernoulli recommended the unified formal-install blocker matrix before any
  single-lane install decision because Batch 83-90 evidence is now spread across
  runtime reports, preflight reports, ledgers, and checklists.
- Averroes recommended a future shared Console/log classifier and report-binding
  layer before any release packet or formal install gate can claim clean Console.

Validation:

- Runtime MSBuild passed: `TheCat.Runtime.csproj`.
- EditModeTests MSBuild passed: `TheCat.EditModeTests.csproj`.
- Editor MSBuild passed with the existing `System.Numerics.Vectors` MSB3277
  warning: `TheCat.Editor.csproj`.
- Focused Unity EditMode TestRunner exited `0` but produced no XML, so it is
  documented as non-authoritative.
- Final offline acceptance log:
  `Logs/P0OfflineAcceptance_B20_formal_install_matrix_report_final_20260626.log`.
- Offline report result: `passed`, gate count `7`, failure count `0`.
- Matrix report decision: formal install allowed `no`; formal install blocked
  `yes`; runtime evidence `6/8` items `8`.

Boundary:

- B20 does not approve any Batch 83-90 formal runtime install.
- B20 does not approve starter-cat formal body-art replacement.
- B20 does not generate clean Console reports, human approval files, scene
  binding reports, or formal runtime binding diffs.
- Next likely blocker slice: shared project-owned Console/log classifier plus
  runtime-evidence report binding, then Batch 87 can be considered as the first
  single-lane formal-install hardening target.

## Shared Unity Console Log Classifier: B21

Goal: create one conservative project-owned log-review contract before any
formal install lane is allowed to claim clean Console evidence.

Status: completed at runtime tool, focused assertion, offline acceptance, and
docs level on 2026-06-26.

Implemented:

- Added `P0UnityConsoleLogClassifier`,
  `P0UnityConsoleLogClassifierReport`, and signal records in
  `P0UnityRuntimeValidationPlan.cs`.
- The classifier separates:
  - strict clean logs;
  - project-owned clean logs that still contain known Unity/environment noise;
  - unknown blocking tokens such as unclassified `Error`, `Exception`, or
    `FAILED` lines;
  - TheCat project failure tokens from acceptance/runtime-evidence logs.
- Known environment noise currently covers Licensing, Unity AI tracing, D3D,
  MemoryLeaks payloads, StackAllocator/ALLOC_TEMP shutdown noise, Package
  Manager, and AcceleratorClient categories.
- Added `P0UnityRuntimeValidationPlanTests` coverage for clean project pass
  logs, known environment noise, unknown plugin errors, and TheCat project
  failure tokens.
- Kept known environment noise non-strict: it can keep a log project-owned
  clean, but it never counts as final strict Console cleanliness.

Validation:

- Runtime MSBuild passed: `TheCat.Runtime.csproj`.
- EditModeTests MSBuild passed: `TheCat.EditModeTests.csproj`.
- Editor MSBuild passed with the existing `System.Numerics.Vectors` MSB3277
  warning: `TheCat.Editor.csproj`.
- Focused Unity EditMode TestRunner produced
  `Logs/B21_unity_console_classifier_editmode_20260626.log` but no XML result
  file, so it is not counted as a passing TestRunner report.
- Offline acceptance log:
  `Logs/P0OfflineAcceptance_B21_unity_console_classifier_20260626.log`.
- Offline report result: `passed`, gate count `7`, failure count `0`.
- The same raw Unity log still includes external Licensing, Unity AI tracing,
  D3D, MemoryLeaks, and StackAllocator noise, so B21 does not claim strict
  clean Console.

Boundary:

- B21 does not replace the per-batch preflight `FindConsoleFailureToken`
  implementations yet.
- B21 does not approve clean Console evidence for Batch 83-90.
- B21 does not approve formal runtime binding, human approval, starter-cat
  replacement, or final P0 visual acceptance.
- Next likely blocker slice: bind this classifier to runtime-evidence report
  files and then migrate the formal-install lanes toward the shared contract.

## Console Classifier Preflight Migration: B22

Goal: remove duplicated per-batch Console token tables and route current
formal clean-log checks through the shared classifier without relaxing the
strict clean-Console gate.

Status: completed at Batch 83-90 preflight code, focused assertion, offline
acceptance, and docs level on 2026-06-26.

Implemented:

- Replaced Batch 83-90 `FindConsoleFailureToken(logContent)` calls with
  `P0UnityConsoleLogClassifier.Classify(logContent)`.
- Kept formal clean-Console semantics strict: preflight evidence is accepted
  only when `consoleLogReport.StrictClean` is true.
- Updated dirty-log blocking messages to include `not strict clean`, first
  classifier signal token, and classifier summary.
- Removed the now-unused private `FindConsoleFailureToken` methods from Batch
  83, 84, 85, 86, 87, 88, 89, and 90 preflight tools.
- Added `not strict clean` assertions to dirty-log preflight tests across the
  migrated lanes.
- Expanded known environment noise classification for Unity AI tracing stack
  lines such as `Unity.AI.Tracing...System.Exception`.

Validation:

- Runtime MSBuild passed: `TheCat.Runtime.csproj`.
- EditModeTests MSBuild passed: `TheCat.EditModeTests.csproj`.
- Editor MSBuild passed with the existing `System.Numerics.Vectors` MSB3277
  warning: `TheCat.Editor.csproj`.
- Focused Unity EditMode TestRunner produced
  `Logs/B22_unity_console_classifier_preflight_migration_editmode_20260626.log`
  but no XML result file, so it is not counted as a passing TestRunner report.
- Offline acceptance log:
  `Logs/P0OfflineAcceptance_B22_console_classifier_preflight_migration_20260626.log`.
- Offline report result: `passed`, gate count `7`, failure count `0`.
- The raw Unity log still includes external Licensing, Unity AI tracing, D3D,
  MemoryLeaks, and StackAllocator noise, so B22 does not claim strict clean
  Console.

Boundary:

- B22 does not approve clean Console evidence for any formal-install lane.
- B22 does not create standalone classifier evidence files yet.
- B22 does not approve formal runtime binding, human approval, starter-cat
  replacement, or final P0 visual acceptance.
- Next likely blocker slice: bind classifier summaries into generated
  runtime-evidence reports and the formal-install matrix before Batch 87
  single-lane hardening.

## Formal Install Report Classifier Binding: B23

Goal: expose the shared Unity Console classifier policy in formal-install
reports before any single-lane formal install hardening or clean-Console claim.

Status: completed at formal-install gate/report, focused assertion, offline
acceptance, and docs level on 2026-06-26.

Implemented:

- Added `SharedConsoleClassifierContractReady` and
  `ConsoleClassifierPolicySummary` to `P0FormalInstallGateReport`.
- Bound `P0FormalInstallGate` to `P0UnityConsoleLogClassifier` with a contract
  check covering known Unity/environment noise and unknown blocking errors.
- Increased `P0FormalInstallGate.ExpectedCoveredCheckCount` from `7` to `8` so
  the shared strict-clean Console classifier contract is covered by the matrix.
- Generated formal-install markdown now includes
  `Shared Console classifier: active strict-clean contract` and the strict-clean
  policy summary.
- Updated focused asset-production readiness assertions for the current blocked
  matrix.

Validation:

- Runtime MSBuild passed: `TheCat.Runtime.csproj`.
- EditModeTests MSBuild passed: `TheCat.EditModeTests.csproj`.
- Editor MSBuild passed with the existing `System.Numerics.Vectors` MSB3277
  warning: `TheCat.Editor.csproj`.
- Focused Unity EditMode TestRunner produced
  `Logs/B23_formal_install_classifier_contract_editmode_20260626.log` but no
  XML result file, so it is not counted as a passing TestRunner report.
- Offline acceptance log:
  `Logs/P0OfflineAcceptance_B23_formal_install_classifier_contract_20260626.log`.
- Offline report result: `passed`, gate count `7`, failure count `0`.
- Generated report anchors:
  `design/development/unity_batchmode/P0_OFFLINE_ACCEPTANCE_REPORT.md` and
  `design/development/asset_review/P0_FORMAL_INSTALL_BLOCKER_EVIDENCE_MATRIX_2026-06-26.md`.

Boundary:

- B23 does not approve clean Console evidence for any formal-install lane.
- B23 does not approve Batch 83-90 formal runtime install.
- B23 does not approve formal runtime binding, human approval, starter-cat
  replacement, candidate install, or final P0 visual acceptance.
- Next likely blocker slice: dispatch a fresh validation-boundary review, then
  harden one formal-install lane with classifier-backed runtime evidence rather
  than broad install approval.

## Batch 87 Formal Runtime Binding Decision Request Packet: B26

Goal: make the Batch 87 formal runtime binding decision auditable without
changing the runtime catalog, battle HUD runtime path, or formal install state.

Status: completed on 2026-06-26. The Batch 87 preflight runner now writes the
formal-runtime-binding decision request packet beside the preflight and
human-review request packets.

Implemented evidence:

- Added
  `P0BattleHudBatch87UnityPreflight.FormalRuntimeBindingDecisionRequestPath`.
- Added
  `P0BattleHudBatch87UnityPreflightReport.BuildFormalRuntimeBindingDecisionRequestMarkdown()`.
- Updated `P0BattleHudBatch87UnityPreflightRunner` so it writes the generated
  formal-runtime-binding request packet.
- Updated `P0BattleHudBatch87UnityPreflightTests` so the request packet must
  list the proposed candidate binding scope, keep formal runtime binding
  unapproved, and avoid approval tokens.
- Generated:
  `design/development/asset_review/BATCH87_BATTLE_HUD_FORMAL_RUNTIME_BINDING_DECISION_REQUEST_2026-06-26.md`.

Validation:

- Runtime MSBuild passed.
- EditModeTests MSBuild passed.
- Editor MSBuild passed with the known `System.Numerics.Vectors` MSB3277
  warning.
- Batch 87 preflight runner completed:
  `Logs/B26_batch87_formal_binding_decision_request_preflight_20260626.log`.
- Focused Unity EditMode TestRunner invocation produced
  `Logs/B26_batch87_formal_binding_decision_request_editmode_20260626.log` but
  no XML result file; it is not counted as a passing TestRunner report.
- Offline acceptance passed:
  `Logs/P0OfflineAcceptance_B26_batch87_formal_binding_decision_request_20260626.log`,
  with report result `passed`, gate count `7`, failure count `0`, and formal
  install still `no`.

Boundary:

- B26 does not add Batch 87 candidate ids to `P0VisualAssetCatalog`.
- B26 does not alter battle HUD scene, presenter, controller, or runtime path.
- B26 does not create or count `console_clean_report.md`.
- B26 does not create or count `human_review_approval.md`.
- B26 does not approve formal install, final visual acceptance, or starter-cat
  replacement.

## Batch 87 Human Review Request Packet: B25

Goal: create a repeatable Batch 87 human-review handoff packet without
creating formal approval evidence or changing the runtime binding decision.

Status: completed on 2026-06-26. The Batch 87 preflight runner now writes the
human-review request packet beside the preflight report.

Implemented evidence:

- Added `P0BattleHudBatch87UnityPreflight.HumanReviewRequestPath`.
- Added `P0BattleHudBatch87UnityPreflightReport.BuildHumanReviewRequestMarkdown()`.
- Updated `P0BattleHudBatch87UnityPreflightRunner` to write both the generated
  preflight report and the review-request packet.
- Updated `P0BattleHudBatch87UnityPreflightTests` so the request packet must
  list evidence paths, keep formal install blocked, and avoid the exact formal
  approval pass token.
- Generated:
  `design/development/asset_review/BATCH87_BATTLE_HUD_HUMAN_REVIEW_REQUEST_2026-06-26.md`.

Validation:

- Runtime MSBuild passed.
- EditModeTests MSBuild passed.
- Editor MSBuild passed with the known `System.Numerics.Vectors` MSB3277
  warning.
- Batch 87 preflight runner completed:
  `Logs/B25_batch87_human_review_request_preflight_20260626_rerun.log`.
- Focused Unity EditMode TestRunner invocation produced only
  `Logs/B25_batch87_human_review_request_editmode_20260626.log`; no XML result
  file exists, so it is not counted as a passing TestRunner report.
- Offline acceptance passed:
  `Logs/P0OfflineAcceptance_B25_batch87_human_review_request_20260626.log`,
  with report result `passed`, gate count `7`, failure count `0`, and formal
  install still `no`.

Boundary:

- B25 does not create or count `human_review_approval.md`.
- B25 does not create or count `console_clean_report.md`.
- B25 does not add Batch 87 candidate ids to formal runtime bindings.
- B25 does not approve formal runtime binding, clean Console, or final visual
  acceptance.

## Batch 87 Formal Gate Precision: B24

Goal: make Batch 87 battle-HUD hardening safe by splitting Unity evidence
completion from formal install approval.

Status: completed at Batch 87 preflight gate/report, focused assertion,
preflight runner, offline acceptance, and docs level on 2026-06-26.

Implemented:

- Added `UnityEvidenceComplete` and `FormalRuntimeBindingDecisionApproved` to
  `P0BattleHudBatch87UnityPreflightReport`.
- Kept `FormalInstallAllowed` false unless a future formal runtime binding
  decision gate explicitly passes.
- Added Batch 87 report fields for the shared strict-clean Console classifier
  contract and policy.
- Tightened dirty-log blocking output so non-strict-clean runtime logs include
  the shared policy summary.
- Updated focused tests so synthetic 8/8 evidence completes Unity evidence but
  still does not approve formal install.
- Regenerated
  `design/development/asset_review/BATCH87_BATTLE_HUD_UNITY_PREFLIGHT_REPORT_2026-06-25.md`
  with `Runtime evidence: 6/8`, `Unity evidence complete: no`,
  `Formal runtime binding decision approved: no`, and
  `Shared Console classifier: active strict-clean contract`.

Validation:

- Runtime MSBuild passed: `TheCat.Runtime.csproj`.
- EditModeTests MSBuild passed: `TheCat.EditModeTests.csproj`.
- Editor MSBuild passed with the existing `System.Numerics.Vectors` MSB3277
  warning: `TheCat.Editor.csproj`.
- Focused Unity EditMode TestRunner produced
  `Logs/B24_batch87_formal_gate_precision_editmode_20260626.log` but no XML
  result file, so it is not counted as a passing TestRunner report.
- Batch 87 preflight runner log:
  `Logs/B24_batch87_classifier_gate_preflight_20260626_rerun.log`.
- Offline acceptance log:
  `Logs/P0OfflineAcceptance_B24_batch87_formal_gate_precision_20260626.log`.
- Offline report result: `passed`, gate count `7`, failure count `0`.

Boundary:

- B24 does not approve clean Console evidence.
- B24 does not create `console_clean_report.md` or
  `human_review_approval.md`.
- B24 does not add Batch 87 candidate ids to formal runtime bindings.
- B24 does not approve Batch 87 formal runtime install, human approval,
  candidate install, starter-cat replacement, or final P0 visual acceptance.
- Next likely blocker slice after B25: obtain a genuinely strict-clean Batch 87
  runtime/preflight log, or wait for explicit human approval before creating any
  approval evidence; do not create approval files from the request packet.

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
until registered active-cat screenshots receive explicit colored-turnaround
comparison approval notes.

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
  have since received K3/K4 candidate-only Unity preflight imports, but still
  remain blocked from formal install after their candidate-backed runtime
  evidence `6/8`; scene/presenter binding, clean Console, explicit human
  approval, and formal runtime binding decisions are still required.
- No Batch 83-88 candidate art was imported, bound, or accepted.
- Starter-cat body art remains locked.

Next:

- G1 has refreshed the current 11-capture Play Mode baseline for Batch 87,
  Batch 88, Batch 86, and Batch 84 review context, but it is not an import or
  final visual acceptance gate.
- Battle world label safe-area / overlay hierarchy polish is required before
  claiming final Batch 87 battle-HUD or battle-world visual acceptance.
- H1 has added the Batch 85 full settings hook and Batch 83 loading/start hook.
  Batch 83 later advanced through K5/M3 to candidate-backed runtime evidence
  6/8, with scene/presenter binding, clean Console, explicit human approval,
  and formal runtime binding decision still open. Batch 85 still needs
  dedicated Unity-rendered screenshots, import settings, binding proof, and
  Console review before any import decision.

## Loading Start And Full Settings Hooks: H1

Goal: make Batch 83 loading/start and Batch 85 full settings reviewable as
runtime hook surfaces without importing candidate art.

Status: completed at code, readiness, and focused-test level on 2026-06-25.
Batch 83 later advanced through K5/M3 to candidate-backed runtime evidence 6/8.
Batch 85 screenshot/import evidence is still pending.

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
- At H1 close, Batch 83 and Batch 85 were hook-ready, not import-approved.
  Batch 83 later advanced through K5/M3 without formal install; Batch 85
  remains hook-ready and pending runtime screenshot evidence.
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
