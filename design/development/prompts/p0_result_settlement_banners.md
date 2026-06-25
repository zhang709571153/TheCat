# P0 Result Settlement Banners

Date: 2026-06-14

## Scope

Generate non-cat symbolic UI banners for P0 battle result and run-settlement
surfaces:

- `thecat_ui_battle_result_victory_banner_512x160_v001`
- `thecat_ui_battle_result_defeat_banner_512x160_v001`
- `thecat_ui_settlement_run_cleared_banner_512x160_v001`
- `thecat_ui_settlement_run_failed_banner_512x160_v001`

## Art Direction

- Size: 512x160.
- Import path: `Assets/TheCat/Art/UI/Banners`.
- Style: hand-painted dreamglass UI with cyan sleep glow, moon gold, violet
  dream haze, and red pressure accents for failure states.
- Do not include UI text in pixels. Runtime presenter labels own all text.
- Do not depict cat bodies, faces, ears, paws, tails, fur markings, collars,
  costumes, or starter-cat props.
- Do not derive from Saiban, Nephthys, Suzune, or their colored three-view
  turnarounds.

## Symbol Rules

- Battle victory: protected bedline, stable sleep wave, small reward glints.
- Battle defeat: cracked bedline, dim sleep crescent, red pressure arcs.
- Run cleared: ten-layer route path, broken Boss seal, fish treat and dream
  shard reward symbols.
- Run failed: broken route path, dim bedline, unresolved Boss pressure.

## Runtime Wiring

- `P0VisualAssetCatalog.GetBattleResultOutcomeBanner(BattleOutcome.Victory)`
  resolves the battle victory banner.
- `P0VisualAssetCatalog.GetBattleResultOutcomeBanner(BattleOutcome.Defeat)`
  resolves the battle defeat banner.
- `P0VisualAssetCatalog.GetSettlementOutcomeBanner(true)` resolves the cleared
  settlement banner.
- `P0VisualAssetCatalog.GetSettlementOutcomeBanner(false)` resolves the failed
  settlement banner.

## Acceptance

- All PNGs are 512x160.
- `.png.meta` uses `textureType: 8`, `spriteMode: 1`, and `spriteBorder:16`.
- `.png.meta` userData includes
  `batch:p0_asset_batch_25_result_settlement_banners` and
  `nonCatSymbolicOnly:true`.
- Manifest count: `P0AssetManifestCatalog.P0ManifestAssetCount == 95`.
- Runtime binding count: `P0VisualAssetCatalog.P0RuntimeVisualBindingCount == 91`.
