# P0 Architecture And Asset Production Readiness Audit

Date: 2026-06-14

## Verdict

Current architecture is ready to start systematic P0 asset production.

It is not yet complete enough to call the final P0 runtime fully integrated.
The code-side gameplay architecture, asset manifest gates, source-lock checks,
and offline compile path are strong enough for disciplined asset batches. The
remaining risk is Unity editor integration: live importer previews, Console
state, prefab/scene visual wiring, and screenshots still need editor-side
validation once Unity MCP or an interactive editor route is available.

Update 2026-06-15: `P0AssetProductionNextBatchGate` now provides the
asset-production decision layer for the current pre-Unity-review state. It
allows a newly scoped Codex candidate/spec batch outside `Assets`, but keeps
starter-cat body import blocked. The gate composes offline production
readiness, queue coverage, Unity validation checklist readiness, strict
starter-cat candidate identity locks, and formal import state. The allowed lane
is `NewScopedCandidateOrSpecBatchOutsideAssets`; cat body import remains `no`
until exact colored-turnaround locks, active-cat screenshots, explicit review
approval, and Console-clean Unity validation are present.

Update 2026-06-15: Batch 69 adds a starter-cat turnaround/runtime comparison
audit package. The package lives outside `Assets`, compares the three
authoritative colored turnarounds against the current Unity combat sprites, and
records active-cat screenshot targets before any cat sprite can move toward
formal import. `P0StarterCatTurnaroundComparisonAuditEvidence` and
`P0AssetReviewPacket` now make this audit part of the offline P0 asset review
gate. This supports Codex-side systematic asset production while preserving
Unity as the authority for installed sprite validation.

Update 2026-06-15: Batch 70 adds source-derived starter-cat reference plates.
The package crops front, side, and back plates from the locked colored
turnarounds for Saiban, Nephthys, and Suzune, without AI generation or Unity
import. `P0StarterCatReferencePlateEvidence` and `P0AssetReviewPacket` now make
these nine plates part of the offline P0 asset review gate, giving future
Codex-side cat image generation a stricter hard-reference input set.

Update 2026-06-15: Batch 71 installs a Saiban Unity-side debug reference atlas
derived directly from the Batch 70 front/side/back plates. It is tracked as a
`reference_atlas` manifest asset with Saiban source-lock coverage and Sprite
import settings, but it is intentionally not runtime-bound and does not replace
the existing Saiban combat sprite. `P0StarterCatUnityReferenceInstallEvidence`
and `P0AssetReviewPacket` now expose this as a separate source-reference
installation gate, preserving the formal starter-cat body-art import block.

Update 2026-06-15: Batch 72 and Batch 73 complete the same Unity-side debug
reference atlas installation for Nephthys and Suzune. The three starter cats
now each have a source-derived `reference_atlas` in
`Assets/TheCat/Art/Characters/References`, with matching `.meta` files,
manifest rows, review sheets, and source-lock evidence. The gate remains
debug-reference-only: no starter-cat combat sprite, HUD avatar, source
turnaround, Batch 70 reference plate, or runtime visual binding was replaced.
Unity AssetDatabase refresh, import-setting inspection, Console check, and
active-cat screenshot comparison remain the live-editor validation gate.

Update 2026-06-15: The existing `P0AssetUnityValidationChecklist` tool and its
EditMode tests are now included in the current MSBuild project files. This
removes a compile-visibility gap where the checklist file existed on disk but
was not visible to the offline Runtime/EditMode build path.

Update 2026-06-14: Boss visual assets now have a runtime data-reference layer.
`P0VisualAssetReference` and `P0VisualAssetCatalog` expose generated manifest
rows to route-map and battle-warning presenters, so the Call Tyrant route icon
and warning VFX are no longer just filesystem assets. This is still not final
SpriteRenderer/UI binding; Unity scene/prefab rendering validation remains the
next gate.

Update 2026-06-14: Starter cat combat sprites and four-core HUD icons are now
included in the same runtime visual binding catalog. The three starter cat
manifest rows also declare explicit colored-turnaround source locks, and the
hard-reference gate now covers the three colored turnarounds alongside enemy,
Boss, and bedroom source files.

Update 2026-06-14: The runtime visual catalog now covers 15 P0 slots, including
starter cats, Black Mud, Cold Light, Call Tyrant, bed, litter box, feeder,
four-core HUD icons, Boss route node icon, and Call Tyrant warning VFX.
`P0WorldVisualAssetView` provides the first reusable SpriteRenderer path for
graybox battle objects, with primitive fallbacks when Sprite resolution fails.
This makes code-side asset wiring ready for systematic batches, but Unity
Play Mode screenshots are still required before accepting presentation quality.

Update 2026-06-14: The five P0 status tag icons are now formal single-image
runtime assets, source-split from the locked 5x64 status icon style sample.
`P0VisualAssetCatalog.GetStatusIcon(statusTagId)` maps Sleep Stable, Slow,
Knockback, Mark, and Shield to status HUD bindings, raising the runtime visual
catalog from 15 to 20 slots and the generated manifest from 19 to 24 assets.

Update 2026-06-14: The Bedroom Dream battle background is now a source-locked
runtime background asset derived from `bedroom_dream_map_concept.png`.
`background.bedroom_dream` raises the runtime visual catalog from 20 to 21
slots and the generated manifest from 24 to 25 assets. This is a non-cat
asset pass; starter cat formal import remains blocked until the active-cat
screenshots match the colored three-view turnaround contact sheet.

Update 2026-06-14: Status HUD entries now carry icon asset references for each
active status tag, and `GrayboxBattleController` draws those icons inline in
the IMGUI Status HUD section. `P0StatusHudCoverage` now gates the five generated
status icons as HUD-visible data, while Unity screenshot validation remains the
next live-editor proof.

Update 2026-06-14: `P0AssetProductionReadiness` now provides an offline
go/no-go gate for the next systematic asset batch. It composes manifest
coverage, batch ordering, PNG/meta import readiness, runtime visual bindings,
asset review packet counts, hard reference locks, starter cat colored-turnaround
locks, and the starter cat contact sheet. It is wired into batchmode offline
acceptance as `P0 Offline Asset Production Readiness`.

Update 2026-06-14: The runtime visual bindings now have an offline contact
sheet evidence artifact:
`design/development/asset_review/p0_runtime_visual_contact_sheet_2026-06-14.png`.
`P0AssetProductionReadiness` now requires this sheet, raising the offline
production gate to 13 covered checks. This gives asset review a visual baseline
without weakening the starter-cat formal import block.

Update 2026-06-14: Play Mode evidence now requires the runtime visual contact
sheet and screenshot file evidence as first-class checks.
`P0PlayModeScreenshotSmoke` locks the expected runtime visual binding ids to
`P0VisualAssetCatalog.CreateP0RuntimeBindings`, and
`P0PlayModeAcceptanceSmoke` now expects 8 evidence checks: screenshot plan,
runtime visual screenshot plan, runtime visual contact sheet, screenshot file
evidence, Unity runtime validation plan, screenshot smoke, route flow, and
defeat flow.

Update 2026-06-14: Batch 06 route node icons expand the runtime visual baseline
from 21 to 28 bindings. Defense, Elite, Partner, Shop, Dream Event, Blessing
Offering, and Rest Nest now have deterministic 128px route-map icon assets,
while the Boss node continues to use the Call Tyrant source-locked icon.
`P0RouteNodePresenter` now exposes icons for all 8 P0 route node types.

Update 2026-06-14: Batch 08 UI shell assets expand the generated/import-ready
manifest from 32 to 38 rows and the runtime visual baseline from 28 to 34
bindings. The new slots cover main menu background, title logo, shared
dreamglass panel frame, primary button frame, and settlement fish treat / dream
shard reward icons. This is non-cat UI production; starter-cat formal import
remains blocked by the colored three-view turnaround gate and missing active-cat
Play Mode screenshots.

Update 2026-06-14: Batch 09 battle feedback VFX assets expand the
generated/import-ready manifest from 38 to 44 rows and the runtime visual
baseline from 34 to 40 bindings. The new slots cover hit spark, bed shield
pulse, Sleep Stable wave, litter cleanse, feeder kibble, and enemy mark ring
feedback. This is non-cat VFX production; starter-cat formal import remains
blocked by the colored three-view turnaround gate and missing active-cat Play
Mode screenshots.

