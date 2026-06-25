# Batch 83 Process Note

Lane: ui_loading_start.

Production method: deterministic local derivative from existing Qr1 UI/style shell assets.

Controls:
- candidate-only under `design/development/asset_candidates/ui/loading_start/`
- no files written under `Assets/`
- no Unity `.meta` files generated
- no starter-cat body, starter-cat frame, or character replacement art
- no baked Chinese text in new candidate sprites
- not image2 provenance; strict `gpt-image-2` CLI generation remains blocked without `OPENAI_API_KEY`

Manifest rows: 8

Promotion gate: use this packet only to decide whether the current loading/start primitives are sufficient. Formal acceptance requires Unity-rendered loading/start screenshots across target resolutions plus import/binding/Console proof.
