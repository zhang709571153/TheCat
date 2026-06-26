# P0 Agent Dispatch And Task Graph - 2026-06-25

Checked: 2026-06-26 09:59 +08:00

This document turns the current source-reading pass into concrete agent lanes
and development tasks. The main session remains architect, integrator, and
final gate owner.

## 1. Active Agents

| Agent | Role | Scope | Output needed |
|-|-|-|-|
| `019efc21-8f7d-7900-be40-c512296a0623` / Harvey | Engineering status audit | Existing code, scenes, tests, current gaps | Implemented systems map, top missing demo gaps, stale docs. |
| `019efc21-e34e-7e53-9b8d-e899683d43d4` / Plato | Gameplay/design consistency audit | `docs` source, current development docs | Non-negotiable P0 loop, coverage gaps, review checklist. |
| `019efc22-1c0e-7a82-9d34-b0571a33505c` / Tesla | UI/asset audit | Asset review, candidates, manifests, runtime art | What is runtime-bound, what is candidate-only, next validation tasks. |
| `019efc22-473a-7151-9d5a-3ea908e2438c` / Turing | Documentation/retirement audit | Development docs and source hierarchy | Current entry points, conflicts, archive/retirement recommendations. |
| `019efc7e-4cd5-7170-bbf5-19b30ae275a2` / Planck | D1 UI contract review | Main-menu and character-select contract | Complete; recommended cat-room as only player-primary CTA and graybox shortcut classification. |
| `019efc7e-74cd-77d2-ac09-5a339db4470a` / Pasteur | D1 validation review | Entry/character-select readiness and coverage | Complete; recommended named entry/character-select readiness check. |
| `019efc95-9023-7f23-b549-8eaf8e95a635` / Arendt | D2 pause/settings review | Runtime settings acceptance contract | Complete; recommended real pause/speed/restart-confirm settings only and no Batch 85 import. |
| `019efc95-b818-7912-8f8e-0e5812d92ebc` / Schrodinger | D2 skill-selection review | Skill-selection acceptance contract | Complete; recommended candidate-only Batch 89 boundary and no combat-rule expansion. |
| `019efcad-bbd5-76e3-bb13-390c54ea7567` / Fermat | E1 battle readability review | Default battle HUD readability | Complete; recommended a top player brief with priority, command deck, and compact threat/status overflow. |
| `019efcad-f473-78b3-9a15-b4fd75bdd2ae` / Erdos | E1 player-path validation review | Battle-result and screenshot evidence boundary | Complete; recommended hoisting continue/return/restart result actions and keeping Batch 85/89/90 candidate-only. |
| `019efcc0-8a6a-7512-a113-76627e49a01d` / Mendel | F1 UI visual-priority gate | Batch 83-88 candidate packets | Complete; recommended 88, 87, 86, 84, 85, 83 review order and no import before fresh screenshots. |
| `019efcc0-b5e4-7683-bf6d-2f8469dd8f19` / Bernoulli | F1 queue/checklist gate | Batch 83-88 production validation evidence | Complete; docs-only gate is sufficient, Unity acceptance/import approval is not. |
| `019efcc2-ddb7-7b12-984a-82a8fbcd1f91` / Hypatia | F1 runtime-surface mapping | Batch 83-88 runtime UI surfaces | Complete; at F1 time, 87/88/86/84 were screenshot-parity ready, 85 was partial, and 83 needed a loading/start hook. H1 has since added the 83/85 hooks without approving import. |
| `019efcd3-65be-73e1-ac19-d1996eb14ee4` / James | H1 loading/start review | Batch 83 loading/start hook | Complete; no P0 issue. Confirmed hook-only status and recommended deep candidate-token scanning, addressed in H1 review-fix. |
| `019efcd3-84b6-7073-92fe-25116088e48b` / Carson | H1 full settings review | Batch 85 full settings hook | Complete; no P0 issue. Recommended semantic non-speed rows and deep Batch 85 scanning, addressed in H1 review-fix. |
| `019efcd3-9a22-74c1-bd9f-7f6ca1764a94` / Hilbert | H1 smoke/readiness review | 11-capture screenshot plan and docs state | Complete; no P0 issue. Confirmed H1 does not change the 11-capture plan and remains screenshot/import pending. |
| `019efced-ab4b-79b3-a73c-e72197380044` / Noether | G1 screenshot visual review | 11-capture screenshot baseline | Complete; no P0 issue. G1 could close as evidence refresh; the battle world label/overlay P1 debt identified here was later resolved by I1. |
| `019efced-bf9a-73e1-be72-0cc2c42c5268` / Mencius | G1 evidence-chain review | Play Mode retry, first-run OOM, report boundaries | Complete; retry is valid evidence, first OOM is superseded, full acceptance remains not evaluated. |
| `019efced-d3b1-77f0-b7e1-6524e9024201` / Locke | G1/H1/F1 boundary review | import/final-acceptance wording | Complete; no P0 issue. Unified G1 as 11-capture evidence refresh only, with no import or final art acceptance. |
| `019efee8-04fd-75b2-84a4-950e68f113f8` / Nietzsche | I1 screenshot visual review | Battle HUD, battle-world, warning VFX, result screenshots | Complete; no P0/P1 issue. I1 visual debt is resolved for the current baseline; faint non-active background trace is not the prior active warning-line/label problem. |
| `019efee8-1980-7933-8928-ee2882547222` / Sartre | I1 code and validation boundary review | Diagnostic gates, warning shapes, smoke timing, material lifecycle | Complete; no P0 scope issue. Follow-up risks around direct controller coverage and warning material lifecycle were addressed before final validation. |
| `019efefd-6078-7dd1-b67d-79889d3c7e7d` / Euclid | J1 Batch 87 visual/evidence review | Batch 87 triage, local 1024x768 mockup, current I1 battle screenshots | Complete; no P0. Current runtime baseline is not confused with candidate-backed evidence; remaining P1 evidence-chain risk is candidate-backed Unity validation. |
| `019efefd-74aa-76e0-b2cd-924f52a7c1d3` / Hubble | J1 Batch 87 gate/boundary review | Candidate-only boundary, queue/checklist/catalog wording | Complete; no P0/P1 boundary issue. Generic screenshot wording was tightened to Batch 87 candidate-backed evidence in current control docs. |
| `019f00cf-156e-7443-ad35-c5cf8e63ddf1` / Ramanujan | B6/B7 gameplay evidence review | RestNest readiness, route-flow evidence checklist, screenshot pause path | Complete; found RestNest hunger safe-line sample gap, summary-marker evidence risk, and settlement screenshot pause cleanup gap. Addressed in B8. |
| `019f00cf-590b-79d2-804c-6d0e3fb375f4` / Euler | B8 DreamEvent route-flow勘察 | DreamEvent catnip selection path, pending modifier consumption, next-battle config evidence | Complete; confirmed default route reaches DreamEvent but default reward is clear-notifications, and B8 must lock DreamEvent before selecting reward slot 2 and verify battle-start config after pending consumption. |
| `019f00db-d30c-7710-b604-f5970b79e02c` / Faraday | B8/B9 route-flow evidence precision review | DreamEvent selected reward identity, pending modifier source precision, structure-backed route/defeat flags | Complete; no P1/P2 issue. Found two P3 precision risks: DreamEvent smoke should assert selected `dream_event_catnip_residue`, and modifier evidence should require one exact source. Addressed in B9. |

