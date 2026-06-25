# P0 Development Blueprint

> Superseded current-state note, 2026-06-24:
> This file is retained as the initial 2026-06-13 planning blueprint. Its
> "Current Unity facts" are no longer authoritative because the project now has
> substantial Runtime, Gameplay, Roguelite, Tools, test, and asset-catalog
> implementation. Use `P0_CURRENT_ARCHITECTURE_2026-06-24.md` and
> `P0_DEMO_EXECUTION_PLAN_2026-06-24.md` for current work.

Date: 2026-06-13
Project: `D:\Unity Workspace\TheCat`
Game: `猫眠所 / 梦境支配者`
Phase: A complete, B ready

## 1. Source Map

Primary design sources:

- `design/梦境支配者核心玩法/docs/00_overview/p0_minimum_design.md`
- `design/梦境支配者核心玩法/docs/01_core_gameplay/core_gameplay_rules_and_player_path.md`
- `design/梦境支配者核心玩法/docs/02_combat_and_systems/core_numeric_system_v1.md`
- `design/梦境支配者核心玩法/docs/02_combat_and_systems/numeric_system_review_and_optimization.md`
- `design/梦境支配者核心玩法/docs/02_combat_and_systems/status_tag_system.md`
- `design/梦境支配者核心玩法/docs/03_characters/character_design.md`
- `design/梦境支配者核心玩法/docs/03_characters/character_voice_lines_appendix.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- `design/梦境支配者核心玩法/docs/05_references/hades_boon_and_roguelike_random_system_reference.md`
- `design/梦境支配者核心玩法/assets/asset_manifest.csv`

Current Unity facts:

- Unity version: `6000.4.10f1`
- Render pipeline: URP `17.4.0`
- Input: New Input System `1.19.0`, default `InputSystem_Actions.inputactions`
- Build Settings: only `Assets/Scenes/SampleScene.unity`
- Current gameplay scripts: none beyond Unity template Readme scripts
- Current imported game assets under `Assets`: none
- Unity MCP: available for console, scene, command, and capture validation; asset writes through MCP are limited and should be avoided

## 2. P0 Interpretation

There are two P0 scopes in the design material:

- Asset inventory MVP: one playable battle demo with HUD and result flow.
- Core gameplay P0: a complete 10-layer route ending in a Boss fight.

Implementation decision:

1. Build a P0.0 graybox vertical slice first, with the full moment-to-moment combat loop.
2. Expand that slice into P0.1 10-layer route structure and Boss resolution.
3. Treat the final P0 acceptance as the 10-layer route with the Boss and result screen.

This preserves the full design goal without blocking early playability behind every route node and asset.

## 3. Core Gameplay Loop

Full P0 loop:

1. Main menu.
2. Character select or default start.
3. Route map chooses a node.
4. Node loads battle, event, shop, blessing offering, partner, rest cat nest, elite, or Boss content.
5. Battle nodes run bedroom dream bed-defense combat.
6. Player switches cats, moves, releases skills, and manages bed pressure.
7. Enemies attack the bed or cats.
8. Four core values evolve: owner sleep, cat HP, team poop gauge, team hunger gauge.
9. Player uses litter box and feeder on the map to handle biological pressure.
10. Node resolves with dried fish, shared experience, blessing, partner, shop, event, or result.
11. Layer 10 resolves through `Call Tyrant` Boss fight and victory/failure screen.

P0.0 graybox loop:

Main menu -> default three-cat start -> bedroom dream defense -> waves -> Boss-lite wave -> victory/failure.

## 4. Core Systems

Recommended runtime folders under `Assets/TheCat/Scripts/Runtime`:

- `Core`: bootstrap, service registry, event bus, save-free run context, common utility.
- `Gameplay`: game state machine, battle state, run state, node resolution.
- `Input`: new Input System facade, rebinding-ready action names, UI input bridge.
- `Combat`: health, damage, targeting, auto attack, skill execution, status tags, hit events.
- `Characters`: cat roster, active cat switching, cat controller, cat runtime state.
- `Enemies`: enemy runtime, AI states, spawner, wave director, Boss controller.
- `Roguelite`: route graph, node definitions, rewards, blessings, events, shop stubs.
- `UI`: HUD presenters, menu flow, result screens, debug overlay.
- `Data`: ScriptableObject definitions and tuning assets.
- `Tools`: editor utilities, graybox scene builder, validation commands.
- `Tests`: EditMode and PlayMode test assemblies.

## 5. Data Model

Use ScriptableObjects for game content and serializable runtime structs/classes for state.

Data definitions:

- `CatDefinition`: id, display name, role, authority, attribute, max HP, defense, move speed, base attack, skill ids, visual id, voice ids.
- `SkillDefinition`: id, owner cat id, slot, cooldown, hunger cost, targeting mode, effects, status tag applications.
- `StatusTagDefinition`: id, display name, target type, duration, magnitude, stack policy, visual token.
- `EnemyDefinition`: id, role, max HP, attack, move speed, bed damage, player damage, behavior type, status response profile, visual id.
- `WaveDefinition`: layer, node type, spawn groups, budget, duration target, elite/boss flags.
- `BossDefinition`: phases, summon pattern, app projectile pattern, bed damage rules.
- `TuningDefinition`: seven P0 v1.1 tuning knobs.
- `BlessingDefinition`: id, authority, rarity, numeric/mechanic type, effect payload.
- `RouteNodeDefinition`: node type, layer, rewards, linked nodes, encounter id.
- `DreamEventDefinition`: three options, costs, rewards, temporary modifiers.

Runtime state:

- `RunState`: current layer, route graph, cats unlocked, blessings, dried fish, shared experience, metrics.
- `BattleState`: phase, timer, active wave, active cat, bed state, enemies alive, interactables, result.
- `TeamVitals`: sleep, poop, hunger, digestion timer.
- `CatRuntimeState`: current HP, shield, weak timer, skill cooldowns, invulnerability timer.
- `StatusInstance`: tag id, source id, target id, duration remaining, magnitude, stack data.
- `TelemetryMetrics`: per-layer time, sleep delta, litter box uses, feeder uses, poop incidents, weak incidents, node success/failure time.

## 6. Scene Structure

Target scenes under `Assets/TheCat/Scenes`:

- `TC_Boot.unity`: bootstrap, persistent systems, preload minimal data.
- `TC_MainMenu.unity`: main menu and default-start entry.
- `TC_RouteMap.unity`: 10-layer node selection, can be UI-only in P0.
- `TC_BedroomDream.unity`: core defense battle graybox, reused for normal/elite/Boss with different definitions.
- `TC_RestNest.unity`: no-enemy recovery node with bed, litter box, feeder.
- `TC_StatusTagTest.unity`: small validation scene for sleep, slow, knockback, mark, shield.

P0.0 may combine menu and battle into one scene for speed, but the code should not assume one permanent scene.

Bedroom dream graybox layout:

- Bed at center.
- Spawn gates on four sides.
- Litter box near enough for hard-pressure handling.
- Feeder farther away to create route-planning pressure.
- Camera top-down or 2.5D orthographic.
- Simple colored primitives are acceptable before art import.

Distance test targets from numeric review:

- Litter box: near to medium distance, about 2-4 seconds from bed.
- Feeder: medium distance, about 4 seconds from bed.
- 6-second distance should be tested but not default for P0.0.

## 7. Four Core Values

Use P0 v1.1 light-punishment tuning as the first implementation, not the full v1.0 punishment stack.

Owner sleep:

- Current and max start at 100.
- Sleep reaching 0 is the only failure condition.
- Bed damage comes from enemies or Boss projectiles.
- UI states: stable, uneasy, danger, critical.

Cat HP:

- Each cat owns independent HP.
- Active cat can become weak at 0 HP.
- P0.0 simplified weak rule: 20 seconds weak, recover at 30% HP, cannot switch to weak cats, current cat auto-switches to highest HP available cat.
- Continuous weak stacking and full edge-case rules are P1.

Team poop gauge:

- Shared 0-100 gauge.
- Grows naturally; digestion doubles growth.
- Full gauge starts countdown.
- P0 v1.1: early layers use longer countdown and lighter sleep-max punishment.
- Litter box is the main solution.

Team hunger gauge:

- Shared 0-100 gauge.
- Natural drain plus skill hunger costs.
- Low hunger mainly reduces output; avoid stacking heavy movement/cooldown punishment with high poop in P0.0.
- Feeder restores hunger and starts digestion.

Seven P0 tuning knobs:

1. Owner sleep damage taken.
2. Sleep max penalty from poop incident.
3. Poop natural growth rate.
4. Digestion poop multiplier.
5. Litter box reduction amount.
6. Hunger drain coefficient.
7. 10-layer difficulty multiplier.

## 8. P0 Status Tags

Only five primary status tags enter P0:

| Tag | Id | Main targets | Required P0 behavior |
|---|---|---|---|
| 安眠 | `sleep_stable` | Owner, bed zone, team | Restore or stabilize sleep pressure. |
| 缓速 | `slow` | Enemies, areas | Reduce enemy movement or approach rhythm. |
| 击退 | `knockback` | Enemies | Move normal enemies away; Boss gets reduced stagger. |
| 标记 | `mark` | Elite, Boss, key enemies | Visible focus target; affects damage, targeting, or reward at least once. |
| 护盾 | `shield` | Cats, bed zone | Absorb or reduce damage with clear remaining-state feedback. |

Status implementation requirements:

- Data definition.
- Runtime application/removal.
- At least one skill source.
- Enemy response.
- HUD or visual token.
- Test scene or automated validation.

## 9. P0 Cats

Initial three cats are the P0 combat identity:

塞班 / 圣剑士:

- Role: front-line bed defense.
- Authority / attribute: Oath / Sun.
- Tags: knockback + shield.
- P0 skills: oath shield, sword sweep/charge, shield-linked bed protection, Boss-safe knockback reduction.

奈芙蒂斯 / 月沙特工:

- Role: ranged zone control and wave clear.
- Authority / attribute: Dominion / Earth.
- Tags: slow + mark.
- P0 skills: moon-sand obelisk/turret, quicksand zone, pyramid blocker, marked/controlled target damage.

铃音 / 安眠巫女:

- Role: sleep stabilization, healing, safety window.
- Authority / attribute: Rhythm / Moon.
- Tags: sleep_stable + shield, with small slow/knockback support.
- P0 skills: sleep bell or healing bell, ice/moon field, torii rescue ultimate, sleep restoration.

P1 cats:

- 影丸: mark + slow, single-target burst.
- 棉花: mark + slow, field persistence and team setup.
- 玉衡:反制 / shield-adjacent, Boss counter.

## 10. P0 Enemies And Boss

Priority implementation order:

1. 黑泥梦魇: basic melee bed pressure.
2. 冷光灯影: ranged pressure on player, fallback bed attack.
3. 来电暴君: Boss summon plus app projectile.
4. 梦轨小火车: charge warning and shield counter.
5. 红眼闹铃: mid-range AOE pressure.
6. 未读红点小飞虫: flying bed attachment pressure.
7. 坠梦玩具熊: elite jump slam.

Naming decision:

- Use `CallTyrant` / `来电暴君` in code and data.
- Treat `手机梦魇` as a descriptive alias in text.

Enemy behavior requirements:

- Clear target priority.
- Bed damage path.
- Player damage path where applicable.
- Status tag responses.
- Telegraphs for ranged, charge, jump, and Boss attacks.
- Graybox-readable shapes before final art.

## 11. Roguelite Route

P0 route structure:

- 10 layers.
- Layer 1 fixed defense.
- Layer 10 fixed Boss.
- Layers 2-9 offer 2-4 reachable nodes.
- Partner opportunities are expected around layers 3 and 6.
- Layer 9 favors shop, rest nest, or blessing offering.

Node types:

- Defense.
- Elite.
- Partner.
- Shop.
- Dream event.
- Blessing offering.
- Rest cat nest.
- Boss.

Blessing P0 scope:

- P0 only opens authority blessings.
- Attribute blessings are P1.
- Economy, route, reroll, discount, and similar effects belong to event items, not normal blessing offerings.

P0 implementation approach:

- Start with deterministic route template.
- Add seeded random graph after battle state and rewards are stable.

## 12. UI And UX

P0 HUD must show:

- Owner sleep current/max and danger state.
- Active cat HP and shield.
- Three cat portraits with active/weak/cooldown states.
- Poop gauge and countdown.
- Hunger gauge and digestion state.
- Skill buttons or key hints with cooldown and hunger cost.
- Current layer/node/wave.
- Interaction progress for litter box, feeder, and bed repair.
- Result screen with success/failure reason and metrics.

Menu flow:

- Main menu.
- Character select or default start.
- Route map.
- Pause.
- Victory/defeat result.

Terminology decision for implementation:

- Use `睡眠度` for owner sleep.
- Use `屎意值` for poop gauge.
- Use `饱肚度` for hunger/fullness gauge.
- Avoid mixing `催眠稳定度`, `拉屎度`, and `饱腹度` in code identifiers.

## 13. Asset Integration Spec

Current art source folder:

- `design/梦境支配者核心玩法/assets`

Future Unity asset target:

- `Assets/TheCat/Art/Characters`
- `Assets/TheCat/Art/Enemies`
- `Assets/TheCat/Art/Levels`
- `Assets/TheCat/Art/UI`

Import rules:

- Import only a small named batch at a time.
- Preserve source manifest references in a local asset log.
- Let Unity generate `.meta` files once assets enter `Assets`.
- Keep source images in `design` untouched.
- Prefer graybox primitives until code and scene contracts stabilize.

Art direction constraints from design:

- Cats remain non-human cat bodies.
- Hand-drawn dream style.
- Each cat carries civilization symbols without becoming a human costume.
- Enemies share black mud, dream crack, or pollution trail motifs.
- Boss app icons must avoid real brands and readable trademark text.
- UI uses dream/sleep/cat motifs with readable combat-first layout.

Generated asset policy:

- Establish or reference a style sheet before each generation batch.
- Generate from existing concept, turnaround, or reference where possible.
- Record file name, purpose, size, import path, source prompt/reference, and consistency check.
- Do not spend Unity asset-generation credits unless explicitly approved for a concrete asset.

## 14. Testing And Validation

Local validation:

- Compile through Unity.
- EditMode tests for value models, status tag stacking, route generation, reward selection.
- PlayMode tests for battle result, cat switching, interactables, simple wave completion.

Unity MCP validation:

- Console logs have zero errors.
- Build Settings include the intended scenes.
- Scene hierarchy has required root objects.
- Read-only command confirms manager components and data references.
- Scene capture verifies bed, cat, enemies, litter box, feeder, and HUD are visible.
- Status tag test scene verifies slow, knockback, mark, shield, and sleep effects.

Graybox metrics:

- Per-layer clear time.
- Per-layer sleep net change.
- Litter box uses.
- Feeder uses.
- Poop incidents.
- Sleep max loss.
- Cat weak incidents.
- Node success/failure time.
- Failure layer distribution.

## 15. Phase Plan

A. Reconnaissance and design distillation:

- Complete enough to proceed.
- Outputs are this blueprint, agent workflow, and development log.

B. Engineering framework setup:

- Add `Assets/TheCat` structure.
- Add runtime asmdef and tests asmdef.
- Add event bus, state machine, input facade, data definitions, tuning models, debug metrics.
- Create minimal boot/menu/battle scene contracts.

C. Combat graybox:

- Bed, player movement, three-cat switching, auto-target, enemies moving to bed, attacks, waves, result flow.

D. Four core values:

- Owner sleep, cat HP, poop, hunger, litter box, feeder, weak state, UI and telemetry.

E. Cats and skills:

- Saiban, Nephthys, Suzune P0 skills, passives, ultimates, status tag links.

F. Enemies and Boss:

- Black Mud Nightmare, Cold Light Shadow, Call Tyrant, warning telegraphs, Boss waves.

G. Roguelite route:

- 10-layer route, node rewards, partner, shop, authority blessings, events, rest nest.

H. UI and feedback:

- Main menu, HUD, portraits, skill buttons, statuses, results, pause, settings, small feedback animation.

I. Assets:

- Import curated assets, create placeholders where needed, maintain asset manifest and consistency checks.

J. Polish and acceptance:

- MCP run/capture/console validation, bug fixing, tuning, feel passes, P0 demo readiness.

## 16. Final P0 Acceptance Checklist

- Unity opens the project without Console errors.
- A player can start from menu and reach combat.
- The player can complete a route through a Boss win or lose to sleep reaching 0.
- Three P0 cats are playable and switchable.
- Auto-targeting and skill release are functional.
- Bed damage and sleep failure are functional.
- Cat HP and simplified weak state are functional.
- Poop, hunger, litter box, feeder, and digestion are functional.
- Sleep, slow, knockback, mark, and shield each have data, source, UI/visual feedback, enemy response, and validation.
- Black Mud Nightmare, Cold Light Shadow, and Call Tyrant cover basic P0 pressure.
- 10-layer route and node rewards are playable.
- HUD and result screens make the state legible.
- Code structure is modular and testable.
- Asset usage is logged and stylistically consistent.
- Development log records completed work, gaps, tests, design deviations, and next tasks.
