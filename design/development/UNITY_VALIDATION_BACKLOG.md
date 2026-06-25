# Unity Validation Backlog

Current note: this backlog began as a 2026-06-13 MCP-blocked validation list.
That MCP status is historical tooling context, not the current runtime evidence
boundary.

As of 2026-06-25, normal Unity Editor Play Mode acceptance has current passing
evidence in:

- `design/development/unity_batchmode/P0_PLAYMODE_ACCEPTANCE_SMOKE_REPORT.md`
- `design/development/screenshots/p0-playmode-smoke`

The 2026-06-20 MCP rechecks still matter for automation availability:

- `Unity_GetConsoleLogs` returned `Connection revoked`.
- `Unity_GetUserGuidelines` returned `Connection revoked`.
- See `design/unity-mcp-smoke-report-2026-06-20.md`.

Use MCP only after editor-side approval is restored. Until then, rely on normal
Editor runs, generated reports, screenshot evidence, and focused compile/test
passes for current validation claims.

## Current Validation Status - 2026-06-25

- Runtime, EditMode, and Editor MSBuild passes are current after the cat-room
  scene-flow insertion.
- Unity scene setup validation includes `P0CatRoom` and passes with `0`
  warnings.
- Focused Unity EditMode coverage for cat-room presenter, main-menu coverage,
  battle-result coverage, state machine, and playable readiness passed `27/27`.
- Focused Unity EditMode coverage for dream-map route state, battle start
  context, route-map surface, and playable readiness passed `78/78`.
- Focused Unity EditMode coverage for Egypt readiness, route-map placeholder
  surface, cat-room dream-entry copy, route state, and battle start context
  passed `85/85`.
- Focused Unity EditMode coverage for D2 pause/settings and skill-selection
  acceptance passed `30/30`.
- Focused Unity EditMode coverage for E1 battle readability, command deck,
  feedback, readiness, code smoke consumers, and state-machine tests passed
  `47/47`.
- Play Mode screenshot/acceptance plan structure tests passed `17/17` after
  the runtime validation plan restored the exact `colored-turnaround` lock
  phrase.
- Focused Unity EditMode coverage for H1 loading/start and full settings hooks
  passed `33/33`.
- Focused screenshot/evidence coverage for the G1 refresh passed `18/18`.
- Focused Unity EditMode coverage for I1 battle-world label / overlay hierarchy
  passed `19/19` in `Logs/p0_i1_world_label_overlay_final4_editmode_20260625.xml`.
- Normal Editor Play Mode acceptance passed with `8/8` evidence checks after
  the I1 final run in `Logs/P0PlayModeAcceptanceVisual_I1_final_20260625.log`.
  The current report has no failures and `0` pending warnings.
- The current screenshot smoke validates 11 screenshots, including
  `02-cat-room.png`, and verifies the main menu -> cat room -> route path.
  The 2026-06-25 21:22 refresh also verifies that default battle-world
  diagnostic labels no longer overlap the normal battle HUD or battle-result
  overlay. These screenshots are baseline smoke evidence, not candidate import
  or final art acceptance.
- Full offline acceptance is not green: `P0_OFFLINE_ACCEPTANCE_REPORT.md`
  currently fails asset gates tied to starter-cat source-lock/turnaround/
  strict-candidate evidence. This is an asset evidence blocker, not a cat-room
  or dream-map compile, scene, focused-test, or Play Mode failure.

## Historical Offline Status - 2026-06-20

- Runtime C# sources compile with a local Roslyn check using Unity 6000.4.10f1
  Mono/UnityEngine reference assemblies and `Unity.InputSystem.dll`.
- EditMode test sources compile with the same offline reference set and Unity's
  bundled `nunit.framework.dll`.
- Current EditMode source contains 89 `[Test]` markers.
- `git diff --check` passes.
- Offline reflection execution was attempted but blocked by the host with
  Windows `Access denied`; this is not counted as a test failure.
- Latest offline UI pass on `2026-06-20 16:24 +08:00`: `TheCat.Runtime.csproj`
  and `TheCat.EditModeTests.csproj` both built with 0 warnings and 0 errors
  after the Chinese UI text, responsive IMGUI scale changes, `P0ChineseUiCoverage`
  smoke-suite gate, and status/cat HUD text validation sync. At that time,
  Editor screenshots and Console checks were still pending until Unity MCP or
  normal Editor validation could run.

## Known Unity-Stale Evidence

- `Library\ScriptAssemblies\TheCat.Runtime.dll` timestamp:
  `2026/6/13 2:45:30`.
- `Library\ScriptAssemblies\TheCat.EditModeTests.dll` timestamp:
  `2026/6/13 2:45:30`.
- Those timestamps predate the route-map, Boss, route reward, authority
  blessing combat, reward-choice, status UI, starter selection, settlement,
  pause/settings, enemy warnings, bed-care interaction, keyboard input commands,
  sleep max loss metrics, sleep-stable bed status, Saiban bed shield passive,
  Nephthys controlled-target passive, Suzune poop countdown relief, and agent
  workflow changes, persistent run core values, RestNest recovery, persistent
  cat HP, RestNest cat recovery, DreamEvent next-battle effects, and concrete
  shop supply purchases, Red Eye Alarm, Unread Red Dot Flyer, Dream Rail Toy
  Train, Falling Dream Teddy, and route branching.

## Required Checks After MCP Re-Approval

0a. P0 Chinese UI and layout scale pass:
   - Open main menu, route map, bedroom battle HUD, battle result, and route
     settlement surfaces.
   - Capture screenshots at 1280x720, 1600x900, 1920x1080, and 1024x768 or a
     similarly narrow editor Game view.
   - Verify Chinese labels do not overlap, clip button text, or stack into the
     same screen area.
   - Verify IMGUI scroll views are usable in battle HUD and route map panels.
   - Check Console for IMGUI layout, missing texture, font, and asset reference
     errors after Play Mode navigation.

0b. Batch 54 bedroom interactable Unity preflight:
   - Review `P0BedroomInteractableBatch54UnityPreflight` output before touching
     any `Assets/TheCat/Art/Scenes/BedroomDream` sprite files.
   - Capture bed, litter box, and feeder runtime screenshots in the bedroom
     battle scene, including interaction range/blocked-state UI.
   - Compare candidate scale against current `thecat_prop_bed`,
     `thecat_prop_litterbox`, and `thecat_prop_feeder` runtime sprites.
   - Check Sprite import settings for any approved install target.
   - Verify scene/prefab binding, pathing readability, and Console state before
     changing the formal install decision.
   - Do not copy Batch 54 candidate PNGs into `Assets` until this evidence is
     present.

0. Starter-cat Unity reference atlases:
   - Refresh `Assets/TheCat/Art/Characters/References`.
   - Inspect Sprite Single import settings for:
     - `thecat_cat_saiban_turnaround_reference_atlas_2304x768_v001.png`
     - `thecat_cat_nephthys_turnaround_reference_atlas_2304x768_v001.png`
     - `thecat_cat_suzune_turnaround_reference_atlas_2304x768_v001.png`
   - Confirm the atlases are not referenced by runtime SpriteRenderers or HUD
     bindings.
   - Capture active-cat Saiban, Nephthys, and Suzune Play Mode screenshots and
     compare them against the three installed atlases before any formal
     starter-cat body-art import decision.

1. Console and compile state:
   - `Unity_GetConsoleLogs` or `Unity_ReadConsole`
   - read-only compile probe referencing:
     - `RouteMapController`
     - `RunProgressionState`
     - `P0BlessingCatalog`
     - `BattleSimulation`
     - `BattleModifierSet`
     - `RouteRewardChoice`
     - `StatusDisplayFormatter`
     - `P0RunSession.StartNewRun(IEnumerable<string>)`
     - `P0RunSettlementSummary`
     - `P0RuntimeSettings`
     - `EnemyWarningFormatter`
     - `BattleSimulation.RecordBedCareUse`
     - `NodeMetrics.BedCareUses`
     - `NodeMetrics.SleepMaxLost`
     - `RunMetricsSummary.SleepMaxLost`
     - `P0KeyboardInputMap`
     - `GrayboxBattleController.ExecuteInputCommand`
     - `BattleSimulation.BedStatuses`
     - `SkillCastResult.BedShieldApplied`
     - `BattleSimulation.SaibanBedShieldRatio`
     - `BattleSimulation.NephthysControlledTargetDamageMultiplier`
     - `BattleSimulation.ApplyDamageToNearestEnemy(float, CatBattleState)`
     - `TeamPoopGauge.ExtendCountdown`
     - `SkillCastResult.PoopCountdownExtendedSeconds`
     - `BattleSimulation.SuzuneSleepBellPoopCountdownExtensionSeconds`
     - `RunCoreValues`
     - `RunProgressionState.CoreValues`
     - `BattleSimulationConfig.StartingPoop`
     - `BattleSimulationConfig.StartingHunger`
     - `P0RouteRewardResolver.GetDefaultPlaceholderChoice`
     - `RouteRewardChoice.OwnerSleepRestored`
     - `RunCatVitals`
     - `RunCatVitalSnapshot`
     - `RouteRewardChoice.CatHpSafePercent`
     - `P0RunSettlementSummary.WeakCatCount`
     - `RunPendingBattleModifiers`
     - `RunPendingBattleModifierSnapshot`
     - `RouteRewardChoice.FishTreatsCost`
     - `RouteRewardChoice.NextBattleSkillDamagePercent`
     - `RouteRewardChoice.NextBattlePoopGrowthPercent`
     - `P0PrototypeCatalog.RedEyeAlarmId`
     - `P0PrototypeCatalog.UnreadRedDotFlyerId`
     - `P0PrototypeCatalog.CreateRedEyeAlarmEliteWave`
     - `EnemyWarningFormatter.RangedPressureWarningThresholdSeconds`
     - `EnemyWarningFormatter.FlyingAttachWarningThresholdSeconds`
     - `P0PrototypeCatalog.DreamRailToyTrainId`
     - `P0PrototypeCatalog.FallingDreamTeddyId`
     - `P0PrototypeCatalog.CreateFallingDreamTeddyEliteWave`
     - `EnemyWarningFormatter.ChargeWarningThresholdSeconds`
     - `EnemyWarningFormatter.JumpSlamWarningThresholdSeconds`
     - `RouteDefinition.LayerOptions`
     - `RunRouteState.SelectCurrentNode`
     - `RunRouteState.CurrentLayerOptions`
2. EditMode tests:
   - run all 89 tests through Unity Test Runner or direct Unity MCP calls.
3. Main-menu smoke:
   - load `P0MainMenu`
   - toggle starter cats and start a selected run
   - confirm `P0GrayboxBattle` loads with no Console errors.
   - confirm selected starter roster is reflected in battle cat buttons.
   - restore default trio and confirm all three cats appear.
4. Route-map smoke:
   - clear layer 1 battle
   - continue to `P0RouteMap`
   - confirm current-layer route choices appear when a layer has multiple
     options
   - select at least one alternate route node and confirm the selected marker
     moves
   - resolve placeholder nodes
   - confirm reward choices display on non-combat nodes.
   - click at least one non-default reward choice and confirm the selected
     reward is applied.
   - confirm wallet, roster, and blessing counts update.
5. Boss smoke:
   - enter layer 10 `boss_call_tyrant`
   - confirm Call Tyrant spawns
   - confirm summon and throw counters increment
   - confirm Boss summon/throw warning text appears near countdown thresholds.
   - confirm Boss victory/failure records route completion.
   - confirm route-map settlement appears after Boss resolution.
   - confirm settlement includes accumulated battle duration, sleep delta,
     sleep max loss, bed/litter/feeder use, poop incidents, and weak incidents.
6. Blessing combat smoke:
   - acquire at least one authority blessing
   - enter a later battle
   - confirm HUD shows blessing count
   - confirm shield, status duration, or recovery effects differ from neutral.
7. Status UI smoke:
   - apply slow, knockback, mark, and shield during graybox battle
   - cast Suzune sleep-bell and confirm `sleep_stable` appears as a bed tag
   - cast Saiban oath shield and confirm `shield` appears as a bed tag
   - apply slow/mark with Nephthys and confirm her sourced damage is higher
   - confirm enemy labels use visual tokens, magnitude, and remaining time
   - confirm bed tag text expires after the P0 duration
   - confirm cat buttons/HUD show shield token while active
8. Pause/settings smoke:
   - pause battle and confirm battle time/enemy movement stop while UI remains
     responsive
   - switch between 0.5x, 1x, and 1.5x battle speed and confirm battle time
     scales accordingly
9. Enemy warning smoke:
   - let a Black Mud Nightmare approach the bed and confirm `Bed contact`
     appears on the enemy label and HUD
   - confirm warning text clears or changes after the enemy reaches the bed or
     is defeated
10. Bed-care interaction smoke:
   - click `Bed Care` during an active battle
   - confirm owner sleep increases by the P0 restore amount, clamped to max
   - confirm team hunger decreases by the P0 cost
   - confirm live HUD, node result, and route settlement include bed-care count
11. Sleep max loss smoke:
   - trigger a poop incident during an active battle
   - confirm owner sleep max decreases
   - confirm live HUD, node result, and route settlement show sleep max loss
12. Suzune poop countdown relief smoke:
   - force poop countdown during battle
   - cast Suzune sleep bell while countdown is active
   - confirm countdown increases by 8 seconds and HUD skill feedback shows
     `poop +8s`
   - confirm casting before countdown starts does not change poop countdown
13. RestNest run-core recovery smoke:
   - enter a battle from a run with non-default sleep, poop, and hunger
   - confirm the battle HUD starts from those values
   - finish the battle and confirm the route map preserves the resulting values
   - resolve a RestNest node and confirm sleep increases by up to 25, poop
     decreases by up to 30, and hunger is at least 80
   - confirm weak cat timers are cleared and low-HP cats recover to at least
     70% max HP
   - enter the next battle and confirm the recovered values are used as the
     next battle's starting state
14. Cat HP persistence smoke:
   - damage or weaken an active cat during battle
   - finish the node and confirm route-map cat HP text preserves the snapshot
   - enter another battle and confirm the cat starts with the saved HP/weak
     state before any RestNest recovery
15. DreamEvent next-battle effect smoke:
   - resolve a DreamEvent with `Breathe Catnip Residue`
   - confirm route-map pending event text shows skill and poop modifiers
   - enter the next battle and confirm the battle start message reports event
     pressure
   - confirm the pending modifier is consumed after battle start and does not
     apply to a second battle
16. Shop concrete purchase smoke:
   - enter shop with at least 3 fish treats
   - buy `Bed Patch` and confirm fish is spent and owner sleep is restored
   - enter shop with at least 2 fish treats
   - buy `Litter Sachet` or `Late Kibble` and confirm poop/hunger core values
     update on the route map
   - enter shop with too few fish treats and confirm `Free Sample` is available
17. Saiban bed-shield passive smoke:
   - cast Saiban oath shield
   - let a bed-contact enemy or Call Tyrant throw hit while the bed shield is
     active
   - confirm bed shield magnitude decreases before owner sleep takes damage
18. Red Eye Alarm elite smoke:
   - enter layer 9 `elite_red_eye_alarm`
   - confirm one Red Eye Alarm spawns
   - confirm Unread Red Dot Flyers spawn in groups
   - confirm `Ranged pressure` and `Flyer attach` text appears near warning
     thresholds
   - confirm the wave can be cleared and route progression reaches the Boss
     node afterward
19. Dream Rail Toy Train smoke:
   - enter layer 6 `layer_06_defense`
   - confirm Dream Rail Toy Train spawns after the first pressure wave
   - confirm `Charge lane` warning text appears near warning threshold
   - confirm shield/knockback/slow interactions remain readable against the
     faster charger pressure
20. Falling Dream Teddy content smoke:
   - select `elite_falling_dream_teddy` from layer 8 or layer 9 route choices
   - confirm Falling Dream Teddy spawns with pressure adds
   - confirm `Jump slam` warning text appears near warning threshold
   - confirm the wave can be cleared without Console errors
21. Keyboard input smoke:
   - use number keys to switch cats
   - use Q/W/E to fire the active cat's three skill slots
   - use P or Esc to pause/resume
   - use F1/F2/F3 to change battle speed
   - use B/L/F for bed care, litter box, and feeder
   - use Enter after battle resolution to continue route
   - use N to restart run
22. Scene/capture checks:
   - capture `P0MainMenu`
   - capture `P0RouteMap`
   - capture `P0GrayboxBattle`
   - verify no overlapping critical OnGUI controls at common desktop size.
23. Final Console:
   - verify no errors or new warnings after the complete smoke path.
24. Authority blessing upgrade smoke:
   - claim all three P0 authority blessings through blessing offerings
   - confirm the next blessing offering switches from new blessings to upgrade
     choices
   - choose a free altar upgrade and confirm the selected blessing reaches level
     2 on the route map
   - enter a shop with an owned non-capped blessing and at least 4 fish treats
   - buy a blessing upgrade and confirm fish treats decrease by 4
   - confirm route-map HUD, battle HUD, and settlement show blessing count and
     total blessing level
   - enter a battle with a level 2 blessing and confirm the matching shield,
     knockback, status-duration, or recovery effect is stronger than level 1
25. Route node presentation smoke:
   - load `P0MainMenu`
   - confirm the route preview shows every branch option with player-facing
     titles instead of raw `contentId` strings
   - confirm route preview text remains readable at common desktop size
   - clear layer 1 and enter `P0RouteMap`
   - confirm current node summary shows title, risk, reward, and detail text
   - confirm branch buttons mark the selected node and show title/risk/reward
     labels
   - select `elite_falling_dream_teddy` and confirm the button, map row, and
     battle-start message all use `Falling Dream Teddy`
   - confirm reward choice headers name the current non-combat node
26. Battle reward report smoke:
   - clear layer 1 `Bedroom Threshold`
   - confirm the victory message includes the completed node title, `+1 fish`,
     and the next node title
   - return to `P0RouteMap`
   - confirm `Last Node` shows the same completion report
   - clear an elite node and confirm `+2 shard +1 fish` appears in the battle
     result or route-map report
   - clear the Boss node and confirm the report includes `+5 shard +3 fish`
     and `route cleared`
   - lose any battle and confirm the report uses `Failure` and `route failed`
     without adding battle rewards
27. Starter skill presentation smoke:
   - load `P0MainMenu`
   - toggle each starter cat on and confirm skill previews appear under the
     selected cats
   - start a default trio run
   - confirm battle skill buttons show `Silver Oath Shield`, `Round Shield
     Rush`, and `Crown Judgement` for Saiban
   - switch to Nephthys and confirm `Moon-Sand Obelisk`, `Quicksand Trap`, and
     `Royal Mark`
   - switch to Suzune and confirm `Sleep Bell`, `Ice Blossom Prayer`, and
     `Moon Torii Seal`
   - confirm each button shows slot, hunger cost, and effect hint without
     overlapping other HUD controls
   - cast at least one skill on cooldown and confirm the cooldown message uses
     the player-facing skill name
28. Status and enemy warning presentation smoke:
   - apply `Mark`, `Slow`, `Knockback`, `Shield`, and `Sleep Stable`
   - confirm status text shows display names plus visual tokens, such as
     `Mark (royal_eye)`
   - confirm enemy warnings use display labels with spaces, not underscore
     debug ids
   - confirm Black Mud Nightmare near bed shows `Bed contact`
   - confirm Dream Rail Toy Train shows `Charge lane`
   - confirm Red Eye Alarm shows `Ranged pressure`
   - confirm Unread Red Dot Flyer shows `Flyer attach`
   - confirm Falling Dream Teddy shows `Jump slam`
   - confirm Call Tyrant warning text shows `Boss summon` and `Boss throw`
29. Cat presentation smoke:
   - load `P0MainMenu`
   - confirm starter toggles show `Saiban`, `Nephthys`, and `Suzune` with
     readable role/authority/attribute labels
   - confirm starter toggles do not show raw authority ids, attribute ids, or
     cat ids such as `saiban`
   - start a default trio run
   - confirm battle run-state cat HP lines show cat names instead of raw ids
   - clear layer 1 and return to `P0RouteMap`
   - confirm route-map roster and cat HP summaries show cat names
   - enter a partner node and confirm the reward summary says `Shadowmaru`,
     not `shadowmaru_preview`
30. Authority blessing presentation smoke:
   - claim at least one authority blessing
   - open a shop with at least 4 fish treats
   - confirm shop upgrade choices show blessing display names such as
     `Oath Bedline`
   - claim all three authority blessings
   - open a blessing-offering node that offers upgrades
   - confirm free altar upgrade choices show blessing display names, not raw
     ids such as `authority_oath_bedline`
31. Core value HUD presentation smoke:
   - start `P0GrayboxBattle`
   - damage owner sleep below 60%, 30%, and 15%
   - confirm owner sleep text shows `Uneasy`, `Danger`, and `Critical` plus the
     matching action hints
   - trigger a poop countdown and confirm the HUD shows countdown seconds and
     `use litter box now`
   - use the feeder and confirm hunger text shows damage multiplier plus
     digestion remaining seconds
   - return to `P0RouteMap` and confirm the route core row uses the same
     player-facing labels
   - clear or fail a route and confirm settlement final core uses the same
     labels without overlapping other result rows
32. Graybox navigation and interaction range smoke:
   - start `P0GrayboxBattle`
   - confirm an `ActiveCat` marker exists near the bed at battle start
   - move with arrow keys and confirm the marker moves within the bedroom bounds
   - switch cats and confirm the marker color changes by role
   - stand near the bed and confirm bed care succeeds
   - move away from the bed and confirm bed care reports that the cat must move
     closer
   - stand far from the litter box and feeder and confirm interactions are
     rejected
   - move near the litter box and feeder and confirm the interactions succeed
   - confirm the `Cat Position` HUD row updates distances while moving
33. Position-based enemy cat pressure smoke:
   - start `P0GrayboxBattle`
   - stand away from Black Mud Nightmare contact and confirm cat HP is not
     reduced by that enemy while it is outside melee pressure range
   - move the active cat marker into melee contact and confirm cat HP pressure
     begins
   - spawn Red Eye Alarm and confirm ranged pressure can affect the cat from a
     longer distance
   - confirm pressure feedback messages include approximate distance in meters
   - spawn Dream Rail Toy Train, Unread Red Dot Flyer, Falling Dream Teddy, and
     Call Tyrant and compare their pressure ranges against expected behavior
   - tune pressure ranges only after screenshot/play validation confirms marker
     scale and camera framing
34. Position-based auto attack targeting smoke:
   - start `P0GrayboxBattle`
   - move Saiban near and far from a Black Mud Nightmare and confirm auto attack
     only fires within close range
   - switch to Nephthys and confirm the same enemy can be auto-attacked from a
     longer distance
   - switch to Suzune and confirm her auto attack uses medium support range
   - place two enemies within range and confirm the closer enemy is targeted
   - confirm auto-attack feedback names the target and includes distance
   - confirm defeating the selected target removes it without damaging another
     active enemy
35. Position-aware active skill targeting smoke:
   - start `P0GrayboxBattle`
   - move Saiban out of directional range and press `W` for `Round Shield Rush`
   - confirm the skill reports that it needs a target in range
   - confirm hunger and cooldown do not change after the rejected cast
   - move Saiban into range and confirm `Round Shield Rush` targets the nearby
     enemy and reports target distance
   - switch to Nephthys and confirm `Quicksand Trap` can target from farther
     away than Saiban's directional skill
   - confirm `Sleep Bell` still casts without enemy target
   - confirm selected targets receive damage/status while other active enemies
     are unchanged
36. Skill cast preview smoke:
   - start `P0GrayboxBattle`
   - confirm each active skill button shows the skill label plus a preview line
     without overlapping nearby HUD text
   - put `Round Shield Rush` on cooldown and confirm its preview line shows the
     remaining cooldown instead of target information
   - move Saiban outside directional skill range and confirm the preview shows
     the required range and the button is disabled
   - move Saiban into range and confirm the preview names the target and shows
     approximate distance/range before casting
   - switch to Nephthys and confirm `Royal Mark`/`Quicksand Trap` previews use
     farther target ranges than Saiban's directional skill
   - switch to Suzune and confirm self, heal, or bed-zone skills show that no
     enemy target is needed and remain castable when no enemy is nearby
   - lower team hunger below at least one skill's cost and confirm the preview
     shows the low-hunger hint while preserving current P0 light-penalty
     behavior
37. Skill indicator Gizmos smoke:
   - start `P0GrayboxBattle` with Scene view or Game view Gizmos visible
   - confirm switching to Saiban auto-tracks `Round Shield Rush` rather than
     the no-target shield skill
   - confirm the tracked skill summary line matches the active ring/target
     marker in the scene
   - click `Track` beside each skill and confirm the scene indicator changes
     without casting the skill
   - move Saiban outside `Round Shield Rush` range and confirm a red
     missing-target cross appears at the active cat marker
   - move Saiban into range and confirm a ring, target line, and target wire
     sphere point to the selected enemy
   - switch to Nephthys and confirm `Quicksand Trap`/`Royal Mark` draw farther
     rings than Saiban's directional skill
   - put the tracked skill on cooldown and confirm the indicator remains
     visible with the disabled color
   - check that the `Track` buttons and indicator summary do not overlap skill
     button text at the default Game view size
38. Runtime skill indicator view smoke:
   - start `P0GrayboxBattle` with Game view Gizmos disabled
   - confirm a `SkillIndicatorView` child is created under `P0GrayboxBattle`
   - confirm `Round Shield Rush` shows a visible runtime range ring around the
     active cat marker
   - move Saiban out of range and confirm the runtime missing-target cross is
     visible without relying on Scene view Gizmos
   - move Saiban into range and confirm the runtime target line and target
     sphere point to the selected enemy
   - click `Track` on the no-target shield skill and confirm all enemy-target
     runtime visuals hide
   - put the tracked enemy skill on cooldown and confirm the runtime range and
     target visuals remain visible in the disabled color
   - switch to Nephthys and confirm the runtime ring is larger for
     `Royal Mark`/`Quicksand Trap`
   - inspect material/shader appearance in Game view and tune line width,
     y-offset, colors, or marker scale if readability is weak
39. Runtime enemy warning indicator smoke:
   - start `P0GrayboxBattle` with Game view Gizmos disabled
   - confirm each spawned enemy view creates an `EnemyWarningIndicator` child
   - let Black Mud Nightmare approach the bed and confirm `Bed contact` shows a
     visible ring plus countdown label
   - spawn Dream Rail Toy Train and confirm `Charge lane` shows a visible line
     toward the bed
   - spawn Cold Light Shadow or Red Eye Alarm and confirm `Ranged pressure`
     shows a visible line toward the bed/pressure target
   - spawn Unread Red Dot Flyer and confirm `Flyer attach` shows a ring warning
     around the flyer
   - spawn Falling Dream Teddy and confirm `Jump slam` shows a larger ring
     warning around the teddy
   - spawn Call Tyrant and confirm `Boss summon`/`Boss throw` warnings appear
     as their timers approach the threshold
   - confirm HUD warning text and runtime label use the same player-facing
     wording with no raw ids or underscores
   - confirm warning visuals follow enemies while they move and disappear when
     the enemy dies or its warning timer is outside threshold
   - check label overlap against enemy HP/status labels and tune height/color if
     readability is weak
40. Runtime status indicator smoke:
   - start `P0GrayboxBattle` with Game view Gizmos disabled
   - confirm `ActiveCatStatusIndicator` exists under the active cat marker and
     `BedStatusIndicator` exists under the bed marker
   - apply `Shield` with Saiban's `Silver Oath Shield` and confirm the active
     cat marker shows `Shield (oath_edge)` plus magnitude/time
   - confirm Saiban's bed-shield passive makes the bed marker show
     `Shield (oath_edge)` when bed shield is active
   - apply `Sleep Stable` with Suzune's bed-zone skill and confirm the bed
     marker shows `Sleep Stable (soft_blue_note)`
   - apply `Slow` with Nephthys and confirm the target enemy's
     `EnemyStatusIndicator` shows `Slow (moon_sand)`
   - apply `Mark` with Nephthys and confirm the target enemy marker shows
     `Mark (royal_eye)`
   - apply knockback with Saiban or Suzune and confirm the target enemy marker
     briefly shows `Knockback (silver_impact)`
   - confirm each status marker disappears after expiration or shield
     consumption
   - check status marker overlap against enemy HP labels, warning labels, skill
     range indicators, and HUD text
41. Battle Smoke Tools smoke:
   - start `P0GrayboxBattle`
   - click `Show Smoke Tools` and confirm the panel opens without hiding core
     HUD rows needed for battle reading
   - click each enemy spawn button: `Mud`, `Train`, `Cold`, `Alarm`, `Flyer`,
     `Teddy`, and `Boss`
   - confirm spawned smoke enemies appear in the scene, create enemy views, and
     use player-facing names
   - confirm smoke-spawned enemies immediately show their expected warning
     indicators because they spawn at warning-threshold timings
   - confirm `Boss` smoke spawn shows `Boss summon` and `Boss throw` warnings
     without waiting for a full Boss cycle
   - click `Enemy Slow`, `Enemy Mark`, and `Enemy Knock` and confirm the first
     active enemy receives the matching status marker and HUD text
   - click `Cat Shield`, `Bed Sleep`, and `Bed Shield` and confirm active cat /
     bed runtime status indicators update immediately
   - click `Sleep -35`, `Hunger -35`, and `Poop Countdown` and confirm core
     HUD labels change to the expected danger/countdown states
   - confirm no Console errors appear while repeatedly opening/closing the
     Smoke Tools panel and spawning enemies
42. Golden path simulator smoke:
   - run EditMode tests and confirm `P0GoldenPathSimulatorTests` passes in the
     Unity Test Runner
   - execute `TheCat/P0/Run Golden Path Smoke`
   - confirm the report summary says `Run Cleared`, `nodes 10/10`, `battles
     5/5`, and `boss observed`
   - confirm the Console prints `P0 Golden Path Acceptance`
   - confirm the acceptance report has zero failures
   - record any acceptance warnings for sleep, poop, hunger, total duration, or
     individual battle duration as tuning follow-ups
   - execute `TheCat/P0/Log Last Golden Path Report` and confirm it reprints
     the same battle report lines without rerunning the simulation
   - inspect the battle reports and confirm the route uses layer 1 defense,
     layer 3 cold-light elite, layer 6 defense, layer 9 red-eye elite, and the
     layer 10 Call Tyrant Boss
   - confirm the Boss report records at least one summon and one throw
   - confirm final settlement has dream shards, fish treats, one dream event,
     one shop purchase, one rest nest use, one authority blessing, and the
     preview partner recruited
   - compare the simulator's core-value snapshots against a manual Play Mode
     run and decide whether P0 v1.1 sleep, poop, and hunger pacing needs tuning
43. P0 scene setup validator smoke:
   - execute `TheCat/P0/Validate P0 Scene Setup`
   - confirm the Console report has zero errors
   - confirm the validator finds `P0MainMenu`, `P0RouteMap`, and
     `P0GrayboxBattle` scene assets
   - confirm Build Settings enabled scenes begin with main menu, route map, and
     graybox battle in that order
   - confirm deep inspection reports `P0MainMenuRoot` with
     `MainMenuController`
   - confirm deep inspection reports `P0RouteMapRoot` with
     `RouteMapController`
   - confirm deep inspection reports `P0GrayboxBattleRoot` with
     `GrayboxBattleController`
   - if the validator reports dirty-scene warnings, save or close modified
     scenes and rerun it before treating scene wiring as validated
44. Route-first start flow smoke:
   - start Play Mode in `P0MainMenu`
   - click `Start Default Route` and confirm `P0RouteMap` loads first
   - click `Main Menu`, then click `Start Selected Route` with a non-default
     starter selection and confirm it also loads `P0RouteMap`
   - from `P0RouteMap`, click `Enter Current Node` on layer 1 and confirm
     `P0GrayboxBattle` loads
   - clear or force-clear the battle, click `Continue Route`, and confirm the
     route map shows layer 1 completed and layer 2 current
   - return to `P0MainMenu`, click `Quick Battle`, and confirm it still loads
     `P0GrayboxBattle` directly for tuning workflows
   - confirm no Console errors appear while moving through this scene flow
45. Enemy view pool smoke:
   - start `P0GrayboxBattle`
   - open `Smoke Tools`
   - spawn several enemies, kill or clear them, then spawn another batch
   - confirm enemy views reappear without stale HP labels, gate text, status
     text, warning text, scale, or color from the previous enemy
   - apply `Enemy Slow`, `Enemy Mark`, and `Enemy Knock`, clear the enemy, then
     spawn a new enemy and confirm status indicators start empty
   - spawn `Boss`, allow or force a summon, clear enemies, and confirm reused
     views do not keep Boss summon/throw warning indicators
   - repeat spawn/clear cycles for at least 30 seconds and confirm no Console
     errors or visible flicker from pooled enemy view reuse
46. P0 status tag coverage matrix smoke:
   - run EditMode tests and confirm `P0StatusTagCoverageTests` passes in the
     Unity Test Runner
   - execute `TheCat/P0/Log Status Tag Coverage`
   - confirm the Console starts with `[TheCat] P0 Status Tag Coverage` and
     reports `P0 status tag coverage complete for 5 tag(s).`
   - confirm the dialog title is `P0 Status Tags Covered` when coverage is
     complete
   - confirm `P0StatusTagCoverage.EvaluatePrototypeCatalog()` reports complete
     coverage for five tags: `sleep_stable`, `slow`, `knockback`, `mark`, and
     `shield`
   - compare each matrix row with runtime visuals in `P0GrayboxBattle`:
     enemy status indicators, active cat status indicator, bed status
     indicator, HUD status text, and Smoke Tools buttons
   - confirm each tag has at least one player-accessible P0 source skill and
     the expected runtime response:
     `sleep_stable` restores owner sleep on the bed, `slow` reduces enemy
     movement, `knockback` pushes enemy time-to-bed, `mark` increases enemy
     damage taken, and `shield` absorbs cat/bed damage
   - confirm no Console errors appear while repeatedly applying all five tags
     through skills or Smoke Tools
47. P0 playable readiness gate smoke:
   - run EditMode tests and confirm `P0PlayableReadinessTests` passes in the
     Unity Test Runner
   - execute `TheCat/P0/Run Playable Readiness`
   - confirm the Console starts with `[TheCat] P0 Playable Readiness`
   - confirm the summary says `P0 playable readiness passed` with zero failures
   - confirm the report includes checks for scene flow, starter trio, starter
     skills, core enemies, route structure, battle waves, status tags, and
     golden path acceptance
   - if warnings appear, record them as tuning follow-ups; if failures appear,
     fix the failing gate before continuing toward final P0 acceptance
   - after this code-side readiness check passes, continue with Play Mode
     traversal from `P0MainMenu` through route, battle, Boss, and settlement
48. P0 battle HUD priority prompt smoke:
   - run EditMode tests and confirm `P0BattleHudPromptPresenterTests` passes in
     the Unity Test Runner
   - start `P0GrayboxBattle`
   - confirm the battle HUD includes a `Priority:` row near the core value rows
   - confirm the row is readable and does not overlap core values, skill
     buttons, Smoke Tools, status tags, or result summary text
   - use Smoke Tools or regular play to trigger sleep danger, poop countdown,
     hunger empty, weak cat, Boss warning, and near-bed enemy contact
   - confirm each case switches the prompt to the expected action:
     bed protection, litter box, feeder, cat switch, Boss preparation, or
     knockback/focus fire
   - clear a battle and confirm victory/defeat prompts tell the player to
     continue route or restart appropriately
49. P0 graybox telemetry report smoke:
   - run EditMode tests and confirm `P0GrayboxTelemetryTests` passes in the
     Unity Test Runner
   - execute `TheCat/P0/Run Golden Path Telemetry`
   - confirm the Console starts with `[TheCat] P0 Graybox Telemetry`
   - confirm the report lists node result, duration, sleep delta, poop
     incidents, sleep max loss, litter box uses, feeder uses, bed care uses,
     and weak incidents
   - complete at least one live `P0GrayboxBattle`, then execute
     `TheCat/P0/Log Current Run Telemetry`
   - confirm current-run telemetry matches the just-completed battle metrics
     shown in the HUD result summary
   - compare golden path telemetry and live telemetry before changing P0 v1.1
     sleep, poop, hunger, or interaction tuning
50. P0 code smoke suite:
   - run EditMode tests and confirm `P0CodeSmokeSuiteTests` passes in the Unity
     Test Runner
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm the Console starts with `[TheCat] P0 Code Smoke Suite`
   - confirm the report includes checks for golden path simulation, golden path
     acceptance, status tag coverage, route choice coverage, playable readiness,
     route map input coverage, and graybox telemetry
   - confirm the summary reports zero failures
   - record any warnings as tuning follow-ups, not blocking errors, unless the
     warning indicates a failed route, missing coverage, or unusable telemetry
   - after this suite passes, execute `TheCat/P0/Validate P0 Scene Setup`
     before beginning manual Play Mode traversal