| `019f00ed-ce93-75d0-aa64-598b03ce588e` / Pauli | B11 starter-cat docs stale-wording review | Current blockers, runtime packet, validation backlog, and asset ledgers | Complete; confirmed formal starter-cat import remains blocked but old `0/3 screenshots`, `3/10 screenshots`, and `offline not green` wording should be retired from current control surfaces. Addressed in B11. |
| `019f00ed-91d8-7f11-8a21-c183f6ab9633` / Pascal | B11 starter-cat formal gate review | Formal import gate, review packet generator, tests, strict candidate boundary | Complete; confirmed legacy missing-screenshot notes should be invalid, current strict block phrase should remain, and next-batch work must remain outside `Assets`. Addressed in B11. |
| `019f00f9-ed03-7602-bfe1-0fe541195b39` / Hume | B12 gameplay/session boundary review | Dream-map catalog, route catalog, run session, cat-room presenter/controller | Complete; flagged roster-reset risk from restarting the Bedroom run and confirmed Egypt must stay disabled/no-target. Addressed in B12 with `EnsureBedroomDreamRun()`. |
| `019f00fa-1ca2-7bd3-ab45-a7fd258b8115` / Locke | B12 UI/docs boundary review | Cat-room copy, Egypt placeholder wording, task/docs state | Complete; flagged stale copy, mojibake visible title, and Egypt appearing like an unlockable route. Addressed in B12 with updated copy and disabled placeholder labeling. |
| `019f010a-38cb-7fa3-a19d-3c3d3f389596` / McClintock | B14 cat-room dream-choice readability review | `02-cat-room.png`, `CatRoomController`, dream-choice presenter copy | Complete; flagged weak status hierarchy, dream choices being separated from actions, and repeated long Egypt placeholder copy. Addressed in B14 with local presentation/copy changes only. |
| `019f0116-8345-7421-a886-e00e8b4fd62e` / Singer | B15 main-menu UI boundary review | `MainMenuController`, `P0MainMenuPresenter`, `P0MainMenuCoverageTests` | Complete; confirmed the slice should stay local to IMGUI presentation/tests and preserve D1 action ids, categories, targets, and `StartP0Run()` defaults. |
| `019f0116-b53b-7c60-b0d1-bdbbfce6673f` / Huygens | B15 validation boundary review | main-menu/readiness coverage, screenshot smoke, Play Mode evidence report | Complete; confirmed Play Mode smoke does not depend on button coordinates and recommended coverage plus refreshed normal Editor screenshot evidence. |
| `019f0122-472f-7d51-85e2-f219e5f71c35` / Gauss | B16 route-map code-boundary review | `RouteMapController`, `P0RouteMapPresenter`, route-map surface coverage/tests | Complete; confirmed B16 should stay in route-map presentation plus surface assertions and preserve route state, action ids, and command/router contracts. |
| `019f0122-5b4e-7bc0-b25d-b587a525e71a` / Einstein | B16 route-map validation-boundary review | screenshot smoke, route-flow smoke, defeat-flow smoke, evidence runner | Complete; confirmed route-map validation should remain command/surface driven and avoid coordinate-click dependencies. |
| `019f012e-1aa9-78f0-b33c-4e8a070b5079` / Turing | B17 battle-result code-boundary review | `GrayboxBattleController`, `P0BattleResultPresenter`, battle-result coverage/tests | Complete; confirmed B17 should stay in result presentation plus surface assertions, preserving action ids, route completion, rewards, and post-battle route-map semantics. |
| `019f012e-2eca-71f1-ab89-95c21b29c7e7` / Socrates | B17 battle-result validation-boundary review | screenshot smoke, route-flow smoke, defeat-flow smoke, evidence runner | Complete; confirmed validation should remain command/surface driven, with visual inspection focused on `10-battle-result-layer1.png` and `11-settlement.png`. |
| `019f013f-0084-7693-a9ba-031b86cdba6c` / Chandrasekhar | B18 route-settlement code-boundary review | `RouteMapController`, `P0RouteMapPresenter`, `P0SettlementPresenter`, route-map surface coverage/tests | Complete; confirmed B18 should stay in completed-route presentation plus surface assertions, preserving `return_cat_room`, route completion, and cat-room return semantics. |
| `019f013f-2a7c-7de1-8a67-cabffb747707` / Ohm | B18 route-settlement validation-boundary review | screenshot smoke, route-flow smoke, defeat-flow smoke, offline reports/tests | Complete; confirmed validation should remain action/surface driven, avoid coordinate clicks, and visually inspect `11-settlement.png` plus B17 screenshot regression. |
| `019f0165-8e13-7321-8c3b-f538dfcf0b89` / Kierkegaard | B19 demo-readiness architecture-boundary review | `P0ArchitectureCompletionAudit`, `P0VisualAcceptanceReport`, focused tests | Complete; confirmed B19 should add an explicit current-demo readiness middle state and must not set final runtime or final visual acceptance true. |
| `019f0165-cf07-7870-ba16-dfa253bdd58e` / Mendel | B19 demo-readiness validation-boundary review | `P0PlayModeReportFileEvidence`, screenshot/report evidence, formal gates | Complete; confirmed parser evidence should require structural report and screenshot anchors while starter-cat and formal install gates remain blocked. |
| `019f0171-b5cb-7ec3-a898-43dde2844083` / Newton | B20 release/readiness architecture review | B19 gates, formal install boundary, task graph | Complete; confirmed B19 boundary is clear and recommended Batch 87 as the future narrow single-lane formal-install hardening target. |
| `019f0171-c9e0-7e91-b70f-6df9a18fa566` / Bernoulli | B20 asset/formal-install blocker review | Batch 83-90 evidence, starter-cat gate, ledgers | Complete; recommended a unified formal-install blocker matrix before any single-lane install decision. |
| `019f0172-7318-7012-9800-fd1a7f548760` / Averroes | B20 validation/Console review | offline/Play Mode evidence, runtime validation plan, recent logs | Complete; recommended shared Console/log classification and runtime-evidence report binding before release packets or formal install claims. |
| `019f0199-c66f-7c40-832f-1411393913f8` / Poincare | B24 validation-boundary review | Batch 87, B21-B23 classifier/formal gates, logs | Complete; recommended Batch 87 as hardening lane only, requiring strict-clean Console and preserving no formal install approval. |
| `019f0199-f71c-71e3-ac0d-15c0fe0e1eb3` / Descartes | B24 asset/design evidence review | Batch 87 candidate packet, runtime evidence, reports | Complete; confirmed Batch 87 is suitable for hardening but still lacks clean Console, explicit human approval, and formal runtime binding decision. |
| `019f019a-21fa-7ff0-8450-28fdba8a028a` / Linnaeus | B24 implementation-scope review | Batch 87 gate/tests/reports and formal matrix | Complete; recommended splitting evidence-complete state from `FormalInstallAllowed` so synthetic 8/8 evidence cannot auto-approve install. |
| `019f01c0-8666-7de2-83ab-bde48318288b` / Darwin | B27 Egypt gameplay boundary review | Dream-map catalog, run session, cat-room, route-map/battle routing | Complete; recommended Egypt minimum shared-route entry through a dedicated cat-room action, preserving Bedroom default and not touching route/battle/reward rules. |
| `019f01c0-b7c5-7b92-9e51-d73a85d1f95a` / Carson | B27 Egypt UI/validation boundary review | Cat-room copy/actions, playable readiness, Play Mode evidence boundary | Complete; recommended changing Egypt from disabled placeholder to minimum shared-route entry while keeping formal-install gates untouched; refreshed Egypt-entry route-map, battle-HUD, first-result, and route-map-return Play Mode evidence has since landed. |
| `019f01ec-110e-7341-b28e-86a6b87248ce` / Meitner | B27 result/return gameplay semantics review | Egypt first-result and route-map-return shared-route boundary | Complete; confirmed the cat-room -> route-map -> battle -> result -> route-map-return path is safe when described as Egypt context over shared route rules; flagged shared Bedroom node naming and restart/new-run Bedroom reset as wording/future-work boundaries. |
| `019f01ec-48a6-7090-b2b4-4948ef0ed356` / Leibniz | B27 result/return validation evidence review | Egypt 5-shot smoke, report, screenshots, and runner boundary | Complete; confirmed dedicated Egypt smoke should stay separate from default 11-shot acceptance, requires 5/5 screenshot evidence, and must not imply clean Console, human approval, formal install, or Egypt-specific content. |

