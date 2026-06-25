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
| `019efced-ab4b-79b3-a73c-e72197380044` / Noether | G1 screenshot visual review | Complete; no P0 issue. The 11-capture baseline supports G1 closure, but battle world label safe-area / overlay hierarchy remains P1 visual debt. |
| `019efced-bf9a-73e1-be72-0cc2c42c5268` / Mencius | G1 evidence-chain review | Complete; retry Play Mode acceptance is valid G1 evidence, the first screenshot-evidence OOM is superseded, and full acceptance remains not evaluated. |
| `019efced-d3b1-77f0-b7e1-6524e9024201` / Locke | G1/F1/H1 boundary review | Complete; no P0 issue. G1 must stay documented as 11-capture Play Mode evidence refresh only, with no import or final art acceptance. |

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
| 83 loading/start | Unity-rendered loading/start screenshots, spinner non-character proof, placeholder text/state replacement, 1024x768 crowding check, Sprite import settings, UI binding proof, clean Console. |
| 84 result/settlement | Unity victory/defeat/run-cleared/run-failed screenshots, Chinese text/number/button replacement, 1024x768 run-failed crowding, failure-stamp semantics, Sprite import settings, binding proof, clean Console. |
| 85 settings/pause | Unity settings/pause screenshots, Chinese text/value replacement, compact key-hint semantics, tab/close/back/key-hint/slider/switch/checkbox click-target proof, Sprite import settings, binding proof, clean Console. |
| 86 dream-route | Unity dream-entry/route-map screenshots, Chinese route/reward text replacement, current/selected/available/locked/Boss node-path semantics, 1024x768 route-card density, boss-gate scale, route-card click targets, Sprite import settings, binding proof, clean Console. |
| 87 battle HUD | Unity battle HUD screenshots, Chinese labels/tooltips/text/number replacement, 1024x768 four-gauge proof, skill-state readability, enemy/telegraph occlusion proof, pause/speed/restart click targets, Sprite import settings, binding proof, clean Console. |
| 88 character-select | Unity character-select screenshots, source-lock HUD avatar consistency, Chinese names/roles/descriptions/start labels, 1280x720 and 1024x768 density proof, selected/idle distinction, starter-card/start click targets, Sprite import settings, binding proof, clean Console. |

## Runtime Surface Readiness

| Batch | Runtime surface state | Action |
|-|-|-|
| 87 battle HUD | Baseline screenshot refreshed in G1. E1 added the default player brief, battle HUD sections, threat/status/cat/skill/action surfaces, and surfaced result actions. | Fix/review battle world label safe-area and overlay hierarchy before final visual acceptance or import approval. |
| 88 character-select | Baseline main-menu/starter screenshots refreshed in G1 through D1 main-menu and starter-card contracts. | Compare against source-locked avatar refs and capture batch-specific import/binding/Console proof before any import decision. |
| 86 dream-route | Baseline route-map screenshot refreshed in G1. `RouteMapController` and `P0RouteMapPresenter` cover map context, route options, reward cards, boss state, and cat-upgrade choices. | Continue route-state parity review with current/selected/available/locked/Boss semantics before any import decision. |
| 84 result/settlement | Baseline battle-result and settlement screenshots refreshed in G1. `P0BattleResultPresenter`, `P0SettlementPresenter`, and route-map settlement focus/details exist. | Review dense settlement route history and result overlay hierarchy before any import decision. |
| 85 settings/pause | Hook-ready for screenshot review. D2 covers pause overlay, runtime speed controls, and restart confirmation; H1 adds a full settings panel/tab/option-row surface with semantic non-speed rows and deep candidate-token scanning. | Capture Unity-rendered full settings screenshots, import settings, binding proof, and Console evidence before any Batch 85 import decision. |
| 83 loading/start | Hook-ready for screenshot review. H1 adds a dedicated loading/start presenter with target, progress, spinner, detail rows, screenshot-hook proof, and deep candidate-token scanning. | Capture Unity-rendered loading/start screenshots, spinner/state proof, import settings, binding proof, and Console evidence before any Batch 83 import decision. |

## Decision

- F1 passes as a docs-only candidate import-order gate.
- Batch 83-88 stay `CandidatePackCompletePendingUnityReview`.
- No Batch 83-88 PNGs should be copied into `Assets/` from this gate.
- No `.meta` files should be generated in candidate folders.
- No formal install row should be written until the matching Unity screenshot,
  import-settings, binding, and Console evidence exists.
- Starter-cat body art remains locked behind active-cat screenshot comparison
  against the colored three-view turnarounds.

## Follow-Up Queue

1. Address Batch 87 battle HUD / battle world label safe-area and overlay
   hierarchy before final visual acceptance.
2. Run Batch 88 character-select/main-menu source-lock avatar consistency
   proof and batch-specific import/binding/Console evidence.
3. Continue Batch 86 route-state screenshot parity with current/selected/
   available/locked/Boss semantics.
4. Continue Batch 84 battle-result and settlement screenshot parity, including
   dense route-history readability.
5. Capture Batch 85 full settings screenshots against the H1 hook before full
   Batch 85 import review; pause overlay screenshots can be reviewed earlier
   as partial evidence.
6. Capture Batch 83 loading/start screenshots against the H1 hook before Batch
   83 import review.
7. Keep Batch 89 and Batch 90 in the broader Unity-evidence queue, but route
   their import gates through the next skill-selection and cat-room UI passes.
