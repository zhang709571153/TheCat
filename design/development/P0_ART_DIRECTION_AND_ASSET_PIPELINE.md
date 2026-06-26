# P0 Art Direction and Asset Pipeline

Date: 2026-06-13

Source docs:

- `design/梦境支配者核心玩法/docs/00_overview/p0_minimum_design.md`
- `design/梦境支配者核心玩法/docs/03_characters/character_design.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`

## Purpose

This file is the local production contract for P0 visual assets. It must be
updated before generating, importing, or replacing visual assets. The goal is to
keep all Codex-generated placeholder and semi-final assets consistent enough to
support a playable P0 without locking final art too early.

## Visual Pillars

1. Non-humanoid cats first.
   Cats may wear symbolic gear, carry relics, and use exaggerated readable
   silhouettes, but their bodies remain cats rather than human characters with
   ears.
2. Hand-painted dream defense.
   Shapes should feel soft, painterly, and slightly unreal. Avoid hard sci-fi,
   photorealism, glossy mobile-RPG rendering, and noisy stock-art lighting.
3. Civilization symbols as readable accents.
   Each major cat uses a small set of cultural motifs tied to relic projections.
   These motifs should be originalized and simplified, not direct museum-copy
   replicas.
4. Bedroom dream as the anchor.
   Even when the route expands, P0 readability comes from defending a bed inside
   a dream-tinted bedroom space.
5. Monster language is black-mud intrusion.
   Enemies should feel like sleep interruptions and corrupted dream objects, not
   generic fantasy monsters.

## P0 Character Visual Rules

Authoritative reference priority for existing cat characters:

1. Character `turnaround/*_turnaround_colored_2026-06-03.png` is the hard
   source of truth for body proportion, coat color, eye color, clothing,
   weapons, accessories, silhouette, and civilization motifs.
2. Character `concept/`, `animation/`, and `references/` files may clarify pose
   and action language, but must not override the colored turnaround.
3. Batch 01 generated lineup/style anchors are only secondary mood references.
   They cannot change character proportions, palettes, costumes, or props.
4. If a generated asset diverges from the colored turnaround, move it outside
   `Assets` into `design/development/rejected_assets/` and do not mark its
   manifest row `generated`.

| Cat | Role | Core Read | Civilization / Relic Motifs | Dominant Accent | Avoid |
| --- | --- | --- | --- | --- | --- |
| Saiban | Defender | heavy, loyal, shield-line guardian | Celtic / medieval Britain, oath shield, helm, sword | silver, blue-white, sun gold | human knight armor proportions |
| Nephthys | Controller | elegant, sand-moon strategist | ancient Egypt, obelisk, palette, dream script, royal mark | lapis, sand gold, moon teal | direct Cleopatra costume cliche |
| Suzune | Healer | small ritual bell healer | Japanese shrine, kagura bell, torii, sleep circle | vermilion, warm white, soft moon blue | human shrine maiden body |

Common cat rules:

- Body proportion: follow the colored turnaround exactly for existing
  characters, including compact upright chibi-cat designs when present.
- Face: expressive but cat-first. Large eyes are allowed; human lips are not.
- Gear attachment, clothing, weapons, and props: preserve the colored
  turnaround for existing characters. Do not simplify away signature props such
  as Saiban's shield/sword, Nephthys' obelisk/hood, or Suzune's kagura bells.
- Combat readability: each cat must remain readable at small HUD/avatar size.
- Starter cat combat sprites are protected by both
  `P0StarterCatTurnaroundSourceLocks` and `P0HardReferenceSourceLocks`.
  Their manifest rows must include `source_lock_ids`:
  `saiban_turnaround_colored`, `nephthys_turnaround_colored`, and
  `suzune_turnaround_colored`. Do not update those locked Unity sprites or
  source turnaround files without a deliberate visual review, refreshed
  SHA-256 lock values, and development-log entry explaining the replacement.
- Starter cat production is also protected by
  `P0StarterCatVisualConsistencyChecklist`. This gate requires each starter cat
  to bind:
  - its colored-turnaround source lock
  - its locked Unity sprite path
  - its active-cat Play Mode screenshot filename
  - at least five cat-specific visual traits from the colored turnaround
  - an explicit blocker for colored-turnaround drift and human-proportion drift
- Do not accept a new or revised Saiban, Nephthys, or Suzune asset from a
  generic prompt such as "cute dream cat" or from Batch 01 style anchors alone.
  The acceptance note must name the exact colored-turnaround traits preserved.

### Batch 69 Starter Cat Runtime Comparison Audit

Before any new Saiban, Nephthys, or Suzune production batch is installed into
Unity, compare it against both the authoritative colored three-view turnaround
and the current runtime combat sprite. Batch 69 is the current audit package:

- `design/development/asset_candidates/starter_cats/batch_69_turnaround_runtime_comparison_audit_2026-06-15/thecat_cat_starter_turnaround_runtime_comparison_batch69_review_sheet.png`
- `design/development/asset_candidates/starter_cats/batch_69_turnaround_runtime_comparison_audit_2026-06-15/starter_cat_turnaround_runtime_comparison_batch69_manifest.csv`
- `design/development/asset_candidates/starter_cats/batch_69_turnaround_runtime_comparison_audit_2026-06-15/starter_cat_turnaround_runtime_comparison_batch69_review.md`

This package is audit-only. It does not approve import, sprite replacement, or
source-lock hash changes. Its purpose is to make drift visible before the next
systematic asset-production pass.

Batch 70 adds source-derived front, side, and back reference plates for the
three starter cats:

- `design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/thecat_cat_starter_turnaround_reference_plates_batch70_review_sheet.png`
- `design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/starter_cat_turnaround_reference_plates_batch70_manifest.csv`

These plates are deterministic crop-and-resize derivatives from the colored
three-view turnarounds. They are reference inputs for future Codex image
generation and review only. They are not transparent runtime sprites and do not
authorize Unity import.

Batch 71-73 install one debug reference atlas per starter cat into Unity:

- `Assets/TheCat/Art/Characters/References/thecat_cat_saiban_turnaround_reference_atlas_2304x768_v001.png`
- `design/development/asset_candidates/starter_cats/saiban/batch_71_saiban_unity_reference_install_2026-06-15/thecat_cat_saiban_batch71_unity_reference_install_review_sheet.png`
- `design/development/asset_candidates/starter_cats/saiban/batch_71_saiban_unity_reference_install_2026-06-15/saiban_batch71_unity_reference_install_manifest.csv`
- `Assets/TheCat/Art/Characters/References/thecat_cat_nephthys_turnaround_reference_atlas_2304x768_v001.png`
- `design/development/asset_candidates/starter_cats/nephthys/batch_72_nephthys_unity_reference_install_2026-06-15/thecat_cat_nephthys_batch72_unity_reference_install_review_sheet.png`
- `design/development/asset_candidates/starter_cats/nephthys/batch_72_nephthys_unity_reference_install_2026-06-15/nephthys_batch72_unity_reference_install_manifest.csv`
- `Assets/TheCat/Art/Characters/References/thecat_cat_suzune_turnaround_reference_atlas_2304x768_v001.png`
- `design/development/asset_candidates/starter_cats/suzune/batch_73_suzune_unity_reference_install_2026-06-15/thecat_cat_suzune_batch73_unity_reference_install_review_sheet.png`
- `design/development/asset_candidates/starter_cats/suzune/batch_73_suzune_unity_reference_install_2026-06-15/suzune_batch73_unity_reference_install_manifest.csv`

Each atlas is a direct front/side/back concatenation of that cat's Batch 70
reference plates. They are installed so Unity reviewers have the hard colored
turnaround references available inside the project. They are not runtime-bound,
do not replace `thecat_cat_*_combat_sprite_512_v001`, and do not approve formal
cat body-art import.

Codex may create candidate PNGs, review sheets, manifests, and validators
outside `Assets`. Unity is still the authority for installed-asset validation:
Sprite import settings, scene/prefab references, active-cat Play Mode
screenshots, HUD binding, and Console checks must pass before any generated cat
asset becomes a formal runtime sprite.

## P0 Enemy Visual Rules

Authoritative reference priority for existing enemy and Boss assets:

1. Enemy `concept/` and `animation/` source PNGs under
   `design/梦境支配者核心玩法/assets/enemies/` are the hard source of truth for
   silhouette, palette, material language, attack cues, and behavior read.
2. Batch 01 generated monster/style anchors are secondary mood references only.
   They may guide brushwork and lighting, but cannot override source concepts.
3. If an enemy, Boss, warning VFX, or route icon diverges from the source
   concept/animation read, move it outside `Assets` into
   `design/development/rejected_assets/` and keep its manifest row out of
   `generated` / `imported` status.

| Enemy | Shape Rule | Readability Hook | Color / Material | P0 Status Cues |
| --- | --- | --- | --- | --- |
| Black Mud Nightmare | low crawling black mass | soft sludge body, bright hostile eye or crack | black-violet mud, dim red core | slow: moon sand dust; mark: royal eye sigil |
| Cold Light Shadow | thin floating lamp-shadow | cold rectangular glow, long shadow limbs | pale cyan light, dark silhouette | ranged charge glow, cold beam warning |
| Call Tyrant | phone-call nightmare boss | ringing phone crown, waveform limbs, vibration pulse | black device shell, red notification light, electric cyan | summon ring, throw arc, warning pulse |

Monster constraints:

- Black mud is the base infection language.
- No gore, no realistic horror anatomy.
- Dream interruption objects may be embedded: phone, alarm, notification, lamp,
  toy, train track.
- Boss silhouettes must be readable from top/angled camera.

Mandatory P0 hard reference files:

- Starter cats:
  `design/.../characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`,
  `design/.../characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`,
  and
  `design/.../characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`
- Black Mud Nightmare:
  `design/梦境支配者核心玩法/assets/enemies/en01_black_mud_nightmare/concept/black_mud_nightmare_concept.png`
  and
  `design/梦境支配者核心玩法/assets/enemies/en01_black_mud_nightmare/animation/black_mud_nightmare_animation.png`
- Cold Light Shadow:
  `design/梦境支配者核心玩法/assets/enemies/en04_cold_light_shadow/concept/cold_light_shadow_concept.png`
  and
  `design/梦境支配者核心玩法/assets/enemies/en04_cold_light_shadow/animation/cold_light_shadow_animation.png`
- Call Tyrant:
  `design/梦境支配者核心玩法/assets/enemies/en06_call_tyrant/concept/call_tyrant_concept.png`
  and
  `design/梦境支配者核心玩法/assets/enemies/en06_call_tyrant/animation/call_tyrant_animation.png`

## P0 Bedroom Prop Visual Rules

Authoritative reference priority for bedroom props:

1. The bedroom map concept and sprite sheets under
   `design/梦境支配者核心玩法/assets/levels/lv01_bedroom_dream/` are the hard
   source of truth for bed, litter box, feeder, floor, and room palette.
2. The generated bedroom anchor may guide mood and lighting, but cannot change
   the source prop identity, relative proportions, or interaction readability.
3. Prop replacements must keep gameplay ids stable: `bed`, `litter_box`, and
   `feeder`.

Mandatory P0 hard reference files:

- `design/梦境支配者核心玩法/assets/levels/lv01_bedroom_dream/concept/bedroom_dream_map_concept.png`
- `design/梦境支配者核心玩法/assets/levels/lv01_bedroom_dream/sprites/bedroom_dream_foreground_sprites.png`
- `design/梦境支配者核心玩法/assets/levels/lv01_bedroom_dream/sprites/bedroom_dream_mid_background_sprites.png`

## P0 UI Color and Icon Rules

HUD colors:

| System | Color Token | UI Shape |
| --- | --- | --- |
| Owner sleep | soft blue / lavender | long bed-linked stability bar |
| Cat HP | warm green | per-cat compact bar |
| Poop gauge | earthy brown / amber | rounded litter icon plus fill |
| Hunger gauge | cream / fish-gold | feeder/fish icon plus fill |
| Weak state | desaturated gray with red edge | disabled cat portrait overlay |
| Shield | silver blue rim | small shield icon |
| Slow | moon-sand cyan | spiral / sand icon |
| Knockback | silver impact burst | outward arrow impact icon |
| Mark | royal eye purple-gold | eye sigil icon |
| Sleep stable | blue note / crescent | crescent note icon |

