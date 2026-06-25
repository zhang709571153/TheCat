# P0 Systematic Art Asset Master Ledger

Project: `D:\Unity Workspace\TheCat`
Updated: 2026-06-25 21:03 +08:00

This file is the 2026-06-25 control ledger for systematic P0 art production.
It separates four states that must not be mixed:

- `source_ready`: source truth is available for this pass.
- `candidate_complete`: Codex-side PNGs, review notes, manifests, or validators exist.
- `installed_pending_unity`: files are under `Assets/` but still need Unity evidence.
- `unity_accepted`: runtime screenshots, import settings, binding proof, and Console checks pass.

## Source Access

| Source | Role | Current access | Working basis | Action |
| --- | --- | --- | --- | --- |
| `Qr1XdXd6KosnjMxjgW7cS89kn9c` | UI/style source truth | Live outline fetch passed as user `章航宁`, revision `816`; section 9 fetched live on 2026-06-25 08:03 +08:00 | Live outline, live P0 minimum art boundary, plus local synced markdown | Use as UI style authority. |
| `MDrSdEoaToB5cnxZgrOcAE34nof` | Core gameplay/path reference | Personal user token is valid, but live user fetch still returned `3380004` no permission on 2026-06-25 08:03 +08:00 | Local synced markdown only | Request view access before claiming current-live coverage. |
| `IAdkdcpciobUTXxa7dBcRx7Bngf` | Character design source truth | Personal and bot fetch both returned `3380004` no permission on 2026-06-25 08:03 +08:00 | Local synced markdown plus source-lock packets | Use local source-locks; request view access for refresh. |
| `IZpFdIwtboEzzrx4ZFlcZLD2npe` | Linked Feishu doc | Personal user token is valid, but live user fetch still returned `3380004` no permission on 2026-06-25 08:03 +08:00 | None in local manifest | Blocked until access is granted. |
| `HDoWdDNR3oZ6uhxuMzPcT8qCn5f` | Linked Feishu doc | Personal user token is valid, but live user fetch still returned `3380004` no permission on 2026-06-25 08:03 +08:00 | None in local manifest | Blocked until access is granted. |
| `FoW9fKYcDllwJjdTxGHcu4pbnab` | Drive folder | Inspect resolves folder token as `folder`, but read-only `drive +status --quick` returned `1061004 forbidden` on 2026-06-25 08:03 +08:00 | None | Blocked until folder list access is granted. |

## Source Truth Rules

| Area | Rule |
| --- | --- |
| UI/style | `Qr1...` is the authority. Keep P0 focused on the actual playable loop: entry/loading, main menu, cat room, character select, skill selection, dream entry/route, battle HUD, pause/settings, and victory/defeat/settlement. Do not broaden into commercial or P1 systems before runtime validation. |
| Characters | `IAd...` plus the colored three-view turnarounds and source-lock packets are the authority. Starter-cat body, sequence frames, runtime replacement, recolor, or crop import remain blocked until registered active-cat screenshots receive explicit colored-turnaround comparison approval. |
| Image generation | Prefer non-cat symbolic UI, VFX, warnings, props, and screen compositions. Cat body generation is not allowed outside reference-only candidate review. |
| Text | Avoid baked Chinese text in generated art until the typography and number rules pass screenshot checks. Use textless sprites plus Unity-rendered text where possible. |
| Candidate boundary | Candidate packs stay under `design/development/asset_candidates/...` with no `.meta` until formal install approval. |

## Integrated Agent Review

