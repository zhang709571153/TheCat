# Batch 87 Battle HUD Process Note

- Lane: `battle_hud` / screen-level preflight.
- Source truth: Qr1 UI/style; existing core gauges, status icons, skill slot frames, recommended symbolic skill icons, HUD avatars, bedroom battle background, and runtime control icons.
- Generation mode: deterministic local derivative from existing raster assets; not image2 provenance.
- Reason: `OPENAI_API_KEY` is not set, so strict `gpt-image-2` CLI generation is unavailable in this shell.
- Candidate boundary: all PNGs stay under `design/development/asset_candidates`; do not import into `Assets` before Unity review.
- Text rule: no baked Chinese text; Unity-rendered text, numbers, cooldowns, and status counts remain required.
- Starter-cat rule: HUD avatar reuse is allowed; no new body, pose, costume, color, or framesheet generation is included.
- Runtime gate: Unity-rendered battle HUD screenshots, skill state readability, gauge value replacement, click targets, import settings, binding, and Console.