51. P0 battle start context guard smoke:
   - run EditMode tests and confirm `P0BattleStartContextTests` passes in the
     Unity Test Runner
   - start a normal route battle from `P0RouteMap` and confirm the battle uses
     the current combat node wave
   - clear the normal route battle and confirm the current route node completes
     exactly once
   - advance the route to a non-battle node such as dream event, shop, partner,
     blessing, or rest nest
   - load `P0GrayboxBattle` directly while that non-battle node is current
   - confirm the battle starts as standalone graybox smoke and shows a message
     that the current route node is not a battle
   - clear or fail that standalone battle and confirm the non-battle route node
     remains current, pending battle modifiers remain unconsumed, run core
     values and cat vitals remain unchanged, and route telemetry is not polluted
     by the standalone battle
52. P0 route choice coverage smoke:
   - run EditMode tests and confirm `P0RouteChoiceCoverageTests` passes in the
     Unity Test Runner
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm the Console report includes `Route Choice Coverage` and marks it
     passed
   - start Play Mode in `P0MainMenu`, enter the route, and inspect the
     non-battle nodes from `P0RouteMap`
   - confirm dream event, partner, shop, blessing offering, and rest nest nodes
     each show at least one readable choice with no raw ids or underscore-heavy
     debug text
   - click a representative visible choice on each node type and confirm the
     route advances to the next layer
   - confirm default choice behavior is usable for keyboard/controller smoke or
     automated traversal when no manual choice is selected
   - verify shop choices apply cost/reward, blessing choices add authority
     blessing state, rest nest choices heal/recover, partner choices recruit a
     preview partner, and dream event choices change run state as shown
   - confirm no Console errors appear while opening and resolving route choices
53. P0 route map keyboard input smoke:
   - run EditMode tests and confirm `P0RouteMapCommandRouterTests`,
     `P0RouteMapInputCoverageTests`, and `P0CodeSmokeSuiteTests` pass in the
     Unity Test Runner
   - start Play Mode in `P0MainMenu`, start a default route, and confirm
     `P0RouteMap` is active
   - press `Enter` on layer 1 and confirm `P0GrayboxBattle` loads without
     advancing route progress before the battle resolves
   - return to route map after a completed battle and press `Enter` on the
     layer 2 non-battle default node; confirm the default reward applies and
     the route advances
   - start a fresh route or reach another branch layer, press `2` or `3` before
     selecting a reward, and confirm the current route option changes without
     completing the layer
   - after a non-battle route option is selected, press `1`, `2`, or `3` for a
     visible reward slot and confirm the selected reward applies and advances
     the route
   - press `N` on the route map and confirm a new route starts cleanly
   - execute `TheCat/P0/Run Code Smoke Suite` and confirm `Route Map Input
     Coverage` passes
   - confirm no Console errors appear while using route map keyboard input
54. P0 MCP-safe acceptance gate smoke:
   - execute `TheCat/P0/Run Acceptance Gates (Log Only)`
   - confirm the command completes without a modal dialog or MCP timeout
   - confirm the output reports:
     `P0 code smoke suite passed 7 check(s) with 0 warning(s).`
   - confirm the output reports:
     `P0 playable readiness passed with 0 warning(s).`
   - confirm the output reports:
     `P0 scene setup valid with 0 warning(s).`
   - confirm Unity Console has zero errors and zero warnings after the run
   - use the log-only gate for MCP/CI automation; keep dialog-based menu items
     for manual editor use only
55. Short Play Mode route-flow smoke:
   - open `P0MainMenu`
   - enter Play Mode
   - start a default route from `MainMenuController`
   - confirm `P0RouteMap` loads with `RouteMapController` and route layer 1
   - execute the route-map confirm command on layer 1
   - confirm `P0GrayboxBattle` loads with `GrayboxBattleController` and an
     active `BattleSimulation`
   - resolve or clear the first battle and confirm route progress reaches layer
     2 with one completed node
   - continue route and confirm `P0RouteMap` reloads
   - confirm the layer 2 default dream-event reward advances to layer 3 and
     updates reward state
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
56. Full assisted Play Mode route-flow smoke:
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Route Flow Smoke`
   - poll `TheCat/P0/Log Play Mode Route Flow Smoke` or
     `P0PlayModeRouteFlowSmoke.LastSummary`
   - confirm the final state is `Passed`
   - confirm the summary reports `nodes 10/10`, `battles 5`, and
     `boss observed`
   - confirm the detailed log includes layer 1 defense, layer 3 elite, layer 6
     defense, layer 9 elite, and layer 10 Boss victories
   - confirm non-battle default rewards resolve for dream event, partner, shop,
     blessing, and rest nest nodes
   - confirm the route returns to `P0RouteMap` after each battle and clears the
     final Boss route node
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
   - treat any unassisted defeat as a tuning signal, but require the assisted
     smoke to pass before P0 demo acceptance
57. P0 settlement presenter smoke:
   - run EditMode tests and confirm `P0SettlementPresenterTests` passes in the
     Unity Test Runner
   - execute `TheCat/P0/Start Play Mode Route Flow Smoke`
   - confirm the detailed smoke log includes
     `Settlement rows verified: Run Cleared route 10/10 battles 5W/0L`
   - confirm the route map settlement area shows:
     `Settlement: Run Cleared`
   - confirm the route map settlement area shows:
     `Route: 10/10 nodes`
   - confirm the route map settlement area shows:
     `Battles: 5W / 0L`
   - confirm the route map settlement area includes final run state, final core
     values, and final cat HP rows
   - capture a settlement screenshot before treating P0 presentation as demo
     ready
58. P0 Play Mode screenshot smoke:
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Screenshot Smoke`
   - poll `TheCat/P0/Log Play Mode Screenshot Smoke` or
     `P0PlayModeScreenshotSmoke.LastSummary`
   - confirm the final state is `Passed`
   - confirm the summary reports five screenshots in
     `design/development/screenshots/p0-playmode-smoke`
   - confirm the directory contains non-empty PNGs for main menu, route map
     layer 1, battle HUD layer 1, battle result layer 1, and settlement
   - visually inspect `01-main-menu.png` and confirm starter skill text is not
     clipped
   - visually inspect `03-battle-hud-layer1.png` and confirm core combat HUD
     values are visible
   - visually inspect `04-battle-result-layer1.png` and confirm the first
     battle result exposes outcome, metrics, core values, route reward, next
     node, and result actions
   - visually inspect `05-settlement.png` and confirm the cleared 10/10 route
     settlement is visible
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
59. P0 battle HUD section layout smoke:
   - run EditMode tests and confirm `P0BattleHudSummaryPresenterTests` passes
     in the Unity Test Runner
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Screenshot Smoke`
   - confirm the detailed smoke log includes
     `Battle HUD sections verified`
   - confirm the detailed smoke log lists Objective, Core Values, Threats,
     Team, Run, and Node Metrics sections
   - visually inspect `03-battle-hud-layer1.png`
   - confirm the runtime speed buttons are fully visible
   - confirm there is no horizontal scrollbar in the battle HUD panel
   - confirm the left HUD panel keeps the bed, active cat marker, litter box,
     feeder, and enemy threat readable in the remaining Game View
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
60. P0 battle action affordance smoke:
   - run EditMode tests and confirm `P0BattleActionAffordancePresenterTests`
     passes in the Unity Test Runner
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Screenshot Smoke`
   - confirm the detailed smoke log includes `Battle HUD actions verified`
   - confirm the action summary reports at least three skills, three
     interactions, and at least one enabled action at battle start
   - visually inspect `03-battle-hud-layer1.png`
   - confirm skill buttons show ready/cooldown/target/hunger state in readable
     wrapped text
   - confirm Bed Care, Litter Box, and Feeder buttons show shortcut keys and
     ready/move-closer/inactive state
   - confirm low hunger does not hard-disable skills, but is visible as a
     warning state
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
61. P0 battle action telemetry smoke:
   - run EditMode tests and confirm `RunMetricsTests`,
     `BattleSimulationTests`, `P0BattleHudSummaryPresenterTests`,
     `P0SettlementPresenterTests`, and `P0GrayboxTelemetryTests` pass
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Screenshot Smoke`
   - confirm the full route still reaches settlement
   - confirm the settlement rows include `Actions:`
   - confirm the action row reports skill successes/attempts, cooldown blocks,
     target blocks, interaction successes/attempts, and range blocks
   - confirm `P0GrayboxTelemetry.BuildDetailedSummary()` includes skill and
     interaction telemetry for at least one battle node after a manual or
     assisted Play Mode route
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
62. P0 golden path assisted action telemetry smoke:
   - run EditMode tests and confirm `P0GoldenPathSimulatorTests` and
     `P0GrayboxTelemetryTests` pass
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Screenshot Smoke`
   - confirm the full route still reaches settlement
   - confirm the final settlement action row reports non-zero switch
     successes/attempts, non-zero skill successes/attempts, and non-zero
     interaction successes/attempts
   - confirm `P0PlayModeRouteFlowSmoke.LastDetailedLog` includes
     `Settlement action telemetry verified`
   - confirm `P0PlayModeRouteFlowSmoke.LastSummary` includes compact
     non-zero action telemetry totals
   - confirm `P0GoldenPathSimulator.SimulateDefaultRun()` reports action
     telemetry comparable to the offline harness result
   - visually inspect `05-settlement.png` and confirm the action row is
     readable without clipping
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
63. P0 golden path acceptance action gate smoke:
   - run EditMode tests and confirm `P0GoldenPathSimulatorTests` and
     `P0PlayableReadinessTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm the code smoke suite still reports playable readiness as passed
   - confirm golden-path acceptance detailed output includes
     `Action telemetry: switches 10/10, targets auto 5/5 skill 5/5, skills 15/15, interactions 15/15`
   - confirm a local diagnostic run with assisted opening actions disabled is
     rejected by `P0GoldenPathAcceptance`
   - confirm Unity Console has zero errors and zero warnings after the run
64. P0 cat switch telemetry smoke:
   - run EditMode tests and confirm `RunMetricsTests`,
     `P0BattleHudSummaryPresenterTests`, `P0GrayboxTelemetryTests`,
     `P0GoldenPathSimulatorTests`, and `P0SettlementPresenterTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm golden-path acceptance detailed output includes
     `Action telemetry: switches 10/10, targets auto 5/5 skill 5/5, skills 15/15, interactions 15/15`
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Screenshot Smoke`
   - confirm the battle HUD Node Metrics section includes `Switches:`
   - confirm `P0PlayModeRouteFlowSmoke.LastSummary` includes non-zero switch,
     auto target, skill target, skill, and interaction totals
   - confirm final settlement `Actions:` row includes switch totals and weak
     switch blocks
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
65. P0 target acquisition telemetry smoke:
   - run EditMode tests and confirm `RunMetricsTests`,
     `P0BattleHudSummaryPresenterTests`, `P0GrayboxTelemetryTests`,
     `P0GoldenPathSimulatorTests`, and `P0SettlementPresenterTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm golden-path acceptance detailed output includes
     `Action telemetry: switches 10/10, targets auto 5/5 skill 5/5, skills 15/15, interactions 15/15`
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Screenshot Smoke`
   - confirm the battle HUD Node Metrics section includes `Targets:`
   - confirm `P0PlayModeRouteFlowSmoke.LastSummary` includes non-zero auto
     target and skill target totals
   - confirm final settlement `Actions:` row includes target acquisition totals
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
66. P0 enemy sleep pressure telemetry smoke:
   - run EditMode tests and confirm `BattleSimulationTests`,
     `RunMetricsTests`, `P0BattleHudSummaryPresenterTests`,
     `P0GrayboxTelemetryTests`, `P0GoldenPathSimulatorTests`,
     `RouteStateTests`, and `P0SettlementPresenterTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm golden-path acceptance detailed output includes
     `Enemy pressure: events 1, bed 0, boss throws 1, sleep 0/4, absorbed 4`
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Screenshot Smoke`
   - confirm the battle HUD Node Metrics section includes `Enemy Pressure:`
   - confirm final settlement rows include `Enemy Pressure:`
   - confirm final settlement pressure row shows non-zero pressure events and
     incoming sleep pressure
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
67. P0 cat vital telemetry smoke:
   - run EditMode tests and confirm `BattleSimulationTests`,
     `P0CatPressureApplierTests`, `RunMetricsTests`,
     `P0BattleHudSummaryPresenterTests`, `P0GrayboxTelemetryTests`,
     `P0GoldenPathSimulatorTests`, and `P0SettlementPresenterTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm golden-path acceptance detailed output includes
     `Cat vitality: pressure 0, damage 0/0, absorbed 0, heals 5/100, shields 5/192.5`
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Screenshot Smoke`
   - create or wait for a battle moment where an enemy pressures the active cat
     while the active cat has shield
   - confirm the battle HUD Node Metrics section includes `Cat Vitals:`
   - confirm `Cat Vitals:` shows non-zero cat pressure and non-zero absorbed
     damage after shielded pressure
   - confirm final settlement rows include `Cat Vitals:`
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
68. P0 runtime settings coverage smoke:
   - run EditMode tests and confirm `GameStateMachineTests`,
     `P0KeyboardInputMapTests`, `P0RuntimeSettingsCoverageTests`, and
     `P0CodeSmokeSuiteTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm the suite reports
     `P0 code smoke suite passed 8 check(s) with 0 warning(s).`
   - confirm the detailed suite output includes
     `Runtime Settings Coverage: Passed`
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Screenshot Smoke`
   - confirm the battle HUD shows `Runtime Settings:` with `pause P/Esc` and
     `speeds F1/F2/F3`
   - toggle Pause / Resume in Play Mode and confirm the battle stops advancing
     while paused
   - switch F1 / F2 / F3 speed presets and confirm the HUD speed label updates
     to `0.5x`, `1x`, and `1.5x`
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
69. P0 battle feedback coverage smoke:
   - run EditMode tests and confirm `P0BattleFeedbackCoverageTests`,
     `P0CatPressureApplierTests`, `P0RuntimeSettingsCoverageTests`, and
     `P0CodeSmokeSuiteTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm the suite reports
     `P0 code smoke suite passed 9 check(s) with 0 warning(s).`
   - confirm the detailed suite output includes
     `Battle Feedback Coverage: Passed`
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Screenshot Smoke`
   - cast a skill and confirm the battle HUD shows `Feedback: Positive SkillCast`
   - try a cooldown or no-target skill and confirm the HUD shows
     `Feedback: Warning SkillBlocked`
   - use or range-block Bed / Litter / Feeder and confirm interaction feedback
     appears
   - pressure a shielded active cat and confirm the HUD shows
     `Feedback: Warning CatPressure` with absorbed damage even when HP loss is
     fully blocked
   - pressure a cat into weak state and confirm `Feedback: Critical CatWeak`
   - clear or fail a battle and confirm result feedback appears
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
70. P0 battle feedback visual smoke:
   - run EditMode tests and confirm `P0BattleFeedbackVisualCoverageTests`,
     `P0BattleFeedbackCoverageTests`, and `P0CodeSmokeSuiteTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm the suite reports
     `P0 code smoke suite passed 10 check(s) with 0 warning(s).`
   - confirm the detailed suite output includes
     `Battle Feedback Visual Coverage: Passed`
   - enter Play Mode
   - trigger a successful skill cast and confirm the battle HUD shows a green
     feedback card with a visible pulse bar
   - trigger a blocked skill or blocked interaction and confirm the card uses
     amber warning styling
   - pressure a cat into weak state and confirm the card uses red critical
     styling
   - clear or fail a battle and confirm result feedback remains visible after
     the pulse bar drains
   - confirm the feedback card does not overlap action controls, HUD sections,
     or the battle view at the default screenshot-smoke Game View size
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
71. P0 cat HUD card smoke:
   - run EditMode tests and confirm `P0CatHudCoverageTests`,
     `P0CodeSmokeSuiteTests`, and `P0BattleActionAffordancePresenterTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm the suite reports
     `P0 code smoke suite passed 11 check(s) with 0 warning(s).`
   - confirm the detailed suite output includes `Cat HUD Coverage: Passed`
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Screenshot Smoke`
   - confirm the detailed smoke log includes `Battle HUD cat cards verified`
   - confirm the battle HUD shows three cat cards with active / reserve state,
     role token, HP bar, skill count, and ready/cooldown text
   - apply Cat Shield through Smoke Tools and confirm the active cat card shows
     shield text and keeps the HP bar readable
   - pressure a cat into weak state and confirm the card changes to `Weak`,
     cannot be switched to, and shows weak remaining time
   - confirm cat cards do not overlap skill buttons, interaction buttons, or
     battle HUD sections at the default screenshot-smoke Game View size
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
72. P0 skill HUD card smoke:
   - run EditMode tests and confirm `P0SkillHudCoverageTests`,
     `P0CodeSmokeSuiteTests`, and `P0BattleActionAffordancePresenterTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm the suite reports
     `P0 code smoke suite passed 12 check(s) with 0 warning(s).`
   - confirm the detailed suite output includes `Skill HUD Coverage: Passed`
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Screenshot Smoke`
   - confirm the detailed smoke log includes `Battle HUD skill cards verified`
   - confirm the battle HUD skill cards show S1 / S2 / ULT slot tokens,
     ready/cooldown/no-target/low-hunger states, target text, and hunger
     before-after text
   - cast a skill and confirm its card shows a cooldown state and draining bar
   - move out of range for an enemy-targeted skill and confirm its card shows
     missing-target text without spending hunger or cooldown
   - lower team hunger and confirm low-hunger state remains visible and still
     castable under the P0 light-penalty model
   - confirm skill cards do not overlap cat cards, interaction buttons, or
     battle HUD sections at the default screenshot-smoke Game View size
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
73. P0 status HUD response smoke:
   - run EditMode tests and confirm `P0StatusHudCoverageTests`,
     `P0StatusIndicatorPresenterTests`, and `P0CodeSmokeSuiteTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm the suite reports
     `P0 code smoke suite passed 13 check(s) with 0 warning(s).`
   - confirm the detailed suite output includes `Status HUD Coverage: Passed`
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Screenshot Smoke`
   - confirm the detailed smoke log includes
     `Battle HUD status indicators verified`
   - confirm the battle HUD shows a `Status HUD` section with bed, enemy, and
     cat status rows after smoke priming
   - confirm the status rows include Sleep Stable, Slow, Knockback, Mark, and
     Shield visual tokens
   - confirm enemy status rows show movement, time-to-bed, and damage-taken
     response text for Slow, Knockback, and Mark
   - confirm bed shield and cat shield rows remain visually distinct from HP
     bars and owner sleep text
   - confirm status rows do not overlap skill cards, cat cards, interaction
     buttons, or battle HUD sections at the default screenshot-smoke Game View
     size
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
74. P0 enemy HUD threat card smoke:
   - run EditMode tests and confirm `P0EnemyHudCoverageTests`,
     `P0EnemyWarningIndicatorPresenterTests`, `P0EnemyPressureResolverTests`,
     and `P0CodeSmokeSuiteTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm the suite reports
     `P0 code smoke suite passed 14 check(s) with 0 warning(s).`
   - confirm the detailed suite output includes `Enemy HUD Coverage: Passed`
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Screenshot Smoke`
   - confirm the detailed smoke log includes `Battle HUD enemy cards verified`
   - confirm the battle HUD shows an `Enemy HUD` section with Black Mud
     Nightmare, Cold Light Shadow, and Call Tyrant rows after smoke priming
   - confirm Black Mud is marked as critical bed pressure with a bed-contact
     warning and counter hint
   - confirm Cold Light is marked as ranged cat pressure and as the active
     pressure source
   - confirm Call Tyrant is marked as a Boss pattern and shows Boss summon plus
     Boss throw warnings
   - confirm enemy cards do not overlap status rows, skill cards, cat cards,
     interaction buttons, or battle HUD sections at the default screenshot-smoke
     Game View size
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
75. P0 main menu start surface smoke:
   - run EditMode tests and confirm `P0MainMenuCoverageTests`,
     `GameStateMachineTests`, and `P0CodeSmokeSuiteTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm the suite reports
     `P0 code smoke suite passed 15 check(s) with 0 warning(s).`
   - confirm the detailed suite output includes `Main Menu Coverage: Passed`
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Screenshot Smoke`
   - confirm the detailed smoke log includes
     `Main menu start surface verified`
   - confirm the main menu shows Saiban, Nephthys, and Suzune with role,
     authority, attribute, and skill preview labels
   - uncheck all starters and confirm selected-route and quick-battle actions
     are disabled while default-route start remains available
   - restore the default trio and confirm selected-route start enters
     `P0RouteMap`
   - confirm quick battle enters `P0GrayboxBattle`
   - confirm the route preview shows ten layers, branch options, non-battle
     nodes, and the Call Tyrant Boss layer without clipping at the default
     screenshot-smoke Game View size
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
76. P0 pause and speed runtime surface smoke:
   - run EditMode tests and confirm `P0RuntimeSettingsCoverageTests`,
     `P0KeyboardInputMapTests`, and `P0CodeSmokeSuiteTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm the suite reports
     `P0 code smoke suite passed 15 check(s) with 0 warning(s).`
   - confirm the detailed suite output includes
     `Runtime Settings Coverage: Passed`
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Screenshot Smoke`
   - confirm the detailed smoke log includes
     `Battle HUD runtime settings verified`
   - confirm the battle HUD runtime settings controls show Pause / Resume,
     `P/Esc`, and speed shortcuts `F1/F2/F3`
   - confirm the current speed button is disabled at `1x`
   - switch to `0.5x` and `1.5x` and confirm the disabled current-speed button
     moves with the active speed
   - toggle pause and confirm battle time / enemy movement stop advancing while
     the button label changes to `Resume`
   - confirm runtime controls do not overlap feedback cards, status rows, enemy
     cards, cat cards, skill cards, interaction buttons, or battle HUD sections
     at the default screenshot-smoke Game View size
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
77. P0 route map surface smoke:
   - run EditMode tests and confirm `P0RouteMapSurfaceCoverageTests`,
     `P0RouteMapCommandRouterTests`, `P0RouteMapInputCoverageTests`, and
     `P0CodeSmokeSuiteTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm the suite reports
     `P0 code smoke suite passed 17 check(s) with 0 warning(s).`
   - confirm the detailed suite output includes
     `Route Map Surface Coverage: Passed`
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Screenshot Smoke`
   - confirm the detailed smoke log includes
     `Route map surface verified`
   - confirm the route map screenshot shows current progress, current node,
     ten route rows, branch options, wallet, core values, cat HP, roster,
     blessings, and pending event rows without clipping
   - select the layer 2 branch option and confirm the selected marker moves
   - open a non-battle node and confirm numbered reward choices are
     player-facing and do not expose raw id tokens
   - complete the route and confirm settlement rows still render after Boss
     clear
   - force a battle failure and confirm route map settlement rows show
     `Settlement: Run Failed`, `Route: 1/10 nodes`, `Battles: 0W / 1L`,
     final core values, final cat HP, action telemetry, and pressure rows
   - confirm route map controls do not overlap summary rows, route rows, or
     reward choices at the default screenshot-smoke Game View size
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
78. P0 battle result surface smoke:
   - run EditMode tests and confirm `P0BattleResultCoverageTests`,
     `P0BattleHudPromptPresenterTests`, `GameStateMachineTests`, and
     `P0CodeSmokeSuiteTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm the suite reports
     `P0 code smoke suite passed 17 check(s) with 0 warning(s).`
   - confirm the detailed suite output includes
     `Battle Result Coverage: Passed`
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Route Flow Smoke` or the full
     `TheCat/P0/Start Play Mode Screenshot Smoke`
   - confirm the detailed route-flow smoke log includes
     `Battle result surface verified`
   - finish a normal battle and confirm the result panel shows the outcome,
     duration, sleep delta, care usage, pressure rows, final core values,
     route result, reward, next node, `Continue Route [Enter]`, and
     `Restart Run [R]`
   - force a defeat and confirm the result panel keeps
     `Continue Route [Enter]` enabled so the failed-route settlement can be
     viewed
   - confirm `Continue Route [Enter]` is not available before the battle has a
     victory or defeat outcome
   - confirm result rows and result actions do not overlap interaction buttons,
     battle HUD sections, feedback cards, enemy cards, status rows, cat cards,
     or skill cards at the default screenshot-smoke Game View size
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
79. P0 failed route settlement surface smoke:
   - run EditMode tests and confirm `P0SettlementPresenterTests`,
     `P0RouteMapSurfaceCoverageTests`, and `P0CodeSmokeSuiteTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm the suite reports
     `P0 code smoke suite passed 17 check(s) with 0 warning(s).`
   - confirm the detailed route map surface coverage output reports
     `P0 route map surface coverage complete for 7 surface check(s).`
   - enter Play Mode and force the first battle to defeat by collapsing owner
     sleep
   - use `Continue Route [Enter]` from the battle result panel
   - confirm the route map status is failed and progress is `1/10`
   - confirm the settlement area shows `Settlement: Run Failed`,
     `Route: 1/10 nodes`, `Battles: 0W / 1L`, `Enemy Pressure:`,
     `Cat Vitals:`, `Actions:`, `Final Core:`, and `Final Cat HP:`
   - confirm `New Run` remains enabled from the failed settlement
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode
80. P0 Play Mode defeat flow smoke:
   - run EditMode tests and confirm `P0PlayModeDefeatFlowSmokeTests`,
     `P0BattleResultCoverageTests`, `P0SettlementPresenterTests`, and
     `P0RouteMapSurfaceCoverageTests` pass
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Defeat Flow Smoke`
   - poll `TheCat/P0/Log Play Mode Defeat Flow Smoke`
   - confirm the final state is `Passed`
   - confirm the detailed log includes `Defeat battle result surface verified`
   - confirm the detailed log includes `Failed settlement rows verified`
   - confirm the summary reports `P0 play mode defeat flow smoke passed`
   - confirm route progress is `1/10`, battle summary is `0W / 1L`, and route
     status is failed
   - confirm `Continue Route [Enter]` moves from battle result to the failed
     route-map settlement
   - confirm `New Run` remains enabled from the failed settlement
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode

## Unity MCP Note

The latest Unity MCP calls in this thread returned `unsupported call` before
reaching the editor. The standing editor-side validation gap remains: open Unity
Editor > Project Settings > AI > Unity MCP and re-approve the current Codex MCP
connection before running additional Play Mode screenshot or Console validation.

The route map command script import issue was resolved by merging the small
route map command types into `RouteMapController.cs`. Current offline
validation reports `P0 code smoke suite passed 17 check(s) with 0 warning(s)`.
`Route Map Surface Coverage` now covers 7 surface checks including failed route
settlement rows, and `Battle Result Coverage` remains part of the editor-side
suite that should be rerun after Unity MCP routing is restored. A dedicated
`TheCat/P0/Start Play Mode Defeat Flow Smoke` menu item now covers the forced
defeat path once editor-side validation is available.

81. P0 Play Mode evidence acceptance gate:
   - run EditMode tests and confirm `P0PlayModeEvidenceChecklistTests`,
     `P0PlayModeScreenshotSmokeTests`, `P0PlayModeDefeatFlowSmokeTests`, and
     `P0CodeSmokeSuiteTests` pass
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Screenshot Smoke`
   - poll `TheCat/P0/Log Play Mode Screenshot Smoke`
   - confirm the final state is `Passed`
   - confirm five screenshots are captured:
     `01-main-menu.png`, `02-route-map-layer1.png`,
     `03-battle-hud-layer1.png`, `04-battle-result-layer1.png`, and
     `05-settlement.png`
   - execute `TheCat/P0/Start Play Mode Route Flow Smoke`
   - poll `TheCat/P0/Log Play Mode Route Flow Smoke`
   - confirm the final state is `Passed`
   - execute `TheCat/P0/Start Play Mode Defeat Flow Smoke`
   - poll `TheCat/P0/Log Play Mode Defeat Flow Smoke`
   - confirm the final state is `Passed`
   - execute `TheCat/P0/Run Acceptance Gates (Log Only)`
   - confirm `P0 Play Mode Evidence` reports 0 failure(s)
   - confirm any remaining Play Mode evidence warnings correspond only to
     smoke runs intentionally left pending
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode

Updated offline validation: `P0PlayModeEvidenceChecklist` now feeds the editor
acceptance gate. Pending Play Mode smoke states are warnings, while failed
smoke states are blocking failures. Offline validation reports `P0 code smoke
suite passed 17 check(s) with 0 warning(s)` and EditMode `[Test]` count is now
290. Editor-side gate execution remains pending until Unity MCP routing is
restored.

82. P0 Play Mode acceptance smoke one-button run:
   - run EditMode tests and confirm `P0PlayModeAcceptanceSmokeTests`,
     `P0PlayModeEvidenceChecklistTests`, `P0PlayModeScreenshotSmokeTests`, and
     `P0PlayModeDefeatFlowSmokeTests` pass
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Acceptance Smoke`
   - poll `TheCat/P0/Log Play Mode Acceptance Smoke`
   - confirm the final state is `Passed`
   - confirm the detailed log includes `Screenshot smoke passed`
   - confirm the detailed log includes `Route-flow smoke passed`
   - confirm the detailed log includes `Defeat-flow smoke passed`
   - confirm the detailed log includes `Evidence gate summary`
   - confirm the evidence summary reports 4 passed evidence checks and 0
     blocking failures
   - confirm the five screenshot files exist and are non-empty
   - confirm the forced defeat route returns to failed settlement with
     `进度：1/10`
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode

Updated offline validation: `P0PlayModeAcceptanceSmoke` now provides a single
menu entry for the current Play Mode evidence chain. Offline validation reports
`P0 code smoke suite passed 17 check(s) with 0 warning(s)` and EditMode `[Test]`
count is now 291. Editor-side execution remains pending until Unity MCP routing
is restored.

83. P0 starter cat design coverage smoke:
   - run EditMode tests and confirm `P0CharacterDesignCoverageTests`,
     `PrototypeCatalogTests`, `P0MainMenuCoverageTests`, and
     `P0CodeSmokeSuiteTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm the suite reports
     `P0 code smoke suite passed 18 check(s) with 0 warning(s).`
   - confirm detailed output includes `Character Design Coverage: Passed`
   - enter Play Mode
   - execute `TheCat/P0/Start Play Mode Screenshot Smoke`
   - confirm the main-menu starter cards show design preview lines for Saiban,
     Nephthys, and Suzune when selected
   - confirm the design preview lines include stable visual tokens:
     `silver_oath_sun_sword`, `moon_sand_obelisk_crown`, and
     `moon_bell_torii`
   - confirm the starter card text does not clip at the default screenshot
     smoke Game View size
   - confirm Unity Console has zero errors and zero warnings after exiting Play
     Mode

Updated offline validation: `P0CharacterDesignCoverage` is now part of the
code smoke suite. Offline validation reports `P0 character design coverage
complete for 5 design check(s)`, `P0 code smoke suite passed 18 check(s) with
0 warning(s)`, and EditMode `[Test]` count is now 293. Editor-side screenshot
validation remains pending until Unity MCP routing is restored.

84. P0 asset manifest coverage smoke:
   - run EditMode tests and confirm `P0AssetManifestCoverageTests`,
     `P0CharacterDesignCoverageTests`, `P0CodeSmokeSuiteTests`, and
     `PrototypeCatalogTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm the suite reports
     `P0 code smoke suite passed 19 check(s) with 0 warning(s).`
   - confirm detailed output includes `Asset Manifest Coverage: Passed`
   - confirm `design/development/P0_ASSET_MANIFEST.csv` has 19 planned P0 rows
   - confirm planned rows cover style anchors, starter cats, Black Mud, Cold
     Light, Call Tyrant, bed, litter box, feeder, four core HUD icons, Boss
     warning VFX, and Boss route node icon
   - after generation/import, confirm every accepted row has a corresponding
     non-empty file under `Assets/TheCat/Art`
   - confirm Unity import settings match the intended usage: sprites for
     gameplay objects/icons/VFX and texture/background settings for concepts
     and scene backgrounds
   - confirm Unity Console has zero errors and zero warnings after import

Updated offline validation: `P0AssetManifestCoverage` is now part of the code
smoke suite. Offline validation reports `P0 asset manifest coverage complete
for 7 asset check(s)`, `P0 code smoke suite passed 19 check(s) with 0
warning(s)`, and EditMode `[Test]` count is now 296. Actual generated/imported
image validation remains pending.

85. P0 asset generation batch and import readiness smoke:
   - run EditMode tests and confirm `P0AssetGenerationBatchCoverageTests`,
     `P0AssetImportReadinessTests`, `P0AssetManifestCoverageTests`, and
     `P0CodeSmokeSuiteTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm the suite reports
     `P0 code smoke suite passed 21 check(s) with 0 warning(s).`
   - confirm detailed output includes
     `Asset Generation Batch Coverage: Passed`
   - confirm detailed output includes `Asset Import Readiness: Passed`
   - confirm import readiness reports
     `1 workspace file(s), 18 planned`
   - confirm
     `Assets/TheCat/Art/_GeneratedReferences/thecat_style_bedroomdream_anchor_1920x1080_v001.png`
     exists and is non-empty
   - confirm the texture dimensions are `1920x1080`
   - confirm the manifest row for
     `thecat_style_bedroomdream_anchor_1920x1080_v001` is `generated`
   - refresh Unity project assets and confirm the imported texture has the
     expected background/concept settings
   - inspect the generated bedroom anchor in the Project preview
   - confirm Unity Console has zero errors and zero warnings after import

Updated offline validation: asset generation batches and import readiness are
now part of the code smoke suite. Offline validation reports `P0 code smoke
suite passed 21 check(s) with 0 warning(s)` and import readiness reports
`1 workspace file(s), 18 planned`. Unity editor import validation remains
pending until Unity MCP routing is restored.

86. P0 Batch 01 style anchor import smoke:
   - run EditMode tests and confirm `P0AssetImportReadinessTests`,
     `P0AssetGenerationBatchCoverageTests`, and `P0CodeSmokeSuiteTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm the suite reports
     `P0 code smoke suite passed 21 check(s) with 0 warning(s).`
   - confirm detailed output includes
     `P0 asset import readiness passed for 19 asset(s): 4 workspace file(s), 15 planned.`
   - confirm manifest status counts:
     - `generated`: 4
     - `planned`: 15
   - confirm these files exist and are non-empty:
     - `Assets/TheCat/Art/_GeneratedReferences/thecat_style_bedroomdream_anchor_1920x1080_v001.png`
     - `Assets/TheCat/Art/_GeneratedReferences/thecat_style_startercats_lineup_2048_v001.png`
     - `Assets/TheCat/Art/_GeneratedReferences/thecat_style_blackmud_concept_2048_v001.png`
     - `Assets/TheCat/Art/_GeneratedReferences/thecat_style_status_icons_5x64_v001.png`
   - confirm dimensions:
     - bedroom anchor: `1920x1080`
     - starter cats lineup: `2048x2048`
     - black mud concept: `2048x2048`
     - status icon sheet: `320x64`
   - refresh Unity project assets and confirm `.meta` files/import settings are
     created for the four PNGs
   - inspect Project preview for each anchor
   - confirm the status icon sheet preserves transparency
   - confirm Unity Console has zero errors and zero warnings after import

Updated offline validation: Batch 01 style anchors are now generated in the
workspace. Offline validation reports `P0 code smoke suite passed 21 check(s)
with 0 warning(s)` and import readiness reports `4 workspace file(s), 15
planned`. Unity editor import validation remains pending until Unity MCP routing
is restored.

