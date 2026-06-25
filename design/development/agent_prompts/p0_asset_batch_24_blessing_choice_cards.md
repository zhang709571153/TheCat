# Agent Prompt: P0 Asset Batch 24 Blessing Choice Cards

## Task Scope

Produce and integrate the P0 authority blessing choice card assets for:

- `blessing_authority_oath_bedline`
- `blessing_authority_dominion_sandglass`
- `blessing_authority_rhythm_lullaby`

This is a non-cat UI-symbol batch. It must not create or modify cat body art,
starter-cat derivatives, formal cat candidates, or colored turnaround assets.

## Design Sources

- `D:\Unity Workspace\TheCat\design\姊﹀鏀厤鑰呮牳蹇冪帺娉昞docs`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/asset_review/P0_RUNTIME_VISUAL_REVIEW_PACKET.md`
- `design/development/prompts/p0_blessing_choice_cards.md`

## Code Files To Read

- `Assets/TheCat/Scripts/Runtime/Roguelite/P0BlessingCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Roguelite/P0RouteRewardResolver.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetGenerationBatchCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0RouteMapPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetManifestCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RuntimeVisualBindingCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RouteMapSurfaceCoverage.cs`
- `Assets/TheCat/Tests/EditMode/P0VisualAssetCatalogTests.cs`
- `Assets/TheCat/Tests/EditMode/P0RouteMapSurfaceCoverageTests.cs`

## Expected Output

- Three `384x160` PNGs in `Assets/TheCat/Art/UI/Cards`.
- Matching Unity `.png.meta` files with sprite importer settings and batch
  userData.
- Manifest rows, runtime bindings, batch catalog entries, coverage checks, and
  EditMode tests for all three blessing choice cards.
- Updated runtime visual contact sheet and asset review packet.
- Development log entry with validation commands and Unity MCP follow-up.

## Forbidden Modifications

- Do not replace or regenerate Saiban, Nephthys, Suzune, or any starter-cat
  files.
- Do not create cat silhouettes, body parts, markings, costumes, or character
  portraits.
- Do not modify Unity scene or prefab files in this batch.
- Do not loosen starter-cat source-lock or formal import readiness gates.

## Acceptance Standards

- `P0AssetManifestCatalog.P0ManifestAssetCount == 91`.
- `P0VisualAssetCatalog.P0RuntimeVisualBindingCount == 87`.
- `P0AssetGenerationBatchCatalog.BlessingChoiceCardBatchId` exists after Batch
  23.
- `GetRouteChoiceCard` resolves specific blessing choice card assets for both
  `GainAuthorityBlessing` and `UpgradeAuthorityBlessing` choices.
- Runtime binding coverage includes `blessing_choice.oath_bedline`,
  `blessing_choice.dominion_sandglass`, and `blessing_choice.rhythm_lullaby`.
- Route map surface coverage confirms both first-pick and upgrade blessing card
  states.
- Generated images are visibly non-cat symbolic UI cards.

## Validation

Run:

```powershell
& 'D:\Unity Workspace\TheCat\design\development\tools\build_blessing_choice_cards.ps1'
& 'D:\Unity Workspace\TheCat\design\development\tools\build_runtime_visual_contact_sheet.ps1'
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' 'TheCat.Runtime.csproj' /p:OutputPath='Temp\bin\RuntimeBatch24BlessingCards\' /p:BaseIntermediateOutputPath='Temp\obj\RuntimeBatch24BlessingCards\' /v:minimal /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' 'TheCat.EditModeTests.csproj' /p:OutputPath='Temp\bin\EditModeBatch24BlessingCards\' /p:BaseIntermediateOutputPath='Temp\obj\EditModeBatch24BlessingCards\' /v:minimal /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' 'TheCat.Editor.csproj' /p:OutputPath='Temp\bin\EditorBatch24BlessingCards\' /p:BaseIntermediateOutputPath='Temp\obj\EditorBatch24BlessingCards\' /v:minimal /nologo
git diff --check
```

Unity MCP follow-up when available:

- Reimport the three card PNGs and inspect TextureImporter sprite settings.
- Open route map, navigate to blessing node, and screenshot first-pick and
  upgrade reward states.
- Confirm Console has no new errors.
