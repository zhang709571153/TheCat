# P0 Asset Batch 46 Production Dashboard Review

Decision: candidate-only production dashboard; do not import into Unity.

This batch answers the production question: Codex can generate, clean, compare, and package candidate art outside Unity, while Unity remains the install-time and runtime validation gate.

## Scope

- Covers the three starter cats and three P0 core enemy/Boss subjects.
- Consolidates source lock, current runtime asset, latest candidate preview, install target, and active screenshot gate.
- Generates no new model art and performs no Unity asset install.
- Keeps all Batch 46 outputs outside `Assets`.
- Creates no Unity `.meta` files.
- Explicitly preserves the starter-cat colored three-view turnaround gate.
- Marks all rows as Unity validation pending.

## Outputs

- Manifest: `design/development/asset_candidates/p0_asset_dashboard/batch_46_p0_asset_production_dashboard_2026-06-15/p0_asset_batch46_production_dashboard_manifest.csv`
- Review sheet: `design/development/asset_candidates/p0_asset_dashboard/batch_46_p0_asset_production_dashboard_2026-06-15/thecat_p0_asset_batch46_production_dashboard_review_sheet.png`
- Process note: `design/development/asset_candidates/p0_asset_dashboard/batch_46_p0_asset_production_dashboard_2026-06-15/p0_asset_batch46_production_dashboard_process_note.md`
- Agent prompt: `design/development/agent_prompts/p0_asset_batch_46_p0_asset_production_dashboard.md`

## Dashboard Rows

### Saiban / Sword Saint

- Subject: `cat:saiban`
- Source lock: `saiban_turnaround_colored`
- Current Unity asset: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png`
- Latest candidate preview: `design/development/asset_candidates/starter_cats/saiban/batch_31_cutout_candidate_2026-06-14/thecat_cat_saiban_cutout_alpha_512_preview_v001.png`
- Install target: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png`
- Active screenshot: `04-active-cat-saiban.png`
- Unity validation gate: active-cat screenshot, Console clean, Sprite import settings, prefab/scene binding
- Next action: Do not generate or install a replacement until the colored three-view turnaround and active-cat screenshot agree.

### Nephthys / Moon-Sand Agent

- Subject: `cat:nephthys`
- Source lock: `nephthys_turnaround_colored`
- Current Unity asset: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png`
- Latest candidate preview: `design/development/asset_candidates/starter_cats/nephthys/batch_37_nephthys_cutout_candidate_2026-06-15/thecat_cat_nephthys_cutout_alpha_512_preview_v001.png`
- Install target: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png`
- Active screenshot: `05-active-cat-nephthys.png`
- Unity validation gate: active-cat screenshot, Console clean, Sprite import settings, prefab/scene binding
- Next action: Do not generate or install a replacement until the colored three-view turnaround and active-cat screenshot agree.

### Suzune / Sleep Shrine Healer

- Subject: `cat:suzune`
- Source lock: `suzune_turnaround_colored`
- Current Unity asset: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png`
- Latest candidate preview: `design/development/asset_candidates/starter_cats/suzune/batch_35_suzune_cutout_candidate_2026-06-15/thecat_cat_suzune_cutout_alpha_512_preview_v001.png`
- Install target: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png`
- Active screenshot: `06-active-cat-suzune.png`
- Unity validation gate: active-cat screenshot, Console clean, Sprite import settings, prefab/scene binding
- Next action: Do not generate or install a replacement until the colored three-view turnaround and active-cat screenshot agree.

### Black Mud Nightmare

- Subject: `enemy:black_mud_nightmare`
- Source lock: `black_mud_concept;black_mud_animation`
- Current Unity asset: `Assets/TheCat/Art/Enemies/Sprites/thecat_enemy_blackmud_combat_sprite_512_v001.png`
- Latest candidate preview: `design/development/asset_candidates/enemies/black_mud_nightmare/batch_40_black_mud_cutout_candidate_2026-06-15/thecat_enemy_black_mud_nightmare_cutout_alpha_512_preview_v001.png`
- Install target: `Assets/TheCat/Art/Enemies/Sprites/thecat_enemy_blackmud_combat_sprite_512_v001.png`
- Active screenshot: `07-active-enemy-black-mud.png`
- Unity validation gate: active-enemy screenshot, Console clean, Sprite import settings, prefab/scene binding
- Next action: Compare Batch 40 cutout against the active-enemy screenshot, then replace the runtime sprite only after Console and prefab binding checks pass.

### Cold Light Shadow

- Subject: `enemy:cold_light_shadow`
- Source lock: `cold_light_concept;cold_light_animation`
- Current Unity asset: `Assets/TheCat/Art/Enemies/Sprites/thecat_enemy_coldlight_combat_sprite_512_v001.png`
- Latest candidate preview: `design/development/asset_candidates/enemies/cold_light_shadow/batch_42_cold_light_cutout_candidate_2026-06-15/thecat_enemy_cold_light_shadow_cutout_beam_alpha_512_preview_v001.png`
- Install target: `Assets/TheCat/Art/Enemies/Sprites/thecat_enemy_coldlight_combat_sprite_512_v001.png`
- Active screenshot: `08-active-enemy-cold-light.png`
- Unity validation gate: active-enemy screenshot, Console clean, Sprite import settings, prefab/scene binding
- Next action: Compare Batch 42 beam-preserving cutout against the active-enemy screenshot, then replace the runtime sprite only after warning-beam readability passes.

### Call Tyrant

- Subject: `enemy:call_tyrant`
- Source lock: `call_tyrant_concept;call_tyrant_animation`
- Current Unity asset: `Assets/TheCat/Art/Enemies/Concepts/thecat_enemy_calltyrant_concept_2048_v001.png`
- Latest candidate preview: `design/development/asset_candidates/enemies/call_tyrant/batch_44_call_tyrant_cutout_candidate_2026-06-15/thecat_enemy_call_tyrant_cutout_boss_alpha_512_preview_v001.png`
- Install target: `Assets/TheCat/Art/Enemies/Sprites/thecat_enemy_calltyrant_combat_sprite_512_v001.png`
- Active screenshot: `09-active-enemy-call-tyrant.png`
- Unity validation gate: active-enemy screenshot, Console clean, Sprite import settings, prefab/scene binding
- Next action: Create a formal boss combat sprite binding only after Batch 44 cutout, app-throw VFX, summon VFX, and active Boss screenshot are reviewed together.

## Blocking Items

- Unity MCP/editor validation is still pending for active-cat and active-enemy screenshots.
- Console, AssetDatabase refresh, Sprite import settings, prefab/scene binding, and screenshot readability must pass before any candidate is installed.
- Call Tyrant still needs a formal boss combat sprite binding decision because the current runtime asset is a concept proxy.