87. P0 starter cat colored-turnaround consistency smoke:
   - run EditMode tests and confirm `P0AssetManifestCoverageTests`,
     `P0AssetImportReadinessTests`, and `P0CodeSmokeSuiteTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm the suite reports
     `P0 code smoke suite passed 21 check(s) with 0 warning(s).`
   - confirm detailed output includes
     `Asset Manifest Coverage: Passed`
   - confirm detailed output includes
     `Asset Import Readiness: Passed`
   - confirm import readiness reports
     `7 workspace file(s), 12 planned`
   - confirm import readiness detailed output includes:
     - `PNG dimensions checked: 7`
     - `PNG dimensions matched: 7`
     - `PNG dimension mismatches: 0`
   - confirm manifest status counts:
     - `generated`: 7
     - `planned`: 12
   - confirm these corrected sprites exist and are non-empty:
     - `Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png`
     - `Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png`
     - `Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png`
   - confirm each corrected sprite is `512x512`
   - compare each corrected sprite against its source colored turnaround:
     - `saiban_turnaround_colored_2026-06-03.png`
     - `nephthys_turnaround_colored_2026-06-03.png`
     - `suzune_turnaround_colored_2026-06-03.png`
   - confirm each sprite preserves the documented front-view silhouette,
     markings, outfit colors, props, and visual symbols
   - confirm no adjacent turnaround-view fragments remain in the cropped
     Nephthys or Suzune sprites
   - confirm the rejected divergent sprites remain isolated outside Unity
     import scope under
     `design/development/rejected_assets/2026-06-13_turnaround_mismatch/`
   - refresh Unity project assets and confirm `.meta` files/import settings are
     created for the three PNGs
   - inspect Project preview for each corrected sprite
   - confirm Unity Console has zero errors and zero warnings after import

Updated offline validation: starter cat sprites are now corrected from the
colored turnaround front views. Offline validation reports `P0 code smoke suite
passed 21 check(s) with 0 warning(s)` and import readiness reports `7 workspace
file(s), 12 planned`. Unity editor import validation remains pending until
Unity MCP routing is restored.

88. P0 generated PNG dimension gate smoke:
   - run EditMode tests and confirm `P0AssetImportReadinessTests` pass
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm detailed output includes
     `Generated, imported, and replaced PNG files match their manifest dimensions.`
   - confirm generated workspace files are dimension-checked from PNG headers,
     not only accepted by path existence
   - confirm the status icon sheet row `5 icons 64x64` validates as a 320x64
     PNG
   - confirm a deliberate temporary copy with mismatched dimensions fails the
     import-readiness gate before being removed
   - refresh Unity project assets and confirm Unity-imported texture dimensions
     match the PNG header dimensions for all seven generated rows
   - confirm Unity Console has zero errors and zero warnings after import

Updated offline validation: `P0AssetImportReadiness` now validates generated
PNG dimensions against manifest sizes. Offline validation reports `PNG
dimensions checked: 7`, `PNG dimensions matched: 7`, and `PNG dimension
mismatches: 0`. Unity editor import validation remains pending until Unity MCP
routing is restored.

89. P0 Unity asset import settings validator smoke:
   - refresh Unity project assets
   - execute `TheCat/P0/Validate P0 Asset Imports`
   - confirm the validator logs a `P0 asset import settings` summary
   - confirm all seven generated/imported rows have loadable `Texture2D`
     assets and `TextureImporter` instances
   - confirm imported texture dimensions match manifest dimensions
   - confirm generated starter cat sprites import as Sprite, Single mode,
     mipmaps disabled, and alpha transparency enabled
   - confirm generated status icon sheet imports as Sprite, Multiple mode,
     mipmaps disabled, and alpha transparency enabled
   - confirm generated background/concept anchors import as Default texture
   - fix importer settings until the validator reports zero errors
   - execute `TheCat/P0/Run Acceptance Gates (Log Only)`
   - confirm the new `P0 Asset Imports` section passes inside the acceptance
     gates log
   - confirm Unity Console has zero errors and zero warnings after validation

Updated offline validation: `P0AssetImportSettingsValidator` now provides the
Unity-side import settings gate and is wired into `Run Acceptance Gates (Log
Only)`. Offline editor compilation passes; actual Unity importer validation
remains pending until Unity MCP routing is restored.

90. P0 Unity asset import applier and batchmode recovery:
   - verify Unity can open the project without crashing during initial asset
     import
   - if using batchmode, confirm `UnityShaderCompiler.exe` launches normally
     before running project validation methods
   - execute `TheCat/P0/Apply P0 Asset Import Settings`
   - confirm the applier logs `P0 Asset Import Settings Apply`
   - confirm the applier touches only generated/imported/replaced manifest rows
   - confirm rejected mismatch PNGs remain outside Unity import scope under
     `design/development/rejected_assets/2026-06-13_turnaround_mismatch/`
   - confirm Unity generates full `.png.meta` files with TextureImporter
     sections, not partial `fileFormatVersion/guid` only files
   - execute `TheCat/P0/Validate P0 Asset Imports`
   - confirm the validator reports zero errors
   - execute `TheCat/P0/Run Acceptance Gates (Log Only)`
   - confirm the `P0 Asset Imports` section passes
   - confirm Unity Console has zero errors and zero warnings after the full
     apply + validate flow

Current batchmode finding: an initial batchmode run crashed while importing
`thecat_style_startercats_lineup_2048_v001.png`; a second `-nographics` run
crashed before project validation because `UnityShaderCompiler.exe` could not
launch. The seven accepted PNGs were re-encoded, rejected mismatch PNGs were
moved outside `Assets`, and crash-generated partial `.png.meta` files were
removed. Unity-side importer validation remains pending.

91. P0 source-reference asset gate smoke:
   - run EditMode tests and confirm `P0AssetManifestCoverageTests` passes
   - execute `TheCat/P0/Run Code Smoke Suite`
   - confirm detailed output includes
     `Asset Manifest Coverage: Passed`
   - confirm the asset manifest coverage summary reports
     `8 asset check(s)`
   - confirm the detailed manifest coverage output includes
     `hard source reference notes`
   - confirm Batch 02 prompt requires these hard references:
     - Saiban colored turnaround
     - Nephthys colored turnaround
     - Suzune colored turnaround
     - Black Mud Nightmare concept and animation
     - Cold Light Shadow concept and animation
     - Bedroom Dream map concept and sprite sheets
   - confirm Batch 03 prompt requires Call Tyrant concept and animation as hard
     references
   - confirm generated style anchors are treated as secondary mood references
     and cannot override source silhouettes, palettes, or prop identity
   - confirm no updated prompt or agent prompt contains mojibake design paths
   - confirm no asset is marked `generated` unless it exists at the manifest
     path and passes source-reference review
   - if a candidate diverges from the source concept/animation/turnaround,
     move it outside `Assets` to `design/development/rejected_assets/`

Updated offline validation: the source-reference gate compiles and the offline
smoke runner reports `P0 code smoke suite passed 21 check(s) with 0 warning(s)`.
The asset manifest coverage summary now reports `8 asset check(s)`. Unity has
created seven full `.png.meta` files with `TextureImporter` blocks, but they are
still default imports (`textureType: 0`, `spriteMode: 0`, `enableMipMap: 1` on
sprite/icon rows). Run the import settings applier and validator in Unity before
accepting Unity-side asset import validation.

92. P0 offline asset meta import settings gate:
   - confirm `P0AssetMetaImportSettingsReadinessTests` covers missing meta,
     default sprite settings, icon-sheet Multiple mode, and mixed generated
     manifest rows
   - confirm `P0CodeSmokeSuite` includes the `Asset Meta Import Settings` check
   - confirm all generated/imported/replaced PNG rows require marker
     `TheCatP0ImportSettings:v1`
   - confirm starter cat sprite rows require Sprite, Single mode, mipmaps
     disabled, and alpha transparency enabled
   - confirm status icon sheet rows require Sprite, Multiple mode, mipmaps
     disabled, and alpha transparency enabled
   - confirm background/concept rows require Default texture and mipmaps
     disabled
   - confirm `TheCat/P0/Run Acceptance Gates (Log Only)` still passes after
     Unity refreshes the `.meta` files
   - confirm Unity Console has zero errors and zero warnings after refresh
   - visually compare starter cat sprite previews against the colored
     turnaround sheets before accepting further cat assets

Updated offline validation: the seven generated PNG `.meta` files now carry the
P0 import settings marker and the expected text-level importer values. This
gate is now part of the current 23-check offline smoke suite. Unity editor
validation is still required because the current pass validates `.meta` text,
not live `TextureImporter` objects or Project preview rendering.

93. P0 starter cat turnaround source-lock smoke:
   - confirm `P0StarterCatTurnaroundSourceLocksTests` covers missing source
     turnarounds, changed sprite hashes, duplicate starter locks, manifest
     drift, and missing hard-reference notes
   - execute the offline smoke runner and confirm detailed output includes
     `Starter Cat Turnaround Source Locks: Passed`
   - confirm the source-lock summary reports 3 cat sprites
   - confirm source hashes are locked for:
     - Saiban colored turnaround
     - Nephthys colored turnaround
     - Suzune colored turnaround
   - confirm sprite hashes are locked for:
     - `thecat_cat_saiban_combat_sprite_512_v001.png`
     - `thecat_cat_nephthys_combat_sprite_512_v001.png`
     - `thecat_cat_suzune_combat_sprite_512_v001.png`
   - confirm the manifest rows still use generated status and mention colored
     turnaround hard-reference front-view extraction
   - do not accept visually similar regenerated cat sprites unless the visual
     review is documented and the lock values are intentionally refreshed
   - after Unity MCP is restored, inspect each starter cat Project preview
     against its source colored turnaround and confirm Console has zero errors
     and zero warnings

Updated offline validation: `P0StarterCatTurnaroundSourceLocks` is wired into
`P0CodeSmokeSuite`. The current offline smoke runner now reports `P0 code smoke
suite passed 24 check(s) with 0 warning(s)` after adding the non-cat hard
reference source-lock gate. The starter cat source-lock gate still passes for
3 cat sprites. Unity editor preview validation remains pending.

94. P0 hard reference source-lock smoke:
   - confirm `P0HardReferenceSourceLocksTests` covers missing source files,
     changed source hashes, duplicate lock ids, and missing source groups
   - execute the offline smoke runner and confirm detailed output includes
     `Hard Reference Source Locks: Passed`
   - confirm the source-lock summary reports 12 source files and 12 manifest
     asset links
   - confirm hard references are locked for:
     - Saiban colored turnaround
     - Nephthys colored turnaround
     - Suzune colored turnaround
     - Black Mud Nightmare concept and animation
     - Cold Light Shadow concept and animation
     - Call Tyrant concept and animation
     - Bedroom Dream map concept
     - Bedroom Dream foreground sprites
     - Bedroom Dream mid/background sprites
   - do not generate or accept Batch 02 enemy/prop assets if any of these
     source locks fails
   - after Unity MCP is restored, run `TheCat/P0/Run Acceptance Gates (Log
     Only)` and confirm the hard-reference source-lock check appears in the
     logged smoke output

Updated offline validation: `P0HardReferenceSourceLocks` is wired into
`P0CodeSmokeSuite`. The offline smoke runner reports `P0 code smoke suite passed
24 check(s) with 0 warning(s)` and now reports `Hard Reference Source Locks:
Passed - P0 hard reference source locks ready for 12 source file(s) and 11
manifest asset link(s)`. Unity editor validation remains pending.

95. P0 Batch 02 source-extracted enemy and prop placeholders:
   - confirm Unity imports these new Sprite assets without Console errors:
     - `Assets/TheCat/Art/Enemies/Sprites/thecat_enemy_blackmud_combat_sprite_512_v001.png`
     - `Assets/TheCat/Art/Enemies/Sprites/thecat_enemy_coldlight_combat_sprite_512_v001.png`
     - `Assets/TheCat/Art/Scenes/BedroomDream/thecat_prop_bed_sleepglow_sprite_512_v001.png`
     - `Assets/TheCat/Art/Scenes/BedroomDream/thecat_prop_litterbox_sprite_256_v001.png`
     - `Assets/TheCat/Art/Scenes/BedroomDream/thecat_prop_feeder_sprite_256_v001.png`
   - visually compare Black Mud and Cold Light Project previews against their
     locked source concept images before wiring them into combat prefabs
   - visually confirm the litter box and feeder crops have no magenta key
     residue or neighboring sprite-sheet props
   - accept the protected bed only as an opaque P0 map-concept placeholder, not
     as final transparent prop art
   - run `TheCat/P0/Validate P0 Asset Imports` and confirm all 12
     generated/imported workspace assets have the intended live importer values
   - run `TheCat/P0/Run Acceptance Gates (Log Only)` and confirm:
     - `P0 code smoke suite passed 24 check(s) with 0 warning(s)`
     - `Asset Import Readiness` reports 12 workspace files and 7 planned assets
     - `Asset Meta Import Settings` reports 12 generated/imported assets
     - `Hard Reference Source Locks` still reports 12 source files and 11
       manifest asset links
   - after sprite wiring, capture a Play Mode screenshot with the new enemies
     and props visible in the bedroom battle scene

Updated offline validation: the 5 Batch 02 source-extracted PNG files have
matching dimensions and Sprite `.meta` settings. The offline smoke runner now
reports 12 workspace files, 7 planned assets, and 24 passing checks with zero
warnings. Unity editor preview validation remains pending.

Post-review cleanup: exact low-alpha `#FF00FF` pixels were removed from the
transparent enemy/prop PNGs, and the Cold Light manifest/catalog notes now
clarify that the black mud anchor is only the shared monster-language reference.
Unity preview validation is still required to confirm the visible edges against
dark battle backgrounds.

96. P0 manifest source-lock id link gate:
   - confirm `P0_ASSET_MANIFEST.csv` includes `source_lock_ids` as a separate
     field from `reference_asset_ids`
   - confirm generated/planned source-sensitive rows link to exact source locks:
     - Black Mud sprite -> `black_mud_concept`
     - Cold Light sprite -> `cold_light_concept`
     - Saiban sprite -> `saiban_turnaround_colored`
     - Nephthys sprite -> `nephthys_turnaround_colored`
     - Suzune sprite -> `suzune_turnaround_colored`
     - Call Tyrant concept -> `call_tyrant_concept`
     - bed placeholder -> `bedroom_map_concept`
     - litter box and feeder -> `bedroom_mid_background_sprites`
     - Call Tyrant warning VFX -> `call_tyrant_animation`
     - boss route icon -> `call_tyrant_concept`
   - confirm future reviews treat `reference_asset_ids` as visual/batch anchors
     and `source_lock_ids` as source authority
   - rerun `TheCat/P0/Run Acceptance Gates (Log Only)` in Unity and confirm the
     `Hard Reference Source Locks` line includes 12 manifest asset links

Updated offline validation: the runtime hard-reference gate now resolves
manifest source-lock ids and fails if Cold Light points at the Black Mud source
lock. Offline smoke remains green with 24 checks and zero warnings. Unity
editor validation remains pending.

97. P0 Batch 02 four-core HUD icon placeholders:
   - confirm Unity imports these new Sprite assets without Console errors:
     - `Assets/TheCat/Art/UI/Icons/thecat_ui_core_sleep_icon_64_v001.png`
     - `Assets/TheCat/Art/UI/Icons/thecat_ui_core_hp_icon_64_v001.png`
     - `Assets/TheCat/Art/UI/Icons/thecat_ui_core_poop_icon_64_v001.png`
     - `Assets/TheCat/Art/UI/Icons/thecat_ui_core_hunger_icon_64_v001.png`
   - visually compare the Project previews against
     `thecat_style_status_icons_5x64_v001` for outline weight, glow language,
     and small-size readability
   - confirm the icons read correctly over the battle HUD background at 64px and
     at any smaller runtime scale used by the HUD
   - run `TheCat/P0/Validate P0 Asset Imports` and confirm all 16
     generated/imported workspace assets have the intended live importer values
   - run `TheCat/P0/Run Acceptance Gates (Log Only)` and confirm:
     - `P0 code smoke suite passed 24 check(s) with 0 warning(s)`
     - `Asset Import Readiness` reports 16 workspace files and 3 planned assets
     - `Asset Meta Import Settings` reports 16 generated/imported assets
   - after HUD wiring, capture a Play Mode battle HUD screenshot and confirm the
     sleep, HP, poop, and hunger icons do not overlap text or controls

Updated offline validation: the four 64px HUD icon PNGs have matching
dimensions, Sprite `.meta` settings, and zero exact `#FF00FF` alpha pixels after
cleanup. Offline smoke remains green with 16 workspace files, 3 planned assets,
24 checks, and zero warnings. Unity editor validation remains pending.

98. P0 Batch 03 Call Tyrant Boss readiness assets:
   - confirm Unity imports these new assets without Console errors:
     - `Assets/TheCat/Art/Enemies/Concepts/thecat_enemy_calltyrant_concept_2048_v001.png`
     - `Assets/TheCat/Art/Enemies/VFX/thecat_vfx_calltyrant_warning_512_v001.png`
     - `Assets/TheCat/Art/UI/Icons/thecat_ui_route_bossnode_icon_128_v001.png`
   - visually compare the Project previews against the locked Call Tyrant
     concept and animation source images:
     - concept board preserves device-shell silhouette, red call eyes, purple
       tie, black mud body, and thrown app language
     - warning VFX communicates summon and throw pressure with no grey source
       background residue
     - boss route node icon remains readable at 128px and smaller route-map
       scale
   - run `TheCat/P0/Validate P0 Asset Imports` and confirm all 24
     generated/imported workspace assets have intended live importer values
   - run `TheCat/P0/Run Acceptance Gates (Log Only)` and confirm:
     - `P0 code smoke suite passed 24 check(s) with 0 warning(s)`
     - `Asset Import Readiness` reports 25 workspace files and 0 planned assets
     - `Asset Meta Import Settings` reports 25 generated/imported assets
     - `Hard Reference Source Locks` reports the Call Tyrant concept/VFX/icon
       manifest links through `call_tyrant_concept` and `call_tyrant_animation`
   - after runtime wiring, capture route-map and battle screenshots with the
     boss node icon and warning VFX visible

Updated offline validation: the Batch 03 PNG files have matching dimensions,
expected RGB/RGBA channels, and P0 `.meta` markers. Visual Studio MSBuild
compiles Runtime and EditModeTests after the manifest/catalog status update.
Unity editor preview and screenshot validation remain pending.

99. P0 Boss visual asset runtime reference wiring:
   - confirm route-map Boss node presentation exposes
     `thecat_ui_route_bossnode_icon_128_v001` through current-node and option
     card state
   - confirm Call Tyrant Boss summon/throw warnings expose
     `thecat_vfx_calltyrant_warning_512_v001` through warning indicator state
   - after Unity import refresh, add or verify scene/UI binding so the route
     icon and warning VFX are visible, not just referenced by data state
   - capture a route-map screenshot with the final Boss node visible
   - capture a battle screenshot where Call Tyrant warning VFX appears during
     summon or throw warning timing
   - compare both screenshots against locked Call Tyrant concept/animation
     source rules before approving further Boss-related asset batches

Updated offline validation: `P0VisualAssetReference` and
`P0VisualAssetCatalog` compile in Runtime, EditMode catalog tests compile, route
and battle visual coverage gates now require the Boss icon/VFX asset ids, and
the generated asset acceptance helper follows the manifest catalog. Unity editor
rendering validation remains pending.

100. P0 runtime visual binding and starter cat colored-turnaround source locks:
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings` covers exactly the
     20 P0 runtime-bound visual slots:
     - Saiban combat sprite
     - Nephthys combat sprite
     - Suzune combat sprite
     - Black Mud battle-world sprite
     - Cold Light battle-world sprite
     - Call Tyrant battle-world visual
     - bed interactable sprite
     - litter box interactable sprite
     - feeder interactable sprite
     - owner sleep HUD icon
     - cat HP HUD icon
     - team poop HUD icon
     - team hunger HUD icon
     - Sleep Stable status HUD icon
     - Slow status HUD icon
     - Knockback status HUD icon
     - Mark status HUD icon
     - Shield status HUD icon
     - Boss route node icon
     - Call Tyrant warning VFX
   - confirm the three starter cat manifest rows include `source_lock_ids`:
     - `saiban_turnaround_colored`
     - `nephthys_turnaround_colored`
     - `suzune_turnaround_colored`
   - run `TheCat/P0/Run Acceptance Gates (Log Only)` in Unity and confirm:
     - `Starter Cat Turnaround Source Locks` reports 3 locked cat sprites
     - `Hard Reference Source Locks` reports 12 source files and 12 manifest
       asset links
     - `Asset Manifest Coverage` mentions starter cat source-lock ids
   - in Play Mode, capture cat HUD screenshots proving the three starter cat
     sprites and HP icon are visible, readable, and still match the colored
     turnaround previews
   - capture battle-world screenshots proving Black Mud, Cold Light, bed,
     litter box, feeder, and active cat SpriteRenderer bindings are visible
     with graybox MeshRenderer fallbacks hidden only after Sprite resolution
   - capture battle HUD screenshots proving owner sleep, poop, and hunger icon
     bindings are visible and do not overlap text

Updated offline validation: Runtime and EditModeTests compile after adding the
binding catalog and source-lock changes. `git diff --check` passes, source CSV
scan reports 11 linked manifest rows, and EditMode source now has 336 `[Test]`
markers. Unity editor rendering validation remains pending.

101. P0 runtime visual preview gate and architecture go/no-go:
   - run `TheCat/P0/Run Acceptance Gates (Log Only)` in Unity and confirm:
     - `P0 code smoke suite passed 25 check(s) with 0 warning(s)`
     - `Runtime Visual Binding Coverage` reports 34 runtime bindings and 34
       resolved textures
     - `Starter Cat Turnaround Source Locks` still reports Saiban, Nephthys,
       and Suzune locked to their colored turnarounds
   - enter Play Mode and capture:
     - route-map screenshot with the Boss route node icon visible
     - battle screenshot with owner sleep, cat HP, poop, and hunger icons
     - battle screenshot with all three starter cat HUD sprites visible
     - battle screenshot with active cat, Black Mud, Cold Light, bed, litter
       box, and feeder world sprites visible
     - battle screenshot with Call Tyrant warning VFX visible
   - compare starter cat screenshots directly against the colored turnaround
     source images before approving any new cat sprite generation
   - if any icon/sprite is missing, clipped, mirrored incorrectly, or visually
     inconsistent with its source lock, block the next asset batch

Updated offline validation: Runtime and EditModeTests compile after adding the
IMGUI runtime visual preview layer. `git diff --check` passes, the code smoke
suite now includes a 25th `Runtime Visual Binding Coverage` gate, and EditMode
source now has 339 `[Test]` markers. Unity editor screenshot validation remains
pending.

102. P0 asset review packet gate:
   - run `TheCat/P0/Run Acceptance Gates (Log Only)` in Unity and confirm:
     - `P0 code smoke suite passed 26 check(s) with 0 warning(s)`
     - `Asset Review Packet` reports 38 review assets, 12 source-locked
        entries, 3 starter cat entries, 3 starter cat visual checklist entries,
        15 starter cat visual traits, 3 starter cat asset-production spec
        entries, 12 allowed starter cat derivative asset types, and 34
        runtime-bound entries
   - run `TheCat/P0/Write P0 Asset Review Packet`
   - confirm
     `design/development/asset_review/P0_RUNTIME_VISUAL_REVIEW_PACKET.md`
     is refreshed and includes:
     - `saiban_turnaround_colored`
     - `nephthys_turnaround_colored`
      - `suzune_turnaround_colored`
      - `Starter Cat Visual Consistency Checklist`
      - `Starter Cat Asset Production Spec`
      - `04-active-cat-saiban.png`
      - `05-active-cat-nephthys.png`
      - `06-active-cat-suzune.png`
     - all 34 runtime binding ids, including `background.bedroom_dream`, the
       five `status.*` icon bindings, and the Batch 08 UI shell bindings
   - open
     `design/development/asset_review/p0_starter_cat_turnaround_review_2026-06-14.png`
     and compare it against the cat HUD screenshots before approving any new
     cat sprite or cat animation generation
   - block the next cat-related asset batch if the generated or extracted
     sprite drifts from colored-turnaround markings, costume, props, or
     non-human cat body proportions

Updated offline validation: Runtime, Editor, and EditModeTests compile after
adding the asset review packet gate, Unity menu, and starter cat visual
checklist exposure. `git diff --check` passes, EditMode source now has 356
`[Test]` markers, and the starter-cat contact sheet has been generated under
`design/development/asset_review`. Unity editor execution and screenshots
remain pending.

103. P0 batchmode acceptance runner:
   - close any open Unity editor instance for `D:/Unity Workspace/TheCat`
   - run the offline command from
     `design/development/unity_batchmode/P0_BATCHMODE_ACCEPTANCE_RUNBOOK.md`
   - confirm
     `design/development/unity_batchmode/P0_OFFLINE_ACCEPTANCE_REPORT.md`
     is written and reports all offline gates passing:
     - P0 Code Smoke Suite
     - P0 Playable Readiness
     - P0 Scene Setup
     - P0 Asset Imports
     - P0 Asset Review Packet
     - P0 Offline Asset Production Readiness
   - after Play Mode screenshots exist, run the full command and confirm
     `design/development/unity_batchmode/P0_FULL_ACCEPTANCE_REPORT.md`
     includes passing P0 Play Mode Evidence with zero pending warnings
   - confirm full acceptance fails while screenshot, route-flow, or defeat-flow
     smoke checks are still `Idle` or `Running`; these states are usable for
     diagnostics but incomplete for final acceptance

Updated validation attempt: `P0BatchmodeAcceptanceRunner` compiles in the
Editor assembly, Runtime and EditModeTests still compile, and Unity batchmode
reached licensing/project setup. The run was blocked because another Unity
instance already had the same project open; Unity did not execute the method or
write the offline report.

Updated acceptance strictness: `P0PlayModeEvidenceReport` now separates
`IsUsable` from `IsComplete`; full batchmode acceptance uses `IsComplete`, so
pending Play Mode warnings can no longer be reported as a completed P0 full
acceptance result.

104. P0 battle-world SpriteRenderer visual binding:
   - refresh Unity AssetDatabase after `P0WorldVisualAssetView` is imported
   - run `TheCat/P0/Run Acceptance Gates (Log Only)` and confirm:
     - `Runtime Visual Binding Coverage` reports 34 runtime bindings and 34
       resolved textures
     - `Asset Review Packet` reports 34 runtime-bound entries after Batch 08
     - `Status HUD Coverage` reports generated icon asset mappings for Sleep
       Stable, Slow, Knockback, Mark, and Shield
   - enter `P0GrayboxBattle` Play Mode and capture screenshots proving:
     - active cat sprite changes when switching Saiban, Nephthys, and Suzune
     - Black Mud and Cold Light enemy sprites render in the battle world
     - bed, litter box, and feeder sprites render at their interactable anchors
     - Call Tyrant warning VFX renders during summon or throw warnings
     - Status HUD rows draw the five generated status icons inline without
       overlapping status text, enemy HUD rows, or core gauge rows
     - graybox primitive fallbacks remain available when a Sprite cannot resolve
       but are hidden when the accepted Sprite is present
   - compare starter cat screenshots against
     `design/development/asset_review/p0_starter_cat_turnaround_review_2026-06-14.png`
     before approving any further cat sprite or animation generation

Updated offline validation: Runtime and EditModeTests compile after adding the
battle-world SpriteRenderer visual binding path. Unity MCP tools were not
exposed in the current session, so Console, importer, Play Mode, and screenshot
validation remain pending.

105. P0 offline asset production readiness gate:
   - run `TheCat/P0/Run Acceptance Gates (Log Only)` or the offline batchmode
     command from `design/development/unity_batchmode/P0_BATCHMODE_ACCEPTANCE_RUNBOOK.md`
   - confirm the report includes `P0 Offline Asset Production Readiness`
   - confirm that gate reports:
     - 38 review assets
     - 34 runtime bindings
     - 34 resolved runtime textures
     - 12 source-locked entries
     - 3 starter cat locks
     - starter cat contact sheet present
   - if this gate fails, do not start a new asset production batch
   - if this gate passes, it authorizes only controlled offline asset work; live
     Unity screenshot acceptance is still required before marking assets final

Updated offline validation: Runtime, Editor, and EditModeTests compile after
adding `P0AssetProductionReadiness` and wiring it into batchmode acceptance.
The open Unity editor still prevents a safe batchmode rerun in this session.

106. P0 10-screenshot runtime visual evidence plan:
   - confirm `P0PlayModeScreenshotSmoke.HasP0ScreenshotCapturePlan()` covers
     exactly 10 captures:
     - `01-main-menu.png`
     - `02-route-map-layer1.png`
     - `03-battle-hud-layer1.png`
     - `04-active-cat-saiban.png`
     - `05-active-cat-nephthys.png`
     - `06-active-cat-suzune.png`
     - `07-battle-world-visuals.png`
     - `08-call-tyrant-warning-vfx.png`
     - `09-battle-result-layer1.png`
     - `10-settlement.png`
   - confirm `P0PlayModeEvidenceChecklist` includes
     `runtime_visual_screenshot_plan`
   - confirm `P0PlayModeEvidenceReport.IsComplete` is false until screenshot,
     route-flow, and defeat-flow smoke checks all pass with zero warnings
   - run the Play Mode screenshot smoke and verify it blocks if the 34 runtime
     visual bindings do not resolve
   - compare `04-active-cat-saiban.png`, `05-active-cat-nephthys.png`, and
     `06-active-cat-suzune.png` against
     `design/development/asset_review/p0_starter_cat_turnaround_review_2026-06-14.png`
     before allowing any new cat sprite or cat animation production
   - review `07-battle-world-visuals.png` for the Bedroom Dream battle
     background, Black Mud, Cold Light, bed, litter box, feeder, and active cat
     visibility
   - review `08-call-tyrant-warning-vfx.png` for summon/throw warning VFX
     readability and source-lock consistency

Updated offline validation: Runtime, Editor, and EditModeTests compile after
adding the 10-screenshot runtime visual evidence plan. `git diff --check`
passes, and EditMode source now has 352 `[Test]` markers. Unity editor
execution and the 10 screenshots remain pending.

107. P0 starter cat visual consistency checklist:
   - run `P0StarterCatVisualConsistencyChecklist.EvaluateP0Checklist()` through
     `P0 Offline Asset Production Readiness`
   - confirm the report covers:
     - 3 checklist cats
     - at least 15 required colored-turnaround visual traits
     - 3 active-cat Play Mode screenshot matches
     - 3 colored-turnaround source-lock matches
     - 6 existing reference files: 3 source turnarounds plus 3 locked sprites
   - confirm generic trait lists such as "cute dream cat" would fail the gate
   - confirm wrong screenshot filenames fail the gate
   - after Play Mode screenshot capture, compare:
     - `04-active-cat-saiban.png` against Saiban's colored turnaround traits:
       armored non-human cat proportions, tabby face, shield, sword, cape/helm
     - `05-active-cat-nephthys.png` against Nephthys' colored turnaround
       traits: hood, moon-sand Egyptian motifs, pyramid/obelisk prop,
       gold-blue palette, non-human cat body
     - `06-active-cat-suzune.png` against Suzune's colored turnaround traits:
       calico markings, shrine outfit, bells, wand/branch, healer palette
   - block any cat sprite/animation production if one of these checks fails

Updated offline validation: Runtime, Editor, and EditModeTests compile after
adding the starter cat visual consistency checklist and wiring it into
`P0AssetProductionReadiness`. `git diff --check` passes, and EditMode source
now has 355 `[Test]` markers. Unity editor execution remains pending.

108. P0 starter cat asset-production spec gate:
   - run `P0StarterCatAssetProductionSpec.EvaluateP0Spec()` through
     `P0 Offline Asset Production Readiness`
   - confirm the report covers:
     - 3 starter cat production spec entries
     - 3 source-lock matches against the visual consistency checklist
     - 12 allowed derivative asset types
     - at least 21 required evidence items
     - at least 18 strict prompt clauses
     - at least 12 rejection rules
   - confirm the allowed derivative types are limited to:
     - `combat_sprite_refinement_512`
     - `hud_avatar_256`
     - `skill_icon_motif_128`
     - `front_animation_keyframe_512`
   - confirm candidates are kept under
     `design/development/asset_candidates/starter_cats/<cat_id>/` and are not
     written directly to `Assets/TheCat/Art/Characters/Sprites`
   - confirm `P0_RUNTIME_VISUAL_REVIEW_PACKET.md` includes the
     `Starter Cat Asset Production Spec` section before giving an asset agent a
     cat-related production task
   - block any starter cat candidate that lacks side-by-side comparison against
     `design/development/asset_review/p0_starter_cat_turnaround_review_2026-06-14.png`
   - block any candidate with colored-turnaround drift, human proportions,
     palette drift, or missing required costume / prop / marking traits

Updated offline validation: Runtime, Editor, and EditModeTests compile after
adding `P0StarterCatAssetProductionSpec` and wiring it into
`P0AssetProductionReadiness` and `P0AssetReviewPacket`. `git diff --check`
passes and EditMode source now has 359 `[Test]` markers. Unity editor
execution remains pending for this item.

109. P0 Play Mode screenshot file evidence gate:
   - run `P0PlayModeScreenshotFileEvidence.EvaluateP0Directory()` before
     accepting runtime visual quality for the current P0 loop
   - confirm the report covers the 10-file `P0PlayModeScreenshotSmoke`
     capture plan and reports:
     - 10/10 existing expected screenshots
     - zero missing expected screenshots
     - zero unexpected PNG files
   - current offline evidence observed on 2026-06-14 is intentionally not
     complete:
     - existing expected screenshots: 3/10
     - missing expected screenshots: 7
     - unexpected PNG files: 1
     - stale file: `04-settlement.png`
   - regenerate `design/development/screenshots/p0-playmode-smoke` through the
     Play Mode screenshot smoke before claiming presentation approval
   - compare `04-active-cat-saiban.png`, `05-active-cat-nephthys.png`, and
     `06-active-cat-suzune.png` against the locked colored-turnaround contact
     sheet before starting cat-related derivative asset production
   - remove or archive stale unexpected PNGs only after the new 10-file capture
     set exists and has been reviewed

Updated offline validation: Runtime, Editor, and EditModeTests compile after
adding `P0PlayModeScreenshotFileEvidence` and wiring it into
`P0AssetReviewPacket`. Editor compile keeps the known Unity-generated
`System.Numerics.Vectors` conflict warning. EditMode source now has 361
`[Test]` markers. Unity editor execution and the 10-screenshot regeneration
remain pending.

110. P0 Batch 05 starter cat candidate evidence gate:
   - run `P0StarterCatDerivativeCandidateEvidence.EvaluateBatch05()` and
     confirm:
     - 20/20 expected evidence files exist
     - 12/12 candidate PNG files exist
     - 3/3 review notes exist
     - 3/3 review sheets exist
     - candidate outputs remain outside `Assets`
   - inspect the three review sheets:
     - `design/development/asset_candidates/starter_cats/saiban/batch_05_source_locked_derivatives_2026-06-14/thecat_cat_saiban_batch05_source_locked_review_sheet.png`
     - `design/development/asset_candidates/starter_cats/nephthys/batch_05_source_locked_derivatives_2026-06-14/thecat_cat_nephthys_batch05_source_locked_review_sheet.png`
     - `design/development/asset_candidates/starter_cats/suzune/batch_05_source_locked_derivatives_2026-06-14/thecat_cat_suzune_batch05_source_locked_review_sheet.png`
   - confirm `P0_RUNTIME_VISUAL_REVIEW_PACKET.md` includes
     `Starter Cat Derivative Candidate Evidence`
   - do not import these candidates into Unity until the active-cat Play Mode
     screenshots exist and match the colored-turnaround source locks
   - after candidate acceptance, update manifest rows, source locks, sprite
     hashes, Unity import metadata, active-cat screenshots, and this backlog

Updated offline validation: Runtime, Editor, and EditModeTests compile after
adding `P0StarterCatDerivativeCandidateEvidence` and wiring it into
`P0AssetReviewPacket`. Editor compile keeps the known Unity-generated
`System.Numerics.Vectors` conflict warning. EditMode source now has 364
`[Test]` markers, and `git diff --check` passes. Unity editor execution and
Play Mode screenshot review remain pending.

111. P0 starter cat formal import readiness gate:
   - run `P0StarterCatFormalImportReadiness.EvaluateCurrentGate()` through
     `P0AssetReviewPacket` and `P0 Offline Asset Production Readiness`
   - confirm the report currently covers:
     - gate valid: yes
     - state: `Blocked`
     - import allowed: no
     - 3/3 starter cat review notes present
     - 3/3 explicit block notes
     - 0/3 explicit approval notes
     - 0/3 active-cat screenshots
   - confirm approved import requires all three active-cat screenshots:
     - `04-active-cat-saiban.png`
     - `05-active-cat-nephthys.png`
     - `06-active-cat-suzune.png`
   - confirm `P0_RUNTIME_VISUAL_REVIEW_PACKET.md` includes
     `Starter Cat Formal Import Readiness`
   - block any attempt to copy Batch 05 cat candidates into
     `Assets/TheCat/Art/Characters/Sprites` while the gate state is `Blocked`
   - after the screenshots exist, update the three per-cat review notes only if
     the screenshots match the colored turnaround contact sheet

Updated offline validation: Runtime and EditModeTests compile after adding
`P0StarterCatFormalImportReadiness` and wiring it into
`P0AssetReviewPacket` plus `P0AssetProductionReadiness`. EditMode source now
has 369 `[Test]` markers. Unity editor execution and active-cat screenshot
review remain pending.

112. P0 Bedroom Dream battle background runtime asset gate:
   - confirm the manifest row exists for
     `thecat_bg_bedroomdream_battle_1920x1080_v001`
   - confirm source lock id is `bedroom_map_concept`
   - confirm Unity import path exists:
     `Assets/TheCat/Art/Scenes/BedroomDream/thecat_bg_bedroomdream_battle_1920x1080_v001.png`
   - confirm `.png.meta` uses Sprite Single, alpha transparency, no mipmaps,
     and `TheCatP0ImportSettings:v1`
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` includes
     `background.bedroom_dream`
   - confirm `P0RuntimeVisualBindingCoverage` reports 34 bindings and includes
     the battle background binding
   - confirm `P0AssetReviewPacket` reports 38 review assets, 34 runtime-bound
     entries, and 12 source-locked entries after Batch 08
   - capture `07-battle-world-visuals.png` and verify the Bedroom Dream
     background is visible behind the bed, active cat, enemies, litter box, and
     feeder without hiding gameplay silhouettes
   - do not treat this non-cat background acceptance as permission to formally
     import starter cat candidate sprites

