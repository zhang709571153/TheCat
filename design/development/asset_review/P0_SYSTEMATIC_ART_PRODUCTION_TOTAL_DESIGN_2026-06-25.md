# P0 Systematic Art Production Total Design

Project: `D:\Unity Workspace\TheCat`
Updated: 2026-06-25 21:03 +08:00

This document is the current control surface for P0 art production. It starts with the required asset table, then defines production order, archive rules, image generation policy, review gates, and TODO state.

## Source Status

| Source | Role | Current result | Working rule |
| --- | --- | --- | --- |
| `Qr1XdXd6KosnjMxjgW7cS89kn9c` | UI/style and minimum art boundary truth | [x] Live readable as user `章航宁`, revision `816`; section 9 fetched at 08:03 +08:00 | Use as UI/style authority before every UI or screen batch. |
| `IAdkdcpciobUTXxa7dBcRx7Bngf` | Character design truth | [ ] Live fetch blocked by Feishu `3380004`; local synced document and source-lock packets exist | Use local source-locks; do not claim live refresh; do not generate/import starter-cat bodies. |
| `MDrSdEoaToB5cnxZgrOcAE34nof` | Gameplay/path reference | [ ] Live fetch blocked by Feishu `3380004`; local synced document exists | Use local synced gameplay copy until access is granted. |
| `IZpFdIwtboEzzrx4ZFlcZLD2npe` | Linked Feishu doc | [ ] Live fetch blocked by Feishu `3380004`; no local manifest entry | Do not derive asset requirements from it yet. |
| `HDoWdDNR3oZ6uhxuMzPcT8qCn5f` | Map art archive | [ ] Live fetch blocked by Feishu `3380004`; no local manifest entry | Treat Egypt/map archive gaps as blocked until access is granted. |
| `FoW9fKYcDllwJjdTxGHcu4pbnab` | Drive folder | [ ] Inspect resolves folder token; listing blocked by Drive `1061004` | Do not assume folder asset coverage. |

## Required Asset Table

Legend: `Done` means Codex-side packet/file/check exists. `Unity` means runtime screenshots/import settings/bindings/Console proof have passed.