| Review lane | Finding | Resulting control |
| --- | --- | --- |
| UI/style | 84 UI PNGs exist, but runtime screenshots are incomplete. Five reward detail badge metas were still Default Texture. | Fixed the five reward detail badge metas to Sprite/alpha. Keep screen-level validation open. |
| Character | Three starter cats remain source-locked. P0-adjacent cats have design references but no P0 source-lock packet. | Do not generate/import starter or adjacent cat bodies. Add placeholder lanes for source locks, framesheets, icons, and VFX. |
| Scene/enemy/VFX | Bedroom map, props, enemy sprites, and symbolic VFX exist, but GUID scan found no direct scene/prefab refs. Enemy framesheet source metas are now Sprite/alpha and 12 deterministic split sprites exist for import testing. | Treat scene/enemy/VFX as generated or installed pending Unity evidence, not accepted. Keep active-enemy screenshots, binding proof, Console, pivot/playback, and color-drift checks open. |
| UI common components | 17 common UI component rows are inventoried: 8 installed pending Unity evidence, 9 candidate-only/import-test rows, 0 missing design-needed rows after Batch 82. Batch 82 generated 25 derivative textless candidate sprites and passed review after one visual revision. | Use `P0_UI_COMMON_COMPONENT_INVENTORY_2026-06-25.md` plus `BATCH82_COMMON_UI_STATE_AGENT_REVIEW_2026-06-25.md` as the local common-component control packet. Screen-level entry/cat-room/skill-selection UI remains separate and Unity evidence is still required. |
| Qr1 live UI coverage | Qr1 live requires entry, main menu, cat room, battle HUD, pause, settings, skill selection, and victory/defeat settlement. Dedicated local preflight packets now exist for loading/start, result/settlement, settings/pause, dream-route, battle HUD, character select, skill selection, and cat room. | Continue treating Batch 83-90 plus Batch 75 as Unity-evidence work, not accepted runtime screen coverage. Main menu and all current screen packets still need Unity-rendered screenshots, import settings, binding proof, and Console checks. |
| Loading/start screen | Batch 83 generated 4 transparent candidate sprites and 4 local loading/start mockups from existing UI shell primitives. Visual review passed with P2 Unity watch items; production QA P1 was fixed and follow-up passed. Batch 83 is now queued in the P0 asset production queue/checklist as candidate-complete pending Unity review. | Use `BATCH83_LOADING_START_AGENT_REVIEW_2026-06-25.md` as the local loading/start preflight packet. Unity-rendered screenshots, text/state replacement, spinner interpretation, 1024x768 crowding, import settings, binding, and Console remain required before any install. |
| Result/settlement screen | Batch 84 generated 7 transparent result/settlement sprites and 4 local mockups from existing UI shell primitives. Visual review passed with one remaining P2 Unity 1024x768 crowding watch after the failure-stamp semantic fix; production QA passed after contact/review sheet hash locking. Batch 84 is now queued in the P0 asset production queue/checklist as candidate-complete pending Unity review. | Use `BATCH84_RESULT_SETTLEMENT_AGENT_REVIEW_2026-06-25.md` as the local result/settlement preflight packet. Unity-rendered victory/defeat/settlement screenshots, Chinese text/number replacement, 1024x768 run-failed crowding, import settings, binding, and Console remain required before any install. |
| Settings/pause screen | Batch 85 generated 6 transparent settings/pause sprites and 4 local mockups from existing UI shell primitives, Batch 78 controls, and Batch 79 icons. Visual review passed with one P2 Unity watch for 1024x768 key-hint semantics; production QA passed. Batch 85 is now queued in the P0 asset production queue/checklist as candidate-complete pending Unity review. | Use `BATCH85_SETTINGS_PAUSE_AGENT_REVIEW_2026-06-25.md` as the local settings/pause preflight packet. Unity-rendered settings/pause screenshots, Chinese text/value replacement, key-hint-vs-back semantics, click-target proof, import settings, binding, and Console remain required before any install. |
| Dream route screen | Batch 86 generated 6 transparent dream-route sprites and 4 local dream-entry/route-map mockups from existing UI shell primitives, route icons, route card frames, and Batch 65 route-map accents. Visual review passed with P2 watches for compact lower-half density and boss gate scale; production QA passed after manifest provenance hash fields were added and validated. Batch 86 is now queued in the P0 asset production queue/checklist as candidate-complete pending Unity review. | Use `BATCH86_DREAM_ROUTE_AGENT_REVIEW_2026-06-25.md` as the local dream-route preflight packet. Unity-rendered dream-entry/route-map screenshots, Chinese text/route label/reward replacement, node/path semantics, 1024x768 card density, boss gate scale, click-target proof, import settings, binding, and Console remain required before any install. |
| Character select screen | Batch 88 generated 6 transparent character-select sprites and 4 local character-select mockups from existing UI shell primitives, main-menu background, source-locked HUD avatars, and Batch 80 symbolic skill icons. Visual review passed with P2 low-height density watches for `1280x720` and `1024x768`. Production QA passed after the provenance P2 was fixed in the process note and validator. Batch 88 is now queued in the P0 asset production queue/checklist as candidate-complete pending Unity review. | Use `BATCH88_CHARACTER_SELECT_AGENT_REVIEW_2026-06-25.md` as the local character-select preflight packet. Unity-rendered character-select screenshots, source-lock avatar consistency, Chinese name/role/description/start-label replacement, low-height crowding proof, card/start click targets, import settings, binding, and Console remain required before any install. |
| Battle HUD screen | Batch 87 generated 6 transparent battle HUD sprites and 4 local battle HUD mockups from existing UI shell primitives, core gauges, runtime controls, Batch 80 skill icons, and existing HUD avatars/enemy sprites. Production QA passed. Initial visual review found a P1 missing fourth gauge in the 1024x768 dense mockup; the builder was fixed and follow-up visual review passed. Batch 87 is now queued in the P0 asset production queue/checklist as candidate-complete pending Unity review. | Use `BATCH87_BATTLE_HUD_AGENT_REVIEW_2026-06-25.md` as the local battle HUD preflight packet. Unity-rendered battle HUD screenshots, four-gauge proof, Chinese text/number replacement, skill states, enemy/telegraph occlusion, pause/speed/restart click targets, import settings, binding, and Console remain required before any install. |
| Skill selection screen | Batch 89 generated 8 transparent skill-selection sprites and 4 local mockups from existing UI shell primitives, Batch 80 starter skill icons, Batch 81 skill slot frames, and prior skill UI assets. Visual review passed with P2 density and state-semantics watches; production QA passed. Batch 89 is now queued in the P0 asset production queue/checklist as candidate-complete pending Unity review. | Use `BATCH89_SKILL_SELECTION_AGENT_REVIEW_2026-06-25.md` as the local skill-selection preflight packet. Unity-rendered skill-selection screenshots, selected/ready/disabled/locked states, cooldown/low-resource/no-target semantics, Chinese text/number replacement, click targets, import settings, binding, and Console remain required before any install. |
| Cat room screen | Batch 90 generated 6 transparent cat-room sprites and 4 local mockups from existing bedroom map and interaction assets. Visual review passed with P2 dream-entry and disabled-state semantics watches; production QA passed. Batch 90 is queued in the P0 asset production queue/checklist as candidate-complete pending Unity review. The Play Mode 11-capture smoke now also verifies the existing runtime cat-room surface and writes `02-cat-room.png`. | Use `BATCH90_CAT_ROOM_AGENT_REVIEW_2026-06-25.md` as the local cat-room preflight packet. `02-cat-room.png` proves surface reachability only; Batch90 sprite import, four-resolution parity, bed/feeder/litter/dream entrance interactions, hover/disabled/range states, click targets, import settings, binding, and Console remain required before any install. |
| Cat room map elements | Batch 91 uses built-in Codex `imagegen` to produce a visible sprite source sheet, alpha sheet, 12 semantic sprites, semantic contact sheet, semantic manifest, process note, agent review note, and semantic review table under `design/development/asset_candidates/map/cat_room_elements/batch_91_cat_room_map_elements_imagegen_2026-06-25/`. Final semantic pack review result is `PASS_WITH_P2`; v004 hard-alpha portal replaces blocked `prop_04`. | Run Unity scale/import/binding review, including portal scale/pivot and interaction-ring curation, before any runtime promotion. |
| Cat room interaction states | Batch 92 uses built-in Codex `imagegen` to produce a narrow textless interaction-state sprite pack under `design/development/asset_candidates/ui/cat_room_interaction_states/batch_92_cat_room_interaction_states_imagegen_2026-06-25/`. It includes source/alpha sheets, 12 semantic sprites, cooldown v002/v003 rework traceability, final contact sheet v003, semantic manifest, process note, and agent review note. Final review result is `PASS_WITH_P2`; cooldown v003 closes the original P1 grey-overlay issue. | Run Unity import/binding review for hover, ready, selected, blocked, sleep/feed/litter markers, dream button frame, attention marker, cooldown v003, locator badge, and reticle. Curate ring semantics and classify locator before promotion. |
| Cat room map decals/trim | Batch 93 uses built-in Codex `imagegen` to produce a narrow cat-room floor/wall decal and trim kit under `design/development/asset_candidates/map/cat_room_decals/batch_93_cat_room_map_decals_imagegen_2026-06-25/`. It includes source/alpha sheets, 12 semantic sprites, contact sheet, semantic manifest, final review CSV, process note, and agent review note. Final review result is `PASS_WITH_P2`; no P1/FAIL rework item remains. | Run Unity import/binding review for floor patch, soft shadow, rug edge, mat/pad, baseboard straight/corner, paw scuff, window light, dream crack, threshold, dust sparkle, and boundary corner. Validate sorting layer, pivot/baseline, floor contrast, no-collider policy, and scene readability before promotion. |
| Cat room dream portal animation | Batch 94 uses built-in Codex `imagegen` plus a stable Batch91-derived shell to produce a 12-frame dream entrance idle-loop package under `design/development/asset_candidates/map/cat_room_dream_portal_animation/batch_94_cat_room_dream_portal_animation_imagegen_2026-06-25/`. It includes the original source sheet, stable base, interior mask, 12 v004 transparent 512x512 frames, contact sheet v004, loop preview GIF v004, semantic manifest, final review CSV, process note, and agent review note. Final review result is `PASS_WITH_P2`; v001/v002/v003 are isolated under `superseded/`. | Run Unity import/binding review for loop jitter, scale, pivot, sorting layer, and static dream entrance hitbox reuse before runtime promotion. |
| Role / scene UI tokens | Batch 95 uses built-in Codex `imagegen` to produce a static no-animation 9-sprite token pack under `design/development/asset_candidates/ui/role_scene_ui_tokens/batch_95_role_scene_ui_tokens_imagegen_2026-06-25/`. It includes the chroma-key source sheet, alpha sheet, 9 semantic sprites, asset table, manifest, contact sheet, final review CSV, process note, and agent review note. Final review result is `PASS_WITH_P2`; it contains role symbols, scene tokens, and UI card frames only, with no cat body/portrait/framesheet content. | Run Unity import/binding review for 64px readability, selected-vs-ready/locked states, import settings, no recursive import from `superseded/`, and Console before runtime promotion. |

