# Batch 86 Dream Route Process Note

- Lane: `dream_route` / route-map screen-level preflight.
- Source truth: Qr1 UI/style; local route icons, route card frames, and Batch 65 route-map readability accents.
- Generation mode: deterministic local derivative from existing raster assets; not image2 provenance.
- Reason: `OPENAI_API_KEY` is not set, so strict `gpt-image-2` CLI generation is unavailable in this shell.
- Candidate boundary: all PNGs stay under `design/development/asset_candidates`; do not import into `Assets` before Unity review.
- Text rule: no baked Chinese text; Unity-rendered text replacement remains required.
- Starter-cat rule: no body, pose, costume, color, or framesheet generation is included.
- Runtime gate: Unity-rendered dream-route screenshots, node/path semantics, route-card click targets, import settings, binding, and Console.
