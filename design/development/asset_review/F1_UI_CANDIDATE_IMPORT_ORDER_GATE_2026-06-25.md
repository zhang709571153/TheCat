# F1 UI Candidate Import Order Gate - 2026-06-25

Status: docs-only import-order gate complete for Batch 83-88; zero import or
acceptance performed.

This gate reviews the local UI candidate packets that are already candidate
complete but not Unity accepted. It does not approve import, binding, runtime
use, or starter-cat body-art replacement.

## Scope

Included in F1:

- Batch 83 loading/start.
- Batch 84 result/settlement.
- Batch 85 settings/pause.
- Batch 86 dream-route.
- Batch 87 battle HUD.
- Batch 88 character-select.

Not included in F1:

- Batch 89 skill-selection: still candidate-only, but it belongs to the next
  skill/upgrade UI evidence pass.
- Batch 90 cat-room: still candidate-only, but it belongs to the next cat-room
  UI evidence pass.

The broader asset master ledger may keep Batch 83-90 together as the next
Unity-evidence queue. F1 is narrower: it decides the Batch 83-88 import-review
order and records the remaining evidence required before any install decision.

## Agent Inputs

| Agent | Scope | Result |
|-|-|-|
| `019efcc0-8a6a-7512-a113-76627e49a01d` / Mendel | Visual priority and player-impact order | Complete; recommended 88, 87, 86, 84, 85, 83 for review priority, with all six waiting on fresh Play Mode screenshots. |
| `019efcc0-b5e4-7683-bf6d-2f8469dd8f19` / Bernoulli | Queue/checklist and production-validation gate | Complete; confirmed docs-only F1 evidence is sufficient, but Unity acceptance/import approval is not. |
| `019efcc2-ddb7-7b12-984a-82a8fbcd1f91` / Hypatia | Runtime surface mapping | Complete; at F1 time, 87/88/86/84 were ready for screenshot parity review, 85 was partial, and 83 needed a loading/start surface hook. H1 has since added the 83/85 hooks without approving import. |
| `019efcd3-65be-73e1-ac19-d1996eb14ee4` / James | H1 loading/start hook review | Complete; no P0 issue. Confirmed the Batch 83 hook is preflight/readiness only and must not replace Unity-rendered screenshot/import/Console evidence. |
| `019efcd3-84b6-7073-92fe-25116088e48b` / Carson | H1 full settings hook review | Complete; no P0 issue. Recommended semantic non-speed rows and deep candidate-token scanning, both addressed in H1 review-fix tests. |
| `019efcd3-9a22-74c1-bd9f-7f6ca1764a94` / Hilbert | H1 readiness/screenshot-smoke review | Complete; no P0 issue. Confirmed the 11-capture screenshot plan did not change and H1 must be documented as code-hook only. |
| `019efced-ab4b-79b3-a73c-e72197380044` / Noether | G1 screenshot visual review | Complete; no P0 issue. The 11-capture baseline supports G1 closure; the battle world label/overlay P1 debt identified here was later resolved by I1. |
| `019efced-bf9a-73e1-be72-0cc2c42c5268` / Mencius | G1 evidence-chain review | Complete; retry Play Mode acceptance is valid G1 evidence, the first screenshot-evidence OOM is superseded, and full acceptance remains not evaluated. |
| `019efced-d3b1-77f0-b7e1-6524e9024201` / Locke | G1/F1/H1 boundary review | Complete; no P0 issue. G1 must stay documented as 11-capture Play Mode evidence refresh only, with no import or final art acceptance. |
| `019efee8-04fd-75b2-84a4-950e68f113f8` / Nietzsche | I1 screenshot visual review | Complete; no P0/P1 issue. Battle world labels are hidden in normal screenshots, warning VFX remains readable, and result overlay is clear of active warning lines/labels. |
| `019efee8-1980-7933-8928-ee2882547222` / Sartre | I1 code/validation boundary review | Complete; no P0 issue. Diagnostic gates, warning-shape preservation, smoke-runner timing, and material lifecycle are within I1 scope after final fixes. |
| `019efefd-6078-7dd1-b67d-79889d3c7e7d` / Euclid | J1 Batch 87 visual/evidence-chain review | Complete; no P0. Confirms J1 keeps current I1 screenshots as baseline-only and keeps 1024x768 four-gauge proof limited to local mockup evidence. |
| `019efefd-74aa-76e0-b2cd-924f52a7c1d3` / Hubble | J1 Batch 87 gate/boundary review | Complete; no P0/P1. Confirms candidate-only/no-import boundary and recommends candidate-backed wording, now applied. |

## Import-Review Order