B21 directly implemented Averroes' completed validation/Console recommendation as
a narrow main-session slice. No new read-only agent was dispatched because the
scope was limited to classifier structure, focused assertions, offline
acceptance, and docs; the next report-binding or formal-install lane migration
should use a fresh validation-boundary review.

B22 continued the same narrow validation-infrastructure lane by migrating Batch
83-90 preflight clean-log checks to the shared classifier and retiring local
token tables. No new read-only agent was dispatched; the next generated-report
binding or Batch 87 formal-install hardening slice should use a fresh
validation-boundary review.

B23 completed the generated-report binding part of that recommendation by
adding the shared strict-clean Console classifier contract and policy to the
formal-install matrix/offline report. No new read-only agent was dispatched
because the slice stayed inside an existing validation-infrastructure thread.
The next actual formal-install lane should use a fresh validation-boundary
review before narrowing to Batch 87 or another candidate lane.

B24 used that fresh review gate and implemented the smallest Batch 87 precision
slice: `UnityEvidenceComplete` is separate from `FormalInstallAllowed`, and the
Batch 87 report now exposes strict-clean Console policy plus the missing formal
runtime binding decision. No formal install, clean Console report, or human
approval file was created.

B25 applied those B24 review boundaries to the reviewer handoff: the Batch 87
preflight runner now generates
`BATCH87_BATTLE_HUD_HUMAN_REVIEW_REQUEST_2026-06-26.md` as a request packet
only. No new agent was dispatched because the task was a direct boundary
implementation from the completed Poincare/Descartes/Linnaeus findings; no
approval file, clean Console report, or formal runtime binding was created.