Update 2026-06-14: Batch 10 enemy warning VFX assets expand the
generated/import-ready manifest from 44 to 48 rows and the runtime visual
baseline from 40 to 44 bindings. The new `battle_warning` slots cover Black
Mud bed claw pressure, Cold Light beam pressure, Call Tyrant app throw, and
Call Tyrant summon portal warnings. The four new assets are source-locked to
`black_mud_animation`, `cold_light_animation`, and `call_tyrant_animation`.
This is non-cat enemy/Boss VFX production; starter-cat formal import remains
blocked by the colored three-view turnaround gate and missing active-cat Play
Mode screenshots.

Update 2026-06-14: Batch 11 enemy animation framesheets expand the
generated/import-ready manifest from 48 to 51 rows and the runtime visual
baseline from 44 to 47 bindings. The new `battle_world` animation slots cover
Black Mud movement, Cold Light casting pressure, and Call Tyrant Boss pattern /
APP throw framesheets. The three new assets are source-cropped from locked
animation sheets and source-locked to `black_mud_animation`,
`cold_light_animation`, and `call_tyrant_animation`. This is non-cat enemy/Boss
animation production; starter-cat formal import remains blocked by the colored
three-view turnaround gate and missing active-cat Play Mode screenshots.

Update 2026-06-14: Batch 12 route choice icons expand the
generated/import-ready manifest from 51 to 57 rows and the runtime visual
baseline from 47 to 53 bindings. The new `route_choice` slots cover partner
recruit, purchase supply, authority blessing, authority upgrade, rest supply,
and dream-event modifier choice icons. The route-map reward-choice surface now
draws these manifest-backed icons through `P0VisualAssetCatalog` and
`RouteMapController`. This is non-cat route/reward UI production; starter-cat
formal import remains blocked by the colored three-view turnaround gate and
missing active-cat Play Mode screenshots.

Update 2026-06-14: Batch 13 route reward card frames expand the
generated/import-ready manifest from 57 to 62 rows and the runtime visual
baseline from 53 to 58 bindings. The new `route_reward_card` slots cover
partner, shop, blessing, dream-event, and rest-nest reward card frames.
`P0RouteMapPresenter` now carries card-frame references into reward-choice
cards, and `RouteMapController` draws framed route choices through the IMGUI
asset path. This is non-cat route/reward UI production; starter-cat formal
import remains blocked by the colored three-view turnaround gate and missing
active-cat Play Mode screenshots.

Update 2026-06-14: Batch 14 status compact icons expand the
generated/import-ready manifest from 62 to 67 rows and the runtime visual
baseline from 58 to 63 bindings. The new `status_compact` slots cover
Sleep Stable, Slow, Knockback, Mark, and Shield as 32px HUD row icons derived
from accepted 64px status icons. `P0StatusHudPresenter` now carries full and
compact icon references, and `GrayboxBattleController` draws the compact icon
first in status HUD rows. This is non-cat status HUD production; starter-cat
formal import remains blocked by the colored three-view turnaround gate and
missing active-cat Play Mode screenshots.

Update 2026-06-14: Batch 15 route reward detail badges expand the
generated/import-ready manifest from 67 to 72 rows and the runtime visual
baseline from 63 to 68 bindings. The new `route_reward_detail` slots cover
gain, cost, recovery, risk, and upgrade effect badges for route reward-choice
cards. `P0VisualAssetCatalog.GetRouteRewardDetailBadge(choice)` resolves badges
from typed reward payloads, and the graybox route-map card draw path renders
the badge on the right side. This is non-cat route/reward UI production;
starter-cat formal import remains blocked by the colored three-view turnaround
gate and missing active-cat Play Mode screenshots.

Update 2026-06-14: The asset baseline counts are now centralized in the
catalog layer: `P0AssetManifestCatalog.P0ManifestAssetCount` and
`P0VisualAssetCatalog.P0RuntimeVisualBindingCount`. Production readiness,
runtime visual binding coverage, screenshot smoke planning, visual acceptance,
review packet text, and tests now reference those baselines instead of
hard-coded batch snapshot counts. This keeps repeated asset batches systematic:
each batch updates the catalog/manifest authority once, then the gates surface
missing files, stale packet counts, and unresolved runtime bindings.

Update 2026-06-14: `P0VisualAcceptanceReport` is now the top-level visual
acceptance readout for asset production planning. It composes playable
readiness, offline asset production readiness, asset review packet readiness,
runtime visual binding coverage, screenshot file evidence, Play Mode evidence,
and starter-cat formal import readiness. Its current expected verdict is:
architecture ready for systematic asset production, final P0 visual acceptance
not ready, and starter-cat formal import still blocked pending active-cat
screenshot review against the colored three-view turnarounds.

Update 2026-06-14: `P0PlayModeScreenshotSmoke` now registers a 10-capture
visual evidence plan. In addition to main menu, route map, battle HUD, first
battle result, and settlement, the Play Mode screenshot smoke must capture
active Saiban, active Nephthys, active Suzune, battle-world visual bindings,
and Call Tyrant warning VFX. The Play Mode evidence checklist now has an
explicit runtime visual screenshot-plan check, so the offline asset gates and
live visual acceptance path align before broad asset production.

Update 2026-06-14: Batch 08 UI shell assets now have a runtime draw-path layer.
`P0UiShellPresenter` exposes the 6 shell assets to main menu and route-map
surfaces, `P0ImGuiVisualAssetDrawer` has shared texture/button helpers, and the
IMGUI main menu / route map now draw the background, logo, dreamglass panel,
primary button, and settlement reward icons. This improves code-side asset
consumption, but Unity screenshot readability validation remains required.

Update 2026-06-14: Starter cat consistency now has a second asset-production
gate beyond SHA/source-lock integrity. `P0StarterCatVisualConsistencyChecklist`
requires Saiban, Nephthys, and Suzune to bind their colored-turnaround source
locks, locked Unity sprite paths, active-cat Play Mode screenshot filenames,
cat-specific visual traits, and explicit drift blockers. This makes generic
cat-style descriptions insufficient for future cat asset acceptance.

Update 2026-06-14: Full acceptance now treats Play Mode evidence as complete
only when there are zero failures and zero pending warnings. `IsUsable` remains
available for diagnostic/offline review, but `P0PlayModeAcceptanceSmoke` and
`P0BatchmodeAcceptanceRunner.RunFullP0AcceptanceForBatchmode` require
`IsComplete`. Pending screenshot, route-flow, or defeat-flow smoke states can no
longer be mistaken for final P0 acceptance.

Update 2026-06-14: Starter cat asset production now has an explicit derivative
spec gate. `P0StarterCatAssetProductionSpec` defines 3 starter-cat production
spec rows, 12 allowed derivative asset types, candidate-vs-approved output
directories, required candidate evidence, strict colored-turnaround prompt
clauses, and rejection rules. This raises `P0AssetProductionReadiness` to 11
covered checks and keeps future cat generation outside `Assets` until reviewed.

Update 2026-06-14: Starter cat formal Unity import now has its own explicit
blocked-or-approved gate. `P0StarterCatFormalImportReadiness` reads the three
Batch 05 per-cat review notes, verifies the candidate pack and production spec,
counts the active-cat Play Mode screenshots, and only allows formal import when
all three notes explicitly approve import after screenshot review. Current
state is valid but blocked: 3/3 review notes explicitly say not to import yet,
and 0/3 active-cat screenshots exist. `P0AssetReviewPacket` and
`P0AssetProductionReadiness` now surface this state, raising their covered
checks to 9 and 12 respectively.

Update 2026-06-14: Current asset baseline after Batch 27 is 103 manifest assets
and 99 runtime visual bindings. Batch 26 adds no runtime art and no Unity
imports; it adds a starter-cat candidate-pack validator and agent prompt so
future Saiban, Nephthys, and Suzune production is forced through review-only
candidate directories before any formal import. Batch 27 adds eight non-cat
core gauge frame/fill assets and wires them into the battle HUD runtime visual
catalog. The existing Batch 05 candidate pack validates with 12 candidate rows,
3 per-cat review notes, and 3 review sheets. The validator also confirms those
candidate PNGs and review sheets stay outside `Assets` and have no Unity
`.meta` files.