Updated offline validation: Runtime and EditModeTests compile after adding the
Bedroom Dream battle background runtime binding. EditMode source now has 371
`[Test]` markers. Unity editor execution and `07-battle-world-visuals.png`
review remain pending.

113. P0 runtime visual contact sheet evidence gate:
   - confirm contact sheet exists:
     `design/development/asset_review/p0_runtime_visual_contact_sheet_2026-06-14.png`
   - confirm review note exists:
     `design/development/asset_review/p0_runtime_visual_contact_sheet_2026-06-14.md`
   - confirm builder exists:
     `design/development/tools/build_runtime_visual_contact_sheet.ps1`
   - confirm the sheet covers all 34 runtime bindings in
     `P0VisualAssetCatalog.CreateP0RuntimeBindings()`
   - confirm `P0AssetReviewPacket` includes `Runtime Visual Contact Sheet
     Evidence`
   - confirm `P0AssetProductionReadiness` fails if the contact sheet is missing
   - use this sheet as the offline visual baseline before comparing Play Mode
     screenshots
   - do not treat this contact sheet as starter-cat import approval; cat import
     remains gated by active-cat screenshots and colored three-view turnarounds

Updated offline validation: Runtime and EditModeTests compile after adding the
runtime visual contact sheet readiness gate. `git diff --check` passes, touched
text files have no trailing whitespace, and EditMode source now has 372
`[Test]` markers. Unity editor execution and Play Mode screenshot comparison
remain pending.

114. P0 Play Mode evidence runtime visual baseline gate:
   - confirm `P0PlayModeScreenshotSmoke.ExpectedRuntimeVisualBindingIds`
     contains 34 ids in the same order as
     `P0VisualAssetCatalog.CreateP0RuntimeBindings()`
   - confirm `P0PlayModeScreenshotSmoke.RuntimeVisualContactSheetPath` equals
     `P0AssetProductionReadiness.RuntimeVisualContactSheetPath`
   - confirm `P0PlayModeEvidenceChecklist` includes
     `runtime_visual_contact_sheet`
   - confirm `P0PlayModeEvidenceChecklist` includes
     `screenshot_file_evidence`
   - confirm `P0PlayModeAcceptanceSmoke.ExpectedEvidenceCheckCount` is 7
   - confirm full Play Mode acceptance fails if the runtime visual contact
     sheet is missing, even if the screenshot plan itself is registered
   - confirm full Play Mode acceptance fails while screenshot file evidence is
     incomplete:
     - current expected screenshots: 3/10
     - current unexpected stale PNG: `04-settlement.png`
   - after Unity screenshots are regenerated, compare the 10 captures against
     both the runtime visual contact sheet and the starter-cat colored
     turnaround contact sheet

Updated offline validation: Runtime and EditModeTests compile after adding the
Play Mode runtime visual baseline evidence gate. `git diff --check` passes,
touched text files have no trailing whitespace, and EditMode source now has
374 `[Test]` markers. Unity editor execution and screenshot comparison remain
pending.

115. P0 visual acceptance top-level report gate:
   - confirm `P0VisualAcceptanceReport` composes:
     - `P0PlayableReadiness`
     - `P0AssetProductionReadiness`
     - `P0AssetReviewPacket`
     - `P0RuntimeVisualBindingCoverage`
     - `P0PlayModeScreenshotFileEvidence`
     - `P0PlayModeEvidenceChecklist`
     - `P0StarterCatFormalImportReadiness`
   - confirm current report says architecture is ready for systematic asset
     production
   - confirm current report says final P0 visual acceptance is not ready
   - confirm current report lists missing Play Mode screenshots, including the
     three active-cat captures and `07-battle-world-visuals.png`
   - confirm current report keeps starter-cat formal imports blocked until the
     active-cat screenshots are approved against colored three-view turnarounds
   - write the report from Unity via:
     `TheCat/P0/Write P0 Visual Acceptance Report`

Updated offline validation: Runtime and EditModeTests compile after adding the
top-level visual acceptance report. EditMode source now has 376 `[Test]`
markers. Unity editor menu execution and screenshot comparison remain pending.

116. P0 Batch 06 route node icon gate:
   - confirm 7 new non-Boss route node icon PNGs exist under
     `Assets/TheCat/Art/UI/Icons`
   - confirm each new route node icon has matching P0 Sprite `.png.meta`
     import settings
   - confirm `P0AssetManifestCatalog` and `P0_ASSET_MANIFEST.csv` include the
     7 new route node icon rows
   - confirm `P0AssetGenerationBatchCatalog.RouteNodeIconBatchId` assigns the
     7 assets exactly once
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` reports 28
     runtime visual bindings
   - confirm `P0RouteNodePresenter` exposes icon assets for all 8 route node
     types
   - confirm runtime visual contact sheet covers 28 bindings
   - confirm starter-cat formal import remains blocked and unchanged

Updated offline validation: Runtime and EditModeTests compile after adding
Batch 06 route node icons. `git diff --check` passes, all 7 new route icon
PNGs are 128x128 with P0 import meta markers, and EditMode source now has 377
`[Test]` markers. Unity route-map screenshot review remains pending.

117. P0 Batch 07 starter cat source-lock packet gate:
   - confirm packet markdown exists:
     `design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.md`
   - confirm packet CSV exists:
     `design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.csv`
   - confirm deterministic builder exists:
     `design/development/tools/build_starter_cat_source_lock_packet.ps1`
   - confirm packet records Saiban, Nephthys, and Suzune source-lock ids,
     colored turnaround paths and SHA-256 hashes, locked Unity sprite paths and
     SHA-256 hashes, runtime binding ids, active-cat screenshot filenames, and
     Batch 05 review sheets
   - confirm `P0StarterCatSourceLockPacketEvidence` reports ready
   - confirm `P0AssetReviewPacket` includes `Starter Cat Source-Lock Packet
     Evidence`
   - confirm `P0AssetProductionReadiness` fails if the packet is missing or
     stale
   - confirm starter-cat formal import remains blocked until
     `04-active-cat-saiban.png`, `05-active-cat-nephthys.png`, and
     `06-active-cat-suzune.png` exist and pass side-by-side review against the
     colored three-view turnaround contact sheet
   - refresh the Unity-generated review packet from menu:
     `TheCat/P0/Write P0 Asset Review Packet`

Updated offline validation: Runtime and EditModeTests compile after adding the
Batch 07 source-lock packet gate. The builder ran successfully, `git diff
--check` passes, source-lock packet key strings are present, and EditMode
source now has 380 `[Test]` markers. Unity editor menu execution and active-cat
screenshot comparison remain pending.

118. P0 Batch 08 UI shell asset gate:
   - confirm 6 new non-cat UI shell PNGs exist under `Assets/TheCat/Art/UI`
   - confirm each new UI shell PNG has matching P0 `.png.meta` import settings
   - confirm `P0AssetManifestCatalog` and `P0_ASSET_MANIFEST.csv` include the
     6 new UI shell rows
   - confirm `P0AssetGenerationBatchCatalog.UiShellBatchId` assigns the 6
     assets exactly once
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` reports 34
     runtime visual bindings
   - confirm runtime visual contact sheet covers 34 bindings
   - confirm `P0AssetReviewPacket` reports 38 review assets and 34
     runtime-bound entries
   - confirm `P0AssetProductionReadiness` passes while starter-cat formal
     import remains blocked and unchanged
   - capture updated main-menu and settlement screenshots after the UI shell is
     wired into the runtime draw path

Updated offline validation: Batch 08 generated non-cat UI shell assets,
refreshed the runtime visual contact sheet to 34 bindings, passed Runtime /
EditModeTests / Editor MSBuild compile, passed `git diff --check`, and confirmed
all 6 new PNGs have expected dimensions plus P0 import meta markers. Unity
MCP local setup files are present, but Unity MCP tools were not exposed in this
session; editor Console, AssetDatabase refresh, screenshot comparison, and
readability review remain pending.

119. P0 UI shell runtime draw-path gate:
   - confirm `P0UiShellPresenter.BuildSurface()` exposes exactly 6 Batch 08 UI
     shell assets
   - confirm `P0MainMenuSurface.UiShell` references the main menu background,
     title logo, dreamglass panel, and primary button frame
   - confirm `P0RouteMapSurface.UiShell` references the dreamglass panel,
     primary button frame, fish treat icon, and dream shard icon
   - load `P0MainMenu` and confirm the dream-entry background, title logo,
     panel, and buttons are visible with no critical text overlap
   - load `P0RouteMap` and confirm the dreamglass panel, main buttons, reward
     choice buttons, and settlement wallet icons are visible and readable
   - capture updated screenshots:
     - `01-main-menu.png`
     - `02-route-map-layer1.png`
     - `10-settlement.png`
   - compare the updated screenshots against the 34-slot runtime visual contact
     sheet before accepting the UI shell as presentation-ready
   - keep starter-cat formal import blocked unless the active-cat screenshot
     gate separately approves Saiban, Nephthys, and Suzune against the colored
     three-view turnarounds

Updated offline validation: Runtime, EditModeTests, and Editor MSBuild compile
after adding `P0UiShellPresenter`, UI shell surface coverage, IMGUI texture
drawing helpers, and main-menu / route-map draw-path wiring. `git diff --check`
passes. EditMode source now has 382 `[Test]` markers across 70 test files.
Unity editor execution and screenshot readability review remain pending.

120. P0 Batch 09 battle feedback VFX gate:
   - confirm 6 new non-cat VFX PNGs exist under `Assets/TheCat/Art/VFX`
   - confirm each VFX PNG has matching P0 `.png.meta` Sprite import settings
   - confirm `P0_ASSET_MANIFEST.csv` and `P0AssetManifestCatalog` include the
     6 Batch 09 VFX rows
   - confirm `P0AssetGenerationBatchCatalog.BattleFeedbackVfxBatchId` assigns
     the 6 assets exactly once after the Batch 08 UI shell
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` reports 40
     runtime visual bindings
   - confirm runtime visual contact sheet covers 40 bindings and includes:
     - `feedback.hit_spark`
     - `feedback.bed_shield_pulse`
     - `feedback.sleep_stable_wave`
     - `feedback.litter_cleanse`
     - `feedback.feeder_kibble`
     - `feedback.enemy_mark_ring`
   - load `P0GrayboxBattle` and trigger:
     - a shield skill feedback card
     - a target/status skill feedback card
     - litter box interaction feedback
     - feeder interaction feedback
     - bed care / Sleep Stable feedback
     - cat pressure / weak feedback
   - confirm the feedback card draws the expected VFX icon without text overlap
   - keep starter-cat formal import blocked unless the active-cat screenshot
     gate separately approves Saiban, Nephthys, and Suzune against the colored
     three-view turnarounds

Updated offline validation: Batch 09 generated non-cat battle feedback VFX,
refreshed the runtime visual contact sheet to 40 bindings, and kept starter-cat
source locks/formal import state unchanged. Unity editor execution, Console
check, AssetDatabase refresh, and screenshot readability review remain pending.

121. P0 Batch 10 enemy warning VFX gate:
   - confirm 4 new non-cat warning VFX PNGs exist under
     `Assets/TheCat/Art/Enemies/VFX`
   - confirm each VFX PNG has matching P0 `.png.meta` Sprite import settings
   - confirm `P0_ASSET_MANIFEST.csv` and `P0AssetManifestCatalog` include the
     4 Batch 10 VFX rows
   - confirm `P0AssetGenerationBatchCatalog.EnemyWarningVfxBatchId` assigns the
     4 assets exactly once after the Batch 09 battle feedback VFX
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` reports 44
     runtime visual bindings
   - confirm runtime visual contact sheet covers 44 bindings and includes:
     - `warning.black_mud_bed_claw`
     - `warning.cold_light_beam`
     - `warning.call_tyrant_app_throw`
     - `warning.call_tyrant_summon_portal`
   - load `P0GrayboxBattle` and trigger:
     - Black Mud near-bed warning
     - Cold Light ranged pressure warning
     - Call Tyrant boss throw warning
     - Call Tyrant boss summon warning
   - confirm warning rings/lines draw the expected VFX without text overlap or
     unreadable color blending
   - keep starter-cat formal import blocked unless the active-cat screenshot
     gate separately approves Saiban, Nephthys, and Suzune against the colored
     three-view turnarounds

Updated offline validation: Batch 10 generated source-locked non-cat enemy
warning VFX, refreshed the runtime visual contact sheet to 44 bindings, and
kept starter-cat source locks/formal import state unchanged. Unity editor
execution, Console check, AssetDatabase refresh, and screenshot readability
review remain pending.

122. P0 Batch 11 enemy animation framesheet gate:
   - confirm 3 new non-cat enemy/Boss framesheet PNGs exist under
     `Assets/TheCat/Art/Enemies/Frames`
   - confirm each framesheet PNG has matching P0 `.png.meta` Sprite import
     settings with `flipbookColumns: 4`
   - confirm `P0_ASSET_MANIFEST.csv` and `P0AssetManifestCatalog` include the
     3 Batch 11 framesheet rows
   - confirm `P0AssetGenerationBatchCatalog.EnemyAnimationFramesheetBatchId`
     assigns the 3 assets exactly once after the Batch 10 enemy warning VFX
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` reports 47
     runtime visual bindings
   - confirm `P0VisualAssetCatalog.GetEnemyAnimationFramesheet(enemyId)`
     resolves:
     - Black Mud Nightmare -> `black_mud_animation`
     - Cold Light Shadow -> `cold_light_animation`
     - Call Tyrant -> `call_tyrant_animation`
   - confirm runtime visual contact sheet covers 47 bindings and includes:
     - `enemy.anim.black_mud_move`
     - `enemy.anim.cold_light_cast`
     - `enemy.anim.call_tyrant_boss_pattern`
   - load `P0GrayboxBattle` and trigger:
     - Black Mud movement / bed-pressure loop
     - Cold Light ranged cast / pressure loop
     - Call Tyrant boss throw or summon pattern loop
   - confirm the framesheet-based previews draw with transparent backgrounds,
     no white sheet bleed, no unreadable scaling, and no overlap with warning
     VFX or HUD text
   - keep starter-cat formal import blocked unless the active-cat screenshot
     gate separately approves Saiban, Nephthys, and Suzune against the colored
     three-view turnarounds

Updated offline validation: Batch 11 generated source-cropped non-cat
enemy/Boss animation framesheets, refreshed the runtime visual contact sheet to
47 bindings, and kept starter-cat source locks/formal import state unchanged.
Unity editor execution, Console check, AssetDatabase refresh, Animator/prefab
wiring, and screenshot readability review remain pending.

123. P0 Batch 12 route choice icon gate:
   - confirm 6 new non-cat route choice icon PNGs exist under
     `Assets/TheCat/Art/UI/Icons`
   - confirm each choice icon PNG has matching P0 `.png.meta` Sprite import
     settings
   - confirm `P0_ASSET_MANIFEST.csv` and `P0AssetManifestCatalog` include the
     6 Batch 12 route choice icon rows
   - confirm `P0AssetGenerationBatchCatalog.RouteChoiceIconBatchId` assigns the
     6 assets exactly once after the Batch 11 enemy animation framesheets
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` reports 53
     runtime visual bindings
   - confirm `P0VisualAssetCatalog.GetRouteChoiceIcon(choiceType)` resolves:
     - `RecruitPartner` -> `route_choice.partner_recruit`
     - `PurchaseSupply` -> `route_choice.purchase_supply`
     - `GainAuthorityBlessing` -> `route_choice.authority_blessing`
     - `UpgradeAuthorityBlessing` -> `route_choice.authority_upgrade`
     - `RestSupply` -> `route_choice.rest_supply`
     - `DreamEventModifier` -> `route_choice.dream_event_modifier`
   - confirm runtime visual contact sheet covers 53 bindings and includes all
     six `route_choice.*` bindings
   - load `P0RouteMap` and expose reward-choice rows for:
     - partner recruitment
     - supply purchase
     - authority blessing gain / upgrade
     - rest supply
     - dream-event modifier
   - confirm the route choice icons draw inline without text overlap, unreadable
     scaling, or color blending against the dreamglass panel
   - keep starter-cat formal import blocked unless the active-cat screenshot
     gate separately approves Saiban, Nephthys, and Suzune against the colored
     three-view turnarounds

Updated offline validation: Batch 12 generated deterministic non-cat route
choice icons, refreshed the runtime visual contact sheet to 53 bindings, and
kept starter-cat source locks/formal import state unchanged. Unity editor
execution, Console check, AssetDatabase refresh, route-map screenshot
readability review, and final UI/prefab binding remain pending.

124. P0 Batch 13 route reward card frame gate:
   - confirm 5 new non-cat route reward card frame PNGs exist under
     `Assets/TheCat/Art/UI/Frames`
   - confirm each card frame PNG has matching P0 `.png.meta` Sprite import
     settings with the 18px sprite border marker
   - confirm `P0_ASSET_MANIFEST.csv` and `P0AssetManifestCatalog` include the
     5 Batch 13 route reward card frame rows
   - confirm `P0AssetGenerationBatchCatalog.RouteRewardCardFrameBatchId`
     assigns the 5 assets exactly once after the Batch 12 route choice icons
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` reports 58
     runtime visual bindings
   - confirm `P0VisualAssetCatalog.GetRouteRewardCardFrame(nodeType)` resolves:
     - `Partner` -> `route_reward_card.partner`
     - `Shop` -> `route_reward_card.shop`
     - `BlessingOffering` -> `route_reward_card.blessing`
     - `DreamEvent` -> `route_reward_card.dream_event`
     - `RestNest` -> `route_reward_card.rest_nest`
   - confirm runtime visual contact sheet covers 58 bindings and includes all
     five `route_reward_card.*` bindings
   - load `P0RouteMap` and expose reward-choice rows for:
     - partner recruitment
     - shop supply purchase
     - authority blessing
     - dream-event modifier
     - rest-nest recovery
   - confirm each reward choice draws inside the correct category card frame
     with inline icon, no text overlap, readable scale, and no cropped frame
     borders
   - keep starter-cat formal import blocked unless the active-cat screenshot
     gate separately approves Saiban, Nephthys, and Suzune against the colored
     three-view turnarounds

Updated offline validation: Batch 13 generated deterministic non-cat route
reward card frames, refreshed the runtime visual contact sheet to 58 bindings,
and kept starter-cat source locks/formal import state unchanged. Unity editor
execution, Console check, AssetDatabase refresh, route-map screenshot
readability review, and final UGUI/prefab binding remain pending.

125. P0 Batch 14 status compact icon gate:
   - confirm 5 new non-cat compact status icon PNGs exist under
     `Assets/TheCat/Art/UI/Icons`
   - confirm each compact status icon PNG is `32x32` with matching P0
     `.png.meta` Sprite import settings
   - confirm `P0_ASSET_MANIFEST.csv` and `P0AssetManifestCatalog` include the
     5 Batch 14 compact status icon rows
   - confirm `P0AssetGenerationBatchCatalog.StatusCompactIconBatchId` assigns
     the 5 assets exactly once after the Batch 13 route reward card frames
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` reports 63
     runtime visual bindings
   - confirm `P0VisualAssetCatalog.GetCompactStatusIcon(statusTagId)` resolves:
     - `SleepStable` -> `status_compact.sleep_stable`
     - `Slow` -> `status_compact.slow`
     - `Knockback` -> `status_compact.knockback`
     - `Mark` -> `status_compact.mark`
     - `Shield` -> `status_compact.shield`
   - confirm `P0StatusHudPresenter` entries carry both full 64px icons and
     compact 32px icons
   - confirm runtime visual contact sheet covers 63 bindings and includes all
     five `status_compact.*` bindings
   - load `P0Battle` and force Sleep Stable, Slow, Knockback, Mark, and Shield
     status rows to be visible on enemies, bed, and active cats
   - confirm compact icons are readable at battle HUD row size, do not overlap
     text, and remain visually paired with the full status icon language
   - keep starter-cat formal import blocked unless the active-cat screenshot
     gate separately approves Saiban, Nephthys, and Suzune against the colored
     three-view turnarounds

Updated offline validation: Batch 14 generated deterministic non-cat compact
status HUD icons, refreshed the runtime visual contact sheet to 63 bindings,
and kept starter-cat source locks/formal import state unchanged. Unity editor
execution, Console check, AssetDatabase refresh, status HUD screenshot
readability review, and final UGUI/prefab binding remain pending.

126. P0 Batch 15 route reward detail badge gate:
   - confirm 5 new non-cat route reward detail badge PNGs exist under
     `Assets/TheCat/Art/UI/Badges`
   - confirm each route reward detail badge PNG is `192x64` with matching P0
     `.png.meta` Sprite import settings and 8px sprite border
   - confirm `P0_ASSET_MANIFEST.csv` and `P0AssetManifestCatalog` include the
     5 Batch 15 route reward detail badge rows
   - confirm `P0AssetGenerationBatchCatalog.RouteRewardDetailBadgeBatchId`
     assigns the 5 assets exactly once after the Batch 14 status compact icons
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` reports 68
     runtime visual bindings
   - confirm `P0VisualAssetCatalog.GetRouteRewardDetailBadge(choice)` resolves:
     - gains -> `route_reward_detail.gain`
     - costs -> `route_reward_detail.cost`
     - recovery effects -> `route_reward_detail.recovery`
     - risk / next-battle penalties -> `route_reward_detail.risk`
     - authority upgrade choices -> `route_reward_detail.upgrade`
   - confirm runtime visual contact sheet covers 68 bindings and includes all
     five `route_reward_detail.*` bindings
   - load `P0RouteMap` and expose reward-choice rows for gain, cost, recovery,
     risk, and authority upgrade cases
   - confirm each reward choice draws the detail badge on the right side of
     the framed card, no text overlap, readable scale, and no cropped badge
   - keep starter-cat formal import blocked unless the active-cat screenshot
     gate separately approves Saiban, Nephthys, and Suzune against the colored
     three-view turnarounds

Updated offline validation: Batch 15 generated deterministic non-cat route
reward detail badges, refreshed the runtime visual contact sheet to 68
bindings, and kept starter-cat source locks/formal import state unchanged.
Unity editor execution, Console check, AssetDatabase refresh, route-map
screenshot readability review, and final UGUI/prefab binding remain pending.

127. P0 Batch 16 authority blessing seal gate:
   - confirm 3 new non-cat authority blessing seal PNGs exist under
     `Assets/TheCat/Art/UI/Icons`
   - confirm each authority blessing seal PNG is `128x128` with matching P0
     `.png.meta` Sprite import settings and
     `batch:p0_asset_batch_16_authority_blessing_seals`
   - confirm `P0_ASSET_MANIFEST.csv` and `P0AssetManifestCatalog` include the
     3 Batch 16 authority blessing seal rows
   - confirm `P0AssetGenerationBatchCatalog.AuthorityBlessingSealBatchId`
     assigns the 3 assets exactly once after the Batch 15 route reward detail
     badges
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` reports 71
     runtime visual bindings
   - confirm `P0VisualAssetCatalog.GetAuthorityBlessingSeal(blessingId)`
     resolves:
     - `authority_oath_bedline` -> `blessing_detail.oath_bedline`
     - `authority_dominion_sandglass` -> `blessing_detail.dominion_sandglass`
     - `authority_rhythm_lullaby` -> `blessing_detail.rhythm_lullaby`
   - confirm runtime visual contact sheet covers 71 bindings and includes all
     three `blessing_detail.*` bindings
   - load `P0RouteMap`, move to `layer_07_blessing`, and confirm the blessing
     reward choices show the three specific seal assets in their framed cards
   - keep starter-cat formal import blocked unless the active-cat screenshot
     gate separately approves Saiban, Nephthys, and Suzune against the colored
     three-view turnarounds

Updated offline validation: Batch 16 generated deterministic non-cat authority
blessing seals, refreshed the runtime visual contact sheet to 71 bindings, and
kept starter-cat source locks/formal import state unchanged. Unity editor
execution, Console check, AssetDatabase refresh, route-map blessing screenshot
readability review, and final UGUI/prefab binding remain pending.

128. P0 starter cat turnaround conformance gate:
   - confirm `P0StarterCatTurnaroundConformanceSpec.EvaluateP0Spec()` reports:
     - 3 starter cat specs
     - 3 source-lock matches
     - 3 existing colored three-view source files
     - 9 front-view anchors
     - 9 side-view anchors
     - 9 back-view anchors
     - 9 palette anchors
     - 9 prop/costume anchors
     - 12 prohibited drift rules
   - confirm `P0AssetReviewPacket` includes the "Starter Cat Turnaround
     Conformance Spec" section and points to
     `design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`
   - confirm `P0AssetProductionReadiness` requires the conformance spec and
     still reports starter-cat formal import as blocked
   - run Play Mode screenshot smoke and capture:
     - `04-active-cat-saiban.png`
     - `05-active-cat-nephthys.png`
     - `06-active-cat-suzune.png`
   - compare each active-cat screenshot against the conformance spec:
     - Saiban: silver-gray tabby face, red cape, silver-gold armor, round sun
       shield, single sword, side/back cape and striped tail identity
     - Nephthys: gold-brown tabby face, dark blue hood/cloak, gold script
       border, floating pyramid/obelisk, back ankh/winged gem, striped tail
     - Suzune: calico patches, white shrine robe, vermilion skirt/bow, gold
       bells, bell wand, blue snowflake/talisman motifs, calico tail
   - reject any future cat candidate that matches only the front view while
     drifting from side/back anchors, palette, props, or non-human cat body
     proportions

Updated offline validation: Added the starter cat turnaround conformance code
gate, human-readable conformance spec, and agent prompt. Runtime and EditMode
MSBuild compiles passed. Unity editor execution, Console check, AssetDatabase
refresh, active-cat screenshot capture, and formal import approval remain
pending.

129. P0 starter cat candidate review-note conformance gate:
   - confirm `P0StarterCatDerivativeCandidateEvidence.EvaluateBatch05()`
     reports:
     - 20/20 expected evidence files
     - 12/12 candidate PNG files
     - 3/3 review notes
     - 3/3 review sheets
     - 3/3 conformance spec mentions
     - 3/3 front-view anchor sections
     - 3/3 side-view anchor sections
     - 3/3 back-view anchor sections
     - 3/3 palette anchor sections
     - 3/3 prop/costume anchor sections
     - 3/3 prohibited-drift sections
   - confirm `P0AssetReviewPacket` includes the new candidate-note
     conformance counts under "Starter Cat Derivative Candidate Evidence"
   - confirm Batch 05 candidate review notes for Saiban, Nephthys, and Suzune
     include `Turnaround Conformance Checklist` and all six required
     subsection groups
   - keep all candidate PNGs under
     `design/development/asset_candidates/starter_cats`; do not import them
     into `Assets`
   - run Play Mode screenshot smoke before any future approval of a strict
     starter-cat candidate

Updated offline validation: Batch 05 candidate review notes now carry the
strict front/side/back, palette, prop/costume, and prohibited-drift checklist.
Runtime, EditModeTests, and Editor MSBuild compiles passed. Unity editor
execution, Console check, AssetDatabase refresh, Play Mode screenshots, and
formal import approval remain pending.

130. P0 Batch 19 non-battle node summary banner gate:
   - confirm 3 new non-cat route node summary banner PNGs exist under
     `Assets/TheCat/Art/UI/Banners`
   - confirm each node summary banner PNG is `512x160` with matching P0
     `.png.meta` Sprite import settings,
     `batch:p0_asset_batch_19_nonbattle_node_summary_banners`,
     `spriteBorder:12`, and `nonCatSymbolicOnly:true`
   - confirm `P0AssetManifestCatalog` includes the 3 Batch 19 banner rows
     and reports 78 manifest assets
   - confirm `P0AssetGenerationBatchCatalog.NonBattleNodeSummaryBannerBatchId`
     assigns the 3 assets exactly once after the Batch 16 authority blessing
     seals
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` reports 74
     runtime visual bindings
   - confirm `P0VisualAssetCatalog.GetRouteNodeSummaryBanner(nodeType)`
     resolves:
     - `Shop` -> `route_summary.shop`
     - `DreamEvent` -> `route_summary.dream_event`
     - `RestNest` -> `route_summary.rest_nest`
   - confirm runtime visual contact sheet covers 74 bindings and includes all
     three `route_summary.*` bindings
   - load `P0RouteMap`, move to `layer_02_shop_early`,
     `layer_02_dream_event`, and `layer_07_rest_nest`, and confirm the
     current-node card displays the matching banner without text overlap
   - keep starter-cat formal import blocked unless the active-cat screenshot
     gate separately approves Saiban, Nephthys, and Suzune against the colored
     three-view turnarounds

Updated offline validation: Batch 19 generated deterministic non-cat route
node summary banners, refreshed the runtime visual contact sheet to 74
bindings, and kept starter-cat source locks/formal import state unchanged.
Unity editor execution, Console check, AssetDatabase refresh, route-map
current-node screenshot readability review, and final UGUI/prefab binding
remain pending.

131. P0 Batch 20 shop item card gate:
   - confirm 4 new non-cat shop item card PNGs exist under
     `Assets/TheCat/Art/UI/Cards`
   - confirm each shop item card PNG is `384x160` with matching P0 `.png.meta`
     Sprite import settings, `batch:p0_asset_batch_20_shop_item_cards`,
     `spriteBorder:12`, and `nonCatSymbolicOnly:true`
   - confirm `P0AssetManifestCatalog` includes the 4 Batch 20 card rows and
     reports 82 manifest assets
   - confirm `P0AssetGenerationBatchCatalog.ShopItemCardBatchId` assigns the
     4 assets exactly once after the Batch 19 non-battle node summary banners
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` reports 78
     runtime visual bindings
   - confirm `P0VisualAssetCatalog.GetShopItemCard(choice)` resolves:
     - `shop_bed_patch` -> `shop_item.bed_patch`
     - `shop_litter_sachet` -> `shop_item.litter_sachet`
     - `shop_late_kibble` -> `shop_item.late_kibble`
     - `shop_free_sample` -> `shop_item.free_sample`
   - confirm runtime visual contact sheet covers 78 bindings and includes all
     four `shop_item.*` bindings
   - load `P0RouteMap`, move to `layer_02_shop_early` with enough fish treats,
     and confirm the shop reward choices display the matching item cards
     without text overlap
   - keep starter-cat formal import blocked unless the active-cat screenshot
     gate separately approves Saiban, Nephthys, and Suzune against the colored
     three-view turnarounds

Updated offline validation: Batch 20 generated deterministic non-cat shop item
cards, verified the four PNGs are `384x160`, verified their `.png.meta` files
carry the Batch 20 marker plus Sprite import settings, refreshed the runtime
visual contact sheet to 78 bindings, and kept starter-cat source locks/formal
import state unchanged. Runtime, EditModeTests, and Editor MSBuild compiles
passed; Editor compile still reports the known `MSB3277` Unity/VS reference
conflict warning. Unity editor execution, Console check, AssetDatabase refresh,
route-map shop screenshot readability review, and final UGUI/prefab binding
remain pending.

132. P0 Batch 21 DreamEvent choice card gate:
   - confirm 3 new non-cat DreamEvent choice card PNGs exist under
     `Assets/TheCat/Art/UI/Cards`
   - confirm each DreamEvent choice card PNG is `384x160` with matching P0
     `.png.meta` Sprite import settings,
     `batch:p0_asset_batch_21_dream_event_choice_cards`, `spriteBorder:12`,
     and `nonCatSymbolicOnly:true`
   - confirm `P0AssetManifestCatalog` includes the 3 Batch 21 card rows and
     reports 85 manifest assets
   - confirm `P0AssetGenerationBatchCatalog.DreamEventChoiceCardBatchId`
     assigns the 3 assets exactly once after the Batch 20 shop item cards
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` reports 81
     runtime visual bindings
   - confirm `P0VisualAssetCatalog.GetRouteChoiceCard(choice)` resolves:
     - `dream_event_clear_notifications`
     - `dream_event_catnip_residue`
     - `dream_event_mark_all_read`
   - confirm runtime visual contact sheet covers 81 bindings and includes all
     three `dream_event_choice.*` bindings
   - load `P0RouteMap`, move to `layer_02_dream_event`, and confirm the
     DreamEvent reward choices display the matching choice cards without text
     overlap
   - keep starter-cat formal import blocked unless the active-cat screenshot
     gate separately approves Saiban, Nephthys, and Suzune against the colored
     three-view turnarounds

Updated offline validation: Batch 21 generated deterministic non-cat
DreamEvent choice cards, refreshed the runtime visual contact sheet to 81
bindings, and kept starter-cat source locks/formal import state unchanged.
Runtime, EditModeTests, and Editor MSBuild compiles passed; Editor compile
still reports the known `MSB3277` Unity/VS reference conflict warning. Console,
AssetDatabase refresh, route-map DreamEvent screenshot readability review, and
final UGUI/prefab binding remain pending.

133. P0 Batch 22 RestNest recovery card gate:
   - confirm 1 new non-cat RestNest recovery card PNG exists under
     `Assets/TheCat/Art/UI/Cards`
   - confirm the RestNest recovery card PNG is `384x160` with matching P0
     `.png.meta` Sprite import settings,
     `batch:p0_asset_batch_22_rest_nest_recovery_card`, `spriteBorder:12`,
     and `nonCatSymbolicOnly:true`
   - confirm `P0AssetManifestCatalog` includes the Batch 22 card row and
     reports 86 manifest assets
   - confirm `P0AssetGenerationBatchCatalog.RestNestRecoveryCardBatchId`
     assigns the asset exactly once after the Batch 21 DreamEvent choice cards
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` reports 82
     runtime visual bindings
   - confirm `P0VisualAssetCatalog.GetRouteChoiceCard(choice)` resolves
     `rest_nest_recovery`
   - confirm runtime visual contact sheet covers 82 bindings and includes
     `rest_nest_choice.recovery`
   - load `P0RouteMap`, move to `layer_07_rest_nest`, and confirm the
     RestNest reward choice displays the matching recovery card without text
     overlap
   - keep starter-cat formal import blocked unless the active-cat screenshot
     gate separately approves Saiban, Nephthys, and Suzune against the colored
     three-view turnarounds

Updated offline validation: Batch 22 generated a deterministic non-cat
RestNest recovery card, refreshed the runtime visual contact sheet to 82
bindings, and kept starter-cat source locks/formal import state unchanged.
Runtime, EditModeTests, and Editor MSBuild compiles passed; Editor compile
still reports the known `MSB3277` Unity/VS reference conflict warning. Console,
AssetDatabase refresh, route-map RestNest screenshot readability review, and
final UGUI/prefab binding remain pending until Unity MCP/editor tools are
available.

134. P0 Batch 23 Partner choice card gate:
   - confirm 2 new non-cat partner choice card PNGs exist under
     `Assets/TheCat/Art/UI/Cards`
   - confirm both partner choice card PNGs are `384x160` with matching P0
     `.png.meta` Sprite import settings,
     `batch:p0_asset_batch_23_partner_choice_cards`, `spriteBorder:12`, and
     `nonCatSymbolicOnly:true`
   - confirm `P0AssetManifestCatalog` includes the 2 Batch 23 card rows and
     reports 88 manifest assets
   - confirm `P0AssetGenerationBatchCatalog.PartnerChoiceCardBatchId` assigns
     the 2 assets exactly once after the Batch 22 RestNest recovery card
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` reports 84
     runtime visual bindings
   - confirm `P0VisualAssetCatalog.GetRouteChoiceCard(choice)` resolves:
     - `partner_shadowmaru_preview`
     - `partner_preview_duplicate_supply`
   - confirm runtime visual contact sheet covers 84 bindings and includes both
     `partner_choice.*` bindings
   - load `P0RouteMap`, move to `layer_03_partner`, and confirm the first
     partner reward choice displays the invite card without text overlap
   - load `P0RouteMap` with `shadowmaru_preview` already in the run roster and
     confirm the duplicate-fallback reward choice displays the night-fish
     supply card without text overlap
   - keep starter-cat formal import blocked unless the active-cat screenshot
     gate separately approves Saiban, Nephthys, and Suzune against the colored
     three-view turnarounds

Updated offline validation: Batch 23 generated deterministic non-cat partner
choice cards, refreshed the runtime visual contact sheet to 84 bindings, and
kept starter-cat source locks/formal import state unchanged. Runtime,
EditModeTests, and Editor MSBuild compiles passed; Editor compile still
reports the known `MSB3277` Unity/VS reference conflict warning. Console,
AssetDatabase refresh, route-map partner screenshot readability review, and
final UGUI/prefab binding remain pending until Unity MCP/editor tools are
available.

135. P0 Batch 24 Blessing choice card gate:
   - confirm 3 new non-cat authority blessing choice card PNGs exist under
     `Assets/TheCat/Art/UI/Cards`
   - confirm all blessing choice card PNGs are `384x160` with matching P0
     `.png.meta` Sprite import settings,
     `batch:p0_asset_batch_24_blessing_choice_cards`, `spriteBorder:12`, and
     `nonCatSymbolicOnly:true`
   - confirm `P0AssetManifestCatalog` includes the 3 Batch 24 card rows and
     reports 91 manifest assets
   - confirm `P0AssetGenerationBatchCatalog.BlessingChoiceCardBatchId`
     assigns the 3 assets exactly once after the Batch 23 partner choice cards
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` reports 87
     runtime visual bindings
   - confirm `P0VisualAssetCatalog.GetRouteChoiceCard(choice)` resolves:
     - `blessing_authority_oath_bedline`
     - `blessing_authority_dominion_sandglass`
     - `blessing_authority_rhythm_lullaby`
     - the matching `blessing_upgrade_authority_*` choices
   - confirm runtime visual contact sheet covers 87 bindings and includes all
     `blessing_choice.*` bindings
   - load `P0RouteMap`, move to `layer_07_blessing`, and confirm the first-pick
     blessing reward choices display the matching authority cards without text
     overlap
   - load `P0RouteMap` with all three P0 authority blessings already owned and
     confirm the upgrade reward choices display the matching authority cards
     with upgrade badges and no text overlap
   - keep starter-cat formal import blocked unless the active-cat screenshot
     gate separately approves Saiban, Nephthys, and Suzune against the colored
     three-view turnarounds