UI constraints:

- Use dense, readable combat HUD rather than marketing hero layout.
- P0 graybox IMGUI is temporary. Final P0 HUD should move to UGUI or UI Toolkit
  with presenters reading runtime state.
- Icons should be simple silhouettes first, detailed rendering second.
- Status icons need 32x32 and 64x64 exports.

## Asset Import Layout

Target Unity paths:

| Asset Type | Path |
| --- | --- |
| Character concepts | `Assets/TheCat/Art/Characters/Concepts` |
| Character sprites | `Assets/TheCat/Art/Characters/Sprites` |
| Enemy concepts | `Assets/TheCat/Art/Enemies/Concepts` |
| Enemy sprites | `Assets/TheCat/Art/Enemies/Sprites` |
| Scene/background | `Assets/TheCat/Art/Scenes/BedroomDream` |
| UI icons | `Assets/TheCat/Art/UI/Icons` |
| UI panels/buttons | `Assets/TheCat/Art/UI/Frames` |
| UI badges | `Assets/TheCat/Art/UI/Badges` |
| Generated references | `Assets/TheCat/Art/_GeneratedReferences` |
| Asset manifests | `Assets/TheCat/Art/_Manifests` |

Do not import design-source documents into `Assets`. Keep research and prompts
under `design/development` until a concrete asset is selected for Unity import.

## Naming Rules

Format:

`thecat_{category}_{subject}_{variant}_{sizeOrState}_v###`

Examples:

- `thecat_cat_saiban_turnaround_2048_v001.png`
- `thecat_enemy_blackmud_idle_sheet_1024_v001.png`
- `thecat_ui_status_slow_64_v001.png`
- `thecat_scene_bedroomdream_bg_1920x1080_v001.png`

Rules:

- English lowercase file names only.
- Use stable implementation ids: `saiban`, `nephthys`, `suzune`,
  `black_mud_nightmare`, `cold_light_shadow`, `call_tyrant`.
- Never overwrite a generated asset. Increment version.
- Keep prompt/source notes beside the asset manifest, not in Unity component
  fields.

## P0 Asset Manifest Template

Each generation/import batch must add a manifest row.

| Field | Meaning |
| --- | --- |
| asset_id | Stable id matching filename without extension |
| subject_id | Gameplay id, such as `saiban` or `call_tyrant` |
| asset_type | concept, sprite, icon, badge, background, frame, vfx |
| priority | P0, P1, P2 |
| source_prompt_path | local prompt or generation note |
| reference_asset_ids | previous references used |
| source_lock_ids | locked design-source ids used to accept source-sensitive art |
| unity_import_path | final path under `Assets/TheCat/Art` |
| size | pixel size or sprite sheet grid |
| status | planned, generated, imported, rejected, replaced |
| consistency_notes | short visual checks and deviations |

Recommended manifest path:

`Assets/TheCat/Art/_Manifests/p0_asset_manifest.csv`

## First Generation Batches

Batch 1: style anchors.

- Bedroom dream background concept.
- Three-cat lineup concept.
- Black mud nightmare enemy concept.
- UI icon style sample for the five P0 status tags.

Batch 2: gameplay placeholders.

- Saiban / Nephthys / Suzune small combat sprites.
- Black Mud Nightmare and Cold Light Shadow combat sprites.
- Bedroom Dream battle background.
- Bed, litter box, feeder props.
- Sleep, HP, poop, hunger HUD icons.
- Sleep Stable, Slow, Knockback, Mark, Shield status HUD icons split from the
  5x64 style sample.

Batch 2 source-extracted status:

- Complete from locked source images:
  - Black Mud Nightmare combat sprite.
  - Cold Light Shadow combat sprite.
  - Bedroom Dream battle background.
  - Protected bed P0 placeholder.
  - Litter box prop.
  - Feeder prop.
- Complete from deterministic HUD icon generation:
  - Sleep HUD icon.
  - Cat HP HUD icon.
  - Team poop HUD icon.
  - Team hunger HUD icon.
- Complete from source-splitting the status icon style anchor:
  - Sleep Stable status HUD icon.
  - Slow status HUD icon.
  - Knockback status HUD icon.
  - Mark status HUD icon.
  - Shield status HUD icon.
- Still planned:
  - None for Batch 2 gameplay placeholders.
- Notes:
  - Black Mud and Cold Light use source concept crops for readable P0
    silhouettes; their animation sources remain locked for later frame sheets.
  - The Bedroom Dream battle background is a 1920x1080 source-locked image
    derived from `bedroom_dream_map_concept.png` by
    `design/development/tools/build_bedroom_dream_battle_background.ps1`; it is
    a non-cat runtime background for `07-battle-world-visuals.png`.
  - The protected bed is an opaque Bedroom Dream map-concept crop and should not
    be treated as final transparent prop art.
  - Litter box and feeder use the Bedroom Dream mid/background sprite sheet
    with magenta key removal.
  - Four-core HUD icons are deterministic 64px placeholders using the status
    icon style anchor palette/glow language; they are not source-sensitive and
    therefore do not require `source_lock_ids`.
  - P0 status tag icons are exact 64px crops from
    `thecat_style_status_icons_5x64_v001.png`, produced by
    `design/development/tools/build_status_tag_icons_from_sheet.ps1` and bound
    through `P0VisualAssetCatalog.GetStatusIcon(statusTagId)`.
  - Status HUD rows now carry these icon references through
    `P0StatusHudEntry.StatusIcons`; the graybox IMGUI HUD draws them inline for
    screenshot validation.

Batch 3: Boss readiness.

- Call Tyrant boss concept.
- Call Tyrant summon ring / throw warning concept.
- Boss route node icon.

Batch 3 source-derived status:

- Complete from locked Call Tyrant source images:
  - Call Tyrant square concept board.
  - Call Tyrant transparent summon / throw warning VFX.
  - Call Tyrant boss route node icon.
- Still planned:
  - None for the current P0 asset manifest.
- Notes:
  - The concept board preserves the source device-shell silhouette, red call
    eyes, purple tie, black mud body, and thrown app language.
  - The warning VFX uses the animation source summon portal plus red/cyan
    vibration and throw-arc language for gameplay readability.
  - The route icon uses the concept source face crop with a compact red/cyan
    boss-node ring.

## Consistency Checklist

Before importing any generated asset:

- Existing cat characters match their colored turnaround before matching any
  generated style anchor.
- Starter cat combat sprites pass `P0StarterCatTurnaroundSourceLocks`; a
  visually similar generated replacement is not acceptable unless the lock is
  intentionally refreshed after review.
- Starter cat manifest rows must also pass `P0HardReferenceSourceLocks` through
  explicit colored-turnaround `source_lock_ids`; the generated lineup is never
  enough to accept a cat sprite.
- Starter cat derivative production must pass
  `P0StarterCatAssetProductionSpec`: candidates stay under
  `design/development/asset_candidates/starter_cats/<cat_id>/`, approved imports
  stay under `Assets/TheCat/Art/Characters/Sprites`, and every candidate needs
  source, current sprite, active screenshot, side-by-side comparison, trait
  coverage, and accept/reject decision evidence.
- Allowed starter cat derivatives are currently limited to:
  `combat_sprite_refinement_512`, `hud_avatar_256`, `skill_icon_motif_128`, and
  `front_animation_keyframe_512`.
- Enemy, Boss, VFX, and bedroom prop assets match their source concept,
  animation, or sprite-sheet hard references before matching generated anchors.
- Enemy, Boss, VFX, and bedroom prop source files pass
  `P0HardReferenceSourceLocks`; do not regenerate against missing or changed
  source images unless the lock is intentionally refreshed after review.
- Source-sensitive manifest rows must fill `source_lock_ids` with the exact
  lock ids used as source authority. `reference_asset_ids` are visual or batch
  dependency anchors and are not a substitute for source locks.
- Runtime-facing assets must appear in
  `P0VisualAssetCatalog.CreateP0RuntimeBindings` before they are treated as
  wired. A generated file with no binding remains an unused asset candidate.
- The cat remains visibly a cat character, not a human body with ears.
- The asset matches the role/civilization motif table.
- It can be read at intended in-game size.
- It does not introduce a new unrelated palette.
- It does not rely on copyrighted photos, museum display images, or direct
  replica layouts.
- It has a versioned file name and manifest row.
- If it replaces a graybox object, the gameplay id remains unchanged.

## Current P0 Asset State

| Area | Current State | Next Action |
| --- | --- | --- |
| Main menu | IMGUI with Batch 08 main-menu background and title logo now available as runtime-bound assets | wire shell textures into menu rendering and capture main-menu screenshot evidence |
| Route map | IMGUI with 8 route node icon assets plus route-choice icons, category card frames, and detail badges available; all P0 node types expose `P0VisualAssetReference` through `P0RouteNodePresenter`, and reward choices resolve icons, card frames, and effect detail badges through `P0VisualAssetCatalog` | validate Unity route-map screenshots and move toward final UI binding |
| Battle scene | Unity primitive graybox with source-extracted Bedroom Dream battle background, Black Mud, Cold Light, bed, litter box, and feeder sprites now available under `Assets/TheCat/Art`; these are bound through `P0VisualAssetCatalog` and the `P0WorldVisualAssetView` battle-world SpriteRenderer path | validate Unity import previews and Play Mode screenshots before replacing more graybox presentation |
| Cats | corrected starter combat sprites extracted from colored turnaround front views, locked by SHA-256 source checks, bound through `P0VisualAssetCatalog`, and covered by Batch 69 turnaround/runtime comparison evidence | keep future cat art locked to colored turnarounds, compare against Batch 69, and capture Unity active-cat previews |
| Boss | source-derived concept, warning VFX, and route icon generated from locked Call Tyrant source images; route icon, enemy visual, and warning VFX are data-bound | validate Unity import previews, then capture route/battle screenshots |
| Status icons | style sample, four-core HUD icons, P0 64px status icons, and 32px compact status HUD icons generated; UI shell panel/button/reward assets are also available for shared HUD and settlement surfaces | validate Unity import previews, then wire final UI Sprite rendering |

## Runtime Preview Binding Gate

Before a new P0 gameplay asset is considered ready for systematic production,
it must have a planned runtime surface:

- If it is part of the current P0 loop, add or verify a
  `P0VisualAssetBinding`.
- If it is source-sensitive, fill exact `source_lock_ids`; for starter cats,
  the colored turnarounds are mandatory and override any generated style anchor.
- The asset must pass `P0RuntimeVisualBindingCoverage` once it is expected to
  appear in the graybox route map, battle HUD, or battle world.
- The asset must appear in `P0AssetReviewPacket` before it is accepted for the
  next production batch; this gives review agents a single manifest/source-lock
  /runtime-binding packet to inspect.
- The current 71-slot runtime visual baseline must also have an offline contact
  sheet at
  `design/development/asset_review/p0_runtime_visual_contact_sheet_2026-06-14.png`
  before a new formal batch is accepted.
- Before starting a new controlled asset-production batch, run
  `P0AssetProductionReadiness` or the Unity offline acceptance gate and confirm
  `P0 Offline Asset Production Readiness` passes.
- For cat-related batches, also confirm the asset review packet shows the
  `Starter Cat Asset Production Spec` section with 3 starter cats and 12
  allowed derivative asset types.
- For starter-cat work, confirm the asset review packet shows the
  `Starter Cat Source-Lock Packet Evidence` section and that
  `P0StarterCatSourceLockPacketEvidence` is ready. This packet records the
  colored-turnaround hashes, locked Unity sprite hashes, active-cat screenshot
  targets, and Batch 05 review sheets in one place.
- For starter-cat work, also confirm the asset review packet shows the
  `Starter Cat Turnaround Runtime Comparison Audit` section and that
  `P0StarterCatTurnaroundComparisonAuditEvidence` is ready. This audit compares
  the colored three-view turnarounds against the current Unity combat sprites
  before any new Codex-side cat image batch is installed.
- For starter-cat work, also confirm the asset review packet shows the
  `Starter Cat Source Turnaround Reference Plates` section and that
  `P0StarterCatReferencePlateEvidence` is ready. These plates provide front,
  side, and back source-derived visual inputs for future Codex image generation
  without weakening the Unity import block.