## Current Code Shape

Runtime scripts are separated into the intended P0 boundaries:

| Area | File Count | Readiness |
| --- | ---: | --- |
| Combat | 12 | Battle simulation, enemy state, status runtime, warning formatting. |
| Core | 2 | Event bus and object pool foundation. |
| Data | 45 | Prototype catalogs, definitions, four core values, run metrics, visual asset references and runtime visual bindings. |
| Gameplay | 47 | Main menu, route map, graybox battle, HUD presenters, input-facing presenters, UI shell surface, world visual Sprite binding. |
| Input | 3 | P0 keyboard command map. |
| Roguelite | 28 | Ten-layer route, node resolution, blessings, run progression. |
| Tools | 38 | Smoke gates, coverage reports, source locks, asset import readiness, asset production readiness, starter cat production specs, formal cat import gate. |

Editor tooling currently includes 16 P0 menu/validator files:

- Acceptance gate menu.
- Code smoke, playable readiness, golden path, telemetry, status coverage menus.
- Play Mode acceptance, route, defeat, and screenshot smoke menus.
- Scene setup validator.
- Asset import settings validator and applier.

Test coverage currently includes 70 EditMode test files with 407 `[Test]`
markers across combat, core values, status tags, route, HUD, Boss, telemetry,
asset manifest, import readiness, meta settings, source-lock gates, and starter
cat visual consistency gates.

## Architecture Completeness

### Complete Enough For Asset Production

- Data-driven P0 catalogs exist for starter cats, enemies, skills, waves,
  status tags, route nodes, blessings, settlements, asset manifest rows, and
  generation batches.
- `P0VisualAssetCatalog` maps accepted manifest rows into runtime presentation
  references and declares 99 P0 runtime visual bindings for the Bedroom Dream
  battle background, starter cat combat sprites, enemies, enemy framesheets,
  props, four-core HUD icons, four-core gauge bars, status tag icons, compact status HUD icons,
  route nodes, warning VFX, feedback VFX, UI shell assets, settlement rewards,
  route choice icons, route reward card frames, route reward detail badges,
  non-battle choice cards, blessing seals, node-summary banners, and
  battle-result / settlement outcome banners.
- `P0CodeSmokeSuite` aggregates 26 code-side gates, including golden path,
  playable readiness, status/HUD coverage, route map coverage, battle result,
  graybox telemetry, asset manifest coverage, asset generation batch coverage,
  import readiness, meta import settings, starter cat turnaround locks, and
  hard reference source locks.
- `P0AssetProductionReadiness` composes the asset-facing gates into a single
  offline go/no-go report before broader asset production.
- `P0StarterCatVisualConsistencyChecklist` makes colored-turnaround visual
  review explicit: each starter cat must keep source-lock paths, active-cat
  screenshot filenames, and cat-specific trait lists in the gate.
- `P0AssetReviewPacket` now prints the starter cat visual consistency
  checklist and starter cat asset-production spec, so review agents can inspect
  source locks, active screenshots, required traits, allowed derivatives,
  candidate directories, evidence requirements, and rejection rules from one
  packet.
- `P0StarterCatFormalImportReadiness` makes the starter-cat import decision
  explicit. The current gate is healthy but blocked, so systematic asset
  production may continue while formal cat replacements stay outside Unity
  import paths until all three active-cat screenshots are reviewed.
- Starter cat art consistency is guarded by locked colored turnaround source
  hashes and manifest text checks. Future cat sprites must remain tied to the
  colored turnarounds, not to a looser generated style anchor. The visual
  consistency checklist also blocks generic trait lists and wrong active-cat
  screenshot bindings.
- Source-sensitive assets are guarded by 12 hard reference source locks: the
  three starter cat colored turnarounds, Black Mud concept/animation, Cold
  Light concept/animation, Call Tyrant concept/animation, and Bedroom Dream
  map/sprite sheets.
- Manifest rows now include `source_lock_ids`, so source authority is explicit
  instead of hidden in notes or inferred from visual references.
- Generated/imported PNG rows are checked for file presence, expected
  dimensions, `.png.meta` presence, Sprite/Default import settings, and the
  `TheCatP0ImportSettings:v1` marker.
- Asset batches are ordered and tested from style anchors through Batch 27 core
  gauge bars. Batch 26 adds a review-only starter-cat candidate gate without
  adding runtime art.

### Not Yet Complete For Final P0

- Unity MCP tools are not exposed in the current session, so this audit cannot
  claim live editor Console, importer preview, Play Mode, or screenshot
  validation.
- Starter cat formal import is intentionally blocked. The three Batch 05
  candidate packs have review evidence, but none may replace Unity cat sprites
  until `04-active-cat-saiban.png`, `05-active-cat-nephthys.png`, and
  `06-active-cat-suzune.png` exist and are approved against the colored
  three-view turnaround contact sheet.
- The current visual layer is still not final. Generated PNGs now have IMGUI
  preview and battle-world SpriteRenderer binding, but most HUD widgets,
  prefabs, final scene composition, and screenshot-verified placement still
  need Unity-side work. The registered 10-capture Play Mode plan defines this
  evidence, but the screenshots still need to be produced in Unity.
- The project has three P0 scenes in Build Settings, but scene validation still
  needs an editor-side run to prove controller references and runtime flow in
  Unity.
- The CSV manifest and `P0AssetManifestCatalog` are still maintained in
  parallel. Smoke gates reduce drift risk, but a later exporter/importer would
  be healthier if asset volume grows.
- There is no single production CLI for batch generation, preview contact
  sheets, magenta cleanup, meta creation, manifest/catalog update, and review
  packaging. The process is defined, but still partly manual.

## Asset Manifest State

Current P0 manifest has 103 rows after the Batch 27 core gauge bar pass:

- 103 generated/import-ready workspace PNG assets.
- 0 planned assets.

Bedroom Dream battle background asset:

| Asset | Type | Source Authority |
| --- | --- | --- |
| `thecat_bg_bedroomdream_battle_1920x1080_v001` | background | `bedroom_map_concept` |

Batch 3 generated assets:

| Asset | Type | Source Authority |
| --- | --- | --- |
| `thecat_enemy_calltyrant_concept_2048_v001` | concept | `call_tyrant_concept` |
| `thecat_vfx_calltyrant_warning_512_v001` | vfx | `call_tyrant_animation` |
| `thecat_ui_route_bossnode_icon_128_v001` | icon | `call_tyrant_concept` |

Batch 10 enemy warning VFX assets:

| Asset | Type | Source Authority |
| --- | --- | --- |
| `thecat_vfx_blackmud_bed_claw_256_v001` | vfx | `black_mud_animation` |
| `thecat_vfx_coldlight_beam_warning_256_v001` | vfx | `cold_light_animation` |
| `thecat_vfx_calltyrant_app_throw_256_v001` | vfx | `call_tyrant_animation` |
| `thecat_vfx_calltyrant_summon_portal_256_v001` | vfx | `call_tyrant_animation` |

Batch 11 enemy animation framesheet assets:

| Asset | Type | Source Authority |
| --- | --- | --- |
| `thecat_enemy_blackmud_move_framesheet_4x256_v001` | framesheet | `black_mud_animation` |
| `thecat_enemy_coldlight_cast_framesheet_4x256_v001` | framesheet | `cold_light_animation` |
| `thecat_enemy_calltyrant_bosspattern_framesheet_4x256_v001` | framesheet | `call_tyrant_animation` |

Batch 12 route choice icon assets:

| Asset | Type | Source Authority |
| --- | --- | --- |
| `thecat_ui_choice_partner_recruit_icon_128_v001` | icon | `route_choice_icon_batch_12` |
| `thecat_ui_choice_purchase_supply_icon_128_v001` | icon | `route_choice_icon_batch_12` |
| `thecat_ui_choice_authority_blessing_icon_128_v001` | icon | `route_choice_icon_batch_12` |
| `thecat_ui_choice_authority_upgrade_icon_128_v001` | icon | `route_choice_icon_batch_12` |
| `thecat_ui_choice_rest_supply_icon_128_v001` | icon | `route_choice_icon_batch_12` |
| `thecat_ui_choice_dreamevent_modifier_icon_128_v001` | icon | `route_choice_icon_batch_12` |

