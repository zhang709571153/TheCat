# Batch 90 Cat Room Process Note

- Lane: `ui_cat_room` / screen-level preflight.
- UI source truth: Feishu `Qr1XdXd6KosnjMxjgW7cS89kn9c` (`Qr1`) live section 9 fetch passed in this shell.
- Character identity source truth: Feishu `IAdkdcpciobUTXxa7dBcRx7Bngf` (`IAd`) is ACL-blocked for live fetch, so this packet avoids new cat body art.
- Generation mode: deterministic local derivative from existing raster assets; not image2 provenance.
- Reason: `OPENAI_API_KEY` is not set, so strict `gpt-image-2` CLI generation is unavailable in this shell.
- Built-in `image_gen` does not expose an explicit `image2` model selector here, so this batch avoids model-claimed generation and uses deterministic local derivation instead.
- Feishu ACL note: `MDr`, `IAd`, `IZp`, `HDo`, and the `FoW9` Drive folder remain live-read/list blocked for this CLI user; do not claim current-live coverage for those sources.
- Source packs reused: BedroomDream background and props, Batch 67 bedroom interaction affordances, Batch 82 common UI states, Qr1-style UI shell, and core sleep/hunger/poop icons.
- Candidate-only boundary: all PNGs stay under `design/development/asset_candidates`; do not import into `Assets` before Unity review.
- Text rule: no baked Chinese text; Unity-rendered interaction labels, status values, prompts, and dream entrance labels remain required.
- Cat-body rule: no new cat body art, pose, costume, color, portrait, or framesheet generation is included.
- Runtime gate: Unity-rendered cat-room screenshots, bed/feeder/litter/dream entrance interaction proof, hover/disabled/range state proof, click targets, import settings, binding, and Console.