## Master Asset Families

| Family | Required split | Current state | Next gate |
| --- | --- | --- | --- |
| UI screens | Entry/loading, main menu, cat room, character select, skill selection, dream entry/route, battle HUD, pause, settings, victory, defeat, result, settlement | Many primitives and banners exist; Batch 83 adds local loading/start preflight mockups and textless loading sprites; Batch 84 adds local result/settlement preflight mockups and textless result/settlement sprites; Batch 85 adds local settings/pause preflight mockups and textless settings/pause sprites; Batch 86 adds local dream-entry/route-map preflight mockups and textless dream-route sprites; Batch 88 adds local character-select mockups and textless character-select sprites; Batch 87 adds local battle HUD preflight mockups and textless battle HUD sprites; Batch 89 adds local skill-selection mockups and textless skill-selection sprites; Batch 90 adds local cat-room mockups and textless cat-room interaction sprites; Batch 92 adds a focused built-in imagegen cat-room interaction-state sprite pack and cooldown v003 fix. Play Mode smoke captures main menu, cat room, route, battle HUD, active cats, battle world, Call Tyrant warning, battle result, and settlement. All candidate screen packets still need per-batch Unity review before promotion. | Run Unity screen composition screenshots across target resolutions for the candidate packets, then promote only the packets that pass text, click-target, import, binding, and Console gates. |
| UI components | Buttons, selected/disabled states, panels, dialogs, tabs, cards, list rows, locks, warnings | Component inventory complete for this pass: 17 rows, 8 installed pending Unity evidence, 9 candidate-only/import-test rows, 0 missing design-needed rows; Batch 82 adds 25 derivative candidate sprites | Screen-priority review, then Unity click-target/readability screenshots, import settings, binding, and Console checks. |
| Role / scene UI tokens | Starter role badges, static scene tokens, ready/selected/locked card frames | Batch 95 adds 9 static transparent sprites for Saiban/Nephthys/Suzune role badges, bedroom/cat-room/dream-route scene tokens, and UI card states; candidate-only review is PASS_WITH_P2 | Unity 64px readability, card-state screenshot proof, import settings, binding, and Console checks. |
| Typography/numbers | Chinese font, cooldown digits, damage/reward numbers, gauge labels | Batch 75 validation packet and 20 local scale mockups exist; no local P0 visual blocker after independent review | 5 surfaces x 4 resolutions Unity screenshot matrix plus Console notes. |
| Starter cat bodies | Saiban, Nephthys, Suzune runtime sprites | Installed source-locked combat sprites exist; active-cat screenshots are registered 3/3, but formal replacement remains blocked | Explicit colored-turnaround comparison approval notes are required before import. |
| Starter cat framesheets | Idle, walk, attack, small skill, ultimate per cat | Missing as promoted production lanes | Source-lock first, then reference-only candidate batch. |
| Starter skill icons | 5-6 per cat, symbolic civilization/weapon motifs | Batch 80 recommended set selects 14 current v001/v002 icons plus 4 v003 lightframe icons; horizontal HUD overlay review says import-test only, candidate-only | Use Batch 81 slot frames for next HUD fit review before Unity import. |
| Skill slot frames | Square/round ready, cooldown, disabled, selected states | Batch 81 v002_light square-only variant is the preferred import-test candidate; v001 remains badge-legibility fallback; local actual-scale HUD mockup preflight exists; candidate-only | Run Unity import-test screenshots/settings/Console/binding proof, including selected-vs-ready and `99` at actual HUD scale. |
| Starter skill VFX | Saiban bedline, Nephthys moon-sand, Suzune lullaby/torii/heal | Batch 61 installed symbolic VFX exists | Skill-cast timing screenshots and binding proof. |
| Owner sleep | 4 states x 6 frames | Batch 76 candidate complete | Unity slicing, pivot, scale, overlay-vs-bed decision. |
| Owner status icons | 4 states x 3 sizes | Batch 77 candidate complete | HUD/settlement readability and import proof. |
| Bedroom/cat-room map props | Background, bed, litter box, feeder, four entrances, dream cracks, cat-room hub objects | Runtime files plus Batch 54/67 candidates exist; Batch 91 adds built-in imagegen cat-room map element candidates with 12 semantic sprites and a final semantic PASS_WITH_P2 review; Batch 92 adds focused cat-room interaction-state sprites with cooldown v003 P1 fix; Batch 93 adds cat-room decal/trim kit for floor, wall, threshold, light, dust, and dream-crack composition; Batch 94 adds a stable-shell 12-frame dream portal idle-loop sequence | Scene scale, interaction timing, prefab binding, marker scale, ring semantics, portal loop jitter, floor/decal sorting, pivot/baseline choices, and Unity import settings. |
| Enemies | Black Mud, Cold Light, Call Tyrant; future warnings for Dream Rail, Red Eye, Unread Dot, Teddy | Sprites/VFX exist; future warnings candidate complete; local framesheet import/slicing policy complete with 12 split sprites | Enemy screenshots, prefab binding, Console, Call Tyrant f04 pivot/playback/hitbox, and color-drift review. |
| Route map | Current, selected, path, locked, boss route accents | Batch 65 candidate complete; Batch 86 composes route panels, nodes, path ribbons, choice-card slots, and boss gate into screen-level local mockups | Validate if route flow is exercised in P0, with 1024x768 route-card density and boss gate scale checks. |
| Rewards/events/blessings | Reward icons, badges, shop/event/partner/blessing cards | Many installed assets exist; reward badge metas fixed in this pass | UI screenshots and import scan. |
| Settings/system controls | Sliders, switches, checkboxes, sound/mute/back/close/pause/retry/lock/warning | Batch 78/79 candidate complete; Batch 85 composes these into settings/pause screen-level preflight candidates | Settings/pause screenshots, key-hint semantics, click-target proof, and import decisions. |