Batch 13 route reward card frame assets:

| Asset | Type | Source Authority |
| --- | --- | --- |
| `thecat_ui_routecard_partner_frame_512x256_v001` | frame | `route_reward_card_frame_batch_13` |
| `thecat_ui_routecard_shop_frame_512x256_v001` | frame | `route_reward_card_frame_batch_13` |
| `thecat_ui_routecard_blessing_frame_512x256_v001` | frame | `route_reward_card_frame_batch_13` |
| `thecat_ui_routecard_dreamevent_frame_512x256_v001` | frame | `route_reward_card_frame_batch_13` |
| `thecat_ui_routecard_restnest_frame_512x256_v001` | frame | `route_reward_card_frame_batch_13` |

Batch 14 status compact icon assets:

| Asset | Type | Source Authority |
| --- | --- | --- |
| `thecat_ui_status_sleep_stable_32_v001` | icon | `thecat_ui_status_sleep_stable_64_v001` |
| `thecat_ui_status_slow_32_v001` | icon | `thecat_ui_status_slow_64_v001` |
| `thecat_ui_status_knockback_32_v001` | icon | `thecat_ui_status_knockback_64_v001` |
| `thecat_ui_status_mark_32_v001` | icon | `thecat_ui_status_mark_64_v001` |
| `thecat_ui_status_shield_32_v001` | icon | `thecat_ui_status_shield_64_v001` |

Batch 15 route reward detail badge assets:

| Asset | Type | Source Authority |
| --- | --- | --- |
| `thecat_ui_reward_detail_gain_badge_192x64_v001` | badge | `route_reward_detail_badge_batch_15` |
| `thecat_ui_reward_detail_cost_badge_192x64_v001` | badge | `route_reward_detail_badge_batch_15` |
| `thecat_ui_reward_detail_recovery_badge_192x64_v001` | badge | `route_reward_detail_badge_batch_15` |
| `thecat_ui_reward_detail_risk_badge_192x64_v001` | badge | `route_reward_detail_badge_batch_15` |
| `thecat_ui_reward_detail_upgrade_badge_192x64_v001` | badge | `route_reward_detail_badge_batch_15` |

Status icon split assets:

| Asset | Type | Source Authority |
| --- | --- | --- |
| `thecat_ui_status_sleep_stable_64_v001` | icon | `thecat_style_status_icons_5x64_v001` cell 0 |
| `thecat_ui_status_slow_64_v001` | icon | `thecat_style_status_icons_5x64_v001` cell 1 |
| `thecat_ui_status_knockback_64_v001` | icon | `thecat_style_status_icons_5x64_v001` cell 2 |
| `thecat_ui_status_mark_64_v001` | icon | `thecat_style_status_icons_5x64_v001` cell 3 |
| `thecat_ui_status_shield_64_v001` | icon | `thecat_style_status_icons_5x64_v001` cell 4 |

Batch 06 route node icon assets:

| Asset | Type | Source Authority |
| --- | --- | --- |
| `thecat_ui_route_defense_icon_128_v001` | icon | `route_node_icon_batch_06` |
| `thecat_ui_route_elite_icon_128_v001` | icon | `route_node_icon_batch_06` |
| `thecat_ui_route_partner_icon_128_v001` | icon | `route_node_icon_batch_06` |
| `thecat_ui_route_shop_icon_128_v001` | icon | `route_node_icon_batch_06` |
| `thecat_ui_route_dreamevent_icon_128_v001` | icon | `route_node_icon_batch_06` |
| `thecat_ui_route_blessing_icon_128_v001` | icon | `route_node_icon_batch_06` |
| `thecat_ui_route_restnest_icon_128_v001` | icon | `route_node_icon_batch_06` |

Batch 07 starter cat source-lock packet artifacts:

- `design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.md`
- `design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.csv`
- `design/development/tools/build_starter_cat_source_lock_packet.ps1`

Batch 08 UI shell assets:

| Asset | Type | Source Authority |
| --- | --- | --- |
| `thecat_ui_mainmenu_dreamentry_bg_1920x1080_v001` | background | `thecat_style_bedroomdream_anchor_1920x1080_v001` |
| `thecat_ui_title_logo_512x256_v001` | frame | `thecat_style_bedroomdream_anchor_1920x1080_v001` |
| `thecat_ui_panel_dreamglass_512x256_v001` | frame | `thecat_style_status_icons_5x64_v001` |
| `thecat_ui_button_primary_384x96_v001` | frame | `thecat_style_status_icons_5x64_v001` |
| `thecat_ui_reward_fishtreat_icon_128_v001` | icon | `thecat_style_status_icons_5x64_v001` |
| `thecat_ui_reward_dreamshard_icon_128_v001` | icon | `thecat_style_status_icons_5x64_v001` |

## Validation Evidence

Commands run during this audit:

- `dotnet build TheCat.Runtime.csproj -v:minimal` failed because no .NET SDK is
  installed on this machine, not because of project source errors.
- Visual Studio MSBuild compile succeeded for EditMode tests:
  `TheCat.EditModeTests -> Temp/Bin/AssetBindingEditMode2/TheCat.EditModeTests.dll`
- Visual Studio MSBuild compile succeeded for Runtime:
  `TheCat.Runtime -> Temp/Bin/AssetBindingRuntime2/TheCat.Runtime.dll`
- Visual Studio MSBuild compile succeeded for Runtime after world visual
  binding:
  `TheCat.Runtime -> Temp/Bin/RuntimeWorldVisual/TheCat.Runtime.dll`
- Visual Studio MSBuild compile succeeded for EditMode tests after world visual
  binding:
  `TheCat.EditModeTests -> Temp/Bin/EditModeWorldVisual/TheCat.EditModeTests.dll`
- Visual Studio MSBuild compile succeeded for Runtime after the asset production
  readiness gate:
  `TheCat.Runtime -> Temp/Bin/RuntimeAssetProductionGate/TheCat.Runtime.dll`
- Visual Studio MSBuild compile succeeded for Editor after the asset production
  readiness gate:
  `TheCat.Editor -> Temp/Bin/EditorAssetProductionGate/TheCat.Editor.dll`
- Visual Studio MSBuild compile succeeded for EditMode tests after the asset
  production readiness gate:
  `TheCat.EditModeTests -> Temp/Bin/EditModeAssetProductionGate/TheCat.EditModeTests.dll`
- Visual Studio MSBuild compile succeeded for Runtime after UI shell runtime
  draw-path wiring:
  `TheCat.Runtime -> Temp/Bin/RuntimeUiShellWiring/TheCat.Runtime.dll`
- Visual Studio MSBuild compile succeeded for EditMode tests after UI shell
  runtime draw-path wiring:
  `TheCat.EditModeTests -> Temp/Bin/EditModeUiShellWiring/TheCat.EditModeTests.dll`
- Visual Studio MSBuild compile succeeded for Editor after UI shell runtime
  draw-path wiring:
  `TheCat.Editor -> Temp/Bin/EditorUiShellWiring/TheCat.Editor.dll`
- Visual Studio MSBuild compile succeeded for Runtime after the status icon
  split:
  `TheCat.Runtime -> Temp/Bin/RuntimeStatusIcons/TheCat.Runtime.dll`
- Visual Studio MSBuild compile succeeded for EditMode tests after the status
  icon split:
  `TheCat.EditModeTests -> Temp/Bin/EditModeStatusIcons/TheCat.EditModeTests.dll`
- Visual Studio MSBuild compile succeeded for Runtime after the starter cat
  formal import gate:
  `TheCat.Runtime -> Temp/Bin/Debug/TheCat.Runtime/TheCat.Runtime.dll`
- Visual Studio MSBuild compile succeeded for EditMode tests after the starter
  cat formal import gate:
  `TheCat.EditModeTests -> Temp/Bin/Debug/TheCat.EditModeTests/TheCat.EditModeTests.dll`