Updated offline validation: Batch 24 generated deterministic non-cat authority
blessing choice cards, refreshed the runtime visual contact sheet to 87
bindings, and kept starter-cat source locks/formal import state unchanged.
Runtime, EditModeTests, and Editor MSBuild compiles passed; Editor compile
still reports the known `MSB3277` Unity/VS reference conflict warning. Console,
AssetDatabase refresh, route-map blessing screenshot readability review, and
final UGUI/prefab binding remain pending until Unity MCP/editor tools are
available.

136. P0 Batch 25 Result and settlement outcome banner gate:
   - confirm 4 new non-cat outcome banner PNGs exist under
     `Assets/TheCat/Art/UI/Banners`
   - confirm all outcome banner PNGs are `512x160` with matching P0
     `.png.meta` Sprite import settings,
     `batch:p0_asset_batch_25_result_settlement_banners`, `spriteBorder:16`,
     and `nonCatSymbolicOnly:true`
   - confirm `P0AssetManifestCatalog` includes the 4 Batch 25 banner rows and
     reports 95 manifest assets
   - confirm `P0AssetGenerationBatchCatalog.ResultSettlementBannerBatchId`
     assigns the 4 assets exactly once after the Batch 24 blessing choice cards
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` reports 91
     runtime visual bindings
   - confirm `P0VisualAssetCatalog.GetBattleResultOutcomeBanner(outcome)`
     resolves:
     - `BattleOutcome.Victory`
     - `BattleOutcome.Defeat`
   - confirm `P0VisualAssetCatalog.GetSettlementOutcomeBanner(isCleared)`
     resolves:
     - cleared route settlement
     - failed route settlement
   - confirm runtime visual contact sheet covers 91 bindings and includes:
     - `battle_result.victory_banner`
     - `battle_result.defeat_banner`
     - `settlement.run_cleared_banner`
     - `settlement.run_failed_banner`
   - load the graybox battle result flow and confirm
     `09-battle-result-layer1.png` shows the victory banner above result rows
     without text overlap
   - load failed battle result flow and confirm the defeat banner appears above
     route continuation rows
   - load route settlement after full clear and confirm `10-settlement.png`
     shows the cleared-run settlement banner above settlement telemetry rows
   - load failed route settlement and confirm the failed-run settlement banner
     appears above settlement telemetry rows
   - keep starter-cat formal import blocked unless the active-cat screenshot
     gate separately approves Saiban, Nephthys, and Suzune against the colored
     three-view turnarounds

Updated offline validation: Batch 25 generated deterministic non-cat outcome
banners, verified 512x160 PNG dimensions, verified P0 `.png.meta` settings,
refreshed the runtime visual contact sheet to 91 bindings, and passed Runtime,
EditModeTests, and Editor MSBuild compiles. Editor compile still reports the
known `MSB3277` Unity/VS reference conflict warning. Console, AssetDatabase
refresh, battle-result screenshot readability review, settlement screenshot
readability review, and final UGUI/prefab binding remain pending until Unity
MCP/editor tools are available.

137. P0 Batch 26 Starter cat candidate-pack gate:
   - run
     `powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_starter_cat_candidate_pack.ps1`
   - confirm the existing Batch 05 candidate manifest has 12 rows:
     3 starter cats x 4 allowed derivative asset types
   - confirm every candidate PNG stays under
     `design/development/asset_candidates/starter_cats`
   - confirm no starter-cat candidate PNG or review sheet has a Unity `.meta`
     file
   - confirm candidate, source turnaround, and locked Unity sprite SHA-256
     values match the candidate manifest
   - confirm per-cat review notes mention colored turnaround, front / side /
     back anchors, palette anchors, prohibited drift, rejection rules, and
     cat-specific traits
   - confirm every row recommendation remains
     `candidate_review_only_pending_playmode_screenshot`
   - in Unity Play Mode, capture `04-active-cat-saiban.png`,
     `05-active-cat-nephthys.png`, and `06-active-cat-suzune.png`
   - compare those screenshots against the colored three-view turnaround
     contact sheet before changing the starter-cat formal import state

Updated offline validation: the Batch 26 candidate-pack validator passed for
12 existing starter-cat candidate rows, 3 review notes, and 3 review sheets.
No Unity starter-cat import, source lock, manifest row, runtime binding, or
sprite file was changed. Unity active-cat screenshots and Console validation
remain pending until Unity MCP/editor tools are available.

138. P0 Batch 27 Core gauge bar gate:
   - confirm 8 new non-cat core gauge PNGs exist under
     `Assets/TheCat/Art/UI/Frames`
   - confirm all gauge PNGs are `384x48` with matching P0 `.png.meta` Sprite
     import settings, `batch:p0_asset_batch_27_core_gauge_bars`,
     `spriteBorder:10`, and `nonCatSymbolicOnly:true`
   - confirm `P0AssetManifestCatalog` includes the 8 Batch 27 gauge rows and
     reports 103 manifest assets
   - confirm `P0AssetGenerationBatchCatalog.CoreGaugeBarBatchId` assigns the
     8 assets exactly once after Batch 25
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` reports 99
     runtime visual bindings
   - confirm runtime visual contact sheet covers 99 bindings and includes:
     - `core_gauge.owner_sleep.frame`
     - `core_gauge.owner_sleep.fill`
     - `core_gauge.cat_hp.frame`
     - `core_gauge.cat_hp.fill`
     - `core_gauge.team_poop.frame`
     - `core_gauge.team_poop.fill`
     - `core_gauge.team_hunger.frame`
     - `core_gauge.team_hunger.fill`
   - load the graybox battle HUD and confirm owner sleep, cat HP, poop, and
     hunger gauges render with readable runtime labels and no text overlap
   - confirm the cat HP gauge is generic UI only and not a starter-cat body,
     marking, costume, or turnaround-derived asset
   - keep starter-cat formal import blocked unless the active-cat screenshot
     gate separately approves Saiban, Nephthys, and Suzune against the colored
     three-view turnarounds

Updated offline validation: Batch 27 generated deterministic non-cat core gauge
bar assets, refreshed the runtime visual contact sheet to 99 bindings, and
updated manifest/runtime coverage. PNG/meta validation passed for all 8 gauge
files, Runtime/EditModeTests/Editor MSBuild compiles passed, and
`git diff --check` passed. Editor compile still reports the known `MSB3277`
`System.Numerics.Vectors` Unity/VS reference warning. Unity Console,
AssetDatabase refresh, battle-HUD screenshot readability review, and final
UGUI/prefab binding remain pending until Unity MCP/editor tools are available.

139. P0 Batch 28 Starter cat strict reference-pack gate:
   - confirm Batch 17, Batch 18, Batch 26, and Batch 28 starter-cat prompts use
     real `design/梦境支配者核心玩法/...` source paths
   - confirm no starter-cat production prompt contains mojibake design path text
   - confirm every prompt names all three colored turnaround PNGs:
     - `saiban_turnaround_colored_2026-06-03.png`
     - `nephthys_turnaround_colored_2026-06-03.png`
     - `suzune_turnaround_colored_2026-06-03.png`
   - confirm every prompt keeps candidate output under
     `design/development/asset_candidates/starter_cats` and outside `Assets`
   - confirm every prompt states formal import remains blocked until active-cat
     Play Mode screenshot review passes
   - confirm `P0AssetProductionReadiness` includes
     `P0StarterCatProductionPromptReadiness`
   - confirm `P0AssetReviewPacket.BuildMarkdown()` includes "Starter Cat
     Production Prompt Readiness"
   - keep runtime cat sprites unchanged until active-cat screenshot review
     approves a candidate against the colored three-view turnarounds

Updated offline validation: Batch 28 added a code-backed prompt-readiness gate,
rebuilt starter-cat prompts with real UTF-8 design paths, and added a strict
reference-pack prompt for future one-cat-at-a-time candidate production. Runtime
EditModeTests, and Editor MSBuild compiles passed; Editor compile still reports
the known `MSB3277` Unity/VS reference warning. Candidate-pack validation and
`git diff --check` passed. Unity Console, AssetDatabase refresh, active-cat
screenshots, and human visual comparison remain pending until Unity MCP/editor
tools are available.

140. P0 Batch 29 Saiban strict turnaround derivatives:
   - confirm Batch 29 outputs remain under
     `design/development/asset_candidates/starter_cats`
   - confirm no Batch 29 candidate PNG has a Unity `.meta` file
   - confirm the Batch 29 manifest has 7 Saiban rows:
     - `front_view_reference_512`
     - `side_view_reference_512`
     - `back_view_reference_512`
     - `combat_sprite_reference_512`
     - `hud_avatar_reference_256`
     - `skill_icon_motif_reference_128`
     - `palette_swatch_reference_256`
   - confirm the review sheet is `1600x900` and shows source, front, side,
     back, combat, HUD, icon, and palette panels without overlap
   - confirm the review note states formal import remains blocked and candidate
     files stay outside `Assets`
   - confirm `P0StarterCatProductionPromptReadiness` now expects 5 prompts and
     includes Batch 29
   - capture `04-active-cat-saiban.png` in Play Mode before any Unity install
   - compare the active screenshot against the colored three-view turnaround and
     Batch 29 review sheet

Updated offline validation: Batch 29 generated a Saiban-only source-derived
candidate pack in Codex, outside Unity. The dedicated validator passed with
7 rows, 1 review note, and 1 review sheet. Unity import, Console checks,
AssetDatabase refresh, Sprite import settings, and active-cat screenshot review
remain pending until Unity MCP/editor tools are available.

141. P0 Batch 30 Saiban AI refinement candidate:
   - confirm Batch 30 outputs remain under
     `design/development/asset_candidates/starter_cats`
   - confirm no Batch 30 candidate PNG has a Unity `.meta` file
   - confirm the Batch 30 manifest has 3 rows:
     - `ai_refinement_raw_codex`
     - `ai_refinement_combat_1024`
     - `ai_refinement_combat_512_preview`
   - confirm the review sheet is `1600x900` and compares source turnaround,
     Batch 29 front reference, AI candidate, and 512 preview without overlap
   - confirm the review note states formal import remains blocked and candidate
     files stay outside `Assets`
   - confirm `P0StarterCatProductionPromptReadiness` now expects 6 prompts and
     includes Batch 30
   - capture `04-active-cat-saiban.png` in Play Mode before any Unity install
   - compare the active screenshot against the colored three-view turnaround,
     Batch 29 review sheet, and Batch 30 AI candidate
   - decide whether a transparent/cutout pass is needed before Unity import

Updated offline validation: Batch 30 archived the first Codex-generated Saiban
AI refinement candidate outside Unity. The dedicated validator passed with
3 rows, 1 review note, 1 review sheet, and 1 prompt record. Unity import,
Console checks, AssetDatabase refresh, Sprite import settings, active-cat
screenshot review, and cutout/transparent treatment remain pending until the
Unity-side gate is available.

142. P0 Batch 31 Saiban transparent cutout candidate:
   - confirm Batch 31 outputs remain under
     `design/development/asset_candidates/starter_cats`
   - confirm no Batch 31 candidate PNG has a Unity `.meta` file
   - confirm the Batch 31 manifest has 4 rows:
     - `cutout_alpha_1024`
     - `cutout_alpha_512_preview`
     - `cutout_checkerboard_512_review`
     - `cutout_alpha_mask_512_review`
   - confirm the review sheet is `1600x900` and compares source turnaround,
     Batch 30 input candidate, checkerboard cutout, dark-field cutout, alpha
     mask, and 512 preview without overlap
   - confirm the alpha PNG has transparent corners and an opaque visible Saiban
     center
   - confirm the review note states formal import remains blocked and candidate
     files stay outside `Assets`
   - confirm `P0StarterCatProductionPromptReadiness` now expects 7 prompts and
     includes Batch 31
   - if this candidate is later installed, verify Unity Sprite import settings
     for alpha, pivot, pixels-per-unit, filter mode, and compression
   - capture `04-active-cat-saiban.png` in Play Mode before any Unity install
   - compare the active screenshot against the colored three-view turnaround,
     Batch 29 review sheet, Batch 30 AI candidate, and Batch 31 cutout sheet
   - inspect edge quality against dark and warm HUD fields for parchment halos

Updated offline validation: Batch 31 generated a deterministic transparent
cutout candidate from the Batch 30 Saiban image outside Unity. The dedicated
validator passed with 4 rows, 1 review note, 1 review sheet, 1 process note,
and 1 agent prompt. Unity import, Console checks, AssetDatabase refresh, Sprite
import settings, active-cat screenshot review, and edge-halo QA remain pending
until Unity MCP/editor tools are available.

143. P0 Batch 32 Nephthys strict turnaround derivatives:
   - confirm Batch 32 outputs remain under
     `design/development/asset_candidates/starter_cats`
   - confirm no Batch 32 candidate PNG has a Unity `.meta` file
   - confirm the Batch 32 manifest has 7 Nephthys rows:
     - `front_view_reference_512`
     - `side_view_reference_512`
     - `back_view_reference_512`
     - `combat_sprite_reference_512`
     - `hud_avatar_reference_256`
     - `skill_icon_motif_reference_128`
     - `palette_swatch_reference_256`
   - confirm the review sheet is `1600x900` and shows source, front, side,
     back, combat, HUD, pyramid/moon motif, and palette panels without overlap
   - confirm the review note states formal import remains blocked and candidate
     files stay outside `Assets`
   - confirm `P0StarterCatProductionPromptReadiness` now expects 8 prompts and
     includes Batch 32
   - capture `05-active-cat-nephthys.png` in Play Mode before any Unity install
   - compare the active screenshot against the colored three-view turnaround
     and Batch 32 review sheet
   - reject if the runtime presentation drifts into Cleopatra costume cliche,
     human robe posture, generic Egyptian fantasy, or loses hood, script trim,
     pyramid/obelisk prop, ankh, blue gem, crescent ornament, or striped tail

Updated offline validation: Batch 32 generated a Nephthys source-derived
candidate pack in Codex, outside Unity. The dedicated validator passed with
7 rows, 1 review note, and 1 review sheet. Unity import, Console checks,
AssetDatabase refresh, Sprite import settings, active-cat screenshot review,
and runtime binding checks remain pending until Unity MCP/editor tools are
available.

144. P0 Batch 33 Suzune strict turnaround derivatives:
   - confirm Batch 33 outputs remain under
     `design/development/asset_candidates/starter_cats`
   - confirm no Batch 33 candidate PNG has a Unity `.meta` file
   - confirm the Batch 33 manifest has 7 Suzune rows:
     - `front_view_reference_512`
     - `side_view_reference_512`
     - `back_view_reference_512`
     - `combat_sprite_reference_512`
     - `hud_avatar_reference_256`
     - `skill_icon_motif_reference_128`
     - `palette_swatch_reference_256`
   - confirm the review sheet is `1600x900` and shows source, front, side,
     back, combat, HUD, bell-wand motif, and palette panels without overlap
   - confirm the review note states formal import remains blocked and candidate
     files stay outside `Assets`
   - confirm `P0StarterCatProductionPromptReadiness` now expects 9 prompts and
     includes Batch 33
   - capture `06-active-cat-suzune.png` in Play Mode before any Unity install
   - compare the active screenshot against the colored three-view turnaround
     and Batch 33 review sheet
   - reject if the runtime presentation drifts into generic shrine-cat, human
     shrine maiden proportions, generic healer costume, or loses calico
     markings, blue eyes, white shrine robe, vermilion cloth, bell wand, blue
     talismans, snowflake sleeves, flower ornament, hanging bells, back bow, or
     calico tail

Updated offline validation: Batch 33 generated a Suzune source-derived
candidate pack in Codex, outside Unity. The dedicated validator passed with
7 rows, 1 review note, and 1 review sheet. Unity import, Console checks,
AssetDatabase refresh, Sprite import settings, active-cat screenshot review,
and runtime binding checks remain pending until Unity MCP/editor tools are
available.

145. P0 Batch 34 Suzune AI refinement candidate:
   - confirm Batch 34 outputs remain under
     `design/development/asset_candidates/starter_cats`
   - confirm no Batch 34 candidate PNG has a Unity `.meta` file
   - confirm the Batch 34 manifest has 3 Suzune rows:
     - `ai_refinement_raw_codex`
     - `ai_refinement_combat_1024`
     - `ai_refinement_combat_512_preview`
   - confirm the review sheet is `1600x900` and compares source turnaround,
     Batch 33 front reference, AI candidate, and 512 preview without overlap
   - confirm the review note states formal import remains blocked and candidate
     files stay outside `Assets`
   - confirm `P0StarterCatProductionPromptReadiness` now expects 10 prompts and
     includes Batch 34
   - before any Unity install, produce transparent/cutout evidence for the
     candidate and inspect edge quality against dark and warm HUD fields
   - capture `06-active-cat-suzune.png` in Play Mode before any Unity install
   - compare the active screenshot against the colored three-view turnaround,
     Batch 33 review sheet, and Batch 34 AI candidate
   - reject if the runtime presentation loses calico markings, blue eyes,
     white shrine robe, vermilion cloth, gold bell, flower ornament, hanging
     bells, bell wand, blue talismans, snowflake sleeve marks, compact
     non-human cat posture, or calico tail

Updated offline validation: Batch 34 generated a Suzune Codex built-in
image-generation refinement candidate outside Unity. The dedicated validator
passed with 3 rows, 1 review note, 1 review sheet, and 1 prompt record. Unity
import, transparent/cutout treatment, Console checks, AssetDatabase refresh,
Sprite import settings, active-cat screenshot review, and runtime binding
checks remain pending until Unity MCP/editor tools are available.

146. P0 Batch 35 Suzune transparent cutout candidate:
   - confirm Batch 35 outputs remain under
     `design/development/asset_candidates/starter_cats`
   - confirm no Batch 35 candidate PNG has a Unity `.meta` file
   - confirm the Batch 35 manifest has 4 Suzune rows:
     - `cutout_alpha_1024`
     - `cutout_alpha_512_preview`
     - `cutout_checkerboard_512_review`
     - `cutout_alpha_mask_512_review`
   - confirm the review sheet is `1600x900` and compares source turnaround,
     Batch 34 input candidate, checkerboard cutout, dark-field cutout, alpha
     mask, and 512 preview without overlap
   - confirm the alpha PNG has transparent corners and an opaque visible Suzune
     center
   - confirm the review note states formal import remains blocked and candidate
     files stay outside `Assets`
   - confirm `P0StarterCatProductionPromptReadiness` now expects 11 prompts and
     includes Batch 35
   - if this candidate is later installed, verify Unity Sprite import settings
     for alpha, pivot, pixels-per-unit, filter mode, and compression
   - capture `06-active-cat-suzune.png` in Play Mode before any Unity install
   - compare the active screenshot against the colored three-view turnaround,
     Batch 33 review sheet, Batch 34 AI candidate, and Batch 35 cutout sheet
   - inspect edge quality against dark and warm HUD fields for parchment halos
   - reject if the cutout clips ears, calico markings, flower ornament, hanging
     bells, bell wand, blue talismans, snowflake sleeves, robe ribbons, paws,
     or calico tail

Updated offline validation: Batch 35 generated a deterministic transparent
cutout candidate from the Batch 34 Suzune image outside Unity. The dedicated
validator passed with 4 rows, 1 review note, 1 review sheet, 1 process note,
and 1 agent prompt. Unity import, Console checks, AssetDatabase refresh, Sprite
import settings, active-cat screenshot review, and edge-halo QA remain pending
until Unity MCP/editor tools are available.

147. P0 Batch 36 Nephthys AI refinement candidate:
   - confirm Batch 36 outputs remain under
     `design/development/asset_candidates/starter_cats`
   - confirm no Batch 36 candidate PNG has a Unity `.meta` file
   - confirm the Batch 36 manifest has 3 Nephthys rows:
     - `ai_refinement_raw_codex`
     - `ai_refinement_combat_1024`
     - `ai_refinement_combat_512_preview`
   - confirm the review sheet is `1600x900` and compares source turnaround,
     Batch 32 front reference, AI candidate, and 512 preview without overlap
   - confirm the review note states formal import remains blocked and candidate
     files stay outside `Assets`
   - confirm `P0StarterCatProductionPromptReadiness` now expects 12 prompts and
     includes Batch 36
   - before any Unity install, produce transparent/cutout evidence for the
     candidate and inspect edge quality against dark and warm HUD fields
   - capture `05-active-cat-nephthys.png` in Play Mode before any Unity install
   - compare the active screenshot against the colored three-view turnaround,
     Batch 32 review sheet, and Batch 36 AI candidate
   - reject if the runtime presentation loses gold-brown tabby markings,
     golden eyes, deep navy hood/cloak, crescent ornament, blue tear gem,
     sand-gold script trim, blue gemstone chest ornament, winged gold collar,
     ankh emblem, floating pyramid/obelisk prop, compact non-human cat posture,
     or striped tail

Updated offline validation: Batch 36 generated a Nephthys Codex built-in
image-generation refinement candidate outside Unity. The dedicated validator
passed with 3 rows, 1 review note, 1 review sheet, and 1 prompt record. Unity
import, transparent/cutout treatment, Console checks, AssetDatabase refresh,
Sprite import settings, active-cat screenshot review, and runtime binding
checks remain pending until Unity MCP/editor tools are available.

148. P0 Batch 37 Nephthys transparent cutout candidate:
   - confirm Batch 37 outputs remain under
     `design/development/asset_candidates/starter_cats`
   - confirm no Batch 37 candidate PNG has a Unity `.meta` file
   - confirm the Batch 37 manifest has 4 Nephthys rows:
     - `cutout_alpha_1024`
     - `cutout_alpha_512_preview`
     - `cutout_checkerboard_512_review`
     - `cutout_alpha_mask_512_review`
   - confirm the review sheet is `1600x900` and compares source turnaround,
     Batch 36 input candidate, checkerboard cutout, dark-field cutout, alpha
     mask, and 512 preview without overlap
   - confirm the alpha PNG has transparent corners and an opaque visible
     Nephthys center
   - confirm the review note states formal import remains blocked and candidate
     files stay outside `Assets`
   - confirm `P0StarterCatProductionPromptReadiness` now expects 13 prompts and
     includes Batch 37
   - if this candidate is later installed, verify Unity Sprite import settings
     for alpha, pivot, pixels-per-unit, filter mode, and compression
   - capture `05-active-cat-nephthys.png` in Play Mode before any Unity install
   - compare the active screenshot against the colored three-view turnaround,
     Batch 32 review sheet, Batch 36 AI candidate, and Batch 37 cutout sheet
   - inspect edge quality against dark and warm HUD fields for parchment halos
   - reject if the cutout clips hood, ears, crescent ornament, blue tear gem,
     pyramid/obelisk particles, ankh emblem, winged collar, cloak tips, paws,
     or striped tail

Updated offline validation: Batch 37 generated a deterministic transparent
cutout candidate from the Batch 36 Nephthys image outside Unity. The dedicated
validator passed with 4 rows, 1 review note, 1 review sheet, 1 process note,
and 1 agent prompt. Unity import, Console checks, AssetDatabase refresh, Sprite
import settings, active-cat screenshot review, and edge-halo QA remain pending
until Unity MCP/editor tools are available.

149. P0 Batch 38 core enemy source reference pack:
   - confirm Batch 38 outputs remain under
     `design/development/asset_candidates/enemies`
   - confirm no Batch 38 candidate PNG has a Unity `.meta` file
   - confirm the Batch 38 manifest has 15 rows:
     - 3 P0 core enemies
     - 5 asset types per enemy: concept reference, animation reference, combat
       crop, warning motif, and palette swatch
   - confirm the review sheet is `2400x1350` and compares Black Mud Nightmare,
     Cold Light Shadow, and Call Tyrant source concept, animation, combat crop,
     warning motif, palette, required reads, rejection rules, and Unity gate
     notes without overlap
   - confirm the review note states formal import remains blocked and candidate
     files stay outside `Assets`
   - capture active-enemy screenshots before any Unity install:
     - `07-active-enemy-black-mud.png`
     - `08-active-enemy-cold-light.png`
     - `09-active-enemy-call-tyrant.png`
   - compare runtime Black Mud against source concept/animation and reject cute
     pet styling, generic ghost shape, or loss of red eye threat read
   - compare runtime Cold Light against source concept/animation and reject
     ordinary desk lamp, warm fire palette, or missing ranged beam cue
   - compare runtime Call Tyrant against source concept/animation and reject
     human office boss body, generic phone app icon, or missing purple tie/red
     call-eye signal
   - verify Unity Console, AssetDatabase refresh, Sprite import settings,
     prefab references, scene bindings, runtime scale, and hitbox readability
     before any formal enemy import approval

Updated offline validation: Batch 38 generated a deterministic source-reference
pack for the P0 core enemies outside Unity. The dedicated validator passed with
15 rows, 3 enemies, 1 review note, and 1 review sheet. Unity import, Console
checks, AssetDatabase refresh, Sprite import settings, active-enemy screenshot
review, prefab/scene binding checks, runtime scale, and hitbox readability QA
remain pending until Unity MCP/editor tools are available.

150. P0 Batch 39 Black Mud AI refinement candidate:
   - confirm Batch 39 outputs remain under
     `design/development/asset_candidates/enemies`
   - confirm no Batch 39 candidate PNG has a Unity `.meta` file
   - confirm the Batch 39 manifest has 3 Black Mud rows:
     - `ai_refinement_raw_codex`
     - `ai_refinement_combat_1024`
     - `ai_refinement_combat_512_preview`
   - confirm the review sheet is `1600x900` and compares source concept,
     Batch 38 combat reference, AI candidate, and 512 preview without overlap
   - confirm the review note states formal import remains blocked and candidate
     files stay outside `Assets`
   - before any Unity install, produce transparent/cutout evidence for the
     candidate and inspect edge quality against dark and warm HUD fields
   - capture `07-active-enemy-black-mud.png` in Play Mode before any Unity
     install
   - compare the active screenshot against the source concept, source
     animation, Batch 38 reference sheet, and Batch 39 AI candidate
   - reject if the runtime presentation loses black sludge body, red eyes,
     soft-mud monster silhouette, crawling pressure, bed-contact threat, glossy
     pooled mud, sleepy face imprint, top drip, or low squat shape
   - reject cute pet styling, generic ghost shape, humanoid body, gore,
     realistic horror anatomy, extra dream interruption objects, cat features,
     or palette drift
   - verify Unity Console, AssetDatabase refresh, Sprite import settings,
     prefab references, scene bindings, runtime scale, and hitbox readability
     before any formal enemy import approval

Updated offline validation: Batch 39 generated a Black Mud Nightmare Codex
built-in image-generation refinement candidate outside Unity. The dedicated
validator passed with 3 rows, 1 review note, 1 review sheet, and 1 prompt
record. Unity import, transparent/cutout treatment, Console checks,
AssetDatabase refresh, Sprite import settings, active-enemy screenshot review,
runtime scale, hitbox readability, and prefab/scene binding checks remain
pending until Unity MCP/editor tools are available.

151. P0 Batch 40 Black Mud transparent cutout candidate:
   - confirm Batch 40 outputs remain under
     `design/development/asset_candidates/enemies`
   - confirm no Batch 40 candidate PNG has a Unity `.meta` file
   - confirm the Batch 40 manifest has 5 Black Mud rows:
     - `cutout_alpha_1024`
     - `cutout_alpha_512_preview`
     - `cutout_checkerboard_512_review`
     - `cutout_darkfield_512_review`
     - `cutout_alpha_mask_512_review`
   - confirm the review sheet is `1600x900` and compares source concept,
     Batch 39 source candidate, checkerboard cutout, dark-field cutout, alpha
     mask, and 512 preview without overlap
   - confirm the alpha PNG has transparent corners and an opaque visible Black
     Mud center
   - confirm the review note states formal import remains blocked and candidate
     files stay outside `Assets`
   - if this candidate is later installed, verify Unity Sprite import settings
     for alpha, pivot, pixels-per-unit, filter mode, and compression
   - capture `07-active-enemy-black-mud.png` in Play Mode before any Unity
     install
   - compare the active screenshot against the source concept, source
     animation, Batch 38 reference sheet, Batch 39 AI candidate, and Batch 40
     cutout sheet
   - inspect edge quality against dark and warm HUD fields for parchment halos
   - verify dark-field contrast, runtime scale, hitbox readability, warning
     readability, and bed-contact threat read
   - reject if the cutout clips black sludge body, red eyes, eye glow,
     soft-mud monster silhouette, crawling pressure, bed-contact threat,
     glossy pooled mud, sleepy face imprint, top drip, low squat shape, or
     puddled crawl edges
   - reject cute pet styling, generic ghost shape, humanoid body, gore,
     realistic horror anatomy, extra dream interruption objects, cat features,
     or palette drift

Updated offline validation: Batch 40 generated a deterministic transparent
cutout candidate from the Batch 39 Black Mud image outside Unity. The dedicated
validator passed with 5 rows, 1 review note, 1 review sheet, 1 process note,
and 1 agent prompt. Unity import, Console checks, AssetDatabase refresh, Sprite
import settings, active-enemy screenshot review, edge-halo QA, dark-field
contrast, runtime scale, hitbox readability, and prefab/scene binding checks
remain pending until Unity MCP/editor tools are available.

152. P0 Batch 41 Cold Light AI refinement candidate:
   - confirm Batch 41 outputs remain under
     `design/development/asset_candidates/enemies`
   - confirm no Batch 41 candidate PNG has a Unity `.meta` file
   - confirm the Batch 41 manifest has 3 Cold Light rows:
     - `ai_refinement_raw_codex`
     - `ai_refinement_combat_1024`
     - `ai_refinement_combat_512_preview`
   - confirm the review sheet is `1600x900` and compares source concept,
     Batch 38 combat reference, AI candidate, and 512 preview without overlap
   - confirm the review note states formal import remains blocked and candidate
     files stay outside `Assets`
   - before any Unity install, produce transparent/cutout evidence for the
     candidate and inspect edge quality against dark and warm HUD fields
   - capture `08-active-enemy-cold-light.png` in Play Mode before any Unity
     install
   - compare the active screenshot against the source concept, source
     animation, Batch 38 reference sheet, and Batch 41 AI candidate
   - reject if the runtime presentation loses cold lamp silhouette, cyan
     beam/light language, mechanical arm, black mud base, single red eye, long
     shadow-limb feel, or ranged-pressure read
   - reject ordinary clean desk lamp, warm candle or fire palette, cute robot
     styling, humanoid body, black mud removal, missing red eye, missing cyan
     light, missing spring arm, or missing mud base
   - verify Unity Console, AssetDatabase refresh, Sprite import settings,
     prefab references, scene bindings, runtime scale, beam-warning readability,
     hitbox readability, and ranged-pressure read before any formal enemy
     import approval

Updated offline validation: Batch 41 generated a Cold Light Shadow Codex
built-in image-generation refinement candidate outside Unity. The dedicated
validator passed with 3 rows, 1 review note, 1 review sheet, and 1 prompt
record. Unity import, transparent/cutout treatment, Console checks,
AssetDatabase refresh, Sprite import settings, active-enemy screenshot review,
beam-warning readability, runtime scale, hitbox readability, and prefab/scene
binding checks remain pending until Unity MCP/editor tools are available.

153. P0 Batch 42 Cold Light beam-preserving transparent cutout candidate:
   - confirm Batch 42 outputs remain under
     `design/development/asset_candidates/enemies`
   - confirm no Batch 42 candidate PNG has a Unity `.meta` file
   - confirm the Batch 42 manifest has 6 Cold Light rows:
     - `cutout_beam_alpha_1024`
     - `cutout_beam_alpha_512_preview`
     - `cutout_beam_checkerboard_512_review`
     - `cutout_beam_darkfield_512_review`
     - `cutout_beam_warmfield_512_review`
     - `cutout_beam_alpha_mask_512_review`
   - confirm the review sheet is `1600x900` and compares source concept,
     Batch 41 input candidate, checkerboard cutout, dark-field cutout,
     warm-HUD cutout, alpha mask, and 512 preview without overlap
   - confirm the alpha PNG has transparent corners, an opaque lamp/body
     silhouette, and semi-transparent cyan beam/glow pixels
   - confirm the review note states formal import remains blocked and candidate
     files stay outside `Assets`
   - if this candidate is later installed, verify Unity Sprite import settings
     for alpha, pivot, pixels-per-unit, filter mode, and compression
   - capture `08-active-enemy-cold-light.png` in Play Mode before any Unity
     install
   - compare the active screenshot against the source concept, source
     animation, Batch 38 reference sheet, Batch 41 AI candidate, and Batch 42
     cutout sheet
   - inspect edge quality against dark and warm HUD fields for parchment halos
   - verify dark-field contrast, warm-HUD contrast, runtime scale, hitbox
     readability, beam-warning readability, ranged-pressure read, and whether
     the beam should split into a separate warning VFX
   - reject if the cutout clips cold lamp silhouette, cyan beam/light language,
     mechanical spring arm, black mud base, single red eye, long shadow-limb
     feel, or ranged-pressure read
   - reject ordinary clean desk lamp, warm candle or fire palette, cute robot
     styling, humanoid body, black mud removal, missing red eye, missing cyan
     light, missing spring arm, missing mud base, or removal of all beam
     readability

Updated offline validation: Batch 42 generated a deterministic
beam-preserving transparent cutout candidate from the Batch 41 Cold Light image
outside Unity. The dedicated validator passed with 6 rows, 1 review note, 1
review sheet, 1 process note, and 1 agent prompt. Unity import, Console checks,
AssetDatabase refresh, Sprite import settings, active-enemy screenshot review,
edge-halo QA, dark-field/warm-field contrast, runtime scale, hitbox
readability, beam-warning readability, VFX separation, and prefab/scene binding
checks remain pending until Unity MCP/editor tools are available.

154. P0 Batch 43 Call Tyrant AI refinement candidate:
   - confirm Batch 43 outputs remain under
     `design/development/asset_candidates/enemies`
   - confirm no Batch 43 candidate PNG has a Unity `.meta` file
   - confirm the Batch 43 manifest has 3 Call Tyrant rows:
     - `ai_refinement_raw_codex`
     - `ai_refinement_combat_1024`
     - `ai_refinement_combat_512_preview`
   - confirm the review sheet is `1600x900` and compares source concept,
     Batch 38 combat reference, AI candidate, and 512 preview without overlap
   - confirm the review note states formal import remains blocked and candidate
     files stay outside `Assets`
   - before any Unity install, produce transparent/cutout evidence for the
     candidate and inspect edge quality against dark and warm HUD fields
   - capture `09-active-enemy-call-tyrant.png` in Play Mode before any Unity
     install
   - compare the active screenshot against the source concept, source
     animation, Batch 38 reference sheet, and Batch 43 AI candidate
   - reject if the runtime presentation loses giant phone shell, red call-eye
     signal, purple tie, black mud body and base, app projectile language,
     summon portal/minion vibration feel, Boss-scale silhouette, cracked glass
     screen, or phone-call nightmare identity
   - reject human office boss body, generic smartphone icon mascot, cute robot
     styling, clean ordinary phone, brand logos, readable text, keyboard,
     laptop, alarm/lamp/toy motifs, black mud removal, missing purple tie,
     missing red call eyes, missing cracked phone shell, or missing app
     projectile language
   - verify Unity Console, AssetDatabase refresh, Sprite import settings,
     prefab references, scene bindings, runtime scale, summon readability,
     app-throw readability, hitbox readability, and Boss pressure read before
     any formal enemy import approval

Updated offline validation: Batch 43 generated a Call Tyrant Boss Codex
built-in image-generation refinement candidate outside Unity. The dedicated
validator passed with 3 rows, 1 review note, 1 review sheet, and 1 prompt
record. Unity import, transparent/cutout treatment, Console checks,
AssetDatabase refresh, Sprite import settings, active-enemy screenshot review,
summon readability, app-throw readability, runtime scale, hitbox readability,
and prefab/scene binding checks remain pending until Unity MCP/editor tools are
available.