- For runtime visual approval, also confirm the asset review packet shows
  `Play Mode Screenshot File Evidence` as complete: 10/10 expected screenshots
  present and zero unexpected PNG files.
- The IMGUI preview path and `P0WorldVisualAssetView` SpriteRenderer path are
  acceptable for P0 asset review, but final approval still requires Unity
  screenshots and later UGUI/prefab binding where applicable.

## Current Review Artifacts

- Starter cat turnaround contact sheet:
  `design/development/asset_review/p0_starter_cat_turnaround_review_2026-06-14.png`
- Batch 69 starter-cat turnaround/runtime comparison review sheet:
  `design/development/asset_candidates/starter_cats/batch_69_turnaround_runtime_comparison_audit_2026-06-15/thecat_cat_starter_turnaround_runtime_comparison_batch69_review_sheet.png`
- Batch 69 starter-cat turnaround/runtime comparison manifest:
  `design/development/asset_candidates/starter_cats/batch_69_turnaround_runtime_comparison_audit_2026-06-15/starter_cat_turnaround_runtime_comparison_batch69_manifest.csv`
- Batch 70 starter-cat source-turnaround reference plate review sheet:
  `design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/thecat_cat_starter_turnaround_reference_plates_batch70_review_sheet.png`
- Batch 70 starter-cat source-turnaround reference plate manifest:
  `design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/starter_cat_turnaround_reference_plates_batch70_manifest.csv`
- Batch 71-73 starter-cat Unity reference atlas validator:
  `design/development/tools/validate_starter_cat_unity_reference_installs.ps1`
- Batch 72 Nephthys Unity reference review sheet:
  `design/development/asset_candidates/starter_cats/nephthys/batch_72_nephthys_unity_reference_install_2026-06-15/thecat_cat_nephthys_batch72_unity_reference_install_review_sheet.png`
- Batch 73 Suzune Unity reference review sheet:
  `design/development/asset_candidates/starter_cats/suzune/batch_73_suzune_unity_reference_install_2026-06-15/thecat_cat_suzune_batch73_unity_reference_install_review_sheet.png`
- Offline P0 runtime visual review packet:
  `design/development/asset_review/P0_RUNTIME_VISUAL_REVIEW_PACKET.md`
- Bedroom Dream battle background review note:
  `design/development/asset_review/p0_bedroom_dream_battle_background_2026-06-14.md`
- Bedroom Dream battle background deterministic builder:
  `design/development/tools/build_bedroom_dream_battle_background.ps1`
- Runtime visual contact sheet:
  `design/development/asset_review/p0_runtime_visual_contact_sheet_2026-06-14.png`
- Runtime visual contact sheet review note:
  `design/development/asset_review/p0_runtime_visual_contact_sheet_2026-06-14.md`
- Runtime visual contact sheet deterministic builder:
  `design/development/tools/build_runtime_visual_contact_sheet.ps1`
- Batch 06 route node icon deterministic builder:
  `design/development/tools/build_route_node_icons.ps1`
- Batch 06 route node icon production prompt:
  `design/development/prompts/p0_route_node_icons.md`
- Batch 08 UI shell deterministic builder:
  `design/development/tools/build_ui_shell_assets.ps1`
- Batch 08 UI shell production prompt:
  `design/development/prompts/p0_ui_shell_assets.md`
- Batch 08 UI shell execution prompt:
  `design/development/agent_prompts/p0_asset_batch_08_ui_shell.md`
- Batch 12 route choice icon deterministic builder:
  `design/development/tools/build_route_choice_icons.ps1`
- Batch 12 route choice icon production prompt:
  `design/development/prompts/p0_route_choice_icons.md`
- Batch 12 route choice icon execution prompt:
  `design/development/agent_prompts/p0_asset_batch_12_route_choice_icons.md`
- Batch 13 route reward card frame deterministic builder:
  `design/development/tools/build_route_reward_card_frames.ps1`
- Batch 13 route reward card frame production prompt:
  `design/development/prompts/p0_route_reward_card_frames.md`
- Batch 13 route reward card frame execution prompt:
  `design/development/agent_prompts/p0_asset_batch_13_route_reward_card_frames.md`
- Batch 14 status compact icon deterministic builder:
  `design/development/tools/build_status_compact_icons.ps1`
- Batch 14 status compact icon production prompt:
  `design/development/prompts/p0_status_compact_icons.md`
- Batch 14 status compact icon execution prompt:
  `design/development/agent_prompts/p0_asset_batch_14_status_compact_icons.md`
- Batch 14 status compact icon review note:
  `design/development/asset_review/p0_status_compact_icons_2026-06-14.md`
- Batch 15 route reward detail badge deterministic builder:
  `design/development/tools/build_route_reward_detail_badges.ps1`
- Batch 15 route reward detail badge production prompt:
  `design/development/prompts/p0_route_reward_detail_badges.md`
- Batch 15 route reward detail badge execution prompt:
  `design/development/agent_prompts/p0_asset_batch_15_route_reward_detail_badges.md`
- Batch 15 route reward detail badge review note:
  `design/development/asset_review/p0_route_reward_detail_badges_2026-06-14.md`
- Batch 16 authority blessing seal deterministic builder:
  `design/development/tools/build_authority_blessing_seals.ps1`
- Batch 16 authority blessing seal production prompt:
  `design/development/prompts/p0_authority_blessing_seals.md`
- Batch 16 authority blessing seal execution prompt:
  `design/development/agent_prompts/p0_asset_batch_16_authority_blessing_seals.md`
- Batch 16 authority blessing seal review note:
  `design/development/asset_review/p0_authority_blessing_seals_2026-06-14.md`
- Batch 05 source-locked starter cat derivative candidates:
  `design/development/asset_candidates/starter_cats/batch_05_source_locked_derivatives_2026-06-14/README.md`
- Batch 05 candidate manifest:
  `design/development/asset_candidates/starter_cats/batch_05_source_locked_derivatives_2026-06-14/starter_cat_batch05_candidate_manifest.csv`
- Batch 05 deterministic builder:
  `design/development/tools/build_starter_cat_derivative_candidates.py`
- Starter cat source-lock packet:
  `design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.md`
- Starter cat source-lock packet CSV:
  `design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.csv`
- Batch 07 starter cat source-lock packet builder:
  `design/development/tools/build_starter_cat_source_lock_packet.ps1`
- Batch 07 starter cat source-lock packet prompt:
  `design/development/agent_prompts/p0_asset_batch_07_starter_cat_source_lock_packet.md`
- Current Play Mode screenshot file evidence:
  `P0PlayModeScreenshotFileEvidence` reports the current `11/11` expected
  screenshots after the 2026-06-26 Play Mode smoke refresh. These captures are
  baseline runtime evidence, not formal starter-cat replacement approval.
- Offline asset production readiness gate:
  `P0AssetProductionReadiness` / `P0 Offline Asset Production Readiness`
- Starter cat formal import readiness gate:
  `P0StarterCatFormalImportReadiness` / `Starter Cat Formal Import Readiness`
- Unity generation menu:
  `TheCat/P0/Write P0 Asset Review Packet`

## Batch 05 Starter Cat Candidate Status

Batch 05 currently has source-locked baseline derivatives for Saiban, Nephthys,
and Suzune:

- 12 candidate PNGs total:
  - 3 `combat_sprite_refinement_512` baseline candidates
  - 3 `front_animation_keyframe_512` idle-center baseline candidates
  - 3 `hud_avatar_256` crops
  - 3 `skill_icon_motif_128` source-sprite crops
- 3 per-cat review notes with source SHA, current sprite SHA, active screenshot
  filename, trait coverage, and rejection rules.
- 3 per-cat review sheets comparing the locked colored turnaround, current
  Unity sprite, and candidate derivatives.

These are candidate-review assets only. They are blocked from Unity import until
the registered active-cat Play Mode screenshots receive explicit side-by-side
colored-turnaround comparison approval notes.

The block is now enforced in code. `P0StarterCatFormalImportReadiness` must
remain valid and report either `Blocked` or `Approved`; current state is
`Blocked`, import allowed `no`, 3/3 explicit block notes, and 3/3 registered
active-cat screenshots. Do not copy Batch 05 cat candidates into
`Assets/TheCat/Art/Characters/Sprites` until the gate reports `Approved`.

## Batch 07 Starter Cat Source-Lock Packet Status

Batch 07 adds a preflight evidence packet for future Saiban, Nephthys, and
Suzune asset work. It does not generate new cat art and does not approve Unity
imports.

- Source-lock packet:
  `design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.md`
- CSV source-lock table:
  `design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.csv`
- Deterministic builder:
  `design/development/tools/build_starter_cat_source_lock_packet.ps1`
- Code gate:
  `P0StarterCatSourceLockPacketEvidence`
- Readiness integration:
  `P0AssetReviewPacket` and `P0AssetProductionReadiness`

The packet records each starter cat's colored turnaround path and SHA-256,
current locked Unity sprite path and SHA-256, runtime binding id, active-cat
Play Mode screenshot filename, Batch 05 candidate review note, and Batch 05
review sheet. It also repeats the explicit `do not import into Unity yet`
decision because registered active-cat screenshots do not approve import by
themselves; formal replacement still requires colored-turnaround comparison
approval notes.

This batch exists because starter-cat assets are high-risk: any future
derivative, including AI-generated or hand-touched art, must pass through this
packet before review. The colored three-view turnaround remains the hard source
of truth.

## Batch 06 Route Node Icon Status

Batch 06 adds deterministic route-map icon assets for the seven non-Boss P0
route node types. The Boss node continues to use the source-locked Call Tyrant
route icon.

- 7 new 128x128 transparent route node icons:
  - defense
  - elite
  - partner
  - shop
  - dream event
  - blessing offering
  - rest nest
- Runtime visual baseline after Batch 06 was 28 bindings:
  - previous 21 runtime slots
  - 7 new non-Boss route node icons
- `P0RouteNodePresenter` now exposes icon references for all 8 node types.
- `P0RouteMapPresenter` already carries those references into current-node and
  route-option cards, where the graybox route map can draw them through
  `P0ImGuiVisualAssetDrawer`.

This batch is non-cat UI production. It does not approve or modify starter-cat
candidate imports.

## Batch 08 UI Shell Status

Batch 08 adds deterministic, non-cat UI shell assets for main menu, shared
panel/button framing, and settlement reward feedback.

- 6 new UI shell PNGs:
  - main menu dream-entry background
  - title logo frame
  - dreamglass panel frame
  - primary button frame
  - fish treat reward icon
  - dream shard reward icon
- Runtime visual baseline is now 34 bindings:
  - previous 28 runtime slots
  - 6 UI shell slots for main menu, UI frame, and settlement rewards
- `P0VisualAssetCatalog` now exposes lookup helpers for menu, panel, button,
  and reward icon assets.
- `P0AssetManifestCoverage`, `P0AssetImportReadiness`,
  `P0AssetMetaImportSettingsReadiness`, `P0RuntimeVisualBindingCoverage`,
  `P0AssetReviewPacket`, and `P0AssetProductionReadiness` expect the 38-row
  manifest and 34 runtime visual bindings.
- The runtime visual contact sheet has been regenerated and reports 34
  manifest-backed bindings.

This batch is non-cat UI production. It does not approve or modify starter-cat
candidate imports. Starter-cat formal import remains blocked behind active-cat
Play Mode screenshots and colored three-view turnaround comparison.

## Batch 09 Battle Feedback VFX Status

Batch 09 adds deterministic, non-cat battle feedback VFX for the current
graybox feedback card and future skill/interaction polish.

- 6 new 256x256 transparent VFX PNGs:
  - hit spark
  - bed shield pulse
  - Sleep Stable wave
  - litter cleanse
  - feeder kibble
  - enemy mark ring
- Runtime visual baseline after Batch 09 was 40 bindings:
  - previous 34 runtime slots
  - 6 battle feedback VFX slots under the `battle_feedback` surface
- `P0BattleFeedbackVisualPresenter` now routes feedback kinds and details to
  manifest-backed VFX references.
- `GrayboxBattleController` draws feedback VFX icons in the IMGUI feedback
  card, keeping the current graybox loop useful for screenshot review.
- `P0AssetManifestCoverage`, `P0AssetImportReadiness`,
  `P0AssetMetaImportSettingsReadiness`, `P0RuntimeVisualBindingCoverage`,
  `P0AssetReviewPacket`, and `P0AssetProductionReadiness` expected the
  44-row manifest and 40 runtime visual bindings.