| ID | Asset lane | Required P0 deliverable | Current state | Next production / validation | Done | Unity |
| --- | --- | --- | --- | --- | --- | --- |
| SRC-01 | Qr1 UI/style truth | Live P0 art boundary and UI surface list | Source ready, revision `816` | Re-fetch if revision changes | [x] | n/a |
| SRC-02 | IAd character truth | Starter cat identity, skills, turnarounds | Local-only source ready; live ACL blocked | Request live access; keep source-locks authoritative | [x] | n/a |
| UI-01 | Entry/loading | Entry screen, logo area, loading/progress primitives | Batch 83 local preflight complete | Unity screenshots, text/state replacement, import/binding/Console | [x] | [ ] |
| UI-02 | Main menu | Main menu usable entry to core flow | Installed primitives/backgrounds exist | Runtime screenshot and Console check | [x] | [ ] |
| UI-03 | Cat room UI | Out-of-combat cat room interface and dream entrance affordance | Batch 90 local preflight complete with 6 transparent sprites and 4 cat-room mockups; Batch 92 built-in imagegen produces 12 textless cat-room interaction-state semantic sprites plus cooldown v003 P1 fix; Play Mode smoke captures existing runtime cat-room surface as `02-cat-room.png` | Batch90/Batch92 candidate sprite import/binding, four-resolution parity screenshots, interaction states, click targets, ring semantic curation, Console | [x] | [ ] |
| UI-04 | Character select | Three starter cards using source-locked avatars, role scan, start action | Batch 88 local preflight complete | Unity screenshots vs source locks; low-height density; click targets | [x] | [ ] |
| UI-05 | Dream entry / route | Dream entry, route nodes, choice cards, boss gate | Batch 86 local preflight complete | Unity route/dream screenshots; node/path semantics | [x] | [ ] |
| UI-06 | Battle HUD | Sleep, cat HP, poop, hunger, skills, pause/speed/restart, enemy status | Batch 87 local preflight complete | Unity battle HUD screenshots; four-gauge proof; telegraph occlusion | [x] | [ ] |
| UI-07 | Pause menu | Pause overlay, resume/speed/restart, key hints | Batch 85 local preflight complete | Unity pause screenshots; key-hint semantics; click targets | [x] | [ ] |
| UI-08 | Settings page | Slider, switch, checkbox, tabs, close/back affordances | Batch 78/79 plus Batch 85 candidates | Unity settings screenshot and pointer behavior proof | [x] | [ ] |
| UI-09 | Skill selection | Skill cards, selected/disabled/locked states, cooldown fit | Batch 89 local preflight complete with 8 transparent sprites and 4 skill-selection mockups | Unity skill-selection screenshots, selected/disabled/locked states, cooldown semantics | [x] | [ ] |
| UI-10 | Victory/defeat/settlement | Battle victory, battle defeat, run clear/fail, reward scan | Batch 84 local preflight complete | Unity screenshots, text/number replacement, low-height crowding | [x] | [ ] |
| UI-11 | Common components | Buttons, panels, cards, tabs, list rows, lock/warning/focus/danger states | Inventory complete; Batch 82 derivatives complete | Screen-priority Unity validation | [x] | [ ] |
| UI-12 | Typography/numbers | Chinese font, damage/reward numbers, cooldown digits, gauge labels | Batch 75 local scale mockups complete | 5 surfaces x 4 resolutions Unity screenshot matrix | [x] | [ ] |
| UI-13 | Role / scene tokens | Static starter role badges, scene entry tokens, ready/selected/locked card frames | Batch 95 built-in imagegen pack complete with 9 transparent sprites, manifest, contact sheet, process note, final review CSV, and three-agent review result PASS_WITH_P2 | Unity import/settings/binding, 64px readability for Suzune badge and scene tokens, selected-vs-ready/locked card state proof, no recursive import from `superseded/`, Console | [x] | [ ] |
| CAT-01 | Saiban body | Runtime sprite, idle/move/attack/skills/ult/hit/death | Source-locked runtime sprite exists; replacement blocked | Active screenshot vs colored turnaround approval only | [x] | [ ] |
| CAT-02 | Nephthys body | Runtime sprite, idle/move/attack/skills/ult/hit/death | Source-locked runtime sprite exists; replacement blocked | Active screenshot vs colored turnaround approval only | [x] | [ ] |
| CAT-03 | Suzune body | Runtime sprite, idle/move/attack/skills/ult/hit/death | Source-locked runtime sprite exists; replacement blocked | Active screenshot vs colored turnaround approval only | [x] | [ ] |
| CAT-04 | Starter cat framesheets | Per starter: idle, walk, normal attack, two small skills, ultimate, hit, death/exit | No promoted framesheet lane; intentionally blocked | Source-lock approval first, then reference-only candidates | [ ] | [ ] |
| CAT-05 | Starter HUD avatars | Source-locked starter portraits/icons | Existing HUD avatars used by Batch 88 | Verify in character-select and battle HUD screenshots | [x] | [ ] |
| SKILL-01 | Saiban symbolic skills | Shield, sword arc, banner, charge, holy shield/crown motifs | Batch 80 symbolic icon candidates; Batch 61 VFX installed | HUD fit and skill-cast screenshots | [x] | [ ] |
| SKILL-02 | Nephthys symbolic skills | Obelisk, quicksand, pyramid, sand missile, storm/tornado motifs | Batch 80 symbolic icon candidates; Batch 61 VFX installed | HUD fit and skill-cast screenshots | [x] | [ ] |
| SKILL-03 | Suzune symbolic skills | Bell, ice talisman, torii, heal, moon/ice motifs | Batch 80 symbolic icon candidates; Batch 61 VFX installed | HUD fit and skill-cast screenshots | [x] | [ ] |
| MAP-01 | Bedroom dream map | Protected bed, four entry sides, obstacles, theme props | Installed background/props; binding evidence incomplete | Scene screenshot, prefab refs, scale proof | [x] | [ ] |
| MAP-02 | Cat room map | Bed, feeder, litter box, dream entrance | Existing runtime cat-room surface is screenshot-backed by `02-cat-room.png`; Batch 90 preflight covers interaction prompts and local screen composition; Batch 91 built-in imagegen pass produced a visible source sheet, alpha sheet, 12 semantic sprites, semantic manifest, semantic contact sheet, v004 hard-alpha portal replacement, and final semantic pack review PASS_WITH_P2; Batch 92 adds focused interaction-state UI sprites for hover/ready/selected/blocked/markers/cooldown/reticle with final review PASS_WITH_P2; Batch 93 adds cat-room floor/wall decal and trim sprites with final review PASS_WITH_P2; Batch 94 adds a 12-frame stable-shell dream portal idle-loop sequence with final review PASS_WITH_P2. | Unity import/settings/binding, portal scale/pivot, portal loop jitter, interaction-ring curation, hit targets, hover/disabled/range/cooldown proof, marker scale, locator classification, floor/decal sorting layers, pivot/baseline checks, four-resolution Unity review | [x] | [ ] |
| MAP-03 | Egypt dream map | Egypt map with defense target, entrances, obstacles, theme props | Live HDo archive blocked; local P0 scope references Egypt dream | Blocked until map archive access or local source packet exists | [ ] | [ ] |
| PROP-01 | Bedroom interactables | Bed, litter box, feeder, dream cracks, affordance rings | Batch 54/67 candidates complete | Runtime interaction timing screenshots | [x] | [ ] |
| OWNER-01 | Owner sleep states | Sleeping, half-awake, nearly awake, awake; animation frames | Batch 76 candidate complete with 24 frames | Unity slicing/pivot/scale and overlay decision | [x] | [ ] |
| OWNER-02 | Owner sleep status icons | 4 states x 3 sizes | Batch 77 candidate complete | HUD/settlement readability | [x] | [ ] |
| ENEMY-01 | Black Mud body | Readable black-mud basic pressure enemy | Installed sprite/concept exists | Active screenshot, prefab binding, Console | [x] | [ ] |
| ENEMY-02 | Cold Light body | Ranged/cast pressure enemy | Installed sprite/concept exists | Active screenshot, prefab binding, Console | [x] | [ ] |
| ENEMY-03 | Call Tyrant boss | Male-owner dream fixed boss | Installed sprite/concept exists | Active boss screenshot, hitbox/pivot, f04 playback | [x] | [ ] |
| ENEMY-04 | Enemy framesheets | Idle/move/attack/skill-cast loops for current enemies | 3 source framesheets and 12 split sprites exist; offline import gate passes | Unity active animation screenshots and color-drift review | [x] | [ ] |
| ENEMY-05 | Egypt dream enemy pool | Egypt basic monsters and fixed Boss | Source blocked by HDo/linked docs | Do not generate until source is accessible | [ ] | [ ] |
| VFX-01 | Attack telegraphs | Track, circles, lanes, boss throws, warning reads | Enemy warning VFX candidates exist | Battle screenshots and occlusion checks | [x] | [ ] |
| VFX-02 | Combat feedback | Normal hit, shield, slow/freeze, aftershock, bed-hit, monster death, damage numbers | Several installed/candidate lanes exist | Timing, readability, binding, Console | [x] | [ ] |
| PIPE-01 | Review/contact sheets | Contact sheets, manifests, process notes, review notes per batch | Current local matrix: 38 validators passed, 0 failed | Keep every new batch validator-backed | [x] | n/a |
| PIPE-02 | Unity offline acceptance | Offline code/import/review readiness gates | Current `P0_OFFLINE_ACCEPTANCE_REPORT.md` passes 6/6 gates with 0 failures | Does not replace Play Mode or per-lane import evidence | [x] | [x] |
| PIPE-03 | Play Mode acceptance smoke | Runtime screenshot and route/defeat smoke evidence | `P0_PLAYMODE_ACCEPTANCE_SMOKE_REPORT.md` passes 11/11 screenshot evidence, including `02-cat-room.png` | Does not replace Batch90 candidate import/binding or 4-resolution screen review | [x] | [x] |