## Current TODO

| Priority | Task | State |
| --- | --- | --- |
| P0 | Keep `Qr1...` as UI/style authority and update derived tables from it first. | [x] |
| P0 | Use `design/development/asset_review/P0_SYSTEMATIC_ART_PRODUCTION_TOTAL_DESIGN_2026-06-25.md` as the current readable required-asset table and production design entry. | [x] |
| P0 | Record that `MDr...`, `IAd...`, `IZp...`, `HDo...`, and folder `FoW9...` are blocked by ACL even though `personal` user token is valid. | [x] |
| P0 | Fix five reward detail badge metas from Default Texture to Sprite/alpha. | [x] |
| P0 | Keep starter-cat body and framesheet generation blocked until active-cat screenshots pass source-lock review. | [x] |
| P0 | Add or maintain explicit lanes for starter cat framesheets and per-starter skill icons. | [x] |
| P0 | Integrate Batch 80 UI/style and character-consistency review agents. | [x] |
| P0 | Integrate Batch 80 production-pipeline QA agent. | [ ] blocked: agent timed out twice and was closed |
| P0 | Build Batch 80 64/32 px HUD readability board. | [x] |
| P0 | Generate Batch 80 lighter-frame comparison variant. | [x] |
| P0 | Review whether Batch 80 v003 lightframe should replace any accepted v001/v002 icons. | [x] |
| P0 | Build Batch 80 recommended mixed candidate set. | [x] |
| P0 | Generate Batch 80 Battle HUD skill frame/cooldown overlay test board. | [x] |
| P0 | Integrate Batch 80 Battle HUD overlay review agents. | [x] |
| P0 | Generate Batch 81 square/round skill slot frame candidates. | [x] |
| P0 | Integrate Batch 81 skill slot frame review agents. | [x] |
| P0 | Create square-only Batch 81 import-test plan with cooldown digits `1`, `12`, `99`. | [x] |
| P0 | Generate candidate-only Batch 81 square cooldown digit mockups for `1`, `12`, `99`. | [x] |
| P0 | Integrate Batch 81 cooldown digit readability review. | [x] |
| P0 | Generate Batch 81 v002_light square-only slot variant with lighter badge/corner ornamentation. | [x] |
| P0 | Integrate Batch 81 v002_light visual/production QA agents. | [x] |
| P0 | Build Batch 81 v002_light local actual-scale HUD mockup pack. | [x] |
| P0 | Integrate Batch 81 v002_light local HUD mockup review agents. | [x] |
| P0 | Run Batch 80/81 square slot import-test in Unity and capture cooldown digit screenshots. | [ ] |
| P0 | Validate existing non-cat candidate packs before opening a new generation batch. | [x] |
| P0 | Create a dedicated `ui_skill_selection_preflight` candidate packet from textless skill cards, detail panel, confirm button, chips, and selected/disabled/locked states. | [x] |
| P0 | Create a dedicated `ui_cat_room_preflight` candidate packet for cat-room entry, bed/feeder/litter/dream entrance prompts, hover/disabled/range states, and no new cat body art. | [x] |
| P0 | Capture cat-room runtime surface as part of the 11-capture Play Mode smoke (`02-cat-room.png`). | [x] |
| P0 | Produce Batch92 cat-room interaction-state imagegen pack and integrate independent review findings, including cooldown v003 P1 fix. | [x] |
| P0 | Produce Batch93 cat-room map decal/trim imagegen pack and integrate independent review findings. | [x] |
| P0 | Produce Batch94 cat-room dream portal animation sequence and integrate v004 PASS_WITH_P2 review findings. | [x] |
| P0 | Produce Batch95 static role/scene/UI token pack with no animation and no character body/portrait/framesheet content. | [x] |
| P0 | Run Unity import/settings/binding review for Batch95 role/scene/UI tokens, including 64px readability and no recursive import from `superseded/`. | [ ] |
| P0 | Build and validate UI common component inventory. | [x] |
| P0 | Integrate UI common component inventory review agents and fix reproducibility findings. | [x] |
| P0 | Produce Batch 82 derivative candidate sprites for prior missing common UI state families. | [x] |
| P0 | Integrate Batch 82 common UI state review agents and fix visual P1 findings. | [x] |
| P0 | Build and validate Batch 83 loading/start local preflight candidate packet. | [x] |
| P0 | Integrate Batch 83 loading/start visual and production QA reviews and fix P1 findings. | [x] |
| P0 | Add Batch 83 loading/start preflight to the P0 asset production queue/checklist without promoting it into `Assets`. | [x] |
| P0 | Build and validate Batch 84 result/settlement local preflight candidate packet. | [x] |
| P0 | Integrate Batch 84 result/settlement visual and production QA reviews and fix P2 findings. | [x] |
| P0 | Add Batch 84 result/settlement preflight to the P0 asset production queue/checklist without promoting it into `Assets`. | [x] |
| P0 | Build and validate Batch 85 settings/pause local preflight candidate packet. | [x] |
| P0 | Integrate Batch 85 settings/pause visual and production QA reviews and carry the P2 key-hint semantics watch into Unity gates. | [x] |
| P0 | Add Batch 85 settings/pause preflight to the P0 asset production queue/checklist without promoting it into `Assets`. | [x] |
| P0 | Build and validate Batch 86 dream-route local preflight candidate packet. | [x] |
| P0 | Integrate Batch 86 dream-route visual and production QA reviews, including the manifest provenance hash fix. | [x] |
| P0 | Add Batch 86 dream-route preflight to the P0 asset production queue/checklist without promoting it into `Assets`. | [x] |
| P0 | Build and validate Batch 87 battle HUD local preflight candidate packet. | [x] |
| P0 | Integrate Batch 87 battle HUD production QA and visual reviews, including the 1024x768 four-gauge P1 fix. | [x] |
| P0 | Add Batch 87 battle HUD preflight to the P0 asset production queue/checklist without promoting it into `Assets`. | [x] |
| P0 | Build and validate Batch 88 character-select local preflight candidate packet. | [x] |
| P0 | Integrate Batch 88 character-select production QA and visual reviews, including the provenance P2 fix and low-height density watch. | [x] |
| P0 | Add Batch 88 character-select preflight to the P0 asset production queue/checklist without promoting it into `Assets`. | [x] |
| P0 | Resolve enemy framesheet local import settings and slicing policy. | [x] |
| P0 | Run Unity active enemy animation import/screenshots for split sprites, including Call Tyrant f04 pivot/playback/hitbox and color-drift checks. | [ ] |
| P0 | Build Batch 75 local Chinese UI scale preflight mockups. | [x] |
| P0 | Integrate Batch 75 local Chinese UI scale review agents. | [x] |
| P0 | Run Batch 75 Chinese UI scale screenshot matrix in Unity. | [ ] |
| P0 | Capture remaining candidate-specific Unity screen matrices for loading/start, result/settlement, settings/pause, dream-route, skill-selection, battle HUD, and Batch 90 interaction states. | [ ] |
| P0 | Capture Batch92 cat-room interaction-state Unity import/binding and actual-scale screenshot review. | [ ] |
| P0 | Capture Batch93 cat-room map decal/trim Unity import/binding and floor contrast screenshot review. | [ ] |
| P0 | Capture Batch94 dream portal animation Unity import/binding, loop jitter, scale/pivot, sorting, and static-hitbox screenshot or video review. | [ ] |
| P1 | Request Feishu access for blocked docs/folder and refresh this ledger when access changes. | [ ] |
| P1 | Upgrade `lark-cli` from `1.0.53` to `1.0.57` after current asset pass if the user approves. | [ ] |