- The runtime visual contact sheet was regenerated and reported 40
  manifest-backed bindings.

This batch is non-cat VFX production. It does not approve or modify
starter-cat candidate imports. Starter-cat formal import remains blocked behind
active-cat Play Mode screenshots and colored three-view turnaround comparison.

## Batch 10 Enemy Warning VFX Status

Batch 10 adds deterministic, source-locked, non-cat warning VFX for P0 enemy
and Boss pressure telegraphs.

- 4 new 256x256 transparent VFX PNGs:
  - Black Mud bed claw near-bed warning
  - Cold Light beam ranged-pressure warning
  - Call Tyrant app throw warning
  - Call Tyrant summon portal warning
- Runtime visual baseline is now 44 bindings:
  - previous 40 runtime slots
  - 4 enemy/Boss warning VFX slots under the `battle_warning` surface
- `P0EnemyWarningIndicatorPresenter` now routes warning kinds to
  manifest-backed VFX:
  - Black Mud bed contact uses the Black Mud claw warning.
  - Cold Light ranged pressure uses the Cold Light beam warning.
  - Call Tyrant boss throw uses the app throw warning.
  - Call Tyrant boss summon uses the summon portal warning.
- `P0AssetManifestCoverage`, `P0AssetImportReadiness`,
  `P0AssetMetaImportSettingsReadiness`, `P0RuntimeVisualBindingCoverage`,
  `P0AssetReviewPacket`, and `P0AssetProductionReadiness` were advanced to the
  Batch 10 baseline of 48 manifest rows and 44 runtime visual bindings.
- The Batch 10 runtime visual contact sheet reported 44 manifest-backed
  bindings.

This batch is non-cat enemy/Boss VFX production. It does not approve or modify
starter-cat candidate imports. Starter-cat formal import remains blocked behind
active-cat Play Mode screenshots and colored three-view turnaround comparison.

## Batch 11 Enemy Animation Framesheet Status

Batch 11 adds deterministic, source-cropped, non-cat framesheets for P0 enemy
and Boss battle-world animation.

- 3 new `1024x256` transparent 4-frame PNG framesheets:
  - Black Mud Nightmare move/crawl loop
  - Cold Light Shadow cast/pressure loop
  - Call Tyrant Boss pattern / APP throw loop
- Runtime visual baseline after Batch 11 was 47 bindings:
  - previous 44 runtime slots
  - 3 enemy animation framesheet slots under the `battle_world` surface
- `P0VisualAssetCatalog.GetEnemyAnimationFramesheet(enemyId)` now routes:
  - Black Mud Nightmare -> `black_mud_animation`
  - Cold Light Shadow -> `cold_light_animation`
  - Call Tyrant -> `call_tyrant_animation`
- `P0AssetManifestCoverage`, `P0AssetImportReadiness`,
  `P0AssetMetaImportSettingsReadiness`, `P0RuntimeVisualBindingCoverage`,
  `P0AssetReviewPacket`, and `P0AssetProductionReadiness` advanced to the
  Batch 11 baseline of 51 manifest rows and 47 runtime visual bindings.
- The runtime visual contact sheet has been regenerated and reports 47
  manifest-backed bindings.

This batch is non-cat enemy/Boss animation production. It uses source-image
crop/extraction from locked enemy animation sheets and does not approve or
modify starter-cat candidate imports. Starter-cat formal import remains blocked
behind active-cat Play Mode screenshots and colored three-view turnaround
comparison.

## Batch 12 Route Choice Icon Status

Batch 12 adds deterministic, non-cat route choice icons for route reward-choice
cards and future route-map UI polish.

- 6 new 128x128 transparent UI PNGs:
  - partner recruit
  - purchase supply
  - gain authority blessing
  - upgrade authority blessing
  - rest supply
  - dream-event modifier
- Runtime visual baseline is now 53 bindings:
  - previous 47 runtime slots
  - 6 route-choice slots under the `route_choice` surface
- Existing fish-treat and dream-shard reward icons continue to cover
  `GainFishTreats`, `PurchaseFishTreats`, and `GainDreamShards`.
- `P0VisualAssetCatalog.GetRouteChoiceIcon(choiceType)` now routes:
  - `RecruitPartner` -> partner recruit icon
  - `PurchaseSupply` -> purchase supply icon
  - `GainAuthorityBlessing` -> authority blessing icon
  - `UpgradeAuthorityBlessing` -> authority upgrade icon
  - `RestSupply` -> rest supply icon
  - `DreamEventModifier` -> dream-event modifier icon
- `P0RouteMapPresenter` carries these references into reward-choice cards, and
  `RouteMapController` draws them inline in the IMGUI route-map choice surface.
- `P0AssetManifestCoverage`, `P0AssetImportReadiness`,
  `P0AssetMetaImportSettingsReadiness`, `P0RuntimeVisualBindingCoverage`,
  `P0AssetReviewPacket`, and `P0AssetProductionReadiness` now expect the
  57-row manifest and 53 runtime visual bindings.
- The runtime visual contact sheet has been regenerated and reports 53
  manifest-backed bindings.

This batch is non-cat route/reward UI production. It does not approve or modify
starter-cat candidate imports. Starter-cat formal import remains blocked behind
active-cat Play Mode screenshots and colored three-view turnaround comparison.

## Batch 16 Authority Blessing Seal Status

Batch 16 adds deterministic, non-cat 128x128 authority blessing seals for the
three P0 authority blessings.

- 3 new 128x128 transparent UI PNGs:
  - Oath Bedline seal
  - Moon-Sand Dominion seal
  - Lullaby Rhythm seal
- Runtime visual baseline is now 71 bindings:
  - previous 68 runtime slots
  - 3 blessing seal slots under the `blessing_detail` surface
- `P0VisualAssetCatalog.GetAuthorityBlessingSeal(blessingId)` resolves the
  three P0 blessing ids to their specific seal assets.
- `P0VisualAssetCatalog.GetRouteChoiceIcon(RouteRewardChoice)` now routes gain
  and upgrade authority blessing choices to specific seals before falling back
  to the generic route choice icon.
- `P0RouteMapSurfaceCoverage` gates `layer_07_blessing` so Oath Bedline,
  Moon-Sand Dominion, and Lullaby Rhythm each show their specific seal asset.
- `P0AssetManifestCoverage`, `P0AssetImportReadiness`,
  `P0AssetMetaImportSettingsReadiness`, `P0RuntimeVisualBindingCoverage`,
  `P0AssetReviewPacket`, and `P0AssetProductionReadiness` now expect the
  75-row manifest and 71 runtime visual bindings.
- The runtime visual contact sheet has been regenerated and reports 71
  manifest-backed bindings.

This batch is non-cat blessing UI production. It does not approve or modify
starter-cat candidate imports. Starter-cat formal import remains blocked behind
active-cat Play Mode screenshots and colored three-view turnaround comparison.

## Batch 13 Route Reward Card Frame Status

Batch 13 adds deterministic, non-cat route reward card frames for the current
route-map reward-choice surface and future final UI binding.

- 5 new 512x256 transparent frame PNGs:
  - partner recruit card
  - shop supply card
  - authority blessing card
  - dream-event modifier card
  - rest-nest recovery card
- Runtime visual baseline after Batch 13 was 58 bindings:
  - previous 53 runtime slots
  - 5 route reward card frame slots under the `route_reward_card` surface
- `P0VisualAssetCatalog.GetRouteRewardCardFrame(nodeType)` now routes:
  - `Partner` -> partner route card frame
  - `Shop` -> shop route card frame
  - `BlessingOffering` -> blessing route card frame
  - `DreamEvent` -> dream-event route card frame
  - `RestNest` -> rest-nest route card frame
- `P0RouteMapPresenter` carries these references into reward-choice cards, and
  `RouteMapController` draws framed route choice cards with inline icons.
- At the Batch 13 baseline, `P0AssetManifestCoverage`, `P0AssetImportReadiness`,
  `P0AssetMetaImportSettingsReadiness`, `P0RuntimeVisualBindingCoverage`,
  `P0AssetReviewPacket`, and `P0AssetProductionReadiness` expected the
  62-row manifest and 58 runtime visual bindings.
- The runtime visual contact sheet was regenerated and reported 58
  manifest-backed bindings.

This batch is non-cat route/reward UI production. It does not approve or modify
starter-cat candidate imports. Starter-cat formal import remains blocked behind
active-cat Play Mode screenshots and colored three-view turnaround comparison.

## Batch 14 Status Compact Icon Status

Batch 14 adds deterministic, non-cat 32px compact status HUD icons for the
current battle HUD and future final UI binding.

- 5 new 32x32 transparent UI PNGs:
  - Sleep Stable compact icon
  - Slow compact icon
  - Knockback compact icon
  - Mark compact icon
  - Shield compact icon
- Runtime visual baseline is now 63 bindings:
  - previous 58 runtime slots
  - 5 compact status slots under the `status_hud` surface
- `P0VisualAssetCatalog.GetCompactStatusIcon(statusTagId)` now routes the five
  P0 status tags to their compact icon assets.
- `P0StatusHudPresenter` carries both 64px full icons and 32px compact icons,
  and the graybox HUD draws the compact icon first for in-row readability.
- `P0AssetManifestCoverage`, `P0AssetImportReadiness`,
  `P0AssetMetaImportSettingsReadiness`, `P0RuntimeVisualBindingCoverage`,
  `P0AssetReviewPacket`, and `P0AssetProductionReadiness` now expect the
  67-row manifest and 63 runtime visual bindings.
- The runtime visual contact sheet has been regenerated and reports 63
  manifest-backed bindings.

This batch is non-cat status HUD production. It does not approve or modify
starter-cat candidate imports. Starter-cat formal import remains blocked behind
active-cat Play Mode screenshots and colored three-view turnaround comparison.

## Batch 15 Route Reward Detail Badge Status

Batch 15 adds deterministic, non-cat 192x64 route reward detail badges for the
route reward-choice cards.

- 5 new 192x64 transparent UI PNGs:
  - gain detail badge
  - cost detail badge
  - recovery detail badge
  - risk detail badge
  - upgrade detail badge
- Runtime visual baseline is now 68 bindings:
  - previous 63 runtime slots
  - 5 reward detail badge slots under the `route_reward_detail` surface
- `P0VisualAssetCatalog.GetRouteRewardDetailBadge(choice)` now routes reward
  choices to effect-class badges from their typed payload.
- `P0RouteMapRewardChoiceCard` carries the detail badge reference, and the
  graybox route-map IMGUI path draws it on the right side of reward cards.
- `P0AssetManifestCoverage`, `P0AssetImportReadiness`,
  `P0AssetMetaImportSettingsReadiness`, `P0RuntimeVisualBindingCoverage`,
  `P0AssetReviewPacket`, and `P0AssetProductionReadiness` now expect the
  72-row manifest and 68 runtime visual bindings.
- The runtime visual contact sheet has been regenerated and reports 68
  manifest-backed bindings.

This batch is non-cat route/reward UI production. It does not approve or modify
starter-cat candidate imports. Starter-cat formal import remains blocked behind
active-cat Play Mode screenshots and colored three-view turnaround comparison.

## Batch 17 Starter Cat Turnaround Conformance Gate Status

Batch 17 is a gate batch, not an image-generation batch. It hardens the user
requirement that starter cat art must strictly match the colored three-view
turnarounds.

- New review artifact:
  `design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`
- New execution prompt:
  `design/development/agent_prompts/p0_asset_batch_17_starter_cat_turnaround_conformance_gate.md`
- New code gate:
  `P0StarterCatTurnaroundConformanceSpec`
- Review-packet integration:
  `P0AssetReviewPacket` now includes a "Starter Cat Turnaround Conformance
  Spec" section with front, side, back, palette, prop/costume, and prohibited
  drift anchors.
- Offline-readiness integration:
  `P0AssetProductionReadiness` now requires the conformance spec before
  systematic asset production is considered ready.

Gate counts:

- 3 starter cats
- 27 required front/side/back view anchors
- 9 palette anchors
- 9 prop/costume anchors
- 12 prohibited drift rules