## Production Order

1. Source gate: re-check `Qr1`; if blocked sources become readable, refresh their local extracts before producing new art.
2. Table gate: update this table and `P0_SYSTEMATIC_ART_ASSET_REQUIREMENTS_2026-06-25.csv` or add a dated override table.
3. Candidate gate: produce only one narrow batch at a time under `design/development/asset_candidates/...`.
4. Transparency gate: for project-bound sprites, use built-in `image_gen` on a flat chroma-key background, then run the installed chroma-key removal helper and inspect alpha output.
5. Review gate: dispatch independent visual/style and production QA agents before any candidate enters Unity import review.
6. Unity gate: import only after formal approval; require screenshots, import settings, binding proof, and Console checks.
7. Memory gate: update this file, `P0_ASSET_MEMORY_TODO_LEDGER.md`, and the relevant batch review note after each accepted step.

## Image Generation Policy

| Topic | Rule |
| --- | --- |
| Default mode | Use built-in `image_gen`; do not require `OPENAI_API_KEY`. |
| CLI/API fallback | Only use explicit CLI/API/model path if the user asks for it; otherwise use Codex built-in imagegen. |
| User-required imagegen path | Built-in Codex `imagegen` is the active production path for this goal. Do not block normal asset production on `OPENAI_API_KEY`. |
| Transparent sprites | Generate on flat chroma key, remove locally, validate alpha corners and edge fringe. |
| Starter cat bodies | No generation, recolor, replacement, or formal import until active-cat screenshots pass source-lock review. |
| Allowed safe targets | Textless UI pieces, symbolic skill icons, non-body VFX, warnings, props, contact sheets, mockups. |
| Baked text | Avoid baked Chinese text; prefer Unity-rendered labels and numbers. |