B26 completes the matching runtime-binding handoff: the Batch 87 preflight
runner now generates
`BATCH87_BATTLE_HUD_FORMAL_RUNTIME_BINDING_DECISION_REQUEST_2026-06-26.md` as a
decision-request packet only. No new agent was dispatched because the slice
does not change code ownership or runtime bindings; it preserves the same
completed review boundaries and leaves catalog/scene/presenter changes blocked.

B27 used two read-only agents before implementation. Darwin covered the
gameplay/runtime boundary and Carson covered UI/validation evidence. The main
session implemented only the agreed minimum shared-route Egypt entry:
`enter_egypt_dream`, `EnsureEgyptDreamRun`, Egypt playable map status, and
focused readiness/coverage tests. A follow-up dedicated normal Editor Play Mode
Egypt-entry smoke now passes through route-map layer one, battle HUD load, first
shared-route battle result, and route-map return with 5/5 screenshots, visible
Egypt dream-map context, verified Egypt battle-start context, and progress
`1/10` after `ContinueRoute`. Meitner and Leibniz reviewed the result/return
boundary after implementation: the evidence is valid only as Egypt context over
shared route/combat/reward rules, and the result screenshot should be read with
the detailed log plus `05-egypt-route-map-after-layer1.png` because shared
Bedroom node naming remains visible. Batchmode visual capture is documented as
unsupported for this smoke and fails safely. Route/battle/reward rules, formal
install gates, candidate bindings, clean Console, human approval, full Egypt
route settlement/cat-room return, and Egypt-specific content remain untouched.

