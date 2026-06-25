# Batch 74 Starter Cat Runtime Combat Sprite Source Audit

Decision: runtime sprite source audit ready; Unity active-cat Play Mode screenshot comparison remains pending.

This packet makes the current runtime-bound starter-cat combat sprites auditable against the strict colored three-view source chain. It does not generate new cat body art, does not replace any Unity sprite, and does not approve AI-generated or repainted cat-body imports.

## Scope

- Covers the existing runtime-bound Saiban, Nephthys, and Suzune combat sprites.
- Compares each runtime sprite against the Batch 70 front reference plate.
- Records source turnaround hashes, front plate hashes, runtime sprite hashes, Unity `.png.meta` paths, runtime binding ids, and visual catalog constants.
- Keeps `P0StarterCatFormalImportReadiness` blocked until active-cat Play Mode screenshots are captured and reviewed.

## Outputs

- Manifest: `design/development/asset_candidates/starter_cats/batch_74_runtime_combat_sprite_source_audit_2026-06-15/starter_cat_runtime_combat_sprite_source_audit_batch74_manifest.csv`
- Review sheet: `design/development/asset_candidates/starter_cats/batch_74_runtime_combat_sprite_source_audit_2026-06-15/thecat_cat_starter_runtime_combat_sprite_source_audit_batch74_review_sheet.png`
- Process note: `design/development/asset_candidates/starter_cats/batch_74_runtime_combat_sprite_source_audit_2026-06-15/starter_cat_runtime_combat_sprite_source_audit_batch74_process_note.md`
- Agent prompt: `design/development/agent_prompts/p0_asset_batch_74_starter_cat_runtime_combat_sprite_source_audit.md`

## Runtime Sprite Rows

### Saiban / Sword Saint

- Runtime binding: `cat.combat.saiban`
- Visual catalog constant: `SaibanCombatSpriteId`
- Asset id: `thecat_cat_saiban_combat_sprite_512_v001`
- Runtime sprite: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png`
- Runtime sprite SHA-256: `fe13afc3758f19f66fd87debc56943a19b946ac78a44eab22d9ee1b146cc106b`
- Runtime sprite meta: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png.meta`
- Source lock: `saiban_turnaround_colored`
- Source turnaround: `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- Front reference plate: `design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/thecat_cat_saiban_turnaround_front_reference_plate_768_batch70_v001.png`
- Recommendation: `runtime_sprite_source_audit_ready_pending_unity_playmode_screenshot`

### Nephthys / Moon-Sand Agent

- Runtime binding: `cat.combat.nephthys`
- Visual catalog constant: `NephthysCombatSpriteId`
- Asset id: `thecat_cat_nephthys_combat_sprite_512_v001`
- Runtime sprite: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png`
- Runtime sprite SHA-256: `6eabcb75078dd2bfe9c5f0ba5191af40376ad7f9ea545b12d75f39b8ffb45a20`
- Runtime sprite meta: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png.meta`
- Source lock: `nephthys_turnaround_colored`
- Source turnaround: `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- Front reference plate: `design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/thecat_cat_nephthys_turnaround_front_reference_plate_768_batch70_v001.png`
- Recommendation: `runtime_sprite_source_audit_ready_pending_unity_playmode_screenshot`

### Suzune / Sleep Shrine Healer

- Runtime binding: `cat.combat.suzune`
- Visual catalog constant: `SuzuneCombatSpriteId`
- Asset id: `thecat_cat_suzune_combat_sprite_512_v001`
- Runtime sprite: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png`
- Runtime sprite SHA-256: `246ddd74501b4e81482d03e329b27c898ddd61b580bc30abeeeadef3aa61eaae`
- Runtime sprite meta: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png.meta`
- Source lock: `suzune_turnaround_colored`
- Source turnaround: `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`
- Front reference plate: `design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/thecat_cat_suzune_turnaround_front_reference_plate_768_batch70_v001.png`
- Recommendation: `runtime_sprite_source_audit_ready_pending_unity_playmode_screenshot`

## Unity Gate

- Refresh Unity AssetDatabase.
- Inspect the three sprite import settings as Sprite / Single, alpha transparency enabled, mipmaps disabled.
- Capture active-cat Saiban, Nephthys, and Suzune Play Mode screenshots.
- Compare screenshot scale, pose, markings, props, costume, and palette against the Batch 74 review sheet and Batch 70 front plates.
- Keep AI/repainted cat-body import blocked until that review explicitly approves it.