- Visual Studio MSBuild compile succeeded for Runtime after the Bedroom Dream
  battle background runtime binding:
  `TheCat.Runtime -> Temp/Bin/Debug/TheCat.Runtime/TheCat.Runtime.dll`
- Visual Studio MSBuild compile succeeded for EditMode tests after the Bedroom
  Dream battle background runtime binding:
  `TheCat.EditModeTests -> Temp/Bin/Debug/TheCat.EditModeTests/TheCat.EditModeTests.dll`
- Visual Studio MSBuild compile succeeded for Runtime after the Batch 07
  starter cat source-lock packet gate:
  `TheCat.Runtime -> Temp/Bin/RuntimeStarterCatSourceLockPacket/TheCat.Runtime.dll`
- Visual Studio MSBuild compile succeeded for EditMode tests after the Batch 07
  starter cat source-lock packet gate:
  `TheCat.EditModeTests -> Temp/Bin/EditModeStarterCatSourceLockPacket/TheCat.EditModeTests.dll`
- Visual Studio MSBuild compile succeeded for Runtime after the Batch 14 status
  compact icon pass:
  `TheCat.Runtime -> Temp/bin/RuntimeBatch14StatusCompactFinal/TheCat.Runtime.dll`
- Visual Studio MSBuild compile succeeded for EditMode tests after the Batch 14
  status compact icon pass:
  `TheCat.EditModeTests -> Temp/bin/EditModeBatch14StatusCompactFinal/TheCat.EditModeTests.dll`
- Visual Studio MSBuild compile succeeded for Editor after the Batch 14 status
  compact icon pass:
  `TheCat.Editor -> Temp/bin/EditorBatch14StatusCompactFinal/TheCat.Editor.dll`
- Batch14 asset check passed: five compact status PNGs are `32x32`, all five
  `.png.meta` files contain `TheCatP0ImportSettings:v1`, and the manifest has
  67 rows.
- Batch15 asset check passed: five route reward detail badge PNGs are
  `192x64`, all five `.png.meta` files contain
  `TheCatP0ImportSettings:v1` plus
  `batch:p0_asset_batch_15_route_reward_detail_badges`, and the manifest has
  72 rows.
- Visual Studio MSBuild compile succeeded for Runtime after the Batch 15 route
  reward detail badge pass:
  `TheCat.Runtime -> Temp/bin/RuntimeBatch15RouteRewardDetailsFinal/TheCat.Runtime.dll`
- Visual Studio MSBuild compile succeeded for EditMode tests after the Batch
  15 route reward detail badge pass:
  `TheCat.EditModeTests -> Temp/bin/EditModeBatch15RouteRewardDetailsFinal/TheCat.EditModeTests.dll`
- Visual Studio MSBuild compile succeeded for Editor after the Batch 15 route
  reward detail badge pass, with the known `MSB3277` warning:
  `TheCat.Editor -> Temp/bin/EditorBatch15RouteRewardDetailsFinal/TheCat.Editor.dll`
- `git diff --check` passed.
- CSV workspace file scan reports 72 generated/imported assets and 0 missing
  files.

Latest logged asset smoke evidence before editor-side import validation:

- `P0 code smoke suite is expected to pass 26 check(s) with 0 warning(s).`
- `P0PlayModeScreenshotSmoke.HasP0ScreenshotCapturePlan()` is expected to cover
  10 screenshots, including three active cat visuals, battle-world visual
  bindings, and Call Tyrant warning VFX.
- `P0PlayModeScreenshotSmoke.HasRuntimeVisualScreenshotCapturePlan()` is
  expected to cover all 68 runtime visual binding ids and reference the runtime
  visual contact sheet.
- `P0PlayModeAcceptanceSmoke` is expected to report 8 evidence checks,
  including the Unity runtime validation plan, runtime visual contact sheet,
  and screenshot file evidence.
- `P0AssetProductionReadiness` is expected to report 14 covered checks,
  including the starter cat visual consistency checklist and asset-production
  spec, starter cat source-lock packet, starter cat formal import readiness,
  and runtime visual contact sheet evidence.
- `P0StarterCatSourceLockPacketEvidence` is expected to report ready with 3/3
  cat entries, source hash mentions, sprite hash mentions, active screenshot
  mentions, and candidate review sheet mentions.
- `P0StarterCatFormalImportReadiness` is expected to report state `Blocked`,
  import allowed `no`, 3/3 explicit block notes, 0/3 approval notes, and 0/3
  active-cat screenshots until the Unity screenshot pass is regenerated and
  reviewed.
- `P0AssetProductionReadiness` is expected to report runtime visual contact
  sheet present `yes`.
- `Asset Import Readiness` is expected to report 72 workspace files and 0
  planned assets after Unity-side refresh.
- `Asset Meta Import Settings` is expected to cover all 72 generated/imported
  assets after Unity-side refresh.
- `Hard Reference Source Locks` is expected to report 12 source files and 19
  manifest asset links after Unity-side refresh.

## Asset Production Go/No-Go

Go for systematic P0 asset production under the current gates.

Do not treat new assets as final until the batch also passes Unity editor import
preview and in-scene/HUD screenshot review.

Do not formally import or replace starter cat assets until
`P0StarterCatFormalImportReadiness` reports `Approved`. Candidate cat outputs
may be produced and reviewed outside `Assets`, but the current approved import
status is explicitly `Blocked`.

Every asset batch must follow this contract:

1. Read the design docs, art pipeline, manifest row, generation prompt, and
   source images before generating or extracting pixels.
2. For cats, match the locked colored turnaround first. A generated lineup or
   style anchor is secondary.
3. For enemies, Boss, VFX, and bedroom props, match the locked source image
   first and include exact `source_lock_ids`.
4. Write PNGs only to the manifest import path after acceptance.
5. Add or verify `.png.meta` with the P0 import marker.
6. Update both `P0_ASSET_MANIFEST.csv` and `P0AssetManifestCatalog`.
7. Keep rejected outputs outside `Assets` with a mismatch reason.
8. Update the development log with source references, deviations, validation,
   and next tasks.
9. Run compile and smoke/import checks before moving to the next batch.

## Recommended Next Asset Batches

### Batch 3: Boss Readiness

Batch 3 has generated the remaining P0 manifest assets from locked Call Tyrant
source images. Next validation is editor-side import preview, route-map icon
placement, battle warning VFX placement, and screenshot review.

### Batch 4: Runtime Visual Wiring

After Batch 3, wire generated P0 assets into the runtime presentation layer:

- Four-core HUD icons.
- Five P0 status tag icons.
- Starter cat combat sprites.
- Black Mud and Cold Light sprites.
- Bed, litter box, and feeder props.
- Boss route icon and warning VFX.
- These are now data-bound through `P0VisualAssetCatalog`; cats, enemies, and
  props also have a battle-world SpriteRenderer path through
  `P0WorldVisualAssetView`. Next is Unity Sprite/UI rendering validation and
  screenshot proof.

This batch should end with Unity editor validation and screenshots.

### Batch 5: Production Polish

Only after the first wired screenshots:

- Replace opaque/rough source crops where needed.
- Add animation frame sheets or simple tween-ready variants.
- Tighten palettes and silhouettes against reference turnarounds/source art.
- Add missing P1 assets only after the P0 flow remains green.

## 2026-06-14 Runtime Visual Preview Update

The architecture now has a minimal runtime-facing visual path for asset
production review:

- `P0VisualAssetCatalog.CreateP0RuntimeBindings` defines the 21 P0 visual slots
  that must be visible in the graybox loop before a batch can be considered
  wired, including the source-locked Bedroom Dream battle background.
- `P0VisualAssetTextureResolver` resolves those manifest-backed PNGs inside the
  Unity editor as textures and Sprites.
- `P0ImGuiVisualAssetDrawer` draws the resolved textures in route map, battle
  core values, cat HUD, and warning previews.
- `P0StatusHudPresenter` and `GrayboxBattleController` now surface generated
  status tag icons inside the Status HUD rows.
- `P0WorldVisualAssetView` renders manifest-backed Sprites on battle-world
  objects while retaining graybox fallbacks.