This gate does not approve or modify starter-cat candidate imports. It raises
the standard for future Saiban, Nephthys, and Suzune generation: even front-view
derivatives must preserve the colored turnaround's side/back identity anchors,
palette, props, costume, and non-human cat proportions.

## Batch 18 Starter Cat Strict Candidate Production Readiness

Batch 18 is now prepared as the first strict cat-candidate production contract.
It should run one starter cat at a time and keep all outputs outside `Assets`
until formal import is approved.

- New execution prompt:
  `design/development/agent_prompts/p0_asset_batch_18_starter_cat_strict_candidate_production.md`
- Updated candidate generator:
  `design/development/tools/build_starter_cat_derivative_candidates.py`
- Updated candidate evidence gate:
  `P0StarterCatDerivativeCandidateEvidence`
- Batch 05 candidate review notes now include:
  - source conformance spec mention
  - front-view anchors
  - side-view anchors
  - back-view anchors
  - palette anchors
  - prop/costume anchors
  - prohibited drift rules

Current offline status:

- 3/3 candidate review notes mention the conformance spec.
- 3/3 candidate review notes include front-view anchor sections.
- 3/3 candidate review notes include side-view anchor sections.
- 3/3 candidate review notes include back-view anchor sections.
- 3/3 candidate review notes include palette sections.
- 3/3 candidate review notes include prop/costume sections.
- 3/3 candidate review notes include prohibited-drift sections.

The next systematic asset-production step can now be a single-cat candidate
batch, starting with Saiban unless the main session assigns a different cat.
The generated candidate must remain review-only until active-cat Play Mode
screenshots pass the colored three-view turnaround comparison.

## Batch 19 Non-Battle Node Summary Banner Status

Batch 19 adds deterministic, non-cat route-node summary banners for the
route-map current-node surface.

- 3 new 512x160 transparent UI PNGs:
  - shop summary banner
  - dream-event summary banner
  - rest-nest summary banner
- Runtime visual baseline is now 74 bindings:
  - previous 71 runtime slots
  - 3 `route_node_summary` slots under `summary_banner`
- `P0VisualAssetCatalog.GetRouteNodeSummaryBanner(nodeType)` now routes:
  - `Shop` -> shop summary banner
  - `DreamEvent` -> dream-event summary banner
  - `RestNest` -> rest-nest summary banner
- `P0RouteMapCurrentNodeCard` carries the optional summary banner reference,
  and the graybox route-map IMGUI path draws it for those non-battle nodes.
- `P0AssetManifestCoverage`, `P0RuntimeVisualBindingCoverage`,
  `P0RouteMapSurfaceCoverage`, `P0AssetReviewPacket`, and
  `P0AssetProductionReadiness` now expect the 78-row manifest and 74 runtime
  visual bindings.
- The runtime visual contact sheet has been regenerated and reports 74
  manifest-backed bindings.
- Review note:
  `design/development/asset_review/p0_nonbattle_node_summary_banners_2026-06-14.md`

This batch is non-cat route UI production. It does not approve or modify
starter-cat candidate imports. Starter-cat formal import remains blocked behind
active-cat Play Mode screenshots and colored three-view turnaround comparison.

## Batch 20 Shop Item Card Status

Batch 20 adds deterministic, non-cat shop item cards for the route-map shop
reward-choice surface.

- 4 new 384x160 transparent UI PNGs:
  - bed patch item card
  - litter sachet item card
  - late kibble item card
  - free sample item card
- Runtime visual baseline is now 78 bindings:
  - previous 74 runtime slots
  - 4 `shop_item_card` slots under `item_card`
- `P0VisualAssetCatalog.GetShopItemCard(choice)` now routes:
  - `shop_bed_patch` -> bed patch card
  - `shop_litter_sachet` -> litter sachet card
  - `shop_late_kibble` -> late kibble card
  - `shop_free_sample` -> free sample card
- `P0RouteMapRewardChoiceCard` carries the optional item-card reference, and
  the graybox route-map IMGUI path draws it above matching shop choice cards.
- `P0AssetManifestCoverage`, `P0RuntimeVisualBindingCoverage`,
  `P0RouteMapSurfaceCoverage`, `P0AssetReviewPacket`, and
  `P0AssetProductionReadiness` now expect the 82-row manifest and 78 runtime
  visual bindings.
- The runtime visual contact sheet has been regenerated and reports 78
  manifest-backed bindings.
- Review note:
  `design/development/asset_review/p0_shop_item_cards_2026-06-14.md`

This batch is non-cat shop UI production. It does not approve or modify
starter-cat candidate imports. Starter-cat formal import remains blocked behind
active-cat Play Mode screenshots and colored three-view turnaround comparison.

## Batch 21 Dream Event Choice Card Status

Batch 21 adds deterministic, non-cat DreamEvent choice cards for the route-map
dream-event reward-choice surface.

- 3 new 384x160 transparent UI PNGs:
  - clear notifications choice card
  - catnip residue choice card
  - mark all read choice card
- Runtime visual baseline is now 81 bindings:
  - previous 78 runtime slots
  - 3 `dream_event_choice_card` slots under `choice_card`
- `P0VisualAssetCatalog.GetDreamEventChoiceCard(choice)` now routes:
  - `dream_event_clear_notifications` -> red-dot cleanup card
  - `dream_event_catnip_residue` -> residue risk/reward card
  - `dream_event_mark_all_read` -> message check and sleep card
- `P0VisualAssetCatalog.GetRouteChoiceCard(choice)` now provides the generic
  route-choice card lookup used by the route-map presenter; shop and DreamEvent
  cards share the existing `P0RouteMapRewardChoiceCard.ItemCardAsset` slot.
- `P0AssetManifestCoverage`, `P0RuntimeVisualBindingCoverage`,
  `P0RouteMapSurfaceCoverage`, `P0AssetReviewPacket`, and
  `P0AssetProductionReadiness` now expect the 85-row manifest and 81 runtime
  visual bindings.
- The runtime visual contact sheet has been regenerated and reports 81
  manifest-backed bindings.
- Review note:
  `design/development/asset_review/p0_dream_event_choice_cards_2026-06-14.md`

This batch is non-cat dream-event UI production. It does not approve or modify
starter-cat candidate imports. Starter-cat formal import remains blocked behind
active-cat Play Mode screenshots and colored three-view turnaround comparison.

## Batch 22 RestNest Recovery Card Status

Batch 22 adds a deterministic, non-cat RestNest recovery card for the route-map
rest-nest reward-choice surface.

- 1 new 384x160 transparent UI PNG:
  - rest nest recovery choice card
- Runtime visual baseline is now 82 bindings:
  - previous 81 runtime slots
  - 1 `rest_nest_choice_card` slot under `choice_card`
- `P0VisualAssetCatalog.GetRestNestChoiceCard(choice)` now routes:
  - `rest_nest_recovery` -> recovery package card
- `P0VisualAssetCatalog.GetRouteChoiceCard(choice)` now provides the generic
  route-choice card lookup across shop, DreamEvent, and RestNest cards through
  the existing `P0RouteMapRewardChoiceCard.ItemCardAsset` slot.
- `P0AssetManifestCoverage`, `P0RuntimeVisualBindingCoverage`,
  `P0RouteMapSurfaceCoverage`, `P0AssetReviewPacket`, and
  `P0AssetProductionReadiness` now expect the 86-row manifest and 82 runtime
  visual bindings.
- The runtime visual contact sheet has been regenerated and reports 82
  manifest-backed bindings.
- Review note:
  `design/development/asset_review/p0_rest_nest_recovery_card_2026-06-14.md`

This batch is non-cat RestNest UI production. It does not approve or modify
starter-cat candidate imports. Starter-cat formal import remains blocked behind
active-cat Play Mode screenshots and colored three-view turnaround comparison.

## Batch 23 Partner Choice Card Status

Batch 23 adds deterministic, non-cat partner choice cards for the route-map
partner reward-choice surface.

- 2 new 384x160 transparent UI PNGs:
  - Shadowmaru preview invitation choice card
  - duplicate partner fallback supply choice card
- Runtime visual baseline is now 84 bindings:
  - previous 82 runtime slots
  - 2 `partner_choice_card` slots under `choice_card`
- `P0VisualAssetCatalog.GetPartnerChoiceCard(choice)` now routes:
  - `partner_shadowmaru_preview` -> symbolic partner invitation card
  - `partner_preview_duplicate_supply` -> symbolic night-fish fallback card
- `P0VisualAssetCatalog.GetRouteChoiceCard(choice)` now resolves shop,
  DreamEvent, RestNest, and partner concrete choice cards through the existing
  `P0RouteMapRewardChoiceCard.ItemCardAsset` slot.
- `P0AssetManifestCoverage`, `P0RuntimeVisualBindingCoverage`,
  `P0RouteMapSurfaceCoverage`, `P0AssetReviewPacket`, and
  `P0AssetProductionReadiness` now expect the 88-row manifest and 84 runtime
  visual bindings.
- The runtime visual contact sheet has been regenerated and reports 84
  manifest-backed bindings.
- Review note:
  `design/development/asset_review/p0_partner_choice_cards_2026-06-14.md`

This batch is non-cat partner UI production. It does not define Shadowmaru body
art and does not approve or modify starter-cat candidate imports. Starter-cat
formal import remains blocked behind active-cat Play Mode screenshots and
colored three-view turnaround comparison.

## Batch 24 Blessing Choice Card Status

Batch 24 adds deterministic, non-cat authority blessing choice cards for the
route-map blessing reward-choice surface.

- 3 new 384x160 transparent UI PNGs:
  - Oath Bedline authority choice card
  - Moon-Sand Dominion authority choice card
  - Lullaby Rhythm authority choice card
- Runtime visual baseline is now 87 bindings:
  - previous 84 runtime slots
  - 3 `blessing_choice_card` slots under `choice_card`
- `P0VisualAssetCatalog.GetAuthorityBlessingChoiceCard(choice)` now routes:
  - `authority_oath_bedline` -> symbolic bedline shield card
  - `authority_dominion_sandglass` -> symbolic moon sandglass card
  - `authority_rhythm_lullaby` -> symbolic lullaby rhythm card
- `P0VisualAssetCatalog.GetRouteChoiceCard(choice)` now resolves shop,
  DreamEvent, RestNest, partner, and authority blessing concrete choice cards
  through the existing `P0RouteMapRewardChoiceCard.ItemCardAsset` slot.
- `P0AssetManifestCoverage`, `P0RuntimeVisualBindingCoverage`,
  `P0RouteMapSurfaceCoverage`, `P0AssetReviewPacket`, and
  `P0AssetProductionReadiness` now expect the 91-row manifest and 87 runtime
  visual bindings.
- The runtime visual contact sheet has been regenerated and reports 87
  manifest-backed bindings.
- Review note:
  `design/development/asset_review/p0_blessing_choice_cards_2026-06-14.md`

This batch is non-cat blessing UI production. It does not portray Saiban,
Nephthys, or Suzune, and does not approve or modify starter-cat candidate
imports. Starter-cat formal import remains blocked behind active-cat Play Mode
screenshots and colored three-view turnaround comparison.

## Batch 25 Result Settlement Banner Status

Batch 25 adds deterministic, non-cat outcome banners for battle result and
run-settlement surfaces.

- 4 new 512x160 transparent UI PNGs:
  - battle result victory banner
  - battle result defeat banner
  - cleared-run settlement banner
  - failed-run settlement banner
- Runtime visual baseline is now 91 bindings:
  - previous 87 runtime slots
  - 4 `outcome_banner` slots across `battle_result` and `settlement`
- `P0VisualAssetCatalog.GetBattleResultOutcomeBanner(outcome)` now routes:
  - `BattleOutcome.Victory` -> battle victory banner
  - `BattleOutcome.Defeat` -> battle defeat banner
- `P0VisualAssetCatalog.GetSettlementOutcomeBanner(isCleared)` now routes:
  - cleared route -> cleared-run settlement banner
  - failed route -> failed-run settlement banner
- `P0BattleResultSurface` and `P0RouteMapSurface` now expose the outcome banner
  references used by `GrayboxBattleController` and `RouteMapController`.
- `P0AssetManifestCoverage`, `P0RuntimeVisualBindingCoverage`,
  `P0BattleResultCoverage`, `P0RouteMapSurfaceCoverage`,
  `P0AssetReviewPacket`, and `P0AssetProductionReadiness` now expect the
  95-row manifest and 91 runtime visual bindings.
