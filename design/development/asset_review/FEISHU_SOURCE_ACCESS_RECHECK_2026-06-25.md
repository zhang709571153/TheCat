# Feishu Source Access Recheck

Checked: 2026-06-25 08:03 +08:00

CLI: `lark-cli` 1.0.53  
Profile: `personal`  
Identity: user `章航宁`, verified `true`, token status `valid`, access token expiry `2026-06-25 09:20:40 +08:00`, refresh token expiry `2026-07-02 07:20:40 +08:00`

## Results

| Source | Result | Evidence |
| --- | --- | --- |
| `Qr1XdXd6KosnjMxjgW7cS89kn9c` | [x] readable | `docs +fetch --scope outline --max-depth 3 --doc-format xml --detail with-ids` succeeded at 08:03 +08:00; revision `816`. |
| `Qr1` section 9 | [x] readable | `docs +fetch --scope section --start-block-id doxcnicPX8DCDjGpx39lQsRCP4m --doc-format markdown --detail simple` returned the live "首版 Demo 最小美术资产" table at 08:03 +08:00. |
| `MDrSdEoaToB5cnxZgrOcAE34nof` | [ ] blocked | `docs +fetch --scope outline` returned Feishu API `3380004` at 08:03 +08:00, log `202606250803024BE134F35FE5D549D465`, no view/edit permission. |
| `IAdkdcpciobUTXxa7dBcRx7Bngf` | [ ] blocked | `docs +fetch --scope outline` returned Feishu API `3380004` at 08:03 +08:00, log `2026062508030259836452D586D886E5E2`, no view/edit permission. Bot identity also returned `3380004`, log `20260625080320271845CE8FD9B85B153B`. |
| `IZpFdIwtboEzzrx4ZFlcZLD2npe` | [ ] blocked | `docs +fetch --scope outline` returned Feishu API `3380004` at 08:03 +08:00, log `20260625080302F2B51609A6054D9C627B`, no view/edit permission. |
| `HDoWdDNR3oZ6uhxuMzPcT8qCn5f` | [ ] blocked | `docs +fetch --scope outline` returned Feishu API `3380004` at 08:03 +08:00, log `202606250803028CA9B8A4F88B0FCEA845`, no view/edit permission. |
| `FoW9fKYcDllwJjdTxGHcu4pbnab` | [ ] blocked for listing | `drive +inspect --url` resolves the token as `folder`; `drive +status --quick` returned API `1061004 forbidden` at 08:03 +08:00, log `20260625080319F7A798FBF7094F6438AA`. |

## Live P0 Minimum Art Boundary From `Qr1` Section 9

The live source currently requires:

- Three cats: Saiban, Nephthys, and Suzune in-game sprites plus idle, movement, normal attack, two small skills, ultimate, hit, and death/exit performance.
- Two dream maps: male-owner bedroom dream and Egypt dream, each with bed/defense target, monster entrances, major obstacles, and theme props.
- Cat room: out-of-combat map with bed, feeder, litter box, and dream entrance.
- Monsters: male-owner dream pool and Egypt dream pool, each monster with idle, move, attack, and skill-cast actions.
- Bosses: male-owner dream fixed Boss and Egypt dream fixed Boss.
- UI: entry screen, main menu, cat room, battle HUD, pause menu, settings, skill selection, victory/defeat settlement.
- Combat feedback: normal-hit, skill VFX, shield, slow/freeze, aftershock, bed-hit, monster death, and damage numbers.

## Control Update

The current blocker is not OAuth/token validity; it is document/folder ACL. This was rechecked after the user confirmed the active `personal` profile and valid refreshed token. Continue using `Qr1` live data and local synced copies/source-lock packets for blocked docs until access is granted.

Strict `gpt-image-2` CLI generation is blocked in this shell because `OPENAI_API_KEY` is not set. Built-in `image_gen` can still be used for candidate art when appropriate, followed by chroma-key removal, but it does not expose a model selector.

Unity batchmode offline acceptance is currently passing: `design/development/unity_batchmode/P0_OFFLINE_ACCEPTANCE_REPORT.md` reports `Result: passed`, `Gate count: 6`, and `Failure count: 0` from the 2026-06-25 08:00 +08:00 rerun. This does not replace Unity Play Mode screenshots, Console checks, binding proof, or active enemy animation review.

Also note: CLI reports `lark-cli 1.0.57` is available while current version is `1.0.53`; no update was run during this asset pass.