155. P0 Batch 44 Call Tyrant transparent cutout candidate:
   - confirm Batch 44 outputs remain under
     `design/development/asset_candidates/enemies`
   - confirm no Batch 44 candidate PNG has a Unity `.meta` file
   - confirm the Batch 44 manifest has 6 Call Tyrant rows:
     - `cutout_boss_alpha_1024`
     - `cutout_boss_alpha_512_preview`
     - `cutout_boss_checkerboard_512_review`
     - `cutout_boss_darkfield_512_review`
     - `cutout_boss_warmfield_512_review`
     - `cutout_boss_alpha_mask_512_review`
   - confirm the review sheet is `1600x900` and compares source concept,
     Batch 43 input candidate, checkerboard cutout, dark-field cutout,
     warm-HUD cutout, alpha mask, and 512 preview without overlap
   - confirm the alpha PNG has transparent corners, an opaque Boss silhouette,
     red call-eye pixels, purple tie pixels, and saturated app projectile
     pixels
   - confirm the review note states formal import remains blocked and candidate
     files stay outside `Assets`
   - if this candidate is later installed, verify Unity Sprite import settings
     for alpha, pivot, pixels-per-unit, filter mode, and compression
   - capture `09-active-enemy-call-tyrant.png` in Play Mode before any Unity
     install
   - compare the active screenshot against the source concept, source
     animation, Batch 38 reference sheet, Batch 43 AI candidate, and Batch 44
     cutout sheet
   - inspect edge quality against dark and warm HUD fields for parchment halos
     and small border specks
   - verify dark-field contrast, warm-HUD contrast, runtime scale, hitbox
     readability, summon readability, app-throw readability, Boss pressure
     read, and whether the app projectile cluster should split into separate
     warning VFX
   - reject if the cutout clips giant phone shell, red call-eye signal, purple
     tie, black mud body and base, app projectile language, summon
     portal/minion vibration feel, Boss-scale silhouette, cracked glass screen,
     or phone-call nightmare identity
   - reject human office boss body, generic smartphone icon mascot, cute robot
     styling, clean ordinary phone, brand logos, readable text, keyboard,
     laptop, alarm/lamp/toy motifs, black mud removal, missing purple tie,
     missing red call eyes, missing cracked phone shell, or missing app
     projectile language

Updated offline validation: Batch 44 generated a deterministic transparent
cutout candidate from the Batch 43 Call Tyrant image outside Unity. The
dedicated validator passed with 6 rows, 1 review note, 1 review sheet, 1
process note, and 1 agent prompt. Unity import, Console checks, AssetDatabase
refresh, Sprite import settings, active-enemy screenshot review, edge-halo QA,
dark-field/warm-field contrast, runtime scale, hitbox readability, summon
readability, app-throw readability, VFX separation, and prefab/scene binding
checks remain pending until Unity MCP/editor tools are available.

156. P0 Batch 45 starter-cat source-lock audit pack:
   - confirm Batch 45 outputs remain under
     `design/development/asset_candidates/starter_cats`
   - confirm no Batch 45 candidate PNG has a Unity `.meta` file
   - confirm the Batch 45 manifest has 3 starter-cat rows:
     - `saiban`
     - `nephthys`
     - `suzune`
   - confirm each row has asset type
     `source_lock_lineage_card_1000x640`
   - confirm each row uses recommendation
     `source_lock_audit_only_do_not_import`
   - confirm each source turnaround path is the real UTF-8 design path and not
     a mojibake path containing `?assets`
   - confirm each row records source turnaround hash, current Unity sprite
     hash, latest cutout preview hash, latest cutout manifest, source-lock id,
     and active-cat screenshot file
   - confirm the review sheet is `3200x900` and compares locked colored
     turnaround, current Unity combat sprite, and latest transparent cutout
     candidate for all three starter cats
   - confirm the review note states this is source-lock audit only and formal
     import remains blocked
   - capture `04-active-cat-saiban.png`, `05-active-cat-nephthys.png`, and
     `06-active-cat-suzune.png` in Play Mode before any starter-cat replacement
   - compare active-cat screenshots against the Batch 45 sheet for runtime
     scale, silhouette, palette, HUD readability, and source-lock identity
   - reject any future starter-cat asset that matches project style but drifts
     from the colored three-view turnaround
   - reject human body proportions, human costume pose, generic cute mascot
     replacement, missing side/back anchors, missing required props/costume
     pieces, or palette drift from the locked colored turnaround

Updated offline validation: Batch 45 generated a deterministic source-lock
audit pack outside Unity. The dedicated validator passed with 3 rows, 1 review
sheet, 1 review note, 1 process note, and 1 agent prompt. Unity import, Console
checks, AssetDatabase refresh, Sprite import settings, active-cat screenshot
review, runtime scale, HUD readability, source-lock identity comparison, and
prefab/scene binding checks remain pending until Unity MCP/editor tools are
available.

157. P0 Batch 46 asset production dashboard:
   - confirm Batch 46 outputs remain under
     `design/development/asset_candidates/p0_asset_dashboard`
   - confirm no Batch 46 candidate PNG has a Unity `.meta` file
   - confirm the Batch 46 manifest has exactly 6 rows:
     - `cat:saiban`
     - `cat:nephthys`
     - `cat:suzune`
     - `enemy:black_mud_nightmare`
     - `enemy:cold_light_shadow`
     - `enemy:call_tyrant`
   - confirm every row uses recommendation
     `dashboard_only_unity_validation_pending`
   - confirm each row records source lock ids, source reference hashes,
     current Unity runtime asset hash, latest candidate preview hash, latest
     candidate manifest, install target, active screenshot, Unity validation
     gate, next action, and blockers
   - confirm no new Batch 46 source path contains mojibake text or `?assets`
   - confirm the review sheet is `3200x1800` and compares source lock, current
     Unity runtime asset, and latest candidate preview for each P0 subject
   - capture the 6 active subject screenshots before any dashboard row is
     promoted into a Unity install batch:
     - `04-active-cat-saiban.png`
     - `05-active-cat-nephthys.png`
     - `06-active-cat-suzune.png`
     - `07-active-enemy-black-mud.png`
     - `08-active-enemy-cold-light.png`
     - `09-active-enemy-call-tyrant.png`
   - compare starter-cat screenshots against the colored three-view
     turnarounds and Batch 45 source-lock audit sheet
   - compare enemy/Boss screenshots against the concept sources, animation
     sources, and Batch 40/42/44 cutout sheets
   - verify Unity Console, AssetDatabase refresh, Sprite import settings,
     prefab/scene binding, runtime scale, HUD readability, dark/warm field
     readability, hitbox readability, skill/VFX overlap, and bed-pressure
     readability
   - make an explicit Call Tyrant decision before import: keep the current
     concept proxy, create a formal boss combat sprite binding, or split app
     projectile/summon language into the existing VFX lanes
   - reject any dashboard promotion that skips source-lock identity,
     screenshot comparison, manifest/runtime record updates, or Unity editor
     validation

Updated offline validation: Batch 46 generated a deterministic P0 asset
production dashboard outside Unity. The dedicated validator passed with 6 rows,
1 review sheet, 1 review note, 1 process note, and 1 agent prompt. Unity
install, Console checks, AssetDatabase refresh, Sprite import settings,
active-cat screenshots, active-enemy screenshots, runtime readability, and
Call Tyrant boss combat binding decisions remain pending until Unity MCP/editor
tools are available.

158. P0 Batch 47 starter-cat strict generation spec pack:
   - confirm Batch 47 outputs remain under
     `design/development/asset_candidates/starter_cats`
   - confirm no Batch 47 candidate PNG has a Unity `.meta` file
   - confirm the Batch 47 manifest has exactly 3 rows:
     - `saiban`
     - `nephthys`
     - `suzune`
   - confirm every row uses recommendation
     `strict_generation_spec_only_do_not_import`
   - confirm every row records source lock id, source turnaround hash, current
     Unity sprite hash, latest cutout preview hash, prompt file hash, JSON spec
     hash, palette guard, visible source bounding box, and active-cat screenshot
   - confirm every JSON spec includes non-human cat body rule, composition
     rule, must-keep anchors, reject rules, positive prompt, negative prompt,
     and Unity validation requirements
   - confirm every prompt file includes positive prompt, negative prompt, hard
     source lock, must-keep list, reject list, palette guard, and active-cat
     screenshot gate
   - confirm the review sheet is `3600x1120` and compares source turnaround,
     current Unity sprite, latest candidate, palette guard, must-keep anchors,
     and immediate reject rules for all three starter cats
   - reject any future image-generation batch that does not cite the Batch 47
     JSON spec and the colored three-view turnaround as required inputs
   - reject any future generated cat with human body proportions, generic mascot
     drift, missing required props, missing side/back identity anchors, or
     palette drift from the Batch 47 palette guard
   - keep Unity import blocked until generated candidates return through cutout
     preparation, manifest update, review sheet, Console checks, AssetDatabase
     refresh, Sprite import settings, active-cat screenshot comparison, HUD
     readability, runtime scale, and prefab/scene binding checks

Updated offline validation: Batch 47 generated deterministic strict generation
specs outside Unity. The dedicated validator passed with 3 rows, 1 review
sheet, 1 review note, 1 process note, and 1 agent prompt. No model art was
generated, no runtime asset was installed, and formal starter-cat import remains
blocked pending future generated candidate review and active-cat Play Mode
screenshot review.

159. P0 Batch 48 Saiban AI generation pilot:
   - confirm Batch 48 outputs remain under
     `design/development/asset_candidates/starter_cats/saiban/batch_48_saiban_ai_generation_pilot_2026-06-15`
     and
     `design/development/asset_candidates/starter_cats/batch_48_saiban_ai_generation_pilot_2026-06-15`
   - confirm no Batch 48 candidate PNG has a Unity `.meta` file
   - confirm the Batch 48 manifest has 7 rows:
     - `chromakey_source_1024`
     - `alpha_candidate_1024`
     - `alpha_preview_512`
     - `checkerboard_review_512`
     - `darkfield_review_512`
     - `warmfield_review_512`
     - `alpha_mask_review_512`
   - confirm every row uses recommendation
     `candidate_review_only_do_not_import`
   - confirm every row records Batch 47 JSON/prompt/card hashes, source
     turnaround hash, current Unity sprite hash, generated source hash, alpha
     candidate hash, active-cat screenshot, review sheet, review note, process
     note, and agent prompt
   - confirm the alpha candidate is `1024x1024`, has transparent corners, and
     no obvious chroma-key field remains
   - compare the pilot against the locked colored turnaround and Batch 47 spec
     card:
     - preserve non-human cat body
     - preserve shield
     - preserve sword
     - preserve red cape
     - preserve silver armor
     - preserve blue gem
     - preserve tabby face and striped tail
   - review known watch items:
     - helmet and armor are more ornate than the locked source
     - single front combat pose cannot prove side/back identity anchors
   - capture `04-active-cat-saiban.png` in Play Mode before any formal import
   - verify Unity Console, AssetDatabase refresh, Sprite import settings,
     runtime scale, HUD readability, active-cat switch readability,
     skill-button/avatar readability, and prefab/scene binding
   - reject import if the ornate helmet/armor breaks source-lock identity,
     if the shield/sword/cape/tail read poorly at game scale, if alpha edges
     show green fringe, or if the active-cat screenshot drifts from the colored
     turnaround

Updated offline validation: Batch 48 generated the first Saiban-only Codex
built-in image-generation pilot and converted it into a transparent candidate
outside Unity. The dedicated validator passed with 7 rows, 1 review sheet, 1
review note, 1 process note, and 1 agent prompt. No runtime asset was installed
and formal starter-cat import remains blocked pending active-cat Play Mode
screenshot review and Unity editor validation.

160. P0 Batch 49 Saiban low-drift refinement:
   - confirm Batch 49 outputs remain under
     `design/development/asset_candidates/starter_cats/saiban/batch_49_saiban_low_drift_refinement_2026-06-15`
     and
     `design/development/asset_candidates/starter_cats/batch_49_saiban_low_drift_refinement_2026-06-15`
   - confirm no Batch 49 candidate PNG has a Unity `.meta` file
   - confirm the Batch 49 manifest has 7 rows:
     - `chromakey_source_1024`
     - `alpha_candidate_1024`
     - `alpha_preview_512`
     - `checkerboard_review_512`
     - `darkfield_review_512`
     - `warmfield_review_512`
     - `alpha_mask_review_512`
   - confirm every row uses recommendation
     `candidate_review_only_do_not_import`
   - confirm every row records Batch 47 JSON/prompt/card hashes, Batch 48
     pilot comparison hashes, source turnaround hash, current Unity sprite
     hash, generated source hash, alpha candidate hash, active-cat screenshot,
     review sheet, review note, process note, and agent prompt
   - confirm the alpha candidate is `1024x1024`, has transparent corners, and
     no obvious chroma-key field remains
   - compare Batch 49 against the locked colored turnaround, Batch 47 spec card,
     and Batch 48 pilot:
     - preserve non-human cat body
     - preserve exposed ears
     - preserve lower-profile helmet
     - preserve shield
     - preserve sword
     - preserve red cape
     - preserve silver armor and blue gem
     - preserve tabby face and striped tail
     - verify armor ornamentation is reduced versus Batch 48
   - review known watch items:
     - armor is still slightly more polished than the locked source
     - single front combat pose cannot prove side/back identity anchors
   - capture `04-active-cat-saiban.png` in Play Mode before any formal import
   - verify Unity Console, AssetDatabase refresh, Sprite import settings,
     runtime scale, HUD readability, active-cat switch readability,
     skill-button/avatar readability, and prefab/scene binding
   - reject import if the remaining armor polish breaks source-lock identity,
     if shield/sword/cape/tail read poorly at game scale, if alpha edges show
     green fringe, or if the active-cat screenshot drifts from the colored
     turnaround
   - if accepted, promote Batch 49 over Batch 48 as the preferred Saiban install
     candidate

Updated offline validation: Batch 49 generated a lower-drift Saiban Codex
built-in image-generation refinement and converted it into a transparent
candidate outside Unity. The dedicated validator passed with 7 rows, 1 review
sheet, 1 review note, 1 process note, and 1 agent prompt. No runtime asset was
installed and formal starter-cat import remains blocked pending active-cat Play
Mode screenshot review and Unity editor validation.

161. P0 Batch 50 Nephthys strict AI generation candidate:
   - confirm Batch 50 outputs remain under
     `design/development/asset_candidates/starter_cats/nephthys/batch_50_nephthys_strict_ai_generation_candidate_2026-06-15`
     and
     `design/development/asset_candidates/starter_cats/batch_50_nephthys_strict_ai_generation_candidate_2026-06-15`
   - confirm no Batch 50 candidate PNG has a Unity `.meta` file
   - confirm the Batch 50 manifest has 7 rows:
     - `chromakey_source_1024`
     - `alpha_candidate_1024`
     - `alpha_preview_512`
     - `checkerboard_review_512`
     - `darkfield_review_512`
     - `warmfield_review_512`
     - `alpha_mask_review_512`
   - confirm every row uses recommendation
     `candidate_review_only_do_not_import`
   - confirm every row records Batch 47 JSON/card hashes, Batch 37 baseline
     hashes, source turnaround hash, current Unity sprite hash, generated
     source hash, alpha candidate hash, active-cat screenshot, review sheet,
     review note, process note, and agent prompt
   - confirm the alpha candidate is `1024x1024`, has transparent corners, and
     no obvious chroma-key field remains
   - compare Batch 50 against the locked colored turnaround, Batch 47 spec card,
     current Unity sprite, and Batch 37 cutout baseline:
     - preserve non-human cat body
     - preserve exposed ears inside the hood
     - preserve deep navy hood and cloak
     - preserve gold-brown tabby face markings
     - preserve crescent ornament and blue tear gem
     - preserve winged collar and ankh emblem
     - preserve floating pyramid or obelisk prop
     - preserve striped tail
     - verify palette stays close to navy, sand-gold, brown tabby, pale cloth,
       and blue gem guard
   - review known watch items:
     - Batch 50 is more close-up and hero-polished than Batch 37
     - single front combat pose cannot prove side/back identity anchors
   - capture `05-active-cat-nephthys.png` in Play Mode before any formal import
   - verify Unity Console, AssetDatabase refresh, Sprite import settings,
     runtime scale, HUD readability, active-cat switch readability,
     skill-button/avatar readability, and prefab/scene binding
   - reject import if the candidate drifts into human Cleopatra/priestess
     proportions, if the pyramid/hood/gems read poorly at game scale, if alpha
     edges show green fringe, or if the active-cat screenshot drifts from the
     colored turnaround
   - compare Batch 50 against Batch 37 in active-cat context before choosing a
     preferred Nephthys install candidate

Updated offline validation: Batch 50 generated a Nephthys Codex built-in image
generation candidate after the Batch 47 strict spec gate and converted it into
a transparent candidate outside Unity. The dedicated validator passed with 7
rows, 1 review sheet, 1 review note, 1 process note, and 1 agent prompt. No
runtime asset was installed and formal starter-cat import remains blocked
pending active-cat Play Mode screenshot review and Unity editor validation.

162. P0 Batch 51 Suzune strict AI generation candidate:
   - confirm Batch 51 outputs remain under
     `design/development/asset_candidates/starter_cats/suzune/batch_51_suzune_strict_ai_generation_candidate_2026-06-15`
     and
     `design/development/asset_candidates/starter_cats/batch_51_suzune_strict_ai_generation_candidate_2026-06-15`
   - confirm no Batch 51 candidate PNG has a Unity `.meta` file
   - confirm the Batch 51 manifest has 7 rows:
     - `chromakey_source_1024`
     - `alpha_candidate_1024`
     - `alpha_preview_512`
     - `checkerboard_review_512`
     - `darkfield_review_512`
     - `warmfield_review_512`
     - `alpha_mask_review_512`
   - confirm every row uses recommendation
     `candidate_review_only_do_not_import`
   - confirm every row records Batch 47 JSON/card hashes, Batch 35 baseline
     hashes, source turnaround hash, current Unity sprite hash, generated
     source hash, alpha candidate hash, active-cat screenshot, review sheet,
     review note, process note, and agent prompt
   - confirm the alpha candidate is `1024x1024`, has transparent corners, and
     no obvious chroma-key field remains
   - compare Batch 51 against the locked colored turnaround, Batch 47 spec card,
     current Unity sprite, and Batch 35 cutout baseline:
     - preserve non-human cat body
     - preserve blue eyes and triangular ears
     - preserve calico orange, black, and white markings
     - preserve warm white and vermilion shrine outfit
     - preserve moon-blue talismans and tear-drop charms
     - preserve bell ornaments and bell wand
     - preserve calico tail
     - verify palette stays close to warm white, vermilion, moon-blue, gold
       bell, and calico fur guard
   - review known watch items:
     - Batch 51 is more close-up and hero-polished than Batch 35
     - wand strings, bells, talismans, and droplets create more alpha edge
       complexity than the older cutout
     - single front combat pose cannot prove side/back identity anchors
   - capture `06-active-cat-suzune.png` in Play Mode before any formal import
   - verify Unity Console, AssetDatabase refresh, Sprite import settings,
     runtime scale, HUD readability, active-cat switch readability,
     skill-button/avatar readability, and prefab/scene binding
   - reject import if the candidate drifts into human shrine maiden
     proportions, if bell wand/talismans read poorly at game scale, if alpha
     edges show green fringe, or if the active-cat screenshot drifts from the
     colored turnaround
   - compare Batch 51 against Batch 35 in active-cat context before choosing a
     preferred Suzune install candidate

Updated offline validation: Batch 51 generated a Suzune Codex built-in image
generation candidate after the Batch 47 strict spec gate and converted it into
a transparent candidate outside Unity. The dedicated validator passed with 7
rows, 1 review sheet, 1 review note, 1 process note, and 1 agent prompt. No
runtime asset was installed and formal starter-cat import remains blocked
pending active-cat Play Mode screenshot review and Unity editor validation.

163. P0 starter-cat strict candidate evidence gate:
   - confirm `P0StarterCatStrictCandidateEvidence` reports 3/3 candidates:
     - Saiban Batch 49
     - Nephthys Batch 50
     - Suzune Batch 51
   - confirm every candidate has:
     - manifest
     - alpha candidate PNG
     - review sheet
     - review note
     - process note
     - agent prompt
     - baseline candidate reference
     - source-lock id
     - active-cat screenshot filename
   - confirm every strict candidate remains outside `Assets`
   - confirm no strict candidate PNG has a Unity `.meta` file
   - confirm every review note explicitly says candidate review only and do not
     import into Unity before active-cat screenshot review
   - confirm `P0AssetReviewPacket` includes the Starter Cat Strict Candidate
     Evidence markdown section
   - confirm `P0AssetProductionReadiness` fails if the strict candidate gate is
     missing or stale
   - capture `04-active-cat-saiban.png`, `05-active-cat-nephthys.png`, and
     `06-active-cat-suzune.png` in Play Mode before any formal install
   - compare the active-cat screenshots against the locked colored three-view
     turnaround sheets, Batch 47 spec cards, prior baselines, and the current
     Batch 49/50/51 review sheets

Updated offline validation: Added runtime and EditMode coverage for strict
starter-cat candidate evidence. Visual Studio MSBuild passed with 0 errors
after adding the new files to the generated csproj files. `dotnet build` remains
unavailable because this environment has no .NET SDK. Unity editor Console,
AssetDatabase refresh, active-cat screenshot capture, Sprite import settings,
runtime scale, HUD readability, and prefab/scene binding remain pending.

164. P0 systematic asset production queue:
   - confirm `P0AssetProductionQueueCatalog` exposes 5 queue entries
   - confirm exactly 2 entries are ready for Codex candidate production:
     - Batch 54 bedroom interactable candidates
     - Batch 55 starter skill VFX candidates
   - confirm exactly 3 entries are blocked by Unity validation:
     - Batch 52 starter-cat active screenshot validation
     - Batch 53 core-enemy active screenshot validation
     - Batch 56 formal Unity install decision packet
   - confirm every queue entry has an execution prompt under
     `design/development/agent_prompts`
   - confirm every candidate directory stays under
     `design/development/asset_candidates`
   - confirm every Unity import root stays under `Assets`
   - confirm forbidden write roots protect Unity import roots for every
     candidate-production entry
   - confirm `P0AssetReviewPacket` includes the Asset Production Queue
     markdown section
   - confirm `P0AssetProductionReadiness` fails if the queue is missing,
     incomplete, or stale
   - keep Batch 52 blocked until `04-active-cat-saiban.png`,
     `05-active-cat-nephthys.png`, and `06-active-cat-suzune.png` are captured
     in Play Mode and compared against the locked colored three-view
     turnarounds
   - keep Batch 53 blocked until core-enemy active screenshots, Console checks,
     and scene/prefab bindings are verified
   - keep Batch 56 blocked until the starter-cat and core-enemy active
     screenshot gates have pass/fail decisions

Updated offline validation: Added runtime queue definitions, queue catalog,
queue coverage, five agent prompts, review packet integration, production
readiness integration, and EditMode tests. Visual Studio MSBuild passed with 0
errors. Unity editor Console, AssetDatabase refresh, active screenshots, Sprite
import settings, runtime scale, HUD readability, and prefab/scene binding
remain pending.

165. P0 Batch 54 bedroom interactable candidates:
   - confirm Batch 54 outputs remain under
     `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15`
   - confirm no Batch 54 candidate PNG has a Unity `.meta` file
   - confirm the manifest has 21 rows:
     - 7 rows for `bed`
     - 7 rows for `litter_box`
     - 7 rows for `feeder`
   - confirm every row uses recommendation
     `candidate_review_only_do_not_import`
   - confirm alpha candidates are `1024x1024` and have transparent corners
   - compare candidates against the Bedroom Dream map concept and sprite sheets:
     - bed keeps navy star blanket, crescent, wooden frame, pillow, protected
       sleep identity, and source-room palette
     - litter box keeps blue plastic rounded box, tan clean litter, and paw
       emblem
     - feeder keeps pink-lavender body, transparent kibble tank, kibble bowl,
       paw emblem, and moon/star accents
   - review known watch items:
     - bed candidate includes a rug base and may require runtime scale
       reduction
     - litter box v001 was rejected for green edge glow; v002 is the selected
       candidate
     - feeder and litter box are more close-up and polished than current 256px
       runtime sprites
   - verify Unity Console, AssetDatabase refresh, Sprite import settings,
     battle-world runtime scale, HUD/world readability, interaction feedback,
     and prefab/scene binding before any formal install
   - reject install if the candidates obscure gameplay pathing, look too close
     up beside enemy/cat sprites, or drift from Bedroom Dream source palette

Updated offline validation: Batch 54 generated Codex-side review-only
candidates for the P0 bed, litter box, and feeder interactables. The dedicated
validator passed with 21 rows, 1 review sheet, 1 review note, 1 process note,
and 1 agent prompt. No runtime visual binding was changed and no Unity asset
was installed. Formal Unity import remains blocked pending runtime screenshot
review.

166. P0 Batch 55 starter skill VFX candidates:
   - confirm Batch 55 outputs remain under
     `design/development/asset_candidates/vfx/starter_skills/batch_55_starter_skill_vfx_candidates_2026-06-15`
   - confirm no Batch 55 candidate PNG has a Unity `.meta` file
   - confirm the manifest has 21 rows:
     - 7 rows for `saiban`
     - 7 rows for `nephthys`
     - 7 rows for `suzune`
   - confirm every row uses recommendation
     `candidate_review_only_do_not_import`
   - confirm alpha candidates are `1024x1024` and have transparent corners
   - compare VFX candidates against colored starter-cat turnarounds, current
     runtime VFX, and authority blessing icons:
     - Saiban keeps shield, sword, sun-gold oath, and bedline defense language
     - Nephthys keeps obelisk, moon-sand spiral, teal control rings, and royal
       eye mark language
     - Suzune keeps kagura bells, torii, moon-blue healing circle, talismans,
       and lullaby note language
   - review known watch items:
     - Saiban includes a central cat paw emblem that may need simplification
     - Suzune torii symbol may be too large if used directly in combat scale
     - all three candidates may need to be split into smaller per-skill VFX
       sprites rather than installed as full emblems
   - verify Unity Console, AssetDatabase refresh, Sprite import settings,
     runtime scale, HUD/world readability, skill timing, prefab/scene binding,
     and Play Mode screenshots before any formal VFX install

Updated offline validation: Batch 55 generated Codex-side review-only
candidates for the three starter skill VFX families. The dedicated validator
passed with 21 rows, 1 review sheet, 1 review note, 1 process note, and 1
agent prompt. No runtime visual binding was changed and no Unity asset was
installed. Formal Unity import remains blocked pending install decision,
runtime scale review, Console checks, and Play Mode screenshots.

167. P0 queue state and Batch 56 blocked formal install decision:
   - confirm `P0AssetProductionQueueCatalog` reports:
     - 0 remaining Codex-runnable candidate production entries
     - 2 completed candidate packs pending Unity review
     - 3 Unity-blocked validation/install entries
   - confirm Batch 54 and Batch 55 queue states are
     `CandidatePackCompletePendingUnityReview`
   - confirm Batch 56 decision packet exists under
     `design/development/asset_candidates/formal_install_decisions/batch_56_formal_install_decision_packet_2026-06-15`
   - confirm `formal_install_decision_batch56.csv` has 8 rows:
     - Saiban Batch 49
     - Nephthys Batch 50
     - Suzune Batch 51
     - Black Mud Batch 40
     - Cold Light Batch 42
     - Call Tyrant Batch 44
     - Bedroom interactables Batch 54
     - Starter skill VFX Batch 55
   - confirm every row is `blocked_pending_unity_evidence`
   - confirm every row has `install_allowed=false`
   - confirm no Batch 56 output is copied into `Assets`
   - confirm no Batch 56 output creates Unity `.meta` files
   - run Unity MCP when tools are available:
     - `Unity_GetConsoleLogs`
     - `Unity_RunCommand` read-only query
     - AssetDatabase refresh
     - active-cat screenshots
     - active-enemy screenshots
     - Battle scene prop scale screenshots
     - skill VFX timing screenshots
     - prefab/scene binding inspection

Updated offline validation: Queue state now reflects that Batch 54 and Batch 55
candidate production is complete and pending Unity review, not still ready for
Codex production. Batch 56 generated a blocked formal install decision packet
with 8 rows and no install approvals. Local Unity MCP setup exists, but Unity
MCP editor tools are not exposed in the current Codex tool surface, so runtime
Console, screenshot, AssetDatabase, and scene/prefab validation remain pending.

168. P0 architecture completion audit:
   - run `TheCat/P0/Run Architecture Completion Audit` in the Unity editor
   - confirm the audit logs no blocking failures
   - confirm it reports architecture ready for systematic Codex-side asset
     production
   - confirm it reports final P0 Unity runtime as still pending
   - confirm current queue state remains:
     - 0 Codex-runnable candidate packs
     - 3 completed candidate packs pending Unity review
     - 3 Unity-blocked validation/install items
   - confirm Play Mode screenshot evidence is regenerated against the current
     10-capture plan
   - confirm active Saiban, Nephthys, and Suzune screenshots match their locked
     colored three-view turnarounds before any starter-cat body install
   - confirm Batch 54 prop, Batch 55 VFX, and Batch 57 skill-HUD candidates
     remain outside `Assets` until a formal install decision row is approved
   - run Unity Console, AssetDatabase refresh, Sprite import, runtime scale,
     HUD readability, and scene/prefab binding checks before final P0 visual
     acceptance

Updated offline validation: Added the architecture completion audit tool,
editor menu, and EditMode tests. Visual Studio MSBuild passed with 0 errors.
Unity editor evidence remains pending because Unity MCP editor tools are not
exposed in the current Codex tool surface.

169. P0 Batch 57 skill HUD feedback candidates:
   - confirm Batch 57 outputs remain under
     `design/development/asset_candidates/ui/skill_hud/batch_57_skill_hud_feedback_candidates_2026-06-15`
   - confirm no Batch 57 candidate PNG has a Unity `.meta` file
   - confirm the manifest has 30 rows:
     - 5 rows for `skill_ready_frame`
     - 5 rows for `skill_cooldown_overlay`
     - 5 rows for `skill_no_target_marker`
     - 5 rows for `skill_hunger_cost_chip`
     - 5 rows for `auto_target_reticle`
     - 5 rows for `interaction_range_ripple`
   - confirm every row uses recommendation
     `candidate_review_only_do_not_import`
   - confirm all alpha candidates are `512x512` and have transparent corners
   - compare candidates against current UI shell, status, hunger, mark, and
     battle-feedback references
   - verify in Unity before formal install:
     - skill ready button readability
     - cooldown overlay readability and timing
     - no-target marker readability
     - hunger cost chip readability
     - auto-target reticle readability
     - interaction range ripple scale
     - Console state
     - Sprite import settings
     - scene/prefab binding
     - Play Mode screenshots

Updated offline validation: Batch 57 generated Codex-side review-only
candidates for P0 skill HUD feedback. The dedicated validator passed with 30
rows, 1 review sheet, 1 review note, 1 process note, and 1 agent prompt. No
runtime visual binding was changed and no Unity asset was installed.

170. P0 Batch 58 starter-cat HUD avatar install:
   - confirm the three installed avatar PNGs exist:
     - `Assets/TheCat/Art/UI/Icons/thecat_cat_saiban_hud_avatar_256_v001.png`
     - `Assets/TheCat/Art/UI/Icons/thecat_cat_nephthys_hud_avatar_256_v001.png`
     - `Assets/TheCat/Art/UI/Icons/thecat_cat_suzune_hud_avatar_256_v001.png`
   - confirm the three `.png.meta` files import as Sprites with the intended
     pivot, texture type, alpha, and filter settings
   - confirm `P0AssetManifestCatalog.CreateManifest()` reports 106 manifest
     assets and includes the three `avatar_icon` entries
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` reports 102
     runtime bindings and includes:
     - `cat.avatar.saiban`
     - `cat.avatar.nephthys`
     - `cat.avatar.suzune`
   - confirm the runtime visual contact sheet reports 102 bindings and shows
     all three avatar icons beside the current locked combat sprites
   - compare HUD screenshots against:
     - the colored three-view turnarounds
     - current Unity combat sprites
     - Batch 58 review sheet
   - verify the avatars remain readable in the actual battle HUD at target UI
     scale and do not crop away required identity anchors:
     - Saiban shield/cape/armored silver-gray tabby identity
     - Nephthys hood/gold-blue moon-sand identity
     - Suzune calico/shrine/bell healer identity
   - confirm no Batch 49/50/51 AI starter-cat body candidate was imported or
     wired into runtime bindings
   - run Unity Console, AssetDatabase refresh, scene/prefab binding, and Play
     Mode screenshot validation before marking Batch 58 fully runtime-accepted

Updated offline validation: Batch 58 installed three source-locked starter-cat
HUD avatars derived from the current locked Unity combat sprites. The dedicated
validator passed, Runtime and EditMode MSBuild passed with 0 warnings and 0
errors, and the runtime visual contact sheet was regenerated to 102 bindings.
Unity editor-side Sprite import, Console, HUD screenshot, and scene/prefab
binding validation remain pending.

171. P0 Batch 59 cat HUD avatar runtime hookup:
   - confirm `P0CatHudCard.HudAvatar` resolves:
     - `thecat_cat_saiban_hud_avatar_256_v001`
     - `thecat_cat_nephthys_hud_avatar_256_v001`
     - `thecat_cat_suzune_hud_avatar_256_v001`
   - confirm `P0CatHudCard.PrimaryHudIcon` prefers the HUD avatar and only
     falls back to the combat sprite when the avatar is missing
   - confirm the cat switch HUD rows in Play Mode draw the avatar-sized icon
     rather than the full combat sprite
   - capture HUD screenshots for all three active cats and compare against:
     - Batch 58 review sheet
     - colored three-view turnarounds
     - current combat sprites
   - confirm no AI starter-cat body candidate is visible in the cat HUD
   - run Unity Console, AssetDatabase refresh, Sprite import, scene/prefab
     binding, and HUD readability checks before marking Batch 59 accepted

Updated offline validation: Batch 59 wires the Batch 58 avatar assets into
`P0CatHudPresenter` and `GrayboxBattleController.DrawCatControls`. Runtime and
EditMode MSBuild passed with 0 warnings and 0 errors. Unity editor-side visual
confirmation remains pending because Unity MCP editor tools are not exposed in
the current Codex tool surface.

172. P0 Batch 60 skill HUD feedback install:
   - confirm the six installed feedback PNGs exist:
     - `Assets/TheCat/Art/UI/Icons/thecat_ui_skill_ready_frame_512_v001.png`
     - `Assets/TheCat/Art/UI/Icons/thecat_ui_skill_cooldown_overlay_512_v001.png`
     - `Assets/TheCat/Art/UI/Icons/thecat_ui_skill_no_target_marker_512_v001.png`
     - `Assets/TheCat/Art/UI/Icons/thecat_ui_skill_hunger_cost_chip_512_v001.png`
     - `Assets/TheCat/Art/UI/Icons/thecat_ui_auto_target_reticle_512_v001.png`
     - `Assets/TheCat/Art/UI/Icons/thecat_ui_interaction_range_ripple_512_v001.png`
   - confirm the six `.png.meta` files import as Sprites with the intended
     pivot, texture type, alpha, and filter settings
   - confirm `P0AssetManifestCatalog.CreateManifest()` reports 112 manifest
     assets and includes the six `skill_hud_feedback` entries
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` reports 108
     runtime bindings and includes:
     - `skill_hud.ready_frame`
     - `skill_hud.cooldown_overlay`
     - `skill_hud.no_target_marker`
     - `skill_hud.hunger_cost_chip`
     - `skill_hud.auto_target_reticle`
     - `battle_hud.interaction_range_ripple`
   - confirm battle HUD skill controls draw the state icon for:
     - ready
     - cooldown
     - no target
     - low hunger
   - confirm target-resolved skill rows draw the auto-target reticle and
     interaction controls draw the interaction range ripple
   - capture Play Mode screenshots for the above states and compare against the
     Batch 60 review sheet and refreshed runtime visual contact sheet
   - confirm Console has no new texture resolve, import, IMGUI layout, or
     missing-asset errors
   - confirm this non-cat UI batch does not import or display any Batch 49,
     Batch 50, or Batch 51 AI starter-cat body candidate

Updated offline validation: Batch 60 installed six non-cat skill HUD feedback
assets from Batch 57 candidates and wired them through `P0VisualAssetCatalog`,
`P0SkillHudPresenter`, and `GrayboxBattleController`. The dedicated validator
passed, Batch 58 avatar validation still passed, Runtime and EditMode MSBuild
passed with 0 warnings and 0 errors, `git diff --check` passed, and the runtime
visual contact sheet was regenerated to 108 bindings. Unity editor-side Sprite
import, Console, scene/prefab binding, and Play Mode HUD screenshot validation
remain pending because Unity MCP editor tools are not exposed in the current
Codex tool surface.