- `GrayboxBattleController`, `GrayboxEnemyView`, and
  `P0EnemyWarningIndicatorView` now consume the world visual path for cats,
  enemies, props, and Call Tyrant warning VFX.
- `P0RuntimeVisualBindingCoverage` is now part of `P0CodeSmokeSuite`, raising
  the architecture smoke suite to 25 checks.
- `P0AssetReviewPacket` is now part of `P0CodeSmokeSuite`, raising the smoke
  suite to 26 checks and producing a single review packet for manifest rows,
  source-lock paths, runtime visual bindings, and starter-cat turnaround
  evidence.
- `P0BatchmodeAcceptanceRunner` now provides command-line Unity entry points
  for offline gates and full acceptance, with Markdown reports under
  `design/development/unity_batchmode`.
- `P0AssetProductionReadiness` now gives asset agents one offline gate to run
  before starting the next controlled batch.

Go for systematic P0 asset production remains conditional:

- Yes for source-locked, manifest-first batch production.
- Yes for assets that can be represented in the review packet before
  acceptance.
- Yes for starter-cat candidate review outside `Assets`.
- No for uncontrolled bulk generation.
- No for formal starter-cat Unity import while the formal import gate is
  `Blocked`.
- No for claiming final presentation quality until Unity screenshots prove the
  assets render correctly in scene.

## 2026-06-14 Screenshot Evidence Gate Update

Architecture code now includes an offline file-evidence check for the Play Mode
screenshot plan:

- `P0PlayModeScreenshotFileEvidence` compares
  `design/development/screenshots/p0-playmode-smoke` against the 10 expected
  files from `P0PlayModeScreenshotSmoke`.
- `P0AssetReviewPacket` includes the current evidence state so asset review can
  see whether screenshots are complete, missing, or stale.
- Current evidence is not complete: 3/10 expected screenshots exist, 7 expected
  screenshots are missing, and old `04-settlement.png` is an unexpected stale
  file.

This means the code architecture is ready to support controlled asset
production, but runtime visual acceptance is not complete until Unity Play Mode
regenerates and reviews the full 10-screenshot set.

## 2026-06-14 Starter Cat Formal Import Gate Update

Starter cat formal import is now a separate code gate:

- `P0StarterCatFormalImportReadiness` reads the three Batch 05 review notes and
  requires a consistent explicit decision: all blocked, or all approved.
- Approval requires all three active-cat screenshots to exist before import is
  allowed.
- The current state is valid and blocked: 3/3 review notes explicitly block
  import, 0/3 review notes approve import, and 0/3 active-cat screenshots are
  present.
- `P0AssetReviewPacket` now includes a `Starter Cat Formal Import Readiness`
  section with state, import allowance, note counts, and active screenshot
  count.
- `P0AssetProductionReadiness` now treats the formal import gate as required
  evidence, but it does not require approval before non-cat production or cat
  candidate review can continue.

This is the clean boundary for the next phase: start systematic asset output,
but keep starter-cat candidates outside formal Unity import paths until the
gate flips from `Blocked` to `Approved`.

## 2026-06-14 Battle Feedback VFX Baseline Update

Batch 09 added six deterministic, non-cat feedback VFX assets and moved the
offline asset baseline forward at that point:

- Manifest: 44 generated/import-ready P0 PNG assets.
- Runtime visual bindings: 40 manifest-backed slots.
- New runtime surface: `battle_feedback`.
- New binding ids:
  - `feedback.hit_spark`
  - `feedback.bed_shield_pulse`
  - `feedback.sleep_stable_wave`
  - `feedback.litter_cleanse`
  - `feedback.feeder_kibble`
  - `feedback.enemy_mark_ring`
- Regenerated contact sheet:
  `design/development/asset_review/p0_runtime_visual_contact_sheet_2026-06-14.png`
- Generator:
  `design/development/tools/build_battle_feedback_vfx_assets.ps1`
- Prompt:
  `design/development/prompts/p0_battle_feedback_vfx_assets.md`
- Agent prompt:
  `design/development/agent_prompts/p0_asset_batch_09_battle_feedback_vfx.md`

The Batch 09 prompt and manifest notes explicitly forbid cat body derivatives,
starter-cat costume fragments, fur patterns, or colored-turnaround crops. This
keeps the new VFX batch separate from the stricter starter-cat formal import
gate.

## 2026-06-14 Enemy Warning VFX Baseline Update

Batch 10 added four deterministic, source-locked, non-cat warning VFX assets
and moved the then-current offline asset baseline forward:

- Manifest: 48 generated/import-ready P0 PNG assets.
- Runtime visual bindings: 44 manifest-backed slots.
- New `battle_warning` binding ids:
  - `warning.black_mud_bed_claw`
  - `warning.cold_light_beam`
  - `warning.call_tyrant_app_throw`
  - `warning.call_tyrant_summon_portal`
- Regenerated contact sheet:
  `design/development/asset_review/p0_runtime_visual_contact_sheet_2026-06-14.png`
- Generator:
  `design/development/tools/build_enemy_warning_vfx_assets.ps1`
- Prompt:
  `design/development/prompts/p0_enemy_warning_vfx_assets.md`
- Agent prompt:
  `design/development/agent_prompts/p0_asset_batch_10_enemy_warning_vfx.md`

The Batch 10 prompt and manifest notes explicitly forbid cat body derivatives,
starter-cat costume fragments, fur patterns, or colored-turnaround crops. This
keeps enemy/Boss warning production separate from the stricter starter-cat
formal import gate.

## 2026-06-14 Authority Blessing Seal Baseline Update

Batch 16 added three deterministic, non-cat authority blessing seal assets and
moved the current offline asset baseline forward:

- Manifest: 75 generated/import-ready P0 PNG assets.
- Runtime visual bindings: 71 manifest-backed slots.
- New runtime surface: `blessing_detail`.
- New binding ids:
  - `blessing_detail.oath_bedline`
  - `blessing_detail.dominion_sandglass`
  - `blessing_detail.rhythm_lullaby`
- Route reward behavior:
  `P0VisualAssetCatalog.GetRouteChoiceIcon(RouteRewardChoice)` now resolves
  specific seal assets for gain and upgrade authority blessing choices.
- Route-map coverage:
  `layer_07_blessing` is now gated to show Oath Bedline, Moon-Sand Dominion,
  and Lullaby Rhythm with their specific seals.
- Regenerated contact sheet:
  `design/development/asset_review/p0_runtime_visual_contact_sheet_2026-06-14.png`
- Generator:
  `design/development/tools/build_authority_blessing_seals.ps1`
- Prompt:
  `design/development/prompts/p0_authority_blessing_seals.md`
- Agent prompt:
  `design/development/agent_prompts/p0_asset_batch_16_authority_blessing_seals.md`

The Batch 16 prompt and manifest notes explicitly forbid starter cat body
derivatives, markings, costume fragments, props, or turnaround crops. The seals
are symbolic UI for blessing ids only; starter-cat formal import remains
blocked until active-cat screenshots match the colored three-view turnarounds.

## 2026-06-15 Batch 58 Source-Locked HUD Avatar Install Update

Batch 58 refines the asset-production boundary after the architecture audit:
Codex can produce, package, and install selected project assets into `Assets`
when the batch is narrowly scoped, source locked, locally validated, and wired
through the runtime catalogs.

Current baseline after Batch 58:

- 106 generated/import-ready manifest assets.
- 102 runtime visual bindings.
- 6 starter-cat runtime entries:
  - 3 combat sprites
  - 3 HUD avatars

Batch 58 installed:

- `thecat_cat_saiban_hud_avatar_256_v001`
- `thecat_cat_nephthys_hud_avatar_256_v001`
- `thecat_cat_suzune_hud_avatar_256_v001`

These HUD avatars are deterministic crops from the current locked Unity combat
sprites and retain the colored three-view source locks. This is not an approval
path for AI-generated starter-cat body art. Starter-cat body replacement still
requires active-cat Play Mode screenshots, colored-turnaround comparison,
Sprite import validation, Console checks, and scene/prefab binding evidence.

