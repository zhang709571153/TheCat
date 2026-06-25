# Enemy Framesheet Import Policy

Packet date: 2026-06-25

## Decision

Use the three existing 4x256 enemy framesheets as source-locked reference sheets and import them as single Sprite textures with alpha transparency. Their source-sheet `.meta` userData includes `SourceFramesheetSingleSprite`. Do not hand-author Unity Multiple Sprite YAML for these files in this pass, because the project has no reliable sliced Sprite meta template to copy.

Runtime/import-test units are the 12 deterministic 256x256 split sprites under `Assets/TheCat/Art/Enemies/Frames/Sliced/`. Each split sprite has its own Unity `.meta` with `textureType: 8`, `spriteMode: 1`, `alphaIsTransparency: 1`, `maxTextureSize: 256`, `EnemyFramesheetSlice` userData, and a deterministic GUID derived from its project-relative path.

## Source Sheets

| Enemy | Animation | Source sheet | Source lock |
| --- | --- | --- | --- |
| black_mud_nightmare | black_mud_move | `Assets/TheCat/Art/Enemies/Frames/thecat_enemy_blackmud_move_framesheet_4x256_v001.png` | black_mud_animation |
| cold_light_shadow | cold_light_cast | `Assets/TheCat/Art/Enemies/Frames/thecat_enemy_coldlight_cast_framesheet_4x256_v001.png` | cold_light_animation |
| call_tyrant | call_tyrant_boss_pattern | `Assets/TheCat/Art/Enemies/Frames/thecat_enemy_calltyrant_bosspattern_framesheet_4x256_v001.png` | call_tyrant_animation |

## Split Sprites

| Enemy | Animation | Frame | Split sprite | Hash |
| --- | --- | ---: | --- | --- |
| black_mud_nightmare | black_mud_move | 1 | `Assets/TheCat/Art/Enemies/Frames/Sliced/thecat_enemy_blackmud_move_f01_256_v001.png` | `67bd08cf49f86a7d21152208bc614b0482f20d86cb999527acdbf497d7e553c1` |
| black_mud_nightmare | black_mud_move | 2 | `Assets/TheCat/Art/Enemies/Frames/Sliced/thecat_enemy_blackmud_move_f02_256_v001.png` | `c07d095b397b4bd67dcb162e125141c8d49ae9544628971fed60be3200cfe36b` |
| black_mud_nightmare | black_mud_move | 3 | `Assets/TheCat/Art/Enemies/Frames/Sliced/thecat_enemy_blackmud_move_f03_256_v001.png` | `c67f6a0169fd768fbfca318917fbde44c60f95aa5be5b4fff45c9c1fb0f9e043` |
| black_mud_nightmare | black_mud_move | 4 | `Assets/TheCat/Art/Enemies/Frames/Sliced/thecat_enemy_blackmud_move_f04_256_v001.png` | `ff4977cf2037135362fab547a3b13b517d8a179069a70e2bfe9530a7d843d9db` |
| cold_light_shadow | cold_light_cast | 1 | `Assets/TheCat/Art/Enemies/Frames/Sliced/thecat_enemy_coldlight_cast_f01_256_v001.png` | `d351b007ef68e2216a685e10a7bd10f096ab6ae75a8fefa428902d78363dce26` |
| cold_light_shadow | cold_light_cast | 2 | `Assets/TheCat/Art/Enemies/Frames/Sliced/thecat_enemy_coldlight_cast_f02_256_v001.png` | `572621a9f26a06bf1046401a4fd2b748bf90edfbf0e92dbc6f7be1d778c5cbd4` |
| cold_light_shadow | cold_light_cast | 3 | `Assets/TheCat/Art/Enemies/Frames/Sliced/thecat_enemy_coldlight_cast_f03_256_v001.png` | `a1d52b2805d7a221bbfa0548f7d7d5bb135430c4c6a9b9a3a463ceeae89f1acd` |
| cold_light_shadow | cold_light_cast | 4 | `Assets/TheCat/Art/Enemies/Frames/Sliced/thecat_enemy_coldlight_cast_f04_256_v001.png` | `644be4e6799a1262e24ed460c00f35cd61a851a9ce880d656a465411104b909d` |
| call_tyrant | call_tyrant_boss_pattern | 1 | `Assets/TheCat/Art/Enemies/Frames/Sliced/thecat_enemy_calltyrant_bosspattern_f01_256_v001.png` | `75c843ef8c3d23ea273697293cb11f5a2bc0683236b92d222cd6b60cbfbbd934` |
| call_tyrant | call_tyrant_boss_pattern | 2 | `Assets/TheCat/Art/Enemies/Frames/Sliced/thecat_enemy_calltyrant_bosspattern_f02_256_v001.png` | `b4a2b4457395fb6b88c56491c7b0d64a93d3bda56972ed2c0ee04dc65415d725` |
| call_tyrant | call_tyrant_boss_pattern | 3 | `Assets/TheCat/Art/Enemies/Frames/Sliced/thecat_enemy_calltyrant_bosspattern_f03_256_v001.png` | `58fcbaa2c432f98fac2d142e98c86568f5d13cbffcceda6998146fa439b3cc81` |
| call_tyrant | call_tyrant_boss_pattern | 4 | `Assets/TheCat/Art/Enemies/Frames/Sliced/thecat_enemy_calltyrant_bosspattern_f04_256_v001.png` | `fad0198b44be72fb1832ba1fc5d8197329199c572f124a6b3aa932f95e248b14` |

## Review Rules

- This policy resolves local import/slicing package readiness only; it is not Unity runtime acceptance.
- Starter cat bodies, starter cat framesheets, and character replacement sprites remain blocked by the character source-lock screenshot gate.
- No split sprite may be sourced from character asset directories or a starter-cat body packet.
- Next required evidence: Unity import refresh, active enemy animation screenshot, prefab/catalog binding proof, and Console check.
- Validator: `design/development/tools/validate_enemy_framesheet_import_policy.ps1`.

## Review Contact Sheet

`design/development/asset_review/ENEMY_FRAMESHEET_SLICED_SPRITES_2026-06-25.png`