These agents are read-only. No implementation worker should start until their
findings are merged into the current task graph or explicitly bypassed with a
documented reason.

## 2. Workstreams

| Stream | Goal | Primary files | Gate |
|-|-|-|-|
| A. Source and architecture | Keep implementation aligned to live `Qr1`, local `MDr`, blocked `IZp` | `P0_DEVELOPMENT_ARCHITECTURE_2026-06-25.md`, blockers log, README | G0/G1 |
| B. Cat room hub | Add missing live-source hub loop | New cat-room runtime/presenter/scene files, `P0SceneFlow`, `P0RunSession` | G2 |
| C. Dream map themes | Represent bedroom and Egypt as map contexts | Data/catalogs, battle start context, route nodes, visual bindings | G3 |
| D. Opening/UI completion | Make all P0 screens reviewable | Presenters/controllers for entry, character select, pause/settings, upgrade, settlement | G4 |
| E. Battle polish | Preserve and polish current bedroom route/battle | Existing battle, route, skill, enemy, boss, core-value files | G4/G6 |
| F. Asset validation | Move only future-approved install rows into runtime after Unity evidence | Asset manifests, review packets, Unity validation checklist | G5 |
| G. QA and release gate | Prove demo completion | Offline reports, Play Mode smoke, screenshots, Console, independent review | G6 |

