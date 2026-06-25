# P0 Asset Batch 86 - Dream Route Preflight

Review the Batch 86 dream-route candidate packet before any Unity import.

## Candidate Packet

- Directory: `design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25`
- Manifest: `thecat_ui_dream_route_batch86_manifest.csv`
- Contact sheet: `thecat_ui_dream_route_batch86_contact_sheet_v001.png`
- Review sheet: `thecat_ui_dream_route_batch86_review_sheet_v001.png`
- Candidate review: `thecat_ui_dream_route_batch86_candidate_review.md`
- Process note: `thecat_ui_dream_route_batch86_process_note.md`

## Source Rules

- Qr1 UI/style is the visual authority.
- Use Batch 65 route-map readability accents, current route node icons, route-card frames, and existing dreamglass UI primitives as the consistency baseline.
- Do not import these files into `Assets` until Unity review approves a formal install.
- Do not generate, crop, recolor, or runtime-bind starter-cat body art from this packet.
- Do not bake Chinese text into sprites; use Unity-rendered text for titles, labels, route cards, and rewards.

## Required Review

1. Run `powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_ui_dream_route_preflight_candidates.ps1`.
2. Confirm the packet has 6 transparent sprites and 4 local mockups, all outside `Assets`.
3. Confirm the manifest records candidate, contact sheet, review sheet, review note, process note, and agent prompt hashes.
4. Check 1024x768 route-card crowding, lower-half density, and node/path/card click target scale.
5. Check Boss-pressure gate dominance against final card text and boss icon scale.
6. Verify Unity-rendered Chinese text replacement, Sprite import settings, scene/prefab binding, route node/path semantics, and clean Console before any install decision.

Return `PASS`, `PASS_WITH_P2`, or `FAIL` with concrete file paths and remaining Unity gates.
