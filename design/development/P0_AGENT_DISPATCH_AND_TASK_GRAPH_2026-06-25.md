# P0 Agent Dispatch And Task Graph - 2026-06-25

Checked: 2026-06-25 12:04 +08:00

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
| `019efced-ab4b-79b3-a73c-e72197380044` / Noether | G1 screenshot visual review | 11-capture screenshot baseline | Complete; no P0 issue. G1 can close as evidence refresh, but battle world labels and overlay hierarchy remain P1 visual debt. |
| `019efced-bf9a-73e1-be72-0cc2c42c5268` / Mencius | G1 evidence-chain review | Play Mode retry, first-run OOM, report boundaries | Complete; retry is valid evidence, first OOM is superseded, full acceptance remains not evaluated. |
| `019efced-d3b1-77f0-b7e1-6524e9024201` / Locke | G1/H1/F1 boundary review | import/final-acceptance wording | Complete; no P0 issue. Unified G1 as 11-capture evidence refresh only, with no import or final art acceptance. |

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
| D1 | Add entry/character-select UI presenter gap checks (completed 2026-06-25) | A1/C3 | Presenters/tools/tests | UI review |
| D2 | Add pause/settings and skill-selection acceptance coverage (completed 2026-06-25) | D1 | Runtime settings/skill-selection presenters/tools/tests | UI + validation review |
| E1 | Polish bedroom battle readability while preserving rules (completed 2026-06-25) | D2 | Existing battle HUD/presenter files | UI + validation review |
| F1 | Review Batch 83-88 UI candidate packs for import order (completed docs-only 2026-06-25) | Tesla output/E1 | Docs/checklists only | Asset gate review |
| G1 | Capture fresh 11-capture Play Mode evidence baseline (completed 2026-06-25) | E1/F1/H1 | reports/screenshots/logs only | QA + design review complete; visual debt queued |
| H1 | Add loading/start and full settings screenshot hooks (completed 2026-06-25) | F1 runtime-surface mapping | presenter/controller/tests only | UI code review complete; screenshot/import evidence pending |

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
  final visual polish. The next battle-HUD visual review should target battle
  world label safe-area / overlay hierarchy.
- Before G6 claim: full release-gate review covering design, code, UI, assets,
  validation, and blockers.

Reviewers should treat `IZpFdIwtboEzzrx4ZFlcZLD2npe` as unknown until access is
granted. Any claim based on it is invalid.