## 3. Near-Term Task Graph

| Id | Task | Depends on | Write scope | Review |
|-|-|-|-|-|
| A1 | Merge agent findings into current architecture docs | Active read-only agents | `design/development/*.md` only | Documentation review |
| B1 | Add cat-room domain model and presenter | A1 | `Assets/TheCat/Scripts/Runtime/Gameplay`, `Assets/TheCat/Scripts/Runtime/Roguelite`, tests | Code + gameplay review |
| B2 | Add cat-room scene flow and placeholder scene | B1 | `Assets/TheCat/Scenes`, `ProjectSettings/EditorBuildSettings.asset`, scene controller files | Unity integration review |
| B3 | Add return-to-cat-room result handoff | B1/B2 | `P0RunSession`, route/battle result flow, presenter tests | Code review |
| C1 | Add dream map/theme data layer | A1 | Data definitions/catalogs/tests | Design consistency review |
| C2 | Route bedroom/Egypt map context into battle start | C1 | `P0BattleStartContext`, `P0RouteCatalog`, `P0PrototypeCatalog`, battle controller | Gameplay review |
| C3 | Add Egypt placeholder readiness gate (completed 2026-06-25) | C1/C2 | Tools/readiness/checklist docs | Validation review |
| C4 | Enable Egypt minimum shared-route entry (completed through first-result / route-map-return Play Mode evidence 2026-06-26) | C3/B12 | Dream-map catalog, run session, cat-room action, route-map/readiness tests, dedicated Egypt-entry smoke | Darwin/Carson plus Meitner/Leibniz review complete; full Egypt route/cat-room return and formal gates remain future slices |
| D1 | Add entry/character-select UI presenter gap checks (completed 2026-06-25) | A1/C3 | Presenters/tools/tests | UI review |
| D2 | Add pause/settings and skill-selection acceptance coverage (completed 2026-06-25) | D1 | Runtime settings/skill-selection presenters/tools/tests | UI + validation review |
| E1 | Polish bedroom battle readability while preserving rules (completed 2026-06-25) | D2 | Existing battle HUD/presenter files | UI + validation review |
| F1 | Review Batch 83-88 UI candidate packs for import order (completed docs-only 2026-06-25) | Tesla output/E1 | Docs/checklists only | Asset gate review |
| G1 | Capture fresh 11-capture Play Mode evidence baseline (completed 2026-06-25) | E1/F1/H1 | reports/screenshots/logs only | QA + design review complete; I1 resolved the queued battle-world label debt |
| H1 | Add loading/start and full settings screenshot hooks (completed 2026-06-25) | F1 runtime-surface mapping | presenter/controller/tests only | UI code review complete; screenshot/import evidence pending |
| I1 | Fix battle-world label safe-area and result overlay hierarchy (completed 2026-06-25) | G1 | battle view gates, smoke runners, tests, reports/screenshots | Screenshot + code-boundary review complete; no candidate import approval |
| J1 | Triage Batch 87 battle-HUD Unity evidence entry (completed 2026-06-25) | I1/F1 | docs/evidence packet only | Euclid/Hubble review complete; candidate-backed Unity validation still pending |