1. Batch 88 character-select.
2. Batch 87 battle HUD.
3. Batch 86 dream-route.
4. Batch 84 result/settlement.
5. Batch 85 settings/pause.
6. Batch 83 loading/start.

Rationale:

- Batch 88 is the highest first-screen identity surface after entry and touches
  starter identity through HUD avatars.
- Batch 87 is the highest in-run gameplay surface and already had a 1024x768
  four-gauge issue fixed in local mockups.
- Batch 86 controls route-state semantics and core navigation.
- Batch 84 closes the loop, but should follow battle HUD and route readability.
- Batch 85 is important for player control but lower than the core loop
  screens.
- Batch 83 is polish and first impression, but lowest functional impact.

## Required Unity Evidence

| Batch | Required before any import decision |
|-|-|
| 83 loading/start | M3 already covers four Unity-rendered loading/start screenshots, spinner non-character proof, placeholder text/state replacement, 1024x768 crowding check, progress/click-target review, and Sprite import settings. Remaining before any formal import decision: scene/presenter binding proof, clean Console, explicit human approval, and formal runtime binding decision. |
| 84 result/settlement | M4 already covers Unity victory/defeat/run-cleared/run-failed screenshots, Chinese text/number/button replacement, 1024x768 run-failed crowding, failure-stamp semantics, outcome/action review, and Sprite import settings. Remaining before any formal import decision: scene/presenter binding proof, clean Console, explicit human approval, and formal runtime binding decision. |
| 85 settings/pause | M5 already covers Unity settings main, settings audio, pause overlay, and compact settings screenshots, Chinese text/value replacement, compact key-hint semantics, tab/key-hint/control/pause-action click-target proof, and Sprite import settings. Remaining before any formal import decision: scene/presenter binding proof, clean Console, explicit human approval, and formal runtime binding decision. |
| 86 dream-route | Candidate-backed Unity dream-entry/route-map screenshots, Chinese route/reward text replacement, current/selected/available/locked/Boss node-path semantics, 1024x768 route-card density, boss-gate scale, route-card click targets, Sprite import settings, scene/presenter binding proof, clean Console, explicit human approval. |
| 87 battle HUD | L1/M2 already covers Batch 87 candidate-backed Unity battle HUD screenshots, Chinese labels/tooltips/text/number replacement, 1024x768 four-gauge proof, skill-state readability, enemy/telegraph occlusion proof, click targets, Sprite import settings, and fallback-free candidate draws. Remaining before any formal import decision: clean Console, explicit human approval, and formal runtime binding decision. |
| 88 character-select | L2 already covers Unity character-select screenshots, source-lock HUD avatar consistency, Chinese names/roles/descriptions/start labels, 1280x720 and 1024x768 density proof, selected/idle distinction, starter-card/start click targets, Sprite import settings, and fallback-free candidate draws. Remaining before any formal import decision: scene/presenter binding proof, clean Console, explicit human approval, and formal runtime binding decision. |

## Runtime Surface Readiness