173. P0 Batch 61 starter skill VFX install:
   - confirm the three installed VFX PNGs exist:
     - `Assets/TheCat/Art/VFX/thecat_vfx_saiban_bedline_skill_512_v001.png`
     - `Assets/TheCat/Art/VFX/thecat_vfx_nephthys_moonsand_skill_512_v001.png`
     - `Assets/TheCat/Art/VFX/thecat_vfx_suzune_lullaby_skill_512_v001.png`
   - confirm the three `.png.meta` files import as Sprites with the intended
     pivot, texture type, alpha, and filter settings
   - confirm `P0AssetManifestCatalog.CreateManifest()` reports 115 manifest
     assets and includes the three symbolic `starter_skill_vfx` entries
   - confirm `design/development/P0_ASSET_MANIFEST.csv` has 115 rows and
     includes the three Batch 61 VFX rows
   - confirm `P0VisualAssetCatalog.CreateP0RuntimeBindings()` reports 111
     runtime bindings and includes:
     - `skill_vfx.saiban_bedline`
     - `skill_vfx.nephthys_moonsand`
     - `skill_vfx.suzune_lullaby`
   - confirm `P0BattleFeedbackVisualPresenter` routes the starter skill casts:
     - Saiban oath shield, sword sweep, and sun charge feedback
     - Nephthys moon-sand obelisk, quicksand, and royal mark feedback
     - Suzune sleep bell, healing bell, and moon torii feedback
   - capture Play Mode screenshots for at least one skill cast per starter cat
     and compare against:
     - Batch 61 review sheet
     - refreshed runtime visual contact sheet
     - colored three-view turnaround source locks for authority-symbol
       consistency only
   - confirm Console has no new texture resolve, import, IMGUI layout, or
     missing-asset errors
   - confirm this symbolic VFX batch does not import, display, or route any
     Batch 49, Batch 50, or Batch 51 AI starter-cat body candidate

Updated offline validation: Batch 61 installed three symbolic starter skill VFX
assets from Batch 55 candidates and wired them through `P0VisualAssetCatalog`
and `P0BattleFeedbackVisualPresenter`. The dedicated validator passed, Batch 60
skill HUD validation still passed, the runtime visual contact sheet was
regenerated to 111 bindings, and the Batch 61 queue checkpoint reported 1
completed candidate pack pending Unity review plus 5 Unity-blocked
validation/install items. Runtime and EditMode MSBuild passed with 0 warnings
and 0 errors, solution MSBuild passed with 0 errors and existing MSB3277
warnings, `git diff --check` passed, and local Unity MCP setup is present.
Unity editor-side Sprite import, Console, scene/prefab binding, skill timing,
and Play Mode battle-feedback screenshot validation remain pending because
Unity MCP editor tools are not exposed in the current Codex tool surface.

174. P0 Batch 62 runtime control icon candidate review:
   - confirm the six candidate PNGs exist outside `Assets`:
     - `design/development/asset_candidates/ui/runtime_controls/batch_62_runtime_control_icon_candidates_2026-06-15/thecat_ui_runtime_pause_icon_128_candidate_v001.png`
     - `design/development/asset_candidates/ui/runtime_controls/batch_62_runtime_control_icon_candidates_2026-06-15/thecat_ui_runtime_resume_icon_128_candidate_v001.png`
     - `design/development/asset_candidates/ui/runtime_controls/batch_62_runtime_control_icon_candidates_2026-06-15/thecat_ui_runtime_speed_half_icon_128_candidate_v001.png`
     - `design/development/asset_candidates/ui/runtime_controls/batch_62_runtime_control_icon_candidates_2026-06-15/thecat_ui_runtime_speed_normal_icon_128_candidate_v001.png`
     - `design/development/asset_candidates/ui/runtime_controls/batch_62_runtime_control_icon_candidates_2026-06-15/thecat_ui_runtime_speed_fast_icon_128_candidate_v001.png`
     - `design/development/asset_candidates/ui/runtime_controls/batch_62_runtime_control_icon_candidates_2026-06-15/thecat_ui_runtime_restart_icon_128_candidate_v001.png`
   - confirm no Batch 62 `.png.meta` files exist because this is a
     candidate-only pack
   - compare the review sheet against the runtime settings / keyboard-control
     surface for readability:
     - pause
     - resume
     - speed 0.5x
     - speed 1x
     - speed 1.5x
     - restart run
   - confirm Console has no import, texture, or IMGUI layout errors after any
     future install attempt
   - approve or reject a formal install into `Assets/TheCat/Art/UI/Icons`
     only after HUD scale and shortcut affordance checks pass

Updated offline validation: Batch 62 was produced as a Codex-side
candidate-only non-cat UI pack. The dedicated validator passed, Batch 60 and
Batch 61 validators still passed, Runtime and EditMode MSBuild passed, solution
MSBuild passed with 0 errors and existing MSB3277 warnings, `git diff --check`
passed, and the touched-file trailing-whitespace scan passed. It does not
modify manifest/runtime binding counts and does not affect starter-cat body
art. Unity-side review remains required before installation.

175. P0 Batch 63 runtime control panel candidate review:
   - confirm the four candidate PNGs exist outside `Assets`:
     - `design/development/asset_candidates/ui/runtime_controls/batch_63_runtime_control_panel_candidates_2026-06-15/thecat_ui_runtime_pause_overlay_panel_768x432_candidate_v001.png`
     - `design/development/asset_candidates/ui/runtime_controls/batch_63_runtime_control_panel_candidates_2026-06-15/thecat_ui_runtime_speed_segmented_control_512x128_candidate_v001.png`
     - `design/development/asset_candidates/ui/runtime_controls/batch_63_runtime_control_panel_candidates_2026-06-15/thecat_ui_runtime_restart_confirm_plate_512x256_candidate_v001.png`
     - `design/development/asset_candidates/ui/runtime_controls/batch_63_runtime_control_panel_candidates_2026-06-15/thecat_ui_runtime_keyboard_hint_strip_768x128_candidate_v001.png`
   - confirm no Batch 63 `.png.meta` files exist because this is a
     candidate-only pack
   - compare the review sheet against the runtime settings / battle HUD surface
     for readability:
     - pause overlay panel
     - speed segmented control
     - restart confirmation plate
     - keyboard hint strip
   - confirm Console has no import, texture, or IMGUI layout errors after any
     future install attempt
   - approve or reject a formal install into `Assets/TheCat/Art/UI` only after
     HUD scale and shortcut affordance checks pass

Updated offline validation: Batch 63 was produced as a Codex-side
candidate-only non-cat UI panel pack. The dedicated validator passed, Batch 62,
Batch 60, and Batch 61 validators still passed, Runtime and EditMode MSBuild
passed, solution MSBuild passed with 0 errors and existing MSB3277 warnings,
and `git diff --check` passed. It does not modify manifest/runtime binding
counts and does not affect starter-cat body art. Unity-side review remains
required before installation.

176. P0 asset Unity validation checklist menu:
   - run `TheCat/P0/Write P0 Asset Unity Validation Checklist` in the Unity
     editor after Unity MCP/editor tools are exposed
   - confirm the generated checklist appears at
     `design/development/asset_review/P0_ASSET_UNITY_VALIDATION_CHECKLIST.md`
   - verify the checklist reports 9 queue items:
     - 4 candidate packs complete pending Unity review
     - 5 Unity-blocked validation/install items
     - 2 active screenshot validation items
     - 2 installed asset validation items
     - 1 formal install decision item
   - confirm candidate-only packs have no Unity `.meta` files before approval
   - capture active-cat, active-enemy, battle HUD, skill VFX,
     runtime-control, and secondary-warning screenshots as applicable
   - compare starter-cat screenshots against the locked colored three-view
     turnarounds before approving any cat-body asset
   - confirm Console has no new texture resolve, import, IMGUI layout,
     missing-asset, or validation errors

Updated offline validation: added a runtime checklist report, an editor menu,
and EditMode coverage for the current P0 asset-production queue. This does not
install assets or change manifest/runtime counts; it formalizes the bridge
between Codex-side asset generation and Unity-side evidence before any future
install decision.

177. P0 Batch 64 secondary enemy warning candidate review:
   - confirm the four candidate PNGs exist outside `Assets`:
     - `design/development/asset_candidates/enemies/secondary_warning_vfx/batch_64_secondary_enemy_warning_candidates_2026-06-15/thecat_vfx_dreamrail_track_warning_256_candidate_v001.png`
     - `design/development/asset_candidates/enemies/secondary_warning_vfx/batch_64_secondary_enemy_warning_candidates_2026-06-15/thecat_vfx_redeye_alarm_shockring_256_candidate_v001.png`
     - `design/development/asset_candidates/enemies/secondary_warning_vfx/batch_64_secondary_enemy_warning_candidates_2026-06-15/thecat_vfx_unread_swarm_attach_warning_256_candidate_v001.png`
     - `design/development/asset_candidates/enemies/secondary_warning_vfx/batch_64_secondary_enemy_warning_candidates_2026-06-15/thecat_vfx_fallingteddy_slam_marker_256_candidate_v001.png`
   - confirm no Batch 64 `.png.meta` files exist because this is a
     candidate-only pack
   - compare the review sheet against the secondary enemy warning needs:
     - Dream Rail Train straight track charge
     - Red Eye Alarm shock ring
     - Unread Red Dot swarm orbit / attach
     - Falling Dream Teddy jump-slam landing
   - confirm Console has no import, texture, or IMGUI layout errors after any
     future install attempt
   - approve or reject a formal install into `Assets/TheCat/Art/Enemies/VFX`
     only after gameplay-scale warning readability checks pass

Updated offline validation: Batch 64 was produced as a Codex-side
candidate-only non-cat warning VFX pack. The dedicated validator passed. It
does not modify manifest/runtime binding counts and does not affect starter-cat
body art. Unity-side review remains required before installation.

178. P0 starter-cat strict identity gate review:
   - before approving any Saiban, Nephthys, or Suzune body-art replacement,
     confirm the candidate manifest binds to:
     - the exact colored three-view turnaround path and SHA-256 source lock
     - the current Batch 47 strict-generation JSON path and SHA-256
     - the current Batch 47 strict-generation card path and SHA-256
   - confirm the Batch 47 JSON spec for that cat still contains:
     - `source_lock_id`
     - exact `source_turnaround_path`
     - non-human cat body rule
     - must-keep list
     - reject list
     - positive / negative prompts
     - palette drift rejection
     - `strict_generation_spec_only_do_not_import`
   - reject any candidate manifest with corrupted source paths such as
     `?assets` or any source path that does not exactly match the source lock
   - capture the matching active-cat screenshot:
     - `04-active-cat-saiban.png`
     - `05-active-cat-nephthys.png`
     - `06-active-cat-suzune.png`
   - compare the Unity sprite against the locked colored three-view turnaround
     and the Batch 47 spec card before import approval
   - confirm Console has no import, texture, material, missing reference, or
     prefab binding errors after any future install attempt

Updated offline validation: `P0StarterCatStrictCandidateEvidence` now enforces
exact colored-turnaround paths, Batch 47 JSON/card hashes, and Batch 47 JSON
identity clauses for Batch 49/50/51 starter-cat candidates. Runtime and
EditMode MSBuild passed, and the Batch 47 validator passed. Unity active-cat
visual review remains required before import.

179. P0 asset next-batch gate review:
   - run or inspect `P0AssetProductionNextBatchGate.EvaluateCurrentGate()`
     before opening any new systematic asset batch
   - run or inspect `P0AssetSystematicProductionPlan.EvaluateCurrentPlan()`
     before reviewing existing non-cat candidate packs
   - when Unity MCP/editor access is available, run:
     `TheCat/P0/Write P0 Systematic Asset Production Plan`
   - confirm the menu writes:
     `design/development/asset_review/P0_SYSTEMATIC_ASSET_PRODUCTION_PLAN.md`
   - confirm the report says:
     - `Next Codex candidate batch allowed: yes`
     - `Allowed lane: NewScopedCandidateOrSpecBatchOutsideAssets`
     - `Starter-cat body import allowed: no`
     - `Unity review required before install: yes`
     - `Recommended work mode: ReviewExistingNonCatCandidatePacksBeforeNewImageGeneration`
     - `Starter-cat body lane locked: yes`
   - keep new Codex-side work outside `Assets` unless a later formal install
     decision explicitly approves the target
   - reject any starter-cat body import until all three active-cat screenshots,
     exact colored-turnaround identity locks, explicit review approvals, and
     Console-clean Unity validation are complete
   - do not generate, crop, recolor, import, or runtime-bind starter-cat body
     art until active-cat Play Mode screenshots are approved against the locked
     colored three-view turnarounds
   - after Unity MCP/editor access is available, generate the Unity validation
     checklist and compare it against the next-batch gate counts
   - current tool status: Unity MCP local setup is present, but live
     `Unity_GetConsoleLogs` and `Unity_ManageEditor/GetState` returned
     `Connection revoked` on 2026-06-20; re-enable approval before running the
     Editor menu

Updated offline validation: the next-batch gate composes offline production
readiness, asset queue coverage, Unity checklist readiness, strict starter-cat
identity evidence, and formal import state. It permits scoped Codex
candidate/spec work but keeps starter-cat body import blocked. The systematic
asset production plan now has a Unity Editor report menu and explicit report
path so the next review pass can start from the same locked decision state.

180. P0 Batch 65 route-map readability candidate review:
   - confirm the five candidate PNGs exist outside `Assets`:
     - `design/development/asset_candidates/ui/route_map/batch_65_route_map_readability_candidates_2026-06-15/thecat_ui_route_current_node_halo_256_candidate_v001.png`
     - `design/development/asset_candidates/ui/route_map/batch_65_route_map_readability_candidates_2026-06-15/thecat_ui_route_selected_node_ring_256_candidate_v001.png`
     - `design/development/asset_candidates/ui/route_map/batch_65_route_map_readability_candidates_2026-06-15/thecat_ui_route_available_path_connector_512x128_candidate_v001.png`
     - `design/development/asset_candidates/ui/route_map/batch_65_route_map_readability_candidates_2026-06-15/thecat_ui_route_locked_path_connector_512x128_candidate_v001.png`
     - `design/development/asset_candidates/ui/route_map/batch_65_route_map_readability_candidates_2026-06-15/thecat_ui_route_boss_path_pressure_512x128_candidate_v001.png`
   - confirm no Batch 65 `.png.meta` files exist because this is a
     candidate-only pack
   - compare the review sheet against route-map needs:
     - active/current layer readability
     - selected branch contrast
     - available/future path readability
     - Boss path pressure readability
   - confirm Console has no import, texture, or IMGUI layout errors after any
     future install attempt
   - approve or reject formal install into `Assets/TheCat/Art/UI` only after
     route-map scale and screenshot checks pass

Updated offline validation: Batch 65 was produced as a Codex-side
candidate-only non-cat route-map UI pack. The dedicated validator passed. It
does not modify manifest/runtime binding counts and does not affect
starter-cat body art. Unity-side review remains required before installation.

181. P0 asset Unity validation checklist file evidence:
   - confirm
     `design/development/asset_review/P0_ASSET_UNITY_VALIDATION_CHECKLIST.md`
     exists before opening another formal install batch
   - confirm the checklist says:
     - `Queue items: 19`
     - `Candidate review items: 14`
     - `Unity-blocked items: 5`
     - `Candidate no-meta policy items: 14`
     - `Console-gated items: 19`
    - confirm the checklist includes Batch 65 route-map readability and Batch
      67 bedroom interaction affordance coverage, plus Batch 83 loading/start
      preflight, Batch 84 result/settlement preflight, and Batch 85
      settings/pause preflight, Batch 86 dream-route preflight, Batch 87
      battle HUD preflight, Batch 88 character-select preflight, Batch 89
      skill-selection preflight, plus Batch 90 cat-room preflight coverage
   - confirm the checklist keeps starter-cat active screenshot validation tied
     to the locked colored three-view turnarounds
   - when Unity MCP/editor tools are available, run
     `TheCat/P0/Write P0 Asset Unity Validation Checklist`
   - compare Unity-regenerated output against the current offline checklist
   - continue to treat the checklist as pending until Console, screenshots,
     Sprite import settings, and scene/prefab references are checked in Unity

Updated offline validation: `P0AssetUnityValidationChecklistFileEvidence`
accepts the generated checklist artifact and fails if Batch 65, 67, 83, 84, 85,
86, 87, or 88 coverage is removed. Current MCP tool exposure is insufficient
for live Unity Console or screenshot validation, so the checklist is a
Codex-to-Unity handoff artifact.

182. P0 Batch 66 systematic asset master plan:
   - confirm Batch 66 remains a spec/control batch only
   - confirm these files exist:
     - `design/development/asset_candidates/p0_asset_dashboard/batch_66_systematic_asset_master_plan_2026-06-15/p0_asset_batch66_master_gap_matrix.csv`
     - `design/development/asset_candidates/p0_asset_dashboard/batch_66_systematic_asset_master_plan_2026-06-15/p0_asset_batch66_master_blueprint.md`
     - `design/development/asset_candidates/p0_asset_dashboard/batch_66_systematic_asset_master_plan_2026-06-15/p0_asset_batch66_process_note.md`
     - `design/development/agent_prompts/p0_asset_batch_67_bedroom_interaction_affordance_candidates.md`
   - confirm Batch 66 created no Unity `.meta` files
   - confirm Batch 66 changed no manifest/runtime binding counts
   - use Batch 67 as the next Codex-side image production prompt only if the
     new work stays outside `Assets`
   - for Batch 67 Unity review later, capture bed, litter box, feeder, blocked
     interaction, and interaction range screenshots before approving install

Updated offline validation: `validate_systematic_asset_master_plan.ps1`
passes. The next recommended image lane is non-cat bedroom interaction
affordance UI/VFX. Cat body work remains blocked behind colored three-view
turnaround matching and active-cat Unity screenshot review.

183. P0 Batch 67 bedroom interaction affordance candidate review:
   - confirm the six candidate PNGs exist outside `Assets`:
     - `design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15/thecat_ui_interaction_bed_ready_ring_256_candidate_v001.png`
     - `design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15/thecat_ui_interaction_bed_restore_pulse_256_candidate_v001.png`
     - `design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15/thecat_ui_interaction_litter_urgent_marker_256_candidate_v001.png`
     - `design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15/thecat_ui_interaction_feeder_ready_marker_256_candidate_v001.png`
     - `design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15/thecat_ui_interaction_blocked_marker_256_candidate_v001.png`
     - `design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15/thecat_ui_interaction_range_ripple_512_candidate_v001.png`
   - confirm no Batch 67 `.png.meta` files exist because this is a
     candidate-only pack
   - compare the review sheet against P0 interaction needs:
     - bed ready/readable
     - bed restore/readable
     - litter-box urgency/readable
     - feeder ready/readable
     - blocked state/readable
     - valid range/readable
   - after a future formal install attempt, capture screenshots for bed,
     litter box, feeder, blocked interaction, and range states
   - confirm interaction timing does not obscure cat/enemy intent or core
     gauges
   - confirm Console has no import, texture, missing reference, or prefab
     binding errors
   - approve or reject formal install into `Assets/TheCat/Art/UI` only after
     runtime scale and screenshot checks pass

Updated offline validation: Batch 67 was produced as a Codex-side
candidate-only non-cat bedroom interaction UI/VFX pack. The dedicated
validator passed. It does not modify manifest/runtime binding counts and does
not affect starter-cat body art. Unity-side review remains required before
installation.

184. P0 Batch 68 starter-cat core document source-lock gate:
   - confirm the Batch 68 prompt exists:
     `design/development/agent_prompts/p0_asset_batch_68_starter_cat_core_doc_source_lock_gate.md`
   - confirm `P0StarterCatSourceLockPacketEvidence` reports:
     - 3 core source-lock documents
     - 3 core documents with all exact colored-turnaround source paths
     - 3 core documents with starter-cat formal import block language
     - 0 core document mojibake/stale encoded path mentions
   - confirm the generated P0 asset review packet exposes those counts before
     approving any new starter-cat image-production batch
   - keep Saiban, Nephthys, and Suzune runtime sprite replacement blocked until
     active-cat Play Mode screenshots are captured and compared against the
     colored three-view turnarounds
   - do not approve cat sprite import if any core source-lock document is
     missing an exact source path or contains stale encoded design path text

Updated offline validation: Batch 68 tightened the starter-cat core document
source-lock gate only. It generated no images, imported no Unity assets, and
changed no runtime cat sprite bindings. Unity-side active-cat screenshot review
remains required before starter-cat import approval.

185. P0 Batch 69 starter-cat turnaround/runtime comparison audit:
   - confirm the Batch 69 prompt exists:
     `design/development/agent_prompts/p0_asset_batch_69_starter_cat_turnaround_runtime_comparison_audit.md`
   - confirm the audit package exists outside `Assets`:
     `design/development/asset_candidates/starter_cats/batch_69_turnaround_runtime_comparison_audit_2026-06-15`
   - confirm the package contains:
     - `thecat_cat_starter_turnaround_runtime_comparison_batch69_review_sheet.png`
     - `starter_cat_turnaround_runtime_comparison_batch69_manifest.csv`
     - `starter_cat_turnaround_runtime_comparison_batch69_review.md`
     - `starter_cat_turnaround_runtime_comparison_batch69_process_note.md`
   - confirm `P0StarterCatTurnaroundComparisonAuditEvidence` reports:
     - 4/4 audit artifacts
     - 3/3 manifest rows
     - 3/3 source-lock mentions
     - 3/3 exact colored-turnaround source paths
     - 3/3 current Unity combat sprite paths
     - 3/3 active-cat screenshot targets
     - 3/3 audit-only recommendations
     - 0 Unity `.meta` files
   - confirm the generated P0 asset review packet includes the
     `Starter Cat Turnaround Runtime Comparison Audit` section
   - in Unity, capture active-cat Play Mode screenshots for Saiban, Nephthys,
     and Suzune, then compare them against the Batch 69 review sheet and the
     locked colored three-view turnarounds
   - keep cat sprite replacement blocked until Unity Console, Sprite import
     settings, scene/prefab bindings, HUD/battle SpriteRenderer references, and
     active-cat screenshots all pass

Updated offline validation: Batch 69 created a Codex-side audit-only
turnaround/runtime comparison package. It generated no new cat art, imported no
Unity assets, and changed no runtime cat sprite bindings. Unity-side active-cat
screenshot review remains required before starter-cat import approval.

186. P0 Batch 70 starter-cat source-turnaround reference plates:
   - confirm the Batch 70 prompt exists:
     `design/development/agent_prompts/p0_asset_batch_70_starter_cat_source_turnaround_reference_plates.md`
   - confirm the reference package exists outside `Assets`:
     `design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15`
   - confirm the package contains:
     - nine `768x768` front/side/back reference plate PNGs
     - `starter_cat_turnaround_reference_plates_batch70_manifest.csv`
     - `thecat_cat_starter_turnaround_reference_plates_batch70_review_sheet.png`
     - `starter_cat_turnaround_reference_plates_batch70_review.md`
     - `starter_cat_turnaround_reference_plates_batch70_process_note.md`
   - confirm `P0StarterCatReferencePlateEvidence` reports:
     - 13/13 artifacts
     - 9/9 reference plates
     - 9/9 manifest rows
     - 3/3 source-lock mentions
     - 3/3 exact colored-turnaround source paths
     - 9/9 front/side/back view rows
     - 9/9 import blocks
     - 0 Unity `.meta` files
   - confirm the generated P0 asset review packet includes the
     `Starter Cat Source Turnaround Reference Plates` section
   - in Unity, use these plates only as visual reference when comparing active
     Saiban, Nephthys, and Suzune screenshots against the colored turnarounds
   - do not import the Batch 70 plates as runtime sprites; they are reference
     inputs, not transparent gameplay assets

Updated offline validation: Batch 70 created deterministic source-derived
front/side/back reference plates from the locked colored starter-cat
turnarounds. It generated no new cat art, imported no Unity assets, and changed
no runtime cat sprite bindings. Unity-side active-cat screenshot review remains
required before starter-cat import approval.

187. P0 Batch 71 Saiban Unity reference install:
   - confirm the Batch 71 prompt exists:
     `design/development/agent_prompts/p0_asset_batch_71_saiban_unity_reference_install.md`
   - confirm the installed Unity atlas exists:
     `Assets/TheCat/Art/Characters/References/thecat_cat_saiban_turnaround_reference_atlas_2304x768_v001.png`
   - confirm the atlas `.png.meta` uses Sprite Single import settings,
     disabled mipmaps, alpha transparency, max texture size at least 4096, and
     `TheCatP0ImportSettings:v1`
   - confirm the Batch 71 package exists:
     `design/development/asset_candidates/starter_cats/saiban/batch_71_saiban_unity_reference_install_2026-06-15`
   - confirm the package contains:
     - `saiban_batch71_unity_reference_install_manifest.csv`
     - `thecat_cat_saiban_batch71_unity_reference_install_review_sheet.png`
     - `saiban_batch71_unity_reference_install_review.md`
     - `saiban_batch71_unity_reference_install_process_note.md`
   - confirm `P0StarterCatUnityReferenceInstallEvidence` reports:
     - 7/7 artifacts
     - 1/1 installed debug reference asset
     - 1/1 manifest row
     - 1/1 Saiban source-lock mention
     - 3/3 Batch 70 reference plate links
     - 6/6 import setting tokens
     - 5/5 runtime replacement block mentions
   - in Unity, refresh AssetDatabase and inspect the atlas import settings
   - confirm no SpriteRenderer, HUD presenter, prefab, or runtime catalog binds
     this debug reference as gameplay art
   - capture active-cat Saiban Play Mode screenshots and compare them against
     the Batch 71 atlas, Batch 70 plate, and locked colored turnaround before
     approving any future Saiban body-art replacement

Updated offline validation: Batch 71 installed one Saiban source-derived debug
reference atlas into Unity from Batch 70 plates. It did not generate new cat
body art, did not replace the Saiban combat sprite or HUD avatar, and did not
add a runtime binding. Unity-side import inspection and active-cat screenshot
comparison remain required.

188. P0 Chinese UI responsive-scale screenshot validation:
   - use the Batch 75 validation-only evidence packet:
     `design/development/asset_candidates/ui/chinese_ui_scale_evidence/batch_75_chinese_ui_scale_evidence_templates_2026-06-20`
   - confirm the Batch 75 packet contains:
     - `chinese_ui_scale_batch75_manifest.csv`
     - `chinese_ui_scale_batch75_capture_matrix.csv`
     - `thecat_ui_chinese_scale_batch75_review_sheet.png`
     - `chinese_ui_scale_batch75_candidate_review.md`
     - `chinese_ui_scale_batch75_process_note.md`
   - confirm `P0ChineseUiScaleValidationPlan.EvaluateCurrentPlan()` reports:
     - 5/5 UI surfaces
     - 4/4 required resolutions
     - 7/7 acceptance checks
     - 8/8 covered checks
   - confirm `P0ChineseUiScaleEvidencePacket.EvaluateCurrentPacket()` reports:
     - 9/9 expected Batch 75 packet files
     - 4/4 manifest template rows
     - 20/20 capture matrix rows
     - 5/5 UI surface coverage
     - 4/4 resolution coverage
     - 20/20 surface/resolution pair coverage
     - 0 Unity `.meta` files
     - validation-only, no-runtime-binding, non-cat manifest recommendations
   - in Unity Play Mode, capture the following surfaces at `1024x768`,
     `1280x720`, `1600x900`, and `1920x1080`:
     - main menu / character select
     - 10-layer route map
     - bedroom guard battle HUD
     - skill / enemy HUD state
     - result / pause settings surface
   - for every screenshot, verify:
     - player-facing UI text is Chinese except necessary tokens such as `HP`,
       shortcuts, or technical labels
     - panels do not overlap, pile up, or clip long Chinese text
     - long panels remain scrollable and usable
     - narrow-width controls stack cleanly instead of crowding into one row
     - HUD status, skill, enemy, route, result, and settings labels remain
       readable
     - Console has no errors during surface navigation
   - save screenshots and Console notes under the next Unity validation
     evidence folder before marking the UI scale pass accepted
   - current tool status: Unity MCP `ManageEditor/GetState` was attempted on
     2026-06-20 and returned `Connection revoked`; re-enable approval in Unity
     Project Settings before running the screenshot matrix

Updated offline validation: the Chinese UI scale validation plan is covered by
Runtime and EditMode MSBuild gates, Batch 75 provides validation templates plus
a 20-row capture matrix, and `P0ChineseUiScaleEvidencePacket` now guards the
Batch 75 packet against missing files, unsafe import recommendations, accidental
Unity `.meta` files, and stale surface/resolution coverage. Unity-side
screenshots and Console inspection remain required before final visual
acceptance.

189. P0 Unity runtime validation plan execution:
   - confirm `P0UnityRuntimeValidationPlan.EvaluateCurrentPlan()` reports:
     - 17/17 runtime validation steps
     - 10/10 Play Mode screenshot steps matching
       `P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames`
     - 3/3 active-cat screenshot steps for Saiban, Nephthys, and Suzune
     - 2/2 smoke checks: route flow and defeat flow
     - 1/1 clean Console check
     - 2/2 Unity binding checks: scene/prefab bindings and Sprite import
       settings
     - 2/2 review checks: starter-cat turnaround review and Chinese UI scale
       matrix
     - 20/20 Batch 75 Chinese UI scale capture rows
   - confirm the Editor menu exists:
     `TheCat/P0/Write P0 Unity Runtime Validation Plan`
   - use the menu to write:
     `design/development/asset_review/P0_UNITY_RUNTIME_VALIDATION_PLAN.md`
   - confirm the generated report includes:
     - `TheCat/P0/Start Play Mode Acceptance Smoke`
     - `design/development/screenshots/p0-playmode-smoke`
     - the execution-plan disclaimer that final P0 visual acceptance still
       requires Unity Play Mode screenshots, clean Console, scene/prefab
       binding inspection, Sprite import checks, active-cat turnaround review,
       and the Batch 75 Chinese UI scale matrix
   - confirm `P0PlayModeAcceptanceSmoke.ExpectedEvidenceCheckCount == 8` and
     the evidence order includes:
     - screenshot capture plan
     - runtime visual screenshot plan
     - runtime visual contact sheet
     - screenshot file evidence
     - Unity runtime validation plan
     - screenshot smoke
     - route-flow smoke
     - defeat-flow smoke
   - after Unity MCP approval is restored or the Editor is run manually:
     - execute the P0 Play Mode acceptance smoke
     - save the 10 expected screenshots under
       `design/development/screenshots/p0-playmode-smoke`
     - capture Unity Console status with zero errors after the run
     - inspect MainMenu, RouteMap, and GrayboxBattle scene/prefab/controller
       bindings
     - inspect Sprite import settings for runtime-bound art
     - compare `04-active-cat-saiban.png`, `05-active-cat-nephthys.png`, and
       `06-active-cat-suzune.png` against the locked colored three-view
       turnarounds before any starter-cat body-art replacement
     - fill the Batch 75 20-row Chinese UI scale matrix and attach Console
       notes
   - do not mark final P0 visual acceptance complete until this runtime plan,
     Play Mode evidence, screenshot file evidence, starter-cat formal import,
     Console, scene/prefab binding, import settings, and UI scale matrix all
     pass together

Updated offline validation: `P0UnityRuntimeValidationPlan` now turns final Unity
runtime acceptance into a code-backed 17-step plan and `P0PlayModeEvidenceChecklist`
now requires that plan as the eighth evidence check. An Editor report menu now
writes the markdown runbook path for Unity-side review. Unity-side execution is
still pending because Unity MCP currently returns `Connection revoked`.

190. P0 Batch 76 owner sleep-state animation candidate validation:
   - use the Batch 76 candidate packet:
     `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24`
   - confirm the packet contains:
     - `owner_sleep_states_batch76_manifest.csv`
     - `thecat_owner_sleep_states_batch76_chromakey_source_1536x1024_v002.png`
     - `thecat_owner_sleep_states_batch76_alpha_sheet_1536x1024_candidate_v002.png`
     - `thecat_owner_sleep_states_batch76_contact_sheet_1920x1320_v001.png`
     - `thecat_owner_sleep_states_batch76_review_sheet_1920x1320_v001.png`
     - `owner_sleep_states_batch76_candidate_review.md`
     - `owner_sleep_states_batch76_process_note.md`
     - 24 padded frame PNGs under `frames`
   - run:
     `powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_owner_sleep_state_framesheet_candidate.ps1`
   - confirm the validator reports:
     - 24/24 manifest rows
     - 4/4 states
     - 6/6 frames per state
     - unique `asset_id` values matching frame file stems
     - 256x256 frame dimensions
     - at least 12px alpha margins
     - 0 Unity `.meta` files
     - candidate paths outside `Assets`
   - in Unity, do not import automatically; first choose whether the owner
     state should layer over the existing bedroom bed prop or temporarily
     replace that layer for screenshot review
   - if approved for import review, inspect Sprite import settings, slicing,
     pivot, scale, and frame timing for:
     - `deep_sleep`
     - `half_awake`
     - `near_awake`
     - `wake_failure`
   - capture battle-world screenshots for all four owner states and verify:
     - the owner-state overlay does not duplicate or fight the bed prop
     - the consciousness orb, alarm/light vibration, and dream cracks remain
       readable at gameplay scale
     - sleep-state changes align with owner damage thresholds
     - Console has no errors
   - do not mark the owner sleep-state asset accepted until screenshot,
     Console, Sprite import, scene/prefab binding, and timing checks all pass

Updated offline validation: Batch 76 v002 creates a review-only owner
sleep-state overlay packet with 24 padded alpha frames, no Unity `.meta` files,
and no install approval. Unity-side slicing, pivot, scale, overlay-vs-bed-layer,
timing, screenshots, and Console checks remain required.

191. P0 Batch 77 owner sleep status icon candidate validation:
   - use the Batch 77 candidate packet:
     `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24`
   - confirm the packet contains:
     - `owner_sleep_status_icons_batch77_manifest.csv`
     - `thecat_ui_owner_sleep_status_icons_batch77_chromakey_source_v001.png`
     - `thecat_ui_owner_sleep_status_icons_batch77_alpha_sheet_v001.png`
     - `thecat_ui_owner_sleep_status_icons_batch77_contact_sheet_v001.png`
     - `thecat_ui_owner_sleep_status_icons_batch77_review_sheet_v001.png`
     - `owner_sleep_status_icons_batch77_candidate_review.md`
     - `owner_sleep_status_icons_batch77_process_note.md`
     - 4 icons under `icons_256`
     - 4 icons under `icons_64`
     - 4 icons under `icons_32`
   - run:
     `powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_owner_sleep_status_icon_candidates.ps1`
   - confirm the validator reports:
     - 12/12 manifest rows
     - 4/4 owner sleep states
     - 3/3 size variants per state
     - unique `asset_id` values matching file stems
     - matching hashes and dimensions
     - normalized candidate paths outside `Assets`
     - 0 Unity `.meta` files
   - in Unity, do not import automatically; first compare the icons against the
     existing `sleep_stable`, `mark`, and owner-sleep HUD language
   - if approved for import review, inspect Sprite import settings and verify
     all four states at 64px and 32px:
     - `deep_sleep`
     - `half_awake`
     - `near_awake`
     - `wake_failure`
   - capture HUD and settlement screenshots over dark and light panels and
     verify:
     - `wake_failure` does not read like the existing purple Mark/eye sigil
     - `half_awake` reads as a first warning rather than a critical state
     - cooldown masks and numeric overlays remain readable
     - sleep-state icon changes align with owner damage thresholds and Batch 76
       animation timing
     - Console has no errors
   - do not mark the owner sleep status icons accepted until screenshot,
     Console, Sprite import, scene/prefab binding, and readability checks all
     pass

Updated offline validation: Batch 77 creates a review-only symbolic UI icon
packet with 12 transparent owner sleep-state candidates, no Unity `.meta`
files, and no install approval. Unity-side 64px/32px readability,
mark-icon-collision review, HUD/settlement screenshots, cooldown overlays,
Sprite import, scene/prefab binding, and Console checks remain required.

192. P0 Batch 78 settings control candidate validation:
   - use the Batch 78 candidate packet:
     `design/development/asset_candidates/ui/settings_controls/batch_78_settings_control_candidates_2026-06-24`
   - confirm the packet contains:
     - `settings_controls_batch78_manifest.csv`
     - `thecat_ui_settings_controls_batch78_chromakey_source_v001.png`
     - `thecat_ui_settings_controls_batch78_alpha_sheet_v001.png`
     - `thecat_ui_settings_controls_batch78_contact_sheet_v001.png`
     - `thecat_ui_settings_controls_batch78_review_sheet_v001.png`
     - `settings_controls_batch78_candidate_review.md`
     - `settings_controls_batch78_process_note.md`
     - 6 controls under `controls`
   - run:
     `powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_settings_control_candidates.ps1`
   - confirm the validator reports:
     - 6/6 manifest rows
     - 6/6 settings controls
     - exact sizes for slider track, slider knob, switch off/on, and checkbox
       unchecked/checked
     - unique `asset_id` values matching file stems
     - matching hashes and dimensions
     - normalized candidate paths outside `Assets`
     - 0 Unity `.meta` files
   - in Unity, do not import automatically; first compare the controls against
     the current settings / pause UI language and Batch 62/63 runtime-control
     candidates
   - if approved for import review, inspect Sprite import settings and verify:
     - slider track and knob align during dragging
     - slider value-fill behavior reads correctly with the large ornamental knob
     - switch off/on contrast stays clear on the final settings panel
     - switch off/on distinction survives a color-blind accessibility pass
     - checkbox unchecked/checked states remain distinct at runtime scale
     - click/pointer target sizes remain usable
     - controls read on both dark and light settings-panel backgrounds
     - Console has no errors
   - do not mark the settings controls accepted until screenshot, Console,
     Sprite import, scene/prefab binding, accessibility contrast, and pointer
     scale checks all pass