This creates the preferred rule for identity-sensitive cat derivatives:
source-locked derivatives from accepted sprites may be installed in bounded
batches; AI-generated cat body candidates remain review-only until Unity
runtime evidence proves exact colored-turnaround consistency.

## 2026-06-15 Batch 60 Non-Cat Skill HUD Feedback Install Update

Batch 60 confirms the non-cat side of the production boundary: Codex can
promote accepted UI/VFX candidates into Unity when a bounded install batch has
manifest rows, deterministic `.png.meta` files, runtime catalog bindings,
presenter hookup, review evidence, and offline validation.

Current baseline after Batch 60:

- 112 generated/import-ready manifest assets.
- 108 runtime visual bindings.
- 6 installed non-cat Skill HUD feedback assets:
  - `thecat_ui_skill_ready_frame_512_v001`
  - `thecat_ui_skill_cooldown_overlay_512_v001`
  - `thecat_ui_skill_no_target_marker_512_v001`
  - `thecat_ui_skill_hunger_cost_chip_512_v001`
  - `thecat_ui_auto_target_reticle_512_v001`
  - `thecat_ui_interaction_range_ripple_512_v001`

Runtime surfaces added:

- `skill_hud` state and targeting feedback slots.
- `battle_hud` interaction-range feedback slot.

Validation completed:

- Batch 60 install validator passed.
- Runtime and EditMode MSBuild passed with 0 warnings and 0 errors.
- Runtime visual contact sheet regenerated to 108 bindings.
- `git diff --check` and direct trailing-whitespace scan passed.

Validation still pending:

- Unity AssetDatabase refresh.
- Sprite import inspection.
- Console check.
- Play Mode battle HUD screenshots for ready, cooldown, no-target, low-hunger,
  auto-target, and interaction-range states.

Cat consistency impact: none. Batch 60 does not read or replace starter-cat
body art. The strict colored three-view rule remains unchanged for Saiban,
Nephthys, Suzune, and future cat-body production.

## 2026-06-15 Batch 61 Symbolic Starter Skill VFX Install Update

Batch 61 confirms the narrower starter-skill VFX lane: Codex can install
symbolic starter-skill effects when they are source locked to authority symbols
and explicitly avoid cat-body replacement.

Current baseline after Batch 61:

- 115 generated/import-ready manifest assets.
- 111 runtime visual bindings.
- 115 rows in `design/development/P0_ASSET_MANIFEST.csv`.
- 3 installed symbolic starter skill VFX assets:
  - `thecat_vfx_saiban_bedline_skill_512_v001`
  - `thecat_vfx_nephthys_moonsand_skill_512_v001`
  - `thecat_vfx_suzune_lullaby_skill_512_v001`

Runtime surfaces added:

- `battle_feedback` starter skill VFX slots for Saiban, Nephthys, and Suzune.

Validation completed:

- Batch 61 install validator passed.
- Batch 60 skill HUD feedback validator still passed after count updates.
- Runtime visual contact sheet regenerated to 111 bindings.

Validation still pending:

- Unity AssetDatabase refresh.
- Sprite import inspection.
- Console check.
- Scene/prefab binding.
- Play Mode screenshots for starter skill casts and effect timing.

Cat consistency impact: controlled. Batch 61 does not install cat body art and
does not approve Batch 49, Batch 50, or Batch 51 AI starter-cat body
candidates. The colored three-view turnarounds remain the hard source of truth
for any future cat-body generation or runtime replacement.

## 2026-06-15 Batch 62 Runtime Control Icon Candidate Update

Batch 62 keeps the systematic asset lane moving without touching cat identity
assets. Codex produced a non-cat runtime control icon candidate pack outside
Unity, with a manifest, review sheet, review note, process note, validator, and
agent prompt.

Current queue baseline after Batch 62:

- 7 asset-production queue items.
- 0 Codex-runnable candidate packs.
- 2 candidate packs complete pending Unity review:
  - Batch 54 bedroom interactables
  - Batch 62 runtime control icons
- 5 Unity-blocked validation/install items.

No manifest/runtime baseline change:

- 115 generated/import-ready manifest assets.
- 111 runtime visual bindings.

Validation still pending:

- Unity HUD readability and Console checks before install.

Offline validation completed:

- Batch 62 dedicated validator passed.
- Batch 60 skill HUD feedback validator still passed.
- Batch 61 starter skill VFX validator still passed.
- Runtime and EditMode MSBuild passed with 0 warnings and 0 errors.
- Solution MSBuild passed with 0 errors. Existing MSB3277 warnings remain.
- `git diff --check` and Batch 62 touched-file trailing-whitespace scan passed.

Cat consistency impact: none. Batch 62 is non-cat UI. It does not read,
generate, crop, recolor, or route starter-cat body art.

## 2026-06-15 Batch 63 Runtime Control Panel Candidate Update

Batch 63 extends the runtime-control UI lane with larger panel surfaces while
still avoiding cat identity assets. Codex produced a non-cat runtime control
panel candidate pack outside Unity, with a manifest, review sheet, review note,
process note, validator, and agent prompt.

Current queue baseline after Batch 63:

- 8 asset-production queue items.
- 0 Codex-runnable candidate packs.
- 3 candidate packs complete pending Unity review:
  - Batch 54 bedroom interactables
  - Batch 62 runtime control icons
  - Batch 63 runtime control panels
- 5 Unity-blocked validation/install items.

No manifest/runtime baseline change:

- 115 generated/import-ready manifest assets.
- 111 runtime visual bindings.

Validation still pending:

- Unity HUD readability and Console checks before install.

Offline validation completed:

- Batch 63 dedicated validator passed.
- Batch 62 runtime control icon validator still passed.
- Batch 60 skill HUD feedback validator still passed.
- Batch 61 starter skill VFX validator still passed.
- Runtime and EditMode MSBuild passed with 0 warnings and 0 errors.
- Solution MSBuild passed with 0 errors. Existing MSB3277 warnings remain.
- `git diff --check` passed.

Cat consistency impact: none. Batch 63 is non-cat UI. It does not read,
generate, crop, recolor, or route starter-cat body art.

## 2026-06-15 Asset Unity Validation Checklist Update

The readiness boundary is now explicit: Codex may produce systematic bitmap
asset candidates outside Unity, but Unity evidence is required before formal
installation into `Assets`.

Added project coverage:

- `P0AssetUnityValidationChecklist` evaluates the current 9-item asset queue.
- `P0AssetUnityValidationChecklistMenu` writes a Markdown checklist from the
  Unity editor.
- `P0AssetUnityValidationChecklistTests` locks the queue counts and failure
  modes.

Current readiness:

- Codex-side candidate production lane is clear for the next bounded batch.
- Unity install lane remains blocked on editor screenshots, Console status,
  import settings, and scene/prefab reference checks.
- Cat body asset lane remains extra strict: generated cat art is review-only
  until it matches the locked colored three-view turnarounds in Unity.

## 2026-06-15 Batch 64 Secondary Enemy Warning Candidate Update

Batch 64 keeps the systematic asset lane moving without touching cat identity
assets. Codex produced a non-cat secondary enemy warning candidate pack outside
Unity, with a manifest, review sheet, review note, process note, validator,
and agent prompt.

Current queue baseline after Batch 64:

- 9 asset-production queue items.
- 0 Codex-runnable candidate packs.
- 4 candidate packs complete pending Unity review:
  - Batch 54 bedroom interactables
  - Batch 62 runtime control icons
  - Batch 63 runtime control panels
  - Batch 64 secondary enemy warning VFX
- 5 Unity-blocked validation/install items.

No manifest/runtime baseline change:

- 115 generated/import-ready manifest assets.
- 111 runtime visual bindings.

Validation still pending:

- Unity gameplay-scale warning readability, Console checks, screenshots, and
  future secondary-enemy prefab validation before install.

Offline validation completed:

- Batch 64 dedicated validator passed.

Cat consistency impact: none. Batch 64 is non-cat warning VFX. It does not
read, generate, crop, recolor, or route starter-cat body art.

## 2026-06-15 Batch 65 Route Map Readability Candidate Update

Batch 65 moves the systematic asset lane into route-map readability without
touching cat identity assets. Codex produced a non-cat UI candidate pack outside
Unity, with a manifest, review sheet, review note, process note, validator,
build script, and agent prompt.

