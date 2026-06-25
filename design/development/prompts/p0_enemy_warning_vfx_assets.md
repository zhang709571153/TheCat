# P0 Enemy Warning VFX Asset Prompt

## Scope

Create four transparent 256x256 PNG VFX sprites for the P0 enemy/Boss warning layer:

- `thecat_vfx_blackmud_bed_claw_256_v001`
- `thecat_vfx_coldlight_beam_warning_256_v001`
- `thecat_vfx_calltyrant_app_throw_256_v001`
- `thecat_vfx_calltyrant_summon_portal_256_v001`

## Source Authority

- Black Mud warning VFX must use `black_mud_animation` as the hard source lock.
- Cold Light warning VFX must use `cold_light_animation` as the hard source lock.
- Call Tyrant throw and summon VFX must use `call_tyrant_animation` as the hard source lock.
- Use `thecat_style_status_icons_5x64_v001` only for glow/line-weight consistency.

## Visual Rules

- Hand-drawn dream UI feel, transparent background, readable at small warning-ring size in Play Mode.
- Distinct silhouettes:
  - Black Mud bed claw: red near-bed danger ring, black mud mass, three claw pressure marks.
  - Cold Light beam: cyan-white ranged line, cold eye/screen origin, clear beam direction.
  - Call Tyrant app throw: red app projectile with cyan vibration arc and purple tie motif.
  - Call Tyrant summon portal: red-cyan portal ring with dark inner void and phone-call vibration language.
- No text inside the PNG.
- Avoid starter cat body, costume, face, fur pattern, colored-turnaround crops, or any cat anatomy.

## Import Contract

- Path: `Assets/TheCat/Art/Enemies/VFX`
- Type: Unity Sprite, single sprite, alpha transparency, no mipmaps
- Meta marker: `TheCatP0ImportSettings:v1`
- Manifest status: `generated`