Updated offline validation: Batch 78 creates a review-only settings-control UI
packet with 6 transparent slider/switch/checkbox candidates, no Unity `.meta`
files, and no install approval. Unity-side import settings, settings-screen
screenshots, slider interaction, on/off contrast, click target scale, panel
readability, scene/prefab binding, and Console checks remain required.

193. P0 Batch 79 system icon candidate validation:
   - use the Batch 79 candidate packet:
     `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24`
   - confirm the packet contains:
     - `system_icons_batch79_manifest.csv`
     - `thecat_ui_system_icons_batch79_chromakey_source_v001.png`
     - `thecat_ui_system_icons_batch79_alpha_sheet_v001.png`
     - `thecat_ui_system_icons_batch79_contact_sheet_v001.png`
     - `thecat_ui_system_icons_batch79_review_sheet_v001.png`
     - `system_icons_batch79_candidate_review.md`
     - `system_icons_batch79_process_note.md`
     - 10 icons under `icons_128`
     - 10 icons under `icons_64`
     - 10 icons under `icons_32`
   - run:
     `powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_system_icon_candidates.ps1`
   - confirm the validator reports:
     - 30/30 manifest rows
     - 10/10 system icons
     - 3/3 size variants per icon
     - unique `asset_id` values matching file stems
     - matching hashes and dimensions
     - normalized candidate paths outside `Assets`
     - 0 Unity `.meta` files
   - in Unity, do not import automatically; first compare the icons against the
     current settings / pause / runtime-control UI language and Batch 78
     settings-control candidates
   - if approved for import review, inspect Sprite import settings and verify:
     - all icons remain readable at 64px and 32px
     - `mute` is clearly distinct from `sound`
     - `warning` reads as warning without exclamation text
     - `back`, `close`, `pause`, `continue`, and `retry` are not confused with
       each other in the same menu
     - `lock` remains readable over route-map or selection-card surfaces
     - controls read on both dark and light UI backgrounds
     - Console has no errors
   - do not mark the system icons accepted until screenshot, Console, Sprite
     import, scene/prefab binding, and small-size readability checks all pass

Updated offline validation: Batch 79 creates a review-only system-icon UI
packet with 30 transparent settings/sound/mute/back/close/pause/continue/retry/
lock/warning candidates, no Unity `.meta` files, and no install approval.
Unity-side import settings, small-size readability, icon-semantic clarity,
UI screenshots, scene/prefab binding, and Console checks remain required.

194. P0 Batch 83 loading/start preflight candidate Unity review:
   - use the Batch 83 candidate packet:
     `design/development/asset_candidates/ui/loading_start/batch_83_loading_start_preflight_2026-06-25`
   - confirm the packet contains:
     - `thecat_ui_loading_start_batch83_manifest.csv`
     - `thecat_ui_loading_start_batch83_review_sheet_v001.png`
     - `thecat_ui_loading_start_batch83_contact_sheet_v001.png`
     - `thecat_ui_loading_start_batch83_candidate_review.md`
     - `thecat_ui_loading_start_batch83_process_note.md`
     - 4 transparent sprites under `sprites`
     - 4 local mockups under `mockups`
   - run:
     `powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_ui_loading_start_preflight_candidates.ps1`
   - confirm the validator reports:
     - 8/8 manifest rows
     - 4/4 transparent loading/start sprite rows
     - 4/4 resolution mockups
     - matching hashes and dimensions
     - normalized candidate paths outside `Assets`
     - 0 Unity `.meta` files
   - before any import, compare the mockups against the current main-menu UI
     shell and Qr1 UI/style truth
   - if approved for import review, inspect Sprite import settings and verify:
     - loading/start screen renders at 1920x1080, 1365x768, 1280x720, and
       1024x768 without overlap
     - spinner does not read as starter-cat body art or character replacement
     - text and button placeholders are replaced by real Unity UI state
     - progress frame/fill, spinner, and dots are bound to the intended UI
       elements
     - Console has no errors
   - do not mark the loading/start preflight accepted until screenshot,
     Console, Sprite import, binding, placeholder-state, and low-height
     layout checks all pass

Updated offline validation: Batch 83 is now visible in the P0 asset production
queue and Unity validation checklist as a candidate-complete loading/start
preflight pack. It remains outside `Assets` and has no install approval.

195. P0 Batch 84 result/settlement preflight candidate Unity review:
   - use the Batch 84 candidate packet:
     `design/development/asset_candidates/ui/result_settlement/batch_84_result_settlement_preflight_2026-06-25`
   - confirm the packet contains:
     - `thecat_ui_result_settlement_batch84_manifest.csv`
     - `thecat_ui_result_settlement_batch84_review_sheet_v001.png`
     - `thecat_ui_result_settlement_batch84_contact_sheet_v001.png`
     - `thecat_ui_result_settlement_batch84_candidate_review.md`
     - `thecat_ui_result_settlement_batch84_process_note.md`
     - 7 transparent sprites under `sprites`
     - 4 local mockups under `mockups`
   - run:
     `powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_ui_result_settlement_preflight_candidates.ps1`
   - confirm the validator reports:
     - 11/11 manifest rows
     - 7/7 transparent result/settlement sprite rows
     - 4/4 result/settlement mockups
     - matching hashes, dimensions, contact sheet hash, and review sheet hash
     - normalized candidate paths outside `Assets`
     - 0 Unity `.meta` files
   - before any import, compare the mockups against the current result banners,
     reward rows, settlement surfaces, and Qr1 UI/style truth
   - if approved for import review, inspect Sprite import settings and verify:
     - battle victory and battle defeat screens render at 1920x1080 without
       overlap
     - run cleared renders at 1365x768 without reward/stat/button crowding
     - run failed renders at 1024x768 after Unity-rendered Chinese labels,
       numbers, and buttons are inserted
     - failure screens use the red/purple X stamp, not a tinted success check
     - result panel, reward row, stat chip, divider, and action buttons are
       bound to the intended UI elements
     - Console has no errors
   - do not mark the result/settlement preflight accepted until screenshot,
     Console, Sprite import, binding, text replacement, and low-height layout
     checks all pass

Updated offline validation: Batch 84 is now visible in the P0 asset production
queue and Unity validation checklist as a candidate-complete result/settlement
preflight pack. It remains outside `Assets` and has no install approval.

196. P0 Batch 85 settings/pause preflight candidate Unity review:
   - use the Batch 85 candidate packet:
     `design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25`
   - confirm the packet contains:
     - `thecat_ui_settings_pause_batch85_manifest.csv`
     - `thecat_ui_settings_pause_batch85_review_sheet_v001.png`
     - `thecat_ui_settings_pause_batch85_contact_sheet_v001.png`
     - `thecat_ui_settings_pause_batch85_candidate_review.md`
     - `thecat_ui_settings_pause_batch85_process_note.md`
     - 6 transparent sprites under `sprites`
     - 4 local mockups under `mockups`
   - run:
     `powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_ui_settings_pause_preflight_candidates.ps1`
   - confirm the validator reports:
     - 10/10 manifest rows
     - 6/6 transparent settings/pause sprite rows
     - 4/4 settings/pause mockups
     - matching hashes, dimensions, contact sheet hash, and review sheet hash
     - normalized candidate paths outside `Assets`
     - 0 Unity `.meta` files
   - before any import, compare the mockups against the current dreamglass UI
     shell, Batch 78 settings controls, Batch 79 system icons, and Qr1
     UI/style truth
   - if approved for import review, inspect Sprite import settings and verify:
     - settings main renders at 1920x1080 with Unity-rendered Chinese option
       labels and values
     - audio settings renders at 1365x768 without slider/switch/checkbox
       overlap
     - pause overlay renders at 1280x720 with modal and button z-order intact
     - compact settings renders at 1024x768 without the lower-left key hint
       chip reading as a clickable back button
     - tab, close, back, key hint, slider, switch, and checkbox click targets
       match their intended semantics
     - Console has no errors
   - do not mark the settings/pause preflight accepted until screenshot,
     Console, Sprite import, binding, text replacement, click-target, and
     key-hint semantics checks all pass

Updated offline validation: Batch 85 is now visible in the P0 asset production
queue and Unity validation checklist as a candidate-complete settings/pause
preflight pack. It remains outside `Assets` and has no install approval.

197. P0 Batch 86 dream-route preflight candidate Unity review:
   - use the Batch 86 candidate packet:
     `design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25`
   - confirm the packet contains:
     - `thecat_ui_dream_route_batch86_manifest.csv`
     - `thecat_ui_dream_route_batch86_review_sheet_v001.png`
     - `thecat_ui_dream_route_batch86_contact_sheet_v001.png`
     - `thecat_ui_dream_route_batch86_candidate_review.md`
     - `thecat_ui_dream_route_batch86_process_note.md`
     - `thecat_ui_dream_route_batch86_agent_review_prompt.md`
     - 6 transparent sprites under `sprites`
     - 4 local mockups under `mockups`
   - run:
     `powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_ui_dream_route_preflight_candidates.ps1`
   - confirm the validator reports:
     - 10/10 manifest rows
     - 6/6 transparent dream-route sprite rows
     - 4/4 dream-entry/route-map mockups
     - matching hashes, dimensions, contact sheet hash, review sheet hash,
       review note hash, process note hash, and agent prompt hash
     - normalized candidate paths outside `Assets`
     - 0 Unity `.meta` files
   - before any import, compare the mockups against the current dreamglass UI
     shell, Batch 65 route-map readability accents, route node icons,
     route-card frames, and Qr1 UI/style truth
   - if approved for import review, inspect Sprite import settings and verify:
     - dream route renders at 1920x1080 with Unity-rendered Chinese route
       labels and reward text
     - route branch renders at 1365x768 with selected/current/available/locked
       states distinct
     - boss-pressure renders at 1280x720 without the magenta boss gate
       overpowering final boss icon or route-card text
     - compact route renders at 1024x768 without lower-half route-card, node,
       and path connector crowding
     - route-choice card click targets match their intended semantics
     - route node/path semantics remain readable after final text replacement
     - Console has no errors
   - do not mark the dream-route preflight accepted until screenshot, Console,
     Sprite import, binding, text replacement, node/path semantics,
     click-target, boss gate scale, and 1024x768 density checks all pass

Updated offline validation: Batch 86 is now visible in the P0 asset production
queue and Unity validation checklist as a candidate-complete dream-route
preflight pack. It remains outside `Assets` and has no install approval.

198. P0 Batch 87 battle HUD preflight candidate Unity review:
   - use the Batch 87 candidate packet:
     `design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25`
   - confirm the packet contains:
     - `thecat_ui_battle_hud_batch87_manifest.csv`
     - `thecat_ui_battle_hud_batch87_review_sheet_v001.png`
     - `thecat_ui_battle_hud_batch87_contact_sheet_v001.png`
     - `thecat_ui_battle_hud_batch87_candidate_review.md`
     - `thecat_ui_battle_hud_batch87_process_note.md`
     - `thecat_ui_battle_hud_batch87_agent_review_prompt.md`
     - 6 transparent sprites under `sprites`
     - 4 local mockups under `mockups`
   - run:
     `powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_ui_battle_hud_preflight_candidates.ps1`
   - confirm the validator reports:
     - 10/10 manifest rows
     - 6/6 transparent battle HUD sprite rows
     - 4/4 battle HUD mockups
     - matching hashes, dimensions, contact sheet hash, review sheet hash,
       review note hash, process note hash, and agent prompt hash
     - normalized candidate paths outside `Assets`
     - 0 Unity `.meta` files
   - before any import, compare the mockups against the current dreamglass UI
     shell, core gauge assets, Batch 60 skill HUD feedback, Batch 62/63
     runtime controls, Batch 80 skill icons, Batch 81 skill slot frames, and
     Qr1 UI/style truth
   - if approved for import review, inspect Sprite import settings and verify:
     - battle HUD renders at 1920x1080 with Unity-rendered Chinese labels,
       tooltips, values, and cooldown numbers
     - pressure layout renders at 1365x768 without top rail or bottom tray
       overlap
     - compact layout renders at 1280x720 with skill ready/selected/cooldown/
       disabled/low-resource states readable
     - dense layout renders at 1024x768 with all four core gauges visible
     - enemy spawn and telegraph effects are not hidden by the enemy/status
       panel
     - pause, speed, and restart click targets match their intended semantics
     - Console has no errors
   - do not mark the battle HUD preflight accepted until screenshot, Console,
     Sprite import, binding, text replacement, four-gauge proof, skill-state
     readability, enemy/telegraph occlusion, and click-target checks all pass

Updated offline validation: Batch 87 is now visible in the P0 asset production
queue and Unity validation checklist as a candidate-complete battle HUD
preflight pack. It remains outside `Assets` and has no install approval.

199. P0 Batch 88 character-select preflight candidate Unity review:
   - use the Batch 88 candidate packet:
     `design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25`
   - confirm the packet contains:
     - `thecat_ui_character_select_batch88_manifest.csv`
     - `thecat_ui_character_select_batch88_review_sheet_v001.png`
     - `thecat_ui_character_select_batch88_contact_sheet_v001.png`
     - `thecat_ui_character_select_batch88_candidate_review.md`
     - `thecat_ui_character_select_batch88_process_note.md`
     - `thecat_ui_character_select_batch88_agent_review_prompt.md`
     - 6 transparent sprites under `sprites`
     - 4 local mockups under `mockups`
   - run:
     `powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_ui_character_select_preflight_candidates.ps1`
   - confirm the validator reports:
     - 10/10 manifest rows
     - 6/6 transparent character-select sprite rows
     - 4/4 character-select mockups
     - matching hashes, dimensions, contact sheet hash, review sheet hash,
       review note hash, process note hash, and agent prompt hash
     - normalized candidate paths outside `Assets`
     - 0 Unity `.meta` files
     - provenance tokens for Qr1 UI truth, IAd character truth, current
       Feishu ACL blockers, missing `OPENAI_API_KEY`, and unavailable
       explicit built-in model selector
   - before any import, compare the mockups against Qr1 UI/style truth, the
     current main-menu background, the dreamglass panel, source-locked HUD
     avatars, and Batch 80 symbolic skill icons
   - if approved for import review, inspect Sprite import settings and verify:
     - character select renders at 1920x1080 with Unity-rendered Chinese
       starter names, roles, descriptions, and start labels
     - 1365x768 remains readable without selected-card or stage-panel overlap
     - 1280x720 and 1024x768 preserve low-height card density without hiding
       role chips, starter descriptors, or action controls
     - selected and idle starter-card states remain distinct
     - starter cards and the start action meet click-target expectations
     - source-locked HUD avatars still match the approved avatar references
     - Console has no errors
   - do not mark the character-select preflight accepted until screenshot,
     Console, Sprite import, binding, text replacement, source-lock avatar
     consistency, low-height density, and click-target checks all pass

Updated offline validation: Batch 88 is now visible in the P0 asset production
queue and Unity validation checklist as a candidate-complete character-select
preflight pack. It remains outside `Assets` and has no install approval.

200. P0 Unity offline acceptance rerun after 2026-06-25 gate fixes:
   - read:
     `design/development/asset_review/UNITY_BATCHMODE_OFFLINE_ATTEMPT_2026-06-25.md`
   - confirm the code still treats enemy `framesheet` manifest assets as
     Sprite Single import settings in both runtime meta readiness and editor
     import validation
   - confirm blocked starter-cat review notes keep formal import valid and
     blocked even when active-cat screenshot files exist; screenshots alone do
     not approve import
   - final rerun command:
     `D:\SoftWares\6000.4.10f1\Editor\Unity.exe -batchmode -quit -projectPath D:\Unity Workspace\TheCat -executeMethod TheCat.EditorTools.P0BatchmodeAcceptanceRunner.RunOfflineP0GatesForBatchmode -logFile D:\Unity Workspace\TheCat\design\development\unity_batchmode\P0_OFFLINE_ACCEPTANCE_UNITY_LOG.txt`
   - accepted because `design/development/unity_batchmode/P0_OFFLINE_ACCEPTANCE_REPORT.md`
     has a timestamp of 2026-06-25 08:00 +08:00 and reports:
     - `Result: passed`
     - `Failure count: 0`
     - P0 Asset Imports passed
     - P0 Asset Review Packet passed
     - P0 Offline Asset Production Readiness passed
   - Unity log records `[TheCat] P0 batchmode acceptance passed 6 gate(s)`

Updated offline validation: the stale 07:44 Unity offline report exposed two
gate-rule issues and was preserved as evidence. Code-side fixes are in place:
enemy `framesheet` assets use Sprite Single import settings in runtime/editor
readiness, runtime meta readiness accepts v1/v2 TheCat import markers, and
blocked starter-cat review notes keep formal import valid but blocked even when
active-cat screenshots exist. The fresh 08:00 batchmode report passed all 6
offline gates with 0 failures.

201. P0 Batch 89 skill-selection preflight candidate Unity review:
   - use the Batch 89 candidate packet:
     `design/development/asset_candidates/ui/skill_selection/batch_89_skill_selection_preflight_2026-06-25`
   - confirm the packet contains:
     - `thecat_ui_skill_selection_batch89_manifest.csv`
     - `thecat_ui_skill_selection_batch89_review_sheet_v001.png`
     - `thecat_ui_skill_selection_batch89_contact_sheet_v001.png`
     - `thecat_ui_skill_selection_batch89_candidate_review.md`
     - `thecat_ui_skill_selection_batch89_process_note.md`
     - `thecat_ui_skill_selection_batch89_agent_review_prompt.md`
     - 8 transparent sprites under `sprites`
     - 4 local mockups under `mockups`
   - run:
     `powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_ui_skill_selection_preflight_candidates.ps1`
   - confirm the validator reports:
     - 12/12 manifest rows
     - 8/8 transparent skill-selection sprite rows
     - 4/4 skill-selection mockups
     - matching hashes, dimensions, contact sheet hash, review sheet hash,
       review note hash, process note hash, and agent prompt hash
     - normalized candidate paths outside `Assets`
     - 0 Unity `.meta` files
   - before any import, compare the mockups against Qr1 UI/style truth, Batch
     80 symbolic skill icons, Batch 81 skill slot frames, and current battle
     HUD skill-state semantics
   - if approved for import review, inspect Sprite import settings and verify:
     - skill-selection renders at 1920x1080 with Unity-rendered Chinese labels,
       skill costs, cooldown numbers, and detail text
     - 1365x768 and 1280x720 preserve card rail and detail-panel density
     - 1024x768 remains readable without card, confirm action, or detail text
       crowding
     - selected, ready, disabled, locked, cooldown, low-resource, and no-target
       states are distinct and do not conflict with battle HUD states
     - skill cards, tabs, detail panel, and confirm action meet click-target
       expectations
     - Console has no errors
   - do not mark the skill-selection preflight accepted until screenshot,
     Console, Sprite import, binding, text/number replacement, state semantics,
     low-height density, and click-target checks all pass

Updated offline validation: Batch 89 is now visible in the P0 asset production
queue and Unity validation checklist as a candidate-complete skill-selection
preflight pack. It remains outside `Assets` and has no install approval.

202. P0 Batch 90 cat-room preflight candidate Unity review:
   - use the Batch 90 candidate packet:
     `design/development/asset_candidates/ui/cat_room/batch_90_cat_room_preflight_2026-06-25`
   - confirm the packet contains:
     - `thecat_ui_cat_room_batch90_manifest.csv`
     - `thecat_ui_cat_room_batch90_review_sheet_v001.png`
     - `thecat_ui_cat_room_batch90_contact_sheet_v001.png`
     - `thecat_ui_cat_room_batch90_candidate_review.md`
     - `thecat_ui_cat_room_batch90_process_note.md`
     - `thecat_ui_cat_room_batch90_agent_review_prompt.md`
     - 6 transparent sprites under `sprites`
     - 4 local mockups under `mockups`
   - run:
     `powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_ui_cat_room_preflight_candidates.ps1`
   - confirm the validator reports:
     - 10/10 manifest rows
     - 6/6 transparent cat-room sprite rows
     - 4/4 cat-room mockups
     - matching hashes, dimensions, contact sheet hash, review sheet hash,
       review note hash, process note hash, and agent prompt hash
     - normalized candidate paths outside `Assets`
     - 0 Unity `.meta` files
   - before any import, compare the mockups against Qr1 UI/style truth, the
     existing bedroom map, Batch 54/67 cat-room interaction affordances, and
     the starter-cat source-lock rule
   - if approved for import review, inspect Sprite import settings and verify:
     - cat room renders at 1920x1080 with Unity-rendered labels and status text
     - 1365x768 and 1280x720 preserve bed, feeder, litter, and dream entrance
       click-target space
     - 1024x768 remains readable without blocking core object interactions
     - bed, feeder, litter, and dream entrance prompts are distinct
     - hover, disabled, and range-blocked states read as interaction states
       rather than error states
     - no new starter-cat body art is introduced by the candidate packet
     - Console has no errors
   - do not mark the cat-room preflight accepted until screenshot, Console,
     Sprite import, binding, text replacement, interaction-state semantics,
     click-target checks, and no-new-cat-body verification all pass

Updated offline validation: Batch 90 is now visible in the P0 asset production
queue and Unity validation checklist as a candidate-complete cat-room preflight
pack. It remains outside `Assets` and has no install approval.

203. P0 D1 entry and character-select contract validation:
   - code scope:
     - `P0MainMenuActionCategory` classifies player-primary, graybox route
       helper, graybox battle helper, and utility actions
     - main menu `EnterCatRoom` is the only player-primary CTA
     - direct route starts and quick battle remain available but are not player
       main-path proof
     - starter cards expose selected/idle state, ready badge, skill preview,
       and source-locked HUD avatar ids
     - `P0PlayableReadiness.EntryCharacterSelectCheckId` validates entry ->
       selected roster -> cat room -> bedroom dream route handoff
   - compile:
     - Runtime MSBuild passed
     - EditModeTests MSBuild passed
   - focused Unity EditMode:
     - command shape: keep `-testFilter` after `-logFile`; this Unity CLI
       invocation did not run tests when the filter appeared before
       `-testResults`
     - filter:
       `TheCat.Tests.P0MainMenuCoverageTests;TheCat.Tests.P0PlayableReadinessTests;TheCat.Tests.GameStateMachineTests;TheCat.Tests.P0CatRoomPresenterTests`
     - result: `25/25` passed
     - XML: `Logs/p0_d1_entry_character_select_editmode_20260625.xml`
   - consumer Unity EditMode:
     - `P0CodeSmokeSuiteTests` passed `7/7` inside
       `Logs/p0_d1_entry_character_select_consumers_editmode_20260625.xml`
     - wider consumer batch result: `14` total, `12` passed, `2` failed
     - failures are current starter-cat active screenshot/formal-import
       evidence expectations in architecture/visual acceptance tests, not D1
       player-entry or character-select behavior
   - do not import Batch 88 or unlock starter-cat body replacement from this
     contract-only slice

Updated offline validation: D1 is validated as a code/presenter/readiness
contract. The player path is now main menu/character selection -> cat room ->
bedroom dream route, while route/quick-battle shortcuts remain graybox helpers.

204. P0 D2 pause/settings and skill-selection acceptance validation:
   - code scope:
     - `P0PauseSettingsSurface` covers pause/continue, speed controls, key
       hints, and restart request -> confirm semantics
     - `GrayboxBattleController` routes restart input through confirmation
       before beginning a new run from the pause/settings path
     - `P0RuntimeSettingsCoverage` validates pause overlay, settings scope,
       restart confirmation, keyboard/button parity, and Batch 85 boundary
     - `P0SkillSelectionPresenter` surfaces pending upgrade choices as ready,
       selected, disabled, and locked states
     - `P0SkillSelectionAcceptanceCoverage` validates confirm gating,
       small-skill/ultimate runtime mapping, and Batch 89 boundary
     - `P0PlayableReadiness` includes `Pause Settings Acceptance` and
       `Skill Selection Acceptance`
   - compile:
     - Runtime MSBuild passed
     - EditModeTests MSBuild passed
   - focused Unity EditMode:
     - command shape: keep `-testFilter` after `-logFile` and `-testResults`
       because the first invocation may only refresh scripts
     - filter:
       `TheCat.Tests.P0RuntimeSettingsCoverageTests;TheCat.Tests.P0SkillHudCoverageTests;TheCat.Tests.P0PlayableReadinessTests;TheCat.Tests.P0CodeSmokeSuiteTests;TheCat.Tests.GameStateMachineTests`
     - result: `30/30` passed
     - XML: `Logs/p0_d2_pause_skill_selection_editmode_20260625.xml`
   - do not import Batch 85 or Batch 89 from this contract-only slice
   - do not change skill damage, cooldown, target rules, battle progression, or
     route-map upgrade selection behavior from this slice

Updated offline validation: D2 is validated as a code/presenter/readiness
contract. Batch 85 settings/pause and Batch 89 skill-selection assets remain
candidate-only until Unity screenshot, Console, import settings, binding, and
text replacement checks approve specific install rows.

205. P0 E1 bedroom battle readability validation:
   - code scope:
     - `P0BattlePlayerBriefPresenter` builds the default battle top brief from
       existing prompt, command-deck, route, battle, and threat state
     - default player HUD now shows priority/action/threat before denser
       diagnostic-style detail
     - battle result UI now surfaces continue-route, return-to-cat-room, and
       restart actions before cat/skill controls
     - `P0BattleReadabilityCoverage` validates in-progress brief line caps,
       threat overflow, result actions, and candidate-only Batch text rejection
     - `P0PlayableReadiness` includes `Battle Readability Acceptance`
     - `P0UnityRuntimeValidationPlan.BuildMarkdown()` carries the exact
       `colored-turnaround` starter-cat lock phrase
   - compile:
     - Runtime MSBuild passed
     - EditModeTests MSBuild passed
     - Editor MSBuild passed
   - focused Unity EditMode:
     - filter:
       `TheCat.Tests.P0BattlePlayerBriefPresenterTests;TheCat.Tests.P0BattleHudSummaryPresenterTests;TheCat.Tests.P0BattleHudPromptPresenterTests;TheCat.Tests.P0BattleCommandDeckPresenterTests;TheCat.Tests.P0BattleFeedbackCoverageTests;TheCat.Tests.P0PlayableReadinessTests;TheCat.Tests.P0CodeSmokeSuiteTests;TheCat.Tests.GameStateMachineTests`
     - result: `47/47` passed
     - XML: `Logs/p0_e1_battle_readability_editmode_20260625.xml`
   - Play Mode plan/evidence structure Unity EditMode:
     - filter:
       `TheCat.Tests.P0PlayModeScreenshotSmokeTests;TheCat.Tests.P0PlayModeAcceptanceSmokeTests;TheCat.Tests.P0PlayModeEvidenceChecklistTests;TheCat.Tests.P0UnityRuntimeValidationPlanTests;TheCat.Tests.P0PlayModeScreenshotFileEvidenceTests`
     - result: `17/17` passed
     - XML: `Logs/p0_e1_playmode_plan_editmode_20260625.xml`
   - `git diff --check` passed
   - do not import Batch 85, Batch 89, Batch 90, or any starter-cat body art
     from this code/readiness slice
   - G1 later refreshed the baseline Play Mode screenshots; battle world label
     safe-area / overlay hierarchy remains required before claiming final
     visual acceptance for the battle brief and result-action layout

Updated offline validation: E1 is validated as a code/presenter/readiness
contract. It improves first-view battle readability and result-action access
without changing combat rules, route flow, skill-selection mutation, input
mappings, candidate asset state, or starter-cat body-art locks.

206. P0 F1 UI candidate import-order gate:
   - docs scope:
     - added
       `design/development/asset_review/F1_UI_CANDIDATE_IMPORT_ORDER_GATE_2026-06-25.md`
     - scoped F1 to Batch 83-88 candidate packets
     - confirmed Batch 89 skill-selection and Batch 90 cat-room remain in the
       broader Unity-evidence queue for later UI passes
     - integrated Mendel visual-priority review:
       Batch 88, Batch 87, Batch 86, Batch 84, Batch 85, Batch 83
     - integrated Bernoulli queue/checklist review:
       docs-only gate sufficient, Unity acceptance/import approval not
       sufficient
     - integrated Hypatia runtime-surface review:
       Batch 87, 88, 86, and 84 are screenshot-parity ready; at F1 time,
       Batch 85 was partial and Batch 83 needed a loading/start hook
   - current queue/checklist evidence:
     - queue items: 19
     - candidate-review items: 14
     - Unity-blocked items: 5
     - local validation matrix: 38 passed, 0 failed after Batch 89/90
       validators
   - boundary:
     - no Batch 83-88 PNGs imported into `Assets`
     - no candidate `.meta` files generated
     - no formal install decision written
     - starter-cat body art remains locked
   - next Unity evidence order for ready surfaces:
     - Batch 87 battle HUD screenshots and 1024x768 four-gauge proof
     - Batch 88 character-select screenshots and source-lock avatar proof
     - Batch 86 dream-route route-state screenshots
     - Batch 84 result/settlement screenshots
   - follow-up code hooks:
     - H1 has since added the Batch 85 full settings-screen hook and Batch 83
       loading/start hook
     - both batches still need Unity-rendered screenshot evidence before
       import-review evidence

Updated offline validation: F1 is complete as a candidate-only documentation
gate. H1 later resolved the Batch 83/85 hook gaps identified here, but it does
not approve import, runtime binding, Unity acceptance, or starter-cat body-art
replacement.

207. P0 H1 loading/start and full settings hook validation:
   - code scope:
     - `P0LoadingStartPresenter` exposes loading/start target scene, target
       label, state, progress, spinner, detail rows, and screenshot-hook
       contract for Batch 83 review prep
     - loading/start candidate boundary now deep-scans public surface strings
       and detail rows for `.png`, `.meta`, `Assets/`, candidate paths, and
       Batch 83 slugs
     - `P0RuntimeSettingsPresenter` exposes `P0FullSettingsSurface` with
       combat/controls tabs, six option rows, restart modal state, and
       semantic non-speed values for pause/restart rows
     - full settings candidate boundary now deep-scans tabs, option rows,
       action details, modal text, pause surface, and runtime settings text
       for Batch 85 candidate tokens
     - `P0PlayModeScreenshotSmoke` validates the loading/start hook without
       changing the existing 11-capture screenshot plan
     - `P0PlayableReadiness` includes the loading/start surface and describes
       D2/H1 settings acceptance as pause overlay plus full settings hook
   - independent review:
     - James reviewed Batch 83 loading/start: no P0 issue; hook is preflight
       only and cannot replace Unity-rendered screenshot/import evidence
     - Carson reviewed Batch 85 full settings: no P0 issue; semantic row and
       deep-scan risks were fixed
     - Hilbert reviewed readiness/smoke: no P0 issue; 11-capture plan remains
       unchanged and H1 must stay screenshot/import pending
   - compile:
     - Runtime MSBuild passed
     - EditModeTests MSBuild passed
   - focused Unity EditMode:
     - initial result: `31/31` passed in
       `Logs/p0_h1_loading_settings_editmode_20260625.xml`
     - review-fix result: `33/33` passed in
       `Logs/p0_h1_loading_settings_reviewfix_editmode_20260625.xml`
   - Unity CLI note:
     - omit `-quit` for this project test command; including `-quit` can cause
       Unity to refresh scripts and exit before Test Framework writes XML
   - boundary:
     - no Batch 83 or Batch 85 PNGs imported into `Assets`
     - no candidate `.meta` files generated
     - no Sprite import settings, binding proof, formal install row, Console
       acceptance, or visual acceptance claimed
     - Batch 83 and Batch 85 are now hook-ready, not import-approved

Updated offline validation: H1 is validated as a code/presenter/readiness
contract. Batch 83 loading/start and Batch 85 full settings can proceed to
Unity-rendered screenshot evidence, but formal import still requires screenshot,
Sprite import settings, binding, text replacement, interaction-state, and
Console checks.

208. P0 G1 11-capture Play Mode evidence refresh:
   - evidence scope:
     - refreshed `design/development/screenshots/p0-playmode-smoke` with
       11 screenshots at `2026-06-25 11:54 +08:00`
     - regenerated
       `design/development/unity_batchmode/P0_PLAYMODE_ACCEPTANCE_SMOKE_REPORT.md`
     - regenerated
       `design/development/unity_batchmode/P0_FULL_ACCEPTANCE_REPORT.md`
   - first-run issue:
     - `Logs/P0PlayModeAcceptanceVisual_G1_20260625.log` captured the
       screenshots but then hit a fatal Texture allocation OOM while
       `P0PlayModeScreenshotFileEvidence` decoded PNGs for report evidence
     - this first run is failure/root-cause evidence only and is superseded by
       the retry
     - `P0PlayModeScreenshotFileEvidence.DefaultFileIsUsable()` now destroys
       the transient decoded `Texture2D` immediately so one report generation
       pass does not accumulate full-size screenshot textures
   - focused Unity EditMode:
     - result: `18/18` passed
     - XML: `Logs/p0_g1_screenshot_evidence_editmode_20260625.xml`
   - retry Play Mode acceptance:
     - log: `Logs/P0PlayModeAcceptanceVisual_G1_retry_20260625.log`
     - result: passed
     - evidence: `8/8` checks passed, `0` pending warnings, 11/11 expected
       screenshot files validated
     - route-flow smoke reached `10/10` nodes, `5` battles, and boss observed
     - defeat-flow smoke passed
   - non-blocking log noise:
     - retry log still contains Unity AI/Codex Windows signature collection
       `Out of memory` messages
     - those messages are tooling/environment noise and did not prevent the
       runner from passing
   - independent review:
     - Noether: no P0 blocker; battle world yellow world-space labels and
       top-edge warning text remain P1 visual debt
     - Mencius: retry is the valid G1 evidence; the first OOM is superseded;
       do not treat `P0_FULL_ACCEPTANCE_REPORT.md` as full acceptance
     - Locke: G1 wording must stay limited to 11-capture Play Mode evidence
       refresh and screenshot parity, with no import or final visual acceptance

Updated validation: G1 is complete as a baseline Play Mode screenshot/evidence
refresh. It validates expected capture files and smoke flows, but it does not
approve candidate import, runtime binding, install rows, or final visual polish.
The next visual task should address battle world label safe-area / overlay
hierarchy before final Batch 87 battle-HUD or battle-world acceptance.

209. P0 I1 battle-world label safe-area and overlay hierarchy:
   - code scope:
     - `GrayboxBattleController` gates active-cat, bed, enemy, and enemy-status
       world diagnostic labels behind diagnostics HUD plus in-progress battle
       state
     - `GrayboxEnemyView` and `P0EnemyWarningIndicatorView` preserve active
       warning rings/lines while allowing warning text labels to be hidden in
       the default collapsed HUD
     - warning visuals are hidden after battle outcome before result capture
     - status indicators, warning indicators, graybox markers, and enemy
       fallback renderers avoid EditMode `renderer.material` instantiation
       warnings
     - screenshot and route-flow smoke runners batch fixed simulation ticks per
       frame while preserving the same drive order, delta, and max tick budgets
   - focused Unity EditMode:
     - result: `19/19` passed
     - XML: `Logs/p0_i1_world_label_overlay_final4_editmode_20260625.xml`
   - Play Mode acceptance:
     - log: `Logs/P0PlayModeAcceptanceVisual_I1_final_20260625.log`
     - result: passed
     - evidence: `8/8` checks passed, `0` pending warnings, 11/11 expected
       screenshot files validated
     - screenshots refreshed at `2026-06-25 21:22 +08:00`
   - independent review:
     - Nietzsche: no P0/P1 screenshot finding; world-space yellow labels are
       gone from the normal battle HUD and result overlay
     - Sartre: no P0 code-boundary finding; direct controller coverage and
       warning material lifecycle risks were addressed before final validation
   - boundary:
     - no candidate PNGs imported into `Assets`
     - no candidate `.meta` files generated
     - no Sprite import settings, binding proof, formal install row, Console
       acceptance, or final candidate visual acceptance claimed

Updated validation: I1 retires the G1 battle-world label / result-overlay P1
visual debt for the current baseline. Batch 87 can proceed to batch-specific
screenshot parity, import-settings, binding, and Console evidence, but no
candidate import is approved by this pass.