## Archive Rules

| Artifact | Path rule |
| --- | --- |
| Candidate batches | `design/development/asset_candidates/<family>/batch_<number>_<slug>_<date>/` |
| Review notes | `design/development/asset_review/BATCH<number>_<SLUG>_AGENT_REVIEW_<date>.md` |
| Source-lock and formal gates | `design/development/asset_review/*SOURCE_LOCK*`, `*FORMAL_IMPORT*`, `*TURNAROUND*` |
| Unity evidence | `design/development/screenshots/...` plus `design/development/unity_batchmode/...` |
| Runtime assets after approval | `Assets/TheCat/Art/...` with Unity `.meta` validated by the P0 import gates |

## Review Agents

| Agent lane | Status | Expected output |
| --- | --- | --- |
| UI/style review | [x] dispatched | UI constraints, covered lanes, missing/blocked UI sprites, style risks. |
| Character consistency review | [x] dispatched | Starter-cat constraints, blocked body/framesheet rules, symbolic-only safe targets. |
| Map/enemy/VFX review | [x] dispatched | Map/enemy/VFX requirements, existing evidence, Unity-only gaps, next priorities. |
| Pipeline QA review | [x] dispatched | Gate fix coverage, validation evidence, stale wording, release risk. |

## TODO

| Priority | Task | Status |
| --- | --- | --- |
| P0 | Re-check Feishu auth/profile and source access with `personal` user token. | [x] |
| P0 | Confirm Qr1 live P0 minimum art boundary. | [x] |
| P0 | Record that MDr/IAd/IZp/HDo/FoW9 are ACL blocked despite valid token. | [x] |
| P0 | Fix and rerun Unity offline acceptance after framesheet/formal-gate repair. | [x] |
| P0 | Create this total design and required asset table. | [x] |
| P0 | Wait for the four dispatched review agents and merge their findings. | [x] |
| P0 | Build and validate Batch 89 skill-selection preflight packet; keep candidate-only. | [x] |
| P0 | Build and validate Batch 90 cat-room preflight packet; keep candidate-only. | [x] |
| P0 | Add Batch 89 and Batch 90 to the Unity validation queue/checklist without promoting them into `Assets`. | [x] |
| P0 | Repair Batch 47/49/50/51 strict candidate validator/manifests after 11-capture screenshot renumbering. | [x] |
| P0 | Run Play Mode 11-capture smoke including cat-room surface capture `02-cat-room.png`. | [x] |
| P0 | Produce the first visible built-in imagegen Batch91 cat-room map-elements source sheet, alpha sheet, and 12 cutout sprite candidates. | [x] |
| P0 | Integrate independent visual and production QA review for Batch91 before semantic naming or Unity import. | [x] |
| P0 | Regenerate or repair Batch91 `prop_04` dream entrance portal before formal semantic acceptance. | [x] |
| P0 | Build Batch91 12-sprite semantic contact sheet and manifest for Unity import review. | [x] |
| P0 | Produce narrow Batch92 cat-room interaction-state imagegen pack as a focused follow-up to Batch90/Batch91, not a broad new batch. | [x] |
| P0 | Integrate Batch92 visual, production QA, and cooldown v003 P1-fix reviews; keep package candidate-only. | [x] |
| P0 | Produce narrow Batch93 cat-room map decal/trim imagegen pack and integrate PASS_WITH_P2 review findings; keep package candidate-only. | [x] |
| P0 | Produce Batch94 cat-room dream portal 12-frame animation sequence, close visual/production P1 findings, and keep package candidate-only. | [x] |
| P0 | Pick the next safe non-animation imagegen batch under the updated goal and keep it static/candidate-only. | [x] |
| P0 | Produce Batch95 static role/scene/UI token pack with built-in imagegen, chroma-key cutouts, final review CSV, and independent source-lock/visual/production QA review. | [x] |
| P0 | Run Unity import/settings/binding review for Batch95 role/scene/UI tokens, including 64px readability and no recursive import from `superseded/`. | [ ] |
| P0 | Keep starter-cat body/framesheet generation blocked until active-cat screenshots pass. | [x] |
| P0 | Run remaining candidate-specific Unity screen matrices for UI batches 83-90 and Batch 75 scale matrix. | [ ] |
| P0 | Run active enemy animation screenshots, prefab binding, Console, pivot/hitbox/color review. | [ ] |
| P1 | Request Feishu access for blocked docs/folder and refresh source tables. | [ ] |