## Non-Cat Candidate Validation Matrix

| Item | State | Evidence |
| --- | --- | --- |
| Matrix generated | [x] | `design/development/asset_review/P0_NONCAT_CANDIDATE_VALIDATION_MATRIX_2026-06-25.md` |
| Validators run | [x] | 38 non-cat or symbolic candidate/install validators, including enemy framesheet import policy, UI common component inventory, Batch 82 common UI state candidates, Batch 83 loading/start preflight, Batch 84 result/settlement preflight, Batch 85 settings/pause preflight, Batch 86 dream-route preflight, Batch 87 battle HUD preflight, Batch 88 character-select preflight, Batch 89 skill-selection preflight, and Batch 90 cat-room preflight. |
| Local package integrity | [x] | 38 passed, 0 failed after adding the Batch 89 skill-selection and Batch 90 cat-room preflight validators. |
| Unity acceptance | [ ] | Offline acceptance passes 6/6 and Play Mode smoke passes 11/11 screenshot evidence, but per-lane candidate import settings, Console checks, prefab/catalog binding proof, and formal install decisions remain required. |
| CLI update notice | [ ] | Lark CLI reported `1.0.57` available while current is `1.0.53`; not updated during this asset pass. |
| Matrix review | [x] | Independent review confirmed the current matrix only proves curated local package integrity, not exhaustive project or Unity acceptance. |
| Enemy framesheet split review | [x] | `ENEMY_FRAMESHEET_IMPORT_POLICY_AGENT_REVIEW_2026-06-25.md` records visual PASS with TODO caveats and production QA PASS after reproducibility fix. |
| Batch 81 local mockup review | [x] | Independent visual and QA review passed local preflight; `selected` vs `ready` and cooldown `99` remain Unity HUD watch items. |
| Batch 75 local mockup review | [x] | Independent visual and QA review passed 20 local Chinese UI scale mockups; Unity 5x4 screenshots and Console notes remain required. |
| Next evidence queue | [x] | `design/development/asset_review/P0_ASSET_UNITY_VALIDATION_CHECKLIST.md` now includes Batch 83-90; `design/development/asset_review/BATCH81_V002_LIGHT_UNITY_EVIDENCE_QUEUE.md` remains the skill-slot focused queue. |
| Feishu live access recheck | [x] | `design/development/asset_review/FEISHU_SOURCE_ACCESS_RECHECK_2026-06-25.md` records valid user token plus remaining ACL blockers. |
| UI common component review | [x] | `design/development/asset_review/UI_COMMON_COMPONENT_INVENTORY_AGENT_REVIEW_2026-06-25.md` records PASS after cwd-independent matrix and all-evidence path validation fixes. |
| Batch 82 common UI state review | [x] | `design/development/asset_review/BATCH82_COMMON_UI_STATE_AGENT_REVIEW_2026-06-25.md` records local PASS after segmented-control and list-row P1 fixes. |
| Batch 83 loading/start review | [x] | `design/development/asset_review/BATCH83_LOADING_START_AGENT_REVIEW_2026-06-25.md` records visual PASS_WITH_P2 and production QA PASS after builder boundary fix. |
| Batch 84 result/settlement review | [x] | `design/development/asset_review/BATCH84_RESULT_SETTLEMENT_AGENT_REVIEW_2026-06-25.md` records visual PASS_WITH_P2 after failure-stamp fix and production QA PASS after sheet hash locking. |
| Batch 85 settings/pause review | [x] | `design/development/asset_review/BATCH85_SETTINGS_PAUSE_AGENT_REVIEW_2026-06-25.md` records visual PASS_WITH_P2 with key-hint semantics watch and production QA PASS. |
| Batch 86 dream-route review | [x] | `design/development/asset_review/BATCH86_DREAM_ROUTE_AGENT_REVIEW_2026-06-25.md` records visual PASS_WITH_P2 with compact density and boss-gate scale watches, production QA PASS_WITH_P2, and the fixed row-level provenance hashes. |
| Batch 87 battle HUD review | [x] | `design/development/asset_review/BATCH87_BATTLE_HUD_AGENT_REVIEW_2026-06-25.md` records production QA PASS, initial visual P1 for missing 1024x768 fourth gauge, builder fix, and follow-up visual PASS. |
| Batch 88 character-select review | [x] | `design/development/asset_review/BATCH88_CHARACTER_SELECT_AGENT_REVIEW_2026-06-25.md` records visual PASS_WITH_P2 with low-height density watches, production QA PASS_WITH_P2, and the fixed provenance note/validator coverage. |
| Batch 89 skill-selection review | [x] | `design/development/asset_review/BATCH89_SKILL_SELECTION_AGENT_REVIEW_2026-06-25.md` records visual PASS_WITH_P2 with density and state-semantics watches, production QA PASS, validator pass, and remaining Unity gates. |
| Batch 90 cat-room review | [x] | `design/development/asset_review/BATCH90_CAT_ROOM_AGENT_REVIEW_2026-06-25.md` records visual PASS_WITH_P2 with dream-entry and disabled-state semantics watches, production QA PASS, validator pass, remaining Unity gates, and the later narrow `02-cat-room.png` runtime-surface addendum. |
| Batch 83/84/85/86/87/88/89/90 Unity queue coverage | [x] | `design/development/asset_review/P0_ASSET_UNITY_VALIDATION_CHECKLIST.md` now lists 19 queue items, 14 candidate-review items, and the Batch 83 loading/start, Batch 84 result/settlement, Batch 85 settings/pause, Batch 86 dream-route, Batch 87 battle HUD, Batch 88 character-select, Batch 89 skill-selection, plus Batch 90 cat-room gates. |
| Unity offline gate repair | [x] | `design/development/asset_review/UNITY_BATCHMODE_OFFLINE_ATTEMPT_2026-06-25.md` records the stale 07:44 Unity offline failures, the framesheet Sprite/v2-marker fixes, and the starter-cat blocked-with-screenshots formal gate fix; the current `P0_OFFLINE_ACCEPTANCE_REPORT.md` passes 6/6 gates with 0 failures. |

