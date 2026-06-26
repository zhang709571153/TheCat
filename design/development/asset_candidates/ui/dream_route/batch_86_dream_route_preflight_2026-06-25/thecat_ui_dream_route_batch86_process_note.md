# Batch 86 Dream Route Process Note

- Lane: `dream_route` / route-map screen-level preflight.
- Source truth: Qr1 UI/style; local route icons, route card frames, and Batch 65 route-map readability accents.
- Generation mode: deterministic local derivative from existing raster assets; not image2 provenance.
- Reason: `OPENAI_API_KEY` is not set, so strict `gpt-image-2` CLI generation is unavailable in this shell.
- Candidate boundary: all PNGs stay under `design/development/asset_candidates`; do not import into `Assets` before Unity review.
- Text rule: no baked Chinese text; Unity-rendered text replacement remains required.
- Starter-cat rule: no body, pose, costume, color, or framesheet generation is included.
- Runtime gate: Unity-rendered dream-route screenshots, node/path semantics, route-card click targets, import settings, binding, and Console.

## 2026-06-26 K8 Addendum

- Boundary clarification: the original candidate packet remains under `design/development/asset_candidates` with no Unity `.meta` files.
- K8 created controlled candidate-only Unity preflight copies under `Assets/TheCat/Art/UI/DreamRoute` for Sprite import validation only.
- These K8 copies are not formal runtime bindings and do not approve install; `BATCH86_DREAM_ROUTE_UNITY_PREFLIGHT_REPORT_2026-06-26.md` records `Runtime evidence: 0/8`, 0 formal runtime binding leaks, and missing screenshot/review/Console/human gates.
- Future screenshot evidence must be backed by a runtime evidence report with candidate draw count and no fallback, not just correctly sized PNGs.