- The runtime visual contact sheet has been regenerated and reports 91
  manifest-backed bindings.
- Review note:
  `design/development/asset_review/p0_result_settlement_banners_2026-06-14.md`

This batch is non-cat result UI production. It does not portray Saiban,
Nephthys, or Suzune, and does not approve or modify starter-cat candidate
imports. Starter-cat formal import remains blocked behind active-cat Play Mode
screenshots and colored three-view turnaround comparison.

## Batch 26 Starter Cat Candidate Gate Status

Batch 26 does not generate or import Unity art. It turns starter-cat asset
production into a stricter candidate-pack gate before any future Saiban,
Nephthys, or Suzune visual replacement.

- New validator:
  `design/development/tools/validate_starter_cat_candidate_pack.ps1`
- New execution prompt:
  `design/development/agent_prompts/p0_asset_batch_26_starter_cat_candidate_gate.md`
- New review note:
  `design/development/asset_review/p0_starter_cat_candidate_gate_2026-06-14.md`
- The validator confirms the existing Batch 05 candidate pack contains:
  - 12 candidate PNG rows
  - 3 per-cat review notes
  - 3 per-cat review sheets
  - 4 allowed derivative types for each starter cat
- The validator also confirms candidate PNGs and review sheets remain outside
  `Assets` and have no Unity `.meta` files.

This gate is now the required preflight before producing more starter-cat
assets. New cat art may be generated only as review candidates under
`design/development/asset_candidates/starter_cats`; formal import remains
blocked until active-cat Play Mode screenshots match the colored three-view
turnaround contact sheet.

## Batch 27 Core Gauge Bar Status

Batch 27 adds deterministic, non-cat HUD gauge bars for the four P0 core
values.

- 8 new 384x48 transparent UI PNGs:
  - owner sleep frame/fill
  - cat HP frame/fill
  - team poop frame/fill
  - team hunger frame/fill
- At the time of Batch 27, runtime visual baseline moved to 99 bindings:
  - previous 91 runtime slots
  - 8 `core_gauge.*` frame/fill slots across `battle_hud` and `cat_hud`
- `CoreValuePresentation` now carries:
  - visual icon
  - gauge frame
  - gauge fill
  - fill ratio
- `P0CatHudPresenter` uses the generic cat HP gauge pair for cat HUD HP bars.
- `P0AssetManifestCoverage`, `P0RuntimeVisualBindingCoverage`,
  `P0AssetGenerationBatchCoverage`, `P0AssetReviewPacket`, and
  `P0AssetProductionReadiness` expected the then-current 103-row manifest and
  99 runtime visual bindings.
- The runtime visual contact sheet was regenerated for that 99-binding
  baseline. Current baseline updates are recorded in later batch sections.
- Review note:
  `design/development/asset_review/p0_core_gauge_bars_2026-06-14.md`

This batch is non-cat HUD UI production. It does not portray Saiban, Nephthys,
or Suzune, and does not approve or modify starter-cat candidate imports.
Starter-cat formal import remains blocked behind active-cat Play Mode
screenshots and colored three-view turnaround comparison.

## Starter Cat Strict Candidate Evidence Gate

Systematic starter-cat asset production is now split between Codex and Unity.
Codex is allowed to generate, chroma-key, cut out, package, and review raster
asset candidates under `design/development/asset_candidates`. Unity is the
acceptance environment: imported assets must pass AssetDatabase refresh, Sprite
import settings, active-cat Play Mode screenshots, Console checks, scene/prefab
binding, HUD readability, and runtime scale review before any formal install.

The current strict candidate chain is:

- Saiban: Batch 49 low-drift refinement
- Nephthys: Batch 50 strict AI generation candidate
- Suzune: Batch 51 strict AI generation candidate

`P0StarterCatStrictCandidateEvidence` records these three candidates as
review-only evidence. `P0AssetReviewPacket` and `P0AssetProductionReadiness`
now require that evidence, but the requirement does not approve Unity import.
Formal starter-cat import remains blocked until `05-active-cat-saiban.png`,
`06-active-cat-nephthys.png`, and `07-active-cat-suzune.png` receive explicit
approval against the locked colored three-view turnarounds.

## P0 Systematic Asset Production Queue

Asset production now has an explicit queue rather than relying on one-off batch
notes. The queue protects the pipeline boundary:

- Codex may produce raster candidates outside Unity because this is where image
  generation, chroma-key cleanup, transparent PNG packaging, contact sheets,
  review sheets, manifests, and prompt records can be produced.
- Unity is reserved for formal install and runtime acceptance: `.meta` files,
  Sprite import settings, AssetDatabase refresh, scene/prefab binding, Console
  checks, active screenshots, HUD readability, and runtime scale.
- Candidate folders must stay under `design/development/asset_candidates`.
- Formal Unity import roots must stay under `Assets`.
- Candidate batches must list forbidden write roots so production agents cannot
  accidentally install assets while they are only generating review material.

Current queue:

| Batch | Queue item | State | Reason |
| --- | --- | --- | --- |
| 52 | Starter-cat active screenshot validation | Blocked by Unity validation | Batch 49/50/51 must match the colored three-view turnaround sheets in active-cat Play Mode screenshots before any install. |
| 53 | Core-enemy active screenshot validation | Blocked by Unity validation | Enemy candidates need runtime screenshots, Console checks, and prefab/scene binding checks. |
| 54 | Bedroom interactable candidates | Candidate pack complete pending Unity review | Bed, litter box, and feeder props exist as review-only candidates outside Unity. |
| 60 | Skill HUD feedback installed Unity validation | Blocked by Unity validation | Installed non-cat HUD/VFX assets need battle-HUD screenshots, timing review, Console checks, and prefab/scene binding. |
| 61 | Starter skill VFX installed Unity validation | Blocked by Unity validation | Installed symbolic starter-skill VFX need skill-cast screenshots, timing review, Console checks, and prefab/scene binding. |
| 62 | Runtime control icon candidates | Candidate pack complete pending Unity review | Pause, resume, speed, and restart icons stay review-only until HUD screenshot and Console checks pass. |
| 63 | Runtime control panel candidates | Candidate pack complete pending Unity review | Pause overlay, speed selector, restart plate, and keyboard hint stay review-only until HUD scale checks pass. |
| 64 | Secondary enemy warning candidates | Candidate pack complete pending Unity review | Future enemy warning VFX stay outside Unity until enemy-prefab and screenshot gates exist. |
| 65 | Route-map readability candidates | Candidate pack complete pending Unity review | Current/selected/path/Boss route readability accents stay outside Unity until route-map screenshots pass. |
| 56 | Formal Unity install decision packet | Blocked by Unity validation | Install decisions require active screenshot evidence, Console state, Sprite import checks, runtime scale, and scene/prefab binding. |
| 66 | Systematic asset master plan | Spec/control batch complete | Gap matrix confirms the next safe Codex lane is non-cat bedroom interaction affordance candidates. |
| 67 | Bedroom interaction affordance candidates | Candidate pack complete pending Unity review | Bed, litter box, feeder, blocked interaction, and range affordances stay outside Unity until interaction screenshots and timing checks pass. |
| 76 | Owner sleep-state animation candidate | Candidate pack complete pending Unity review | V002 owner/pillow/blanket overlay source and 24 padded alpha frames stay outside Unity until slicing, pivot, overlay-vs-bed-layer, timing, screenshot, and Console checks pass. |
| 77 | Owner sleep status icon candidates | Candidate pack complete pending Unity review | Twelve symbolic non-cat owner sleep-state HUD/settlement icons stay outside Unity until HUD/settlement screenshots, 64px/32px readability, dark/light/cooldown-overlay checks, Sprite import settings, scene/prefab binding, and Console checks pass. |

For starter cats, visual consistency is stricter than general asset polish. A
candidate is not acceptable merely because it is clearer or more attractive.
It must preserve the source-lock silhouette, body proportions, color palette,
costume anchors, symbolic props, and non-human cat identity from the colored
three-view turnaround sheets.

## Batch 66 Systematic Asset Master Plan Status

Batch 66 is a production-control batch. It creates no PNG candidates and no
Unity import state; its purpose is to make the next image-production decision
explicit before more assets are generated.

- Directory:
  `design/development/asset_candidates/p0_asset_dashboard/batch_66_systematic_asset_master_plan_2026-06-15`
- Gap matrix:
  `design/development/asset_candidates/p0_asset_dashboard/batch_66_systematic_asset_master_plan_2026-06-15/p0_asset_batch66_master_gap_matrix.csv`
- Blueprint:
  `design/development/asset_candidates/p0_asset_dashboard/batch_66_systematic_asset_master_plan_2026-06-15/p0_asset_batch66_master_blueprint.md`
- Process note:
  `design/development/asset_candidates/p0_asset_dashboard/batch_66_systematic_asset_master_plan_2026-06-15/p0_asset_batch66_process_note.md`
- Next execution prompt:
  `design/development/agent_prompts/p0_asset_batch_67_bedroom_interaction_affordance_candidates.md`

Batch 66 confirms:

- Codex remains the primary place to generate and package candidate assets.
- Unity remains the installation and runtime acceptance boundary.
- Starter-cat and future cat-body assets remain locked to colored three-view
  turnarounds.
- Batch 67 bedroom interaction affordance UI/VFX was the next safe candidate
  lane because it is non-cat, directly supports the four-core-value interaction
  loop, and has a clear Unity screenshot/Console acceptance gate.

## Batch 67 Bedroom Interaction Affordance Candidate Status

Batch 67 is a candidate-only Codex-side UI/VFX pack for P0 interaction
readability around the bed, litter box, feeder, blocked interaction state, and
valid interaction range.

- Candidate directory:
  `design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15`
- Manifest:
  `design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15/bedroom_interaction_affordance_batch67_manifest.csv`
- Review sheet:
  `design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15/thecat_ui_bedroom_interaction_affordance_batch67_review_sheet.png`
- Review note:
  `design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15/bedroom_interaction_affordance_batch67_candidate_review.md`

Batch 67 produced review-only candidates for:

- `interaction_bed_ready_ring`
- `interaction_bed_restore_pulse`
- `interaction_litter_urgent_marker`
- `interaction_feeder_ready_marker`
- `interaction_blocked_marker`
- `interaction_range_ripple`

This batch is non-cat UI/VFX only. It does not depict or derive Saiban,
Nephthys, Suzune, future partner cats, cat body parts, cat costumes, cat paws,
cat tails, cat faces, cat weapons, or colored-turnaround crops.

No Batch 67 output is installed into Unity. Formal install is blocked until
bed, litter box, feeder, blocked interaction, range, input timing, Console,
Sprite import, and scene/prefab checks pass in Unity.

## Batch 76 Owner Sleep-State Animation Candidate Status

Batch 76 is a candidate-only Codex-side frame packet for the P0 owner sleep
state animation requested by the digital asset inventory.

- Candidate directory:
  `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24`
- Manifest:
  `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/owner_sleep_states_batch76_manifest.csv`
- Active source:
  `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/thecat_owner_sleep_states_batch76_chromakey_source_1536x1024_v002.png`
- Alpha sheet:
  `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/thecat_owner_sleep_states_batch76_alpha_sheet_1536x1024_candidate_v002.png`
- Review sheet:
  `design/development/asset_candidates/owner_sleep_states/batch_76_owner_sleep_state_framesheet_candidate_2026-06-24/thecat_owner_sleep_states_batch76_review_sheet_1920x1320_v001.png`
- Validator:
  `design/development/tools/validate_owner_sleep_state_framesheet_candidate.ps1`

Batch 76 produced 24 padded 256x256 alpha-frame candidates:

- `deep_sleep`: 6 frames
- `half_awake`: 6 frames
- `near_awake`: 6 frames
- `wake_failure`: 6 frames

The active v002 source uses owner/pillow/blanket overlay framing instead of a
full bed-frame prop. The builder trims source-cell edges, centers each frame
inside a transparent 256x256 canvas, and clears a 28px outer edge guard so the
candidate frames are safer for later Sprite slicing review.

No Batch 76 output is installed into Unity. Formal import remains blocked until
Sprite import settings, slicing, pivot, runtime scale, overlay-vs-bed-layer
comparison, sleep-state timing, battle-world screenshots, and clean Console
status pass in Unity. V001 remains only as historical process evidence.