Current queue baseline after Batch 65:

- 10 asset-production queue items.
- 0 Codex-runnable candidate packs.
- 5 candidate packs complete pending Unity review:
  - Batch 54 bedroom interactables
  - Batch 62 runtime control icons
  - Batch 63 runtime control panels
  - Batch 64 secondary enemy warning VFX
  - Batch 65 route-map readability UI
- 5 Unity-blocked validation/install items.

No manifest/runtime baseline change:

- 115 generated/import-ready manifest assets.
- 111 runtime visual bindings.

Validation still pending:

- Unity route-map scale, current/selected contrast, route path readability,
  Boss path pressure readability, screenshots, Console checks, and
  scene/prefab validation before install.

Offline validation completed:

- Batch 65 dedicated validator passed.

Cat consistency impact: none. Batch 65 is non-cat route-map UI. It does not
read, generate, crop, recolor, or route starter-cat body art.

## 2026-06-15 Asset Unity Checklist File Evidence Update

The Codex-side asset lane now has a file-level evidence gate for the Unity
handoff checklist. This closes the gap where the queue evaluator could build
checklist markdown in memory, but the actual review artifact under
`design/development/asset_review` could be missing or stale.

Current checklist artifact:

- `design/development/asset_review/P0_ASSET_UNITY_VALIDATION_CHECKLIST.md`

Current checklist baseline:

- 11 asset-production queue items.
- 6 candidate packs complete pending Unity review.
- 5 Unity-blocked validation/install items.
- 6 candidate-only packs covered by no-Unity-meta policy.
- 11 queue items covered by Console-gated validation.
- Batch 65 route-map readability and Batch 67 bedroom interaction affordance
  coverage required.
- Starter-cat colored three-view turnaround comparison required before any
  body-art import decision.

Offline validation now checks:

- The checklist file exists at the expected review path.
- The file summary matches the current queue evaluation.
- The current queue, candidate-review, and Unity-blocked counts are present.
- Batch 65 route-map readability and Batch 67 bedroom interaction affordance
  remain listed.
- Candidate-only no-meta policy remains listed.
- Console validation gates remain listed.
- Starter-cat colored-turnaround validation remains listed.

Operational conclusion:

- Systematic asset production may continue in Codex for bounded candidate/spec
  batches outside `Assets`.
- Installing or replacing runtime Unity assets remains blocked until Unity
  screenshots, Console checks, import settings, and scene/prefab references
  pass.
- Cat body assets remain stricter than general UI/VFX/prop assets and must
  match the document-colored three-view turnarounds before any import approval.

## 2026-06-15 Batch 66 Systematic Asset Master Plan Update

Batch 66 converts the current asset state into a master gap matrix before
opening more image production. It is a control/spec batch, not an import or
visual-art batch.

Artifacts:

- `design/development/asset_candidates/p0_asset_dashboard/batch_66_systematic_asset_master_plan_2026-06-15/p0_asset_batch66_master_gap_matrix.csv`
- `design/development/asset_candidates/p0_asset_dashboard/batch_66_systematic_asset_master_plan_2026-06-15/p0_asset_batch66_master_blueprint.md`
- `design/development/asset_candidates/p0_asset_dashboard/batch_66_systematic_asset_master_plan_2026-06-15/p0_asset_batch66_process_note.md`
- `design/development/agent_prompts/p0_asset_batch_67_bedroom_interaction_affordance_candidates.md`

Batch 66 decision at the time:

- Next Codex-side asset batch should be Batch 67 bedroom interaction
  affordance candidates.
- Batch 67 is non-cat UI/VFX for bed, litter box, feeder, blocked interaction,
  and valid interaction range feedback.
- Batch 67 must stay outside `Assets` until a later Unity install review.
- Starter-cat and future partner-cat body assets remain blocked behind the
  locked colored three-view turnaround rule.

Validation completed:

- `validate_systematic_asset_master_plan.ps1` passed.
- Batch 66 created no Unity `.meta` files.
- Batch 66 changed no manifest/runtime binding counts.

## 2026-06-15 Batch 67 Bedroom Interaction Affordance Candidate Update

Batch 67 moves the systematic asset lane into P0 interaction feel without
touching cat identity assets. Codex produced a non-cat UI/VFX candidate pack
outside Unity, with a manifest, review sheet, review note, process note,
validator, build script, and queue coverage.

Current queue baseline after Batch 67:

- 11 asset-production queue items.
- 0 Codex-runnable candidate packs.
- 6 candidate packs complete pending Unity review:
  - Batch 54 bedroom interactables
  - Batch 62 runtime control icons
  - Batch 63 runtime control panels
  - Batch 64 secondary enemy warning VFX
  - Batch 65 route-map readability UI
  - Batch 67 bedroom interaction affordance UI/VFX
- 5 Unity-blocked validation/install items.

No manifest/runtime baseline change:

- 115 generated/import-ready manifest assets.
- 111 runtime visual bindings.

Validation still pending:

- Unity bed, litter box, feeder, blocked interaction, range, input timing,
  Sprite import settings, screenshots, Console checks, and scene/prefab
  validation before install.

Offline validation completed:

- Batch 67 dedicated validator passed.
- Batch 66 master-plan validator passed after the gap matrix was refreshed to
  show Batch 67 as candidate-complete pending Unity review.

Cat consistency impact: none. Batch 67 is non-cat interaction UI/VFX. It does
not read, generate, crop, recolor, or route starter-cat body art.

## 2026-06-15 Batch 68 Starter Cat Core Document Source-Lock Gate Update

Batch 68 strengthens the starter-cat consistency lane before any further cat
asset production. It is a code/documentation gate only: no PNG/JPEG files,
Unity sprites, `.meta` files, runtime bindings, source hashes, or formal import
state were modified.

New source-lock evidence checks:

- `P0StarterCatSourceLockPacketEvidence` now covers three core documents:
  - source-lock packet
  - turnaround conformance spec
  - strict reference pack
- Every core document must repeat all three exact colored-turnaround source
  paths for Saiban, Nephthys, and Suzune.
- Every core document must keep formal starter-cat Unity import blocked.
- Core documents must contain zero mojibake or stale encoded design path
  mentions.
- `P0AssetReviewPacket` now prints these core-document counts so a reviewer can
  see whether the starter-cat source-lock docs are trustworthy before approving
  future image production.

Regression coverage added:

- A mojibake/stale encoded path in the strict reference pack fails readiness.
- A core source-lock document missing Suzune's exact colored-turnaround path
  fails readiness.

Pending Unity-side evidence remains unchanged:

- active-cat Play Mode screenshots for Saiban, Nephthys, and Suzune
- side-by-side review against the colored three-view turnarounds
- Console, scene/prefab, and runtime readability checks before any cat sprite
  replacement

## 2026-06-15 Starter Cat Strict Identity Gate Update

Starter-cat body art now has a stronger Codex-side gate before any future
Unity import review. The existing strict candidate evidence report no longer
only checks that Batch 49/50/51 files, review notes, and prompts exist; it also
requires every current starter-cat candidate manifest to bind back to the exact
colored three-view turnaround source lock and the current Batch 47 generation
spec artifacts.

New locked evidence:

- Exact colored three-view turnaround path and SHA-256 per cat.
- Batch 47 strict-generation JSON path and SHA-256 per cat.
- Batch 47 strict-generation card path and SHA-256 per cat.
- Batch 47 JSON identity clauses:
  - source lock id
  - exact source turnaround path
  - non-human cat body rule
  - must-keep list
  - reject list
  - positive and negative prompt
  - palette drift rejection
  - `strict_generation_spec_only_do_not_import`

Regression coverage:

- `P0StarterCatStrictCandidateEvidenceTests` now mutates a Saiban manifest
  source path to `design/?assets/...` and verifies readiness fails.

Validation completed:

- Runtime MSBuild passed.
- EditMode MSBuild passed.
- Batch 47 strict-generation spec validator passed.

Remaining Unity gate:

- Active-cat screenshots for Saiban, Nephthys, and Suzune must still be
  captured and compared against the locked colored three-view turnarounds
  before any cat-body candidate is approved for import.