| Batch | Runtime surface state | Action |
|-|-|-|
| 87 battle HUD | Candidate-backed runtime evidence is now `6/8`. Baseline screenshot refreshed in G1 and cleaned in I1; K1 proves candidate-only Unity preflight import/settings/binding-leak checks; L1/M2 captured four Unity-rendered battle HUD screenshots, verified 6/6 candidate frame draws with fallback=0, and wrote text/skill-state plus telegraph/occlusion/click-target evidence. | Finish clean Console proof, explicit human approval, and formal runtime binding decision before any Batch 87 formal install decision. |
| 88 character-select | Candidate-backed runtime evidence is now `6/8`. Baseline main-menu/starter screenshots refreshed in G1 through D1 main-menu and starter-card contracts; K2 imported the six candidate sprites for candidate-only Unity preflight; L2 captured four Unity-rendered character-select screenshots, source-lock avatar consistency, selected/idle states, Chinese text/density, and card/start click-target evidence. | Finish scene/presenter binding proof, clean Console, explicit human approval, and formal runtime binding decision before any Batch 88 formal install decision. |
| 86 dream-route | Baseline route-map screenshot refreshed in G1. `RouteMapController` and `P0RouteMapPresenter` cover map context, route options, reward cards, boss state, and cat-upgrade choices. K8 imports the six Batch86 component sprites into `Assets/TheCat/Art/UI/DreamRoute` for candidate-only Unity preflight, validates 6/6 P0 Sprite import settings, confirms 0 formal runtime binding leaks, and hardens screenshot/runtime-log gates. L5 captures four candidate-backed Unity screenshots, confirms 6/6 candidate frame draws with fallback=0, and writes text/reward, node/path semantics, route-card density, boss-gate scale, and click-target reviews. | Finish scene/presenter binding, clean Console, explicit human approval, and formal runtime binding decision before any Batch 86 formal install decision. |
| 84 result/settlement | Candidate-backed runtime evidence is now `6/8`. Baseline battle-result and settlement screenshots refreshed in G1; `P0BattleResultPresenter`, `P0SettlementPresenter`, and route-map settlement focus/details exist; K6 imported seven candidate sprites for candidate-only Unity preflight; M4 captured four Unity-rendered result/settlement screenshots and wrote battle/result state, reward/stat readability, and click-target reviews without approving formal runtime binding. | Finish scene/presenter binding proof, clean Console, explicit human approval, and formal runtime binding decision before any Batch 84 formal install decision. |
| 85 settings/pause | Candidate-backed runtime evidence is now `6/8`. D2 covers pause overlay, runtime speed controls, and restart confirmation; H1 adds a full settings panel/tab/option-row surface with semantic non-speed rows and deep candidate-token scanning; K7 imported the six candidate sprites into `Assets/TheCat/Art/UI/SettingsPause`; M5 captured four Unity-rendered settings/pause screenshots and wrote text/key-hint plus settings-control/click-target reviews without approving formal runtime binding. | Finish scene/presenter binding proof, clean Console, explicit human approval, and formal runtime binding decision before any Batch 85 formal install decision. |
| 83 loading/start | Candidate-backed runtime evidence is now `6/8`. H1 adds a dedicated loading/start presenter with target, progress, spinner, detail rows, screenshot-hook proof, and deep candidate-token scanning. K5 imported the four candidate sprites for candidate-only Unity preflight and validated import settings. M3 captured four Unity-rendered loading/start screenshots plus spinner/placeholder and progress/click-target reviews, without approving formal runtime binding. | Finish scene/presenter binding proof, clean Console, explicit human approval, and formal runtime binding decision before any Batch 83 formal install decision. |

## Decision

- F1 passes as a docs-only candidate import-order gate.
- Batch 83-88 stay `CandidatePackCompletePendingUnityReview` unless a later
  batch-specific preflight explicitly records a candidate-only Unity import
  boundary.
- F1 itself copied no Batch 83-88 PNGs into `Assets/`; later candidate-only
  preflight slices copied Batch 83 loading/start, Batch 84 result/settlement,
  Batch 85 settings/pause, Batch 86 dream-route, and Batch 87 HUD components into their declared
  Unity preflight roots, not formal runtime assets.
- No `.meta` files should be generated in candidate folders.
- No formal install row should be written until the matching Unity screenshot,
  import-settings, binding, and Console evidence exists.
- Starter-cat body art remains locked behind active-cat screenshot comparison
  against the colored three-view turnarounds.

## Follow-Up Queue

1. Finish Batch 87 clean Console proof, explicit human approval, and formal
   runtime binding decision from the L1/M2 candidate-backed runtime evidence
   baseline.
2. Finish Batch 88 scene/presenter binding proof, clean Console, explicit human
   approval, and formal runtime binding decision from the L2 candidate-backed
   runtime evidence baseline.
3. Finish Batch 86 runtime evidence gates from the K8/L5 candidate-only Unity
   preflight baseline: scene/presenter binding, clean Console, explicit human
   approval, and formal runtime binding decision. L5 already covers
   dream-entry/route-map screenshots, text/reward replacement,
   current/selected/available/locked/Boss node/path semantics, route-card
   density, boss-gate scale, click targets, and runtime evidence report.
4. Finish Batch 84 battle-result and settlement evidence gates from the K6/M4
   candidate-backed runtime evidence baseline: scene/presenter binding, clean
   Console, explicit human approval, and formal runtime binding decision. M4
   already covers four result/settlement screenshots, text/reward readability,
   outcome/action review, and runtime evidence report.
5. Finish Batch 85 settings/pause evidence gates from the K7/M5
   candidate-backed runtime evidence baseline: scene/presenter binding, clean
   Console, explicit human approval, and formal runtime binding decision. M5
   already covers settings-main/settings-audio, pause-overlay, compact
   screenshots, text/key-hint readability, controls/click-target review, and
   runtime evidence report.
6. Finish Batch 83 loading/start evidence gates from the K5/M3
   candidate-backed Unity preflight baseline: scene/presenter binding, clean
   Console, explicit human approval, and formal runtime binding decision. M3
   already covers the four loading/start screenshots, spinner/placeholder
   review, progress/click-target review, Sprite import settings, and runtime
   evidence report.
7. Keep Batch 89 and Batch 90 in the broader Unity-evidence queue, but route
   their import gates through the next skill-selection and cat-room UI passes.
