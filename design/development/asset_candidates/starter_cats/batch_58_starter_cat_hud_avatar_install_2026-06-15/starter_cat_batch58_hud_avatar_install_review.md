# Batch 58 Starter Cat HUD Avatar Install Review

Decision: installed source-locked HUD avatars; Unity visual smoke still pending.

This batch responds to the current consistency risk by installing only deterministic avatars derived from the already locked starter-cat combat sprites. No AI-generated cat body candidate was imported.

## Scope

- Covers Saiban, Nephthys, and Suzune only.
- Installs 256x256 transparent HUD avatar icons under `Assets/TheCat/Art/UI/Icons`.
- Uses the locked colored three-view turnaround and locked combat sprite as lineage evidence.
- Adds Unity `.png.meta` files with `TheCatP0ImportSettings:v1`.
- Does not modify or replace starter-cat combat sprites.
- Does not approve Batch 48, 49, 50, or 51 AI candidates for body-art import.

## Outputs

- Manifest: `design/development/asset_candidates/starter_cats/batch_58_starter_cat_hud_avatar_install_2026-06-15/starter_cat_batch58_hud_avatar_install_manifest.csv`
- Review sheet: `design/development/asset_candidates/starter_cats/batch_58_starter_cat_hud_avatar_install_2026-06-15/thecat_starter_cat_batch58_hud_avatar_install_review_sheet.png`
- Process note: `design/development/asset_candidates/starter_cats/batch_58_starter_cat_hud_avatar_install_2026-06-15/starter_cat_batch58_hud_avatar_install_process_note.md`
- Agent prompt: `design/development/agent_prompts/p0_asset_batch_58_starter_cat_hud_avatar_install.md`

## Installed Rows

### Saiban / Sword Saint

- Asset id: `thecat_cat_saiban_hud_avatar_256_v001`
- Unity path: `Assets/TheCat/Art/UI/Icons/thecat_cat_saiban_hud_avatar_256_v001.png`
- Source lock: `saiban_turnaround_colored`
- Source turnaround: `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- Locked sprite: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png`
- Active screenshot gate remains required before final visual acceptance.

### Nephthys / Moon-Sand Agent

- Asset id: `thecat_cat_nephthys_hud_avatar_256_v001`
- Unity path: `Assets/TheCat/Art/UI/Icons/thecat_cat_nephthys_hud_avatar_256_v001.png`
- Source lock: `nephthys_turnaround_colored`
- Source turnaround: `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- Locked sprite: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png`
- Active screenshot gate remains required before final visual acceptance.

### Suzune / Sleep Shrine Healer

- Asset id: `thecat_cat_suzune_hud_avatar_256_v001`
- Unity path: `Assets/TheCat/Art/UI/Icons/thecat_cat_suzune_hud_avatar_256_v001.png`
- Source lock: `suzune_turnaround_colored`
- Source turnaround: `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`
- Locked sprite: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png`
- Active screenshot gate remains required before final visual acceptance.

## Remaining Unity Gate

- Refresh Unity AssetDatabase.
- Confirm all three avatar icons import as Sprite, Single mode, no mipmaps, alpha transparency on.
- Capture HUD / character-selection screenshots showing the avatar icons at runtime scale.
- Compare screenshots against the Batch 58 review sheet and the colored three-view source.
