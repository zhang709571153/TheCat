# Batch 119 Bedroom Route Socket Markers Process Note

Batch: `batch_119_bedroom_route_socket_markers_2026-06-26`
Family: `map`
Status: `candidate_complete_pending_unity_review`

## Source Truth

- UI/style authority: `Qr1XdXd6KosnjMxjgW7cS89kn9c`, live-readable UI/style truth, revision `816` in the current control ledger.
- Local gameplay basis: bedroom battle/dream map lanes and existing Batch101/109/117 map-marker context.
- Source boundary: no HDo/FoW9 map-archive claim, no IAd live-refresh claim, no character body, face, portrait, paw/cat silhouette, costume, framesheet, or animation output.

## Process

1. Scaffolded the candidate package before generation with `asset-sprite-production`.
2. Used the built-in Codex `imagegen` skill path, not API key or CLI fallback. Tool token for validators: built-in Codex `imagegen`. The built-in tool does not expose an explicit model selector here, so this process note records the user-requested imagegen/image2 intent but does not claim a model-locked CLI run.
3. Generated a 3x3 textless chroma-key source sheet on a flat magenta background for local background removal.
4. Copied the selected source image from `C:\Users\PC\.codex\generated_images\019efc23-3cc2-7f23-9beb-cb16d118a13f\ig_0dd80315461bcee0016a3dcc85950c819b84cad84e7bf15c13.png` into `source/thecat_map_socket_batch119_chromakey_source_v001.png`.
5. Ran local chroma-key removal with `remove_chroma_key.py --auto-key border --soft-matte --transparent-threshold 12 --opaque-threshold 220 --despill`.
6. Split the alpha sheet with `sprite_batch_tools.py split-sheet --rows 3 --cols 3 --pad 24`, using `names.txt` as the semantic order.
7. Generated review variants with `review_variant_tools.py make-variants --size 512`.
8. Built a 96px/64px dark/warm bedroom-map readability board.

## Chroma And Cut Results

- Source image: `source/thecat_map_socket_batch119_chromakey_source_v001.png`
- Source SHA256: `5f1d5dc7949b11238c07151e77cf11f808cccc9d8420975403ed1b88ad8cf6fe`
- Alpha sheet: `alpha/thecat_map_socket_batch119_alpha_sheet_v001.png`
- Alpha SHA256: `dca340e0be9494736c398a365bbb637e08497596353b0b06a2439fbf7be164fe`
- Auto key: `#f903f8`
- Transparent pixels: `853324/1572516`
- Partially transparent pixels: `51408/1572516`
- Row bands: `(50,405)`, `(449,795)`, `(843,1196)`
- Column bands: `(56,401)`, `(458,795)`, `(847,1191)`
- All 9 active semantic sprites have alpha extrema `(0,255)`, transparent corners, and 0 nontransparent outer-edge pixels.
- Review variants: 45 PNGs plus 9 manifests under `reviews/variants/`; active max variant path length is 220.

## Output Summary

- Asset table: `thecat_batch_119_bedroom_route_socket_markers_2026-06-26_asset_table.csv`
- Manifest: `thecat_map_sock_batch119_semantic_manifest.csv`
- Contact sheet: `thecat_map_sock_batch119_semantic_contact_sheet_v001.png`
- Readability board: `thecat_map_sock_batch119_96px_64px_bedroom_socket_readability_board_v001.png`
- Final review CSV: `thecat_batch_119_bedroom_route_socket_markers_2026-06-26_final_review.csv`
- Review variants: `reviews/variants/`

## Current Review Decision

Initial local review before independent agents:

- `candidate_keep`: `north_entry_socket_marker`, `east_entry_socket_marker`, `south_entry_socket_marker`, `west_entry_socket_marker`, `hazard_noise_socket_marker`
- `candidate_conditional`: `bed_defense_socket_frame`, `safe_rest_socket_marker`, `reward_pickup_socket_marker`, `locked_unknown_socket_marker`

Counts: 5 candidate_keep, 4 candidate_conditional.

The conditional items are usable candidates but need same-screen Unity proof so they do not collide with Batch101 bed-defense/spawn warnings, Batch109 entry/path props, Batch117 pickups, or Batch107/116/118 locked/team-slot UI language.

No runtime import was performed. No Unity `.meta` files were created. Formal runtime promotion remains blocked pending Unity import settings, bedroom map socket screenshots, 96px/64px live contrast, semantic collision proof, no recursive candidate import, explicit human approval, and clean Console evidence.