## Next Production Decision

Do not start a broad new imagegen batch yet. The existing bedroom, enemy,
symbolic starter VFX, settings/system UI, owner sleep frames, Chinese UI
scale evidence, UI common component inventory, Batch 82 derivative common UI states, Batch 83 loading/start preflight, Batch 84 result/settlement preflight, Batch 85 settings/pause preflight, Batch 86 dream-route preflight, Batch 87 battle HUD preflight, Batch 88 character-select preflight, Batch 89 skill-selection preflight, and Batch 90 cat-room preflight have now passed local validator coverage, but remain blocked
from formal promotion by Unity runtime evidence.

The latest visible imagegen production follow-up is Batch 95
`role_scene_ui_tokens_imagegen`: a narrow built-in Codex imagegen static packet
with source sheet, alpha sheet, 9 transparent role/scene/card-frame sprites,
asset table, semantic manifest, contact sheet, final review CSV, process note,
and agent review. It stays under `design/development/asset_candidates/...` and
still requires Unity import, 64px readability, binding, and Console evidence
before promotion.

Next recommended action when Unity editor/MCP access is available:
run `P0_ASSET_UNITY_VALIDATION_CHECKLIST.md` against Batch 83-90, with first
priority on Batch 90 cat room, Batch 89 skill selection, Batch 87 battle HUD,
and Batch 75 Chinese UI scale screenshots. Local mockups and reviewer preflight
do not replace Unity screenshots.

Current offline acceptance is green in
`P0_OFFLINE_ACCEPTANCE_REPORT.md`: 6/6 gates passed, failure count 0, starter-cat
strict candidates ready 3/3, formal starter-cat import still `Blocked`, and
active screenshots registered 3/3. Play Mode smoke is separately green in
`P0_PLAYMODE_ACCEPTANCE_SMOKE_REPORT.md` with 11/11 screenshot evidence,
including `02-cat-room.png`. These reports do not replace per-lane Console
checks, import settings, candidate binding proof, active enemy animation review,
or starter-cat colored-turnaround approval.

Built-in Codex `imagegen` is the active production path for this goal and does
not require `OPENAI_API_KEY`. CLI/API fallback remains available only if the
user explicitly asks for that path.