## Batch 77 Owner Sleep Status Icon Candidate Status

Batch 77 is a candidate-only Codex-side UI icon pack for the P0 owner
sleep-state HUD and settlement status icons requested by the digital asset
inventory.

- Candidate directory:
  `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24`
- Manifest:
  `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/owner_sleep_status_icons_batch77_manifest.csv`
- Source:
  `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/thecat_ui_owner_sleep_status_icons_batch77_chromakey_source_v001.png`
- Alpha sheet:
  `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/thecat_ui_owner_sleep_status_icons_batch77_alpha_sheet_v001.png`
- Review sheet:
  `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/thecat_ui_owner_sleep_status_icons_batch77_review_sheet_v001.png`
- Validator:
  `design/development/tools/validate_owner_sleep_status_icon_candidates.ps1`

Batch 77 produced 12 transparent icon candidates:

- `deep_sleep`: 256px, 64px, and 32px
- `half_awake`: 256px, 64px, and 32px
- `near_awake`: 256px, 64px, and 32px
- `wake_failure`: 256px, 64px, and 32px

The icons are symbolic UI assets, not owner portraits or cat art. They escalate
from calm sleep-blue to amber warning and purple wake failure so they can sync
with the Batch 76 owner sleep-state animation.

No Batch 77 output is installed into Unity. Formal import remains blocked until
Sprite import settings, 64px/32px HUD readability, dark/light HUD and
cooldown-overlay tests, scene/prefab binding, HUD/settlement screenshots, and
clean Console status pass in Unity. Review watch item: `wake_failure` may read
like a purple eye/mark sigil at 32px, and `half_awake` may be too intense for
the first warning step; both require HUD screenshot review before approval.

## Batch 78 Settings Control Candidate Status

Batch 78 is a candidate-only Codex-side UI control pack for the P0 settings
screen controls requested by the digital asset inventory.

- Candidate directory:
  `design/development/asset_candidates/ui/settings_controls/batch_78_settings_control_candidates_2026-06-24`
- Manifest:
  `design/development/asset_candidates/ui/settings_controls/batch_78_settings_control_candidates_2026-06-24/settings_controls_batch78_manifest.csv`
- Source:
  `design/development/asset_candidates/ui/settings_controls/batch_78_settings_control_candidates_2026-06-24/thecat_ui_settings_controls_batch78_chromakey_source_v001.png`
- Alpha sheet:
  `design/development/asset_candidates/ui/settings_controls/batch_78_settings_control_candidates_2026-06-24/thecat_ui_settings_controls_batch78_alpha_sheet_v001.png`
- Review sheet:
  `design/development/asset_candidates/ui/settings_controls/batch_78_settings_control_candidates_2026-06-24/thecat_ui_settings_controls_batch78_review_sheet_v001.png`
- Validator:
  `design/development/tools/validate_settings_control_candidates.ps1`

Batch 78 produced 6 transparent settings-control candidates:

- `slider_track`: 384x64
- `slider_knob`: 96x96
- `switch_off`: 192x96
- `switch_on`: 192x96
- `checkbox_unchecked`: 96x96
- `checkbox_checked`: 96x96

The controls are symbolic UI assets, not character art. They use the
dreamglass, moon-blue, muted gray-blue, and restrained fish-gold language
needed for music volume, sound-effect volume, display toggles, and basic
settings confirmation surfaces.

No Batch 78 output is installed into Unity. Formal import remains blocked until
Sprite import settings, settings-screen screenshots, slider drag / knob
alignment, switch on/off contrast and accessibility, dark/light panel
readability, click/pointer target scale, scene/prefab binding, and clean
Console status pass in Unity.

Independent review found no P0 visual/source-lock, tooling, or tracking
blockers for candidate-complete status. P1 watch items carried forward:
confirm slider drag alignment and value-fill behavior in the real settings
screen, run a switch on/off color-blind contrast pass, and treat supporting
review/process artifact hash coverage plus unpersisted source segment
coordinates as future tooling hardening opportunities.

## Batch 79 System Icon Candidate Status

Batch 79 is a candidate-only Codex-side UI system icon pack for the P0 global
system icons requested by the digital asset inventory.

- Candidate directory:
  `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24`
- Manifest:
  `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/system_icons_batch79_manifest.csv`
- Source:
  `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/thecat_ui_system_icons_batch79_chromakey_source_v001.png`
- Alpha sheet:
  `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/thecat_ui_system_icons_batch79_alpha_sheet_v001.png`
- Review sheet:
  `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/thecat_ui_system_icons_batch79_review_sheet_v001.png`
- Validator:
  `design/development/tools/validate_system_icon_candidates.ps1`

Batch 79 produced 30 transparent system-icon candidates:

- `settings`, `sound`, `mute`, `back`, `close`
- `pause`, `continue`, `retry`, `lock`, `warning`
- 128px, 64px, and 32px variants for every icon

The icons are symbolic UI assets, not character art. They share the Batch 78
moon-blue dreamglass, muted navy, lavender rim-light, and restrained fish-gold
accent language for global settings, navigation, pause, retry, locked-content,
and warning UI surfaces.

No Batch 79 output is installed into Unity. Formal import remains blocked until
Sprite import settings, 64px/32px readability, mute-vs-sound clarity, warning
meaning without text, dark/light panel readability, scene/prefab binding, UI
screenshots, and clean Console status pass in Unity.

Independent review found no P0 visual/source-lock, tooling, or tracking
blockers for candidate-complete status. P1 watch items carried forward:
confirm `mute` does not read as `sound` at 32px, confirm the text-free
`warning` triangle reads as a warning instead of a decorative dream sigil, and
treat supporting review/process artifact hash coverage, unpersisted 2x5 source
cell coordinates, and process-note token coverage as future tooling hardening
opportunities.

## Batch 68 Starter Cat Core Document Source-Lock Gate

Batch 68 is a consistency-control batch for starter-cat assets. It creates no
PNG candidates and no Unity import state. Its purpose is to prevent future
Saiban, Nephthys, or Suzune asset production from starting from stale encoded
paths, generated lineup art, or incomplete source-lock documentation.

- Execution prompt:
  `design/development/agent_prompts/p0_asset_batch_68_starter_cat_core_doc_source_lock_gate.md`
- Core documents guarded by `P0StarterCatSourceLockPacketEvidence`:
  - `design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.md`
  - `design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`
  - `design/development/asset_review/p0_starter_cat_strict_reference_pack_2026-06-14.md`

The gate requires all three core documents to:

- repeat the exact colored-turnaround source paths for Saiban, Nephthys, and
  Suzune
- keep formal starter-cat Unity import blocked until active-cat Play Mode
  screenshots are approved
- contain zero mojibake or stale encoded design path mentions

This is now a production preflight for every future starter-cat candidate
batch. No cat runtime sprite may be replaced merely because a generated image
looks cleaner; it must pass the core document source-lock gate, candidate
review, and Unity active-cat screenshot comparison against the colored
three-view turnarounds.

## Batch 54 Bedroom Interactable Candidate Status

Batch 54 is the first queue-backed Codex candidate production batch for
bedroom-world interactables.

- Candidate directory:
  `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15`
- Manifest:
  `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/bedroom_interactables_batch54_manifest.csv`
- Review sheet:
  `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_props_bedroom_interactables_batch54_review_sheet.png`
- Review note:
  `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/bedroom_interactables_batch54_candidate_review.md`

Batch 54 produced review-only candidates for:

- `bed`: protected wooden bed with navy star blanket, crescent, pillow, and rug
  base.
- `litter_box`: selected v002 blue litter box with tan clean litter and paw
  emblem; v001 is process-rejected because green chroma-key glow remained on
  the rim.
- `feeder`: pink-lavender automatic feeder with transparent kibble tank, kibble
  bowl, paw emblem, and moon/star accents.

No Batch 54 output is installed into Unity. Current runtime props under
`Assets/TheCat/Art/Scenes/BedroomDream` remain unchanged. Formal install is
blocked until Sprite import settings, scene/prefab binding, Console checks,
battle-world screenshot readability, and runtime scale review pass.

## Batch 55 Starter Skill VFX Candidate Status

Batch 55 is the second queue-backed Codex candidate production batch. It
proves that VFX can be produced in Codex first, then installed into Unity only
after a separate runtime acceptance decision.

- Candidate directory:
  `design/development/asset_candidates/vfx/starter_skills/batch_55_starter_skill_vfx_candidates_2026-06-15`
- Manifest:
  `design/development/asset_candidates/vfx/starter_skills/batch_55_starter_skill_vfx_candidates_2026-06-15/starter_skill_vfx_batch55_manifest.csv`
- Review sheet:
  `design/development/asset_candidates/vfx/starter_skills/batch_55_starter_skill_vfx_candidates_2026-06-15/thecat_vfx_starter_skills_batch55_review_sheet.png`
- Review note:
  `design/development/asset_candidates/vfx/starter_skills/batch_55_starter_skill_vfx_candidates_2026-06-15/starter_skill_vfx_batch55_candidate_review.md`

Batch 55 produced review-only candidates for:

- `saiban`: bedline oath shield VFX with silver-blue shield arc, sword light,
  sun-gold oath sigil, and knockback wave language.
- `nephthys`: moon-sand control VFX with obelisk, quicksand spiral, teal rings,
  and royal eye mark.
- `suzune`: lullaby healing VFX with kagura bells, vermilion torii,
  moon-blue healing circle, talismans, and music notes.

No Batch 55 output is installed into Unity. Current runtime VFX under
`Assets/TheCat/Art/VFX` remain unchanged. Formal install is blocked until
Sprite import settings, visual binding targets, Console checks, Play Mode
screenshots, and runtime scale/readability review pass.

## Batch 56 Formal Install Decision Status

Batch 56 is a blocked decision packet, not an install batch. It records that
current Codex candidate production has enough review material to discuss
installation, but it does not approve any asset without Unity runtime evidence.

- Decision directory:
  `design/development/asset_candidates/formal_install_decisions/batch_56_formal_install_decision_packet_2026-06-15`
- Decision CSV:
  `design/development/asset_candidates/formal_install_decisions/batch_56_formal_install_decision_packet_2026-06-15/formal_install_decision_batch56.csv`
- Review note:
  `design/development/asset_candidates/formal_install_decisions/batch_56_formal_install_decision_packet_2026-06-15/formal_install_decision_batch56_review.md`

All rows are blocked pending Unity evidence. No candidate is approved for
install, no runtime visual bindings are changed, and no asset has been copied
into `Assets`. Future installation must convert one row at a time from
`blocked_pending_unity_evidence` to an approved decision after Unity Console,
screenshot, Sprite import, runtime scale, and scene/prefab binding checks pass.

## Architecture Audit Boundary For Systematic Asset Production

`P0ArchitectureCompletionAudit` now records the project-level split between
Codex asset production and Unity runtime acceptance.

Codex may produce assets systematically when the batch is explicitly
candidate-only:

- generated source images
- transparent PNG cleanup
- manifests
- contact sheets
- review sheets
- process notes
- validators
- agent prompts

Unity remains the acceptance and installation boundary:

- `.meta` files
- Sprite import settings
- AssetDatabase refresh
- Console checks
- scene and prefab binding
- Play Mode screenshots
- runtime scale and readability
- formal install decision rows

For starter cats, this boundary is strict. Saiban, Nephthys, and Suzune must
stay source-locked to the colored three-view turnarounds. Cat-body candidates
may be generated in Codex for review, but they must not enter `Assets` or
replace runtime bindings until active-cat screenshots prove they match the
turnarounds in Unity.

## Batch 57 Skill HUD Feedback Candidate Status

Batch 57 is a Codex-side candidate pack for non-cat skill HUD and battle
operation feedback.

- Candidate directory:
  `design/development/asset_candidates/ui/skill_hud/batch_57_skill_hud_feedback_candidates_2026-06-15`
- Manifest:
  `design/development/asset_candidates/ui/skill_hud/batch_57_skill_hud_feedback_candidates_2026-06-15/skill_hud_feedback_batch57_manifest.csv`
- Review sheet:
  `design/development/asset_candidates/ui/skill_hud/batch_57_skill_hud_feedback_candidates_2026-06-15/thecat_ui_skill_hud_feedback_batch57_review_sheet.png`
