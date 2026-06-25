# Batch 89 Skill Selection Process Note

- Lane: `ui_skill_selection` / screen-level preflight.
- UI source truth: Feishu `Qr1XdXd6KosnjMxjgW7cS89kn9c` (`Qr1`) live section 9 fetch passed in this shell.
- Character identity source truth: Feishu `IAdkdcpciobUTXxa7dBcRx7Bngf` (`IAd`) is ACL-blocked for live fetch, so this packet avoids new character body art.
- Generation mode: deterministic local derivative from existing raster assets; not image2 provenance.
- Reason: `OPENAI_API_KEY` is not set, so strict `gpt-image-2` CLI generation is unavailable in this shell.
- Built-in `image_gen` does not expose an explicit `image2` model selector here, so this batch avoids model-claimed generation and uses deterministic local derivation instead.
- Feishu ACL note: `MDr`, `IAd`, `IZp`, `HDo`, and the `FoW9` Drive folder remain live-read/list blocked for this CLI user; do not claim current-live coverage for those sources.
- Source packs reused: Batch 80 symbolic skill icons, Batch 81 v002 light skill slot frames, Batch 82 common UI states, Batch 79 lock/warning icons.
- Candidate-only boundary: all PNGs stay under `design/development/asset_candidates`; do not import into `Assets` before Unity review.
- Text rule: no baked Chinese text; Unity-rendered skill names, descriptions, numbers, cost, cooldown, and confirm labels remain required.
- Starter-cat rule: no body, pose, costume, color, portrait, or framesheet generation is included.
- Runtime gate: Unity-rendered skill-selection screenshots, selected/disabled/locked state proof, cooldown/low-resource/no-target semantics, click targets, import settings, binding, and Console.