## 4. First Worker Prompt Candidate

Use this only after the read-only agents finish and the main session confirms
the scope.

```text
You are a worker agent for TheCat P0 cat-room architecture.
You are not alone in this codebase. Do not revert edits made by others. Keep all writes inside your assigned scope and adapt to existing changes.

Task scope:
Add the first code-level cat-room hub contract without replacing the current menu -> route -> battle implementation. The goal is a testable state/presenter layer and scene-flow hook that can later back a Unity scene.

Relevant design docs:
- D:\Unity Workspace\TheCat\design\development\P0_DEVELOPMENT_ARCHITECTURE_2026-06-25.md
- D:\Unity Workspace\TheCat\tmp\feishu_source_2026-06-25\01_qr1_game_master_plan.md
- D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\docs\01_core_gameplay\core_gameplay_rules_and_player_path.md

Code files to read:
- Assets/TheCat/Scripts/Runtime/Gameplay/P0SceneFlow.cs
- Assets/TheCat/Scripts/Runtime/Gameplay/MainMenuController.cs
- Assets/TheCat/Scripts/Runtime/Roguelite/P0RunSession.cs
- Assets/TheCat/Scripts/Runtime/Gameplay/P0MainMenuPresenter.cs
- Assets/TheCat/Tests/EditMode

Write scope:
- Assets/TheCat/Scripts/Runtime/Gameplay/*CatRoom*.cs
- Assets/TheCat/Scripts/Runtime/Roguelite/*CatRoom*.cs if needed
- Assets/TheCat/Tests/EditMode/*CatRoom*.cs
- Minimal additions to P0SceneFlow/P0RunSession only if necessary

Forbidden scope:
- Do not edit asset candidates.
- Do not import art.
- Do not replace current route/battle controllers.
- Do not edit ProjectSettings in this slice.

Expected output:
- Cat-room state/presenter contract for bed, feeder, litter box, dream entrance, and return feedback.
- Focused EditMode tests.
- Changed file list and validation result.
```

## 5. Review Cadence

Run a review agent at these points:

- After B1: code architecture and design consistency.
- After B2/B3: Unity scene flow and player loop review.
- After C2: gameplay review for bedroom/Egypt split.
- After D1: entry/character-select review completed.
- After E1: battle-readability review completed; next review should target
  candidate-asset import gating or fresh Play Mode visual evidence for the new
  battle brief.
- Before any F1-derived import approval: run the batch-specific Unity screenshot,
  import-settings, binding, and Console gate.
- After H1: Batch 83 and Batch 85 are hook-ready only. Do not treat the
  `33/33` EditMode hook pass as Unity screenshot/import acceptance.
- After G1: treat the 11-capture Play Mode report as baseline screenshot smoke
  only. It does not approve candidate import, runtime binding, install rows, or
  final visual polish. I1 has since resolved the battle world label safe-area /
  overlay hierarchy debt for the current baseline.
- After I1: Batch 87 may proceed to batch-specific screenshot parity,
  import-settings, binding, and Console evidence. Do not treat I1 as Batch 87
  candidate import approval.
- After K1: Batch 87 has candidate-only Unity preflight imports with Sprite
  settings and focused EditMode coverage. Do not treat K1 as formal runtime
  binding, clean Console, screenshot parity, or human approval.
- Before G6 claim: full release-gate review covering design, code, UI, assets,
  validation, and blockers.

Reviewers should treat `IZpFdIwtboEzzrx4ZFlcZLD2npe` as unknown until access is
granted. Any claim based on it is invalid.