- Review note:
  `design/development/asset_candidates/ui/skill_hud/batch_57_skill_hud_feedback_candidates_2026-06-15/skill_hud_feedback_batch57_candidate_review.md`

Batch 57 produced review-only candidates for:

- `skill_ready_frame`
- `skill_cooldown_overlay`
- `skill_no_target_marker`
- `skill_hunger_cost_chip`
- `auto_target_reticle`
- `interaction_range_ripple`

The batch is non-cat UI/VFX only. It does not depict Saiban, Nephthys, Suzune,
cat bodies, starter-cat costumes, fur patterns, symbolic turnaround props, or
colored-turnaround crops.

No Batch 57 output is installed into Unity. Formal install is blocked until
HUD-scale readability, skill timing, Sprite import settings, Console checks,
scene/prefab binding, and Play Mode screenshot evidence pass.

## Batch 58 Starter Cat HUD Avatar Install Status

Batch 58 is a controlled Codex-side production plus Unity-filesystem install
batch for starter-cat HUD avatars.

This batch deliberately avoids AI cat-body replacement. It derives each avatar
from the current locked Unity combat sprite, which is itself locked to the
colored three-view turnaround source:

- Saiban:
  `saiban_turnaround_colored_2026-06-03.png`
- Nephthys:
  `nephthys_turnaround_colored_2026-06-03.png`
- Suzune:
  `suzune_turnaround_colored_2026-06-03.png`

Installed assets:

- `Assets/TheCat/Art/UI/Icons/thecat_cat_saiban_hud_avatar_256_v001.png`
- `Assets/TheCat/Art/UI/Icons/thecat_cat_nephthys_hud_avatar_256_v001.png`
- `Assets/TheCat/Art/UI/Icons/thecat_cat_suzune_hud_avatar_256_v001.png`

Runtime wiring:

- Manifest baseline is now 106 generated/import-ready assets.
- Runtime visual baseline is now 102 bindings.
- New runtime bindings:
  - `cat.avatar.saiban`
  - `cat.avatar.nephthys`
  - `cat.avatar.suzune`
- The runtime visual contact sheet has been regenerated to cover all 102
  bindings.

Evidence:

- Batch directory:
  `design/development/asset_candidates/starter_cats/batch_58_starter_cat_hud_avatar_install_2026-06-15`
- Review sheet:
  `design/development/asset_candidates/starter_cats/batch_58_starter_cat_hud_avatar_install_2026-06-15/thecat_starter_cat_batch58_hud_avatar_install_review_sheet.png`
- Validator:
  `design/development/tools/validate_starter_cat_hud_avatar_install.ps1`

Asset-production rule confirmed by this batch:

- Codex may produce and package project assets, including installing selected
  files into `Assets`, when the batch has a source-lock manifest, review sheet,
  import metadata, runtime catalog wiring, and local validators.
- Unity remains the runtime acceptance boundary: Sprite import settings,
  Console state, AssetDatabase refresh, prefab/scene binding, HUD readability,
  and Play Mode screenshots still decide whether the installed assets are
  accepted in-game.
- For starter cats, AI-generated body art remains review-only until it matches
  the colored three-view turnarounds in Unity screenshots. Source-locked
  crops or derivatives from already accepted sprites are the preferred path for
  HUD portraits and other identity-sensitive derivatives.

## Batch 60 Skill HUD Feedback Install Status

Batch 60 promotes the accepted non-cat Batch 57 Skill HUD feedback candidates
into Unity as a formal installed UI/VFX feedback set.

- Install batch:
  `design/development/asset_candidates/ui/skill_hud/batch_60_skill_hud_feedback_install_2026-06-15`
- Manifest:
  `design/development/asset_candidates/ui/skill_hud/batch_60_skill_hud_feedback_install_2026-06-15/skill_hud_feedback_batch60_install_manifest.csv`
- Review sheet:
  `design/development/asset_candidates/ui/skill_hud/batch_60_skill_hud_feedback_install_2026-06-15/thecat_ui_skill_hud_feedback_batch60_install_review_sheet.png`
- Review note:
  `design/development/asset_candidates/ui/skill_hud/batch_60_skill_hud_feedback_install_2026-06-15/skill_hud_feedback_batch60_install_review.md`

Installed assets:

- `Assets/TheCat/Art/UI/Icons/thecat_ui_skill_ready_frame_512_v001.png`
- `Assets/TheCat/Art/UI/Icons/thecat_ui_skill_cooldown_overlay_512_v001.png`
- `Assets/TheCat/Art/UI/Icons/thecat_ui_skill_no_target_marker_512_v001.png`
- `Assets/TheCat/Art/UI/Icons/thecat_ui_skill_hunger_cost_chip_512_v001.png`
- `Assets/TheCat/Art/UI/Icons/thecat_ui_auto_target_reticle_512_v001.png`
- `Assets/TheCat/Art/UI/Icons/thecat_ui_interaction_range_ripple_512_v001.png`

Runtime wiring:

- Manifest baseline is now 112 generated/import-ready assets.
- Runtime visual baseline is now 108 bindings.
- New runtime bindings:
  - `skill_hud.ready_frame`
  - `skill_hud.cooldown_overlay`
  - `skill_hud.no_target_marker`
  - `skill_hud.hunger_cost_chip`
  - `skill_hud.auto_target_reticle`
  - `battle_hud.interaction_range_ripple`
- The runtime visual contact sheet has been regenerated to cover all 108
  bindings.

Production rule:

- Codex may generate, review, and install non-cat UI/VFX assets into Unity when
  a bounded install batch has manifest rows, meta files, catalog bindings,
  runtime hookup, and offline validation.
- Starter-cat body assets are stricter: future generated cat bodies must remain
  outside `Assets` until active Play Mode screenshots match the colored
  three-view turnarounds and a formal source-lock review approves import.
- Batch 60 does not modify starter-cat combat sprites, HUD avatars, source
  turnarounds, prefabs, or scenes.

Unity validation still required:

- Refresh AssetDatabase.
- Inspect Sprite import settings for all six installed PNGs.
- Capture battle HUD states for ready, cooldown, no-target, low-hunger,
  auto-target, and interaction range.
- Confirm the Console remains clean.

## Batch 61 Starter Skill VFX Install Status

Batch 61 promotes three accepted symbolic starter skill VFX candidates from
Batch 55 into Unity.

- Install batch:
  `design/development/asset_candidates/vfx/starter_skills/batch_61_starter_skill_vfx_install_2026-06-15`
- Manifest:
  `design/development/asset_candidates/vfx/starter_skills/batch_61_starter_skill_vfx_install_2026-06-15/starter_skill_vfx_batch61_install_manifest.csv`
- Review sheet:
  `design/development/asset_candidates/vfx/starter_skills/batch_61_starter_skill_vfx_install_2026-06-15/thecat_vfx_starter_skills_batch61_install_review_sheet.png`
- Review note:
  `design/development/asset_candidates/vfx/starter_skills/batch_61_starter_skill_vfx_install_2026-06-15/starter_skill_vfx_batch61_install_review.md`

Installed assets:

- `Assets/TheCat/Art/VFX/thecat_vfx_saiban_bedline_skill_512_v001.png`
- `Assets/TheCat/Art/VFX/thecat_vfx_nephthys_moonsand_skill_512_v001.png`
- `Assets/TheCat/Art/VFX/thecat_vfx_suzune_lullaby_skill_512_v001.png`

Runtime wiring:

- Manifest baseline is now 115 generated/import-ready assets.
- Runtime visual baseline is now 111 bindings.
- `P0_ASSET_MANIFEST.csv` is aligned to the same 115-asset baseline.
- New runtime bindings:
  - `skill_vfx.saiban_bedline`
  - `skill_vfx.nephthys_moonsand`
  - `skill_vfx.suzune_lullaby`
- The runtime visual contact sheet has been regenerated to cover all 111
  bindings.

Production rule:

- Codex is allowed to produce and install project-bound UI/VFX assets when the
  batch has a prompt, manifest, review sheet, import metadata, catalog wiring,
  runtime hookup, and offline validation.
- Unity remains the acceptance boundary for runtime scale, Sprite import,
  Console state, scene/prefab binding, screenshots, and feel.
- Starter-cat body assets are stricter than symbolic VFX. Saiban, Nephthys,
  Suzune, and future cat-body assets must match the locked colored three-view
  turnarounds before import or runtime replacement.
- Batch 61 uses colored turnarounds only as authority-symbol source locks. It
  does not depict or replace cat bodies.

Unity validation still required:

- Refresh AssetDatabase.
- Inspect Sprite import settings for all three installed PNGs.
- Capture battle-feedback screenshots for one Saiban, one Nephthys, and one
  Suzune starter skill cast.
- Confirm skill timing and on-screen readability at gameplay scale.
- Confirm the Console remains clean.

## Batch 62 Runtime Control Icon Candidate Status

Batch 62 is a candidate-only Codex-side icon pack for runtime control
affordance: pause, resume, speed 0.5x, speed 1x, speed 1.5x, and restart.

- Candidate directory:
  `design/development/asset_candidates/ui/runtime_controls/batch_62_runtime_control_icon_candidates_2026-06-15`
- Manifest:
  `design/development/asset_candidates/ui/runtime_controls/batch_62_runtime_control_icon_candidates_2026-06-15/runtime_control_icons_batch62_manifest.csv`
- Review sheet:
  `design/development/asset_candidates/ui/runtime_controls/batch_62_runtime_control_icon_candidates_2026-06-15/thecat_ui_runtime_controls_batch62_review_sheet.png`
- Review note:
  `design/development/asset_candidates/ui/runtime_controls/batch_62_runtime_control_icon_candidates_2026-06-15/runtime_control_icons_batch62_candidate_review.md`

Production rule:

- Codex can systematically produce transparent PNG candidates, manifests,
  review sheets, and validation scripts outside Unity.
- Candidate-only batches do not create Unity `.meta` files and do not change
  manifest or runtime visual binding counts.
- Unity remains the install boundary for HUD scale, shortcut readability,
  Console checks, Sprite import settings, scene/prefab binding, and screenshots.
- Cat body art remains stricter than non-cat UI. Future Saiban, Nephthys,
  Suzune, Shadowmaru, Mianhua, and Yuheng body assets must match their locked
  colored three-view turnarounds before import or runtime replacement.

Unity validation still required:

- Check pause/resume/speed/restart readability in the runtime settings and
  battle HUD surface.
- Confirm no Console errors after any future install.
- Approve a formal install packet before copying icons into
  `Assets/TheCat/Art/UI/Icons`.

## Batch 63 Runtime Control Panel Candidate Status

Batch 63 is a candidate-only Codex-side panel pack for runtime control surfaces:
pause overlay, speed segmented control, restart confirmation, and keyboard
hint strip.

- Candidate directory:
  `design/development/asset_candidates/ui/runtime_controls/batch_63_runtime_control_panel_candidates_2026-06-15`
- Manifest:
  `design/development/asset_candidates/ui/runtime_controls/batch_63_runtime_control_panel_candidates_2026-06-15/runtime_control_panels_batch63_manifest.csv`
- Review sheet:
  `design/development/asset_candidates/ui/runtime_controls/batch_63_runtime_control_panel_candidates_2026-06-15/thecat_ui_runtime_control_panels_batch63_review_sheet.png`
- Review note:
  `design/development/asset_candidates/ui/runtime_controls/batch_63_runtime_control_panel_candidates_2026-06-15/runtime_control_panels_batch63_candidate_review.md`

Production rule:

- Codex can systematically produce transparent PNG candidates, manifests,
  review sheets, and validation scripts outside Unity.
- Candidate-only batches do not create Unity `.meta` files and do not change
  manifest or runtime visual binding counts.
- Unity remains the install boundary for HUD scale, shortcut readability,
  Console checks, Sprite import settings, scene/prefab binding, and screenshots.
- Cat body art remains stricter than non-cat UI. Future cat-body assets must
  match their locked colored three-view turnarounds before import or runtime
  replacement.

Unity validation still required:

- Check pause overlay, speed selector, restart confirmation, and keyboard hint
  readability in the battle HUD surface.
- Confirm no Console errors after any future install.
- Approve a formal install packet before copying panels into
  `Assets/TheCat/Art/UI`.
