# P0 Battle Feedback VFX Asset Prompt

## Scope

Create six transparent 256x256 PNG VFX sprites for the current P0 graybox battle feedback layer:

- `thecat_vfx_hit_spark_256_v001`
- `thecat_vfx_bed_shield_pulse_256_v001`
- `thecat_vfx_sleep_stable_wave_256_v001`
- `thecat_vfx_litter_cleanse_256_v001`
- `thecat_vfx_feeder_kibble_256_v001`
- `thecat_vfx_enemy_mark_ring_256_v001`

## Style References

- Use `thecat_style_status_icons_5x64_v001` for glow color, line weight, shield, mark, and sleep-stable language.
- Use `thecat_style_bedroomdream_anchor_1920x1080_v001` for litter box and feeder palette.
- Do not use starter cat body, costume, face, fur pattern, or character turnaround fragments.

## Visual Rules

- Hand-drawn dream UI feel, transparent background, readable at 46px in the IMGUI feedback panel.
- Use distinct silhouettes:
  - hit spark: gold-white impact star with cyan shock ring
  - bed shield pulse: silver moon barrier with bed-protection hint
  - sleep stable wave: light-blue breathing wave and crescent moon
  - litter cleanse: teal cleanse swirl with litter-box relief cue
  - feeder kibble: fish-gold kibble burst with bowl cue
  - enemy mark ring: violet/red crosshair ring with paw-mark center
- No text inside the PNG.
- Avoid any cat anatomy or character-specific markings.

## Import Contract

- Path: `Assets/TheCat/Art/VFX`
- Type: Unity Sprite, single sprite, alpha transparency, no mipmaps
- Meta marker: `TheCatP0ImportSettings:v1`
- Manifest status: `generated`
