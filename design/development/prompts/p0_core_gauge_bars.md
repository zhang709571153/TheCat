# P0 Core Gauge Bars

Generated: 2026-06-14

## Scope

Produce non-cat HUD gauge assets for the four core values:

- Owner Sleep
- Cat HP
- Team Poop
- Team Hunger

Each value requires a `384x48` frame and a `384x48` fill strip. Runtime text,
numbers, and clipping are controlled by code. The PNGs must contain no readable
UI text.

## Source Style

- Use the existing dreamglass panel language for frames.
- Use the accepted four-core HUD icons for color identity.
- Keep transparent backgrounds and clear alpha.
- Keep edges readable at small IMGUI HUD sizes.

## Cat Consistency Boundary

- This is a non-cat symbolic UI batch.
- Do not depict cat bodies, faces, ears, tails, paws, collars, costumes, or fur
  markings.
- Do not derive motifs from Saiban, Nephthys, Suzune, or any colored
  three-view turnaround.
- Cat HP uses a generic life/recovery color language only.

## Import

- Unity import path: `Assets/TheCat/Art/UI/Frames`
- `TextureImporter.textureType = 8`
- `spriteMode = 1`
- mipmaps disabled
- alpha transparency enabled
- `.png.meta` `userData` must include
  `TheCatP0ImportSettings:v1`, `batch:p0_asset_batch_27_core_gauge_bars`,
  `spriteBorder:10`, and `nonCatSymbolicOnly:true`.

